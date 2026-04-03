namespace RogueConsole.Core;

public class GameState
{
	public static event EventHandler<GamePhase> CurrentState;
	public static event Action? OnTick;

	public GamePhase Phase { get; set; } = GamePhase.Running;

	public void RaiseStateChanged(GamePhase phase)
	{
		Phase = phase;
		CurrentState?.Invoke(this, phase);
	}

	public void Update()
	{
		OnTick?.Invoke();
	}
}

public enum GamePhase
{
	Running,
	GameOver,
	Victory
}






