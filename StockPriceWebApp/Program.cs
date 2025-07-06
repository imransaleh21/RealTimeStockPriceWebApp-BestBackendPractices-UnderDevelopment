using Microsoft.Extensions.DependencyInjection;
using ServiceContracts;
using Services;
using StockPriceWebApp;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddHttpClient();
builder.Services.Configure<TradingOptions>(builder.Configuration.GetSection("TradingOption"));
builder.Services.AddScoped<IFinnhubService,  FinnhubService>();
var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();
app.MapControllers();
app.Run();
