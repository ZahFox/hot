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

public class Apt : PackageManagerTool, IExePackageManagerTool
{
  public override string Name => "apt";

  public override Exe.Exe Exe => new AptExe();

  public override async Task<string> Version()
  {
    // TODO: add error handling if run fails (create a custom exception type for this)
    var versionResult = await this.Exe.Run(new[] { "--version" });
    var version = versionResult.Stdout.Trim();
    var versionChunks = version.Split(' ');
    if (versionChunks.Length == 3)
    {
      return versionChunks[1];
    }


    // TODO: add error handling if the version is in an unexpected format (create a custom exception type for this)
    return version;
  }

  public override async Task<IEnumerable<string>> ListPackageNames()
  {
    var exe = new Exe.Exe
    {
      Name = "dpkg-query",
      Args = new[] { "--show", "--no-pager", "--showformat='${binary:Package}\n'" }
    };

    var packageNamesResult = await exe.Run();
    var packageNames = packageNamesResult.Stdout.Split(Environment.NewLine);
    return packageNames;
  }
}