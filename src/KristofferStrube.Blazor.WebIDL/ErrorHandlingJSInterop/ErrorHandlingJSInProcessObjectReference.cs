using Microsoft.JSInterop;
using Microsoft.JSInterop.Infrastructure;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace KristofferStrube.Blazor.WebIDL;

/// <inheritdoc cref="IErrorHandlingJSInProcessObjectReference"/>
[JsonConverter(typeof(JSObjectReferenceJsonConverter))]
public class ErrorHandlingJSInProcessObjectReference : ErrorHandlingJSInterop, IErrorHandlingJSInProcessObjectReference
{
    private const string CallInstanceMethod = "callInstanceMethod";

    /// <inheritdoc/>
    public IJSObjectReference JSReference { get; }

    /// <summary>
    /// Creates a new instance of a <see cref="IErrorHandlingJSInProcessRuntime"/>.
    /// </summary>
    /// <param name="jSReference">The JS object that the methods will be called on.</param>
    /// <returns>A new instance of a <see cref="IErrorHandlingJSInProcessRuntime"/></returns>
    public ErrorHandlingJSInProcessObjectReference(IJSInProcessObjectReference jSReference)
    {
        JSReference = jSReference;
    }

    /// <inheritdoc/>
    public void InvokeVoid(string identifier, params object?[]? args)
    {
        Invoke<IJSVoidResult>(identifier, args);
    }

    /// <inheritdoc/>
    public TValue Invoke<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.PublicProperties)] TValue>(string identifier, params object?[]? args)
    {
        if (Helper is null)
        {
            throw new MissingErrorHandlingJSInteropSetupException();
        }

        if (Helper is not IJSInProcessObjectReference inProcessHelper)
        {
            throw new InvalidOperationException("Tried to make syncronous invocation in an environment that does not allow In-Process JS invocations.");
        }

        try
        {
            if (typeof(TValue).IsAssignableTo(typeof(IJSObjectReference)))
            {
                IJSObjectReference result = inProcessHelper.Invoke<IJSObjectReference>(CallInstanceMethod, ExtraErrorProperties, JSReference, identifier, args);
                return (TValue)ConstructErrorHandlingInstanceIfJSInProcessObjectReference(result);
            }
            else
            {
                TValue? result = inProcessHelper.Invoke<TValue>(CallInstanceMethod, ExtraErrorProperties, JSReference, identifier, args);
                return ConstructErrorHandlingInstanceIfJSInProcessObjectReference(result);
            }
        }
        catch (JSException exception)
        {
            if (UnpackMessageOfExeption(exception) is not JSError { } error)
            {
                throw;
            }
            throw MapToWebIDLException(error, exception);
        }
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        GC.SuppressFinalize(this);
        if (JSReference is IJSInProcessObjectReference inProcess)
        {
            inProcess.Dispose();
        }
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
    public ValueTask<TValue> InvokeAsync<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.PublicProperties)] TValue>(string identifier, CancellationToken cancellationToken, object?[]? args)
    {
        return ValueTask.FromResult(Invoke<TValue>(identifier, cancellationToken, args));
    }

    /// <inheritdoc/>
    public ValueTask DisposeAsync()
    {
        GC.SuppressFinalize(this);
        return JSReference.DisposeAsync();
    }
}
