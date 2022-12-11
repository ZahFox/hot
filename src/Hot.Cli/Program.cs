using Bullseye;
using System.CommandLine;
using Hot.Core;
using Hot.Core.IO;
using Hot.Core.Exe;
using Hot.Core.Tool;

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
}

static Command ConfigureCli()
{
  var rootCommand = new RootCommand();
  var helloCommand = new Command("hello", "Prints 'Hello, World!'");
  var tasksCommand = new Command("tasks", "Executes a graph of async tasks.");
  var exeCommand = new Command("exe", "Run an executable file.");
  var toolCommand = new Command("tool", "Use one of the common system tools.");

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

  async Task RunHelloCommand(string msg = "World")
  {
    HelloWorld.Print(msg);
    await Task.Delay(2000);
    HelloWorld.Print(msg);
  }

  async Task RunTasksCommand()
  {
    var targets = new Targets();

    targets.Add(
        "print1",
        async () => await System.Console.Out.WriteLineAsync("111")
    );

    targets.Add(
        "print2",
        async () => await System.Console.Out.WriteLineAsync("222")
    );

    targets.Add("default", dependsOn: new[] { "print1", "print2" });
    await targets.RunWithoutExitingAsync(new[] { "default" }, outputWriter: new VoidTextWriter());
  }

  async Task RunExeCommand(string name, List<string> args)
  {
    var exe = new Exe { Name = name, Args = args.AsReadOnly() };
    var result = await exe.Run();
    Console.Write(result.Stdout);
  }

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

  rootCommand.Add(helloCommand);
  rootCommand.Add(tasksCommand);
  rootCommand.Add(exeCommand);
  rootCommand.Add(toolCommand);

  helloCommand.Add(msgOption);
  exeCommand.Add(exeNameArg);
  exeCommand.Add(exeArgsArg);
  toolCommand.Add(toolNameArg);
  toolCommand.Add(toolArgsArg);

  helloCommand.SetHandler(RunHelloCommand, msgOption);
  tasksCommand.SetHandler(RunTasksCommand);
  exeCommand.SetHandler(RunExeCommand, exeNameArg, exeArgsArg);
  toolCommand.SetHandler(RunToolCommand, toolNameArg, toolArgsArg);

  return rootCommand;
}
