using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace PipelineMonitoring.Services
{
    public class LocalStorageService
    {
        private const string GetItemFunctionName = "interopLocalStorageGetItem";
        private const string SetItemFunctionName = "interopLocalStorageSetItem";

        private readonly IJSRuntime _jsRuntime;

        public LocalStorageService(
            IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public virtual async Task<string> GetItem(string key)
            => await _jsRuntime.InvokeAsync<string>(GetItemFunctionName, key);

        public virtual async Task SetItem(string key, string value)
            => await _jsRuntime.InvokeAsync<string>(SetItemFunctionName, key, value);
    }
}
