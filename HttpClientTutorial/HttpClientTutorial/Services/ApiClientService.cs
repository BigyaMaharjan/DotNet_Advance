using HttpClientTutorial.Models;
using System.Text.Json;

namespace HttpClientProject.Services
{
    public class ApiClientService
    {
        private readonly HttpClient _client;

        public ApiClientService(HttpClient client)
        {
            _client = client;
        }

        public async Task<List<WeatherForecast>> GetWeatherForecastsAsync()
        {
            HttpResponseMessage response = await _client.GetAsync("/weatherforecast");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();

                try
                {
                    return JsonSerializer.Deserialize<List<WeatherForecast>>(content);
                }
                catch (JsonException ex)
                {
                    Console.WriteLine($"Deserialization error: {ex.Message}");
                    return null;
                }
            }
            else
            {
                Console.WriteLine($"Error: {response.StatusCode}");
                return null;
            }
        }





        public async Task PostWeatherForecastAsync(WeatherForecast forecast)
        {
            var jsonContent = JsonSerializer.Serialize(forecast);
            var content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _client.PostAsync("/weatherforecast", content);
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Weather forecast posted successfully.");
            }
            else
            {
                Console.WriteLine($"Failed to post forecast: {response.StatusCode}");
            }
        }
    }
}
