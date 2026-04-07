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
        Set(GetCanvasCoords.GetCanvasCenter(canvas).Add(2, 2), Tile.Chest);
    }
}
