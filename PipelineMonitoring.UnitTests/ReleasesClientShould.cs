using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PipelineMonitoring.Model.Common;
using PipelineMonitoring.Services;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace PipelineMonitoring.UnitTests
{
    [TestClass]
    public class ReleasesClientShould
    {
        private readonly Mock<AzureDevOpsSettingsService> _mockAzureDevOpsSettingsService = new Mock<AzureDevOpsSettingsService>(null);
        private readonly MockHttpMessageHandler _mockHttpMessageHandler = new MockHttpMessageHandler();

        [TestMethod]
        public async Task ReturnNullWhenAzureDevOpsSettingsServiceHasOrganisationAndProjectIsFalse()
        {
            var client = CreateReleasesClient();
            
            var result = await client.GetReleases(new FilterCriteria()).ConfigureAwait(false);

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
            var client = CreateReleasesClient();

            await client.GetReleases(new FilterCriteria()).ConfigureAwait(false);

            _mockHttpMessageHandler.SentMessages.Single(m => m.RequestUri.ToString().Contains($"{organisation}/{project}"));
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
            var client = CreateReleasesClient();

            var releases = await client.GetReleases(new FilterCriteria { ShowAll = true }).ConfigureAwait(false);

            Assert.AreEqual(2, releases.Count());
            Assert.IsTrue(releases.Any(b => b.Environments.All(e => e.Status == Model.Releases.Environment.SucceededStatus)));
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
            var client = CreateReleasesClient();

            var releases = await client.GetReleases(new FilterCriteria { ShowAll = false }).ConfigureAwait(false);

            Assert.AreEqual(1, releases.Count());
            Assert.IsFalse(releases.Any(b => b.Environments.All(e => e.Status == Model.Releases.Environment.SucceededStatus)));
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
            var client = CreateReleasesClient();

            var releases = await client.GetReleases(new FilterCriteria { ShowAll = false }).ConfigureAwait(false);

            Assert.AreEqual(1, releases.Count());
            Assert.AreEqual("queued", releases.Single().Environments.Single().Status);
        }

        private ReleasesClient CreateReleasesClient()
            => new ReleasesClient(
                _mockAzureDevOpsSettingsService.Object,
                new HttpClient(_mockHttpMessageHandler));
    }
}
