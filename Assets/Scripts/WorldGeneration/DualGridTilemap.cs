/*using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using static TileType;

public class DualGridTilemap : MonoBehaviour {
    protected static Vector3Int[] NEIGHBOURS = new Vector3Int[] {
        new Vector3Int(0, 0, 0),
        new Vector3Int(1, 0, 0),
        new Vector3Int(0, 1, 0),
        new Vector3Int(1, 1, 0)
    };

    protected static Dictionary<Tuple<TileType, TileType, TileType, TileType>, Tile> neighbourTupleToTile;

    // Provide references to each tilemap in the inspector
    public Tilemap placeholderTilemap;
    public Tilemap displayStoneTilemap;
    public Tilemap oreTilemap;
    public Tilemap displatEndstoneTilemap;
    public Tilemap trashTilemap;

    // Provide the dirt and grass placeholder tiles in the inspector
    public Tile stoneDebugTile;
    public Tile airDebugTile;
    public Tile oreDebugTile;
    public Tile endstoneDebugTile;

    public Tile oreTile;

    // Provide the 16 tiles in the inspector
    public Tile[] tiles;

    [Header("Прочность блоков")]
    public float stoneDurability = 100f;
    public float oreDurability = 150f;

    public int airTilesCount = 0;
    public int stoneTilesCount = 0;
    public int oreTilesCount = 0;
    public int endstoneCount = 0;
    
    public Dictionary<Vector3Int, float> durabilityData;
    
    public void Initialize() {
        //displayTilemap.chunkCullingBounds = new Bounds(Vector3.zero, Vector3.positiveInfinity);
        // This dictionary stores the "rules", each 4-neighbour configuration corresponds to a tile
        // |_1_|_2_|
        // |_3_|_4_|
        neighbourTupleToTile = new() {
            {new (Stone, Stone, Stone, Stone), tiles[6]},
            {new (Air, Air, Air, Stone), tiles[13]}, // OUTER_BOTTOM_RIGHT
            {new (Air, Air, Stone, Air), tiles[0]}, // OUTER_BOTTOM_LEFT
            {new (Air, Stone, Air, Air), tiles[8]}, // OUTER_TOP_RIGHT
            {new (Stone, Air, Air, Air), tiles[15]}, // OUTER_TOP_LEFT
            {new (Air, Stone, Air, Stone), tiles[1]}, // EDGE_RIGHT
            {new (Stone, Air, Stone, Air), tiles[11]}, // EDGE_LEFT
            {new (Air, Air, Stone, Stone), tiles[3]}, // EDGE_BOTTOM
            {new (Stone, Stone, Air, Air), tiles[9]}, // EDGE_TOP
            {new (Air, Stone, Stone, Stone), tiles[5]}, // INNER_BOTTOM_RIGHT
            {new (Stone, Air, Stone, Stone), tiles[2]}, // INNER_BOTTOM_LEFT
            {new (Stone, Stone, Air, Stone), tiles[10]}, // INNER_TOP_RIGHT
            {new (Stone, Stone, Stone, Air), tiles[7]}, // INNER_TOP_LEFT
            {new (Air, Stone, Stone, Air), tiles[14]}, // DUAL_UP_RIGHT
            {new (Stone, Air, Air, Stone), tiles[4]}, // DUAL_DOWN_RIGHT
            {new (Air, Air, Air, Air), tiles[12]},
        };
        durabilityData = new() { };
    }

    public void SetCell(Vector3Int pos, Tile tile)
    {
        placeholderTilemap.SetTile(pos, tile);

        switch (tile)
        {
            case var _ when tile == stoneDebugTile:
                setDisplayTile(pos);
                SetDurability(pos, stoneDurability);
                stoneTilesCount++;
                break;

            case var _ when tile == airDebugTile:
                setDisplayTile(pos);
                oreTilemap.SetTile(pos, null);
                SetDurability(pos, 0);
                airTilesCount++;
                break;

            case var _ when tile == oreDebugTile:
                oreTilemap.SetTile(pos, oreTile);
                SetDurability(pos, oreDurability);
                stoneTilesCount--;
                oreTilesCount++;
                break;
            
            case var _ when tile == endstoneDebugTile:
                setDisplayTile(pos);
                SetDurability(pos, 0);
                endstoneCount++;
                break;
        }
    }

    private void SetDurability(Vector3Int pos, float durability)
    {
        durabilityData[pos] = durability;
    }
    
    public void UpdateDurability(Vector3Int pos, float d)
    {
        durabilityData[pos] += d;
        if (durabilityData[pos] < 0 && getPlaceholderTileTypeAt(pos) != Endstone)
        {
            SetCell(pos, airDebugTile);
        } 
    }

    public Tile GetCellAt(Vector3Int cords)
    {
        return placeholderTilemap.GetTile(cords) as Tile;
    }

    private TileType getPlaceholderTileTypeAt(Vector3Int coords) {
        Tile tile = placeholderTilemap.GetTile(coords) as Tile;

        if (tile == stoneDebugTile) return Stone;
        if (tile == endstoneDebugTile) return Endstone;
        //if (tile == oreDebugTile) return Ore;
        return Air;
    }
    
    private TileType getVisualTileTypeAt(Vector3Int coords)
    {
        Tile tile = placeholderTilemap.GetTile(coords) as Tile;
        if (tile == stoneDebugTile || tile == oreDebugTile || tile == endstoneDebugTile)
            return Stone; // визуально это камень
        return Air;
    }


    protected Tile calculateDisplayTile(Vector3Int coords) {
        // 4 neighbours
        TileType topRight = getVisualTileTypeAt(coords - NEIGHBOURS[0]);
        TileType topLeft = getVisualTileTypeAt(coords - NEIGHBOURS[1]);
        TileType botRight = getVisualTileTypeAt(coords - NEIGHBOURS[2]);
        TileType botLeft = getVisualTileTypeAt(coords - NEIGHBOURS[3]);

        Tuple<TileType, TileType, TileType, TileType> neighbourTuple = new(topLeft, topRight, botLeft, botRight);
        
        return neighbourTupleToTile[neighbourTuple];
    }

    protected void setDisplayTile(Vector3Int pos)
    {
        Tilemap tilemap;
        TileType type = getPlaceholderTileTypeAt(pos);
        switch (type) {
            case Stone:
                tilemap = displayStoneTilemap;
                break;
            case Endstone:
                tilemap = displatEndstoneTilemap;
                break;
            case Air:
                tilemap = displayStoneTilemap;
                break;
            default:
                tilemap = trashTilemap;
                break;
        }
        
        for (int i = 0; i < NEIGHBOURS.Length; i++) {
            Vector3Int newPos = pos + NEIGHBOURS[i];
            tilemap.SetTile(newPos, calculateDisplayTile(newPos));
        }
    }

    // The tiles on the display tilemap will recalculate themselves based on the placeholder tilemap
    public void RefreshDisplayTilemap() {
        for (int i = 0; i < WorldGenerator.WORLDRADIUS; i++) {
            for (int j = 0; j < WorldGenerator.WORLDRADIUS; j++) {
                setDisplayTile(new Vector3Int(i, j, 0));
            }
        }
    }
}

public enum TileType {
    Air,
    Stone,
    Endstone,
    Ore
}*/