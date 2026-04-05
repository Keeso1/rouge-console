namespace RogueConsole.Entities;

using System.Drawing;
using RogueConsole.Core;

public abstract class Entity
{
    public EntityType Type { get; init; }
    public int Health { get; set; }
    public int MaxHealth { get; set; }
    public bool IsDead => Health <= 0;

    private int _tickCount = 0;
    protected Point _playerPos = new(0, 0);

    protected Entity(int health, int maxhealth, EntityType type)
    {
        Health = health;
        MaxHealth = maxhealth;
        Type = type;

        GameState.OnTick += Update;
        GameState.CurrentState += CheckState;

    }

    protected virtual void Update()
    {
        _tickCount++;
        if (IsDead)
        {
            GameState.OnTick -= Update;
            GameState.CurrentState -= CheckState;
        }
    }

    protected void CheckPlayer()
    {
        if (_tickCount % 2 == 0)
        {
            _playerPos = GameState.PrevPosition;

        }
    }

    protected void CheckState(object? sender, GamePhase phase)
    {
        if (phase is GamePhase.Running) return;
        if (phase is GamePhase.GameOver || phase is GamePhase.Victory)
        {
            GameState.OnTick -= Update;
            GameState.CurrentState -= CheckState;
        }

    }
}

public enum EntityType
{
    Player,
    Enemy
}
