using RogueConsole.Enums;
using Sharpie;

namespace RogueConsole.World.Maps;

public class SpawnRoom(Canvas canvas) : TileMap(canvas)
{
    public override void InitMap()
    {
        RoomType = RoomTypes.Spawn;
        base.InitMap();
    }
}
