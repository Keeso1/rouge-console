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
		Set(GetCanvasCoords.GetCanvasTopCenter(Canvas).Add(0, 2), Tile.Goblin);
		Set(GetCanvasCoords.GetCanvasBottomCenter(Canvas).Subtract(0, 1), Tile.Goblin);
	}

}


