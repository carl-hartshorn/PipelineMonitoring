# [Pipeline Monitoring](https://pipeline-monitoring.web.app)

[![Build Status](https://dev.azure.com/carlhartshorn/PipelineMonitoring/_apis/build/status/carl-hartshorn.PipelineMonitoring%20CI?branchName=master)](https://dev.azure.com/carlhartshorn/PipelineMonitoring/_build/latest?definitionId=4&branchName=master)

A simple Blazor application to list Azure DevOps Builds and Releases using a Personal Access Token.

Born out of the need for an at-a-glance dashboard view of microservice builds and releases.

Given an Azure DevOps Organisation, Project and Personal Access Token, the Pipeline Monitoring site will display (by default) the failed or in progress builds and releases.

## Getting Started

See [Get started with Blazor](https://docs.microsoft.com/en-gb/aspnet/core/client-side/spa/blazor/get-started?view=aspnetcore-3.0&tabs=visual-studio).

## Local Development

The solution can be run locally using Visual Studio 2019 **Preview**.

## Build and Release

The site is published using the `dotnet publish -c Release` command.
The site is built, published and deployed by the CI pipeline described in [azure-pipelines.ci.yml](/azure-pipelines.ci.yml).

## Future Direction

When the Azure DevOps REST API has been updated to provide stage information for the new pipelines feature, this site should be updated to display the stages under a build (as it currently does for releases).