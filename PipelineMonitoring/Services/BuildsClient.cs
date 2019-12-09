using PipelineMonitoring.Model.Builds;
using PipelineMonitoring.Model.Common;
using Microsoft.AspNetCore.Components;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace PipelineMonitoring.Services
{
    public class BuildsClient
    {
        private readonly AzureDevOpsSettingsService _azureDevOpsSettingsService;
        private readonly HttpClient _httpClient;

        public BuildsClient(
            AzureDevOpsSettingsService azureDevOpsSettingsService,
            HttpClient httpClient)
        {
            _azureDevOpsSettingsService = azureDevOpsSettingsService;
            _httpClient = httpClient;
        }

        public async Task<Build[]> GetBuilds(FilterCriteria filterCriteria)
        {
            if (!_azureDevOpsSettingsService.HasOrganisationAndProject)
            {
                return null;
            }

            var buildList = await _httpClient
                .GetJsonAsync<BuildList>(
                    $"https://dev.azure.com/{_azureDevOpsSettingsService.Organisation}/{_azureDevOpsSettingsService.Project}/_apis/build/builds?api-version=5.0&branch=master&maxBuildsPerDefinition=1");

            return filterCriteria.ShowAll
                ? buildList.Value
                : buildList.Value.Where(b => b.Result != Build.SucceededResult).ToArray();
        }
    }
}
