using System;
using UnityEngine;

public class WorldGenerator : MonoBehaviour
{
    [SerializeField] private WorldGenSettings settings;
    [SerializeField] private TerrainGenerator terrainGenerator;
    [SerializeField] private SpawnZoneGenerator spawnZoneGenerator;
    [SerializeField] private TunnelGenerator tunnelGenerator;
    private System.Random rng;

    public void Initialize()
    {
        settings.ApplyPresetForSize(RunData.I.globalRunData.WorldSize);
        int seed = RunData.I.SEED;
        Vector3Int center = Vector3Int.zero;
        rng = new System.Random(RunData.I.SEED);

        terrainGenerator.GenerateTerrain(settings, seed, center);

        Vector3Int spawn = GetRandomPointOnCircle(center, settings.spawnRadius * 2);
        RunData.I.SPAWNPOINT = spawn;
        tunnelGenerator.GenerateTunnels(seed, Vector3Int.zero, settings.worldRadius - settings.nearEdgeBlockRadius/2, settings);
        
        spawnZoneGenerator.ClearSpawnZone(spawn, settings.spawnClearRadius);
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
}
