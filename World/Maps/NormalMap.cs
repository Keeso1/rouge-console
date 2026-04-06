namespace RogueConsole.World.Maps;

using RogueConsole.Utils;
using RogueConsole.Enums;
using Sharpie;

public class NormalMap(Canvas canvas) : TileMap(canvas)
{
	public override void InitMap()
	{
		Active = true;
		base.InitMap();
		RoomType = RoomTypes.Normal;
		var (g1, g2) = GetCanvasCoords.GetCanvasTopCenter(Canvas);
		var (g3, g4) = GetCanvasCoords.GetCanvasBottomCenter(Canvas);
		Set((g1, g2 + 2), Tile.Goblin);
		Set((g3, g4 - 2), Tile.Goblin);
	}

}


