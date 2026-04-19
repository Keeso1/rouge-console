using Vimonia.Entities;
using Vimonia.Assets;
using System.Text;
using System.Drawing;
using Vimonia.Core;

namespace Vimonia.World;


public class Tile {
    public Rune Glyph { get; set; }
    public Entity? Entity { get; set; }
    public bool Walkable { get; set; } = false;
    public bool Visible { get; set; } = true;

    public static Tile Floor() => new() { Glyph = GameConstants.Ground, Walkable = true };
    public static Tile Wall() => new() { Glyph = GameConstants.Wall };
    public static Tile Goblin(Point position, Entity entity) => new() { Glyph = GameConstants.Enemy};
    public static Tile Player() => new() { Glyph = GameConstants.Player };
    public static Tile Chest() => new() { Glyph = GameConstants.Item };
    public static Tile Door() => new() { Glyph = GameConstants.Door, Walkable = true };
    public static Tile Rock() => new() { Glyph = GameConstants.Rock };
}


