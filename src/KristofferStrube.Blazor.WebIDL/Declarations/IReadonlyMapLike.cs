using Microsoft.JSInterop;
using System.Text.Json;

namespace KristofferStrube.Blazor.WebIDL;

/// <summary>
/// Objects implementing the <see cref="IReadonlyMapLike{TMap, TKey, TValue}"/> interface are maplike and have an map of key–value pairs, known as that object’s map entries.
/// </summary>
/// <remarks><see href="https://webidl.spec.whatwg.org/#idl-maplike">See the API definition here</see>.</remarks>
/// <typeparam name="TMap">The type of the map.</typeparam>
/// <typeparam name="TKey">The type of the keys of the map.</typeparam>
/// <typeparam name="TValue">The type of the values in the map.</typeparam>
public interface IReadonlyMapLike<TMap, TKey, TValue> : IJSWrapper where TMap : IReadonlyMapLike<TMap, TKey, TValue> { }

/// <summary>
/// Extensions used to access members of objects that implement <see cref="IReadonlyMapLike{TMap, TKey, TValue}"/>.
/// </summary>
public static class IReadonlyMapLikeExtensions
{
    /// <summary>
    /// Gets the number of entries in the <paramref name="map"/>.
    /// </summary>
    /// <typeparam name="TMap">The type of the map.</typeparam>
    /// <typeparam name="TKey">The type of the keys of the map.</typeparam>
    /// <typeparam name="TValue">The type of the values in the map.</typeparam>
    /// <param name="map">The map to check the size of.</param>
    public static async Task<ulong> GetSizeAsync<TMap, TKey, TValue>(this IReadonlyMapLike<TMap, TKey, TValue> map) where TMap : IReadonlyMapLike<TMap, TKey, TValue>
    {
        IJSObjectReference helper = await map.JSRuntime.GetHelperAsync();
        return await helper.InvokeAsync<ulong>("getAttribute", map.JSReference, "size");
    }

    /// <summary>
    /// Gets an iterator for the entries of the <paramref name="map"/>.
    /// When <paramref name="disposePreviousValueWhenMovingToNextValue"/> is set to <see langword="true"/>; it will dispose each key and value when the iterator moves to the next or completes.
    /// </summary>
    /// <typeparam name="TMap">The type of the map.</typeparam>
    /// <typeparam name="TKey">The type of the keys of the map.</typeparam>
    /// <typeparam name="TValue">The type of the values in the map.</typeparam>
    /// <param name="map">The map to iterate.</param>
    /// <param name="disposePreviousValueWhenMovingToNextValue">Whether it should dispose the prior value and key when the iterator moves on to the next.</param>
    public static async Task<Iterator<TKey, TValue>> EntriesAsync<TMap, TKey, TValue>(this IReadonlyMapLike<TMap, TKey, TValue> map, bool disposePreviousValueWhenMovingToNextValue = true) where TMap : IReadonlyMapLike<TMap, TKey, TValue>
    {
        Iterator<TKey, TValue> iterator = await Iterator<TKey, TValue>.CreateAsync(map.JSRuntime, await map.JSReference.InvokeAsync<IJSObjectReference>("entries"), new CreationOptions() { DisposesJSReference = true });
        iterator.DisposePreviousValueWhenMovingToNextValue = disposePreviousValueWhenMovingToNextValue;
        return iterator;
    }

    /// <summary>
    /// Gets an iterator for the values in the <paramref name="map"/>.
    /// When <paramref name="disposePreviousValueWhenMovingToNextValue"/> is set to <see langword="true"/>; it will dispose each value when the iterator moves to the next or completes.
    /// </summary>
    /// <typeparam name="TMap">The type of the map.</typeparam>
    /// <typeparam name="TKey">The type of the keys of the map.</typeparam>
    /// <typeparam name="TValue">The type of the values in the map.</typeparam>
    /// <param name="map">The map to iterate.</param>
    /// <param name="disposePreviousValueWhenMovingToNextValue">Whether it should dispose the prior value when the iterator moves on to the next.</param>
    public static async Task<Iterator<TValue>> ValuesAsync<TMap, TKey, TValue>(this IReadonlyMapLike<TMap, TKey, TValue> map, bool disposePreviousValueWhenMovingToNextValue = true) where TMap : IReadonlyMapLike<TMap, TKey, TValue>
    {
        Iterator<TValue> iterator = await Iterator<TValue>.CreateAsync(map.JSRuntime, await map.JSReference.InvokeAsync<IJSObjectReference>("values"), new() { DisposesJSReference = true });
        iterator.DisposePreviousValueWhenMovingToNextValue = disposePreviousValueWhenMovingToNextValue;
        return iterator;
    }

