using Vimonia.Enums;
using Vimonia.Utils;
using Sharpie;
using Microsoft.Extensions.Logging;
using Vimonia.Core;
using Vimonia.Entities;

namespace Vimonia.World.Maps;

public class NormalRoom : TileMap
{

    private List<Entity> _enemyBuffer = [];
    private Canvas canvas1 { get; set; }
    private ILogger _logger { get; set; }

    public NormalRoom(Canvas canvas, ILogger logger) : base(canvas, logger)
    {
        canvas1 = canvas;
        _logger = logger;
        // GameState.PlayerInput += Update;
        Entity.EnemyMove += Update;
    }


    public async Task Update(Entity entity)
    {
        await UpdatePos(entity);
    }

    private async Task UpdatePos(Entity entity)
    {
        UnSet(entity.PrevPosition);
        Set(entity.Position, Tile.Goblin(entity.Position));
    }

    public async Task GenerateEnemies()
    {
        int numberOfEnemies = Rng.GetRandom().Next(1, 10);
        for (int num = 0; num < numberOfEnemies; num++)
        {
            _enemyBuffer.Add(new Goblin(Rng.GetRandomFromCanvas().ToPoint(), 100, 100));
        }
    }

    public async override void InitMap()
    {
        base.InitMap();
        RoomType = RoomTypes.Normal;

        await GenerateEnemies();
        foreach (Entity enemy in _enemyBuffer)
        {
            Set(enemy.Position, Tile.Goblin(enemy.Position));
        }
    }

}
