# BlazorWasm.UnitTests

This project contains unit tests for the Blazor WebAssembly application.

## Tests Included

1. **CounterTests** - Tests for the Counter component logic
2. **WeatherTests** - Tests for weather forecast calculations
3. **AppTests** - Tests for the main application component
4. **LayoutTests** - Tests for the main layout components

## Test Structure

The tests use xUnit for testing framework and focus on:
- Business logic validation
- Data transformation calculations
- Component instantiation

## Running Tests

To run the tests:
```bash
dotnet test BlazorWasm.UnitTests/BlazorWasm.UnitTests.csproj
```

## Note on Blazor Testing

Due to limitations with Blazor WebAssembly testing packages in .NET 10, we've implemented:
- Simple logic classes that mirror the component behavior
- Direct unit testing of business logic
- Avoided complex component rendering tests

This approach provides good test coverage for the core functionality while working within the available tooling constraints.