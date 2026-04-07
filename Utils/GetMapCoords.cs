namespace RogueConsole.Utils;

using Sharpie;

public static class GetCanvasCoords
{
	public static (int, int) GetCanvasCenter(Canvas canvas) => (canvas.Size.Width / 2, canvas.Size.Height / 2);
	public static (int, int) GetCanvasTopCenter(Canvas canvas) => (canvas.Size.Width / 2, 0);
	public static (int, int) GetCanvasBottomCenter(Canvas canvas) => (canvas.Size.Width / 2, canvas.Size.Height - 1);
	public static (int, int) GetCanvasTopLeft() => (0, 0);
	public static (int, int) GetCanvasTopRight(Canvas canvas) => (canvas.Size.Width - 1, 0);
	public static (int, int) GetCanvasBottomLeft(Canvas canvas) => (0, canvas.Size.Height - 1);
	public static (int, int) GetCanvasBottomRight(Canvas canvas) => (canvas.Size.Width - 1, canvas.Size.Height - 1);

	public static IEnumerable<(int, int)> GetVerticalLine(int setCoord, int from, int to)
	{
		for (int y = from; y < to; y++)
		{
			yield return (setCoord, y);
		}
	}


	public static IEnumerable<(int, int)> GetHorizontalLine(int setCoord, int from, int to)
	{
		for (int y = from; y < to; y++)
		{
			yield return (y, setCoord);
		}
	}

	public static (int, int) Add(this (int, int) t, int num1, int num2) => (t.Item1 + num1, t.Item2 + num2);
	public static (int, int) Subtract(this (int, int) t, int num1, int num2) => (t.Item1 - num1, t.Item2 - num2);
	public static (int, int) Multiply(this (int, int) t1, int f) => (t1.Item1 * f, t1.Item2 * f);
}
