# dotnet project init

This document demonstrates one way to set up a new dotnet "solution" which includes multiple projects, including a class library and cli.

```sh
dotnet new sln --name Hot

dotnet new classlib -n Hot.Core

dotnet sln add Hot.Core/Hot.Core.csproj

dotnet new console -n Hot.Cli

dotnet add Hot.Cli/Hot.Cli.csproj \
  reference Hot.Core/Hot.Core.csproj

dotnet add Hot.Cli \
  package System.CommandLine --version 2.0.0-beta4.22272.1

dotnet sln add Hot.Cli/Hot.Cli.csproj
```
