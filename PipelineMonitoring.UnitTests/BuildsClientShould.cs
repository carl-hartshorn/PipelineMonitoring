using PipelineMonitoring.Model.Builds;
using PipelineMonitoring.Model.Common;
using PipelineMonitoring.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace PipelineMonitoring.UnitTests
{
    [TestClass]
    public class BuildsClientShould
    {
        private readonly Mock<AzureDevOpsSettingsService> _mockAzureDevOpsSettingsService = new Mock<AzureDevOpsSettingsService>(null);
        private readonly MockHttpMessageHandler _mockHttpMessageHandler = new MockHttpMessageHandler();

        [TestMethod]
        public async Task ReturnNullWhenAzureDevOpsSettingsServiceHasOrganisationAndProjectIsFalse()
        {
            var client = CreateBuildsClient();
            
            var result = await client.GetBuilds(new FilterCriteria()).ConfigureAwait(false);

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
            var client = CreateBuildsClient();

            await client.GetBuilds(new FilterCriteria()).ConfigureAwait(false);

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

            _mockHttpMessageHandler.SetupResponse(@"{ ""value"": [ { ""result"": ""succeeded"" }, { ""result"": ""failed"" } ] }");
            var client = CreateBuildsClient();

            var builds = await client.GetBuilds(new FilterCriteria { ShowAll = true }).ConfigureAwait(false);

            Assert.AreEqual(2, builds.Count());
            Assert.IsTrue(builds.Any(b => b.Result == Build.SucceededResult));
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
            var client = CreateBuildsClient();

            var builds = await client.GetBuilds(new FilterCriteria { ShowAll = false }).ConfigureAwait(false);

            Assert.AreEqual(1, builds.Count());
            Assert.IsFalse(builds.Any(b => b.Result == Build.SucceededResult));
        }

        private BuildsClient CreateBuildsClient()
            => new BuildsClient(
                _mockAzureDevOpsSettingsService.Object,
                new HttpClient(_mockHttpMessageHandler));
    }
}
