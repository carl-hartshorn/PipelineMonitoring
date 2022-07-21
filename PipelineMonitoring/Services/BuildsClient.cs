using PipelineMonitoring.AzureDevOps.Builds;
using System.Net.Http.Json;
using System.Text.Json;

namespace PipelineMonitoring.Services;

internal class BuildsClient
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
            return Array.Empty<Build>();
        }

        var buildList = await _httpClient
            .GetFromJsonAsync<BuildList>(
                $"https://dev.azure.com/{_azureDevOpsSettingsService.Organisation}/{_azureDevOpsSettingsService.Project}/_apis/build/builds?api-version=5.0&maxBuildsPerDefinition=1&queryOrder=startTimeDescending")
            .ConfigureAwait(false);

        if (buildList is null)
        {
            throw new JsonException($"Failed to deserialise {nameof(BuildList)}");
        }
        
        return filterCriteria.ShowAll
            ? buildList.Value.ToArray()
            : buildList.Value.Where(b => b.Result != BuildResult.Succeeded).ToArray();
    }
}