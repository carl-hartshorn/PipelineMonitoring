using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PipelineMonitoring.AzureDevOps.Builds;
using PipelineMonitoring.Services;

namespace PipelineMonitoring.UnitTests;

[TestClass]
public sealed class BuildsClientShould : IDisposable
{
    private readonly Mock<AzureDevOpsSettingsService> _mockAzureDevOpsSettingsService = new(null);
    private readonly MockHttpMessageHandler _mockHttpMessageHandler = new();
    private readonly HttpClient _httpClient;
    private readonly BuildsClient _buildsClient;

    public BuildsClientShould()
    {
        _httpClient = new(_mockHttpMessageHandler);

        _buildsClient = new(
            _mockAzureDevOpsSettingsService.Object,
            _httpClient);
    }

    [TestMethod]
    public async Task ReturnNullWhenAzureDevOpsSettingsServiceHasOrganisationAndProjectIsFalse()
    {
        var result = await _buildsClient.GetBuilds(new FilterCriteria()).ConfigureAwait(false);

        Assert.IsNull(result);
    }

    [TestMethod]
    public async Task GetTheBuildsUsingTheOrganisationAndProjectFromTheAzureDevOpsSettingsServiceWhenAzureDevOpsSettingsServiceHasOrganisationAndProjectIsTrue()
    {
        var organisation = Guid.NewGuid().ToString();
        var project = Guid.NewGuid().ToString();
        _mockAzureDevOpsSettingsService.Setup(m => m.HasOrganisationAndProject).Returns(true);
        _mockAzureDevOpsSettingsService.Setup(m => m.Organisation).Returns(organisation);
        _mockAzureDevOpsSettingsService.Setup(m => m.Project).Returns(project);
            
        await _buildsClient.GetBuilds(new FilterCriteria()).ConfigureAwait(false);

        Assert.IsNotNull(
            _mockHttpMessageHandler
                .SentMessages
                .Single(
                    m => m
                        .RequestUri?
                        .ToString()
                        .Contains(
                            $"{organisation}/{project}",
                            StringComparison.OrdinalIgnoreCase) == true));
    }

    [TestMethod]
    public async Task ReturnSucceededBuildsWhenTheFilterCriteriaShowAllValueIsTrue()
    {
        var organisation = Guid.NewGuid().ToString();
        var project = Guid.NewGuid().ToString();
        _mockAzureDevOpsSettingsService.Setup(m => m.HasOrganisationAndProject).Returns(true);
        _mockAzureDevOpsSettingsService.Setup(m => m.Organisation).Returns(organisation);
        _mockAzureDevOpsSettingsService.Setup(m => m.Project).Returns(project);

        _mockHttpMessageHandler.SetupResponse(@"{ ""value"": [ { ""result"": ""succeeded"" }, { ""result"": ""failed"" } ] }");
            
        var builds = await _buildsClient
            .GetBuilds(
                new FilterCriteria
                {
                    ShowAll = true
                })
            .ConfigureAwait(false);

        Assert.AreEqual(2, builds.Length);
        Assert.IsTrue(builds.Any(b => b.Result == BuildResult.Succeeded));
    }

    [TestMethod]
    public async Task NotReturnSucceededBuildsWhenTheFilterCriteriaShowAllValueIsFalse()
    {
        var organisation = Guid.NewGuid().ToString();
        var project = Guid.NewGuid().ToString();
        _mockAzureDevOpsSettingsService.Setup(m => m.HasOrganisationAndProject).Returns(true);
        _mockAzureDevOpsSettingsService.Setup(m => m.Organisation).Returns(organisation);
        _mockAzureDevOpsSettingsService.Setup(m => m.Project).Returns(project);

        _mockHttpMessageHandler.SetupResponse(@"{ ""value"": [ { ""result"": ""succeeded"" }, { ""result"": ""failed"" } ] }");
            
        var builds = await _buildsClient
            .GetBuilds(
                new FilterCriteria
                {
                    ShowAll = false
                })
            .ConfigureAwait(false);

        Assert.AreEqual(1, builds.Length);
        Assert.IsFalse(builds.Any(b => b.Result == BuildResult.Succeeded));
    }

    public void Dispose()
    {
        _mockHttpMessageHandler.Dispose();
        _httpClient.Dispose();
    }
}