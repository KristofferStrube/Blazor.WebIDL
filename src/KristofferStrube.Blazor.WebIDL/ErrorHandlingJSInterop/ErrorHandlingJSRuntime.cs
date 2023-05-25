using Microsoft.JSInterop;
using Microsoft.JSInterop.Infrastructure;
using System.Diagnostics.CodeAnalysis;

namespace KristofferStrube.Blazor.WebIDL;

/// <inheritdoc cref="IErrorHandlingJSRuntime"/>
public class ErrorHandlingJSRuntime : ErrorHandlingJSInterop, IErrorHandlingJSRuntime
{
    /// <inheritdoc />
    public async ValueTask InvokeVoidAsync(string identifier, params object?[]? args)
    {
        await InvokeVoidAsync(identifier, CancellationToken.None, args);
    }

    /// <inheritdoc />
    public async ValueTask InvokeVoidAsync(string identifier, CancellationToken cancellationToken, params object?[]? args)
    {
        await InvokeAsync<IJSVoidResult>(identifier, cancellationToken, args);
    }

    /// <inheritdoc />
    public async ValueTask<TValue> InvokeAsync<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.PublicProperties)] TValue>(string identifier, params object?[]? args)
    {
        return await InvokeAsync<TValue>(identifier, CancellationToken.None, args);
    }

    /// <inheritdoc />
    public async ValueTask<TValue> InvokeAsync<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.PublicProperties)] TValue>(string identifier, CancellationToken cancellationToken, params object?[]? args)
    {
        if (Helper is null)
        {
            throw new MissingErrorHandlingJSInteropSetupException();
        }

        try
        {
            if (typeof(TValue).IsAssignableTo(typeof(IJSObjectReference)))
            {
                IJSObjectReference result = await Helper.InvokeAsync<IJSObjectReference>("callAsyncGlobalMethod", cancellationToken, identifier, args);
                return (TValue)ConstructErrorHandlingInstanceIfJSObjectReference(result);
            }
            else
            {
                TValue? result = await Helper.InvokeAsync<TValue>("callAsyncGlobalMethod", cancellationToken, identifier, args);
                return ConstructErrorHandlingInstanceIfJSObjectReference(result);
            }
        }
        catch (JSException exception)
        {
            if (UnpackMessageOfExeption(exception) is not Error { } error)
            {
                throw;
            }
            throw MapToWebIDLException(error, exception);
        }
    }
}
