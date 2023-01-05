namespace Hot.Core.IO;

public static class Log
{
  public static class Out
  {
    public static void ColorWrite(string? message, ConsoleColor color)
    {
      ColorWriter.Write(message, color, Console.Out);
    }

    public static void ColorWriteLine(string? message, ConsoleColor color)
    {
      ColorWriter.WriteLine(message, color, Console.Out);
    }

    public static void DarkRedWrite(string? message)
    {
      ColorWrite(message, ConsoleColor.Red);
    }

    public static void DarkRedWriteLine(string? message)
    {
      ColorWriteLine(message, ConsoleColor.Red);
    }

    public static void GreenWrite(string? message)
    {
      ColorWrite(message, ConsoleColor.Green);
    }

    public static void GreenWriteLine(string? message)
    {
      ColorWriteLine(message, ConsoleColor.Green);
    }

    public static void BlueWrite(string? message)
    {
      ColorWrite(message, ConsoleColor.Blue);
    }

    public static void BlueWriteLine(string? message)
    {
      ColorWriteLine(message, ConsoleColor.Blue);
    }
  }

  public static class Error
  {
    public static void ColorWrite(string? message, ConsoleColor color)
    {
      ColorWriter.Write(message, color, Console.Error);
    }

    public static void ColorWriteLine(string? message, ConsoleColor color)
    {
      ColorWriter.WriteLine(message, color, Console.Error);
    }

    public static void DarkRedWrite(string? message)
    {
      ColorWrite(message, ConsoleColor.DarkRed);
    }

    public static void DarkRedWriteLine(string? message)
    {
      ColorWriteLine(message, ConsoleColor.DarkRed);
    }

    public static void GreenWrite(string? message)
    {
      ColorWrite(message, ConsoleColor.Green);
    }

    public static void GreenWriteLine(string? message)
    {
      ColorWriteLine(message, ConsoleColor.Green);
    }

    public static void BlueWrite(string? message)
    {
      ColorWrite(message, ConsoleColor.Blue);
    }

    public static void BlueWriteLine(string? message)
    {
      ColorWriteLine(message, ConsoleColor.Blue);
    }
  }
}

static class ColorWriter
{
  public static void Write(string? message, ConsoleColor color, TextWriter writer)
  {
    Console.ForegroundColor = color;
    writer.Write(message);
    Console.ResetColor();
  }

  public static void WriteLine(string? message, ConsoleColor color, TextWriter writer)
  {
    Console.ForegroundColor = color;
    writer.WriteLine(message);
    Console.ResetColor();
  }
}