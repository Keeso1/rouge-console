using System.Drawing;
using Vimonia.Core;
using Vimonia.Enums;

namespace Vimonia.Entities;

public class Goblin : Entity {

    public int goblinHealth { get; set; }
    public int goblinMaxHealth { get; set; }

    public Goblin(Point position, int health, int maxHealth) : base(position, health, maxHealth, EntityType.Enemy) {
        goblinHealth = health;
        goblinMaxHealth = MaxHealth;
    }
}
