using System.Drawing;
using Microsoft.Extensions.Logging;
using Sharpie;
using Sharpie.Abstractions;

namespace RogueConsole.Core;

public sealed class GameState(ILogger logger, Style playerBody)
{
    public static event EventHandler<GamePhase> CurrentState;
    public static event Action? OnTick;

    public Point PrevPosition { get; set; }

    public required Canvas Canvas { get; set; }

    public enum Direction
    {
        down,
        up,
        left,
        right,
    }

    // public bool InBounds(Point pos)
    // {
    //     bool isInboundsX = pos.X > 0 && pos.X + 1 < Canvas.Size.Width;
    //     bool isInboundsY = pos.Y > 0 && pos.Y + 1 < Canvas.Size.Height;
    //     return isInboundsX && isInboundsY;
    // }

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

public enum GamePhase
{
    Running,
    GameOver,
    Victory,
}
