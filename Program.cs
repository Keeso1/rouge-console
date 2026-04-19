using System.Drawing;
using Vimonia.Core;
using Vimonia.Enums;
using Vimonia.Utils;
using Sharpie;
using Sharpie.Backend;

string logFilePath = "console_log.txt";
Log.Init(logFilePath);

GameSettings settings = new() { NumberOfRooms = 7 }; //grid size 16x16, no way to overshoot index

var terminal = new Terminal(
    CursesBackend.Load(),
    new TerminalOptions(UseColors: true, CaretMode: CaretMode.Invisible, UseMouse: false, ManagedWindows: true)
);


var window = terminal.Screen.Window(new(1, 1, terminal.Screen.Size.Width - 2, (terminal.Screen.Size.Height - 2) / 2));

//MINIMAP TESTING
var subWindow = terminal.Screen.Window(new(1, 1, window.Size.Width / 4, window.Size.Height / 2));

window.Background = (new(' '),
    new() {
        Attributes = VideoAttribute.None,
        ColorMixture = terminal.Colors.MixColors((short)StandardColor.Default, 100)
    });

subWindow.Background = (new(' '),
    new() {
        Attributes = VideoAttribute.None,
        ColorMixture = terminal.Colors.MixColors((short)StandardColor.Default, 60)
    });
//

CanvasWrapper.Init(window.Size);

Canvas minimapCanvas = new(subWindow.Size);

Rng.Init(CanvasWrapper.Instance, 4);

MapGen floor = new(CanvasWrapper.Instance, settings);

var game = new GameState(
    new() {
        Attributes = VideoAttribute.Bold,
        ColorMixture = terminal.Colors.MixColors(StandardColor.Magenta, StandardColor.Black),
    },
    floor,
    settings,
    terminal
) {
    Canvas = CanvasWrapper.Instance,
    PrevPosition = new(CanvasWrapper.Instance.Size.Width / 2, CanvasWrapper.Instance.Size.Height / 2),
    CurrentRoom = floor.Rooms[settings.NumberOfRooms + 1, settings.NumberOfRooms + 1],
    MinimapCanvas = minimapCanvas
};



game.Update(null);

terminal.Repeat(
    t => {
        game.Canvas.DrawOnto(
            window,
            new Rectangle(new Point(1, 1), CanvasWrapper.Instance.Size),
            new Point(0, 0)
        );

        // MINIMAP
        game.MinimapCanvas.DrawOnto(
            subWindow,
            new Rectangle(new Point(0, 0), minimapCanvas.Size),
            new Point(0, 0)
        );

        t.Screen.Refresh();
        // t.Screen.DrawBorder();
        return Task.CompletedTask;
    },
    50
);

terminal.Run(
    (Term, Tevent) => {
        switch (Tevent) {
            case KeyEvent { Char.Value: 'q' }:
                Log.Shutdown();
                Environment.Exit(0);
                break;
            case KeyEvent { Char.Value: 'h' }:
                game.Update(Direction.Left);
                break;
            case KeyEvent { Char.Value: 'j' }:
                game.Update(Direction.Down);
                break;
            case KeyEvent { Char.Value: 'k' }:
                game.Update(Direction.Up);
                break;
            case KeyEvent { Char.Value: 'l' }:
                game.Update(Direction.Right);
                break;
            case KeyEvent { Char.Value: 'm' }:
                subWindow.Visible = !subWindow.Visible; //Toggle window
                if (subWindow.Visible) {
                    window.SendToBack();
                    subWindow.BringToFront();
                }
                break;
        }
        ;
        return Task.FromResult(true);
    }
);

