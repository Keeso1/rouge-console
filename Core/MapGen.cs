namespace RogueConsole.Core;

using RogueConsole.World;
using System.Linq;

public class MapGen
{
	public int[,] Rooms { get; private set; } = new int[16, 16];

	public MapGen()
	{

	}

	public void Generate()
	{
		var rand = new Random();
		List<(int, int)> activeRooms = new();
		for (int x = 0; x < Rooms.GetLength(0); x++)
		{
			for (int y = 0; y < Rooms.GetLength(1); y++)
			{
				if (Rooms[x, y] == 1)
				{
					(int, int) tempTpl = (x, y);
					activeRooms.Add(tempTpl);
				}
			}
		}

		var selIdx = rand.Next(activeRooms.Count());
		(int, int) selRoom = activeRooms[selIdx];
		var checkedRooms = CheckRooms(selRoom);
		bool success = checkedRooms.Count() > 0;
		if (success)
		{
			var r = rand.Next(checkedRooms.Count());
			var randomRoom = checkedRooms[r];
			Rooms[randomRoom.Item1, randomRoom.Item2] = 1;
		}
		else
		{

		}
	}

	private List<(int, int)> CheckRooms((int, int) room)
	{
		List<(int, int)> state = new();

		if (Rooms[room.Item1 + 1, room.Item2] == 0)
		{
			state.Add((room.Item1 + 1, room.Item2));
		}

		if (Rooms[room.Item1 - 1, room.Item2] == 0)
		{

			state.Add((room.Item1 - 1, room.Item2));
		}

		if (Rooms[room.Item1, room.Item2 + 1] == 0)
		{

			state.Add((room.Item1, room.Item2 + 1));
		}

		if (Rooms[room.Item1, room.Item2 - 1] == 0)
		{

			state.Add((room.Item1, room.Item2 - 1));
		}

		return state;

	}

}
