using Vimonia.Enums;
using Sharpie;

using Microsoft.Extensions.Logging;
using Vimonia.Core;

namespace Vimonia.World.Maps;


public class BossRoom(Canvas canvas, ILogger logger) : EnemyRoom(canvas, logger) {
    public override void InitMap() {
        base.InitMap();
        RoomType = RoomTypes.Boss;
    }
}
