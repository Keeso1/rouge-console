namespace RogueConsole.Entities;

using RogueConsole.Core;
using RogueConsole.World;

public class Goblin : Entity
{
	private int _tickCount = 0;
	private (int, int) _playerPos;

	public Goblin(TileMap map, int health, int maxHealth) : base(map, health, maxHealth, EntityType.Enemy)
	{
		GameState.OnTick += Update;
		GameState.CurrentState += CheckState;
	}

	public override void Update()
	{
		_tickCount++;
		CheckPlayer();
	}

	private void CheckPlayer()
	{
		if (_tickCount % 2 == 0)
		{
			_playerPos = _map.GetPlayer() ?? (0, 0);

		}
	}

	public override void CheckState(object? sender, GamePhase phase)
	{
		if (phase is GamePhase.Running) return;
		if (phase is GamePhase.GameOver || phase is GamePhase.Victory)
		{
			GameState.OnTick -= Update;
			GameState.CurrentState -= CheckState;
		}

	}



}
