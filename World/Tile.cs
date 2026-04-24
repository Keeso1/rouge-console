using Vimonia.Entities;
using Vimonia.Assets;
using System.Text;
using Vimonia.Core;
using Vimonia.Interfaces;
using Sharpie;

namespace Vimonia.World;


public class Tile {
    public Rune Glyph { get; set; }
    public string Text { get; set; } = string.Empty;
    public Entity? Entity { get; set; }
    public bool Walkable { get; set; } = false;
    public bool Visible { get; set; } = true;

    public static Tile Floor() => new() { Glyph = GameConstants.Ground, Walkable = true };
    public static Tile Wall() => new() { Glyph = GameConstants.Wall };
    public static Tile Goblin(Entity entity) => new() { Text = entity.Body, Entity = entity, Walkable = true };
    // public static Tile Player(Player player) => new() { Glyph = GameConstants.Player, Entity = player };
    public static Tile Chest() => new() { Glyph = GameConstants.Item };
    public static Tile Door() => new() { Glyph = GameConstants.Door, Walkable = true };
    public static Tile Rock() => new() { Glyph = GameConstants.Rock };
}


