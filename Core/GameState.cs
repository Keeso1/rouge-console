using System.Drawing;
using Microsoft.Extensions.Logging;
using Sharpie;
using Sharpie.Abstractions;

namespace RogueConsole.Core;

public sealed class GameState(ILogger logger, Style playerBody)
{
    public static event EventHandler<GamePhase> CurrentState;
    public static event Action? OnTick;

    public static Point PrevPosition { get; set; }

    public required Canvas Canvas { get; set; }

    public enum Direction
    {
        down,
        up,
        left,
        right,
    }

    public void Update(Direction? direction)
    {
        // if(prevPosition != null)
        // {
        //
        // }
        //
        Point position = direction switch
        {
            Direction.down => PrevPosition with { Y = PrevPosition.Y + 1 },
            Direction.up => PrevPosition with { Y = PrevPosition.Y - 1 },
            Direction.left => PrevPosition with { X = PrevPosition.X - 1 },
            Direction.right => PrevPosition with { X = PrevPosition.X + 1 },
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
