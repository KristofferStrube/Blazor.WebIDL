using Microsoft.JSInterop;

namespace KristofferStrube.Blazor.WebIDL;

[IJSWrapperConverter]
public class Pair : IJSCreatable<Pair>
{
    /// <summary>
    /// A lazily loaded task that evaluates to a helper module instance from the Blazor.WebIDL library.
    /// </summary>
    protected readonly Lazy<Task<IJSObjectReference>> helperTask;

    /// <inheritdoc/>
    public IJSObjectReference JSReference { get; }
    /// <inheritdoc/>
    public IJSRuntime JSRuntime { get; }

    /// <inheritdoc/>
    public bool DisposesJSReference { get; }

    /// <inheritdoc/>
    public static async Task<Pair> CreateAsync(IJSRuntime jSRuntime, IJSObjectReference jSReference)
    {
        return await Task.FromResult(new Pair(jSRuntime, jSReference, new()));
    }

    /// <inheritdoc/>
    public static async Task<Pair> CreateAsync(IJSRuntime jSRuntime, IJSObjectReference jSReference, CreationOptions options)
    {
        return await Task.FromResult(new Pair(jSRuntime, jSReference, options));
    }

    /// <inheritdoc cref="CreateAsync(IJSRuntime, IJSObjectReference, CreationOptions)" />
    protected Pair(IJSRuntime jSRuntime, IJSObjectReference jSReference, CreationOptions options)
    {
        helperTask = new(jSRuntime.GetHelperAsync);
        JSRuntime = jSRuntime;
        JSReference = jSReference;
        DisposesJSReference = options.DisposesJSReference;
    }

    public async Task<TKey> GetKeyAsync<TKey>() where TKey : IJSCreatable<TKey>
    {
        IJSObjectReference helper = await helperTask.Value;
        CreationOptions options = new()
        {
            DisposesJSReference = true
        };

        return await TKey.CreateAsync(JSRuntime, await helper.InvokeAsync<IJSObjectReference>("getAttribute", JSReference, 0), options);
    }

    public async Task<TValue> GetValueAsync<TValue>() where TValue : IJSCreatable<TValue>
    {
        IJSObjectReference helper = await helperTask.Value;
        CreationOptions options = new()
        {
            DisposesJSReference = true
        };

        return await TValue.CreateAsync(JSRuntime, await helper.InvokeAsync<IJSObjectReference>("getAttribute", JSReference, 1), options);
    }

    /// <inheritdoc/>
    public async ValueTask DisposeAsync()
    {
        if (helperTask.IsValueCreated)
        {
            IJSObjectReference module = await helperTask.Value;
            await module.DisposeAsync();
        }
        await IJSWrapper.DisposeJSReference(this);
        GC.SuppressFinalize(this);
    }
}

[IJSWrapperConverter]
public class Pair<TKey, TValue> : IJSCreatable<Pair<TKey, TValue>> where TKey : IJSCreatable<TKey> where TValue : IJSCreatable<TValue>
{
    /// <summary>
    /// A lazily loaded task that evaluates to a helper module instance from the Blazor.WebIDL library.
    /// </summary>
    protected readonly Lazy<Task<IJSObjectReference>> helperTask;

    /// <inheritdoc/>
    public IJSObjectReference JSReference { get; }
    /// <inheritdoc/>
    public IJSRuntime JSRuntime { get; }

    /// <inheritdoc/>
    public bool DisposesJSReference { get; }

    /// <inheritdoc/>
    public static async Task<Pair<TKey, TValue>> CreateAsync(IJSRuntime jSRuntime, IJSObjectReference jSReference)
    {
        return await Task.FromResult(new Pair<TKey, TValue>(jSRuntime, jSReference, new()));
    }

    /// <inheritdoc/>
    public static async Task<Pair<TKey, TValue>> CreateAsync(IJSRuntime jSRuntime, IJSObjectReference jSReference, CreationOptions options)
    {
        return await Task.FromResult(new Pair<TKey, TValue>(jSRuntime, jSReference, options));
    }

    /// <inheritdoc cref="CreateAsync(IJSRuntime, IJSObjectReference, CreationOptions)" />
    protected Pair(IJSRuntime jSRuntime, IJSObjectReference jSReference, CreationOptions options)
    {
        helperTask = new(jSRuntime.GetHelperAsync);
        JSRuntime = jSRuntime;
        JSReference = jSReference;
        DisposesJSReference = options.DisposesJSReference;
    }

    public async Task<TKey> GetKeyAsync()
    {
        IJSObjectReference helper = await helperTask.Value;
        CreationOptions options = new()
        {
            DisposesJSReference = true
        };

        return await TKey.CreateAsync(JSRuntime, await helper.InvokeAsync<IJSObjectReference>("getAttribute", JSReference, 0), options);
    }

    public async Task<TValue> GetValueAsync()
    {
        IJSObjectReference helper = await helperTask.Value;
        CreationOptions options = new()
        {
            DisposesJSReference = true
        };

        return await TValue.CreateAsync(JSRuntime, await helper.InvokeAsync<IJSObjectReference>("getAttribute", JSReference, 1), options);
    }

    /// <inheritdoc/>
    public async ValueTask DisposeAsync()
    {
        if (helperTask.IsValueCreated)
        {
            IJSObjectReference module = await helperTask.Value;
            await module.DisposeAsync();
        }
        await IJSWrapper.DisposeJSReference(this);
        GC.SuppressFinalize(this);
    }
}

