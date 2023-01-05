using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using Hot.Core.IO;

namespace Hot.Core.Host;

public enum OperatingSystem
{
  UNKNOWN = -1,
  LINUX = 0,
  WINDOWS = 1,
  MACOS = 2,
}

public static class OS
{
  public static OperatingSystem GetOperatingSystem()
  {
    if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
    {
      return OperatingSystem.LINUX;
    }
    else if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
    {
      return OperatingSystem.WINDOWS;
    }
    else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
    {
      return OperatingSystem.MACOS;
    }
    return OperatingSystem.UNKNOWN;
  }

  public static string GetOperatingSystemLabel() =>
     GetOperatingSystem() switch
     {
       OperatingSystem.LINUX => "linux",
       OperatingSystem.WINDOWS => "windows",
       OperatingSystem.MACOS => "macos",
       OperatingSystem.UNKNOWN => "unknown",
       _ => throw new ArgumentOutOfRangeException(),
     };

  public static void PrintTargetRuntime()
  {
    var framework = Assembly
            .GetEntryAssembly()?
            .GetCustomAttribute<TargetFrameworkAttribute>()?
            .FrameworkName;

    var stats = new
    {
      OsPlatform = System.Runtime.InteropServices.RuntimeInformation.OSDescription,
      OSArchitecture = System.Runtime.InteropServices.RuntimeInformation.OSArchitecture,
      ProcesArchitecture = System.Runtime.InteropServices.RuntimeInformation.ProcessArchitecture,
    };

    Log.Out.BlueWrite("(ℹ️)");
    Log.Out.GreenWrite(" Framework Version:\t");
    Log.Out.BlueWriteLine(framework);


    Log.Out.BlueWrite("(ℹ️)");
    Log.Out.GreenWrite(" OS:\t\t\t");
    Log.Out.BlueWriteLine(GetOperatingSystemLabel());

    Log.Out.BlueWrite("(ℹ️)");
    Log.Out.GreenWrite(" OS Platform:\t");
    Log.Out.BlueWriteLine(stats.OsPlatform);

    Log.Out.BlueWrite("(ℹ️)");
    Log.Out.GreenWrite(" OS Architecture:\t");
    Log.Out.BlueWriteLine(stats.OSArchitecture.ToString());
  }

  public static void ColorWrite(string? message, ConsoleColor color)
  {
    Console.ForegroundColor = color;
    Console.Write(message);
    Console.ResetColor();
  }

  public static void ColorWriteLine(string? message, ConsoleColor color)
  {
    Console.ForegroundColor = color;
    Console.WriteLine(message);
    Console.ResetColor();
  }
}

