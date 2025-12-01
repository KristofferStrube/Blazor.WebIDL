using Microsoft.JSInterop;
using System.Reflection;
using System.Text.Json;

namespace KristofferStrube.Blazor.WebIDL;
internal static class DeclarationJSMapping
{
    internal static async Task<T> ConstructValueAsync<T>(object? value, IJSRuntime jSRuntime, bool isJSCreatable)
    {
        if (value is IJSObjectReference valueAsJSObjectReference)
        {
            if (isJSCreatable)
            {
                return await CallCreateAsync<T>(valueAsJSObjectReference, jSRuntime);
            }
            else
            {
                return (T)valueAsJSObjectReference;
            }
        }
        else if (value is JsonElement valueAsJsonElement)
        {
            return valueAsJsonElement.Deserialize<T>()!;
        }
        else
        {
            throw new JSInteropHydrationException($"The value that was being hydrated was of an unexpected type. Expected a {nameof(IJSObjectReference)} or a {nameof(JsonElement)} but got a {value?.GetType().Name}");
        }
    }

    internal static async Task<T> CallCreateAsync<T>(IJSObjectReference value, IJSRuntime jSRuntime)
    {
        MethodInfo createAsyncMethod = typeof(T).GetMethod("CreateAsync", new[] { typeof(IJSRuntime), typeof(IJSObjectReference), typeof(CreationOptions) })!;
        return await (Task<T>)createAsyncMethod.Invoke(null, new object[] { jSRuntime, value, new CreationOptions() { DisposesJSReference = true } })!;
    }

    public class JSInteropHydrationException : Exception
    {
        public JSInteropHydrationException(string? message) : base(message)
        {

        }
    }
}
