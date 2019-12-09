# Pipeline Monitoring

A simple Blazor application to list Azure DevOps Builds and Releases using a Personal Access Token.

Born out of the need for an at-a-glance dashboard view of microservice builds and releases.

Given an Azure DevOps Organisation, Project and Personal Access Token, the Pipeline Monitoring site will display (by default) the failed or in progress builds and releases.

## Getting Started

See [Get started with Blazor](https://docs.microsoft.com/en-gb/aspnet/core/client-side/spa/blazor/get-started?view=aspnetcore-3.0&tabs=visual-studio).

## Local Development

The solution can be run locally using Visual Studio 2019 **Preview**.

## Build

The site is published using the `dotnet publish -c Release` command.

## Future Direction

When the Azure DevOps REST API has been updated to provide stage information for the new pipelines feature, this site should be updated to display the stages under a build (as it currently does for releases).