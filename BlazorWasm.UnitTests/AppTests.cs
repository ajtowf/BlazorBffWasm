using Xunit;

namespace BlazorWasm.UnitTests
{
    public class AppTests
    {
        [Fact]
        public void App_CanBeCreated()
        {
            // Arrange & Act
            var app = new App();

            // Assert
            Assert.NotNull(app);
        }
    }
}