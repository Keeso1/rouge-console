using System.Drawing;
using Sharpie;

public static class MinimapWrapper {

    private static bool _initialized;
    private static Canvas _instance;

    public static Canvas Instance {
        get {
            if (!_initialized) {
                throw new Exception("Canvas not initialized");
            }
            return _instance;
        }
    }

    public static void Init(Size size) {
        _instance = new Canvas(size);
        _initialized = true;
    }

    public static HashSet<Point> AllCanvasPoints() {
        HashSet<Point> accumulated = [];
        for (int x = 0; x < Instance.Size.Width; x++) {
            for (int y = 0; y < Instance.Size.Height; y++) {
                accumulated.Add(new(x, y));
            }
        }
        return accumulated;
    }

}
