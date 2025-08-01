using UnityEngine;

public class StructureGenerator : MonoBehaviour
{
    [SerializeField] private TileManager tileManager;
    [SerializeField] private StructureManager structureManager;
    public void GenerateFireBowls(int seed, WorldGenSettings settings)
    {
        var rng = new System.Random(seed);
        int settlementArea = settings.worldRadius - settings.nearEdgeBlockRadius;
        int settlementChunckArea = settings.chunckSize;
        for (int i = -settlementArea; i < settlementArea; i += settings.chunckSize)
        {
            for (int j = -settlementArea; j < settlementArea; j += settings.chunckSize)
            {
                for (int k = 0; k < 16; k++)
                {
                    int x = 4 + rng.Next(8);
                    int y = 4 + rng.Next(8);
                    if (structureManager.TrySpawnStructure(new Vector3Int(i + x, j + y),
                            StructureManager.StructureType.FireBowl)) break;
                }
            }
        }
    }

    public void GenerateAltars()
    {
        
    }
}
