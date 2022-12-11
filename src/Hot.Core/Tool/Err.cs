namespace Hot.Core.Tool;

using Hot.Core.Err;

public abstract class HotToolException : HotCoreException
{
  public HotToolException()
  {
  }

  public HotToolException(string message)
      : base(message)
  {
  }

  public HotToolException(string message, Exception inner)
      : base(message, inner)
  {
  }
}

public class MissingToolException : HotToolException
{
  public MissingToolException(string name)
      : base($"failed to find tool: \"{name}\"")
  {
  }
}

public class InvalidVersionFormatException : HotToolException
{
  public InvalidVersionFormatException(ITool tool, string versionOuput)
      : base($"tool: \"{tool.Name}\" produced an invalid version output: \"{versionOuput}\"")
  {
  }
}