    /// <summary>
    /// Gets an iterator for the keys of the <paramref name="map"/>.
    /// When <paramref name="disposePreviousKeyWhenMovingToNextValue"/> is set to <see langword="true"/>; it will dispose each key when the iterator moves to the next or completes.
    /// </summary>
    /// <typeparam name="TMap">The type of the map.</typeparam>
    /// <typeparam name="TKey">The type of the keys of the map.</typeparam>
    /// <typeparam name="TValue">The type of the values in the map.</typeparam>
    /// <param name="map">The map to iterate.</param>
    /// <param name="disposePreviousKeyWhenMovingToNextValue">Whether it should dispose the prior key when the iterator moves on to the next.</param>
    public static async Task<Iterator<TKey>> KeysAsync<TMap, TKey, TValue>(this IReadonlyMapLike<TMap, TKey, TValue> map, bool disposePreviousKeyWhenMovingToNextValue = true) where TMap : IReadonlyMapLike<TMap, TKey, TValue>
    {
        Iterator<TKey> iterator = await Iterator<TKey>.CreateAsync(map.JSRuntime, await map.JSReference.InvokeAsync<IJSObjectReference>("keys"), new() { DisposesJSReference = true });
        iterator.DisposePreviousValueWhenMovingToNextValue = disposePreviousKeyWhenMovingToNextValue;
        return iterator;
    }

    /// <summary>
    /// Executes the provided <paramref name="function"/> once for each entry in the <paramref name="map"/>.
    /// </summary>
    /// <remarks>
    /// It will not wait for each function call to complete.
    /// So if you need all calls to complete before continuing, then you should use one of these instead:
    /// <list type="bullet">
    /// <item><see cref="EntriesAsync{TMap, TKey, TValue}(IReadonlyMapLike{TMap, TKey, TValue}, bool)"/></item>
    /// <item><see cref="KeysAsync{TMap, TKey, TValue}(IReadonlyMapLike{TMap, TKey, TValue}, bool)"/></item>
    /// <item><see cref="ValuesAsync{TMap, TKey, TValue}(IReadonlyMapLike{TMap, TKey, TValue}, bool)"/></item>
    /// </list>
    /// </remarks>
    /// <typeparam name="TMap">The type of the map.</typeparam>
    /// <typeparam name="TKey">The type of the keys of the map.</typeparam>
    /// <typeparam name="TValue">The type of the values in the map.</typeparam>
    /// <param name="map">The map to iterate.</param>
    /// <param name="function">The function that will be invoked for each entry in the map.</param>
    public static async Task ForEachAsync<TMap, TKey, TValue>(this IReadonlyMapLike<TMap, TKey, TValue> map, Func<Task> function) where TMap : IReadonlyMapLike<TMap, TKey, TValue>
    {
        Callback callback = new(function);
        using var callbackObjRef = DotNetObjectReference.Create(callback);
        IJSObjectReference helper = await map.JSRuntime.GetHelperAsync();
        await helper.InvokeVoidAsync("forEachWithNoArguments", map.JSReference, callbackObjRef);
    }

