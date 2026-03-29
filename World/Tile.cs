namespace RogueConsole.World;

class Tile
{
	public char Glyph { get; set; }
	public bool Walkable { get; set; }
	public bool Visible { get; set; }
	public bool Explored { get; set; }

	public static Tile Floor => new() { Glyph = '.', Walkable = true };
	public static Tile Wall => new() { Glyph = '#', Walkable = false };
}
