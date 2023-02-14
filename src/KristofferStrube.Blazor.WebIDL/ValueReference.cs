using Microsoft.JSInterop;

namespace KristofferStrube.Blazor.WebIDL;

public class ValueReference : IJSWrapper<ValueReference>
{
    protected readonly Lazy<Task<IJSObjectReference>> helperTask;
    public IJSObjectReference JSReference { get; }
    public IJSRuntime JSRuntime { get; }

    public static async Task<ValueReference> CreateAsync(IJSRuntime jSRuntime, IJSObjectReference jSReference)
    {
        return await Task.FromResult(new ValueReference(jSRuntime, jSReference));
    }

    public ValueReference(IJSRuntime jSRuntime, IJSObjectReference jSReference)
    {
        helperTask = new(jSRuntime.GetHelperAsync);
        JSRuntime = jSRuntime;
        JSReference = jSReference;
    }

    public async Task<object> GetValueAsync()
    {
        var typeString = await GetTypeNameAsync();
        return typeString switch
        {
            "number" => await GetValueAsync<float>(),
            "boolean" => await GetValueAsync<bool>(),
            "string" => await GetValueAsync<string>(),
            "object" => await GetValueAsync<object>(),
            _ => await GetValueAsync<IJSObjectReference>(),
        };
    }

    public async Task<T> GetValueAsync<T>()
    {
        var helper = await helperTask.Value;
        return await helper.InvokeAsync<T>("valuePropertiesValue", JSReference);
    }

    public async Task<Type> GetTypeAsync()
    {
        var typeString = await GetTypeNameAsync();
        return typeString switch
        {
            "number" => typeof(float),
            "boolean" => typeof(bool),
            "string" => typeof(string),
            "object" => typeof(object),
            _ => typeof(IJSObjectReference)
        };
    }
    public async Task<string> GetTypeNameAsync()
    {
        var helper = await helperTask.Value;
        return await helper.InvokeAsync<string>("valuePropertiesType", JSReference);
    }
}
