using FluentAssertions.Execution;
using KristofferStrube.Blazor.WebIDL.Declarations;
using Microsoft.JSInterop;
using Microsoft.JSInterop.Implementation;
using System.Reflection;
using System.Text.Json.Serialization;

namespace IntegrationTests.Declarations;

/// <remarks>
/// This is not tested for Webkit because it does not seem to be supported in the webkit that PlayWright ships yet.
/// </remarks>
[TestFixture("Chrome")]
[TestFixture("Firefox")]
public class PairAsyncIterableOverSimpleAndWrapperPairTest(string browserName) : BlazorTest(browserName)
{
    private FileSystemDirectoryHandle opfs = default!;
    private FileSystemDirectoryHandle directoryHandle = default!;
    private FileSystemFileHandle fileHandle = default!;

    [SetUp]
    public async Task CreateFileSystemHandles()
    {
        IJSObjectReference opfsJsReference = await JSRuntime.InvokeAsync<IJSObjectReference>("navigator.storage.getDirectory");

        opfs = await FileSystemDirectoryHandle.CreateAsync(JSRuntime, opfsJsReference, new() { DisposesJSReference = true });

        directoryHandle = await opfs.GetDirectoryHandleAsync("my-directory", new() { Create = true });

        fileHandle = await opfs.GetFileHandleAsync("my-file", new() { Create = true });
    }

    [Test]
    public async Task ValuesAsync_ShouldReturnAsyncIteratorThatCanReturnValues()
    {
        // Act
        await using AsyncIterator<FileSystemHandle> iterator = await opfs.ValuesAsync();

        // Assert
        using AssertionScope assertionScope = new();

        _ = await iterator.MoveNextAsync();
        FileSystemHandle firstHandle = iterator.Current;
        bool firstIsSameAsFileHandle = await firstHandle.IsSameEntryAsync(fileHandle);

        _ = await iterator.MoveNextAsync();
        FileSystemHandle secondHandle = iterator.Current;
        bool secondIsSameAsDirectoryHandle = await secondHandle.IsSameEntryAsync(directoryHandle);

        _ = firstIsSameAsFileHandle.Should().BeTrue();
        _ = secondIsSameAsDirectoryHandle.Should().BeTrue();
    }

    [Test]
    public async Task ValuesAsync_ShouldReturnIteratorThatDisposesElements_WhenConfiguredToDisposeThem()
    {
        // Act
        await using AsyncIterator<FileSystemHandle> iterator = await opfs.ValuesAsync(
            disposePreviousValueWhenMovingToNextValue: true);

        // Assert
        List<FileSystemHandle> elements = await iterator.ToListAsync();

        _ = elements.Should().AllSatisfy(element =>
        {
            _ = IsDisposed(element.JSReference).Should().BeTrue();
        });
    }

    [Test]
    public async Task ValuesAsync_ShouldReturnIteratorThatDoesNotDisposeElements_WhenConfiguredToNotDisposeThem()
    {
        // Act
        await using AsyncIterator<FileSystemHandle> iterator = await opfs.ValuesAsync(
            disposePreviousValueWhenMovingToNextValue: false);

        // Assert
        List<FileSystemHandle> elements = await iterator.ToListAsync();

        _ = elements.Should().AllSatisfy(element =>
        {
            _ = IsDisposed(element.JSReference).Should().BeFalse();
        });
    }

    [Test]
    public async Task ValuesAsync_ShouldReturnIteratorThatCanIterateEachElementAsItsConcreteType()
    {
        // Act
        await using AsyncIterator<FileSystemHandle> iterator = await opfs.ValuesAsync(
            disposePreviousValueWhenMovingToNextValue: false);

        // Assert
        List<FileSystemHandle> elements = await iterator.ToListAsync();

        _ = elements.Should().ContainItemsAssignableTo<FileSystemFileHandle>()
            .And.ContainItemsAssignableTo<FileSystemDirectoryHandle>();
    }

    [Test]
    public async Task KeysAsync_ShouldReturnAsyncIteratorThatCanReturnValues()
    {
        // Act
        await using AsyncIterator<string> iterator = await opfs.KeysAsync();

        // Assert
        using AssertionScope assertionScope = new();

        _ = await iterator.MoveNextAsync();
        string firstName = iterator.Current;

        _ = await iterator.MoveNextAsync();
        string secondName = iterator.Current;

        using AssertionScope scope = new();

        _ = firstName.Should().Be("my-file");
        _ = secondName.Should().Be("my-directory");
    }

