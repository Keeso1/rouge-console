using Vimonia.Enums;
using Sharpie;

using Vimonia.Core;

namespace Vimonia.World.Maps;


public class BossRoom(Canvas canvas) : EnemyRoom(canvas) {
    public override void InitMap() {
        base.InitMap();
        RoomType = RoomTypes.Boss;
    }
}
