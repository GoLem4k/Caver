using UnityEngine;

public class BlockData
{
    private BlockType _type;
    Vector3Int _position;
    private float _durability;
    private float _maxDurability;
    
    public BlockData(BlockType type, Vector3Int position)
    {
        _type = type;
        _position = position;
        switch (type)
        {
            case BlockType.None:
                return;
            case BlockType.Dirt:
                _durability = (int)BlockType.Dirt;
                break;
            case BlockType.Stone:
                _durability = (int)BlockType.Stone;
                break;
            case BlockType.Endstone:
                _durability = (int)BlockType.Endstone;
                break;
            case BlockType.Expstone:
                _durability = (int)BlockType.Expstone; 
                break;
            default:
                _durability = 50;
                break;
        }
        _maxDurability = _durability;
        BlockDataManager.addBlock(this);
    }
    
    public BlockType type {get  {return _type; }}
    public Vector3Int position {get { return _position; }}
    public float durability { get { return _durability; }}
    public float maxDurability {get { return _maxDurability; }}

    public void setDurability(float durability)
    {
        if (maxDurability == (int)BlockType.Endstone) return;
        _durability = durability;
        if (_durability < 0) Destroy();
    }

    public override string ToString()
    {
        return $"BlockType: {_type}, position: {_position}, durability: {_durability}, maxDurability: {_maxDurability}";
    }
    
    public void Destroy() {
        TileManager.ClearCell(_position, TileType.Block);
        BlockDataManager.removeBlock(this);
    }
}