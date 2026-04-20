using System.Drawing;
using System.Text;
using Sharpie;
using Vimonia.Enums;
using Vimonia.World;

public static class CanvasHelpers {

    public static void RenderToMap(Canvas canvas, Rune[,] Tiles, Terminal terminal) {
        int width = Tiles.GetLength(0);
        int height = Tiles.GetLength(1);

        for (int w = 0; w < width; w++) {
            for (int h = 0; h < height; h++) {
                if (w < canvas.Size.Width && h < canvas.Size.Height) {

                    if (Rune.IsUpper(Tiles[w, h])) {

                        canvas.Glyph(new Point(w, h), Tiles[w, h], new Style() {
                            Attributes = VideoAttribute.Bold,
                            ColorMixture = terminal.Colors.MixColors(StandardColor.Magenta, StandardColor.Black),
                        });
                    } else {

                        canvas.Glyph(new Point(w, h), Tiles[w, h], Style.Default);
                    }

                }
            }
        }
    }

    public static Rune[,] RoomsToString(GameSettings settings, TileMap[,] Rooms, TileMap currentRoom, (int x, int y) maxDiff) //Helper func to see the grid in a clean way
    {
        int midX = Rooms.GetLength(0) / 2;
        int midY = Rooms.GetLength(1) / 2;

        int width = 2 * maxDiff.x + 1; //1 is the middle, maxdiff to both sides hence * 2
        int height = 2 * maxDiff.y + 1;

        Rune[,] map = new Rune[width, height];

        for (int x = midX - maxDiff.x; x <= midX + maxDiff.x; x++) {
            for (int y = midY - maxDiff.y; y <= midY + maxDiff.y; y++) {
                int targetX = x - (midX - maxDiff.x); // Make mid - diff into 0 so map starts at 0,0
                int targetY = y - (midY - maxDiff.y);

                if (x >= 0 && x < Rooms.GetLength(0) && y >= 0 && y < Rooms.GetLength(1) && Rooms[x, y] != null) {
                    char roomType = Rooms[x, y].RoomType switch {
                        RoomTypes.Spawn => 's',
                        RoomTypes.Item => '¤',
                        RoomTypes.Boss => '☠',
                        RoomTypes.Normal => '⛶',
                        _ => ' '
                    };

                    if (Rooms[x, y] == currentRoom) {
                        roomType = '⛑';
                    }

                    map[targetX, targetY] = new Rune(roomType);
                } else {
                    map[targetX, targetY] = new Rune(' ');
                }
            }
        }
        return map;
    }
}

