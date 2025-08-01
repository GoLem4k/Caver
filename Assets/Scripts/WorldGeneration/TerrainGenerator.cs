using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    [SerializeField] private TileManager tileManager;

    public void GenerateTerrain(WorldGenSettings settings, int seed, Vector3Int center)
    {
        // 1. Генерация фона
        GenerateBackgroundCircle(center, settings.worldRadius * 2);

        // 2.1. Генерация основной массы камня
        GenerateSolidCircle(center, settings.worldRadius, settings.baseBlock);

        // 2.2. Генерация приграничных блоков
        GenerateRing(center, settings.worldRadius-settings.nearEdgeBlockRadius, settings.worldRadius, settings.nearEdgeBlock);
        
        // 3. Генерация внешнего кольца, границы мира
        GenerateRing(center,
            settings.worldRadius + settings.spaceBetweenWorldAndEdge,
            settings.worldRadius + settings.spaceBetweenWorldAndEdge + settings.worldEdgeSize,
            settings.edgeBlock);
        
        // 4. Пещеры
        GenerateNoiseLayer(seed, center, settings.worldRadius,
            settings.airNoiseScale, settings.airMinThreshold, settings.airMaxThreshold,
            settings.airBlock, settings.baseBlock);

        // 5. Земля
        GenerateNoiseLayer(seed, center, settings.worldRadius,
            settings.dirtNoiseScale, settings.dirtMinThreshold, settings.dirtMaxThreshold,
            settings.dirtBlock, settings.baseBlock);
    }

    private void GenerateBackgroundCircle(Vector3Int center, int radius)
    {
        IterateCircle(center, radius, pos => tileManager.SetBgCell(pos));
    }

    private void GenerateSolidCircle(Vector3Int center, int radius, BlockType type)
    {
        IterateCircle(center, radius, pos => tileManager.SetCell(pos, type));
    }

    private void GenerateRing(Vector3Int center, int minRadius, int maxRadius, BlockType type)
    {
        float minR2 = minRadius * minRadius;
        float maxR2 = maxRadius * maxRadius;
        for (int x = -maxRadius; x <= maxRadius; x++)
        for (int y = -maxRadius; y <= maxRadius; y++)
        {
            float d2 = x * x + y * y;
            if (d2 >= minR2 && d2 <= maxR2)
                tileManager.SetCell(new Vector3Int(center.x + x, center.y + y, 0), type);
        }
    }

    private void GenerateNoiseLayer(int seed, Vector3Int center, int radius,
        float noiseScale, float minT, float maxT, BlockType placeType, BlockType onlyOnType)
    {
        IterateCircle(center, radius, pos =>
        {
            float noise = Mathf.PerlinNoise((pos.x + seed) * noiseScale, (pos.y + seed) * noiseScale);
            if (noise >= minT && noise <= maxT && tileManager.IsBlockOnPos(pos, onlyOnType))
                tileManager.SetCell(pos, placeType);
        });
    }

    private void IterateCircle(Vector3Int center, int radius, System.Action<Vector3Int> action)
    {
        float r2 = radius * radius;
        for (int x = -radius; x <= radius; x++)
        for (int y = -radius; y <= radius; y++)
        {
            if (x * x + y * y <= r2)
                action.Invoke(new Vector3Int(center.x + x, center.y + y, 0));
        }
    }
}
