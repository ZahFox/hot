namespace Hot.Core.Tool;

public interface ITool
{
  public abstract string Name { get; }
  public abstract Task<string> Version();
}

public interface IExeTool : ITool
{
  public abstract Exe.Exe Exe { get; }
}

public interface IPackageManagerTool : ITool
{
  public abstract Task<IEnumerable<string>> ListPackageNames();
}

public interface IExePackageManagerTool : IExeTool, IPackageManagerTool
{

}

public abstract class Tool : ITool
{
  public abstract string Name { get; }
  public abstract Task<string> Version();
}

public abstract class ExeTool : Tool, IExeTool
{
  public abstract Exe.Exe Exe { get; }
}

public abstract class PackageManagerTool : Tool, IPackageManagerTool
{
  public abstract Task<IEnumerable<string>> ListPackageNames();
}

public abstract class ExePackageManagerTool : PackageManagerTool, IExeTool
{
  public abstract Exe.Exe Exe { get; }
}
