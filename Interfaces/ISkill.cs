using Vimonia.Enums;
namespace Vimonia.Interfaces;

public interface ISkill {
    float Cooldown { get; set; }
    string Combination { get; set; }
    SkillTypes Type { get; set; }
}
