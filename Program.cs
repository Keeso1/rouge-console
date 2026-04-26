using System.Drawing;
using Vimonia.Utils;
using Sharpie;
using Sharpie.Backend;

string logFilePath = "console_log.txt";
Log.Init(logFilePath);

GameSettings settings = new() { NumberOfRooms = 7 }; //grid size 16x16, no way to overshoot index

var terminal = new Terminal(
    CursesBackend.Load(),
    new TerminalOptions(UseColors: true, CaretMode: CaretMode.Invisible, UseMouse: false, ManagedWindows: true, AllocateHeader: true) { AllocateHeader = true }
);

var availableWidth = terminal.Screen.Size.Width - 2;
var availableHeight = terminal.Screen.Size.Height - 2;
var windowWidth = Math.Min(availableWidth, availableHeight * 2);
var windowHeight = windowWidth / 2;
var window = terminal.Screen.Window(new(1, 1, windowWidth, windowHeight));

//MINIMAP TESTING
var safeX = Math.Min(0 + windowWidth, terminal.Screen.Size.Width - (windowWidth / 4));
var subWindow = terminal.Screen.Window(new(safeX, ((windowWidth / 4) / 2), windowWidth / 4, windowHeight / 4));

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
MinimapWrapper.Init(subWindow.Size);
HeaderWrapper.Init(terminal.Header.Size);

Game Game = new(terminal, settings, 4);

// IAsciiFont figFont = await FigletFont.LoadAsync("Assets/fonts/small.flf");

terminal.Repeat(
    t => {

        t.Header.Background = ((new(' '),
        new() {
            Attributes = VideoAttribute.None,
            ColorMixture = terminal.Colors.MixColors((short)StandardColor.Default, 100)
        })
        );

        var currCombo = Game.Player.Combo.Length > 0 ? Game.Player.Combo : "  ";
        var currHealth = Game.Player.Health;

        HeaderWrapper.Instance.Text(new(20, 0), $"Health: {currHealth}/{Game.Player.MaxHealth} ", Canvas.Orientation.Horizontal, Style.Default);
        HeaderWrapper.Instance.Text(new(0, 0), $"Combo: {currCombo}", Canvas.Orientation.Horizontal, Style.Default);
        HeaderWrapper.Instance.DrawOnto(t.Header, new Rectangle(Point.Empty, t.Header.Size), Point.Empty);

        t.Header.Refresh();

        CanvasWrapper.Instance.DrawOnto(
            window,
            new Rectangle(new Point(1, 1), CanvasWrapper.Instance.Size),
            new Point(0, 0)
        );

        // MINIMAP
        MinimapWrapper.Instance.DrawOnto(
            subWindow,
            new Rectangle(new Point(0, 0), MinimapWrapper.Instance.Size),
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
        Game.InputHandler(Tevent);
        return Task.FromResult(true);
    }
);

