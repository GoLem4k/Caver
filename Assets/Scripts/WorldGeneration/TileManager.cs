using UnityEngine;
using UnityEngine.Tilemaps;

public class TileManager : MonoBehaviour
{

    [Header("Прочность блоков")] [SerializeField]
    public static Tilemap blocksTilemap;

    [SerializeField] private Tilemap oreTilemap;
    [SerializeField] private Tilemap decorationsTilemap;

    [SerializeField] private BlockTile dirtTile;
    [SerializeField] private BlockTile stoneTile;
    [SerializeField] private BlockTile expstoneTile;
    [SerializeField] private BlockTile endstoneTile;
    [SerializeField] private BlockTile debugSmartTile;

    [SerializeField] private OreTile copperOreTile;
    [SerializeField] private OreTile debugOreTile;

    public void Initialize()
    {
        blocksTilemap = gameObject.GetComponentInChildren<Tilemap>();
    }

    public void SetCell(Vector3Int pos, BlockType type)
    {
        BlockTile blockTile;
        switch (type)
        {
            case BlockType.None:
                ClearCell(pos, TileType.Block);
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

    public void SetCell(Vector3Int pos, OreType type)
    {
        OreTile oreTile;
        switch (type)
        {
            case OreType.Copper:
                oreTile = copperOreTile;
                break;
            default:
                Debug.Log("Ошибка с типом руды для размещения");
                //oreTile = debugOreTile;
                return;
        }

        if (oreTile != null) oreTilemap.SetTile(pos, oreTile);
    }

    public static void ClearCell(Vector3Int pos, TileType type) {
        Tilemap tilemap = null;
        switch (type)
        {
            case TileType.Block:
                tilemap = blocksTilemap;
                break;
            default:
                Debug.Log("Ошибка с типом тайла для удаления, тип не найден");
                return;
        }
        if (tilemap != null) tilemap.SetTile(pos, null);
    }

    public bool IsBlockOnPos(Vector3Int pos, BlockType type)
    {
        var tile = blocksTilemap?.GetTile(pos);

        if (type == BlockType.None)
        {
            return tile == null;
        }
        return tile is BlockTile blockTile && blockTile.type == type;
    }


    public bool IsOreOnPos(Vector3Int pos, OreType type)
    {
        var tile = oreTilemap?.GetTile(pos);
        if (tile is OreTile oreTile) return oreTile.type == type;
        return false;
    }
}


public enum BlockType {
    None,
    Dirt = 50,
    Stone = 100,
    Expstone = 40,
    Endstone = 500
}

public enum OreType {
    Copper
}

public enum TileType
{
    Block,
    Ore,
    Decorate
}
