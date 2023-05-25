using Microsoft.JSInterop;
using Microsoft.JSInterop.Infrastructure;
using System.Diagnostics.CodeAnalysis;

namespace KristofferStrube.Blazor.WebIDL;

/// <inheritdoc cref="IErrorHandlingJSObjectReference"/>
public class ErrorHandlingJSObjectReference : ErrorHandlingJSInterop, IErrorHandlingJSObjectReference
{
    /// <inheritdoc/>
    public IJSObjectReference JSReference { get; }

    /// <summary>
    /// Constructs a Error Handling version of a <see cref="IJSObjectReference" />.
    /// </summary>
    /// <param name="jSReference">A JS reference that you would like to make error handling calls with.</param>
    public ErrorHandlingJSObjectReference(IJSObjectReference jSReference)
    {
        JSReference = jSReference;
    }

    /// <inheritdoc/>
    public async ValueTask InvokeVoidAsync(string identifier, params object?[]? args)
    {
        await InvokeVoidAsync(identifier, CancellationToken.None, args);
    }

    /// <inheritdoc/>
    public async ValueTask InvokeVoidAsync(string identifier, CancellationToken cancellationToken, params object?[]? args)
    {
        await InvokeAsync<IJSVoidResult>(identifier, cancellationToken, args);
    }

    /// <inheritdoc/>
    public async ValueTask<TValue> InvokeAsync<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.PublicProperties)] TValue>(string identifier, object?[]? args)
    {
        return await InvokeAsync<TValue>(identifier, CancellationToken.None, args);
    }

    /// <inheritdoc/>
    public async ValueTask<TValue> InvokeAsync<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.PublicProperties)] TValue>(string identifier, CancellationToken cancellationToken, object?[]? args)
    {
        if (Helper is null)
        {
            throw new MissingErrorHandlingJSInteropSetupException();
        }

        try
        {
            if (typeof(TValue).IsAssignableTo(typeof(IJSObjectReference)))
            {
                IJSObjectReference result = await Helper.InvokeAsync<IJSObjectReference>("callAsyncInstanceMethod", cancellationToken, JSReference, identifier, args);
                return (TValue)ConstructErrorHandlingInstanceIfJSObjectReference(result);
            }
            else
            {
                TValue? result = await Helper.InvokeAsync<TValue>("callAsyncInstanceMethod", cancellationToken, JSReference, identifier, args);
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

    /// <inheritdoc/>
    public ValueTask DisposeAsync()
    {
        return JSReference.DisposeAsync();
    }
}
