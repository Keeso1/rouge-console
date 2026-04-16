using Vimonia.Utils;
using Sharpie;

namespace Vimonia.World;

public class RockGen {
    private List<(int, int)> _rockSpawns = new();
    private Random _rand = new();
    public required Canvas Canvas { get; set; }

    public (int, int) GetRock() {
        if (_rockSpawns.Count == 0) CreateRockPresets();
        int r = _rand.Next(_rockSpawns.Count);
        return _rockSpawns[r];
    }

    private void CreateRockPresets() {
        for (int i = 0; i < GameSettings.MaxRockPresets; i++) {
            _rockSpawns.Add(GetRandomFromCanvas());
        }
    }

    private (int, int) GetRandomFromCanvas() => (_rand.Next(1, Canvas.Size.Width - 1), _rand.Next(1, Canvas.Size.Height - 1));
}
