using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BlockDataManager : MonoBehaviour
{
    public static BlockDataManager I { get; private set; }
    
    public static List<BlockData> BLOCKDATALIST = new List<BlockData>();
    
    public static void addBlock(BlockData block) { BLOCKDATALIST.Add(block); }
    public static void removeBlock(BlockData block) { BLOCKDATALIST.Remove(block); }
    public static void removeBlockAtPos(Vector3Int pos) { BLOCKDATALIST.RemoveAll(b => b.position == pos); }

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
        foreach (var VARIABLE in BLOCKDATALIST)
        {
            switch (VARIABLE.type)
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
