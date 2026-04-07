using System.Drawing;
using Microsoft.Extensions.Logging;
using RogueConsole.Core;
using RogueConsole.Entities;
using RogueConsole.Enums;
using RogueConsole.Utils;
using RogueConsole.World.Maps;
using Sharpie;

namespace RogueConsole.World;

public class TileMap
{
    public Tile[,] Tiles { get; private set; }
    public Canvas Canvas { get; init; }
    public RoomTypes RoomType { get; set; }

    public TileMap(Canvas canvas)
    {
        Canvas = canvas;

        // GameState.OnTick += Update;
    }

    // public TileMap(Canvas canvas, RoomTypes type = RoomTypes.Normal)
    // {
    // 	RoomType = RoomTypes.Normal;
    // 	Canvas = canvas;
    // 	InitMap();
    //
    // 	GameState.OnTick += Update;
    // }

    public Tile Get(int x, int y) => Tiles[x, y];

    ///<summary>
    ///Simple set function. Set tile at x and y to specific <c>Tile</c>.
    ///There are two other overloads regarding coords and looping over them
    ///</summary>
    protected void Set(int x, int y, Tile tile) => Tiles[x, y] = tile;

    /// <summary>
    /// Overload for looping over tuples, ex. a line across the room.
    /// Looping over tuples can be achieved by <c> GetCanvasCoords.GetVerticalLine() </c> or <c> GetCanvasCoords.GetHorizontalLine() </c> helper functions
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

    public bool InBounds(int x, int y) =>
        x >= 0 && y >= 0 && x < Canvas.Size.Width && y < Canvas.Size.Height;

    public bool IsWalkable(int x, int y) => InBounds(x, y) && Tiles[x, y].Walkable;

    protected void Fill()
    {
        for (int w = 0; w < Canvas.Size.Width; w++)
        {
            for (int h = 0; h < Canvas.Size.Height; h++)
            {
                Set(w, h, Tile.Floor);
            }
        }
    }

    public virtual void InitMap()
    {
        Tiles = new Tile[Canvas.Size.Width, Canvas.Size.Height];
        Fill();
        Set(GetCanvasCoords.GetVerticalLine(0, 0, Canvas.Size.Height), Tile.Wall);
        Set(
            GetCanvasCoords.GetVerticalLine(Canvas.Size.Width - 1, 0, Canvas.Size.Height),
            Tile.Wall
        );
        Set(GetCanvasCoords.GetHorizontalLine(1, 0, Canvas.Size.Width), Tile.Wall);
        Set(
            GetCanvasCoords.GetHorizontalLine(Canvas.Size.Height - 1, 0, Canvas.Size.Width),
            Tile.Wall
        );
    }

    public void RenderToCanvas(ILogger logger)
    {
        for (int w = 0; w < Canvas.Size.Width; w++)
        {
            for (int h = 0; h < Canvas.Size.Height; h++)
            {
                Canvas.Glyph(new Point(w, h), Tiles[w, h].Glyph, Style.Default);
            }
        }
    }

    // public void Update()
    // {
    // 	// RenderToCanvas();
    // }

    public static TileMap GetRoom(RoomTypes type, Canvas canvas, ILogger logger) =>
        type switch
        {
            RoomTypes.Spawn => new SpawnRoom(canvas),
            RoomTypes.Normal => new NormalMap(canvas),
            RoomTypes.Item => new ItemRoom(canvas, logger),
            RoomTypes.Boss => new BossRoom(canvas),
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, "Unsupported room type"),
        };
}