    [Test]
    public async Task EntriesAsync_ShouldReturnAsyncIteratorThatCanReturnValues()
    {
        // Act
        await using AsyncIterator<string, FileSystemHandle> iterator = await opfs.EntriesAsync(
            disposePreviousValueWhenMovingToNextValue: false);

        // Assert
        using AssertionScope assertionScope = new();

        _ = await iterator.MoveNextAsync();
        (string firstName, FileSystemHandle firstHandle) = iterator.Current;

        _ = await iterator.MoveNextAsync();
        (string secondName, FileSystemHandle secondHandle) = iterator.Current;

        using AssertionScope scope = new();

        _ = firstName.Should().Be("my-file");
        _ = secondName.Should().Be("my-directory");

        _ = (await firstHandle.IsSameEntryAsync(fileHandle)).Should().BeTrue();
        _ = (await secondHandle.IsSameEntryAsync(directoryHandle)).Should().BeTrue();
    }

    [Test]
    public async Task EntriesAsync_ShouldReturnIteratorThatDisposesElements_WhenConfiguredToDisposeThem()
    {
        // Act
        await using AsyncIterator<string, FileSystemHandle> iterator = await opfs.EntriesAsync(
            disposePreviousValueWhenMovingToNextValue: true);

        // Assert
        Dictionary<string, FileSystemHandle> elements = await iterator.ToDictionaryAsync();

        _ = elements.Should().AllSatisfy(element =>
        {
            _ = IsDisposed(element.Value.JSReference).Should().BeTrue();
        });
    }

    [Test]
    public async Task EntriesAsync_ShouldReturnIteratorThatDoesNotDisposeElements_WhenConfiguredToNotDisposeThem()
    {
        // Act
        await using AsyncIterator<string, FileSystemHandle> iterator = await opfs.EntriesAsync(
            disposePreviousValueWhenMovingToNextValue: false);

        // Assert
        Dictionary<string, FileSystemHandle> elements = await iterator.ToDictionaryAsync();

        _ = elements.Should().AllSatisfy(element =>
        {
            _ = IsDisposed(element.Value.JSReference).Should().BeFalse();
        });
    }

    [Test]
    public async Task EntriesAsync_ShouldReturnIteratorThatCanIterateEachElementAsItsConcreteType()
    {
        // Act
        await using AsyncIterator<string, FileSystemHandle> iterator = await opfs.EntriesAsync(
            disposePreviousValueWhenMovingToNextValue: false);

        // Assert
        Dictionary<string, FileSystemHandle> elements = await iterator.ToDictionaryAsync();

        _ = elements.Values.Should().ContainItemsAssignableTo<FileSystemFileHandle>()
            .And.ContainItemsAssignableTo<FileSystemDirectoryHandle>();
    }

    private static bool IsDisposed(IJSObjectReference reference)
    {
        PropertyInfo disposedProperty = typeof(JSObjectReference).GetProperty("Disposed", BindingFlags.Instance | BindingFlags.NonPublic)!;
        bool value = (bool)disposedProperty.GetValue(reference, null)!;
        return value;
    }

    /// <remarks><see href="https://fs.spec.whatwg.org/#api-filesystemdirectoryhandle">See the API definition here</see>.</remarks>
    [IJSWrapperConverter]
    public class FileSystemDirectoryHandle : FileSystemHandle, IJSCreatable<FileSystemDirectoryHandle>, IPairAsyncIterable<FileSystemDirectoryHandle, string, FileSystemHandle>
    {
        /// <inheritdoc/>
        public static new async Task<FileSystemDirectoryHandle> CreateAsync(IJSRuntime jSRuntime, IJSObjectReference jSReference)
        {
            return await CreateAsync(jSRuntime, jSReference, new());
        }

        /// <inheritdoc/>
        public static new Task<FileSystemDirectoryHandle> CreateAsync(IJSRuntime jSRuntime, IJSObjectReference jSReference, CreationOptions options)
        {
            return Task.FromResult(new FileSystemDirectoryHandle(jSRuntime, jSReference, options));
        }

        /// <inheritdoc cref="CreateAsync(IJSRuntime, IJSObjectReference, CreationOptions)"/>
        protected FileSystemDirectoryHandle(IJSRuntime jSRuntime, IJSObjectReference jSReference, CreationOptions options) : base(jSRuntime, jSReference, options) { }

        /// <summary>
        /// Returns a file handle or creates if configured to do so.
        /// </param>
        public async Task<FileSystemFileHandle> GetFileHandleAsync(string name, FileSystemGetFileOptions? options = null)
        {
            IJSObjectReference jSFileSystemFileHandle = await JSReference.InvokeAsync<IJSObjectReference>("getFileHandle", name, options);
            return await FileSystemFileHandle.CreateAsync(JSRuntime, jSFileSystemFileHandle, new() { DisposesJSReference = true });
        }
        /// <summary>
        /// Returns a directory handle or creates if configured to do so.
        /// </param>
        public async Task<FileSystemDirectoryHandle> GetDirectoryHandleAsync(string name, FileSystemGetDirectoryOptions? options = null)
        {
            IJSObjectReference jSFileSystemDirectoryHandle = await JSReference.InvokeAsync<IJSObjectReference>("getDirectoryHandle", name, options);
            return new(JSRuntime, jSFileSystemDirectoryHandle, new() { DisposesJSReference = true });
        }
    }

