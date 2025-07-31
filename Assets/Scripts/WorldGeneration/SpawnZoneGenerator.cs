using UnityEngine;

public class SpawnZoneGenerator : MonoBehaviour
{
    [SerializeField] private TileManager tileManager;

    public void ClearSpawnZone(Vector3Int spawnPoint, int radius)
    {
        float r2 = radius * radius;
        for (int x = -radius; x <= radius; x++)
        for (int y = -radius; y <= radius; y++)
        {
            if (x * x + y * y <= r2)
            {
                Vector3Int pos = new Vector3Int(spawnPoint.x + x, spawnPoint.y + y, 0);
                tileManager.SetCell(pos, BlockType.None);
            }
        }
    }
}