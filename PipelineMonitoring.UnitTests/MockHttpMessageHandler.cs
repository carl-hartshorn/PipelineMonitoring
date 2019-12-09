using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace PipelineMonitoring.UnitTests
{
    public class MockHttpMessageHandler : HttpMessageHandler
    {
        private HttpResponseMessage _response;
        
        public List<HttpRequestMessage> SentMessages { get; } = new List<HttpRequestMessage>();

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            SentMessages.Add(request);

            if (_response is null)
            {
                SetupResponse("{ \"value\": [] }");
            }

            return Task.FromResult(_response);
        }

        public void SetupResponse(string jsonString)
        {
            _response = new HttpResponseMessage(System.Net.HttpStatusCode.OK)
            {
                Content = new StringContent(jsonString)
            };
        }
    }
}
