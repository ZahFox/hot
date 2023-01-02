param([string] $command = "")

switch ($command) {
  build {
    dotnet build ./src/Hot.Cli/Hot.Cli.csproj
  }
  start {
    ./src/Hot.Cli/bin/Debug/net7.0/hot $args
  }
  run {
    dotnet build ./src/Hot.Cli/Hot.Cli.csproj
    ./src/Hot.Cli/bin/Debug/net7.0/hot $args
  }
  default {
    Write-Output "invalid command: $command"
  }
}
