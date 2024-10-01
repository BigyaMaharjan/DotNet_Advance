using Microsoft.EntityFrameworkCore;
using MinimalAPI.Services;
using MinimalApiProject.Data;

namespace MinimalApiProject.Services
{
 

    public class WeatherForecastService : IWeatherForecastService
    {
        private readonly AppDbContext _context;

        public WeatherForecastService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<WeatherForecast>> GetAllAsync() =>
            await _context.WeatherForecasts.ToListAsync();

        public async Task<WeatherForecast?> GetByIdAsync(int id) =>
            await _context.WeatherForecasts.FindAsync(id);

        public async Task AddAsync(WeatherForecast forecast)
        {
            _context.WeatherForecasts.Add(forecast);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(WeatherForecast forecast)
        {
            _context.WeatherForecasts.Update(forecast);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var forecast = await GetByIdAsync(id);
            if (forecast != null)
            {
                _context.WeatherForecasts.Remove(forecast);
                await _context.SaveChangesAsync();
            }
        }
    }
}
