using Microsoft.JSInterop;

namespace KristofferStrube.Blazor.WebIDL;

/// <summary>
/// An interface that can be used to declare an <see cref="IJSWrapper"/> as being a set that can be read.
/// </summary>
/// <typeparam name="TSet">The type of the set.</typeparam>
/// <typeparam name="TElement">The type of the elements that the set contains.</typeparam>
public interface IReadonlySetlike<TSet, TElement> : IJSWrapper where TSet : IReadonlySetlike<TSet, TElement> { }

/// <summary>
/// Extensions for <see cref="IReadWriteSetlike{TSet, TElement}"/> types.
/// </summary>
public static class IReadonlySetlikeExtensions
{
    /// <summary>
    /// Gets an iterator for the elements of the <paramref name="set"/>.
    /// When <paramref name="disposePreviousValueWhenMovingToNextValue"/> is set to <see langword="true"/>; it will dispose both values when the iterator moves to the next or completes.
    /// </summary>
    /// <remarks>
    /// This iterator deserializes each element twice, so in practice you would rather use <see cref="ValuesAsync{TSet, TElement}(IReadonlySetlike{TSet, TElement}, bool)"/> in most cases.
    /// </remarks>
    /// <typeparam name="TSet">The type of the set.</typeparam>
    /// <typeparam name="TElement">The type of the elements that the set contains.</typeparam>
    /// <param name="set">The set to iterate.</param>
    /// <param name="disposePreviousValueWhenMovingToNextValue">Whether it should dispose the prior value and key when the iterator moves on to the next.</param>
    public static async Task<Iterator<TElement, TElement>> EntriesAsync<TSet, TElement>(this IReadonlySetlike<TSet, TElement> set, bool disposePreviousValueWhenMovingToNextValue = true) where TSet : IReadonlySetlike<TSet, TElement>
    {
        Iterator<TElement, TElement> iterator = await Iterator<TElement, TElement>.CreateAsync(set.JSRuntime, await set.JSReference.InvokeAsync<IJSObjectReference>("entries"), new CreationOptions() { DisposesJSReference = true });
        iterator.DisposePreviousValueWhenMovingToNextValue = disposePreviousValueWhenMovingToNextValue;
        return iterator;
    }

    /// <summary>
    /// Executes the provided <paramref name="function"/> once for each element in the <paramref name="set"/>.
    /// </summary>
    /// <remarks>
    /// It will not wait for each function call to complete.
    /// So if you need all calls to complete before continuing, then you should use <see cref="KeysAsync{TSet, TElement}(IReadonlySetlike{TSet, TElement}, bool)"/> instead.
    /// </remarks>
    /// <typeparam name="TSet">The type of the set.</typeparam>
    /// <typeparam name="TElement">The type of the elements that the set contains.</typeparam>
    /// <param name="set">The set to iterate.</param>
    /// <param name="function">The function that will be invoked for each entry in the set.</param>
    public static async Task ForEachAsync<TSet, TElement>(this IReadonlySetlike<TSet, TElement> set, Func<Task> function) where TSet : IReadonlySetlike<TSet, TElement>
    {
        Callback callback = new(function);
        using var callbackObjRef = DotNetObjectReference.Create(callback);
        IJSObjectReference helper = await set.JSRuntime.GetHelperAsync();
        await helper.InvokeVoidAsync("forEachWithNoArguments", set.JSReference, callbackObjRef);
    }

    /// <summary>
    /// Executes the provided <paramref name="function"/> once for each element in the <paramref name="set"/>.
    /// When <paramref name="disposeValueWhenFunctionHasBeenInvoked"/> is set to <see langword="true"/>; it will dispose each value after the <paramref name="function"/> has been invoked.
    /// </summary>
    /// <remarks>
    /// It will not wait for each function call to complete.
    /// So if you need all calls to complete before continuing, then you should use <see cref="KeysAsync{TSet, TElement}(IReadonlySetlike{TSet, TElement}, bool)"/> instead.
    /// </remarks>
    /// <typeparam name="TSet">The type of the set.</typeparam>
    /// <typeparam name="TElement">The type of the elements that the set contains.</typeparam>
    /// <param name="set">The set to iterate.</param>
    /// <param name="function">The function that will be invoked for each entry in the set.</param>
    /// <param name="disposeValueWhenFunctionHasBeenInvoked">Whether each value that is parsed as a argument for the <paramref name="function"/> should be disposed after the function has completed.</param>
#if NET9_0_OR_GREATER
    [System.Runtime.CompilerServices.OverloadResolutionPriority(1)]
#endif
    public static async Task ForEachAsync<TSet, TElement>(this IReadonlySetlike<TSet, TElement> set, Func<TElement, Task> function, bool disposeValueWhenFunctionHasBeenInvoked = true) where TSet : IReadonlySetlike<TSet, TElement>
    {
        bool valueIsJSCreatable = typeof(TElement).GetInterfaces().Any(i => i.IsConstructedGenericType && i.GetGenericTypeDefinition() == typeof(IJSCreatable<>));

        OneParameterCallback callback = new(async (arg) =>
        {
            TElement? createdObjectForArg = await DeclarationJSMapping.ConstructValueAsync<TElement>(arg, set.JSRuntime, valueIsJSCreatable);

            await function(createdObjectForArg);

            if (disposeValueWhenFunctionHasBeenInvoked && createdObjectForArg is IAsyncDisposable disposableArg)
                await disposableArg.DisposeAsync();
        });
        using var callbackObjRef = DotNetObjectReference.Create(callback);
        IJSObjectReference helper = await set.JSRuntime.GetHelperAsync();
        await helper.InvokeVoidAsync("forEachWithOneArgument", set.JSReference, callbackObjRef, valueIsJSCreatable);
    }

