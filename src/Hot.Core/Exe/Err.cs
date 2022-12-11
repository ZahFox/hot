namespace Hot.Core.Exe;

using Hot.Core.Err;

public abstract class HotExeException : HotCoreException
{
  public HotExeException()
  {
  }

  public HotExeException(string message)
      : base(message)
  {
  }

  public HotExeException(string message, Exception inner)
      : base(message, inner)
  {
  }
}

public class ExeRunFailedException : HotExeException
{
  public ExeRunFailedException(IExe exe)
      : base($"failed to run exe: \"{exe.Name}\"")
  {
  }
}