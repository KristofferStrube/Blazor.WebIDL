using Microsoft.JSInterop;

namespace KristofferStrube.Blazor.WebIDL;

/// <summary>
/// A wrapper that points to some attribute of a object. It can be used to check the type of that attribute and fetch it as either a <see cref="object" /> or a concrete type.
/// </summary>
[IJSWrapperConverter]
public class ValueReference : IJSCreatable<ValueReference>, IAsyncDisposable
{
    /// <summary>
    /// A lazily loaded task that evaluates to a helper module instance from the Blazor.WebIDL library.
    /// </summary>
    protected readonly Lazy<Task<IJSObjectReference>> helperTask;

    /// <inheritdoc/>
    public IJSObjectReference JSReference { get; }

    /// <inheritdoc/>
    public IJSRuntime JSRuntime { get; }

    /// <summary>
    /// The attribute of the <see cref="JSReference"/> that this <see cref="ValueReference"/> points to.
    /// </summary>
    public object Attribute { get; set; }

    /// <summary>
    /// A mapper from JS type names to .NET <see cref="Type"/>s.
    /// </summary>
    public Dictionary<string, Type?> TypeMapper { get; set; } = new()
        {
            { "number", typeof(float) },
            { "boolean", typeof(bool) },
            { "string", typeof(string) },
            { "object", typeof(object) },
            { "undefined", null },
        };

    /// <summary>
    /// A mapper from JS type names to creator methods for that type.
    /// </summary>
    public Dictionary<string, Func<Task<object?>>> ValueMapper { get; set; }

    /// <inheritdoc cref="IJSCreatable{T}.CreateAsync(IJSRuntime, IJSObjectReference)"/>
    /// <param name="jSRuntime"></param>
    /// <param name="jSReference"></param>
    /// <param name="attribute">The attribute name that should be accessed.</param>
    public static Task<ValueReference> CreateAsync(IJSRuntime jSRuntime, IJSObjectReference jSReference, object attribute)
    {
        return Task.FromResult(new ValueReference(jSRuntime, jSReference, attribute));
    }

    /// <inheritdoc/>
    public static Task<ValueReference> CreateAsync(IJSRuntime jSRuntime, IJSObjectReference jSReference)
    {
        return Task.FromResult(new ValueReference(jSRuntime, jSReference, "value"));
    }

    /// <inheritdoc cref="CreateAsync(IJSRuntime, IJSObjectReference, object)" />
    public ValueReference(IJSRuntime jSRuntime, IJSObjectReference jSReference, object attribute)
    {
        helperTask = new(jSRuntime.GetHelperAsync);
        JSRuntime = jSRuntime;
        JSReference = jSReference;
        Attribute = attribute;

        ValueMapper = new()
        {
            { "number", async () => await GetValueAsync<float>() },
            { "boolean", async () => await GetValueAsync<bool>() },
            { "string", async () => await GetValueAsync<string>() },
            { "object", async () => await GetValueAsync<object>() },
            { "undefined", () => Task.FromResult<object?>(null) },
        };
    }

    /// <summary>
    /// Gets the value as an object but could be any registered type underneath. If the JS value is <c>undefined</c> then the returned value is <see langword="null"/>.
    /// </summary>
    /// <returns>Returns a object that is the best matching type if we have the JS type registed in <see cref="ValueMapper"/>.</returns>
    public async Task<object?> GetValueAsync()
    {
        string typeString = await GetTypeNameAsync();
        if (ValueMapper.TryGetValue(typeString, out Func<Task<object?>>? creator))
        {
            return await creator();
        }
        return await GetValueAsync<IJSObjectReference>();
    }

    /// <summary>
    /// Gets the value as the defined type.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns>Returns the property as a <typeparamref name="T"/></returns>
    public async Task<T> GetValueAsync<T>()
    {
        IJSObjectReference helper = await helperTask.Value;
        return await helper.InvokeAsync<T>("getAttribute", JSReference, Attribute);
    }

    /// <summary>
    /// Gets the value as a reference and constructs the <typeparamref name="T"/> type.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns>Returns the property as a <typeparamref name="T"/></returns>
    public async Task<T> GetCreatableAsync<T>() where T : IJSCreatable<T>
    {
        IJSObjectReference helper = await helperTask.Value;
        IJSObjectReference jSInstance = await helper.InvokeAsync<IJSObjectReference>("getAttribute", JSReference, Attribute);
        return await T.CreateAsync(JSRuntime, jSInstance);
    }

    /// <summary>
    /// Gets the type of the value. If the JS value is undefined then the type is <see langword="null"/>.
    /// </summary>
    /// <returns>The <see cref="Type"/> of the value.</returns>
    public async Task<Type?> GetTypeAsync()
    {
        string typeString = await GetTypeNameAsync();
        if (TypeMapper.TryGetValue(typeString, out Type? type))
        {
            return type;
        }
        return typeof(IJSObjectReference);
    }

    /// <summary>
    /// Gets the name of JS type.
    /// </summary>
    /// <returns>The string representation of the name of the JS type.</returns>
    public async Task<string> GetTypeNameAsync()
    {
        IJSObjectReference helper = await helperTask.Value;
        return await helper.InvokeAsync<string>("valuePropertiesType", JSReference, Attribute);
    }

    /// <summary>
    /// Disposes the value wrapper.
    /// </summary>
    public async ValueTask DisposeAsync()
    {
        if (helperTask.IsValueCreated)
        {
            IJSObjectReference module = await helperTask.Value;
            await module.DisposeAsync();
        }
        GC.SuppressFinalize(this);
    }
}
