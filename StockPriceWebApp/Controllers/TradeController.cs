using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using StockPrice.ServiceContracts;
using StockPriceWebApp.Models;

namespace StockPriceWebApp.Controllers
{
    [Route("[Controller]")]
    public class TradeController : Controller
    {
        private readonly IFinnhubService _finnhubService;
        private readonly TradingOptions _options;

        public TradeController(
            IFinnhubService finnhubService,
            IOptions<TradingOptions> options
            )
        {
            _finnhubService = finnhubService;
            _options = options.Value;
        }

        [Route("[Action]")]
        [Route("~/[Controller]")]
        public async Task<IActionResult> Index()
        {
            Dictionary<string, object>? companyProfile = await _finnhubService.GetCompanyProfile(_options.DefaultStockSymbol);
            Dictionary<string, object>? stockPrices = await _finnhubService.GetStockPriceQuote(_options.DefaultStockSymbol);
            StockTrade stockTrade = new()
            {
                StockSymbol = companyProfile["ticker"].ToString(),
                StockName = companyProfile["name"].ToString(),
                Price = Convert.ToDouble(stockPrices["c"].ToString())
            };
            return Json(stockTrade);
        }
    }
}
