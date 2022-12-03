using Hot.Core;
using System.CommandLine;

await Main();

async Task Main() {
    var rootCommand = new RootCommand();
    var helloCommand = new Command("hello", "Prints 'Hello, World!'");

    var msgOptions = new Option<string>(
        aliases: new[] { "-n", "--name" },
        description: "The name to print out. Defaults to 'World'.",
        getDefaultValue: () => "World"
    );

    async Task RunHelloCommand(string msg = "World") {
        HelloWorld.Print(msg);
        await Task.Delay(2000);
        HelloWorld.Print(msg);
    }

    rootCommand.Add(helloCommand);
    helloCommand.Add(msgOptions);
    helloCommand.SetHandler(RunHelloCommand, msgOptions);

    await rootCommand.InvokeAsync(args);
}






