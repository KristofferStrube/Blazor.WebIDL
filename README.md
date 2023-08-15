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

This can be used to catch strongly typed JS errors from Blazor. An example could be trying to access the clipboard which can fail in many ways.
```csharp
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
public class Set : IReadWriteSetlike<Set>
{
    public IJSObjectReference JSReference { get; }
    public IJSRuntime JSRuntime { get; }

    public static async Task<Set> CreateAsync<T>(IJSRuntime jSRuntime, IEnumerable<T>? iterable = null)
    {
        var jSInstance = await jSRuntime.InvokeAsync<IJSObjectReference>("constructSet", iterable);
        return new Set(jSRuntime, jSInstance);
    }

    public Set(IJSRuntime jSRuntime, IJSObjectReference jSReference)
    {
        JSRuntime = jSRuntime;
        JSReference = jSReference;
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

And it is planned to be used in these projects:
- https://github.com/KristofferStrube/Blazor.FileAPI
- https://github.com/KristofferStrube/Blazor.FileSystem
- https://github.com/KristofferStrube/Blazor.FileSystemAccess
- https://github.com/KristofferStrube/Blazor.WebAudio

# Related articles
How this project was developed is explored in this article from my blog:
- [Typed exceptions for JSInterop in Blazor](https://kristoffer-strube.dk/post/typed-exceptions-for-jsinterop-in-blazor/)

This repository was build with inspiration and help from the following series of articles:
- [Wrapping JavaScript libraries in Blazor WebAssembly/WASM](https://blog.elmah.io/wrapping-javascript-libraries-in-blazor-webassembly-wasm/)
- [Call anonymous C# functions from JS in Blazor WASM](https://blog.elmah.io/call-anonymous-c-functions-from-js-in-blazor-wasm/)
- [Using JS Object References in Blazor WASM to wrap JS libraries](https://blog.elmah.io/using-js-object-references-in-blazor-wasm-to-wrap-js-libraries/)
- [Blazor WASM 404 error and fix for GitHub Pages](https://blog.elmah.io/blazor-wasm-404-error-and-fix-for-github-pages/)
- [How to fix Blazor WASM base path problems](https://blog.elmah.io/how-to-fix-blazor-wasm-base-path-problems/)