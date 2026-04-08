using RogueConsole.Enums;
using RogueConsole.Utils;
using Sharpie;

namespace RogueConsole.World.Maps;


public class BossRoom(Canvas canvas) : TileMap(canvas)
{
    public override void InitMap()
    {
        base.InitMap();
        RoomType = RoomTypes.Boss;
        Set(GetCanvasCoords.GetCanvasTopCenter(Canvas).Add(0, 2).Clamp(canvas.Size), Tile.Goblin);
        Set(GetCanvasCoords.GetCanvasBottomCenter(Canvas).Subtract(0, 2).Clamp(canvas.Size), Tile.Goblin);
    }
}
