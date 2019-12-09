using PipelineMonitoring.Services;
using Microsoft.JSInterop;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;

namespace PipelineMonitoring.UnitTests
{
    [TestClass]
    public class LocalStorageServiceShould
    {
        private readonly Mock<IJSRuntime> _mockJSRuntime = new Mock<IJSRuntime>();

        [TestMethod]
        public async Task InvokeTheLocalStorageGetItemInteropFunctionWhenGetItemIsCalled()
        {
            const string key = "Key";
            var service = CreateLocalStorageService();

            await service.GetItem(key).ConfigureAwait(false);

            _mockJSRuntime.Verify(m => m.InvokeAsync<string>("interopLocalStorageGetItem", new[] { key }), Times.Once);
        }

        [TestMethod]
        public async Task InvokeTheLocalStorageSetItemInteropFunctionWhenSetItemIsCalled()
        {
            const string key = "Key";
            const string value = "Value";
            var service = CreateLocalStorageService();

            await service.SetItem(key, value).ConfigureAwait(false);

            _mockJSRuntime.Verify(m => m.InvokeAsync<string>("interopLocalStorageSetItem", new[] { key, value }), Times.Once);
        }

        private LocalStorageService CreateLocalStorageService()
            => new LocalStorageService(_mockJSRuntime.Object);
    }
}
