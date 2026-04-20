using Vimonia.Enums;
using Vimonia.Utils;
using Sharpie;
using Vimonia.Core;
using Vimonia.Entities;
using System.Drawing;

namespace Vimonia.World.Maps;

public class EnemyRoom : TileMap {
    private List<Entity> _enemyBuffer = [];
    private Canvas canvas1 { get; set; }

    public EnemyRoom(Canvas canvas) : base(canvas) {
        canvas1 = canvas;
        Entity.EnemyMove += Update;
    }


    public Task Update(Entity entity) {
        if (!_enemyBuffer.Contains(entity)) return Task.CompletedTask;
        UnSet(entity.PrevPosition);
        Set(entity.Position, Tile.Goblin(entity));
        return Task.CompletedTask;
    }

    public void GenerateEnemies() {
        int numberOfEnemies = Rng.GetRandom().Next(1, 10);
        int allTilesLen = CanvasWrapper.AllCanvasPoints().Count;

        for (int num = 0; num < numberOfEnemies; num++) {
            HashSet<Point> visitedTiles = [];
            Point position;
            do {
                position = Rng.GetRandomFromCanvas().ToPoint();
                visitedTiles.Add(position);
                if (visitedTiles.Count >= allTilesLen) {
                    position = new(-1, -1);
                    break;
                }
            }
            while (!Tiles[position.X, position.Y].Walkable || Tiles[position.X, position.Y].Entity is not null);


            if (position != position with { X = -1, Y = -1 }) {
                _enemyBuffer.Add(new Goblin(position, 100, 100, this));
            } else {
                throw new Exception("No available positions to place tile");
            }
        }
    } //Evil fuckhack! #HACKTHEPLANET!

    public override void InitMap() {
        base.InitMap();
        RoomType = RoomTypes.Normal;

        GenerateEnemies();
        foreach (Entity enemy in _enemyBuffer) {
            CanvasWrapper.Instance.Text(enemy.Position, enemy.Body, Canvas.Orientation.Horizontal, Style.Default);
            // Set(enemy.Position, Tile.Goblin(enemy));
        }
    }

}
