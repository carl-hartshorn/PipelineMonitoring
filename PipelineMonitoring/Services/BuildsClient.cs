using PipelineMonitoring.AzureDevOps.Builds;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PipelineMonitoring.Services;

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
            .GetFromJsonAsync<BuildList>(
                $"https://dev.azure.com/{_azureDevOpsSettingsService.Organisation}/{_azureDevOpsSettingsService.Project}/_apis/build/builds?api-version=5.0&maxBuildsPerDefinition=1&queryOrder=startTimeDescending")
            .ConfigureAwait(false);

        return (filterCriteria?.ShowAll ?? false)
            ? buildList.Value.ToArray()
            : buildList.Value.Where(b => b.Result != BuildResult.Succeeded).ToArray();
    }
}