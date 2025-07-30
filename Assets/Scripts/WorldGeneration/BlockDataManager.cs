using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BlockDataManager : MonoBehaviour
{
    public static BlockDataManager I { get; private set; }
    
    public static Dictionary<Vector3Int, BlockData> blockDataDict = new Dictionary<Vector3Int, BlockData>();
    
    private System.Random rng;

    public static void AddBlock(BlockData block)
    {
        blockDataDict[block.position] = block;
    }

    public static void RemoveBlock(BlockData block)
    {
        blockDataDict.Remove(block.position);
    }

    public static void RemoveBlockAtPos(Vector3Int pos)
    {
        blockDataDict.Remove(pos);
    }


    [SerializeField] private GameObject _defaultExpOrb;
    [SerializeField] private GameObject _greenExpOrb;
    [SerializeField] private GameObject _purpleExpOrb;

    private void Start()
    {
        rng = new System.Random(RunData.I.SEED);
        if (I == null) I = this;
    }


    public override string ToString()
    {
        int dirtCount = 0;
        int stoneCount = 0;
        int endstoneCount = 0;
        int expstoneCount = 0;
        foreach (var VARIABLE in blockDataDict)
        {
            switch (VARIABLE.Value.type)
            {
                case BlockType.Dirt:
                    dirtCount++;
                    break;
                case BlockType.Stone:
                    stoneCount++;
                    break;
                case BlockType.Endstone:
                    endstoneCount++;
                    break;
                case BlockType.Expstone:
                    expstoneCount++;
                    break;
            }
        }
        return $"Dirt: {dirtCount}, Stone: {stoneCount}, Endstone: {endstoneCount}, Expstone: {expstoneCount}";
    }

    public void SpawnGreenExpOrb(Vector3Int pos)
    {
        GameObject orb = Instantiate(_greenExpOrb);
        orb.transform.position = pos+ new Vector3(0.5f + (float)rng.NextDouble()*0.8f - 0.4f,0.5f + (float)rng.NextDouble()*0.8f - 0.4f, 0);
    }
    
    public void SpawnPurpleExpOrb(Vector3Int pos)
    {
        GameObject orb = Instantiate(_purpleExpOrb);
        orb.transform.position = pos+ new Vector3(0.5f + (float)rng.NextDouble()*0.8f - 0.4f,0.5f + (float)rng.NextDouble()*0.8f - 0.4f, 0);
    }
    
    public void SpawnDefaultExpOrb(Vector3Int pos)
    {
        GameObject orb = Instantiate(_defaultExpOrb);
        orb.transform.position = pos+ new Vector3(0.5f + (float)rng.NextDouble()*0.8f - 0.4f,0.5f + (float)rng.NextDouble()*0.8f - 0.4f, 0);
    }
}
