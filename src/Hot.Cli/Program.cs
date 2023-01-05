using System.CommandLine;
using Hot.Core.Tool;
using Hot.Core.Tool.Shell;
using Hot.Core.Host;

await Main();

async Task Main()
{
  ConfigureTools();
  var cli = ConfigureCli();
  await cli.InvokeAsync(args);
}

static void ConfigureTools()
{
  // TODO: These will eventually be part of a host/environment configuration
  ToolBag.AddTool(new NodeJs());
  ToolBag.AddTool(new Apt());
  ToolBag.AddTool(new Git());
  ToolBag.AddTool(new Sh());
}

static Command ConfigureCli()
{
  var rootCommand = new RootCommand();
  var toolCommand = new Command("tool", "Use one of the common system tools.");
  var hostCommand = new Command("host", "Gather information about the host.");

  var msgOption = new Option<string>(
      aliases: new[] { "-n", "--name" },
      description: "The name to print out. Defaults to 'World'.",
      getDefaultValue: () => "World"
  );

  var exeNameArg = new Argument<string>
      (name: "name",
      description: "The name of an executable.",
      getDefaultValue: () => "");

  var exeArgsArg = new Argument<List<string>>(
      name: "args",
      description: "Arguments to pass to the executable.",
      getDefaultValue: () => new() { }
  )
  { Arity = ArgumentArity.ZeroOrMore };

  var toolNameArg = new Argument<string>
      (name: "name",
      description: "The name of a system tool.",
      getDefaultValue: () => "");

  var toolArgsArg = new Argument<List<string>>(
      name: "args",
      description: "Arguments to pass to the system tool.",
      getDefaultValue: () => new() { }
  )
  { Arity = ArgumentArity.ZeroOrMore };

  async Task RunToolCommand(string name, List<string> args)
  {
    try
    {
      var tool = ToolBag.GetTool<IExeTool>(name);
      var version = await tool.Version();
      Console.WriteLine($"Name:     {tool.Name}");
      Console.WriteLine($"Version:  {version}");
      Console.WriteLine($"Path:     {tool.Exe.Path}");
    }
    catch (MissingToolException err)
    {
      Console.Error.WriteLine(err.Message);
      Environment.Exit(1);
    }
    catch (Exception err)
    {
      Console.Error.WriteLine("unexpected error", err);
      Environment.Exit(1);
    }
  }

  void RunHostCommand()
  {
    try
    {
      OS.PrintTargetRuntime();
    }
    catch (Exception err)
    {
      Console.Error.WriteLine("unexpected error", err);
      Environment.Exit(1);
    }
  }

  rootCommand.Add(toolCommand);
  rootCommand.Add(hostCommand);

  toolCommand.Add(toolNameArg);
  toolCommand.Add(toolArgsArg);
  toolCommand.SetHandler(RunToolCommand, toolNameArg, toolArgsArg);

  hostCommand.SetHandler(RunHostCommand);

  return rootCommand;
}
