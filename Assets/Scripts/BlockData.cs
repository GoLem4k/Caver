using UnityEngine;

public class BlockData
{
    private BlockType _type;
    Vector3Int _position;
    private float _durability;
    private float _maxDurability;
    private bool _isBeingDestroyed = false;

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
        BlockDataManager.AddBlock(this);
    }
    
    public BlockType type => _type;
    public Vector3Int position => _position;
    public float durability => _durability;
    public float maxDurability => _maxDurability;

    public void setDurability(float durability)
    {
        if (_maxDurability == (int)BlockType.Endstone) return;
        _durability = durability;

        if (!_isBeingDestroyed)
            TileManager.UpdateCracks(_position, this);

        if (_durability <= 0 && !_isBeingDestroyed)
            Destroy();
    }

    public override string ToString()
    {
        return $"BlockType: {_type}, position: {_position}, durability: {_durability}, maxDurability: {_maxDurability}";
    }

    public void Destroy()
    {
        //PlayerDataManager.I._exp += RunData.I.blockBreakExp * RunData.I.globalExpMultiplier;
        
        if (_isBeingDestroyed) return;
        _isBeingDestroyed = true;
        
        BlockDataManager.I.SpawnExpOrb(_position);
        

        if (this.type == BlockType.Expstone)
        {
            TileManager.I.StartCoroutine(TileManager.I.DamageNeighborsWithDelay(_position, _maxDurability));
        }

        TileManager.ClearCell(_position);
        BlockDataManager.RemoveBlock(this);
    }
}
