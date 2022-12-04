#!/usr/bin/env bash

case $1 in
   build)
      dotnet build ./src/Hot.Cli/Hot.Cli.csproj
      ;;
   start)
      ./src/Hot.Cli/bin/Debug/net7.0/hot "${@:2}"
      ;;
   run)
      dotnet build ./src/Hot.Cli/Hot.Cli.csproj
      ./src/Hot.Cli/bin/Debug/net7.0/hot "${@:2}"
      ;;
   *)
     printf "invalid command: $1\n"
     ;;
esac
