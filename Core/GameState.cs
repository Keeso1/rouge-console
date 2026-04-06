using System.Drawing;
using Microsoft.Extensions.Logging;
using RogueConsole.Enums;
using Sharpie;
using Sharpie.Abstractions;

namespace RogueConsole.Core;

public sealed class GameState(ILogger logger, Style playerBody, FloorLayout floorLayout)
{
    public static event EventHandler<GamePhase> CurrentState;
    public static event Action? OnTick;

    public static Point PrevPosition { get; set; }
    public required Canvas Canvas { get; set; }

    public void Update(Direction? direction)
    {
        Point position = direction switch
        {
            Direction.down => PrevPosition with
            {
                Y = Math.Clamp(PrevPosition.Y + 1, 0, Canvas.Size.Height - 1),
            },
            Direction.up => PrevPosition with
            {
                Y = Math.Clamp(PrevPosition.Y - 1, 0, Canvas.Size.Height - 1),
            },
            Direction.left => PrevPosition with
            {
                X = Math.Clamp(PrevPosition.X - 1, 0, Canvas.Size.Width - 1),
            },
            Direction.right => PrevPosition with
            {
                X = Math.Clamp(PrevPosition.X + 1, 0, Canvas.Size.Width - 1),
            },
            _ => PrevPosition,
        };

        Canvas.Glyph(PrevPosition, Assets.Assets.Space, Style.Default);
        Canvas.Glyph(position, Assets.Assets.Player, playerBody);
        PrevPosition = position;
    }
}
