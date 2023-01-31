# Blazor.WebIDL
A Blazor wrapper for types and interfaces used in and defined in [the standard WebIDL specification](https://webidl.spec.whatwg.org/).
Among these are declarations that define specific behaviour for interfaces and the standard exception definition types used across all WebIDL based APIs.

**This wrapper is still being developed so ideas are still being tested and experimented with.**

## Standard behaviour and method definitions
The standard WebIDL specification make definitions that are used across all API specifications that use WebIDL for defining their interfaces.
Declarations like `Iterable`, `Asynchronously iterable`, `Maplike`, `Setlike` define expected behaviour and methods that should apply to the interfaces that use these definitions. This wrapper defines C# interfaces for these declarations that have implementations for the expected methods. This makes it easy and safe to implement wrappers for interfaces that define i.e. Setlike behaviour.
```csharp
// Defined in this project
public interface IReadonlySetlike<T> : IJSWrapper, IAsyncEnumerable<T> where T : IJSWrapper
{
    public async Task<IDictionary<string, T>> EntriesAsync() { ... }
    public async Task<bool> HasAsync(T element) { ... }
    public async Task<ReadOnlyCollection<T>> ValuesAsync() { ... }
    public async Task<IAsyncEnumerator<T>> IteratorAsync() { ... }
    public async Task<ulong> GetSizeAsync() { ... }
}

public interface IReadWriteSetlike<T> : IReadonlySetlike<T> where T : IJSWrapper
{
    public async Task<T> AddAsync(T element) { ... }
    public async Task<bool> ClearAsync() { ... }
    public async Task<bool> DeleteAsync(T element) { ... }
}
```

and then definitions like the [FontFaceSet](https://drafts.csswg.org/css-font-loading/#fontfaceset) from the [CSS Font Loading API](https://drafts.csswg.org/css-font-loading/) that has the following WebIDL definition:

```WebIDL
[Exposed=(Window,Worker)]
interface FontFaceSet : EventTarget {
  constructor(sequence<FontFace> initialFaces);

  setlike<FontFace>;
  
  // other attributes and methods in place of this.
};
```

would just need to be implemented like this to have the benefits of being setlike (this is simplified):
```csharp
public class FontFaceSet : EventTarget, IReadWriteSetlike<FontFace> { ... }
```

which means that it could be used like this:
```csharp
var fontFaceSet = await FontFacet.CreateAsync(jSRuntime, jSInstance); // Here we expect som existing jSInstance.

var size = await fontFaceSet.GetSizeAsync();
await foreach (FontFace fontFace in fontFaceSet) {
  // do something with each font face.
}
await fontFaceSet.DeleteAsync(someExistingFontFace);
```

## Exceptions
The specification also defines the types and names for all the standard exceptions and the standard names for [DomExceptions](https://webidl.spec.whatwg.org/#idl-DOMException-error-names).
