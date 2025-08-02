using UnityEngine;

public class StructureGenerator : MonoBehaviour
{
    [SerializeField] private TileManager tileManager;
    [SerializeField] private StructureManager structureManager;

    public void GenerateStructures(int seed, WorldGenSettings settings)
    {
        var rng = new System.Random(seed);
        GenerateFireBowls(rng, settings);
        GenerateAltars(rng, settings);
    }
    public void GenerateFireBowls(System.Random rng, WorldGenSettings settings)
    {
        int settlementArea = settings.worldRadius - settings.nearEdgeBlockRadius;
        for (int i = -settlementArea; i < settlementArea; i += settings.chunckSize)
        {
            for (int j = -settlementArea; j < settlementArea; j += settings.chunckSize)
            {
                for (int k = 0; k < 16; k++)
                {
                    int x = 4 + rng.Next(settings.chunckSize - 8);
                    int y = 4 + rng.Next(settings.chunckSize - 8);
                    if (structureManager.TrySpawnStructure(new Vector3Int(i + x, j + y),
                            StructureManager.StructureType.FireBowl)) break;
                }
            }
        }
    }

    public void GenerateAltars(System.Random rng, WorldGenSettings settings)
    {
        for (int i = 0; i < 4; i++)
        {
            float x = (float)(rng.NextDouble() * (settings.worldRadius/1.5f - settings.worldRadius * 0.25f) + settings.worldRadius * 0.25f);
            float y = (float)(rng.NextDouble() * (settings.worldRadius/1.5f - settings.worldRadius * 0.25f) + settings.worldRadius * 0.25f);

            switch (i)
            {
                case 0: x = -Mathf.Abs(x); y =  Mathf.Abs(y); break; // -x +y
                case 1: x =  Mathf.Abs(x); y =  Mathf.Abs(y); break; // +x +y
                case 2: x =  Mathf.Abs(x); y = -Mathf.Abs(y); break; // +x -y
                case 3: x = -Mathf.Abs(x); y = -Mathf.Abs(y); break; // -x -y
            }

            Vector2 pos = new Vector2(x, y);
            Vector3Int intPos = new Vector3Int(Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y), 0); 
            structureManager.TrySpawnStructure(intPos, StructureManager.StructureType.Altar, true);
        }
    }
}
