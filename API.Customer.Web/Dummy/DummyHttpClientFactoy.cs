using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace API.Customer.Web.Dummy
{
  public class DummyHttpClientFactoy : IHttpClientFactory
  {
    public HttpClient CreateClient(string name)
    {
      return new DummyHttpClient();
    }
  }

  public class DummyHttpClient : HttpClient {
    public override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
      return Task.FromResult(new HttpResponseMessage(System.Net.HttpStatusCode.OK) { Content = new StringContent("true") });
    }
  }
}
