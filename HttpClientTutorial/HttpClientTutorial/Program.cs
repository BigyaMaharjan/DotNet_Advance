using HttpClientProject.Services;
using HttpClientTutorial.Models;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((_, services) =>
    {
        services.AddHttpClient<ApiClientService>(client =>
        {
            client.BaseAddress = new Uri("https://localhost:50587");
            client.DefaultRequestHeaders.Add("Accept", "application/json");
        });
    })
    .Build();

var apiClient = host.Services.GetRequiredService<ApiClientService>();

// Fetch weather forecasts
var forecasts = await apiClient.GetWeatherForecastsAsync();
if (forecasts == null)
{
    Console.WriteLine("No weather forecasts were retrieved.");
}
else
{
    Console.WriteLine("Weather Forecasts:");
    foreach (var forecast in forecasts)
    {
        Console.WriteLine($"Date: {forecast.Date}, Temp: {forecast.TemperatureC}C");
    }
}

// Add a new weather forecast
await apiClient.PostWeatherForecastAsync(new WeatherForecast
{
    Date = DateTime.Now.AddDays(1),
    TemperatureC = 22,
    Summary = "Partly Cloudy"
});
