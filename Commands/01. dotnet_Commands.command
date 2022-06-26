***********************************************
## Displays installed dotnet information
dotnet --info

## Dotnet help
dotnet -h

## Displays dotnet new project template list
dotnet new --list

## Build dotnet project
dotnet build

## Build and run dotnet project
dotnet run

## Run dotnet application from Google cloud shell
dotnet run --urls=http://localhost:8080

## How to launch ASP.NET Core app from Google Cloud Shell : 
## https://codelabs.developers.google.com/codelabs/cloud-aspnetcore-cloudshell#0

## Run test in dotnet project with filter
dotnet test --filter FullyQualifiedName!~IntegrationTests
