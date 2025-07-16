using System;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Tilemaps;

public class VectorMovementController : PausedBehaviour
{
    public float moveSpeed = 0f;

    //public float accelerationSpeed = 2f;
    //public float breakingSpeed = 5f;
    //public float rotationSpeed = 0.1f;
    public bool canSpeedUp = true;

    public float forceSpeed = 0f;
    //public float maxForceSpeed = 10f;
    public float breakingForceSpeed = 4f;

    //При какой скорости произойдёт отскок
    [SerializeField] private float bounceThreshold = 5f;
    [SerializeField] private float WallBounceDamping = 0.99f;

    private System.Random rng;
    
    private Rigidbody2D rb;
    
    private Vector2 inputVector;
    private Vector2 externalForces = Vector2.zero; // Можно менять из других скриптов
    
    private Vector2 inputMovement;
    private Vector2 forceMovement;
    private Vector2 finalMovement;
    
    private Vector2 inputVelocity;
    Vector2 previousInput;
    private PlayerAnimationController animationController;

    //DEBUG
    [SerializeField] private float debugForce;
    private LineRenderer lineExternalForceRaw;
    private LineRenderer lineExternalForceFinal;
    private LineRenderer lineInput;
    private LineRenderer lineFinal;

    public TileManager TileManager;
    
    public void Initialize() {
        rng = new System.Random(WorldGenerator.SEED);
        rb = GetComponent<Rigidbody2D>();
        animationController = GetComponent<PlayerAnimationController>();

        transform.position = WorldGenerator.SPAWNPOINT;
        
        lineFinal = CreateLineRenderer(Color.white, 0.03f);
        lineInput = CreateLineRenderer(Color.red, 0.07f);
        lineExternalForceFinal = CreateLineRenderer(Color.cyan, 0.09f);
        lineExternalForceRaw = CreateLineRenderer(Color.blue, 0.2f);
    }
    
    public override void GameUpdate()
    {
        // Получаем вход игрока
        inputVector = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (inputVector.sqrMagnitude > 1f)
            inputVector.Normalize();

        
        if (inputVector.magnitude > Mathf.Epsilon)
        {
            moveSpeed = (canSpeedUp && Math.Abs(Vector2.SignedAngle(finalMovement, inputVector)) < 90) ? Mathf.Clamp(moveSpeed + RunData.I.movementSpeed * 0.15f * Time.deltaTime, RunData.I.movementSpeed, RunData.I.movementSpeed*1.2f) : RunData.I.movementSpeed;
    
            // Сглаживаем только если есть ввод
            previousInput = Vector2.SmoothDamp(previousInput, inputVector, ref inputVelocity, 0.15f - RunData.I.movementSpeed*0.05f);
        }
        else
        {
            moveSpeed = Mathf.Clamp(moveSpeed - RunData.I.movementSpeed * 2f * Time.deltaTime, 0f, RunData.I.movementSpeed*1.25f);

            // Если полностью остановились — сбрасываем направление
            if (moveSpeed <= Mathf.Epsilon)
                previousInput = Vector2.zero;
        }
        
        
        // Здесь можно менять externalForces, например, из других систем (отбрасывание, ветер и т.п.)
        forceSpeed = Mathf.Clamp(forceSpeed - breakingForceSpeed * Time.deltaTime, 0f, RunData.I.reboundScale * 3f);
        if (forceSpeed <= 0) externalForces = Vector2.zero;
        
        inputMovement = previousInput * moveSpeed;
        forceMovement = externalForces.normalized * forceSpeed;
        finalMovement = inputMovement + forceMovement;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPos.z = 0f; // Обнуляем Z для 2D
            Vector2 direction = (mouseWorldPos - transform.position);

            forceSpeed += RunData.I.reboundScale;
            AddExternalForce(direction.normalized);
        }

        DebugLineUpdate();
    }
    
    public override void GameFixedUpdate()
    {
        rb.MovePosition(rb.position + finalMovement * Time.fixedDeltaTime);
    }

    // Пример метода для добавления внешней силы
    public void AddExternalForce(Vector2 force)
    {
        externalForces += force;
    }
    
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Wall"))
            TryReflect(collision);
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Wall"))
        {
            canSpeedUp = false;
            if (forceSpeed > 0f)
                TryReflect(collision);
        }
        else
        {
            canSpeedUp = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        canSpeedUp = collision.collider.CompareTag("Wall");
    }

    private void DamageBlock(Vector3Int pos)
    {
        //dualGridTilemap.UpdateDurability(pos, -damage);
    }
    
    private void TryReflect(Collision2D collision)
    {
        ContactPoint2D contact = collision.GetContact(collision.contactCount - 1);
        Vector2 normal = contact.normal;

        float impactDot = Vector2.Dot(forceMovement.normalized, -normal);

        if (impactDot > 0.1f && forceSpeed >= bounceThreshold)
        {
            Vector2 reflected = Vector2.Reflect(externalForces, normal);
            forceSpeed = Mathf.Clamp(forceSpeed * WallBounceDamping, 0f, RunData.I.reboundScale * 3f);
            externalForces = reflected;
            previousInput = Vector2.zero;
            moveSpeed = RunData.I.movementSpeed;

            Vector3 contactPoint = contact.point - contact.normal * 0.1f;
            Vector3Int tilePos = TileManager.BlocksTilemap.WorldToCell(contactPoint);
            //Debug.Log($"contact.point: {contact.point}, adjusted: {contactPoint}, tilePos: {tilePos}");
            TileManager.damageBlock(tilePos, (rng.NextDouble() < RunData.I.critChance) ? RunData.I.reboundDamage * 2f : RunData.I.reboundDamage);

        }
    }

    private void DebugLineUpdate()
    {
        Vector3 pos = transform.position;

        lineExternalForceRaw.SetPosition(0, pos);
        lineExternalForceRaw.SetPosition(1, pos + (Vector3)(externalForces / 2f));

        lineExternalForceFinal.SetPosition(0, pos);
        lineExternalForceFinal.SetPosition(1, pos + (Vector3)(externalForces * forceSpeed / 2f));

        lineInput.SetPosition(0, pos);
        lineInput.SetPosition(1, pos + (Vector3)(previousInput * moveSpeed / 2f));

        lineFinal.SetPosition(0, pos);
        lineFinal.SetPosition(1, pos + (Vector3)(finalMovement / 2f));
    }
    
    private LineRenderer CreateLineRenderer(Color color, float width)
    {
        GameObject go = new GameObject("DebugLine_" + color.ToString());
        go.transform.SetParent(transform); // чтобы линия следовала за объектом
        LineRenderer lr = go.AddComponent<LineRenderer>();
        lr.material = new Material(Shader.Find("Sprites/Default"));
        lr.startColor = lr.endColor = color;
        lr.startWidth = lr.endWidth = width;
        lr.positionCount = 2;
        lr.useWorldSpace = true;
        return lr;
    }

}