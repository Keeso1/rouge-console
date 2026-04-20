using System.Drawing;
using Sharpie;
using Vimonia.World;

namespace Vimonia.Utils;

public static class GetCanvasCoords {

    public static (int, int) GetCanvasCenter(Canvas canvas) => (canvas.Size.Width / 2, canvas.Size.Height / 2);
    public static (int, int) GetCanvasTopCenter(Canvas canvas) => (canvas.Size.Width / 2, 0);
    public static (int, int) GetCanvasBottomCenter(Canvas canvas) => (canvas.Size.Width / 2, canvas.Size.Height - 1);
    public static (int, int) GetCanvasTopLeft() => (0, 0);
    public static (int, int) GetCanvasLeftCenter(Canvas canvas) => (0, canvas.Size.Height / 2);
    public static (int, int) GetCanvasRightCenter(Canvas canvas) => (canvas.Size.Width - 1, canvas.Size.Height / 2);
    public static (int, int) GetCanvasTopRight(Canvas canvas) => (canvas.Size.Width - 1, 0);
    public static (int, int) GetCanvasBottomLeft(Canvas canvas) => (0, canvas.Size.Height - 1);
    public static (int, int) GetCanvasBottomRight(Canvas canvas) => (canvas.Size.Width - 1, canvas.Size.Height - 1);

    public static IEnumerable<(int, int)> GetVerticalLine(int setCoord, int from, int to) {
        for (int y = from; y < to; y++) {
            yield return (setCoord, y);
        }
    }

    public static IEnumerable<(int, int)> GetHorizontalLine(int setCoord, int from, int to) {
        for (int y = from; y < to; y++) {
            yield return (y, setCoord);
        }
    }

    public static (int x, int y) GetMaxDimensions(TileMap[,] floorLayout) {
        int maxY = 0;
        int maxX = 0;
        int midX = floorLayout.GetLength(0) / 2;
        int midY = floorLayout.GetLength(1) / 2;
        for (int x = 0; x < floorLayout.GetLength(0); x++) {
            for (int y = 0; y < floorLayout.GetLength(1); y++) {
                if (floorLayout[x, y] != null) {
                    maxX = Math.Max(maxX, Math.Abs(x - midX));
                    maxY = Math.Max(maxY, Math.Abs(y - midY));
                }
            }
        }

        return (maxX, maxY);
    }

}
