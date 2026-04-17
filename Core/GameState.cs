using System.Drawing;
using Microsoft.Extensions.Logging;
using Vimonia.Assets;
using Vimonia.Enums;
using Vimonia.Utils;
using Vimonia.World;
using Sharpie;
using System.Text;

namespace Vimonia.Core;

public sealed class GameState(Style playerBody, MapGen floor, ILogger logger, GameSettings settings, Terminal terminal)
{


    public static event EventHandler<Point> PlayerInput;
    public static event EventHandler<GamePhase> CurrentState;
    public static event Action? OnTick; //TODO: Keep or not to keep? That is the question...

    public required TileMap CurrentRoom { get; set; }

    public Point PrevPosition { get; set; }
    public required Canvas Canvas { get; set; }

    public required Canvas MinimapCanvas { get; set; }


    public void Update(Direction? direction)
    {
        Point position = direction switch
        {
            Direction.Down => PrevPosition with
            {
                Y = Math.Clamp(PrevPosition.Y + 1, 0, Canvas.Size.Height - 1),
            },
            Direction.Up => PrevPosition with {
                Y = Math.Clamp(PrevPosition.Y - 1, 0, Canvas.Size.Height - 1),
            },
            Direction.Left => PrevPosition with {
                X = Math.Clamp(PrevPosition.X - 1, 0, Canvas.Size.Width - 1),
            },
            Direction.Right => PrevPosition with {
                X = Math.Clamp(PrevPosition.X + 1, 0, Canvas.Size.Width - 1),
            },
            _ => PrevPosition,
        };

        if (CurrentRoom.Tiles[position.X, position.Y].Glyph == GameConstants.Door) {

            position = EnterNewRoom(position);
        }


        CurrentRoom.RenderToCanvas();
        Canvas.Glyph(position, GameConstants.Player, playerBody); //Update player position
        PrevPosition = position;

		Rune[,] map = CanvasHelpers.RoomsToString(logger, settings, floor.Rooms, CurrentRoom);
		CanvasHelpers.RenderToMap(logger, MinimapCanvas, map, terminal);

        PlayerInput?.Invoke(this, PrevPosition);
    }

    public Point EnterNewRoom(Point position) {

        var (roomX, roomY) = CurrentRoom.GetCoordsInFloor(floor);
        if ((roomX, roomY) == (-1, -1)) {
            throw new Exception("Current room has no coords");
        }

        CurrentRoom = (position.X, position.Y) switch {

            var p when p == GetCanvasCoords.GetCanvasTopCenter(Canvas) => floor.Rooms[roomX, roomY - 1], // North
            var p when p == GetCanvasCoords.GetCanvasBottomCenter(Canvas) => floor.Rooms[roomX, roomY + 1], // South
            var p when p == GetCanvasCoords.GetCanvasLeftCenter(Canvas) => floor.Rooms[roomX - 1, roomY], // West
            var p when p == GetCanvasCoords.GetCanvasRightCenter(Canvas) => floor.Rooms[roomX + 1, roomY], // East
            _ => throw new Exception("Position is somehow not at the door")
        };

        var (newRoomx, newRoomy) = CurrentRoom.GetCoordsInFloor(floor);
        var offset = (newRoomx - roomX, newRoomy - roomY);

        var directionOfNewRoom = Utils.TupleExtensions.ToCardinal(offset);
        position = directionOfNewRoom switch {

            Cardinals.North => new(GetCanvasCoords.GetCanvasBottomCenter(Canvas).Item1, GetCanvasCoords.GetCanvasBottomCenter(Canvas).Item2 - 1), // minus one so it is not on the door
            Cardinals.East => new(GetCanvasCoords.GetCanvasLeftCenter(Canvas).Item1 + 1, GetCanvasCoords.GetCanvasLeftCenter(Canvas).Item2),
            Cardinals.West => new(GetCanvasCoords.GetCanvasRightCenter(Canvas).Item1 - 1, GetCanvasCoords.GetCanvasRightCenter(Canvas).Item2),
            Cardinals.South => new(GetCanvasCoords.GetCanvasTopCenter(Canvas).Item1, GetCanvasCoords.GetCanvasTopCenter(Canvas).Item2 + 1),
            Cardinals.Unknown => throw new Exception("New room hasn't got a direction relative to old room"),
            _ => throw new Exception("directionOfNewRoom is not valid")
        };
        return position;
    }
}
