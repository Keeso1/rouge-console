using System.Drawing;
using Microsoft.Extensions.Logging;
using RogueConsole.Core;
using RogueConsole.Enums;
using Sharpie;
using Sharpie.Backend;

//Define the path to the text file
string logFilePath = "console_log.txt";

//Create a StreamWriter to write logs to a text file
using StreamWriter logFileWriter = new StreamWriter(logFilePath, append: true);
GameSettings settings = new() { NumberOfRooms = 8 };

//Create an ILoggerFactory
ILoggerFactory loggerFactory = LoggerFactory.Create(builder =>
{
    //Add console output
    // builder.AddSimpleConsole(options =>
    // {
    //     options.IncludeScopes = true;
    //     options.SingleLine = true;
    //     options.TimestampFormat = "HH:mm:ss ";
    // });

    //Add a custom log provider to write logs to text files
    builder.AddProvider(new CustomFileLoggerProvider(logFileWriter));
});

//Create an ILogger
ILogger<Program> logger = loggerFactory.CreateLogger<Program>();

var terminal = new Terminal(
    CursesBackend.Load(),
    new TerminalOptions(UseColors: true, CaretMode: CaretMode.Invisible, UseMouse: false)
);

Canvas canvas = new(terminal.Screen.Size);

FloorLayout floor = new(logger);
for (var room = 0; room < settings.NumberOfRooms; room++)
{
    floor.Generate();
    logger.LogInformation("Rooms: {rooms}", FloorLayout.RoomsToString(floor.Rooms));
}
;

var game = new GameState(
    logger,
    new()
    {
        Attributes = VideoAttribute.Bold,
        ColorMixture = terminal.Colors.MixColors(StandardColor.Magenta, StandardColor.Black),
    },
    floor.Rooms
)
{
    Canvas = canvas,
};

GameState.PrevPosition = new(terminal.Screen.Size.Width / 2, terminal.Screen.Size.Height / 2);

terminal.Repeat(
    t =>
    {
        game.Canvas.DrawOnto(
            t.Screen,
            new Rectangle(new Point(0, 0), canvas.Size),
            new Point(0, 0)
        );
        t.Screen.Refresh();
        t.Screen.DrawBorder();
        return Task.CompletedTask;
    },
    50
);

terminal.Run(
    (Term, Tevent) =>
    {
        switch (Tevent)
        {
            case KeyEvent { Char.Value: 'q' }:
                Environment.Exit(0);
                break;
            case KeyEvent { Char.Value: 'h' }:
                game.Update(Direction.left);
                break;
            case KeyEvent { Char.Value: 'j' }:
                game.Update(Direction.down);
                break;
            case KeyEvent { Char.Value: 'k' }:
                game.Update(Direction.up);
                break;
            case KeyEvent { Char.Value: 'l' }:
                game.Update(Direction.right);
                break;
        }
        ;
        return Task.FromResult(true);
    }
);

// Customized ILoggerProvider, writes logs to text files
public class CustomFileLoggerProvider : ILoggerProvider
{
    private readonly StreamWriter _logFileWriter;

    public CustomFileLoggerProvider(StreamWriter logFileWriter)
    {
        _logFileWriter = logFileWriter ?? throw new ArgumentNullException(nameof(logFileWriter));
    }

    public ILogger CreateLogger(string categoryName)
    {
        return new CustomFileLogger(categoryName, _logFileWriter);
    }

    public void Dispose()
    {
        _logFileWriter.Dispose();
    }
}

// Customized ILogger, writes logs to text files
public class CustomFileLogger : ILogger
{
    private readonly string _categoryName;
    private readonly StreamWriter _logFileWriter;

    public CustomFileLogger(string categoryName, StreamWriter logFileWriter)
    {
        _categoryName = categoryName;
        _logFileWriter = logFileWriter;
    }

    public IDisposable? BeginScope<TState>(TState state)
        where TState : notnull
    {
        return null;
    }

    public bool IsEnabled(LogLevel logLevel)
    {
        // Ensure that only information level and higher logs are recorded
        return logLevel is not LogLevel.None && logLevel >= LogLevel.Information;
    }

    public void Log<TState>(
        LogLevel logLevel,
        EventId eventId,
        TState state,
        Exception? exception,
        Func<TState, Exception?, string> formatter
    )
    {
        // Ensure that only information level and higher logs are recorded
        if (!IsEnabled(logLevel))
        {
            return;
        }

        // Get the formatted log message
        var message = formatter(state, exception);

        //Write log messages to text file
        _logFileWriter.WriteLine($"[{logLevel}] [{_categoryName}] {message}");
        if (exception is not null)
        {
            _logFileWriter.WriteLine(exception);
        }
        _logFileWriter.Flush();
    }
}