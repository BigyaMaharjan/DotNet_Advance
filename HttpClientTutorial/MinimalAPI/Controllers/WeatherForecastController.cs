using MinimalAPI.Services;
using MinimalApiProject.Data;
using MinimalApiProject.Services;

namespace MinimalApiProject
{
    public static class WeatherForecastEndpoints
    {
        public static void MapWeatherForecastEndpoints(this WebApplication app)
        {
            app.MapGet("/weatherforecast", async (IWeatherForecastService service) =>
            {
                return Results.Ok(await service.GetAllAsync());
            });

            app.MapGet("/weatherforecast/{id}", async (int id, IWeatherForecastService service) =>
            {
                var forecast = await service.GetByIdAsync(id);
                return forecast is not null ? Results.Ok(forecast) : Results.NotFound();
            });

            app.MapPost("/weatherforecast", async (WeatherForecast forecast, IWeatherForecastService service) =>
            {
                await service.AddAsync(forecast);
                return Results.Created($"/weatherforecast/{forecast.Id}", forecast);
            });

            app.MapPut("/weatherforecast/{id}", async (int id, WeatherForecast forecast, IWeatherForecastService service) =>
            {
                var existingForecast = await service.GetByIdAsync(id);
                if (existingForecast is null) return Results.NotFound();

                existingForecast.Date = forecast.Date;
                existingForecast.TemperatureC = forecast.TemperatureC;
                existingForecast.Summary = forecast.Summary;
                await service.UpdateAsync(existingForecast);

                return Results.Ok(existingForecast);
            });

            app.MapDelete("/weatherforecast/{id}", async (int id, IWeatherForecastService service) =>
            {
                await service.DeleteAsync(id);
                return Results.NoContent();
            });
        }
    }
}
