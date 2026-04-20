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


    public void Dispose() {
        foreach (var enemy in _enemyBuffer) {
            enemy.Dispose();
        }
        Entity.EnemyMove -= Update;
    }

    public EnemyRoom(Canvas canvas) : base(canvas) {
        canvas1 = canvas;
        Entity.EnemyMove += Update;
    }


    public Task Update(Entity enemy) {
        if (!_enemyBuffer.Contains(enemy)) return Task.CompletedTask;
        UnSet(CanvasHelpers.GetWordBound(enemy.PrevPosition, enemy.Body));
        Set(CanvasHelpers.GetWordBound(enemy.Position, enemy.Body), Tile.Goblin(enemy));
        return Task.CompletedTask;
    }


    public void GenerateEnemies() {
        int numberOfEnemies = Rng.GetRandom().Next(1, 10);
        var canvasSize = CanvasWrapper.Instance.Size;
        int maxAttempts = (canvasSize.Width - 2) * (canvasSize.Height - 2);

        for (int num = 0; num < numberOfEnemies; num++) {
            HashSet<Point> visitedTiles = [];
            Point position = new(-1, -1);
            string body = "Goblin";

            while (visitedTiles.Count < maxAttempts) {
                Point candidate = Rng.GetRandomFromCanvas().ToPoint();
                if (!visitedTiles.Add(candidate)) continue;

                var wordBound = CanvasHelpers.GetWordBound(candidate, body);

                if (!wordBound.InBounds(canvasSize)) continue;

                bool canPlace = true;
                foreach (var p in wordBound) {
                    if (!Tiles[p.X, p.Y].Walkable || Tiles[p.X, p.Y].Entity is not null) {
                        canPlace = false;
                        break;
                    }
                }

                if (canPlace) {
                    position = candidate;
                    break;
                }
            }


            if (position.X != -1) {
                var goblin = new Goblin(position, 100, 100, this);
                _enemyBuffer.Add(goblin);
                // Mark tiles immediately so they are taken for the next enemy in this same room generation
                Set(CanvasHelpers.GetWordBound(position, goblin.Body), Tile.Goblin(goblin));
            } else {
                // If we can't place more enemies, just stop instead of throwing and crashing the game
                break;
            }
        }
    } //Evil fuckhack! #HACKTHEPLANET!

    public override void InitMap() {
        base.InitMap();
        RoomType = RoomTypes.Normal;

        GenerateEnemies();
        foreach (Entity enemy in _enemyBuffer) {
            Set(CanvasHelpers.GetWordBound(enemy.Position, enemy.Body), Tile.Goblin(enemy));
        }
    }

}
