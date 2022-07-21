using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using PipelineMonitoring.Services;

namespace PipelineMonitoring;

public static class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebAssemblyHostBuilder.CreateDefault(args);

        builder
            .Services
            .AddSingleton<HttpClient>()
            .AddSingleton<LocalStorageService>()
            .AddSingleton<PersonalAccessTokenService>()
            .AddSingleton<AzureDevOpsSettingsService>()
            .AddSingleton<EventService>()
            .AddSingleton<BuildsClient>()
            .AddSingleton<ReleasesClient>();

        builder.RootComponents.Add<App>("#app");

        await builder.Build().RunAsync().ConfigureAwait(false);
    }
}