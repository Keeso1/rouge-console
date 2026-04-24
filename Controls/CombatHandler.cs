using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System.Drawing;
using System.Runtime.CompilerServices;
using Vimonia.Entities;
using Vimonia.Enums;
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
        if (lengthToRemove == null || lengthToRemove.Count == 0) {
            return;
        }

        int startIndex = playerPos.X - entity.Position.X;
        if (startIndex >= 0 && startIndex < entity.Body.Length) {
            entity.Body = entity.Body.Remove(startIndex);
            CurrentRoom.Tiles[playerPos.X, playerPos.Y].Text = entity.Body;
            entity.Health = entity.Body.Length;
            CurrentRoom.Set(lengthToRemove, Tile.Floor());
            _player.Heal(lengthToRemove.Count * 10);
            Log.Info($"Entity health: {entity.Health}, Tile Text Prop: {CurrentRoom.Tiles[playerPos.X, playerPos.Y].Text}");

            if (entity.Health <= 0) {
                entity.Dispose();
                Log.Info($"Enemy defeated: {entity.Type}");
            }
        }
    }

    public static CombatHandler Instance => _combatHandler ?? throw new InvalidOperationException($"CombatHandler not initialized");
}
