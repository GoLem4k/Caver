using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;

public class WorldGenerator : MonoBehaviour
{
    [Header("Настройки генерации")]
    public int worldRadius = 256;
    public int spawnAreaInRange = 32;
    public int spawnAreaRadius = 8;
    public int spaceBetweenWorldAndEdge = 4;
    public int worldEdgeSize = 8;

    private Vector3Int spawnPoint;
    
    public static int WORLDRADIUS = 256;
    public static Vector3Int SPAWNPOINT = new Vector3Int(0,0,0);
    //public float noiseScale = 0.1f;
    public static int SEED = 12345;
    public float stoneThreshold = 0.3f;
    
    [Header("Шум земли")]
    public float dirtMinThreshold = 0.5f;
    public float dirtMaxThreshold = 0.5f;
    public float dirtNoiseScale = 0.05f;
    [Header("Шум тунелей")]
    public float airMinThreshold = 0.7f;
    public float airMaxThreshold = 0.7f;
    public float airNoiseScale = 0.1f;
    
    
    [Header("Tilemap")]
    public TileManager tileManager;
    
    [Header("Настройки залеж руд")]
    public float depositChance = 0.01f;

    public float[] multipleOreChance = { 1f, 0.9f, 0.5f, 0.2f, 0.05f };
    public System.Random rng;
    
    Vector3Int[] NEIGHBOURS8X = new Vector3Int[] {
        new Vector3Int(1, 0, 0),     // справа
        new Vector3Int(-1, 0, 0),    // слева
        new Vector3Int(0, 1, 0),     // сверху
        new Vector3Int(0, -1, 0),    // снизу
        new Vector3Int(-1, 1, 0),    // сверху и слева
        new Vector3Int(1, 1, 0),    // сверху и справа
        new Vector3Int(-1, -1, 0),    // снизу и слева
        new Vector3Int(1, -1, 0)    // снизу и справа
    };
    
    public static Vector3Int[] NEIGHBOURS4X = new Vector3Int[] {
        new Vector3Int(1, 0, 0),     // справа
        new Vector3Int(-1, 0, 0),    // слева
        new Vector3Int(0, 1, 0),     // сверху
        new Vector3Int(0, -1, 0)    // снизу
    };

    private Vector3Int[] N2Xvertical = { Vector3Int.up, Vector3Int.down };
    private Vector3Int[] N2Xhorizontal = { Vector3Int.left, Vector3Int.right };

    Vector3Int[] neighbours;
    
    public void Initialize()
    {
        WORLDRADIUS = worldRadius;
        rng = new System.Random(SEED);

        SPAWNPOINT = GetRandomPointOnCircle(new Vector3Int(0, 0, 0), spawnAreaInRange);
        
        gBlockCircleArea(new Vector3Int(0,0,0), WORLDRADIUS, BlockType.Stone); //Генерация основы мира из камня
        gBlockRingArea(new Vector3Int(0,0,0), WORLDRADIUS+spaceBetweenWorldAndEdge, WORLDRADIUS+spaceBetweenWorldAndEdge+worldEdgeSize, BlockType.Endstone); //Генерация границы мира
        gBlockCircleArea(SPAWNPOINT, spawnAreaRadius, BlockType.None); //Генерация спавн зоны
        gNoiseBlocksInRadius(SEED, airNoiseScale, airMinThreshold, airMaxThreshold, new Vector3Int(0,0,0), WORLDRADIUS, BlockType.None, BlockType.Stone); //Генерация пещер
        gNoiseBlocksInRadius(SEED, dirtNoiseScale, dirtMinThreshold, dirtMaxThreshold, new Vector3Int(0,0,0), WORLDRADIUS, BlockType.Dirt, BlockType.Stone); //Генерация земли
        //GenerateOres(); //Генерация медной руды
        for (int i = 0; i < 25; i++)
        {
            gTunnelBlocksToPos(GetRandomPointOnCircle(new Vector3Int(0,0,0), WORLDRADIUS/2), NEIGHBOURS4X[rng.Next(NEIGHBOURS4X.Count())], 0.4f, 16, BlockType.Expstone);
        }
    }

    public Vector3Int GetRandomPointOnCircle(Vector3Int center, int radius)
    {
        // Случайный угол от 0 до 2π
        double angle = rng.NextDouble() * 2.0f * Math.PI;

        // Вычисляем координаты на окружности
        double x = center.x + Math.Cos(angle) * radius;
        double y = center.y + Math.Sin(angle) * radius;
        
        return new Vector3Int((int)Math.Round(x), (int)Math.Round(y), center.z);
    }

