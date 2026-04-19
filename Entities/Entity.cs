using System.Drawing;
using Vimonia.Core;
using Vimonia.Enums;
using Vimonia.World;

namespace Vimonia.Entities;


public abstract class Entity : IEntity {

    public EntityType Type { get; init; }
    public int Health { get; set; }
    public int MaxHealth { get; set; }
    public bool IsDead => Health <= 0;
    public Point Position { get; set; }
    public Point PrevPosition { get; set; }
    private TileMap _currentRoom { get; set; }
    private static readonly Direction[] s_directions = Enum.GetValues<Direction>();

    public delegate Task EnemyInfo(Entity sender);
    public static event EnemyInfo EnemyMove;

    private int _tickCount = 0;
    protected Point _playerPos = new(0, 0);

    protected Entity(Point position, int health, int maxhealth, TileMap currentRoom, EntityType type) {
        Health = health;
        MaxHealth = maxhealth;
        Type = type;
        Position = position;
        _currentRoom = currentRoom;

        GameState.PlayerInput += Update;
        GameState.CurrentState += CheckState;

    }

    protected virtual void Update(object sender, Point playerPos) {
        _tickCount++;
        if (IsDead) {
            GameState.CurrentState -= CheckState;
            GameState.PlayerInput -= Update;
            return;
        }

        _playerPos = playerPos;

        Point newPos = Controls.Move(s_directions[Rng.GetRandom().Next(s_directions.Length)], Position, _currentRoom, _playerPos);
        PrevPosition = Position;
        Position = newPos;
        EnemyMove?.Invoke(this);
    }

    // protected void CheckPlayer() {
    //     if (_tickCount % 2 == 0) {
    //         _playerPos = new(2, 2); //TODO: FIX THIS
    //
    //     }
    // }

    protected void CheckState(object? sender, GamePhase phase) {
        if (phase is GamePhase.Running) return;
        if (phase is GamePhase.GameOver || phase is GamePhase.Victory) {
            GameState.CurrentState -= CheckState;
        }

    }
}


