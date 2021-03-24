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
        private readonly Mock<LocalStorageService> _mockLocalStorageService = new(Mock.Of<IJSRuntime>());
        private readonly AzureDevOpsSettingsService _azureDevOpsSettingsService;

        public AzureDevOpsSettingsServiceShould()
        {
            _azureDevOpsSettingsService = new(_mockLocalStorageService.Object);
        }

        [TestMethod]
        public void ReturnFalseForHasOrganisationAndProjectInitially()
        {
            Assert.IsFalse(_azureDevOpsSettingsService.HasOrganisationAndProject);
        }

        [TestMethod]
        public void ReturnNullForOrganisationInitially()
        {
            Assert.IsNull(_azureDevOpsSettingsService.Organisation);
        }

        [TestMethod]
        public void ReturnNullForProjectInitially()
        {
            Assert.IsNull(_azureDevOpsSettingsService.Project);
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
            
            await _azureDevOpsSettingsService.InitialiseFromLocalStorage().ConfigureAwait(false);

            Assert.AreEqual(expectedOrganisation, _azureDevOpsSettingsService.Organisation);
            Assert.AreEqual(expectedProject, _azureDevOpsSettingsService.Project);
            Assert.AreEqual(expectedHasOrganisationAndProject, _azureDevOpsSettingsService.HasOrganisationAndProject);
        }

        [TestMethod]
        public void SaveTheSettingsToLocalStorageWhenUpdatingTheOrganisation()
        {
            const string organisation = "Organisation";
            
            _azureDevOpsSettingsService.Organisation = organisation;

            _mockLocalStorageService
                .Verify(
                    m => m
                        .SetItem(
                            "AzureDevOpsSettings",
                            "Organisation:"),
                    Times.Once);
        }

        [TestMethod]
        public void SaveTheSettingsToLocalStorageWhenUpdatingTheProject()
        {
            const string project = "Project";
            
            _azureDevOpsSettingsService.Project = project;

            _mockLocalStorageService
                .Verify(
                    m => m
                        .SetItem(
                            "AzureDevOpsSettings",
                            ":Project"),
                    Times.Once);
        }
    }
}
