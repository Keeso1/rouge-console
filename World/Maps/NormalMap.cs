namespace RogueConsole.World.Maps;

using RogueConsole.Enums;
using RogueConsole.Utils;
using Sharpie;

public class NormalMap(Canvas canvas) : TileMap(canvas)
{
	public override void InitMap()
	{
		base.InitMap();
		RoomType = RoomTypes.Normal;
		Set(GetCanvasCoords.GetCanvasTopCenter(Canvas).Add(0, 2).Clamp(canvas.Size), Tile.Goblin);
		Set(GetCanvasCoords.GetCanvasBottomCenter(Canvas).Subtract(0, 1).Clamp(canvas.Size), Tile.Goblin);
	}

}
