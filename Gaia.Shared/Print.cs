namespace Gaia.Shared;

public static class Print
{
    public static void Error(string message)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write("e: ");
        Console.ResetColor();
        Console.WriteLine(message);
    }

    public static void Success(string message)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write("s: ");
        Console.ResetColor();
        Console.WriteLine(message);
    }

    public static void Warning(string message)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write("w: ");
        Console.ResetColor();
        Console.WriteLine(message);
    }

    public static void Informational(string message)
    {
        Console.ForegroundColor = ConsoleColor.Gray;
        Console.Write("i: ");
        Console.ResetColor();
        Console.WriteLine(message);
    } 
}
