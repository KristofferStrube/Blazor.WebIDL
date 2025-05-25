using KristofferStrube.Blazor.WebIDL.Exceptions;
using Microsoft.JSInterop;

namespace IntegrationTests;

public class ErrorHandlingJSRuntimeTest(string browserName) : JSInteropBlazorTest(browserName)
{
    [Test]
    public async Task InvokeAsync_CanThrow()
    {
        // Arrange
        AfterRenderAsync = async () =>
        {
            IErrorHandlingJSRuntime errorHandlingJSRuntime = new ErrorHandlingJSRuntime(EvaluationContext.JSRuntime);
            return await errorHandlingJSRuntime.InvokeAsync<IJSObjectReference>("window.attributeThatDoesntExist.someMethod");
        };

        // Act
        await OnAfterRerenderAsync();

        // Assert
        _ = EvaluationContext.Exception.Should().BeOfType<ReferenceErrorException>();
    }

    [Test]
    public async Task InvokeAsync_CanReturnNumber()
    {
        // Arrange
        AfterRenderAsync = async () =>
        {
            IErrorHandlingJSRuntime errorHandlingJSRuntime = new ErrorHandlingJSRuntime(EvaluationContext.JSRuntime);
            return await errorHandlingJSRuntime.InvokeAsync<double>("Math.random");
        };

        // Act
        await OnAfterRerenderAsync();

        // Assert
        _ = EvaluationContext.Result.Should().BeOfType<double>();
    }

    [Test]
    public async Task InvokeAsync_CanReturnString()
    {
        // Arrange
        AfterRenderAsync = async () =>
        {
            IErrorHandlingJSRuntime errorHandlingJSRuntime = new ErrorHandlingJSRuntime(EvaluationContext.JSRuntime);
            return await errorHandlingJSRuntime.InvokeAsync<string>("toString");
        };

        // Act
        await OnAfterRerenderAsync();

        // Assert
        _ = EvaluationContext.Result.Should().BeOfType<string>();
    }

    [Test]
    public async Task InvokeAsync_CanReturnObjectReference()
    {
        // Arrange
        AfterRenderAsync = async () =>
        {
            IErrorHandlingJSRuntime errorHandlingJSRuntime = new ErrorHandlingJSRuntime(EvaluationContext.JSRuntime);
            return await errorHandlingJSRuntime.InvokeAsync<ErrorHandlingJSObjectReference>("document.createElement", "div");
        };

        // Act
        await OnAfterRerenderAsync();

        // Assert
        _ = EvaluationContext.Result.Should().BeAssignableTo<IJSObjectReference>();
    }
}
