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

    public CombatHandler() {
        _combatHandler = this;
        Entities.Player.UsedSkill += onSkillUse;
    }

    public void Init(TileMap currentRoom) {
        CurrentRoom = currentRoom;
    }

    private void onSkillUse(ISkill skill, Point playerPos) {
        if (CurrentRoom.Tiles[playerPos.X, playerPos.Y].Entity != null) {
        }
    }

    public static CombatHandler Instance => _combatHandler ?? throw new InvalidOperationException($"Logger not initialized");
}
