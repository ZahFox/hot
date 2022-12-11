using Hot.Core.Exe;

namespace Hot.Core.Tool;

public class NodeJs : ExeTool
{
  public override string Name => "node";

  public override Exe.Exe Exe => new NodeExe();

  public override async Task<string> Version()
  {
    var versionResult = await this.Exe.Run(new[] { "--version" }, new ExeRunOpts(Throw: true));
    var version = versionResult.Stdout.Trim();
    return version;
  }
}

public class Git : ExeTool
{
  public override string Name => "git";

  public override Exe.Exe Exe => new GitExe();

  public override async Task<string> Version()
  {
    var versionResult = await this.Exe.Run(new[] { "--version" }, new ExeRunOpts(Throw: true));
    var version = versionResult.Stdout.Trim();
    var versionChunks = version.Split(' ');
    if (versionChunks.Length != 3)
    {
      throw new InvalidVersionFormatException(this, version);
    }

    return versionChunks[2];
  }
}

public class Apt : PackageManagerTool, IExePackageManagerTool
{
  public override string Name => "apt";

  public override Exe.Exe Exe => new AptExe();

  public override async Task<string> Version()
  {
    var versionResult = await this.Exe.Run(new[] { "--version" }, new ExeRunOpts(Throw: true));
    var version = versionResult.Stdout.Trim();
    var versionChunks = version.Split(' ');
    if (versionChunks.Length != 3)
    {
      throw new InvalidVersionFormatException(this, version);
    }

    return versionChunks[1];
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