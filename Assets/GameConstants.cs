using System.Text;

namespace RogueConsole.Assets;

public static class Assets
{
    public static readonly Rune Player = new('@');
    public static readonly Rune Enemy = new('X');
    public static readonly Rune Ground = new('.');
    public static readonly Rune Bush = new('B');
    public static readonly Rune Wall = new('#');
    public static readonly Rune Space = new(' ');
}
