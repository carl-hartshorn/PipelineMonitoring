using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace PipelineMonitoring.Services
{
    public class PersonalAccessTokenService
    {
        private const string _localStorageKey = "PersonalAccessToken";

        private readonly HttpClient _httpClient;
        private readonly LocalStorageService _localStorageService;
        private string _personalAccessToken;

        public PersonalAccessTokenService(
            HttpClient httpClient,
            LocalStorageService localStorageService)
        {
            _httpClient = httpClient;
            _localStorageService = localStorageService;

            PersonalAccessTokenChanged += new EventHandler<PersonalAccessTokenEventArgs>(
                async (sender, args) =>
                {
                    await SaveToLocalStorage().ConfigureAwait(false);
                });
        }
        
        public string PersonalAccessToken
        {
            get => _personalAccessToken;
            set
            {
                _personalAccessToken = value;
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                "Basic",
                Convert
                    .ToBase64String(
                        Encoding.UTF8
                            .GetBytes(
                                $":{value}")));

                PersonalAccessTokenChanged(this, new PersonalAccessTokenEventArgs());
            }
        }

        public async Task InitialiseFromLocalStorage()
        {
            var token = await _localStorageService
                .GetItem(_localStorageKey)
                .ConfigureAwait(false);

            if (!string.IsNullOrWhiteSpace(token))
            {
                PersonalAccessToken = token;
            }
        }

        private async Task SaveToLocalStorage()
        {
            await _localStorageService
                .SetItem(_localStorageKey, PersonalAccessToken)
                .ConfigureAwait(false);
        }

        public event EventHandler<PersonalAccessTokenEventArgs> PersonalAccessTokenChanged;
    }
}
