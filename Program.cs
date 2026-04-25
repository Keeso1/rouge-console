using System.Drawing;
using Vimonia.Core;
using Vimonia.Enums;
using Vimonia.Utils;
using Vimonia.Entities;
using Sharpie;
using Sharpie.Backend;
using Vimonia.Interfaces;
using Sharpie.Abstractions;
using Sharpie.Font;

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
var subWindow = terminal.Screen.Window(new(window.Size.Width + ((window.Size.Width / 4) / 2), ((window.Size.Width / 4) / 2), window.Size.Width / 4, window.Size.Height / 4));

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

Player Player = new(100, 100, new() {
    Attributes = VideoAttribute.Bold,
    ColorMixture = terminal.Colors.MixColors(StandardColor.Magenta, StandardColor.Black),
});

CombatHandler combatHandler = new(Player);
Player.AddSkill(new DeleteSkill());

Canvas headerCanvas = new(terminal.Header.Size);

var game = new GameState(
        headerCanvas,
        Player,
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
// IAsciiFont figFont = await FigletFont.LoadAsync("Assets/fonts/small.flf");

terminal.Repeat(
    t => {

        t.Header.Background = ((new(' '),
        new() {
            Attributes = VideoAttribute.None,
            ColorMixture = terminal.Colors.MixColors((short)StandardColor.Default, 100)
        })
        );

        var currCombo = Player.Combo.Length > 0 ? Player.Combo : "  ";
        var currHealth = Player.Health;



        headerCanvas.Text(new(20, 0), $"Health: {currHealth}/{Player.MaxHealth} ", Canvas.Orientation.Horizontal, Style.Default);
        headerCanvas.Text(new(0, 0), $"Combo: {currCombo}", Canvas.Orientation.Horizontal, Style.Default);
        headerCanvas.DrawOnto(t.Header, new Rectangle(Point.Empty, t.Header.Size), Point.Empty);

        t.Header.Refresh();

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
            case KeyEvent { Char.Value: 'd' }:
                Player.Combo += 'd';
                if (Player.Combo.Length > 1) { // Don't shift enemies on first combo input
                    game.Update(null);
                }
                break;
            case KeyEvent { Char.Value: 'w' }:
                Player.Combo += 'w';
                if (Player.Combo.Length > 1) { // Don't shift enemies on first combo input
                    game.Update(null);
                }
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

