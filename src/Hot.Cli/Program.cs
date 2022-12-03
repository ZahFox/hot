using Bullseye;
using System.CommandLine;
using Hot.Core;
using Hot.Core.IO;

await Main();

async Task Main()
{
    var rootCommand = new RootCommand();
    var helloCommand = new Command("hello", "Prints 'Hello, World!'");
    var tasksCommand = new Command("tasks", "Executes a graph of async tasks.");

    var msgOptions = new Option<string>(
        aliases: new[] { "-n", "--name" },
        description: "The name to print out. Defaults to 'World'.",
        getDefaultValue: () => "World"
    );

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

    rootCommand.Add(helloCommand);
    rootCommand.Add(tasksCommand);
    helloCommand.Add(msgOptions);
    helloCommand.SetHandler(RunHelloCommand, msgOptions);
    tasksCommand.SetHandler(RunTasksCommand);
    await rootCommand.InvokeAsync(args);
}