    /// <summary>
    /// Checks whether there exists an element in the <paramref name="set"/> equal to the specific <paramref name="element"/>.
    /// </summary>
    /// <typeparam name="TSet">The type of the set.</typeparam>
    /// <typeparam name="TElement">The type of the elements that the set contains.</typeparam>
    /// <param name="set">The set to make the lookup in.</param>
    /// <param name="element">The element that needs to be in the set for the method to return <see langword="true"/>.</param>
    /// <returns></returns>
    public static async Task<bool> HasAsync<TSet, TElement>(this IReadonlySetlike<TSet, TElement> set, TElement element) where TSet : IReadonlySetlike<TSet, TElement>
    {
        return await set.JSReference.InvokeAsync<bool>("has", element);
    }

    /// <summary>
    /// Gets an iterator for the elements in the <paramref name="set"/>.
    /// When <paramref name="disposePreviousValueWhenMovingToNextValue"/> is set to <see langword="true"/>; it will dispose each element when the iterator moves to the next or completes.
    /// </summary>
    /// <typeparam name="TSet">The type of the set.</typeparam>
    /// <typeparam name="TElement">The type of the elements that the set contains.</typeparam>
    /// <param name="set">The set to iterate.</param>
    /// <param name="disposePreviousValueWhenMovingToNextValue">Whether it should dispose the prior value when the iterator moves on to the next.</param>
    public static async Task<Iterator<TElement>> ValuesAsync<TSet, TElement>(this IReadonlySetlike<TSet, TElement> set, bool disposePreviousValueWhenMovingToNextValue = true) where TSet : IReadonlySetlike<TSet, TElement>
    {
        Iterator<TElement> iterator = await Iterator<TElement>.CreateAsync(set.JSRuntime, await set.JSReference.InvokeAsync<IJSObjectReference>("values"), new() { DisposesJSReference = true });
        iterator.DisposePreviousValueWhenMovingToNextValue = disposePreviousValueWhenMovingToNextValue;
        return iterator;
    }

    /// <inheritdoc cref="ValuesAsync{TSet, TElement}(IReadonlySetlike{TSet, TElement}, bool)"/>
    public static async Task<Iterator<TElement>> KeysAsync<TSet, TElement>(this IReadonlySetlike<TSet, TElement> set, bool disposePreviousValueWhenMovingToNextValue = true) where TSet : IReadonlySetlike<TSet, TElement>
    {
        Iterator<TElement> iterator = await Iterator<TElement>.CreateAsync(set.JSRuntime, await set.JSReference.InvokeAsync<IJSObjectReference>("keys"), new() { DisposesJSReference = true });
        iterator.DisposePreviousValueWhenMovingToNextValue = disposePreviousValueWhenMovingToNextValue;
        return iterator;
    }

    /// <summary>
    /// Gets the number of elements in the <paramref name="set"/>.
    /// </summary>
    /// <typeparam name="TSet">The type of the set.</typeparam>
    /// <typeparam name="TElement">The type of the elements that the set contains.</typeparam>
    /// <param name="set">The set to check the size of.</param>
    public static async Task<ulong> GetSizeAsync<TSet, TElement>(this IReadonlySetlike<TSet, TElement> set) where TSet : IReadonlySetlike<TSet, TElement>
    {
        await using IJSObjectReference helper = await set.JSRuntime.GetHelperAsync();
        return await helper.InvokeAsync<ulong>("getAttribute", set.JSReference, "size");
    }
}
