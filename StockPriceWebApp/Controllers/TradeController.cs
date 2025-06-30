using Microsoft.AspNetCore.Mvc;
using StockPrice.ServiceContracts;

namespace StockPriceWebApp.Controllers
{
    public class TradeController : Controller
    {
        private readonly IFinnhubService _finnhubService;

        public TradeController
            (
            IFinnhubService finnhubService
            )
        {
            _finnhubService = finnhubService;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
