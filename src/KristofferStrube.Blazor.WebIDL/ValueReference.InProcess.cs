using Microsoft.JSInterop;

namespace KristofferStrube.Blazor.WebIDL;

/// <inheritdoc/>
public class ValueReferenceInProcess : ValueReference
{
    /// <summary>
    /// A lazily loaded task that evaluates to a helper module instance from the Blazor.WebIDL library.
    /// </summary>
    protected readonly IJSInProcessObjectReference inProcessHelper;

    /// <summary>
    /// A mapper from JS type names to creator methods for that type.
    /// </summary>
    public Dictionary<string, Func<object?>> ValueMapperInProcess { get; set; }

    /// <inheritdoc/>
    public static new async Task<ValueReferenceInProcess> CreateAsync(IJSRuntime jSRuntime, IJSObjectReference jSReference, object attribute)
    {
        IJSInProcessObjectReference inProcessHelper = await jSRuntime.GetInProcessHelperAsync();
        return new(jSRuntime, inProcessHelper, jSReference, attribute);
    }

    private ValueReferenceInProcess(IJSRuntime jSRuntime, IJSInProcessObjectReference inProcessHelper, IJSObjectReference jSReference, object attribute) : base(jSRuntime, jSReference, attribute, new())
    {
        this.inProcessHelper = inProcessHelper;

        ValueMapperInProcess = new()
        {
            { "number", () => GetValue<float>() },
            { "boolean", () => GetValue<bool>() },
            { "string", () => GetValue<string>() },
            { "object", () => GetValue<object>() },
            { "undefined", () => null },
        };
    }

    /// <summary>
    /// Gets the value as an object but could be any registered type underneath. If the JS value is <c>undefined</c> then the returned value is <see langword="null"/>.
    /// </summary>
    /// <returns>Returns a object that is the best matching type if we have the JS type registed in <see cref="ValueReference.ValueMapper"/>.</returns>
    public object? GetValue()
    {
        if (ValueMapperInProcess.TryGetValue(TypeName, out Func<object?>? creator))
        {
            return creator();
        }
        return GetValue<IJSObjectReference>();
    }

    /// <summary>
    /// Gets the value as the defined type.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns>Returns the property as a <typeparamref name="T"/></returns>
    public T GetValue<T>()
    {
        return inProcessHelper.Invoke<T>("getAttribute", JSReference, Attribute);
    }

    /// <summary>
    /// Gets the value as a reference and constructs the <typeparamref name="T"/> type.
    /// </summary>
    /// <typeparam name="TInProcess"></typeparam>
    /// <typeparam name="T"></typeparam>
    /// <returns>Returns the property as a <typeparamref name="T"/></returns>
    public async Task<T> GetCreatableAsync<TInProcess, T>() where TInProcess : IJSInProcessCreatable<TInProcess, T> where T : IJSCreatable<T>
    {
        IJSObjectReference helper = await helperTask.Value;
        IJSObjectReference jSInstance = await helper.InvokeAsync<IJSObjectReference>("getAttribute", JSReference, Attribute);
        return await TInProcess.CreateAsync(JSRuntime, jSInstance, new() { DisposesJSReference = true });
    }

    /// <summary>
    /// The name of JS type.
    /// </summary>
    public string TypeName => inProcessHelper.Invoke<string>("valuePropertiesType", JSReference, Attribute);
}
