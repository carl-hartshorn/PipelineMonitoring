using PipelineMonitoring.Services;
using Microsoft.JSInterop;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;

namespace PipelineMonitoring.UnitTests;

[TestClass]
public class LocalStorageServiceShould
{
    private readonly Mock<IJSRuntime> _mockJSRuntime = new();
    private readonly LocalStorageService _localStorageService;

    public LocalStorageServiceShould()
    {
        _localStorageService = new(_mockJSRuntime.Object);
    }

    [TestMethod]
    public async Task InvokeTheLocalStorageGetItemInteropFunctionWhenGetItemIsCalled()
    {
        const string key = "Key";

        await _localStorageService.GetItem(key).ConfigureAwait(false);

        _mockJSRuntime.Verify(m => m.InvokeAsync<string>("interopLocalStorageGetItem", new[] { key }), Times.Once);
    }

    [TestMethod]
    public async Task InvokeTheLocalStorageSetItemInteropFunctionWhenSetItemIsCalled()
    {
        const string key = "Key";
        const string value = "Value";

        await _localStorageService.SetItem(key, value).ConfigureAwait(false);

        _mockJSRuntime.Verify(m => m.InvokeAsync<object>("interopLocalStorageSetItem", new[] { key, value }), Times.Once);
    }
}