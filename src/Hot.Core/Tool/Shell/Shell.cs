using Hot.Core.Exe;

namespace Hot.Core.Tool.Shell;

public interface IShellTool : ITool
{
  public abstract Task<string> RunCommand(IEnumerable<string> args);
}

public interface IExeShellTool : IExeTool, IShellTool
{
}

public abstract class ShellTool : Tool, IShellTool
{
  public abstract Task<string> RunCommand(IEnumerable<string> args);
}

public abstract class ExeShellTool : ShellTool, IExeShellTool
{
  public abstract Exe.Exe Exe { get; }
}
