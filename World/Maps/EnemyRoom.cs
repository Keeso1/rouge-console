using Vimonia.Enums;
using Vimonia.Utils;
using Sharpie;
using Vimonia.Core;
using Vimonia.Entities;
using System.Drawing;

namespace Vimonia.World.Maps;

public class EnemyRoom : TileMap
{
    private List<Entity> _enemyBuffer = [];
    private Canvas canvas1 { get; set; }

    public EnemyRoom(Canvas canvas) : base(canvas)
    {
        canvas1 = canvas;
        Entity.EnemyMove += Update;
    }


    public Task Update(Entity entity)
    {
        if (!_enemyBuffer.Contains(entity)) return Task.CompletedTask;
        UnSet(entity.PrevPosition);
        Set(entity.Position, Tile.Goblin(entity));
        return Task.CompletedTask;
    }

    public void GenerateEnemies()
    {
        int numberOfEnemies = Rng.GetRandom().Next(1, 10);
        for (int num = 0; num < numberOfEnemies; num++)
        {
            Point position;
            do
            {
                position = Rng.GetRandomFromCanvas().ToPoint();
            }
            while (!Tiles[position.X, position.Y].Walkable || Tiles[position.X, position.Y].Entity is not null);

            _enemyBuffer.Add(new Goblin(position, 100, 100, this));
        }
    }

    public override void InitMap()
    {
        base.InitMap();
        RoomType = RoomTypes.Normal;

        GenerateEnemies();
        foreach (Entity enemy in _enemyBuffer)
        {
            Set(enemy.Position, Tile.Goblin(enemy));
        }
    }

}
