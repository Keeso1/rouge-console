using System.Drawing;
using System.Text;
using Microsoft.Extensions.Logging;
using Sharpie;
using Vimonia.Enums;
using Vimonia.World;

public static class CanvasHelpers {



    public static void RenderToMap(ILogger logger, Canvas canvas, Rune[,] Tiles, Terminal terminal) {
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

    public static Rune[,] RoomsToString(ILogger logger, GameSettings settings, TileMap[,] Rooms, TileMap currentRoom) //Helper func to see the grid in a clean way
    {
        Rune[,] map = new Rune[Rooms.GetLength(0), Rooms.GetLength(1)];
        for (int y = 0; y < Rooms.GetLength(1); y++) {
            for (int x = 0; x < Rooms.GetLength(0); x++) {
                if (Rooms[x, y] != null) {
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

                    map[x, y] = new Rune(roomType); //dafuq
                } else {
                    map[x, y] = new Rune(' ');
                }
            }
        }
        return map;
    }
}

