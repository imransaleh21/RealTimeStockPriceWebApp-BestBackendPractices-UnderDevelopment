using Microsoft.Extensions.Configuration;
using ServiceContracts;
using System.Net.Http;
using System.Reflection.PortableExecutable;
using System.Text.Json;

namespace Services
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
                Dictionary<string, object>? companyProfileResponse = JsonSerializer.Deserialize<Dictionary<string, object>>(responseBody);
                
                if (companyProfileResponse == null) 
                {
                    throw new InvalidOperationException("No response from finnhub");
                }
                if (companyProfileResponse.ContainsKey("error"))
                {
                    throw new InvalidOperationException(Convert.ToString(companyProfileResponse["error"]));
                }
                return companyProfileResponse;
            }
        }
        public async Task<Dictionary<string, object>?> GetStockPriceQuote(string stockSymbol)
        {
            using (HttpClient httpClient = _httpClientFactory.CreateClient())
            {
                HttpRequestMessage httpRequestMessage = new()
                {
                    RequestUri = new Uri($" https://finnhub.io/api/v1/quote?symbol={stockSymbol}&token={_configuration["FinnHubToken"]}"),
                    Method = HttpMethod.Get
                };

                HttpResponseMessage httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);
                Stream stream = httpResponseMessage.Content.ReadAsStream();
                StreamReader streamReader = new StreamReader( stream );
                string responseBody = await streamReader.ReadToEndAsync();
                Dictionary<string, object>? stockPriceResponse = JsonSerializer.Deserialize<Dictionary<string, object>>(responseBody);

                if (stockPriceResponse == null)
                {
                    throw new InvalidOperationException("No response from finnhub");
                }
                if (stockPriceResponse.ContainsKey("error"))
                {
                    throw new InvalidOperationException(Convert.ToString(stockPriceResponse["error"]));
                }
                return stockPriceResponse;

            }
            ;
        }
    }
}
