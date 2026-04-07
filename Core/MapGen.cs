namespace RogueConsole.Core;

using System.Linq;
using System.Text;
using Microsoft.Extensions.Logging;
using RogueConsole.Enums;
using RogueConsole.World;
using Sharpie;

public class FloorLayout
{
    public TileMap[,] Rooms { get; private set; } = new TileMap[16, 16];
    private readonly ILogger _logger;

    public FloorLayout(ILogger Logger, Canvas canvas, GameSettings settings)
    {
        _logger = Logger;
        Rooms[8, 8] = TileMap.GetRoom(RoomTypes.Spawn, canvas, _logger);
        Rooms[8, 8].InitMap();

        for (var room = 0; room < settings.NumberOfRooms; room++)
        {
            Generate(canvas);
            _logger.LogInformation("Run nr {room}", room);
            _logger.LogInformation("Rooms: {rooms}", FloorLayout.RoomsToString(Rooms));
        }
    }

    public static string RoomsToString(TileMap[,] Rooms) //Helper func to see the grid in a clean way
    {
        var sb = new StringBuilder();
        for (int y = 0; y < Rooms.GetLength(1); y++)
        {
            for (int x = 0; x < Rooms.GetLength(0); x++)
            {
                if (Rooms[x, y] != null)
                {
                    sb.Append(Convert.ToInt32(Rooms[x, y].Active)); //dafuq
                    sb.Append(' ');
                }
                else
                {
                    sb.Append("0");
                    sb.Append(' ');
                }
            }
            sb.AppendLine();
        }
        return sb.ToString();
    }

    public void Generate(Canvas canvas)
    {
        var rand = new Random();
        List<(int, int)> activeRooms = new();
        for (int x = 0; x < Rooms.GetLength(0); x++)
        {
            for (int y = 0; y < Rooms.GetLength(1); y++)
            {
                if (Rooms[x, y] != null)
                {
                    (int, int) tempTpl = (x, y);
                    activeRooms.Add(tempTpl);
                }
            }
        }

        _logger.LogInformation("active rooms count {ac}", activeRooms.Count);

        // Retry until we find an expandable room or run out of candidates
        while (activeRooms.Count > 0)
        {
            var selIdx = rand.Next(activeRooms.Count);
            (int, int) selRoom = activeRooms[selIdx];
            var checkedRooms = CheckRooms(selRoom);
            _logger.LogInformation("checkedRooms: {chrooms}", checkedRooms);
            if (checkedRooms.Count > 0)
            {
                // Successfully found an expandable room
                var r = rand.Next(checkedRooms.Count);
                var randomRoom = checkedRooms[r];
                Rooms[randomRoom.Item1, randomRoom.Item2] = new TileMap(canvas) { Active = true };
                break;
            }
            else
            {
                // This room cannot expand, remove it from candidates
                _logger.LogInformation("removing Room at {index}", selIdx);
                activeRooms.RemoveAt(selIdx);
            }
        }
    }

    public List<(int, int)> CheckRooms((int, int) room)
    {
        _logger.LogInformation("Coming into checkrooms");
        List<(int, int)> state = new();
        int maxX = Rooms.GetLength(0);
        int maxY = Rooms.GetLength(1);

        _logger.LogInformation(
            "room.Item1 {item1} \n room.Item2 {item2} \n maxX {maxX} \n maxY {maxY}",
            room.Item1,
            room.Item2,
            maxX,
            maxY
        );
        // Check right neighbor
        if (room.Item1 + 1 < maxX && Rooms[room.Item1 + 1, room.Item2] == null)
        {
            state.Add((room.Item1 + 1, room.Item2));
        }

        // Check left neighbor
        if (room.Item1 - 1 >= 0 && Rooms[room.Item1 - 1, room.Item2] == null)
        {
            state.Add((room.Item1 - 1, room.Item2));
        }

        // Check down neighbor
        if (room.Item2 + 1 < maxY && Rooms[room.Item1, room.Item2 + 1] == null)
        {
            state.Add((room.Item1, room.Item2 + 1));
        }

        // Check up neighbor
        if (room.Item2 - 1 >= 0 && Rooms[room.Item1, room.Item2 - 1] == null)
        {
            state.Add((room.Item1, room.Item2 - 1));
        }

        return state;
    }
}
