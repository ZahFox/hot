using Hot.Core.Exe;

namespace Hot.Core.Tool.Shell;

public class Sh : ExeShellTool
{
  public override string Name => "sh";

  public override Exe.Exe Exe => new ShExe();

  public override async Task<string> Version()
  {
    var versionResult = await this.Exe.Run(new[] { "--version" }, new ExeRunOpts(Throw: true));
    var version = versionResult.Stdout.Trim();
    var versionLines = version.Split(Environment.NewLine);
    if (versionLines.Length < 1)
    {
      throw new InvalidVersionFormatException(this, version);
    }


    var versionLine = versionLines[0];
    var versionChunks = versionLine.Split(' ');
    if (versionChunks.Length < 4)
    {
      throw new InvalidVersionFormatException(this, versionLine);
    }

    return versionChunks[3];
  }

  public override async Task<string> RunCommand(IEnumerable<string> args)
  {
    var commandArg = '"' + String.Join(' ', args) + '"';
    var commandResult = await this.Exe.Run(new[] { "-c", commandArg }, new ExeRunOpts(Throw: true));
    return commandResult.IsSuccess ? commandResult.Stdout : commandResult.Stderr;
  }
}