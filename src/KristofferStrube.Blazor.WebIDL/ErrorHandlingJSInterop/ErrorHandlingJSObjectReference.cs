using Microsoft.JSInterop;
using Microsoft.JSInterop.Infrastructure;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace KristofferStrube.Blazor.WebIDL;

/// <inheritdoc cref="IErrorHandlingJSObjectReference"/>
[JsonConverter(typeof(JSObjectReferenceJsonConverter))]
public class ErrorHandlingJSObjectReference : ErrorHandlingJSInterop, IErrorHandlingJSObjectReference
{
    private const string CallAsyncInstanceMethod = "callAsyncInstanceMethod";
    private readonly IJSRuntime jSRuntime;
    private readonly Lazy<Task<IJSObjectReference>> helperTask;

    /// <inheritdoc/>
    public IJSObjectReference JSReference { get; }

    /// <summary>
    /// Constructs a Error Handling version of a <see cref="IJSObjectReference" />.
    /// </summary>
    /// <param name="jSRuntime">The <see cref="IJSRuntime"/>.</param>
    /// <param name="jSReference">A JS reference that you would like to make error handling calls with.</param>
    public ErrorHandlingJSObjectReference(IJSRuntime jSRuntime, IJSObjectReference jSReference)
    {
        this.jSRuntime = jSRuntime;
        helperTask = new(jSRuntime.GetHelperAsync);
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
        IJSObjectReference helper = await helperTask.Value;
        try
        {
            if (typeof(TValue).IsAssignableTo(typeof(IJSObjectReference)))
            {
                IJSObjectReference result = await helper.InvokeAsync<IJSObjectReference>(CallAsyncInstanceMethod, cancellationToken, ExtraErrorProperties, JSReference, identifier, args);
                return (TValue)ConstructErrorHandlingInstanceIfJSObjectReference(jSRuntime, result);
            }
            else
            {
                TValue? result = await helper.InvokeAsync<TValue>(CallAsyncInstanceMethod, cancellationToken, ExtraErrorProperties, JSReference, identifier, args);
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

    /// <inheritdoc/>
    public ValueTask DisposeAsync()
    {
        GC.SuppressFinalize(this);
        return JSReference.DisposeAsync();
    }
}
