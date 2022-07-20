using PipelineMonitoring.Services;
using Microsoft.JSInterop;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Net.Http;
using System.Threading.Tasks;
using System;

namespace PipelineMonitoring.UnitTests;

[TestClass]
public sealed class PersonalAccessTokenServiceShould : IDisposable
{
    private readonly HttpClient _httpClient = new();
    private readonly Mock<LocalStorageService> _mockLocalStorageService = new(Mock.Of<IJSRuntime>());
    private readonly PersonalAccessTokenService _personalAccessTokenService;

    public PersonalAccessTokenServiceShould()
    {
        _personalAccessTokenService = new(_httpClient, _mockLocalStorageService.Object);
    }

    [TestMethod]
    public void ReturnNullForPersonalAccessTokenInitially()
    {
        Assert.IsNull(_personalAccessTokenService.PersonalAccessToken);
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
            
        await _personalAccessTokenService.InitialiseFromLocalStorage().ConfigureAwait(false);

        Assert.AreEqual(expectedPersonalAccessToken, _personalAccessTokenService.PersonalAccessToken);
        Assert.AreEqual(expectedDefaultAuthorizationHeader, _httpClient.DefaultRequestHeaders.Authorization?.ToString());
    }

    public void Dispose()
    {
        _httpClient.Dispose();
    }
}