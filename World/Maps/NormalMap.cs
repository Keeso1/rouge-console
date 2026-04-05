namespace RogueConsole.World.Maps;

using RogueConsole.Utils;
using Sharpie;

public class NormalMap(int width, int height, Canvas canvas) : TileMap(width, height, canvas)
{
	protected override void InitMap()
	{
		Fill();
		var (g1, g2) = GetCanvasCoords.GetCanvasTopCenter(Canvas);
		var (g3, g4) = GetCanvasCoords.GetCanvasBottomCenter(Canvas);
		Set((g1, g2 - 1), Tile.Goblin);
		Set((g3, g4 + 1), Tile.Goblin);
		Set(GetCanvasCoords.GetLine(0, 0, Canvas.Size.Height), Tile.Wall);
		Set(GetCanvasCoords.GetLine(Canvas.Size.Width - 1, 0, Canvas.Size.Height), Tile.Wall);
		Set(GetCanvasCoords.GetLine(0, 0, Canvas.Size.Width), Tile.Wall);
		Set(GetCanvasCoords.GetLine(Canvas.Size.Height - 1, 0, Canvas.Size.Width), Tile.Wall);
	}

}


