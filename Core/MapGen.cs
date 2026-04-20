using System.Drawing;
using System.Text;
using Vimonia.Enums;
using Vimonia.Utils;
using Vimonia.World;
using Sharpie;

namespace Vimonia.Core;

public class MapGen {
    public TileMap[,] Rooms { get; private set; }
    private GameSettings Settings { get; set; }
    private Size Size { get; set; }
    public (int x, int y) maxSize { get; set; }

    public MapGen(Canvas canvas, GameSettings settings) {
        Settings = settings;
        Rooms = new TileMap[(Settings.NumberOfRooms + 1) * 2, (Settings.NumberOfRooms + 1) * 2];
        Rooms[Settings.NumberOfRooms + 1, Settings.NumberOfRooms + 1] = TileMap.GetRoom(
            RoomTypes.Spawn,
            canvas
        );
        Rooms[Settings.NumberOfRooms + 1, Settings.NumberOfRooms + 1].InitMap();
        Size = new(Rooms.GetLength(0), Rooms.GetLength(1));

        for (var room = 0; room < Settings.NumberOfRooms; room++) {
            Generate(canvas);
        } // Generate layout

        GenerateBossRoom(canvas); // Add bossroom at furthest x value
        GenerateItemRoom(canvas);
        SetDoors();
        foreach (var (x, y) in GetNonEmptyRooms()) {
            Rooms[x, y].InitMap();
        }
    }

    private void GenerateItemRoom(Canvas canvas) {
        List<(int x, int y)> activeRooms = [];

        for (int x = 0; x < Rooms.GetLength(0); x++) {
            for (int y = 0; y < Rooms.GetLength(1); y++) {
                if (
                    Rooms[x, y] != null
                    && Rooms[x, y].RoomType != RoomTypes.Boss
                    && Rooms[x, y].RoomType != RoomTypes.Spawn
                ) {
                    activeRooms.Add((x, y));
                }
            }
        }

        if (activeRooms.Count == 0) {
            throw new Exception(
                "Can't generate itemroom because there are no eligible rooms to select from."
            );
        }

        var rand = new Random();
        (int x, int y) randRoom = activeRooms[rand.Next(activeRooms.Count)];

        Rooms[randRoom.x, randRoom.y] = TileMap.GetRoom(RoomTypes.Item, canvas);
        Rooms[randRoom.x, randRoom.y].InitMap();
    }

    private void GenerateBossRoom(Canvas canvas) {
        (int x, int y) = BFS.Execute(Rooms, Settings); //Breadth-first-search
        Rooms[x, y] = TileMap.GetRoom(RoomTypes.Boss, canvas);
        Rooms[x, y].InitMap();
        maxSize = (x, y);
    }

    public void SetDoors() {
        List<(int x, int y)> activeRooms = GetNonEmptyRooms();
        foreach (var room in activeRooms) {
            List<Cardinals> activeNeighbors = [];

            foreach (var neighbor in room.GetCardinalNeighbours()) {
                if (neighbor.InBounds(Size) && Rooms[neighbor.x, neighbor.y] != null) {
                    activeNeighbors.Add((neighbor.x - room.x, neighbor.y - room.y).ToCardinal());
                }
            }

            Rooms[room.x, room.y].Neighbors = activeNeighbors;
        }
    }

    public static string RoomsToString(TileMap[,] Rooms, TileMap currentRoom) //Helper func to see the grid in a clean way
    {
        var sb = new StringBuilder();
        for (int y = 0; y < Rooms.GetLength(1); y++) {
            for (int x = 0; x < Rooms.GetLength(0); x++) {
                if (Rooms[x, y] != null) {
                    sb.Append(Convert.ToInt32(Rooms[x, y].RoomType)); //dafuq
                    sb.Append(' ');
                } else {
                    sb.Append("0");
                    sb.Append(' ');
                }
            }
            sb.AppendLine();
        }
        return sb.ToString();
    }

    public List<(int x, int y)> GetNonEmptyRooms() {
        List<(int, int)> activeRooms = new();
        for (int x = 0; x < Rooms.GetLength(0); x++) {
            for (int y = 0; y < Rooms.GetLength(1); y++) {
                if (Rooms[x, y] != null) {
                    (int, int) tempTpl = (x, y);
                    activeRooms.Add(tempTpl);
                }
            }
        }

        return activeRooms;
    }

    public void Generate(Canvas canvas) {
        var rand = new Random();
        List<(int, int)> activeRooms = GetNonEmptyRooms();

        // Retry until we find an expandable room or run out of candidates
        while (activeRooms.Count > 0) {
            var selIdx = rand.Next(activeRooms.Count);
            (int, int) selRoom = activeRooms[selIdx];
            var checkedRooms = CheckRooms(selRoom);
            if (checkedRooms.Count > 0) {
                // Successfully found an expandable room
                var r = rand.Next(checkedRooms.Count);
                var randomRoom = checkedRooms[r];
                Rooms[randomRoom.Item1, randomRoom.Item2] = TileMap.GetRoom(
                    RoomTypes.Normal,
                    canvas
                );

                Rooms[randomRoom.Item1, randomRoom.Item2].InitMap();
                break;
            } else {
                // This room cannot expand, remove it from candidates
                activeRooms.RemoveAt(selIdx);
            }
        }
    }

    public List<(int, int)> CheckRooms((int, int) room) {
        return room.GetCardinalNeighbours()
            .Where(r => r.InBounds(Size) && Rooms[r.x, r.y] == null)
            .ToList();
    }
}
