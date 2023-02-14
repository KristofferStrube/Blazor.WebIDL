using Microsoft.JSInterop;

namespace KristofferStrube.Blazor.WebIDL;

public class Callback
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

public class Callback<TArg1> where TArg1 : IJSWrapper<TArg1>
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
        await function.Invoke(await TArg1.CreateAsync(jSRuntime, t1JSReference));
    }
}

public class Callback<TArg1, TArg2> where TArg1 : IJSWrapper<TArg1> where TArg2 : IJSWrapper<TArg2>
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
        await function.Invoke(await TArg1.CreateAsync(jSRuntime, t1JSReference), await TArg2.CreateAsync(jSRuntime, t2JSReference));
    }
}

public class StructCallback<TArg1> where TArg1 : struct
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

public class StructCallback<TArg1, TArg2> where TArg1 : struct where TArg2 : struct
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