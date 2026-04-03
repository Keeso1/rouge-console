using System.Drawing;
using RogueConsole.Core;
using RogueConsole.Entities;
using Sharpie;

namespace RogueConsole.World;

public class TileMap
{

	public int Width { get; private set; }
	public int Height { get; private set; }
	public static Tile[,] Tiles { get; private set; }
	public Canvas Canvas { get; init; }

	public TileMap(int width, int height, Canvas canvas)
	{
		Width = width;
		Height = height;
		Canvas = canvas;
		InitMap();

		GameState.OnTick += Update;
	}

	Tile Get(int x, int y) => Tiles[x, y];
	public void Set(int x, int y, Tile tile) => Tiles[x, y] = tile;
	public bool InBounds(int x, int y) => x >= 0 && y >= 0 && x < Width && y < Height;
	public bool IsWalkable(int x, int y) => InBounds(x, y) && Tiles[x, y].Walkable;


	private void InitMap()
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

	public static (int, int)? GetPlayer()
	{
		//TODO: Temporär for loop. Ska senare vara en property
		for (int y = 0; y < Tiles.GetLength(1); y++)
		{
			for (int x = 0; x < Tiles.GetLength(0); x++)
			{
				if (Tiles[y, x].Entity?.Type == EntityType.Player)
				{
					return (x, y);
				}
			}
		}

		return null;
	}
}