    /// <summary>
    /// Executes the provided <paramref name="function"/> once for each entry in the <paramref name="map"/>.<br />
    /// When <paramref name="disposeValueWhenFunctionHasBeenInvoked"/> is set to <see langword="true"/>; it will dispose each value after the <paramref name="function"/> has been invoked.
    /// </summary>
    /// <remarks>
    /// It will not wait for each function call to complete.
    /// So if you need all calls to complete before continuing, then you should use <see cref="ValuesAsync{TMap, TKey, TValue}(IReadonlyMapLike{TMap, TKey, TValue}, bool)"/> instead.
    /// </remarks>
    /// <typeparam name="TMap">The type of the map.</typeparam>
    /// <typeparam name="TKey">The type of the keys of the map.</typeparam>
    /// <typeparam name="TValue">The type of the values in the map.</typeparam>
    /// <param name="map">The map to iterate.</param>
    /// <param name="function">The function that will be invoked for each entry in the map.</param>
    /// <param name="disposeValueWhenFunctionHasBeenInvoked">Whether each value that is parsed as a argument for the <paramref name="function"/> should be disposed after the function has completed.</param>
#if NET9_0_OR_GREATER
    [System.Runtime.CompilerServices.OverloadResolutionPriority(1)]
#endif
    public static async Task ForEachAsync<TMap, TKey, TValue>(this IReadonlyMapLike<TMap, TKey, TValue> map, Func<TValue, Task> function, bool disposeValueWhenFunctionHasBeenInvoked = true) where TMap : IReadonlyMapLike<TMap, TKey, TValue>
    {
        bool valueIsJSCreatable = typeof(TValue).GetInterfaces().Any(i => i.IsConstructedGenericType && i.GetGenericTypeDefinition() == typeof(IJSCreatable<>));
        bool valueIsIJSObjectReference = typeof(TValue) == typeof(IJSObjectReference);

        OneParameterCallback callback = new(async (arg) =>
        {
            TValue? createdObjectForArg = await DeclarationJSMapping.ConstructValueAsync<TValue>(arg, map.JSRuntime, valueIsJSCreatable);

            await function(createdObjectForArg);

            if (disposeValueWhenFunctionHasBeenInvoked && createdObjectForArg is IAsyncDisposable disposableArg)
            {
                await disposableArg.DisposeAsync();
            }
        });
        using var callbackObjRef = DotNetObjectReference.Create(callback);
        IJSObjectReference helper = await map.JSRuntime.GetHelperAsync();
        await helper.InvokeVoidAsync(
            "forEachWithOneArgument",
            map.JSReference,
            callbackObjRef,
            valueIsJSCreatable || valueIsIJSObjectReference
        );
    }
    /// <summary>
    /// Executes the provided <paramref name="function"/> once for each entry in the <paramref name="map"/>.<br />
    /// When <paramref name="disposeKeyAndValueWhenFunctionHasBeenInvoked"/> is set to <see langword="true"/>; it will dispose each value and key after the <paramref name="function"/> has been invoked.
    /// </summary>
    /// <remarks>
    /// It will not wait for each function call to complete.
    /// So if you need all calls to complete before continuing, then you should use <see cref="EntriesAsync{TMap, TKey, TValue}(IReadonlyMapLike{TMap, TKey, TValue}, bool)"/> instead.
    /// </remarks>
    /// <typeparam name="TMap">The type of the map.</typeparam>
    /// <typeparam name="TKey">The type of the keys of the map.</typeparam>
    /// <typeparam name="TValue">The type of the values in the map.</typeparam>
    /// <param name="map">The map to iterate.</param>
    /// <param name="function">The function that will be invoked for each entry in the map.</param>
    /// <param name="disposeKeyAndValueWhenFunctionHasBeenInvoked">Whether the values and keys that are parsed as arguments for the <paramref name="function"/> should be disposed after the function has completed.</param>
#if NET9_0_OR_GREATER
    [System.Runtime.CompilerServices.OverloadResolutionPriority(2)]
#endif
    public static async Task ForEachAsync<TMap, TKey, TValue>(this IReadonlyMapLike<TMap, TKey, TValue> map, Func<TValue, TKey, Task> function, bool disposeKeyAndValueWhenFunctionHasBeenInvoked = true) where TMap : IReadonlyMapLike<TMap, TKey, TValue>
    {
        bool valueIsJSCreatable = typeof(TValue).GetInterfaces().Any(i => i.IsConstructedGenericType && i.GetGenericTypeDefinition() == typeof(IJSCreatable<>));
        bool valueIsIJSObjectReference = typeof(TValue) == typeof(IJSObjectReference);

        bool keyIsJSCreatable = typeof(TKey).GetInterfaces().Any(i => i.IsConstructedGenericType && i.GetGenericTypeDefinition() == typeof(IJSCreatable<>));
        bool keyIsIJSObjectReference = typeof(TKey) == typeof(IJSObjectReference);

        TwoParameterCallback callback = new(async (arg1, arg2) =>
        {
            TValue? createdObjectForArg1 = await DeclarationJSMapping.ConstructValueAsync<TValue>(arg1, map.JSRuntime, valueIsJSCreatable);

            TKey? createdObjectForArg2 = await DeclarationJSMapping.ConstructValueAsync<TKey>(arg2, map.JSRuntime, keyIsJSCreatable);

            await function(createdObjectForArg1, createdObjectForArg2);

            if (disposeKeyAndValueWhenFunctionHasBeenInvoked)
            {
                if (createdObjectForArg1 is IAsyncDisposable disposableArg1)
                    await disposableArg1.DisposeAsync();

                if (createdObjectForArg2 is IAsyncDisposable disposableArg2)
                    await disposableArg2.DisposeAsync();
            }
        });
        using var callbackObjRef = DotNetObjectReference.Create(callback);
        IJSObjectReference helper = await map.JSRuntime.GetHelperAsync();
        await helper.InvokeVoidAsync(
            "forEachWithTwoArguments",
            map.JSReference,
            callbackObjRef,
            valueIsJSCreatable || valueIsIJSObjectReference,
            keyIsJSCreatable || keyIsIJSObjectReference
        );
    }

