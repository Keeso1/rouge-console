namespace RogueConsole.World;

class TileMap
{
	public int Width { get; set; }
	public int Height { get; set; }
	private readonly Tile[,] _tiles;

	public Tile Get(int x, int y) => _tiles[x, y];
	public void Set(int x, int y, Tile tile) => _tiles[x, y] = tile;
	public bool InBounds(int x, int y) => x >= 0 && y >= 0 && x < Width && y < Height;
	public bool IsWalkable(int x, int y) => InBounds(x, y) && _tiles[x, y].Walkable;

	public virtual void Update()
	{
		// override for animated tiles, fog of war updates, etc.
	}
}
