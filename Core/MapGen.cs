using System.Drawing;
using System.Text;
using Microsoft.Extensions.Logging;
using RogueConsole.Enums;
using RogueConsole.World;
using RogueConsole.Utils;
using Sharpie;

namespace RogueConsole.Core;


public class MapGen
{
    public TileMap[,] Rooms { get; private set; } = new TileMap[16, 16];
    private readonly ILogger _logger;

    public MapGen(ILogger Logger, Canvas canvas, GameSettings settings)
    {
        _logger = Logger;
        Rooms[8, 8] = TileMap.GetRoom(RoomTypes.Item, canvas, _logger);
        Rooms[8, 8].InitMap();

        for (var room = 0; room < settings.NumberOfRooms; room++)
        {
            Generate(canvas);
            _logger.LogInformation("Run nr {room}", room);
            _logger.LogInformation("Rooms: {rooms}", MapGen.RoomsToString(Rooms));
        } // Generate layout

        GenerateBossRoom(_logger, canvas); // Add bossroom at furthest x value
    }

    private void GenerateBossRoom(ILogger _logger, Canvas canvas) // TODO: Improve the select to choose the room furthest i.e most moves from spawn
    {
        var biggestDiff = (8, 8);
        foreach ((int, int) tuple in GetNonEmptyRooms())
        {
            if (Math.Abs(tuple.Item1 - 8) > Math.Abs(biggestDiff.Item1 - 8))
            {
                biggestDiff = tuple;
            }
            ;

            _logger.LogInformation("activerooms: {t}", tuple);
            _logger.LogInformation("BiggestDiff on X axis: {diff}", biggestDiff);
        }
        Rooms[biggestDiff.Item1, biggestDiff.Item2] = TileMap.GetRoom(
            RoomTypes.Boss,
            canvas,
            _logger
        );
        Rooms[biggestDiff.Item1, biggestDiff.Item2].InitMap();
        _logger.LogInformation(
            "RoomType of biggestDiff {type}",
            Rooms[biggestDiff.Item1, biggestDiff.Item2].RoomType
        );
        _logger.LogInformation("Rooms: {rooms}", MapGen.RoomsToString(Rooms));
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
                    sb.Append(Convert.ToInt32(Rooms[x, y].RoomType)); //dafuq
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

    public List<(int, int)> GetNonEmptyRooms()
    {
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

        return activeRooms;
    }

    public void Generate(Canvas canvas)
    {
        var rand = new Random();
        List<(int, int)> activeRooms = GetNonEmptyRooms();
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
                Rooms[randomRoom.Item1, randomRoom.Item2] = TileMap.GetRoom(
                    RoomTypes.Normal,
                    canvas,
                    _logger
                );
                Rooms[randomRoom.Item1, randomRoom.Item2].InitMap();
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
        Size size = new(Rooms.GetLength(0), Rooms.GetLength(1));

        _logger.LogInformation(
            "room.Item1 {item1} \n room.Item2 {item2} \n maxX {maxX} \n maxY {maxY}",
            room.Item1,
            room.Item2,
            size.Width,
            size.Height
        );

        return room.GetCardinalNeighbours()
            .Where(r => r.InBounds(size) && Rooms[r.x, r.y] == null)
            .ToList();
    }
}
