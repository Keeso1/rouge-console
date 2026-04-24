using System.Drawing;
using Microsoft.Extensions.Logging;
using Vimonia.Assets;
using Vimonia.Enums;
using Vimonia.Utils;
using Vimonia.World;
using Sharpie;
using System.Text;
using Vimonia.World.Maps;
using Vimonia.Entities;
using Vimonia.Interfaces;

namespace Vimonia.Core;

public sealed class GameState(Player player, MapGen floor, GameSettings settings, Terminal terminal) {
    public static event EventHandler<GamePhase> CurrentState;
    public static event EventHandler<Point> PlayerInput;
    public static event Action? OnTick; //TODO: Keep or not to keep? That is the question...

    private TileMap _currentRoom;
    public required TileMap CurrentRoom {
        get => _currentRoom;
        set {
            _currentRoom?.Deactivate();
            _currentRoom = value;
            _currentRoom?.Activate();
            CombatHandler.Instance.Init(_currentRoom);
        }
    }

    public Point PrevPosition { get; set; }

    public required Canvas Canvas { get; set; }
    public required Canvas MinimapCanvas { get; set; }


    public void Update(Direction? direction) {

        Point position = Controls.Move(direction, PrevPosition, CurrentRoom);

        if (CurrentRoom.Tiles[position.X, position.Y].Glyph == GameConstants.Door) {

            position = EnterNewRoom(position);
        }

        if (CurrentRoom.Tiles[position.X, position.Y].Entity != null) {
            player.TakeDamage(10);
            Log.Info($"Health: {player.Health}");
        }

        CurrentRoom.RenderToCanvas();
        Canvas.Glyph(position, GameConstants.Player, player.Style); //Update player position
        PrevPosition = position;
        PlayerInput?.Invoke(this, PrevPosition);

        if (player.Combo == "dw") {
            player.UseSkill("dw");
            player.Combo = "";
        }

        if (player.Combo.Length > 2) {
            player.Combo = "";
        }

        Rune[,] map = CanvasHelpers.RoomsToString(settings, floor.Rooms, CurrentRoom, GetCanvasCoords.GetMaxDimensions(floor.Rooms));
        CanvasHelpers.RenderToMap(MinimapCanvas, map, terminal);
        OnTick?.Invoke();
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

        var directionOfNewRoom = Utils.TupleExtensions.ToCardinal(offset); position = directionOfNewRoom switch {

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