    /// <remarks><see href="https://fs.spec.whatwg.org/#api-filesystemfilehandle">See the API definition here</see>.</remarks>
    [IJSWrapperConverter]
    public class FileSystemFileHandle : FileSystemHandle, IJSCreatable<FileSystemFileHandle>
    {
        /// <inheritdoc/>
        public static new async Task<FileSystemFileHandle> CreateAsync(IJSRuntime jSRuntime, IJSObjectReference jSReference)
        {
            return await CreateAsync(jSRuntime, jSReference, new());
        }

        /// <inheritdoc/>
        public static new Task<FileSystemFileHandle> CreateAsync(IJSRuntime jSRuntime, IJSObjectReference jSReference, CreationOptions options)
        {
            return Task.FromResult(new FileSystemFileHandle(jSRuntime, jSReference, options));
        }

        /// <inheritdoc cref="CreateAsync(IJSRuntime, IJSObjectReference, CreationOptions)"/>
        protected FileSystemFileHandle(IJSRuntime jSRuntime, IJSObjectReference jSReference, CreationOptions options) : base(jSRuntime, jSReference, options) { }
    }

    [IJSWrapperConverter]
    public class FileSystemHandle : IJSCreatable<FileSystemHandle>
    {
        /// <inheritdoc/>
        public IJSRuntime JSRuntime { get; }
        /// <inheritdoc/>
        public IJSObjectReference JSReference { get; }
        /// <inheritdoc/>
        public bool DisposesJSReference { get; }

        /// <inheritdoc/>
        public static async Task<FileSystemHandle> CreateAsync(IJSRuntime jSRuntime, IJSObjectReference jSReference)
        {
            return await CreateAsync(jSRuntime, jSReference, new());
        }

        /// <inheritdoc/>
        public static async Task<FileSystemHandle> CreateAsync(IJSRuntime jSRuntime, IJSObjectReference jSReference, CreationOptions options)
        {
            await using ValueReference handle = new(jSRuntime, jSReference, null, new() { DisposesJSReference = false });

            handle.ValueMapper = new()
            {
                ["filesystemdirectoryhandle"] = async () => await FileSystemDirectoryHandle.CreateAsync(jSRuntime, jSReference, new() { DisposesJSReference = true }),
                ["filesystemfilehandle"] = async () => await FileSystemFileHandle.CreateAsync(jSRuntime, jSReference, new() { DisposesJSReference = true }),
            };

            return (FileSystemHandle)(await handle.GetValueAsync())!;
        }

        /// <inheritdoc cref="CreateAsync(IJSRuntime, IJSObjectReference, CreationOptions)"/>
        protected FileSystemHandle(IJSRuntime jSRuntime, IJSObjectReference jSReference, CreationOptions options)
        {
            JSRuntime = jSRuntime;
            JSReference = jSReference;
            DisposesJSReference = options.DisposesJSReference;
        }

        /// <summary>
        /// Checks if <see langword="this"/> handle and the <paramref name="other"/> handle are the same.
        /// </summary>
        /// <param name="other">Some other handle</param>
        /// <returns><see langword="true"/> if <see langword="this"/> handle is the same as the <paramref name="other"/> handle; else <see langword="false"/></returns>
        public async Task<bool> IsSameEntryAsync(FileSystemHandle other)
        {
            return await JSReference.InvokeAsync<bool>("isSameEntry", other);
        }

        /// <inheritdoc/>
        public async ValueTask DisposeAsync()
        {
            await IJSWrapper.DisposeJSReference(this);
            GC.SuppressFinalize(this);
        }
    }

    /// <remarks><see href="https://fs.spec.whatwg.org/#dictdef-filesystemgetfileoptions">See the API definition here</see>.</remarks>
    public class FileSystemGetFileOptions
    {
        /// <summary>
        /// If the file does not already exist and this is set to <see langword="true"/> it will create the file; Else it will fail.
        /// </summary>
        [JsonPropertyName("create")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public bool Create { get; set; }
    }

    /// <remarks><see href="https://fs.spec.whatwg.org/#dictdef-filesystemgetdirectoryoptions">See the API definition here</see>.</remarks>
    public class FileSystemGetDirectoryOptions
    {
        /// <summary>
        /// If the directory does not already exist and this is set to <see langword="true"/> it will create the directory; Else it will fail.
        /// </summary>
        [JsonPropertyName("create")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public bool Create { get; set; }
    }
}
