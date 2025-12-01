using Microsoft.JSInterop;
using System.Text.Json;

namespace KristofferStrube.Blazor.WebIDL;

internal class Callback
{
    private readonly Func<Task> function;

    public Callback(Func<Task> function)
    {
        this.function = function;
    }

    [JSInvokable]
    public async Task InvokeCallback()
    {
        await function.Invoke();
    }
}

internal class OneParameterCallback
{
    private readonly Func<object?, Task> function;

    public OneParameterCallback(Func<object?, Task> function)
    {
        this.function = function;
    }

    [JSInvokable]
    public async Task InvokeCallbackObject(JsonElement arg)
    {
        await function.Invoke(arg);
    }

    [JSInvokable]
    public async Task InvokeCallbackJSObjectReference(IJSObjectReference arg)
    {
        await function.Invoke(arg);
    }
}

internal class TwoParameterCallback
{
    private readonly Func<object?, object?, Task> function;

    public TwoParameterCallback(Func<object?, object?, Task> function)
    {
        this.function = function;
    }

    [JSInvokable]
    public async Task InvokeCallbackObjectObject(JsonElement arg1, JsonElement arg2)
    {
        await function(arg1, arg2);
    }

    [JSInvokable]
    public async Task InvokeCallbackJSObjectReferenceObject(IJSObjectReference arg1, JsonElement arg2)
    {
        await function(arg1, arg2);
    }

    [JSInvokable]
    public async Task InvokeCallbackJSObjectReferenceJSObjectReference(IJSObjectReference arg1, IJSObjectReference arg2)
    {
        await function(arg1, arg2);
    }

    [JSInvokable]
    public async Task InvokeCallbackObjectJSObjectReference(JsonElement arg1, IJSObjectReference arg2)
    {
        await function(arg1, arg2);
    }
}