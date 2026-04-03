using RogueConsole.World;
using RogueConsole.Core;
using System.Drawing;
using Sharpie;
using Sharpie.Backend;

internal class Program
{
    private static void Main(string[] args)
    {
        var backend = CursesBackend.Load();
        using var terminal = new Terminal(
            backend,
            new TerminalOptions(UseMouse: false, SuppressControlKeys: false)
        );

        terminal.Screen.Refresh();

        Canvas canvas = new Canvas(terminal.Screen.Size);
        TileMap map = new(terminal.Screen.Size.Width, terminal.Screen.Size.Height, canvas);
        GameState state = new();
        InputHandler input = new();

        foreach (var @event in terminal.Events.Listen(terminal.Screen))
        {

            state.Update();
            // terminal.Screen.WriteText($"{@event}\n");
            RenderScreen();
            if (@event is KeyEvent { Char.Value: 'c' })
            {
                break;
            }

            if (@event is StartEvent)
            {
            }
        }

        void RenderScreen()
        {
            map.Render();
            canvas.DrawOnto(terminal.Screen, new Rectangle(new Point(0, 0), canvas.Size), new Point(0, 0));
            terminal.Screen.DrawBorder();
            terminal.Screen.Refresh();
        }
    }


}
