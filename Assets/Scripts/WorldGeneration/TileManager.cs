using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileManager : PausedBehaviour
{

    public static TileManager I { get; private set; }
    
    [Header("Прочность блоков")] [SerializeField]
    public static Tilemap BlocksTilemap;
    public static Tilemap CracksTilemap;
    public static Tilemap SelectorTilemap;
    
    public static Tile[] CrackTiles;

    [SerializeField] private Tilemap blocksTilemap;
    [SerializeField] private Tilemap cracksTilemap;
    [SerializeField] private Tilemap selectorTilemap;
    [SerializeField] private Tilemap BackgroundTilemap;

    [SerializeField] private Tile[] crackTiles;

    [SerializeField] private BlockTile dirtTile;
    [SerializeField] private BlockTile stoneTile;
    [SerializeField] private BlockTile endstoneTile;
    [SerializeField] private BlockTile expstoneTile;
    [SerializeField] private BlockTile magicstoneTile;
    [SerializeField] private BlockTile tntTile;
    [SerializeField] private BlockTile darkstoneTile;
    [SerializeField] private BlockTile debugSmartTile;
    
    [SerializeField] private RuleTile bgRuleTile;

    private Vector3Int previousSelectorPos;
    private Vector3Int currentSelectorPos;
    [SerializeField] private Tile selectorTile;
    [SerializeField] private Tile selectorCooldownTile;

    private List<Vector3Int> toDestroy = new List<Vector3Int>();
    
    public Tilemap physicalTilemap;   // Тайлмап-копия
    public Tilemap bgDynemicsTilemap;   // Тайлмап-копия
    public Tilemap displayTilemap;   // Тайлмап-копия
    public Transform player;        // Игрок
    Dictionary<Tilemap, HashSet<Vector3Int>> previousPositionsDict = new();

    private BoundsInt _lastBounds;
    private TileBase[] _lastTiles;
    
    Vector3 lastPlayerCell;
    int frameCounter;
    
    public static Vector3Int[] NEIGHBOURS8X = new Vector3Int[] {
        new Vector3Int(1, 0, 0),     // справа
        new Vector3Int(-1, 0, 0),    // слева
        new Vector3Int(0, 1, 0),     // сверху
        new Vector3Int(0, -1, 0),    // снизу
        new Vector3Int(-1, 1, 0),    // сверху и слева
        new Vector3Int(1, 1, 0),    // сверху и справа
        new Vector3Int(-1, -1, 0),    // снизу и слева
        new Vector3Int(1, -1, 0)    // снизу и справа
    };
    
    public static Vector3Int[] NEIGHBOURS4X = new Vector3Int[] {
        new Vector3Int(1, 0, 0),     // справа
        new Vector3Int(-1, 0, 0),    // слева
        new Vector3Int(0, 1, 0),     // сверху
        new Vector3Int(0, -1, 0)    // снизу
    };

    

    public void Initialize()
    {
        if (I == null) I = this;
        else Destroy(gameObject);
        BlocksTilemap = blocksTilemap;
        CracksTilemap = cracksTilemap;
        SelectorTilemap = selectorTilemap;
        CrackTiles = crackTiles;
    }

    public Tilemap GetBlockTilemap()
    {
        return BlocksTilemap;
    }
    
    public void SetCell(Vector3Int pos, BlockType type)
    {
        BlockTile blockTile;
        switch (type)
        {
            case BlockType.None:
                ClearCell(pos);
                BlockDataManager.RemoveBlockAtPos(pos);
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
            case BlockType.Magicstone:
                blockTile = magicstoneTile;
                break;
            case BlockType.Tnt:
                blockTile = tntTile;
                break;
            case BlockType.Endstone:
                blockTile = endstoneTile;
                break;
            case BlockType.Darkstone:
                blockTile = darkstoneTile;
                break;
            default:
                Debug.Log("Ошибка с типом блока для размещения");
                //blockTile = debugSmartTile;
                return;
        }
        
        BlockDataManager.RemoveBlockAtPos(pos);
        new BlockData(type, pos);
        if (blockTile != null) blocksTilemap.SetTile(pos, blockTile);
    }

    public void SetBgCell(Vector3Int pos)
    {
        BackgroundTilemap.SetTile(pos, bgRuleTile);
    }

    public static void DamageBlock(Vector3Int pos, float damage)
    {
        if (!BlockDataManager.blockDataDict.TryGetValue(pos, out BlockData t))
        {
            Debug.LogWarning($"[BlockDataManager] Нет блока по координате {pos}");
            return;
        }

        t.setDurability(t.durability - damage);
    }
    
    public IEnumerator DamageNeighborsWithDelay(Vector3Int origin, float damage)
    {
        float delay = 0.1f; // задержка между разрушениями

        foreach (var offset in NEIGHBOURS4X)
        {
            if (PAUSE)
                while (PAUSE)
                {
                    yield return null;
                }

            Vector3Int neighborPos = origin + offset;
            if (IsBlockOnPos(neighborPos, BlockType.Magicstone))
            {
                if (!toDestroy.Contains(neighborPos))
                {
                    toDestroy.Add(neighborPos);
                    yield return new WaitForSeconds(delay);
                    DamageBlock(neighborPos, damage);
                    toDestroy.Remove(neighborPos);
                }
            }
        }
    }

    public void DamageSurrounded(Vector3Int pos, float damage)
    {
        foreach (var offset in NEIGHBOURS8X)
        {
            if (!IsBlockOnPos(pos + offset, BlockType.None))
            {
                DamageBlock(pos + offset, damage);
            };
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

    public static bool IsBlockOnPos(Vector3Int pos)
    {
        var tile = BlocksTilemap.GetTile(pos);
        return tile != null;
    }

    public bool IsTileByWorldPos(Vector3 pos)
    {
        Vector3Int newPos = GetBlockTilemap().WorldToCell(pos);
        if (GetBlockTilemap().HasTile(newPos))
        {
            return true;
        }
        return false;
    }

    public void SetSelectorTileOnPos(Vector3Int pos, bool isCooldown)
    {
        if (pos != currentSelectorPos)
        {
            previousSelectorPos = currentSelectorPos;
            currentSelectorPos = pos;
            selectorTilemap.SetTile(previousSelectorPos, null);            
        }
        selectorTilemap.SetTile(pos, (isCooldown) ? selectorCooldownTile : selectorTile);
    }
    

    public void ResetSelector()
    {
        selectorTilemap.SetTile(currentSelectorPos, null);
        currentSelectorPos = new Vector3Int(0,0,-1);
    }

    public void DamageSelectTile()
    {
        if (selectorTilemap.HasTile(currentSelectorPos))
        {
            DamageBlock(currentSelectorPos, RunData.I.damage);
        }
    }



    protected override void GameUpdate()
    {
        frameCounter++;
        Vector3Int currentCell = blocksTilemap.WorldToCell(player.position);

        if (frameCounter % 5 == 0 || currentCell != lastPlayerCell)
        {
            SyncTilemapAroundPlayer(blocksTilemap, physicalTilemap, 4);
            SyncTilemapAroundPlayer(blocksTilemap, displayTilemap, 12);
            SyncTilemapAroundPlayer(BackgroundTilemap, bgDynemicsTilemap, 12);
            lastPlayerCell = currentCell;
        }
    }

    void SyncTilemapAroundPlayer(Tilemap sourceTilemap, Tilemap targetTilemap, int copyRadius)
    {
        Vector3Int playerCell = sourceTilemap.WorldToCell(player.position);
        BoundsInt bounds = new BoundsInt(
            playerCell.x - copyRadius, playerCell.y - copyRadius, 0,
            copyRadius * 2 + 1, copyRadius * 2 + 1, 1);

        // Инициализация кэша, если его ещё нет
        if (!previousPositionsDict.ContainsKey(targetTilemap))
            previousPositionsDict[targetTilemap] = new HashSet<Vector3Int>();

        var prevPositions = previousPositionsDict[targetTilemap];
        var newPositions = new HashSet<Vector3Int>();

        foreach (var pos in bounds.allPositionsWithin)
        {
            TileBase sourceTile = sourceTilemap.GetTile(pos);
            TileBase targetTile = targetTilemap.GetTile(pos);

            if (sourceTile != targetTile)
                targetTilemap.SetTile(pos, sourceTile);

            newPositions.Add(pos);
        }

        foreach (var oldPos in prevPositions)
        {
            if (!newPositions.Contains(oldPos))
                targetTilemap.SetTile(oldPos, null);
        }

        previousPositionsDict[targetTilemap] = newPositions;
    }

}


public enum BlockType {
    None,
    Dirt = 25,
    Expstone = 30,
    Stone = 55,
    Tnt = 60,
    Endstone = 75,
    Magicstone = 90,
    Darkstone = -1,
    
}
