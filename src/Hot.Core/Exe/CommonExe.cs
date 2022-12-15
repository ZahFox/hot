namespace Hot.Core.Exe;

public class NodeExe : Exe
{
  public override string Name => "node";
}

public class GitExe : Exe
{
  public override string Name => "git";
}

public class AptExe : Exe
{
  public override string Name => "apt";
}

public class ShExe : Exe
{
  public override string Name => "sh";
}

public class BashExe : Exe
{
  public override string Name => "bash";
}

public class ZshExe : Exe
{
  public override string Name => "zsh";
}
