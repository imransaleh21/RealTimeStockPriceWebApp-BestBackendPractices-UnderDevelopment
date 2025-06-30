using Microsoft.Extensions.Configuration;
using StockPrice.ServiceContracts;
using System.Net.Http;
using System.Reflection.PortableExecutable;
using System.Text.Json;

namespace StockPrice.Services
{
    public class FinnhubService : IFinnhubService
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;

        public FinnhubService
            (
            IConfiguration configuration,
            IHttpClientFactory httpClientFactory
            )
        {
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
        }
        public async Task<Dictionary<string, object>?> GetCompanyProfile(string stockSymbol)
        {
            using (HttpClient httpClient = _httpClientFactory.CreateClient())
            {
                HttpRequestMessage httpRequestMessage = new()
                {
                    RequestUri = new Uri($" https://finnhub.io/api/v1/stock/profile2?symbol={stockSymbol}&token={_configuration["FinnHubToken"]}"),
                    Method = HttpMethod.Get
                };
                HttpResponseMessage httpResponseMessage = await httpClient.SendAsync( httpRequestMessage );
                Stream stream = httpResponseMessage.Content.ReadAsStream();
                StreamReader streamReader = new StreamReader( stream );
                string responseBody = await streamReader.ReadToEndAsync();
                Dictionary<string, object>? weatherResponse = JsonSerializer.Deserialize<Dictionary<string, object>>(responseBody);
                
                if (weatherResponse == null) 
                {
                    throw new InvalidOperationException("No response from finnhub");
                }
                if (weatherResponse.ContainsKey("error"))
                {
                    throw new InvalidOperationException(Convert.ToString(weatherResponse["error"]));
                }
                return weatherResponse;
            }
        }
        public Task<Dictionary<string, object>?> GetStockPriceQuote(string stockSymbol)
        {
            throw new NotImplementedException();
        }
    }
}
