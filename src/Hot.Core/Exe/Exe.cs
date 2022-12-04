using System.Diagnostics;
using System.Text;

namespace Hot.Core.Exe;

public interface IExe
{
    public string Name { get; init; }
    public IList<string> Args { get; init; }
}


public class Exe : IExe
{
    public string Name { get; init; } = "";
    public IList<string> Args { get; init; } = new List<string>().AsReadOnly();
}

public class ExeResult
{
    public string Stdout { get; init; } = "";
    public string Stderr { get; init; } = "";
    public int ExitCode { get; init; }
}


public static class ExeUtils
{
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
        var exitCode = 1;

        try
        {
            using (var exeProcess = Process.Start(startInfo))
            {
                if (exeProcess != null)
                {
                    exeProcess.OutputDataReceived += (sender, args) =>
                         { stdout.Append(args.Data); if (args.Data != null) stdout.Append('\n'); };
                    exeProcess.ErrorDataReceived += (sender, args) =>
                        { stderr.Append(args.Data); if (args.Data != null) stderr.Append('\n'); };
                    exeProcess.BeginOutputReadLine();
                    exeProcess.WaitForExit();
                    exitCode = exeProcess.ExitCode;
                }
            }
        }
        catch (Exception err)
        {
            await Console.Error.WriteLineAsync(err.Message);
        }

        return new ExeResult
        {
            Stdout = stdout.ToString(),
            Stderr = stderr.ToString(),
            ExitCode = exitCode
        };
    }
}