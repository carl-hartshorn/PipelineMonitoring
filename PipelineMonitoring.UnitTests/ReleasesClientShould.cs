using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PipelineMonitoring.AzureDevOps.Releases;
using PipelineMonitoring.Services;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace PipelineMonitoring.UnitTests;

[TestClass]
public sealed class ReleasesClientShould : IDisposable
{
    private readonly Mock<AzureDevOpsSettingsService> _mockAzureDevOpsSettingsService = new(null);
    private readonly MockHttpMessageHandler _mockHttpMessageHandler = new();
    private readonly HttpClient _httpClient;
    private readonly ReleasesClient _releasesClient;

    public ReleasesClientShould()
    {
        _httpClient = new(_mockHttpMessageHandler);

        _releasesClient = new(
            _mockAzureDevOpsSettingsService.Object,
            _httpClient);
    }

    [TestMethod]
    public async Task ReturnNullWhenAzureDevOpsSettingsServiceHasOrganisationAndProjectIsFalse()
    {            
        var result = await _releasesClient.GetReleases(new FilterCriteria()).ConfigureAwait(false);

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

        await _releasesClient.GetReleases(new FilterCriteria()).ConfigureAwait(false);

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

        _mockHttpMessageHandler.SetupResponse(@"
{
    ""value"": [
        {
            ""releaseDefinition"": {
                ""id"": 1
            },
            ""environments"": [
                {
                    ""status"": ""succeeded""
                },
                {
                    ""status"": ""succeeded""
                }
            ]
        },
        {
            ""releaseDefinition"": {
                ""id"": 2
            },
            ""environments"": [
                {
                    ""status"": ""succeeded""
                },
                {
                    ""status"": ""rejected""
                }
            ]
        }
    ]
}");

        var releases = await _releasesClient.GetReleases(new FilterCriteria { ShowAll = true }).ConfigureAwait(false);

        Assert.AreEqual(2, releases.Length);

        Assert.IsTrue(
            releases
                .Any(
                    b => b
                        .Environments
                        .All(
                            e => e.Status == EnvironmentStatus.Succeeded)));
    }

    [TestMethod]
    public async Task NotReturnSucceededBuildsWhenTheFilterCriteriaShowAllValueIsFalse()
    {
        var organisation = Guid.NewGuid().ToString();
        var project = Guid.NewGuid().ToString();
        _mockAzureDevOpsSettingsService.Setup(m => m.HasOrganisationAndProject).Returns(true);
        _mockAzureDevOpsSettingsService.Setup(m => m.Organisation).Returns(organisation);
        _mockAzureDevOpsSettingsService.Setup(m => m.Project).Returns(project);

        _mockHttpMessageHandler.SetupResponse(@"
{
    ""value"": [
        {
            ""releaseDefinition"": {
                ""id"": 1
            },
            ""environments"": [
                {
                    ""status"": ""succeeded""
                },
                {
                    ""status"": ""succeeded""
                }
            ]
        },
        {
            ""releaseDefinition"": {
                ""id"": 2
            },
            ""environments"": [
                {
                    ""status"": ""succeeded""
                },
                {
                    ""status"": ""rejected""
                }
            ]
        }
    ]
}");

        var releases = await _releasesClient
            .GetReleases(
                new FilterCriteria
                {
                    ShowAll = false
                })
            .ConfigureAwait(false);

        Assert.AreEqual(1, releases.Length);

        Assert.IsFalse(
            releases
                .Any(
                    b => b
                        .Environments
                        .All(
                            e => e.Status == EnvironmentStatus.Succeeded)));
    }

    [TestMethod]
    public async Task ReturnOnlyTheMostRecentReleaseForAGivenDefinition()
    {
        var organisation = Guid.NewGuid().ToString();
        var project = Guid.NewGuid().ToString();
        _mockAzureDevOpsSettingsService.Setup(m => m.HasOrganisationAndProject).Returns(true);
        _mockAzureDevOpsSettingsService.Setup(m => m.Organisation).Returns(organisation);
        _mockAzureDevOpsSettingsService.Setup(m => m.Project).Returns(project);

        _mockHttpMessageHandler.SetupResponse(@"
{
    ""value"": [
        {
            ""releaseDefinition"": {
                ""id"": 1
            },
            ""modifiedOn"": ""2019-09-29T12:54:29.998208Z"",
            ""environments"": [
                {
                    ""status"": ""queued""
                }
            ]
        },
        {
            ""releaseDefinition"": {
                ""id"": 1
            },
            ""modifiedOn"": ""2019-09-29T12:53:40.7865892Z"",
            ""environments"": [
                {
                    ""status"": ""rejected""
                }
            ]
        }
    ]
}");

        var releases = await _releasesClient
            .GetReleases(
                new FilterCriteria
                { 
                    ShowAll = false
                })
            .ConfigureAwait(false);

        Assert.AreEqual(1, releases.Length);
        Assert.AreEqual(EnvironmentStatus.Queued, releases.Single().Environments.Single().Status);
    }

    public void Dispose()
    {
        _mockHttpMessageHandler.Dispose();
        _httpClient.Dispose();
    }
}