namespace RogueConsole.Enums;

public enum GamePhase
{
    Running,
    GameOver,
    Victory,
}

public enum Direction
{
    down,
    up,
    left,
    right,
}

public enum EntityType
{
    Player,
    Enemy,
}

public enum RoomTypes
{
    Spawn,
    Item,
    Boss,
    Normal
}
