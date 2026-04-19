using System.Drawing;
using Sharpie;

public static class CanvasWrapper{

    private static bool _initialized;
    private static Canvas _instance;

    public static Canvas Instance{get{
        if(!_initialized){
            throw new Exception("Canvas not initialized");
        }
        return _instance;
    }}

    public static void Init(Size size) {
        _instance = new Canvas(size);
        _initialized = true;
    }

}
