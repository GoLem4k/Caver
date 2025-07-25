using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BlockDataManager : MonoBehaviour
{
    public static BlockDataManager I { get; private set; }
    
    public static Dictionary<Vector3Int, BlockData> blockDataDict = new Dictionary<Vector3Int, BlockData>();

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


    [SerializeField] private GameObject _expOrb;

    private void Start()
    {
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

    public void SpawnExpOrb(Vector3Int pos)
    {
        GameObject orb = Instantiate(_expOrb);
        orb.transform.position = pos+ new Vector3(0.5f,0.5f, 0);
    }
}
