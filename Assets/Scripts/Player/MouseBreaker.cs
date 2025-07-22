using UnityEngine;

public class MouseBreaker : MonoBehaviour
{
    public float rayLength = 100f;
    public LayerMask layerMask = ~0;

    private Collider2D selfCollider;

    private float damageCooldown = 0f;
    void Start()
    {
        selfCollider = GetComponent<Collider2D>();
    }

    void Update()
    {
        damageCooldown = Mathf.Clamp(damageCooldown - Time.deltaTime, 0, RunData.I.damageCooldown);
        Vector3 mouseScreenPos = Input.mousePosition;
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(mouseScreenPos);
        mouseWorldPos.z = 0f;

        Vector2 origin = transform.position;
        Vector2 direction = (mouseWorldPos - transform.position).normalized;

        // Получаем все попадания
        RaycastHit2D[] hits = Physics2D.RaycastAll(origin, direction, rayLength, layerMask);

        if (Input.GetMouseButton(0) && damageCooldown <= 0)
        {
            TileManager.I.DamageSelectTile();
            damageCooldown = RunData.I.damageCooldown;
        }
        foreach (var hit in hits)
        {
            if (hit.collider != null && hit.collider != selfCollider)
            {
                if (TileManager.I.IsTileByWorldPos(hit.point + direction * 0.2f))
                {
                    TileManager.I.SetSelectorTileOnPos(TileManager.SelectorTilemap.WorldToCell(hit.point + direction * 0.2f));
                }
                //Debug.Log($"Hit: {hit.collider.name}");
                Debug.DrawRay(origin, direction * hit.distance, Color.green);
                return;
            }
        }
        if (hits.Length == 1) TileManager.I.ResetSelector();
        
        // Если ничего не попало (или только в себя) — рисуем красный
        Debug.DrawRay(origin, direction * rayLength, Color.red);
    }
}