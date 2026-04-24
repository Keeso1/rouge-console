using System.Drawing;
using Sharpie;
using Vimonia.Core;
using Vimonia.Interfaces;
using Vimonia.Enums;
using Vimonia.Utils;

namespace Vimonia.Entities;

public sealed class Player : IEntity {

    public static event Action<ISkill, Point> UsedSkill;

    public EntityType Type { get; set; }
    public int Health { get; private set; }
    public int MaxHealth { get; private set; }
    public bool IsDead => Health <= 0;
    public Dictionary<string, ISkill> Skills { get; private set; } = [];
    public Point Position { get; set; }
    public Style Style { get; set; }
    public string Combo { get; set; }

    public Player(int health, int maxHealth, Style style, List<ISkill>? skills = null) {
        Health = health;
        MaxHealth = maxHealth;
        if (skills != null) AddSkills(skills);
        Type = EntityType.Player;
        Style = style;
        Combo = "";

        GameState.PlayerInput += OnPlayerInput;
        GameState.OnTick += Update;
        GameState.CurrentState += CheckState;
    }

    private void OnPlayerInput(object? _, Point point) => Position = point;

    private void Update() {
        if (IsDead) Unsub();
    }

    private void CheckState(object? sender, GamePhase phase) {
        if (phase == GamePhase.Running) return;
        Unsub();
    }

    public void UseSkill(string combo) {
        if (Skills.TryGetValue(combo, out var skill)) {

            UsedSkill?.Invoke(skill, Position);
            Log.Info($"Used skill: {skill.Type}");
        }
        Combo = "";
        Log.Info($"Combo: {Combo}");

    }

    public void AddSkills(List<ISkill> skills) {
        foreach (var skill in skills) {
            AddSkill(skill);
        }
    }

    public void AddSkill(ISkill skill) => Skills[skill.Combination] = skill;

    public void TakeDamage(int damage) {
        if (Health >= 0 || !IsDead)
            Health = Math.Max(0, Health - damage);
    }

    public void Heal(int heal) {
        if (Health < MaxHealth || !IsDead)
            Health = Math.Min(MaxHealth, Health + heal);
    }

    private void Unsub() {
        GameState.PlayerInput -= OnPlayerInput;
        GameState.OnTick -= Update;
        GameState.CurrentState -= CheckState;
    }
}

