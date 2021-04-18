using API.Customer.Data.Interfaces;
using API.Customer.Data.Options;
using Microsoft.Extensions.Options;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace API.Customer.Data.Providers
{
  public class HRSystemProvider : IValidateOfficialIdProvider
  {
    private readonly IHttpClientFactory _clientFactory;
    private readonly string _baseUrl;
    private readonly string _validateOfficialIdUrl;

    public HRSystemProvider(IHttpClientFactory clientFactory, IOptions<HRSystemOptions> options)
    {
      _clientFactory = clientFactory;
      _baseUrl = options.Value.BaseUrl;
      _validateOfficialIdUrl = options.Value.ValidateOfficialIdUrl;
    }

    public async Task ValidateOfficialId(string officialId)
    {
      var client = _clientFactory.CreateClient();

      var request = new HttpRequestMessage(HttpMethod.Post, _validateOfficialIdUrl) { Content = new StringContent(officialId) };
      var response = await client.SendAsync(request);
      if (!response.IsSuccessStatusCode)
        throw await GenerateException(response);

      var successfullValidation = await response.Content.ReadAsStringAsync();
      if (!successfullValidation.Equals("true", StringComparison.InvariantCultureIgnoreCase)) //Pretend that the api returns "true" as response in content if its a valid officialId
        throw new Exception($"{officialId} was not deemed as a valid official id by HRSystem.");  //May not want do log official id in clear text here
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
