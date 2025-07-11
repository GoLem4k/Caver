using UnityEngine;

public class Initializer : MonoBehaviour
{
    public TileManager tileManager;
    public WorldGenerator worldGenerator;
    public VectorMovementController vectorMovementController;
    
    void Start()
    {
        tileManager.Initialize();
        worldGenerator.Initialize();
        vectorMovementController.Initialize();
    }
}
