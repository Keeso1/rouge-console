using Vimonia.Enums;
using Vimonia.Utils;
using Sharpie;
using Microsoft.Extensions.Logging;
using Vimonia.Core;
using Vimonia.Entities;

namespace Vimonia.World.Maps;

public class EnemyRoom : TileMap
{
    private List<Entity> _enemyBuffer = [];
    private Canvas canvas1 { get; set; }
    private ILogger _logger { get; set; }

    public EnemyRoom(Canvas canvas, ILogger logger) : base(canvas, logger)
    {
        canvas1 = canvas;
        _logger = logger;
        Entity.EnemyMove += Update;
    }


    public Task Update(Entity entity)
    {
        if (!_enemyBuffer.Contains(entity)) return Task.CompletedTask;
        UnSet(entity.PrevPosition);
        Set(entity.Position, Tile.Goblin(entity.Position, entity));
        return Task.CompletedTask;
    }

    public async Task GenerateEnemies()
    {
        int numberOfEnemies = Rng.GetRandom().Next(1, 10);
        for (int num = 0; num < numberOfEnemies; num++)
        {
            _enemyBuffer.Add(new Goblin(Rng.GetRandomFromCanvas().ToPoint(), 100, 100, this));
        }
    }

    public async override void InitMap()
    {
        base.InitMap();
        RoomType = RoomTypes.Normal;

        await GenerateEnemies();
        foreach (Entity enemy in _enemyBuffer)
        {
            Set(enemy.Position, Tile.Goblin(enemy.Position, enemy));
        }
    }

}
