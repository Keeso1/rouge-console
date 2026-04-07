namespace RogueConsole.World.Maps;

using RogueConsole.Enums;
using RogueConsole.Utils;
using Sharpie;

public class BossRoom(Canvas canvas) : TileMap(canvas)
{
    public override void InitMap()
    {
        base.InitMap();
        RoomType = RoomTypes.Boss;
        var (g1, g2) = GetCanvasCoords.GetCanvasTopCenter(Canvas);
        var (g3, g4) = GetCanvasCoords.GetCanvasBottomCenter(Canvas);
        Set((g1, g2 + 2), Tile.Goblin);
        Set((g3, g4 - 2), Tile.Goblin);
    }
}
