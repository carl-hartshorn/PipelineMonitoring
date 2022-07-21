using PipelineMonitoring.AzureDevOps.Releases;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace PipelineMonitoring.Services;

public class ReleasesClient
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
            return null;
        }

        var releaseList = await _httpClient
            .GetFromJsonAsync<ReleaseList>(
                $"https://vsrm.dev.azure.com/{_azureDevOpsSettingsService.Organisation}/{_azureDevOpsSettingsService.Project}/_apis/release/releases?api-version=5.0&$expand=environments")
            .ConfigureAwait(false);

        return (filterCriteria?.ShowAll ?? false)
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