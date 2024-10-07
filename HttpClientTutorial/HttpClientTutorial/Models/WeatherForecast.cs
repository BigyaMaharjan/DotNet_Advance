using System.Text.Json.Serialization;

namespace HttpClientTutorial.Models;
public class WeatherForecast
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
    [JsonPropertyName("date")]
    public DateTime Date { get; set; }
    [JsonPropertyName("temperatureC")]
    public int TemperatureC { get; set; }
    [JsonPropertyName("summary")]
    public string Summary { get; set; }
}