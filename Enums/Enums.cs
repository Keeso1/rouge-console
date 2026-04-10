namespace RogueConsole.Enums;

public enum GamePhase
{
    Running,
    GameOver,
    Victory,
}

public enum Direction
{
    Down,
    Up,
    Left,
    Right,
}

public enum Cardinals
{
    North,
    West,
    East,
    South,
    Unknown,
}

public enum EntityType
{
    Player,
    Enemy,
}

public enum RoomTypes
{
    Spawn = 1,
    Item = 2,
    Boss = 3,
    Normal = 4,
}
