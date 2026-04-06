using RogueConsole.Utils;
using RogueConsole.Enums;
using Sharpie;
using Microsoft.Extensions.Logging;

namespace RogueConsole.World.Maps;

public class ItemRoom(Canvas canvas, ILogger logger) : TileMap(canvas)
{
	public override void InitMap()
	{
		base.InitMap();
		Active = true;
		RoomType = RoomTypes.Item;
		var (c1, c2) = GetCanvasCoords.GetCanvasTopLeft();
		Set((c1 + 1, c1 + 1), Tile.Chest);
	}
}
