using System.Drawing;
using Microsoft.Extensions.Logging;
using Vimonia.Core;
using Vimonia.Enums;

namespace Vimonia.Entities;

public class Goblin : Entity{

    public int goblinHealth {get; set;}
    public int goblinMaxHealth {get; set;}
    private ILogger _logger {get; set;}

    public Goblin(int health, int maxHealth, ILogger logger): base(health, maxHealth, EntityType.Enemy, logger){
        goblinHealth = health;
        goblinMaxHealth = MaxHealth;
        _logger = logger;
        GameState.PlayerInput += Update;
    }

    protected override void Update(object sender, Point playerPos) {
        base.Update(sender, playerPos);
    }
}