    //генерация круга из блоков
    private void gBlockCircleArea(Vector3Int center, int radius, BlockType type)
    {
        float radiusSqr = radius * radius;
        for (int x = -radius; x < radius; x++) {
            for (int y = -radius; y < radius; y++) {
                float distSqr = x * x + y * y;
                if (distSqr <= radiusSqr)
                {
                    Vector3Int pos = new Vector3Int(center.x + x, center.y + y, 0);
                    tileManager.SetCell(pos, type);
                }
            }
        }
    }
    
    //генерация кольца из блоков
    private void gBlockRingArea(Vector3Int center, int minRadius, int maxRadius, BlockType type) {
        float minRadiusSqr = minRadius * minRadius;
        float maxRadiusSqr = maxRadius * maxRadius;
        for (int x = -maxRadius; x < maxRadius; x++) {
            for (int y = -maxRadius; y < maxRadius; y++) {
                float distSqr = x * x + y * y;
                if (distSqr <= maxRadiusSqr && distSqr >= minRadiusSqr) {
                    Vector3Int pos = new Vector3Int(center.x + x, center.y + y, 0);
                    tileManager.SetCell(pos, type);
                }
            }
        }
    }

    private void gNoiseBlocksInRadius(int seed, float noiseScale, float minThreshold, float maxThreshold, Vector3Int center, int radius, BlockType type, BlockType filter = BlockType.None)
    {
        float radiusSqr = radius * radius;
        for (int x = -radius; x < radius; x++)
        {
            for (int y = -radius; y < radius; y++)
            {
                float distSqr = x * x + y * y;
                if (distSqr <= radiusSqr)
                {
                    float value = Mathf.PerlinNoise((x + seed) * noiseScale, (y + seed) * noiseScale);
                    Vector3Int pos = new Vector3Int(center.x + x, center.y + y, 0);
                    if (value > minThreshold && value < maxThreshold && TileManager.IsBlockOnPos(pos, filter))
                        tileManager.SetCell(pos, type);
                }
            }
        }
    }

    private void gTunnelBlocksToPos(Vector3Int pos, Vector3Int dir, float curvature, int length, BlockType type, BlockType filter = BlockType.None)
    {
        if (!TileManager.IsBlockOnPos(pos, filter))
        {
            length--;
            tileManager.SetCell(pos, type);
            Vector3Int offset = dir;
            if (rng.NextDouble() < curvature) {
                offset = (dir == Vector3Int.up  || dir == Vector3Int.down) ? N2Xhorizontal[rng.Next(N2Xhorizontal.Count())] : dir;
                offset = (dir == Vector3Int.left  || dir == Vector3Int.right) ? N2Xvertical[rng.Next(N2Xvertical.Count())] : dir; }
            if (length > 0) gTunnelBlocksToPos(pos+offset, dir, curvature, length, type, filter);
        }
    }
    

    /*private bool isPlaceForOre(Vector3Int pos, int neighborsCount, HashSet<Vector3Int> currentDeposit = null)
    {
        if (neighborsCount == 8) neighbours = NEIGHBOURS8X;
        if (neighborsCount == 4) neighbours = NEIGHBOURS4X;

        // хотя бы один сосед Dirt
        foreach (var offset in neighbours)
        {
            Vector3Int checkPos = pos + offset;
            if (tileManager.IsBlockOnPos(checkPos, BlockType.Stone))
                return true;
        }

        return false;
    }

    private void GenerateOreDeposit(Vector3Int pos)
    {
        HashSet<Vector3Int> currentDeposit = new();
        tileManager.SetCell(pos, OreType.Copper);
        currentDeposit.Add(pos);

        for (int i = 0; i < multipleOreChance.Length; i++)
        {
            List<Vector3Int> candidates = new();
            foreach (var offset in NEIGHBOURS8X)
            {
                Vector3Int neighbor = pos + offset;
                if (!currentDeposit.Contains(neighbor) && isPlaceForOre(neighbor, 8, currentDeposit) && tileManager.IsBlockOnPos(neighbor, BlockType.Stone))
                {
                    candidates.Add(neighbor);
                }
            }

            if (candidates.Count > 0 && rng.NextDouble() < multipleOreChance[i])
            {
                Vector3Int chosen = candidates[rng.Next(candidates.Count)];
                tileManager.SetCell(chosen, OreType.Copper);
                currentDeposit.Add(chosen);
                pos = chosen;
            }
            else break;
        }
    }
    
    
    public void GenerateOres() {
        for (int x = -WORLDRADIUS; x < WORLDRADIUS; x++)
        {
            for (int y = -WORLDRADIUS; y < WORLDRADIUS; y++)
            {
                Vector3Int pos = new Vector3Int(x, y, 0);
                if (isPlaceForOre(pos, 8) && tileManager.IsBlockOnPos(pos, BlockType.Stone) && rng.NextDouble() < depositChance) {
                    GenerateOreDeposit(pos);
                }
            }
        }
    }*/
}