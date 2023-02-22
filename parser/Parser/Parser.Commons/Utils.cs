using System.Web;

namespace Parser.Commons;

public static class Utils
{
    public static string EscapeString(this string? toEscape) => string.IsNullOrEmpty(toEscape) ? string.Empty : HttpUtility.HtmlDecode(toEscape.Replace("\n", string.Empty).Replace("\r", string.Empty).Replace("\t", string.Empty));

    public static void WriteOutcome(string message, bool succeed = true)
    {
        Console.ForegroundColor = succeed ? ConsoleColor.Green : ConsoleColor.Red;
        Console.WriteLine(message);

        Console.ForegroundColor = ConsoleColor.White;

    }

    private static readonly object @lock = new object();

    public static void Log(string identifier)
    {
        lock (@lock)
        {
            File.AppendAllLines("log.txt", new[] { $"File with identifier {identifier} failed. " });
        }
    }
}