using Sharpie;

namespace Vimonia.Core;

public static class Rng {
    private static Random _rand = null!;
    private static Canvas _canvas = null!;
    public static int Seed { get; private set; }

    public static void Init(Canvas canvas, int? seed = null) {

        _canvas = canvas;
        Seed = seed ?? Random.Shared.Next();
        _rand = new Random(Seed);
    }

    public static (int, int) GetRandomFromCanvas() => (_rand.Next(1, _canvas.Size.Width - 1), _rand.Next(1, _canvas.Size.Height - 1));
}
