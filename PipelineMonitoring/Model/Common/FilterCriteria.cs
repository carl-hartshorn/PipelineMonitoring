using PipelineMonitoring.Services;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace PipelineMonitoring.Model.Common
{
    public class FilterCriteria
    {
        private const string _key = "FilterCriteria";

        public bool ShowAll { get; set; }

        public static async Task<FilterCriteria> LoadFromLocalStorage(LocalStorageService localStorageService)
        {
            if (localStorageService is null)
            {
                throw new ArgumentNullException(nameof(localStorageService));
            }

            var persistedFilterCriteria = await localStorageService.GetItem(_key).ConfigureAwait(false);

            if (!string.IsNullOrWhiteSpace(persistedFilterCriteria))
            {
                return JsonSerializer.Deserialize<FilterCriteria>(persistedFilterCriteria);
            }

            return new FilterCriteria();
        }

        public async Task StoreToLocalStorage(LocalStorageService localStorageService)
        {
            if (localStorageService is null)
            {
                throw new ArgumentNullException(nameof(localStorageService));
            }

            var jsonFilterCriteria = JsonSerializer.Serialize(this);

            await localStorageService.SetItem(_key, jsonFilterCriteria).ConfigureAwait(false);
        }
    }
}
