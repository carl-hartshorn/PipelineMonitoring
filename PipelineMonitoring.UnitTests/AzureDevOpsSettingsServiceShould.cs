using PipelineMonitoring.Services;
using Microsoft.JSInterop;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;

namespace PipelineMonitoring.UnitTests
{
    [TestClass]
    public class AzureDevOpsSettingsServiceShould
    {
        private readonly Mock<LocalStorageService> _mockLocalStorageService = new Mock<LocalStorageService>(Mock.Of<IJSRuntime>());

        [TestMethod]
        public void ReturnFalseForHasOrganisationAndProjectInitially()
        {
            var service = CreateAzureDevOpsSettingsService();

            Assert.IsFalse(service.HasOrganisationAndProject);
        }

        [TestMethod]
        public void ReturnNullForOrganisationInitially()
        {
            var service = CreateAzureDevOpsSettingsService();

            Assert.IsNull(service.Organisation);
        }

        [TestMethod]
        public void ReturnNullForProjectInitially()
        {
            var service = CreateAzureDevOpsSettingsService();

            Assert.IsNull(service.Project);
        }

        [DataTestMethod]
        [DataRow(null, null, null, false)]
        [DataRow("", null, null, false)]
        [DataRow(":", "", "", false)]
        [DataRow("Organisation:", "Organisation", "", false)]
        [DataRow(":Project", "", "Project", false)]
        [DataRow("Organisation:Project", "Organisation", "Project", true)]
        public async Task InitialiseOrganisationAndProjectFromLocalStorage(
            string localStorageContents,
            string expectedOrganisation,
            string expectedProject,
            bool expectedHasOrganisationAndProject)
        {
            _mockLocalStorageService.Setup(m => m.GetItem("AzureDevOpsSettings")).ReturnsAsync(localStorageContents);
            var service = CreateAzureDevOpsSettingsService();
            
            await service.InitialiseFromLocalStorage().ConfigureAwait(false);

            Assert.AreEqual(expectedOrganisation, service.Organisation);
            Assert.AreEqual(expectedProject, service.Project);
            Assert.AreEqual(expectedHasOrganisationAndProject, service.HasOrganisationAndProject);
        }

        [TestMethod]
        public void SaveTheSettingsToLocalStorageWhenUpdatingTheOrganisation()
        {
            const string organisation = "Organisation";
            var service = CreateAzureDevOpsSettingsService();
            
            service.Organisation = organisation;

            _mockLocalStorageService.Verify(m => m.SetItem("AzureDevOpsSettings", "Organisation:"), Times.Once);
        }

        [TestMethod]
        public void SaveTheSettingsToLocalStorageWhenUpdatingTheProject()
        {
            const string project = "Project";
            var service = CreateAzureDevOpsSettingsService();

            service.Project = project;

            _mockLocalStorageService.Verify(m => m.SetItem("AzureDevOpsSettings", ":Project"), Times.Once);
        }

        private AzureDevOpsSettingsService CreateAzureDevOpsSettingsService()
            => new AzureDevOpsSettingsService(_mockLocalStorageService.Object);
    }
}
