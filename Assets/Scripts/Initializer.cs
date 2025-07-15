using UnityEngine;

public class Initializer : MonoBehaviour
{
    public TileManager tileManager;
    public WorldGenerator worldGenerator;
    public VectorMovementController vectorMovementController;
    public PlayerDataManager playerDataManager;
    public UpgradesManager upgradesManager;
    
    void Start()
    {
        tileManager.Initialize();
        worldGenerator.Initialize();
        vectorMovementController.Initialize();
        upgradesManager.Initialize();
        playerDataManager.Initialize();
    }
}
