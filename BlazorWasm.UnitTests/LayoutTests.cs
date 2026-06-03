using Xunit;

namespace BlazorWasm.UnitTests
{
    public class LayoutTests
    {
        [Fact]
        public void MainLayout_CanBeCreated()
        {
            // Arrange & Act
            var layout = new Layout.MainLayout();

            // Assert
            Assert.NotNull(layout);
        }

        [Fact]
        public void NavMenu_CanBeCreated()
        {
            // Arrange & Act
            var navMenu = new Layout.NavMenu();

            // Assert
            Assert.NotNull(navMenu);
        }
    }
}