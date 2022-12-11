namespace Hot.Core.Err;

public abstract class HotException : Exception
{
  public HotException()
  {
  }

  public HotException(string message)
      : base(message)
  {
  }

  public HotException(string message, Exception inner)
      : base(message, inner)
  {
  }
}

public abstract class HotCoreException : HotException
{
  public HotCoreException()
  {
  }

  public HotCoreException(string message)
      : base(message)
  {
  }

  public HotCoreException(string message, Exception inner)
      : base(message, inner)
  {
  }
}