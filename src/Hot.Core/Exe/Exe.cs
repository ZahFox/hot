using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace Hot.Core.Exe;

public readonly record struct ExeRunOpts(bool Throw = false);

public interface IExe
{
  public string Name { get; init; }
  public IEnumerable<string> Args { get; init; }
  public ExeRunOpts Opts { get; init; }
  public Task<ExeResult> Run();
  public Task<ExeResult> Run(ExeRunOpts opts);
  public Task<ExeResult> Run(IEnumerable<string> args);
  public Task<ExeResult> Run(IEnumerable<string> args, ExeRunOpts opts);
  public string Path { get; }
}


public class Exe : IExe
{
  public virtual string Name { get; init; } = "";
  public virtual IEnumerable<string> Args { get; init; } = new List<string>().AsReadOnly();

  public virtual ExeRunOpts Opts { get; init; }

  public string Path => ExeUtils.FindExePath(this.Name);


  public Task<ExeResult> Run()
  {
    return ExeUtils.RunExe(this);
  }

  public Task<ExeResult> Run(ExeRunOpts opts)
  {
    var exe = new Exe { Name = this.Name, Opts = opts };
    return ExeUtils.RunExe(exe);
  }

  public Task<ExeResult> Run(IEnumerable<string> args)
  {
    var exe = new Exe { Name = this.Name, Args = args };
    return ExeUtils.RunExe(exe);
  }

  public Task<ExeResult> Run(IEnumerable<string> args, ExeRunOpts opts)
  {
    var exe = new Exe { Name = this.Name, Args = args, Opts = opts };
    return ExeUtils.RunExe(exe);
  }
}

public enum ExeExitCode
{
  UNKNOWN = -1,
  SUCCESS = 0,
  GENERAL_ERROR = 1,
  COMMAND_NOT_FOUND = 127,
}

public class ExeResult
{
  public string Stdout { get; init; } = "";
  public string Stderr { get; init; } = "";
  public ExeExitCode ExitCode { get; init; } = ExeExitCode.UNKNOWN;

  public bool IsSuccess => ExitCode == ExeExitCode.SUCCESS;
}


static class ExeUtils
{
  private static string ExePathDelimeter = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? ";" : ":";

  /// <summary>
  /// Expands environment variables and, if unqualified, locates the exe in the working directory
  /// or the evironment's path.
  /// </summary>
  /// <param name="exe">The name of the executable file.</param>
  /// <returns>The fully-qualified path to the file.</returns>
  /// <exception cref="System.IO.FileNotFoundException">Raised when the exe was not found.</exception>
  public static string FindExePath(string exe)
  {
    exe = Environment.ExpandEnvironmentVariables(exe);
    if (!File.Exists(exe))
    {
      if (Path.GetDirectoryName(exe) == String.Empty)
      {
        foreach (string test in (Environment.GetEnvironmentVariable("PATH") ?? "").Split(ExePathDelimeter))
        {
          string path = test.Trim();
          if (!String.IsNullOrEmpty(path) && File.Exists(path = Path.Combine(path, exe)))
            return Path.GetFullPath(path);
        }
      }
      throw new FileNotFoundException(new FileNotFoundException().Message, exe);
    }
    return Path.GetFullPath(exe);
  }

  public static async Task<ExeResult> RunExe(IExe exe)
  {
    ProcessStartInfo startInfo = new ProcessStartInfo();
    startInfo.CreateNoWindow = false;
    startInfo.UseShellExecute = false;
    startInfo.FileName = exe.Name;
    startInfo.RedirectStandardOutput = true;
    startInfo.RedirectStandardError = true;
    startInfo.WindowStyle = ProcessWindowStyle.Hidden;
    startInfo.Arguments = String.Join(" ", exe.Args.ToArray());

    var stdout = new StringBuilder();
    var stderr = new StringBuilder();
    var exitCodeValue = 1;

    try
    {
      using var exeProcess = Process.Start(startInfo);
      if (exeProcess != null)
      {
        exeProcess.OutputDataReceived += (sender, args) =>
             { stdout.Append(args.Data); if (args.Data != null) stdout.Append(Environment.NewLine); };
        exeProcess.ErrorDataReceived += (sender, args) =>
            { stderr.Append(args.Data); if (args.Data != null) stderr.Append(Environment.NewLine); };
        exeProcess.BeginOutputReadLine();
        exeProcess.WaitForExit();
        exitCodeValue = exeProcess.ExitCode;
      }
    }
    catch (Exception err)
    {
      // TODO: Find out which errors to handle here
      await Console.Error.WriteLineAsync(err.Message);
    }

    // TODO: Consider interpreting this differently based on OS also,
    //       this part may need to be made as overrideable method if there
    //       ends up being an exe which doesn't conform to usual standards.
    var exitCode = exitCodeValue switch
    {
      0 => ExeExitCode.SUCCESS,
      1 => ExeExitCode.GENERAL_ERROR,
      127 => ExeExitCode.COMMAND_NOT_FOUND,
      _ => ExeExitCode.UNKNOWN
    };

    if (exitCode != ExeExitCode.SUCCESS && exe.Opts.Throw)
    {
      throw new ExeRunFailedException(exe);
    }

    return new ExeResult
    {
      Stdout = stdout.ToString(),
      Stderr = stderr.ToString(),
      ExitCode = exitCode
    };
  }
}