[IJSWrapperConverter]
public class StructKeyJSCreatableValuePair<TKey, TValue> : IJSCreatable<StructKeyJSCreatableValuePair<TKey, TValue>> where TKey : struct where TValue : IJSCreatable<TValue>
{
    /// <summary>
    /// A lazily loaded task that evaluates to a helper module instance from the Blazor.WebIDL library.
    /// </summary>
    protected readonly Lazy<Task<IJSObjectReference>> helperTask;

    /// <inheritdoc/>
    public IJSObjectReference JSReference { get; }
    /// <inheritdoc/>
    public IJSRuntime JSRuntime { get; }

    /// <inheritdoc/>
    public bool DisposesJSReference { get; }

    /// <inheritdoc/>
    public static async Task<StructKeyJSCreatableValuePair<TKey, TValue>> CreateAsync(IJSRuntime jSRuntime, IJSObjectReference jSReference)
    {
        return await Task.FromResult(new StructKeyJSCreatableValuePair<TKey, TValue>(jSRuntime, jSReference, new()));
    }

    /// <inheritdoc/>
    public static async Task<StructKeyJSCreatableValuePair<TKey, TValue>> CreateAsync(IJSRuntime jSRuntime, IJSObjectReference jSReference, CreationOptions options)
    {
        return await Task.FromResult(new StructKeyJSCreatableValuePair<TKey, TValue>(jSRuntime, jSReference, options));
    }

    /// <inheritdoc cref="CreateAsync(IJSRuntime, IJSObjectReference, CreationOptions)" />
    protected StructKeyJSCreatableValuePair(IJSRuntime jSRuntime, IJSObjectReference jSReference, CreationOptions options)
    {
        helperTask = new(jSRuntime.GetHelperAsync);
        JSRuntime = jSRuntime;
        JSReference = jSReference;
        DisposesJSReference = options.DisposesJSReference;
    }

    public async Task<TKey> GetKeyAsync()
    {
        IJSObjectReference helper = await helperTask.Value;
        CreationOptions options = new()
        {
            DisposesJSReference = true
        };

        return await helper.InvokeAsync<TKey>("getAttribute", JSReference, 0);
    }

    public async Task<TValue> GetValueAsync()
    {
        IJSObjectReference helper = await helperTask.Value;
        CreationOptions options = new()
        {
            DisposesJSReference = true
        };

        return await TValue.CreateAsync(JSRuntime, await helper.InvokeAsync<IJSObjectReference>("getAttribute", JSReference, 1), options);
    }

    /// <inheritdoc/>
    public async ValueTask DisposeAsync()
    {
        if (helperTask.IsValueCreated)
        {
            IJSObjectReference module = await helperTask.Value;
            await module.DisposeAsync();
        }
        await IJSWrapper.DisposeJSReference(this);
        GC.SuppressFinalize(this);
    }
}

[IJSWrapperConverter]
public class StructKeyStructValuePair<TKey, TValue> : IJSCreatable<StructKeyStructValuePair<TKey, TValue>> where TKey : struct where TValue : struct
{
    /// <summary>
    /// A lazily loaded task that evaluates to a helper module instance from the Blazor.WebIDL library.
    /// </summary>
    protected readonly Lazy<Task<IJSObjectReference>> helperTask;

    /// <inheritdoc/>
    public IJSObjectReference JSReference { get; }
    /// <inheritdoc/>
    public IJSRuntime JSRuntime { get; }

    /// <inheritdoc/>
    public bool DisposesJSReference { get; }

    /// <inheritdoc/>
    public static async Task<StructKeyStructValuePair<TKey, TValue>> CreateAsync(IJSRuntime jSRuntime, IJSObjectReference jSReference)
    {
        return await Task.FromResult(new StructKeyStructValuePair<TKey, TValue>(jSRuntime, jSReference, new()));
    }

    /// <inheritdoc/>
    public static async Task<StructKeyStructValuePair<TKey, TValue>> CreateAsync(IJSRuntime jSRuntime, IJSObjectReference jSReference, CreationOptions options)
    {
        return await Task.FromResult(new StructKeyStructValuePair<TKey, TValue>(jSRuntime, jSReference, options));
    }

    /// <inheritdoc cref="CreateAsync(IJSRuntime, IJSObjectReference, CreationOptions)" />
    protected StructKeyStructValuePair(IJSRuntime jSRuntime, IJSObjectReference jSReference, CreationOptions options)
    {
        helperTask = new(jSRuntime.GetHelperAsync);
        JSRuntime = jSRuntime;
        JSReference = jSReference;
        DisposesJSReference = options.DisposesJSReference;
    }

    public async Task<TKey> GetKeyAsync()
    {
        IJSObjectReference helper = await helperTask.Value;
        CreationOptions options = new()
        {
            DisposesJSReference = true
        };

        return await helper.InvokeAsync<TKey>("getAttribute", JSReference, 0);
    }

    public async Task<TValue> GetValueAsync()
    {
        IJSObjectReference helper = await helperTask.Value;
        CreationOptions options = new()
        {
            DisposesJSReference = true
        };

        return await helper.InvokeAsync<TValue>("getAttribute", JSReference, 1);
    }

    /// <inheritdoc/>
    public async ValueTask DisposeAsync()
    {
        if (helperTask.IsValueCreated)
        {
            IJSObjectReference module = await helperTask.Value;
            await module.DisposeAsync();
        }
        await IJSWrapper.DisposeJSReference(this);
        GC.SuppressFinalize(this);
    }
}