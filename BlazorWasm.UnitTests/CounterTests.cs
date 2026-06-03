using Xunit;

namespace BlazorWasm.UnitTests
{
    public class CounterTests
    {
        [Fact]
        public void Counter_IncrementsWhenButtonClicked()
        {
            // Arrange
            var counter = new CounterLogic();

            // Act
            counter.Increment();

            // Assert
            Assert.Equal(1, counter.CurrentCount);
        }

        [Fact]
        public void Counter_StartsAtZero()
        {
            // Arrange
            var counter = new CounterLogic();

            // Assert
            Assert.Equal(0, counter.CurrentCount);
        }
    }

    // Simple class to represent the counter logic
    public class CounterLogic
    {
        public int CurrentCount { get; private set; } = 0;

        public void Increment()
        {
            CurrentCount++;
        }
    }
}