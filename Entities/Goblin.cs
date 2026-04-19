using System.Drawing;
using Vimonia.Core;
using Vimonia.Enums;
using Vimonia.World;

namespace Vimonia.Entities;

public class Goblin : Entity {

    public int goblinHealth { get; set; }
    public int goblinMaxHealth { get; set; }

    public Goblin(Point position, int health, int maxHealth, TileMap currentRoom) : base(position, health, maxHealth, currentRoom, EntityType.Enemy) {
        goblinHealth = health;
        goblinMaxHealth = MaxHealth;
    }
}
