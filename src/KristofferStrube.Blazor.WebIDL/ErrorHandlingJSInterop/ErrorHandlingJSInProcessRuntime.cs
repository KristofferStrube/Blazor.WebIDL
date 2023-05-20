using Microsoft.JSInterop;
using System.Diagnostics.CodeAnalysis;

namespace KristofferStrube.Blazor.WebIDL;

/// <inheritdoc cref="IErrorHandlingJSInProcessRuntime"/>
public class ErrorHandlingJSInProcessRuntime : ErrorHandlingJSRuntime, IErrorHandlingJSInProcessRuntime
{
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
                IJSObjectReference result = inProcessHelper.Invoke<IJSObjectReference>("callGlobalMethod", identifier, args);
                return (TResult)ConstructErrorHandlingInstanceIfJSObjectReference(result);
            }
            else
            {
                TResult? result = inProcessHelper.Invoke<TResult>("callGlobalMethod", identifier, args);
                return ConstructErrorHandlingInstanceIfJSObjectReference(result);
            }
        }
        catch (JSException exception)
        {
            if (UnpackMessageOfExeption(exception) is not Error { } error)
            {
                throw;
            }
            throw MapToWebIDLException(error, exception, ErrorMapper);
        }
    }
}
