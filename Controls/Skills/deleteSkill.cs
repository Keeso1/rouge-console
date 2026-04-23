using Vimonia.Enums;
using Vimonia.Interfaces;

public class DeleteSkill : ISkill {
    public float Cooldown { get; set; } = 8.0f;
    public string? Combination { get; set; } = "dw";
    public SkillTypes Type { get; init; } = SkillTypes.Delete;

}
