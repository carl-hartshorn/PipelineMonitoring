using PipelineMonitoring.Services;
using System.Text.Json;
using System.Threading.Tasks;

namespace PipelineMonitoring.Model.Common
{
    public class FilterCriteria
    {
        public bool ShowAll { get; set; }

        private const string Key = "FilterCriteria";

        public static async Task<FilterCriteria> LoadFromLocalStorage(LocalStorageService localStorageService)
        {
            var persistedFilterCriteria = await localStorageService.GetItem(Key);

            if (!string.IsNullOrWhiteSpace(persistedFilterCriteria))
            {
                return JsonSerializer.Deserialize<FilterCriteria>(persistedFilterCriteria);
            }

            return new FilterCriteria();
        }

        public async Task StoreToLocalStorage(LocalStorageService localStorageService)
        {
            var jsonFilterCriteria = JsonSerializer.Serialize(this);

            await localStorageService.SetItem(Key, jsonFilterCriteria);
        }
    }
}
