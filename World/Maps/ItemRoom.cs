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
        Active = true;
        RoomType = RoomTypes.Item;
        var (c1, c2) = GetCanvasCoords.GetCanvasCenter(canvas);
        Set((c1 + 1, c2 + 1), Tile.Chest);
    }
}
