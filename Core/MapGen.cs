namespace RogueConsole.Core;

using System.Linq;
using System.Text;
using Microsoft.Extensions.Logging;
using RogueConsole.World;

public class FloorLayout
{
    public int[,] Rooms { get; private set; } = new int[16, 16];
    private readonly ILogger logger;

    public FloorLayout(ILogger _logger)
    {
        logger = _logger;
        Rooms[8, 8] = 1;
    }

    public static string RoomsToString(int[,] Rooms) //Helper func to see the grid in a clean way
    {
        var sb = new StringBuilder();
        for (int y = 0; y < Rooms.GetLength(1); y++)
        {
            for (int x = 0; x < Rooms.GetLength(0); x++)
            {
                sb.Append(Rooms[x, y]);
                sb.Append(' ');
            }
            sb.AppendLine();
        }
        return sb.ToString();
    }

    public void Generate()
    {
        logger.LogInformation(Rooms.ToString());
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

        logger.LogInformation("active rooms count {ac}", activeRooms.Count);

        // Retry until we find an expandable room or run out of candidates
        while (activeRooms.Count > 0)
        {
            var selIdx = rand.Next(activeRooms.Count);
            (int, int) selRoom = activeRooms[selIdx];
            var checkedRooms = CheckRooms(selRoom);

            if (checkedRooms.Count > 0)
            {
                // Successfully found an expandable room
                var r = rand.Next(checkedRooms.Count);
                var randomRoom = checkedRooms[r];
                Rooms[randomRoom.Item1, randomRoom.Item2] = 1;
                break;
            }
            else
            {
                // This room cannot expand, remove it from candidates
                activeRooms.RemoveAt(selIdx);
            }
        }
    }

    private List<(int, int)> CheckRooms((int, int) room)
    {
        List<(int, int)> state = new();
        int maxX = Rooms.GetLength(0);
        int maxY = Rooms.GetLength(1);

        // Check right neighbor
        if (room.Item1 + 1 < maxX && Rooms[room.Item1 + 1, room.Item2] == 0)
        {
            state.Add((room.Item1 + 1, room.Item2));
        }

        // Check left neighbor
        if (room.Item1 - 1 >= 0 && Rooms[room.Item1 - 1, room.Item2] == 0)
        {
            state.Add((room.Item1 - 1, room.Item2));
        }

        // Check down neighbor
        if (room.Item2 + 1 < maxY && Rooms[room.Item1, room.Item2 + 1] == 0)
        {
            state.Add((room.Item1, room.Item2 + 1));
        }

        // Check up neighbor
        if (room.Item2 - 1 >= 0 && Rooms[room.Item1, room.Item2 - 1] == 0)
        {
            state.Add((room.Item1, room.Item2 - 1));
        }

        return state;
    }
}