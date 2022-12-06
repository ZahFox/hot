using Hot.Core.Exe;

namespace Hot.Core.Tool;

public class NodeJs : ExeTool
{
  public override string Name => "node";

  public override Exe.Exe Exe => new NodeExe();

  public override async Task<string> Version()
  {
    // TODO: add error handling if run fails (create a custom exception type for this)
    var versionResult = await this.Exe.Run(new[] { "--version" });
    var version = versionResult.Stdout.Trim();
    return version;
  }
}