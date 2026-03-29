internal class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("Hello, World!");
        var input = new InputHandler();
        while (true)
        {
            var inputKey = input.WaitForInput();
            Console.WriteLine(inputKey);
            Thread.Sleep(16);
        }
    }
}
