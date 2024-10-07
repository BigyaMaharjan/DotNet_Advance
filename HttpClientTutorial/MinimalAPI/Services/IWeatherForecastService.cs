using MinimalApiProject.Data;

namespace MinimalAPI.Services;

public interface IWeatherForecastService
{
    Task<List<WeatherForecast>> GetAllAsync();
    Task<WeatherForecast?> GetByIdAsync(int id);
    Task AddAsync(WeatherForecast forecast);
    Task UpdateAsync(WeatherForecast forecast);
    Task DeleteAsync(int id);
}
