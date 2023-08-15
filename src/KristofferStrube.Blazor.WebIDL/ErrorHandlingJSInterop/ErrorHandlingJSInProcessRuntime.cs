using Microsoft.JSInterop;
using Microsoft.JSInterop.Infrastructure;
using System.Diagnostics.CodeAnalysis;

namespace KristofferStrube.Blazor.WebIDL;

/// <inheritdoc cref="IErrorHandlingJSInProcessRuntime"/>
public class ErrorHandlingJSInProcessRuntime : ErrorHandlingJSInterop, IErrorHandlingJSInProcessRuntime
{
    private const string CallGlobalMethod = "callGlobalMethod";

    /// <inheritdoc/>
    public void InvokeVoid(string identifier, params object?[]? args)
    {
        Invoke<object>(identifier, args);
    }

    /// <inheritdoc/>
    public TResult Invoke<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.PublicProperties)] TResult>(string identifier, params object?[]? args)
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
            if (typeof(TResult).IsAssignableTo(typeof(IJSObjectReference)))
            {
                IJSObjectReference result = inProcessHelper.Invoke<IJSObjectReference>(CallGlobalMethod, ExtraErrorProperties, identifier, args);
                return (TResult)ConstructErrorHandlingInstanceIfJSInProcessObjectReference(result);
            }
            else
            {
                TResult? result = inProcessHelper.Invoke<TResult>(CallGlobalMethod, ExtraErrorProperties, identifier, args);
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
    public ValueTask<TValue> InvokeAsync<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.PublicProperties)] TValue>(string identifier, CancellationToken cancellationToken, params object?[]? args)
    {
        return ValueTask.FromResult(Invoke<TValue>(identifier, cancellationToken, args));
    }
}
