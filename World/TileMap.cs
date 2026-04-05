using System.Drawing;
using RogueConsole.Core;
using RogueConsole.Entities;
using Sharpie;

namespace RogueConsole.World;

public class TileMap
{

	public int Width { get; private set; }
	public int Height { get; private set; }
	protected Tile[,] Tiles { get; private set; }
	protected Canvas Canvas { get; init; }

	public TileMap(int width, int height, Canvas canvas)
	{
		Width = width;
		Height = height;
		Canvas = canvas;
		Tiles = new Tile[Width, Height];
		InitMap();

		GameState.OnTick += Update;
	}

	public Tile Get(int x, int y) => Tiles[x, y];

	///<summary>
	///Simple set function. Set tile at x and y to specific <c>Tile</c>.
	///There are two other overloads regarding coords and looping over them
	///</summary>
	protected void Set(int x, int y, Tile tile) => Tiles[x, y] = tile;

	/// <summary>
	/// Overload for looping over tuples, ex. a line across the room
	/// looping over tuples can be achieved by <c> GetCanvasCoords.GetLine() </c> helper function
	/// </summary>
	protected void Set(IEnumerable<(int x, int y)> coords, Tile tile)
	{
		foreach (var coord in coords)
			Tiles[coord.x, coord.y] = tile;
	}

	///<summary>
	///Overload to be able to accept tuples using <c> GetCanvasCoords </c> helper class
	///</summary>
	protected void Set((int x, int y) coord, Tile tile) => Tiles[coord.x, coord.y] = tile;

	public bool InBounds(int x, int y) => x >= 0 && y >= 0 && x < Width && y < Height;
	public bool IsWalkable(int x, int y) => InBounds(x, y) && Tiles[x, y].Walkable;

	protected void Fill()
	{
		for (int w = 0; w < Width; w++)
		{
			for (int h = 0; h < Height; h++)
			{
				Set(w, h, Tile.Floor);
			}
		}
	}

	protected virtual void InitMap()
	{

		Tiles = new Tile[Width, Height];
		for (int w = 0; w < Width; w++)
		{
			for (int h = 0; h < Height; h++)
			{
				Set(w, h, Tile.Floor);
			}
		}
	}

	public void Render()
	{
		for (int w = 0; w < Width; w++)
			for (int h = 0; h < Height; h++)
				Canvas.Glyph(new Point(w, h), Tiles[w, h].Glyph, Style.Default);
	}

	public void Update()
	{
		Render();
	}
}
