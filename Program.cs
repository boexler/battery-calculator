using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using battery_calculator.Components;
using battery_calculator.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Configure base path for GitHub Pages
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

// Register services
builder.Services.AddScoped<CsvParserService>();
builder.Services.AddScoped<BatterySimulationService>();
builder.Services.AddScoped<AmortizationCalculatorService>();
builder.Services.AddScoped<VendorDetectionService>();
builder.Services.AddScoped<BatteryPriceService>();

await builder.Build().RunAsync();

