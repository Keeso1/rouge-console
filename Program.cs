using System.Drawing;
using Sharpie;
using Sharpie.Abstractions;
using Sharpie.Backend;

var terminal = new Terminal(
    CursesBackend.Load(),
    new TerminalOptions(UseColors: true, CaretMode: CaretMode.Invisible, UseMouse: false)
);

terminal.Repeat(
    t =>
    {
        t.Screen.Refresh();
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
                Term.Screen.DrawCell(prevPosition, new System.Text.Rune(' '), new Style());
                Term.Screen.DrawCell(position, new System.Text.Rune('h'), new Style());
                prevPosition = position;
                break;
            case KeyEvent { Char.Value: 'j' }:
                position = prevPosition with { Y = prevPosition.Y + 1 };
                Term.Screen.DrawCell(prevPosition, new System.Text.Rune(' '), new Style());
                Term.Screen.DrawCell(position, new System.Text.Rune('h'), new Style());
                prevPosition = position;
                break;
            case KeyEvent { Char.Value: 'k' }:
                position = prevPosition with { Y = prevPosition.Y - 1 };
                Term.Screen.DrawCell(prevPosition, new System.Text.Rune(' '), new Style());
                Term.Screen.DrawCell(position, new System.Text.Rune('h'), new Style());
                prevPosition = position;
                break;
            case KeyEvent { Char.Value: 'l' }:
                position = prevPosition with { X = prevPosition.X + 1 };
                Term.Screen.DrawCell(prevPosition, new System.Text.Rune(' '), new Style());
                Term.Screen.DrawCell(position, new System.Text.Rune('h'), new Style());
                prevPosition = position;
                break;
        }
        ;
        return Task.FromResult(true);
    }
);
