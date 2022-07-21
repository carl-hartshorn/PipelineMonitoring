using System.Diagnostics.CodeAnalysis;
using System.Net;

namespace PipelineMonitoring.UnitTests;

public class MockHttpMessageHandler : HttpMessageHandler
{
    private HttpResponseMessage? _response;
        
    public ICollection<HttpRequestMessage> SentMessages { get; } = new List<HttpRequestMessage>();

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        SentMessages.Add(request);

        if (_response is null)
        {
            SetupResponse("{ \"value\": [] }");
        }

        return Task.FromResult(_response);
    }

    [MemberNotNull(nameof(_response))]
    public void SetupResponse(string jsonString)
    {
        _response = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(jsonString)
        };
    }
}