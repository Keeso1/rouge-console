using System.Drawing;
using Microsoft.Extensions.Logging;
using RogueConsole.Assets;
using RogueConsole.Enums;
using Sharpie;

namespace RogueConsole.Core;

public sealed class GameState(Style playerBody, MapGen mapGen)
{
    public static event EventHandler<GamePhase> CurrentState;
    public static event Action? OnTick;

    public static Point PrevPosition { get; set; }
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

        mapGen.Rooms[8, 8].RenderToCanvas();
        Canvas.Glyph(position, GameConstants.Player, playerBody);
        PrevPosition = position;
    }
}
