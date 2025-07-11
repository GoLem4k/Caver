/*using UnityEngine;

public class MousePlacer : MonoBehaviour
{
    public Camera mainCamera;
    public DualGridTilemap dualGridTilemap;
    
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // ЛКМ — поставить блок
        {
            Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            Debug.Log(dualGridTilemap.durabilityData[WorldToGrid(mouseWorldPos)]);
        }

        if (Input.GetMouseButtonDown(1)) // ПКМ — удалить блок
        {
            // Аналогично можешь получить координаты
        }
    }

    private Vector3Int WorldToGrid(Vector3 worldPos)
    {
        return new Vector3Int(
            Mathf.FloorToInt(worldPos.x - 0.5f),
            Mathf.FloorToInt(worldPos.y - 0.5f),
            0
        );
    }
}*/