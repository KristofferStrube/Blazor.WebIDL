using Microsoft.JSInterop;
using Microsoft.JSInterop.Infrastructure;
using System.Diagnostics.CodeAnalysis;

namespace KristofferStrube.Blazor.WebIDL;

/// <inheritdoc cref="IErrorHandlingJSInProcessObjectReference"/>
public class ErrorHandlingJSInProcessObjectReference : ErrorHandlingJSObjectReference, IErrorHandlingJSInProcessObjectReference
{
    /// <inheritdoc/>
    public new IJSInProcessObjectReference JSReference { get; }

    /// <summary>
    /// Creates a new instance of a <see cref="IErrorHandlingJSInProcessRuntime"/>.
    /// </summary>
    /// <param name="jSReference">The JS object that the methods will be called on.</param>
    /// <returns>A new instance of a <see cref="IErrorHandlingJSInProcessRuntime"/></returns>
    public ErrorHandlingJSInProcessObjectReference(IJSInProcessObjectReference jSReference) : base(jSReference)
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
                IJSObjectReference result = inProcessHelper.Invoke<IJSObjectReference>("callInstanceMethod", JSReference, identifier, args);
                return (TValue)ConstructErrorHandlingInstanceIfJSObjectReference(result);
            }
            else
            {
                TValue? result = inProcessHelper.Invoke<TValue>("callInstanceMethod", JSReference, identifier, args);
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

    /// <inheritdoc/>
    public void Dispose()
    {
        JSReference.Dispose();
    }
}
