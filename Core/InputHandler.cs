public enum InputAction
{
    None,
    MoveNorth,
    MoveSouth,
    MoveEast,
    MoveWest,
    Wait,
    Confirm,
    Cancel,
    OpenInventory,
    OpenMap,
    Quit,
}

public class InputHandler
{
    public ConsoleKeyInfo? PollKey()
    {
        if (!Console.KeyAvailable)
        {
            return null;
        }
        return Console.ReadKey(true);
    }

    public InputAction WaitForInput()
    {
        var key = Console.ReadKey(intercept: true);
        return Map(key);
    }

    public InputAction Poll()
    {
        if (!Console.KeyAvailable)
        {
            return InputAction.None;
        }
        var key = Console.ReadKey(true);
        return Map(key);
    }

    private InputAction Map(ConsoleKeyInfo key)
    {
        // bool shift = key.Modifiers.HasFlag(ConsoleModifiers.Shift);

        return key.Key switch
        {
            ConsoleKey.UpArrow or ConsoleKey.W => InputAction.MoveNorth,
            ConsoleKey.DownArrow or ConsoleKey.S => InputAction.MoveSouth,
            ConsoleKey.RightArrow or ConsoleKey.D => InputAction.MoveEast,
            ConsoleKey.LeftArrow or ConsoleKey.A => InputAction.MoveWest,

            ConsoleKey.Q => InputAction.Quit,
            _ => InputAction.None,
        };
    }
}
