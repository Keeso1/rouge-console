using Vimonia.Enums;
namespace Vimonia.Interfaces;

public interface IEntity {
    int Health { get; }
    int MaxHealth { get; }
    EntityType Type { get; }
}
