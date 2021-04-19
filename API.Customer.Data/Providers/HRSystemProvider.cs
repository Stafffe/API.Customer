using API.Customer.Data.DataObjects;
using API.Customer.Data.Interfaces;
using API.Customer.Data.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace API.Customer.Data.Providers
{
  public class HRSystemProvider : IHRSystemProvider
  {
    private readonly IHttpClientFactory _clientFactory;
    private readonly ILogger<HRSystemProvider> _logger;
    private readonly string _baseUrl;
    private readonly string _validateOfficialIdUrl;
    private readonly string _auditLogUrl;

    public HRSystemProvider(IHttpClientFactory clientFactory, IOptions<HRSystemOptions> options, ILogger<HRSystemProvider> logger)
    {
      _clientFactory = clientFactory;
      _logger = logger;
      _baseUrl = options.Value.BaseUrl;
      _validateOfficialIdUrl = options.Value.ValidateOfficialIdUrl;
      _auditLogUrl = options.Value.AuditLogUrl;
    }

    public async Task ValidateOfficialId(string officialId)
    {
      var client = _clientFactory.CreateClient();
      client.BaseAddress = new Uri(_baseUrl);

      var request = new HttpRequestMessage(HttpMethod.Post, _validateOfficialIdUrl) { Content = new StringContent(officialId) };
      var response = await client.SendAsync(request, new System.Threading.CancellationToken());
      if (!response.IsSuccessStatusCode)
        throw await GenerateException(response);

      var successfullValidation = await response.Content.ReadAsStringAsync();
      if (!successfullValidation.Equals("true", StringComparison.InvariantCultureIgnoreCase)) //Pretend that the api returns "true" as response in content if its a valid officialId
        throw new Exception($"{officialId} was not deemed as a valid official id by HRSystem.");  //May not want do log official id in clear text here
    }

    public async Task NotifyAboutChange(string officialId, ChangeType changeType)
    {
      var client = _clientFactory.CreateClient();
      client.BaseAddress = new Uri(_baseUrl);

      var request = new HttpRequestMessage(HttpMethod.Post, _auditLogUrl) { Content = new StringContent($"{changeType} on customer with id {officialId}.") };
      var response = await client.SendAsync(request, new System.Threading.CancellationToken());
      if (!response.IsSuccessStatusCode)
        _logger.LogError(await GenerateException(response), $"Failed to audit log {changeType}-change to HRSystem.");
    }

    private static async Task<Exception> GenerateException(HttpResponseMessage response)
    {
      string responseContent = "";
      try
      {
        responseContent = await response.Content.ReadAsStringAsync();
      }
      catch
      {
        //ignore
      }

      return new Exception($"Recieved error-code: {response.StatusCode} from calling {response.RequestMessage.RequestUri}. Got response content: {responseContent}.");
    }
  }
}
