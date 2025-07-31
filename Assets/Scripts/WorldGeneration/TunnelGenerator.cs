using UnityEngine;

public class TunnelGenerator : MonoBehaviour
{
    [SerializeField] private TileManager tileManager;

    private static readonly Vector3Int[] Horizontal = { Vector3Int.left, Vector3Int.right };
    private static readonly Vector3Int[] Vertical = { Vector3Int.up, Vector3Int.down };

    public void GenerateTunnels(int seed, Vector3Int center, int radius, WorldGenSettings settings)
    {
        var rng = new System.Random(seed);

        for (int i = 0; i < settings.tunnelCount; i++)
        {
            var start = GetRandomPointOnCircle(center, radius, rng);
            var dir = GetRandomCardinalDirection(rng);
            GenerateTunnel(start, dir, settings.tunnelLength, settings.tunnelCurvature, settings.tunnelBlock, rng);
        }
    }

    private void GenerateTunnel(Vector3Int pos, Vector3Int dir, int length, float curvature, BlockType type, System.Random rng)
    {
        for (int i = 0; i < length; i++)
        {
            tileManager.SetCell(pos, type);

            bool turn = rng.NextDouble() < curvature;
            if (turn)
            {
                dir = (dir.x != 0) ? Vertical[rng.Next(Vertical.Length)] : Horizontal[rng.Next(Horizontal.Length)];
            }

            pos += dir;
        }
    }

    private Vector3Int GetRandomPointOnCircle(Vector3Int center, int radius, System.Random rng)
    {
        double angle = rng.NextDouble() * 2 * Mathf.PI;
        int x = Mathf.RoundToInt(center.x + Mathf.Cos((float)angle) * radius);
        int y = Mathf.RoundToInt(center.y + Mathf.Sin((float)angle) * radius);
        return new Vector3Int(x, y, 0);
    }

    private Vector3Int GetRandomCardinalDirection(System.Random rng)
    {
        return new[] { Vector3Int.up, Vector3Int.down, Vector3Int.left, Vector3Int.right }[rng.Next(4)];
    }
}