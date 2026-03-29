namespace RogueConsole.Core;

using RogueConsole.Entities;
using RogueConsole.World;

class GameState
{
	public event EventHandler<GamePhase> CurrentState;

	public Entity Player { get; set; } = new();
	public TileMap Map { get; set; } = new();
	public List<Entity> Enemies { get; set; } = new();
	public GamePhase Phase { get; set; } = GamePhase.Running;
	public int Tick { get; private set; }

	public void RaiseStateChanged(GamePhase phase)
	{
		Phase = phase;
		CurrentState?.Invoke(this, phase);
	}

	public void Update()
	{
		foreach (var enemy in Enemies)
			enemy.Update();

		Tick++;
	}
}

enum GamePhase
{
	Running,
	GameOver,
	Victory
}






