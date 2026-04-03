namespace RogueConsole.Entities;

using RogueConsole.Core;

public abstract class Entity
{
    public EntityType Type { get; init; }
    public int Health { get; set; }
    public int MaxHealth { get; set; }
    public bool IsDead => Health <= 0;

    protected Entity(int health, int maxhealth, EntityType type)
    {
        Health = health;
        MaxHealth = maxhealth;
        Type = type;
    }

    public virtual void Update()
    {
        // override per entity type
    }

    public virtual void CheckState(object? sender, GamePhase phase)
    {

    }


}

public enum EntityType
{
    Player,
    Enemy
}
