using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileManager : MonoBehaviour
{

    public static TileManager I { get; private set; }
    
    [Header("Прочность блоков")] [SerializeField]
    public static Tilemap BlocksTilemap;

    public static Tilemap CracksTilemap;
    public static Tile[] CrackTiles;

    [SerializeField] private Tilemap blocksTilemap;
    [SerializeField] private Tilemap cracksTilemap;

    [SerializeField] private Tile[] crackTiles;

    [SerializeField] private BlockTile dirtTile;
    [SerializeField] private BlockTile stoneTile;
    [SerializeField] private BlockTile expstoneTile;
    [SerializeField] private BlockTile endstoneTile;
    [SerializeField] private BlockTile debugSmartTile;

    private List<Vector3Int> toDestroy = new List<Vector3Int>();
    

    public void Initialize()
    {
        if (I == null) I = this;
        else Destroy(gameObject);
        BlocksTilemap = blocksTilemap;
        CracksTilemap = cracksTilemap;
        CrackTiles = crackTiles;
    }

    public void SetCell(Vector3Int pos, BlockType type)
    {
        BlockTile blockTile;
        switch (type)
        {
            case BlockType.None:
                ClearCell(pos);
                BlockDataManager.removeBlockAtPos(pos);
                return;
            case BlockType.Dirt:
                blockTile = dirtTile;
                break;
            case BlockType.Stone:
                blockTile = stoneTile;
                break;
            case BlockType.Expstone:
                blockTile = expstoneTile;
                break;
            case BlockType.Endstone:
                blockTile = endstoneTile;
                break;
            default:
                Debug.Log("Ошибка с типом блока для размещения");
                //blockTile = debugSmartTile;
                return;
        }
        
        BlockDataManager.removeBlockAtPos(pos);
        new BlockData(type, pos);
        if (blockTile != null) blocksTilemap.SetTile(pos, blockTile);
    }

    public static void damageBlock(Vector3Int pos, float damage)
    {
        BlockData t = BlockDataManager.BLOCKDATALIST.Find(b => b.position == pos);
        if (t == null)
        {
            Debug.LogWarning($"[BlockDataManager] Нет блока по координате {pos}");
            return;
        }

        t.setDurability(t.durability - damage);
    }
    
    public IEnumerator DamageNeighborsWithDelay(Vector3Int origin, float damage)
    {
        float delay = 0.1f; // задержка между разрушениями

        foreach (var offset in WorldGenerator.NEIGHBOURS4X)
        {
            Vector3Int neighborPos = origin + offset;

            if (IsBlockOnPos(neighborPos, BlockType.Expstone))
            {
                if (!toDestroy.Contains(neighborPos))
                {
                    toDestroy.Add(neighborPos);
                    yield return new WaitForSeconds(delay);
                    damageBlock(neighborPos, damage);
                    toDestroy.Remove(neighborPos);
                }
            }
        }
    }
    
    public static void UpdateCracks(Vector3Int pos, BlockData data)
    {
        
        if (data.durability >= data.maxDurability || data.durability <= 0)
        {
            CracksTilemap.SetTile(pos, null);
            return;
        }

        float percent = 1f - data.durability / data.maxDurability;
        int index = Mathf.RoundToInt(percent * (CrackTiles.Length - 1));
        index = Mathf.Clamp(index, 0, CrackTiles.Length - 1);

        CracksTilemap.SetTile(pos, CrackTiles[index]);
    }

    public static void ClearCell(Vector3Int pos) {
        BlocksTilemap.SetTile(pos, null);
    }

    public static bool IsBlockOnPos(Vector3Int pos, BlockType type)
    {
        var tile = BlocksTilemap.GetTile(pos);

        if (type == BlockType.None)
        {
            return tile == null;
        }
        return tile is BlockTile blockTile && blockTile.type == type;
    }
}


public enum BlockType {
    None,
    Dirt = 50,
    Stone = 100,
    Expstone = 40,
    Endstone = 500
}
