[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](/LICENSE.md)
[![GitHub issues](https://img.shields.io/github/issues/KristofferStrube/Blazor.WebIDL)](https://github.com/KristofferStrube/Blazor.WebIDL/issues)
[![GitHub forks](https://img.shields.io/github/forks/KristofferStrube/Blazor.WebIDL)](https://github.com/KristofferStrube/Blazor.WebIDL/network/members)
[![GitHub stars](https://img.shields.io/github/stars/KristofferStrube/Blazor.WebIDL)](https://github.com/KristofferStrube/Blazor.WebIDL/stargazers)
[![NuGet Downloads (official NuGet)](https://img.shields.io/nuget/dt/KristofferStrube.Blazor.WebIDL?label=NuGet%20Downloads)](https://www.nuget.org/packages/KristofferStrube.Blazor.WebIDL/)

# Blazor.WebIDL
A Blazor wrapper for types and interfaces used in and defined in [the standard WebIDL specification](https://webidl.spec.whatwg.org/).
Among these are declarations that define specific behavior for interfaces and the standard exception definition types used across all WebIDL based APIs.

# Demo
The sample project can be demoed at https://kristofferstrube.github.io/Blazor.WebIDL/

On each page you can find the corresponding code for the example in the top right corner.

# Exceptions
The specification defines the types and names for all the standard exceptions and the standard names for [DomExceptions](https://webidl.spec.whatwg.org/#idl-DOMException-error-names).

It also provides a way to make JSInterop calls that automatically throw these typed errors when a standard exception is thrown in JS.

## Setting it up
In `Programs.cs`, we can inject a service to make Error Handling JSInterop easy in each of our pages/components. In Blazor WASM, we additionally need to call a function in `Program.cs` before being able to use Error Handling JSInterop. This is only needed for WASM, as JSInterop in WASM can return `IJSObjectReferences` synchronously.

```
var builder = WebAssemblyHostBuilder.CreateDefault(args);

// Setting up other services before this.
builder.Services.AddErrorHandlingJSRuntime();

var app = builder.Build();

// For Blazor WASM you need to call this to make Error Handling JSInterop.
await app.Services.SetupErrorHandlingJSInterop();

await app.RunAsync();
```

## Error Handling JSInterop calls in a page.

This can be used to catch strongly typed JS errors from Blazor. An example could be trying to access the clipboard which can fail in many ways.
```razor
@using KristofferStrube.Blazor.WebIDL.Exceptions;
@inject IErrorHandlingJSRuntime ErrorHandlingJSRuntime
@inject ILogger Logger

@code {
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (!firstRender) return;
        try
        {
            var result = await ErrorHandlingJSRuntime.InvokeAsync<string>("navigator.clipboard.readText");
            Console.WriteLine(result);
        }
        catch (NotAllowedErrorException exception)
        {
            Logger.LogWarning(exception, "The user has not given permission to read the clipboard.");
        }
        catch (DOMException exception)
        {
            Logger.LogError(exception, $"{exception.Name} (which is a DOMException): \"{exception.Message}\"");
        }
        catch (WebIDLException exception)
        {
            Logger.LogError(exception, $"{exception.GetType().Name}: \"{exception.Message}\"");
        }
    }
}
```

# Standard behavior and method definitions
The standard WebIDL specification make definitions that are used across all API specifications that use WebIDL for defining their interfaces.
Declarations like `Iterable`, `Asynchronously iterable`, `Maplike`, `Setlike` define expected behavior and methods that should apply to the interfaces that use these definitions. This wrapper defines C# interfaces for these declarations that have implementations for the expected methods. This makes it easy and safe to implement wrappers for interfaces that define i.e. Setlike behavior.

We have currently implemented 2 interfaces for the `Setlike` declaration called `IReadonlySetlike` and `IReadWriteSetlike` where `IReadWriteSetlike` inherits from `IReadonlySetlike`

They can be used like this to make a very simple wrapper for the JS `Set` type:
```csharp
[IJSWrapperConverter]
public class Set : IReadWriteSetlike<Set>
{
    /// <inheritdoc/>
    public IJSObjectReference JSReference { get; }
    /// <inheritdoc/>
    public IJSRuntime JSRuntime { get; }
    /// <inheritdoc/>
    public bool DisposesJSReference { get; }

    public static async Task<Set> CreateAsync<T>(IJSRuntime jSRuntime, IEnumerable<T>? iterable = null)
    {
        var jSInstance = await jSRuntime.InvokeAsync<IJSObjectReference>("constructSet", iterable);
        return new Set(jSRuntime, jSInstance, new() { DisposesJSReference = true });
    }

    public Set(IJSRuntime jSRuntime, IJSObjectReference jSReference, CreationOptions options)
    {
        JSRuntime = jSRuntime;
        JSReference = jSReference;
        DisposesJSReference = options.DisposesJSReference;
    }

    /// <inheritdoc/>
    public async ValueTask DisposeAsync()
    {
        await IJSWrapper.DisposeJSReference(this);
        GC.SuppressFinalize(this);
    }
}
```

# Issues
Feel free to open issues on the repository if you find any errors with the package or have wishes for features.

# Related repositories
This project is used in the following projects:
- https://github.com/KristofferStrube/Blazor.Streams
- https://github.com/KristofferStrube/Blazor.DOM
- https://github.com/KristofferStrube/Blazor.MediaCaptureStreams
- https://github.com/KristofferStrube/Blazor.WebAudio

And it is planned to be used in these projects:
- https://github.com/KristofferStrube/Blazor.FileAPI
- https://github.com/KristofferStrube/Blazor.FileSystem
- https://github.com/KristofferStrube/Blazor.FileSystemAccess

# Related articles
How this project was developed is explored in this article from my blog:
- [Typed exceptions for JSInterop in Blazor](https://kristoffer-strube.dk/post/typed-exceptions-for-jsinterop-in-blazor/)

This repository was build with inspiration and help from the following series of articles:
- [Wrapping JavaScript libraries in Blazor WebAssembly/WASM](https://blog.elmah.io/wrapping-javascript-libraries-in-blazor-webassembly-wasm/)
- [Call anonymous C# functions from JS in Blazor WASM](https://blog.elmah.io/call-anonymous-c-functions-from-js-in-blazor-wasm/)
- [Using JS Object References in Blazor WASM to wrap JS libraries](https://blog.elmah.io/using-js-object-references-in-blazor-wasm-to-wrap-js-libraries/)
- [Blazor WASM 404 error and fix for GitHub Pages](https://blog.elmah.io/blazor-wasm-404-error-and-fix-for-github-pages/)
- [How to fix Blazor WASM base path problems](https://blog.elmah.io/how-to-fix-blazor-wasm-base-path-problems/)
