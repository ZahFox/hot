using Bullseye;
using System.CommandLine;
using Hot.Core;
using Hot.Core.IO;
using Hot.Core.Exe;

await Main();

async Task Main()
{
    var rootCommand = new RootCommand();
    var helloCommand = new Command("hello", "Prints 'Hello, World!'");
    var tasksCommand = new Command("tasks", "Executes a graph of async tasks.");
    var exeCommand = new Command("exe", "Run an executable file.");

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
        var result = await ExeUtils.RunExe(exe);
        Console.Write(result.Stdout);
    }

    rootCommand.Add(helloCommand);
    rootCommand.Add(tasksCommand);
    rootCommand.Add(exeCommand);
    helloCommand.Add(msgOption);
    exeCommand.Add(exeNameArg);
    exeCommand.Add(exeArgsArg);
    helloCommand.SetHandler(RunHelloCommand, msgOption);
    tasksCommand.SetHandler(RunTasksCommand);
    exeCommand.SetHandler(RunExeCommand, exeNameArg, exeArgsArg);
    await rootCommand.InvokeAsync(args);
}
