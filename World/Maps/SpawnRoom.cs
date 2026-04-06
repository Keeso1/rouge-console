
using RogueConsole.Utils;
using RogueConsole.Enums;
using Sharpie;

namespace RogueConsole.World.Maps;

public class SpawnRoom(Canvas canvas) : TileMap(canvas)
{
	public override void InitMap()
	{
		Active = true;
		RoomType = RoomTypes.Spawn;
		base.InitMap();
	}
}
