using System.Drawing;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RogueConsole.Enums;
using RogueConsole.Utils;
using RogueConsole.World;
using Sharpie;

namespace RogueConsole.Core;

public class MapGen
{
    public TileMap[,] Rooms { get; private set; }
    private readonly ILogger _logger;
    private GameSettings _settings { get; set; }

    public MapGen(ILogger Logger, Canvas canvas, GameSettings settings)
    {
        _settings = settings;
        Rooms = new TileMap[(_settings.NumberOfRooms + 1) * 2, (_settings.NumberOfRooms + 1) * 2];
        _logger = Logger;
        Rooms[_settings.NumberOfRooms + 1, _settings.NumberOfRooms + 1] = TileMap.GetRoom(
            RoomTypes.Spawn,
            canvas
        );
        Rooms[_settings.NumberOfRooms + 1, _settings.NumberOfRooms + 1].InitMap();

        _logger.LogInformation("_settings number of rooms {rr}", _settings.NumberOfRooms);
        _logger.LogInformation("size: {x}x{y}", Rooms.GetLength(0), Rooms.GetLength(1));

        for (var room = 0; room < _settings.NumberOfRooms; room++)
        {
            Generate(canvas);
            _logger.LogInformation("Run nr {room}", room);
            // _logger.LogInformation("Rooms: {rooms}", RoomsToString(Rooms));
        } // Generate layout

        GenerateBossRoom(_logger, canvas); // Add bossroom at furthest x value
        GenerateItemRoom(_logger, canvas);
        SetDoors();
        foreach (var (x, y) in GetNonEmptyRooms())
        {
            _logger.LogInformation("NonEmptyRoom: {room}", (x, y));
            Rooms[x, y].InitMap();
        }
        _logger.LogInformation("Rooms: {rooms}", RoomsToString(Rooms));
        _logger.LogInformation("Neghbors of spawn: {neigh}", Rooms[8, 8].Neighbors);
    }

    private void GenerateItemRoom(ILogger logger, Canvas canvas)
    {
        List<(int x, int y)> activeRooms = [];

        for (int x = 0; x < Rooms.GetLength(0); x++)
        {
            for (int y = 0; y < Rooms.GetLength(1); y++)
            {
                if (
                    Rooms[x, y] != null
                    && Rooms[x, y].RoomType != RoomTypes.Boss
                    && Rooms[x, y].RoomType != RoomTypes.Spawn
                )
                {
                    activeRooms.Add((x, y));
                }
            }
        }

        if (activeRooms.Count == 0)
        {
            throw new Exception(
                "Can't generate itemroom because there are no eligible rooms to select from."
            );
        }

        var rand = new Random();
        (int x, int y) randRoom = activeRooms[rand.Next(activeRooms.Count)];

        Rooms[randRoom.x, randRoom.y] = TileMap.GetRoom(RoomTypes.Item, canvas);
        Rooms[randRoom.x, randRoom.y].InitMap();
    }

    private void GenerateBossRoom(ILogger _logger, Canvas canvas)
    {
        (int x, int y) = BFS.Execute(Rooms, _settings); //Breadth-first-search
        Rooms[x, y] = TileMap.GetRoom(RoomTypes.Boss, canvas);
        Rooms[x, y].InitMap();

        _logger.LogInformation("Rooms: {rooms}", RoomsToString(Rooms));
    }

    public void SetDoors()
    {
        Size size = new(Rooms.GetLength(0), Rooms.GetLength(1));
        List<(int x, int y)> activeRooms = GetNonEmptyRooms();
        foreach (var room in activeRooms)
        {
            List<Cardinals> activeNeighbors = [];
            var neighbors = room.GetCardinalNeighbours().ToList();

            for (
                int neighborIndex = (int)Cardinals.North;
                neighborIndex < neighbors.Count;
                neighborIndex++
            )
            {
                var neighbor = neighbors[neighborIndex];
                if (Rooms[neighbor.x, neighbor.y] != null && neighbor.InBounds(size))
                {
                    activeNeighbors.Add((Cardinals)neighborIndex);
                }
            }

            Rooms[room.x, room.y].Neighbors = activeNeighbors;
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

    public List<(int x, int y)> GetNonEmptyRooms()
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
                    canvas
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
