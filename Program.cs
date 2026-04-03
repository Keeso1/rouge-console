using System.Drawing;
using System.Text;
using RogueConsole.Assets;
using Sharpie;
using Sharpie.Abstractions;
using Sharpie.Backend;

var terminal = new Terminal(
    CursesBackend.Load(),
    new TerminalOptions(UseColors: true, CaretMode: CaretMode.Invisible, UseMouse: false)
);

Canvas game = new(terminal.Screen.Size);

terminal.Repeat(
    t =>
    {
        game.DrawOnto(t.Screen, new Rectangle(new Point(0, 0), game.Size), new Point(0, 0));
        t.Screen.Refresh();
        t.Screen.DrawBorder();
        return Task.CompletedTask;
    },
    100
);

var prevPosition = new Point(terminal.Screen.Size.Width / 2, terminal.Screen.Size.Height / 2);
var position = new Point(terminal.Screen.Size.Width / 2, terminal.Screen.Size.Height / 2);

terminal.Run(
    (Term, Tevent) =>
    {
        switch (Tevent)
        {
            case KeyEvent { Char.Value: 'q' }:
                Environment.Exit(0);
                break;
            case KeyEvent { Char.Value: 'h' }:
                position = prevPosition with { X = prevPosition.X - 1 };
                game.Glyph(prevPosition, Assets.Space, Style.Default);
                game.Glyph(position, Assets.Player, Style.Default);
                prevPosition = position;
                break;

            case KeyEvent { Char.Value: 'j' }:
                position = prevPosition with { Y = prevPosition.Y + 1 };
                game.Glyph(prevPosition, Assets.Space, Style.Default);
                game.Glyph(position, Assets.Player, Style.Default);
                prevPosition = position;
                break;
            case KeyEvent { Char.Value: 'k' }:
                position = prevPosition with { Y = prevPosition.Y - 1 };
                game.Glyph(prevPosition, Assets.Space, Style.Default);
                game.Glyph(position, Assets.Player, Style.Default);
                prevPosition = position;
                break;
            case KeyEvent { Char.Value: 'l' }:
                position = prevPosition with { X = prevPosition.X + 1 };
                game.Glyph(prevPosition, Assets.Space, Style.Default);
                game.Glyph(position, Assets.Player, Style.Default);
                prevPosition = position;
                break;
        }
        ;
        return Task.FromResult(true);
    }
);
