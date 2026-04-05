namespace RogueConsole.World.Maps;

using RogueConsole.Utils;
using Sharpie;

public class NormalMap(int width, int height, Canvas canvas) : TileMap(width, height, canvas)
{
	protected override void InitMap()
	{
		Fill();
		Set(GetCanvasCoords.GetVerticalLine(0, 0, Canvas.Size.Height), Tile.Wall);
		Set(GetCanvasCoords.GetVerticalLine(Canvas.Size.Width - 1, 0, Canvas.Size.Height), Tile.Wall);
		Set(GetCanvasCoords.GetHorizontalLine(1, 0, Canvas.Size.Width), Tile.Wall);
		Set(GetCanvasCoords.GetHorizontalLine(Canvas.Size.Height - 1, 0, Canvas.Size.Width), Tile.Wall);
		var (g1, g2) = GetCanvasCoords.GetCanvasTopCenter(Canvas);
		var (g3, g4) = GetCanvasCoords.GetCanvasBottomCenter(Canvas);
		Set((g1, g2 + 2), Tile.Goblin);
		Set((g3, g4 - 2), Tile.Goblin);
	}

}


