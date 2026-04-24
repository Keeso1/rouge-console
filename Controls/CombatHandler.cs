using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System.Drawing;
using System.Runtime.CompilerServices;
using Vimonia.Entities;
using Vimonia.Interfaces;
using Vimonia.World;

namespace Vimonia.Utils;

public class CombatHandler {
    private static CombatHandler _combatHandler { get; set; }
    private TileMap CurrentRoom { get; set; }
    private Player _player { get; set; }
    public CombatHandler(Player player) {
        _player = player;
        _combatHandler = this;
        Entities.Player.UsedSkill += onSkillUse;
    }

    public void Init(TileMap currentRoom) {
        CurrentRoom = currentRoom;
    }

    private void onSkillUse(ISkill skill, Point playerPos) {
        if (CurrentRoom == null) {
            return;
        }

        var entity = CurrentRoom.Tiles[playerPos.X, playerPos.Y].Entity;
        if (entity == null) {
            return;
        }

        List<Point>? lengthToRemove = CanvasHelpers.GetRemainingLetters(playerPos, entity, CurrentRoom);
        if (lengthToRemove == null) {
            return;
        }

        int startIndex = entity.Body.Length - lengthToRemove.Count;
        if (startIndex >= 0 && startIndex <= entity.Body.Length) {
            if (startIndex == 0) {
                entity.Health = 0;
                entity.Dispose();
                CurrentRoom.Set(lengthToRemove, Tile.Floor());
                Log.Info($"enemy health: {entity.Health}");

            }
            entity.Body = entity.Body.Remove(startIndex);
            CurrentRoom.Tiles[playerPos.X, playerPos.Y].Text = entity.Body;
            entity.Health = entity.Body.Length;
            CurrentRoom.Set(lengthToRemove, Tile.Floor());
            _player.Heal(lengthToRemove.Count * 10);
            Log.Info($"Entity health: {entity.Health}, Tile Text Prop: {CurrentRoom.Tiles[playerPos.X, playerPos.Y].Text}");
        }
    }

    public static CombatHandler Instance => _combatHandler ?? throw new InvalidOperationException($"CombatHandler not initialized");
}
