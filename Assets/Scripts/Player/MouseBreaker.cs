using UnityEngine;

public class MouseBreaker : PausedBehaviour
{
    public float rayLength = 100f;
    public LayerMask layerMask = ~0;

    private Collider2D selfCollider;

    private float damageCooldown = 0f;
    void Start()
    {
        selfCollider = GetComponent<Collider2D>();
    }

    public override void GameUpdate()
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
                // Проверка тега
                if (hit.collider.CompareTag("Wall")) // Заменить "Tile" на нужный тег
                {
                    Vector3 tilePoint = hit.point + direction * 0.2f;

                    if (TileManager.I.IsTileByWorldPos(tilePoint))
                    {
                        TileManager.I.SetSelectorTileOnPos(TileManager.SelectorTilemap.WorldToCell(tilePoint));
                    }

                    Debug.DrawRay(origin, direction * hit.distance, Color.green);
                    return;
                }
            }
        }
        if (hits.Length == 1) TileManager.I.ResetSelector();
        
        // Если ничего не попало (или только в себя) — рисуем красный
        Debug.DrawRay(origin, direction * rayLength, Color.red);
    }
}