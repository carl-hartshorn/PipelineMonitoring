using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using PipelineMonitoring.Services;
using System.Net.Http;
using System.Threading.Tasks;

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