using Xunit;

namespace BlazorWasm.UnitTests
{
    public class WeatherTests
    {
        [Fact]
        public void WeatherForecast_HasCorrectTemperatureConversion()
        {
            // Arrange
            var forecast = new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now),
                TemperatureC = 25,
                Summary = "Sunny"
            };

            // Act
            var temperatureF = forecast.TemperatureF;

            // Assert
            Assert.Equal(77, temperatureF); // 25C = 77F
        }

        [Fact]
        public void WeatherForecast_HasCorrectProperties()
        {
            // Arrange
            var forecast = new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now),
                TemperatureC = 25,
                Summary = "Sunny"
            };

            // Act & Assert
            Assert.Equal(25, forecast.TemperatureC);
            Assert.Equal("Sunny", forecast.Summary);
        }
    }

    // Simple class to represent WeatherForecast logic
    public class WeatherForecast
    {
        public DateOnly Date { get; set; }
        public int TemperatureC { get; set; }
        public string Summary { get; set; } = string.Empty;

        public int TemperatureF => (int)(TemperatureC * 9 / 5.0 + 32);
    }
}