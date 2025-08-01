using UnityEngine;

public class Initializer : MonoBehaviour
{
    public TileManager tileManager;
    public WorldGenerator worldGenerator;
    public VectorMovementController vectorMovementController;
    public PlayerDataManager playerDataManager;
    public UpgradesManager upgradesManager;
    public RunData runData;
    public StructureManager structureManager;
    
    void Awake()
    {
        structureManager.Initialize();
        runData.Initialize();
        tileManager.Initialize();
        worldGenerator.Initialize();
        vectorMovementController.Initialize();
        upgradesManager.Initialize();
        playerDataManager.Initialize();
    }
}
