namespace RogueConsole.Entities;

abstract class Entity
{
    public int X { get; set; }
    public int Y { get; set; }
    public char Glyph { get; set; }
    public int Health { get; set; }
    public int MaxHealth { get; set; }
    public bool IsDead => Health <= 0;

    public virtual void Update()
    {
        // override per entity type
    }
}
