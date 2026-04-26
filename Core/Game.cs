
using Vimonia.Core;
using Vimonia.Utils;
using Vimonia.Entities;
using Sharpie;
using Vimonia.Enums;

public class Game {

    private Terminal _terminal { get; set; }
    private GameSettings _gameSettings { get; set; }
    public CombatHandler CombatHandler { get; set; }
    public Player Player { get; set; }
    public MapGen FloorLayout { get; set; }
    public GameState GameState { get; set; }
    public int Seed { get; private set; }

    public Game(Terminal terminal, GameSettings gameSettings, int? seed = null) {
        _terminal = terminal;
        _gameSettings = gameSettings;
        Seed = seed ?? Random.Shared.Next();

        Rng.Init(CanvasWrapper.Instance, Seed);

        FloorLayout = new(CanvasWrapper.Instance, _gameSettings);

        Player = new(100, 100, new() {
            Attributes = VideoAttribute.Bold,
            ColorMixture = _terminal.Colors.MixColors(StandardColor.Magenta, StandardColor.Black),
        });

        CombatHandler = new(Player);
        Player.AddSkill(new DeleteSkill());

        GameState = new GameState(
                Player,
            FloorLayout,
            gameSettings,
            terminal
        ) {
            Canvas = CanvasWrapper.Instance,
            PrevPosition = new(CanvasWrapper.Instance.Size.Width / 2, CanvasWrapper.Instance.Size.Height / 2),
            CurrentRoom = FloorLayout.Rooms[gameSettings.NumberOfRooms + 1, gameSettings.NumberOfRooms + 1],
        };
    }


    public void InputHandler(Event e) {
        switch (e) {

            case KeyEvent { Char.Value: 'q' }:
                Log.Shutdown();
                Environment.Exit(0);
                break;

            case KeyEvent { Char.Value: 'h' }:
                GameState.Update(Direction.Left);
                break;

            case KeyEvent { Char.Value: 'j' }:
                GameState.Update(Direction.Down);
                break;

            case KeyEvent { Char.Value: 'k' }:
                GameState.Update(Direction.Up);
                break;

            case KeyEvent { Char.Value: 'l' }:
                GameState.Update(Direction.Right);
                break;

            case KeyEvent { Char.Value: 'd' }:
                Player.Combo += 'd';
                if (Player.Combo.Length > 1) { // Don't shift enemies on first combo input
                    GameState.Update(null);
                }
                break;
            case KeyEvent { Char.Value: 'w' }:
                Player.Combo += 'w';
                if (Player.Combo.Length > 1) { // Don't shift enemies on first combo input
                    GameState.Update(null);
                }
                break;
                // case KeyEvent { Char.Value: 'm' }:
                //     subWindow.Visible = !subWindow.Visible; //Toggle window
                //     if (subWindow.Visible) {
                //         window.SendToBack();
                //         subWindow.BringToFront();
                //     }
                //     break;
        }

    }

}
