using Vimonia.Enums;
using Sharpie;

namespace Vimonia.World.Maps;

public class SpawnRoom(Canvas canvas) : TileMap(canvas)
{
    public override void InitMap()
    {
        RoomType = RoomTypes.Spawn;
        base.InitMap();
    }
}
