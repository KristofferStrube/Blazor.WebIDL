using Microsoft.JSInterop;

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

internal class Callback<TArg1> where TArg1 : IJSCreatable<TArg1>
{
    private readonly IJSRuntime jSRuntime;
    private readonly Func<TArg1, Task> function;

    public Callback(IJSRuntime jSRuntime, Func<TArg1, Task> function)
    {
        this.jSRuntime = jSRuntime;
        this.function = function;
    }

    [JSInvokable]
    public async Task InvokeCallback(IJSObjectReference t1JSReference)
    {
        CreationOptions options = new()
        {
            DisposesJSReference = true
        };

        await function.Invoke(await TArg1.CreateAsync(jSRuntime, t1JSReference, options));
    }
}

internal class Callback<TArg1, TArg2> where TArg1 : IJSCreatable<TArg1> where TArg2 : IJSCreatable<TArg2>
{
    private readonly IJSRuntime jSRuntime;
    private readonly Func<TArg1, TArg2, Task> function;

    public Callback(IJSRuntime jSRuntime, Func<TArg1, TArg2, Task> function)
    {
        this.jSRuntime = jSRuntime;
        this.function = function;
    }

    [JSInvokable]
    public async Task InvokeCallback(IJSObjectReference t1JSReference, IJSObjectReference t2JSReference)
    {
        CreationOptions options1 = new()
        {
            DisposesJSReference = true
        };

        CreationOptions options2 = new()
        {
            DisposesJSReference = true
        };

        await function.Invoke(await TArg1.CreateAsync(jSRuntime, t1JSReference, options1), await TArg2.CreateAsync(jSRuntime, t2JSReference, options2));
    }
}

internal class StructCallback<TArg1> where TArg1 : struct
{
    private readonly Func<TArg1, Task> function;

    public StructCallback(Func<TArg1, Task> function)
    {
        this.function = function;
    }

    [JSInvokable]
    public async Task InvokeCallback(TArg1 arg1)
    {
        await function.Invoke(arg1);
    }
}

internal class StructCallback<TArg1, TArg2> where TArg1 : struct where TArg2 : struct
{
    private readonly Func<TArg1, TArg2, Task> function;

    public StructCallback(Func<TArg1, TArg2, Task> function)
    {
        this.function = function;
    }

    [JSInvokable]
    public async Task InvokeCallback(TArg1 arg1, TArg2 arg2)
    {
        await function.Invoke(arg1, arg2);
    }
}