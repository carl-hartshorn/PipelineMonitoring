using PipelineMonitoring.AzureDevOps.Releases;
using System.Net.Http.Json;
using System.Text.Json;

namespace PipelineMonitoring.Services;

internal class ReleasesClient
{
    private readonly AzureDevOpsSettingsService _azureDevOpsSettingsService;
    private readonly HttpClient _httpClient;

    public ReleasesClient(
        AzureDevOpsSettingsService azureDevOpsSettingsService,
        HttpClient httpClient)
    {
        _azureDevOpsSettingsService = azureDevOpsSettingsService;
        _httpClient = httpClient;
    }

    public async Task<Release[]> GetReleases(FilterCriteria filterCriteria)
    {
        if (!_azureDevOpsSettingsService.HasOrganisationAndProject)
        {
            return Array.Empty<Release>();
        }

        var releaseList = await _httpClient
            .GetFromJsonAsync<ReleaseList>(
                $"https://vsrm.dev.azure.com/{_azureDevOpsSettingsService.Organisation}/{_azureDevOpsSettingsService.Project}/_apis/release/releases?api-version=5.0&$expand=environments")
            .ConfigureAwait(false);

        if (releaseList is null)
        {
            throw new JsonException($"Failed to deserialise {nameof(ReleaseList)}");
        }
        
        return filterCriteria.ShowAll
            ? releaseList.MostRecentReleasesByDefinition().ToArray()
            : releaseList
                .MostRecentReleasesByDefinition()
                .Where(
                    b => b
                        .Environments
                        .Any(
                            e => e.Status != EnvironmentStatus.Succeeded))
                .ToArray();
    }
}