    /// <summary>
    /// Gets the value in the <paramref name="map"/> specified by the <paramref name="key"/>.
    /// </summary>
    /// <typeparam name="TMap">The type of the map.</typeparam>
    /// <typeparam name="TKey">The type of the keys of the map.</typeparam>
    /// <typeparam name="TValue">The type of the values in the map.</typeparam>
    /// <param name="map">The map to make the lookup in.</param>
    /// <param name="key">The key used to lookup in the map.</param>
    public static async Task<TValue?> GetAsync<TMap, TKey, TValue>(this IReadonlyMapLike<TMap, TKey, TValue> map, TKey key) where TMap : IReadonlyMapLike<TMap, TKey, TValue>
    {
        bool valueIsJSCreatable = typeof(TValue).GetInterfaces().Any(i => i.IsConstructedGenericType && i.GetGenericTypeDefinition() == typeof(IJSCreatable<>));
        bool valueIsIJSObjectReference = typeof(TValue) == typeof(IJSObjectReference);

        object? value = (valueIsIJSObjectReference || valueIsJSCreatable)
            ? await map.JSReference.InvokeAsync<IJSObjectReference?>("get", key)
            : await map.JSReference.InvokeAsync<JsonElement>("get", key);

        if (value is null or JsonElement { ValueKind: JsonValueKind.Null or JsonValueKind.Undefined })
            return default(TValue?);

        TValue createdObject = await DeclarationJSMapping.ConstructValueAsync<TValue>(value, map.JSRuntime, valueIsJSCreatable);

        return createdObject;
    }

    /// <summary>
    /// Checks whether there exists an entry in the <paramref name="map"/> with the specific <paramref name="key"/>.
    /// </summary>
    /// <typeparam name="TMap">The type of the map.</typeparam>
    /// <typeparam name="TKey">The type of the keys of the map.</typeparam>
    /// <typeparam name="TValue">The type of the values in the map.</typeparam>
    /// <param name="map">The map to make the lookup in.</param>
    /// <param name="key">The key of the entry that should be checked.</param>
    /// <returns></returns>
    public static async Task<bool> HasAsync<TMap, TKey, TValue>(this IReadonlyMapLike<TMap, TKey, TValue> map, TKey key) where TMap : IReadonlyMapLike<TMap, TKey, TValue>
    {
        return await map.JSReference.InvokeAsync<bool>("has", key);
    }
}