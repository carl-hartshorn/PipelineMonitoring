using Microsoft.JSInterop;

namespace PipelineMonitoring.Services;

public class LocalStorageService
{
    private const string _getItemFunctionName = "interopLocalStorageGetItem";
    private const string _setItemFunctionName = "interopLocalStorageSetItem";

    private readonly IJSRuntime _jsRuntime;

    public LocalStorageService(
        IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    public async Task<string> GetItem(string key)
        => await _jsRuntime.InvokeAsync<string>(_getItemFunctionName, key).ConfigureAwait(false);

    public async Task SetItem(string key, string value)
        => await _jsRuntime.InvokeVoidAsync(_setItemFunctionName, key, value).ConfigureAwait(false);
}