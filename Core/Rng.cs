using Sharpie;

namespace Vimonia.Core;

public static class Rng {
    private static Random _rand = null!;
    public static Canvas _canvas = null!;
    private static bool _initialized;
    private static int _seed { get; set; }


    public static void Init(Canvas canvas, int seed) {
        _canvas = canvas;
        _seed = seed;
        _rand = new Random(_seed);
        _initialized = true;
    }

    private static void CheckInit() {
        if (!_initialized) {
            throw new InvalidOperationException($"Rng not initialized");
        }
    }

    public static Random GetRandom() {
        CheckInit();
        return _rand;
    }

    public static (int, int) GetRandomFromCanvas() {
        CheckInit();
        return (_rand.Next(1, _canvas.Size.Width - 1), _rand.Next(1, _canvas.Size.Height - 1));
    }
}
