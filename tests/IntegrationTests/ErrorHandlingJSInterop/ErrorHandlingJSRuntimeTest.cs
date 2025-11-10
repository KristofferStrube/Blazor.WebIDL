using KristofferStrube.Blazor.WebIDL.Exceptions;
using Microsoft.JSInterop;

namespace IntegrationTests.ErrorHandlingJSInterop;

public class ErrorHandlingJSRuntimeTest(string browserName) : BlazorTest(browserName)
{
    [Test]
    public async Task InvokeAsync_CanThrow()
    {
        // Act
        Func<Task<IJSObjectReference>> action = async () => await ErrorHandlingJSRuntime.InvokeAsync<IJSObjectReference>("window.attributeThatDoesntExist.someMethod");

        // Assert
        _ = await action.Should().ThrowAsync<ReferenceErrorException>();
    }

    [Test]
    public async Task InvokeAsync_CanReturnNumber()
    {
        // Act
        double random = await ErrorHandlingJSRuntime.InvokeAsync<double>("Math.random");

        // Assert
        _ = random.Should().BeLessThanOrEqualTo(1).And.BeGreaterThan(0);
    }

    [Test]
    public async Task InvokeAsync_CanReturnString()
    {
        // Act
        string result = await ErrorHandlingJSRuntime.InvokeAsync<string>("toString");

        // Assert
        _ = result.Should().Be("[object Window]");
    }

    [Test]
    public async Task InvokeAsync_CanReturnObjectReference()
    {
        // Act
        IJSObjectReference element = await ErrorHandlingJSRuntime.InvokeAsync<IJSObjectReference>("document.createElement", "div");

        // Assert
        _ = element.Should().BeAssignableTo<IJSObjectReference>();
    }
}
