using System.Drawing;
using Microsoft.Extensions.Logging;
using Vimonia.Assets;
using Vimonia.Enums;
using Vimonia.Utils;
using Vimonia.World;
using Sharpie;

namespace Vimonia.Core;

public sealed class GameState(Style playerBody, MapGen floor)
{
    public static event EventHandler<GamePhase> CurrentState;
    public static event Action? OnTick;

	public required TileMap CurrentRoom {get; set;}

    public Point PrevPosition { get; set; }
    public required Canvas Canvas { get; set; }

    public void Update(Direction? direction)
    {
        Point position = direction switch
        {
            Direction.Down => PrevPosition with
            {
                Y = Math.Clamp(PrevPosition.Y + 1, 0, Canvas.Size.Height - 1),
            },
            Direction.Up => PrevPosition with
            {
                Y = Math.Clamp(PrevPosition.Y - 1, 0, Canvas.Size.Height - 1),
            },
            Direction.Left => PrevPosition with
            {
                X = Math.Clamp(PrevPosition.X - 1, 0, Canvas.Size.Width - 1),
            },
            Direction.Right => PrevPosition with
            {
                X = Math.Clamp(PrevPosition.X + 1, 0, Canvas.Size.Width - 1),
            },
            _ => PrevPosition,
        };
		
		if (CurrentRoom.Tiles[position.X, position.Y] == Tile.Door)
		{
			var (roomX, roomY) = CurrentRoom.GetCoordsInFloor(floor);
			if ((roomX, roomY) == (-1, -1))
			{
				throw new Exception("Current room has no coords");
			}
	   
			CurrentRoom = (position.X, position.Y) switch
			{
				var p when p == GetCanvasCoords.GetCanvasTopCenter(Canvas)    => floor.Rooms[roomX, roomY - 1], // North
				var p when p == GetCanvasCoords.GetCanvasBottomCenter(Canvas) => floor.Rooms[roomX, roomY + 1], // South
				var p when p == GetCanvasCoords.GetCanvasLeftCenter(Canvas)   => floor.Rooms[roomX - 1, roomY], // West
				var p when p == GetCanvasCoords.GetCanvasRightCenter(Canvas)  => floor.Rooms[roomX + 1, roomY], // East
				_ => throw new Exception("Position is somehow not at the door")
			};
		};

        CurrentRoom.RenderToCanvas();
        Canvas.Glyph(position, GameConstants.Player, playerBody);
        PrevPosition = position;
    }
}
