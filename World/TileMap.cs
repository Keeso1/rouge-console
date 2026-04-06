using System.Drawing;
using RogueConsole.Core;
using RogueConsole.Entities;
using Sharpie;

namespace RogueConsole.World;

public class TileMap
{
    public static Tile[,] Tiles { get; private set; }
    public Canvas Canvas { get; init; }
    public bool Active { get; set; }

    public TileMap(Canvas canvas)
    {
        Canvas = canvas;
        InitMap();

        GameState.OnTick += Update;
    }

    public Tile Get(int x, int y) => Tiles[x, y];

    public void Set(int x, int y, Tile tile) => Tiles[x, y] = tile;

    public bool InBounds(int x, int y) =>
        x >= 0 && y >= 0 && x < Canvas.Size.Width && y < Canvas.Size.Height;

    public bool IsWalkable(int x, int y) => InBounds(x, y) && Tiles[x, y].Walkable;

    private void InitMap()
    {
        Tiles = new Tile[Canvas.Size.Width, Canvas.Size.Height];
        for (int w = 0; w < Canvas.Size.Width; w++)
        {
            for (int h = 0; h < Canvas.Size.Height; h++)
            {
                Set(w, h, Tile.Floor);
            }
        }
    }

    public void Render()
    {
        for (int w = 0; w < Canvas.Size.Width; w++)
        for (int h = 0; h < Canvas.Size.Height; h++)
            Canvas.Glyph(new Point(w, h), Tiles[w, h].Glyph, Style.Default);
    }

    public void Update()
    {
        Render();
    }
}
