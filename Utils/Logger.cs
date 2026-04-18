using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System.Runtime.CompilerServices;

namespace Vimonia.Utils;

public static class Log {

    private static StreamWriter? _file;
    private static readonly Lock _sync = new();

    public static void Init(string filePath) {
        _file?.Dispose();
        _file = new StreamWriter(filePath, append: true) { AutoFlush = true };
    }

    private static StreamWriter File => _file ?? throw new InvalidOperationException($"Logger not initialized");


    private static void Write(string level, string member, string file, int line, string msg, Exception? ex = null) {
        var prefix = $"{DateTime.Now:HH:mm:ss} [{level}] [{Path.GetFileName(file)}:{line} {member}] {msg}";
        lock (_sync) {
            if (ex != null) {
                File.WriteLine(prefix);
                File.WriteLine(ex);
            } else {
                File.WriteLine(prefix);
            }
        }
    }

    public static void Info(string msg,
        [CallerMemberName] string member = "",
        [CallerFilePath] string file = "",
        [CallerLineNumber] int line = 0) {

        Write("INFO", member, file, line, msg);
    }

    public static void Warn(string msg,
        [CallerMemberName] string member = "",
        [CallerFilePath] string file = "",
        [CallerLineNumber] int line = 0) {

        Write("WARN", member, file, line, msg);
    }

    public static void Error(string msg,
        [CallerMemberName] string member = "",
        [CallerFilePath] string file = "",
        [CallerLineNumber] int line = 0) {

        Write("ERROR", member, file, line, msg);
    }

    public static void Error(string msg,
        Exception error,
        [CallerMemberName] string member = "",
        [CallerFilePath] string file = "",
        [CallerLineNumber] int line = 0) {

        Write("ERROR", member, file, line, msg, error);
    }

    public static void Shutdown() => _file?.Dispose();
}
