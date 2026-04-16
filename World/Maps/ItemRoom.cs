using Microsoft.Extensions.Logging;
using Vimonia.Enums;
using Vimonia.Utils;
using Sharpie;

namespace Vimonia.World.Maps;

public class ItemRoom(Canvas canvas) : TileMap(canvas) {
    public override void InitMap() {
        base.InitMap();
        RoomType = RoomTypes.Item;
        Set(GetCanvasCoords.GetCanvasCenter(canvas)
                .Subtract(0, 10)
                .Clamp(canvas.Size),
                Tile.Chest);
    }
}
