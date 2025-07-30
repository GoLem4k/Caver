using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "Tiles/BlockTile")]
public class BlockTile : RuleTile<BlockTile.Neighbor> {

    public BlockType type;

    public class Neighbor : RuleTile.TilingRule.Neighbor {
        public const int AnySmartTile = 10;
    }

    public override bool RuleMatch(int neighbor, TileBase tile) {
        if (neighbor == Neighbor.AnySmartTile) {
            return tile is BlockTile;
        }
        return base.RuleMatch(neighbor, tile);
    }
}