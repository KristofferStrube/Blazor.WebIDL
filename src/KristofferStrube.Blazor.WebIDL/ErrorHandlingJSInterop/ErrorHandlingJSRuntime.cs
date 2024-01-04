using Microsoft.JSInterop;
using Microsoft.JSInterop.Infrastructure;
using System.Diagnostics.CodeAnalysis;

namespace KristofferStrube.Blazor.WebIDL;

/// <inheritdoc cref="IErrorHandlingJSRuntime"/>
public class ErrorHandlingJSRuntime : ErrorHandlingJSInterop, IErrorHandlingJSRuntime
{
    private readonly IJSRuntime jSRuntime;
    private readonly Lazy<Task<IJSObjectReference>> helperTask;

    /// <summary>
    /// Constructs a Error Handling version of a <see cref="IJSRuntime" />.
    /// </summary>
    /// <param name="jSRuntime">The <see cref="IJSRuntime"/>.</param>
    public ErrorHandlingJSRuntime(IJSRuntime jSRuntime)
    {
        this.jSRuntime = jSRuntime;
        helperTask = new(jSRuntime.GetHelperAsync);
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
    public async ValueTask<TValue> InvokeAsync<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.PublicProperties)] TValue>(string identifier, CancellationToken cancellationToken, params object?[]? args)
    {
        IJSObjectReference helper = await helperTask.Value;
        try
        {
            if (typeof(TValue).IsAssignableTo(typeof(IJSObjectReference)))
            {
                IJSObjectReference result = await helper.InvokeAsync<IJSObjectReference>(CallAsyncGlobalMethod, cancellationToken, ExtraErrorProperties, identifier, args);
                return (TValue)ConstructErrorHandlingInstanceIfJSObjectReference(jSRuntime, result);
            }
            else
            {
                TValue? result = await helper.InvokeAsync<TValue>(CallAsyncGlobalMethod, cancellationToken, ExtraErrorProperties, identifier, args);
                return ConstructErrorHandlingInstanceIfJSObjectReference(jSRuntime, result);
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
}
