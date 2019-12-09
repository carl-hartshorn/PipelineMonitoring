using PipelineMonitoring.Services;
using Microsoft.AspNetCore.Components.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace PipelineMonitoring
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddSingleton<LocalStorageService>()
                .AddSingleton<PersonalAccessTokenService>()
                .AddSingleton<AzureDevOpsSettingsService>()
                .AddSingleton<EventService>()
                .AddSingleton<BuildsClient>()
                .AddSingleton<ReleasesClient>();
        }

        public void Configure(IComponentsApplicationBuilder app)
        {
            app.AddComponent<App>("app");
        }
    }
}
