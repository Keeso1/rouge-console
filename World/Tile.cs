using RogueConsole.Entities;
using RogueConsole.Assets;
using System.Text;

namespace RogueConsole.World;


public class Tile
{
	public Rune Glyph { get; set; }
	public Entity? Entity { get; set; }
	public bool Walkable { get; set; } = false;
	public bool Visible { get; set; } = true;

	public static Tile Floor => new() { Glyph = GameConstants.Ground, Walkable = true };
	public static Tile Wall => new() { Glyph = GameConstants.Wall };
	public static Tile Goblin => new() { Glyph = GameConstants.Enemy, Entity = new Goblin(100, 100) };
	public static Tile Player => new() { Glyph = GameConstants.Player };
	public static Tile Chest => new() { Glyph = GameConstants.Item };
}


