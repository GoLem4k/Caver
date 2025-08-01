using UnityEngine;

[CreateAssetMenu(fileName = "WorldGenSettings", menuName = "Generation/WorldGenSettings")]
public class WorldGenSettings : ScriptableObject
{
    [Header("Спавн-зона")]
    public int spawnClearRadius = 8;
    
    [Header("Радиусы")]
    public int worldRadius = 64;
    public int spawnRadius = 8;
    public int worldEdgeSize = 8;
    public int spaceBetweenWorldAndEdge = 4;
    public int nearEdgeBlockRadius = 20;

    [Header("Шум пещер (воздух)")]
    public float airNoiseScale = 0.07f;
    public float airMinThreshold = 0.65f;
    public float airMaxThreshold = 1f;

    [Header("Шум земли (грунт)")]
    public float dirtNoiseScale = 0.05f;
    public float dirtMinThreshold = 0.5f;
    public float dirtMaxThreshold = 1f;
    
    [Header("Туннели")]
    public int tunnelCount = 25;
    public int tunnelLength = 16;
    public float tunnelCurvature = 0.3f;
    public BlockType tunnelBlock = BlockType.Magicstone;

    [Header("Fire Bowl")] public int chunckSize = 16;

    [Header("Обычные блоки")]
    public BlockType baseBlock = BlockType.Stone;
    public BlockType edgeBlock = BlockType.Darkstone;
    public BlockType dirtBlock = BlockType.Dirt;
    public BlockType airBlock = BlockType.None;
    public BlockType nearEdgeBlock = BlockType.Endstone;
    
    public void ApplyPresetForSize(WorldSize size)
    {
        switch (size)
        {
            case WorldSize.Small:
                worldRadius = 64;
                spawnClearRadius = 6;
                tunnelCount = 64;
                tunnelLength = 8;
                nearEdgeBlockRadius = 16;
                break;

            case WorldSize.Medium:
                worldRadius = 96;
                spawnClearRadius = 8;
                tunnelCount = 64;
                tunnelLength = 12;
                nearEdgeBlockRadius = 24;
                break;

            case WorldSize.Large:
                worldRadius = 128;
                spawnClearRadius = 10;
                tunnelCount = 64;
                tunnelLength = 16;
                nearEdgeBlockRadius = 32;
                break;
        }

        spawnRadius = spawnClearRadius;
        worldEdgeSize = 16;
        spaceBetweenWorldAndEdge = 0;
    }
}