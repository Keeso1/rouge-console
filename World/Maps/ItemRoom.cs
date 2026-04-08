using Microsoft.Extensions.Logging;
using RogueConsole.Enums;
using RogueConsole.Utils;
using Sharpie;

namespace RogueConsole.World.Maps;

public class ItemRoom(Canvas canvas, ILogger logger) : TileMap(canvas)
{
    public override void InitMap()
    {
        base.InitMap();
        RoomType = RoomTypes.Item;
        Set(GetCanvasCoords.GetCanvasCenter(canvas)
                .Subtract(0, 10)
                .Clamp(canvas.Size),
                Tile.Chest);
    }
}
