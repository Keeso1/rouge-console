using System.Drawing;
using RogueConsole.Enums;

namespace RogueConsole.Utils;

public static class TupleExtensions
{
    public static IEnumerable<(int x, int y)> GetCardinalNeighbours(this (int x, int y) t)
    {
        yield return (t.x, t.y - 1); //North
        yield return (t.x - 1, t.y); //West
        yield return (t.x + 1, t.y); //East
        yield return (t.x, t.y + 1); //South
    }

    public static Cardinals ToCardinal(this (int x, int y) offset)
    {
        return offset switch
        {
            (0, -1) => Cardinals.North,
            (-1, 0) => Cardinals.West,
            (1, 0) => Cardinals.East,
            (0, 1) => Cardinals.South,
            _ => Cardinals.Unknown,
        };
    }

    public static bool InBounds(this (int x, int y) t, Size size) =>
        t.x >= 0 && t.y >= 0 && t.x < size.Width && t.y < size.Height;

    public static (int, int) Add(this (int x, int y) t, int X, int Y) => (t.x + X, t.y + Y);

    public static (int, int) Subtract(this (int x, int y) t, int X, int Y) => (t.x - X, t.y - Y);

    public static (int, int) Multiply(this (int x, int y) t, int f) => (t.x * f, t.y * f);

    public static (int, int) Clamp(this (int x, int y) t, Size size) =>
        (Math.Clamp(t.x, 0, size.Width - 1), Math.Clamp(t.y, 0, size.Height - 1));
}
