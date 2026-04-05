namespace RogueConsole.Entities;

using RogueConsole.World;

public class Goblin(int health, int maxHealth) : Entity(health, maxHealth, EntityType.Enemy)
{
	protected override void Update()
	{
		base.Update();
		CheckPlayer();
	}
}
