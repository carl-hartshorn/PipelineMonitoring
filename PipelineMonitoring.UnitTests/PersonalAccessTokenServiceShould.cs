using PipelineMonitoring.Services;
using Microsoft.JSInterop;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Net.Http;
using System.Threading.Tasks;

namespace PipelineMonitoring.UnitTests
{
    [TestClass]
    public class PersonalAccessTokenServiceShould
    {
        private readonly HttpClient _httpClient = new HttpClient();
        private readonly Mock<LocalStorageService> _mockLocalStorageService = new Mock<LocalStorageService>(Mock.Of<IJSRuntime>());

        [TestMethod]
        public void ReturnNullForPersonalAccessTokenInitially()
        {
            var service = CreatePersonalAccessTokenService();

            Assert.IsNull(service.PersonalAccessToken);
        }

        [DataTestMethod]
        [DataRow(null, null, null)]
        [DataRow("", null, null)]
        [DataRow("PersonalAccessToken", "PersonalAccessToken", "Basic OlBlcnNvbmFsQWNjZXNzVG9rZW4=")]
        public async Task InitialisePersonalAccessTokenFromLocalStorage(
            string localStorageContents,
            string expectedPersonalAccessToken,
            string expectedDefaultAuthorizationHeader)
        {
            _mockLocalStorageService.Setup(m => m.GetItem("PersonalAccessToken")).ReturnsAsync(localStorageContents);
            var service = CreatePersonalAccessTokenService();
            
            await service.InitialiseFromLocalStorage().ConfigureAwait(false);

            Assert.AreEqual(expectedPersonalAccessToken, service.PersonalAccessToken);
            Assert.AreEqual(expectedDefaultAuthorizationHeader, _httpClient.DefaultRequestHeaders.Authorization?.ToString());
        }

        private PersonalAccessTokenService CreatePersonalAccessTokenService()
            => new PersonalAccessTokenService(_httpClient, _mockLocalStorageService.Object);
    }
}
