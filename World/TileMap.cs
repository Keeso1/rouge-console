using System.Drawing;
using Vimonia.Core;
using Vimonia.Enums;
using Vimonia.Utils;
using Vimonia.World.Maps;
using Sharpie;

namespace Vimonia.World;

public class TileMap {
    public Tile[,] Tiles { get; private set; } = null!;
    public Canvas Canvas { get; init; }
    public RoomTypes RoomType { get; set; }
    public List<Cardinals> Neighbors { get; set; } = [];

    public TileMap(Canvas canvas) {
        Canvas = canvas;
    }

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
    protected void Set(IEnumerable<(int x, int y)> coords, Tile tile) {
        foreach (var coord in coords)
            Tiles[coord.x, coord.y] = tile;
    }

    ///<summary>
    ///Overload to be able to accept tuples using <c> GetCanvasCoords </c> helper class
    ///</summary>
    protected void Set((int x, int y) coord, Tile tile) => Tiles[coord.x, coord.y] = tile;

    protected void Set(Point coord, Tile tile) => Tiles[coord.X, coord.Y] = tile;

    protected void UnSet(Point coord) => Tiles[coord.X, coord.Y] = Tile.Floor();

    public bool IsWalkable(int x, int y) => (x, y).InBounds(Canvas.Size) && Tiles[x, y].Walkable;

    protected void Fill() {
        for (int w = 0; w < Canvas.Size.Width; w++) {
            for (int h = 0; h < Canvas.Size.Height; h++) {
                Set(w, h, Tile.Floor());
            }
        }
    }

    protected IEnumerable<(int, int)> GenRocks() {
        for (int i = 0; i < GameSettings.MaximumRocks; i++) {
            yield return Rng.GetRandomFromCanvas();
        }
    }

    public void RenderDoors() {
        foreach (Cardinals neighbor in Neighbors) {
            switch (neighbor) {
                case Cardinals.North:
                    Set(GetCanvasCoords.GetCanvasTopCenter(Canvas), Tile.Door());
                    break;
                case Cardinals.East:
                    Set(GetCanvasCoords.GetCanvasRightCenter(Canvas), Tile.Door());
                    break;
                case Cardinals.West:
                    Set(GetCanvasCoords.GetCanvasLeftCenter(Canvas), Tile.Door());
                    break;
                case Cardinals.South:
                    Set(GetCanvasCoords.GetCanvasBottomCenter(Canvas), Tile.Door());
                    break;
                case Cardinals.Unknown:
                    continue;
                default:
                    throw new Exception("Neighbor must have a cardinal direction");
            }
        }
    }

    public virtual void InitMap() {
        Tiles = new Tile[Canvas.Size.Width, Canvas.Size.Height];
        Fill();
        Set(GenRocks(), Tile.Rock());
        Set(GetCanvasCoords.GetVerticalLine(0, 0, Canvas.Size.Height), Tile.Wall());
        Set(
            GetCanvasCoords.GetVerticalLine(Canvas.Size.Width - 1, 0, Canvas.Size.Height),
            Tile.Wall()
        );
        Set(GetCanvasCoords.GetHorizontalLine(0, 0, Canvas.Size.Width), Tile.Wall());
        Set(
            GetCanvasCoords.GetHorizontalLine(Canvas.Size.Height - 1, 0, Canvas.Size.Width),
            Tile.Wall()
        );

        if (Neighbors != null) {
            RenderDoors();
        }
    }

    public void RenderToCanvas() {
        for (int h = 0; h < Canvas.Size.Height; h++) {
            var text = "";
            for (int w = 0; w < Canvas.Size.Width; w++) {
                if (text.Length > 0) {
                    Canvas.Text(new Point(w, h), text[0].ToString(), Canvas.Orientation.Horizontal, Style.Default);
                    text = text[1..];
                    continue;
                }

                if (Tiles[w, h].Entity != null) {
                    text = Tiles[w, h].Text;
                    Canvas.Text(new Point(w, h), text[0].ToString(), Canvas.Orientation.Horizontal, Style.Default);
                    text = text[1..];
                } else {
                    Canvas.Glyph(new Point(w, h), Tiles[w, h].Glyph, Style.Default);
                }
            }
        }
    }

    public static TileMap GetRoom(RoomTypes type, Canvas canvas) =>
        type switch {
            RoomTypes.Spawn => new SpawnRoom(canvas),
            RoomTypes.Normal => new EnemyRoom(canvas),
            RoomTypes.Item => new ItemRoom(canvas),
            RoomTypes.Boss => new BossRoom(canvas),
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, "Unsupported room type"),
        };

    public (int x, int y) GetCoordsInFloor(MapGen floor) {
        (int X, int Y) coord = (-1, -1); //TODO: Fix hardcoded value
        for (int x = 0; x < floor.Rooms.GetLength(0); x++) {
            for (int y = 0; y < floor.Rooms.GetLength(1); y++) {
                if (floor.Rooms[x, y] == this) {
                    coord = (x, y);
                    break;
                }
            }
        }
        return coord;
    }
}
