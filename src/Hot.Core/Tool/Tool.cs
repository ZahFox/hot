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

public abstract class Tool : ITool
{
  public abstract string Name { get; }
  public abstract Task<string> Version();
}

public abstract class ExeTool : Tool, IExeTool
{
  public abstract Exe.Exe Exe { get; }
}