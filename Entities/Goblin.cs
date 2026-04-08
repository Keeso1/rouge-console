namespace RogueConsole.Entities;

public class Goblin(int health, int maxHealth) : Entity(health, maxHealth, EntityType.Enemy)
{
	protected override void Update()
	{
		base.Update();
		CheckPlayer();
	}
}
