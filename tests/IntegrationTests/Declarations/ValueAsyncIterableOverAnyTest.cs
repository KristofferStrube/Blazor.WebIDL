using Microsoft.JSInterop;
using Microsoft.JSInterop.Implementation;
using System.Reflection;
using System.Text.Json.Serialization;

namespace IntegrationTests.Declarations;

/// <remarks>
/// This is not tested for Webkit (Safari), because it does not support it.
/// </remarks>
[TestFixture("Chrome")]
[TestFixture("Firefox")]
public class ValueAsyncIterableOverAnyTest(string browserName) : BlazorTest(browserName)
{
    private ReadableStream readableStream = default!;

    [SetUp]
    public async Task CreateReadableStream()
    {
        byte i = 1;
        readableStream = await ReadableStream.CreateAsync(JSRuntime, new UnderlyingSource(JSRuntime)
        {
            Pull = async (controller) =>
            {
                double? size = await controller.GetDesiredSizeAsync();
                if (size > 0)
                {
                    if (i % 2 == 1)
                    {
                        await controller.EnqueueAsync($"hey {i++}!");
                    }
                    else
                    {
                        await controller.EnqueueAsync(i++);
                    }
                }
            },
        });
    }

    [Test]
    public async Task ValuesAsync_ShouldReturnAsyncIteratorThatCanReturnValues()
    {
        // Act
        await using AsyncIterator<ValueReference> iterator = await readableStream.ValuesAsync();

        // Assert
        _ = await iterator.MoveNextAsync();
        ValueReference firstChunk = iterator.Current;
        string? firstChunkTypeName = await firstChunk.GetTypeNameAsync();
        string? firstChunkValue = await firstChunk.GetValueAsync<string>();

        _ = await iterator.MoveNextAsync();

        ValueReference secondChunk = iterator.Current;
        string? secondChunkTypeName = await secondChunk.GetTypeNameAsync();
        float? secondChunkValue = (float?)await secondChunk.GetValueAsync();

        _ = firstChunkTypeName.Should().Be("string");
        _ = firstChunkValue.Should().Be("hey 1!");
        _ = secondChunkTypeName.Should().Be("number");
        _ = secondChunkValue.Should().Be(2);
    }

    [Test]
    public async Task ValuesAsync_WithPreventCancelOptionForStream_ShouldReturnAsyncIteratorThatDoesNotPropagateCancel()
    {
        // Act
        AsyncIterator<ValueReference> iterator = await readableStream.ValuesAsync(new ()
        {
            PreventCancel = true
        });

        // Assert
        _ = await iterator.MoveNextAsync();
        ValueReference firstChunk = iterator.Current;
        string? firstChunkTypeName = await firstChunk.GetTypeNameAsync();
        string? firstChunkValue = await firstChunk.GetValueAsync<string>();

        await iterator.ReturnAsync(); // Even though we have cancelled here, the new iterator can still continue.
        await iterator.DisposeAsync();
        iterator = await readableStream.ValuesAsync();

        _ = await iterator.MoveNextAsync();

        ValueReference secondChunk = iterator.Current;
        string? secondChunkTypeName = await secondChunk.GetTypeNameAsync();
        float? secondChunkValue = (float?)await secondChunk.GetValueAsync();

        _ = firstChunkTypeName.Should().Be("string");
        _ = firstChunkValue.Should().Be("hey 1!");
        _ = secondChunkTypeName.Should().Be("number");
        _ = secondChunkValue.Should().Be(2);

        await iterator.DisposeAsync();
    }

    [Test]
    public async Task ValuesAsync_ShouldReturnIteratorThatDisposesElements_WhenConfiguredToDisposeThem()
    {
        // Act
        await using AsyncIterator<ValueReference> iterator = await readableStream.ValuesAsync(
            disposePreviousValueWhenMovingToNextValue: true);

        // Assert
        List<ValueReference> elements = await iterator.Take(5).ToListAsync();

        _ = elements.Should().AllSatisfy(element =>
        {
            _ = IsDisposed(element.JSReference).Should().BeTrue();
        });
    }

    [Test]
    public async Task ValuesAsync_ShouldReturnIteratorThatDoesNotDisposeElements_WhenConfiguredToNotDisposeThem()
    {
        // Act
        await using AsyncIterator<ValueReference> iterator = await readableStream.ValuesAsync(
            disposePreviousValueWhenMovingToNextValue: false);

        // Assert
        List<ValueReference> elements = await iterator.Take(5).ToListAsync();

        _ = elements.Should().AllSatisfy(element =>
        {
            _ = IsDisposed(element.JSReference).Should().BeFalse();
        });
    }

    private static bool IsDisposed(IJSObjectReference reference)
    {
        PropertyInfo disposedProperty = typeof(JSObjectReference).GetProperty("Disposed", BindingFlags.Instance | BindingFlags.NonPublic)!;
        bool value = (bool)disposedProperty.GetValue(reference, null)!;
        return value;
    }

    [IJSWrapperConverter]
    public class ReadableStream : IJSCreatable<ReadableStream>, IValueAsyncIterable<ReadableStream, ValueReference, ReadableStreamIteratorOptions>
    {
        /// <inheritdoc/>
        public IJSObjectReference JSReference { get; }

        /// <inheritdoc/>
        public IJSRuntime JSRuntime { get; }

        /// <inheritdoc/>
        public bool DisposesJSReference { get; }

        public static async Task<ReadableStream> CreateAsync(IJSRuntime jSRuntime, UnderlyingSource underlyingSource)
        {
            IJSObjectReference jSInstance = await jSRuntime.InvokeAsync<IJSObjectReference>("constructReadableStream", underlyingSource);
            return new ReadableStream(jSRuntime, jSInstance, new() { DisposesJSReference = true });
        }

        /// <inheritdoc/>
        public static async Task<ReadableStream> CreateAsync(IJSRuntime jSRuntime, IJSObjectReference jSReference)
        {
            return await CreateAsync(jSRuntime, jSReference, new());
        }

        /// <inheritdoc/>
        public static Task<ReadableStream> CreateAsync(IJSRuntime jSRuntime, IJSObjectReference jSReference, CreationOptions options)
        {
            return Task.FromResult(new ReadableStream(jSRuntime, jSReference, options));
        }

        protected ReadableStream(IJSRuntime jSRuntime, IJSObjectReference jSReference, CreationOptions options)
        {
            JSRuntime = jSRuntime;
            JSReference = jSReference;
            DisposesJSReference = options.DisposesJSReference;
        }

        public async ValueTask DisposeAsync()
        {
            await IJSWrapper.DisposeJSReference(this);
            GC.SuppressFinalize(this);
        }
    }

    public class ReadableStreamIteratorOptions
    {
        [JsonPropertyName("preventCancel")]
        public bool PreventCancel { get; set; }
    }

    /// <summary>
    /// <see href="https://streams.spec.whatwg.org/#dictdef-underlyingsource">Streams browser specs</see>
    /// </summary>
    public class UnderlyingSource : IDisposable
    {
        protected readonly IJSRuntime jSRuntime;

        /// <summary>
        /// Constructs a wrapper instance.
        /// </summary>
        /// <param name="jSRuntime">An <see cref="IJSRuntime"/> instance.</param>
        public UnderlyingSource(IJSRuntime jSRuntime)
        {
            this.jSRuntime = jSRuntime;
            ObjRef = DotNetObjectReference.Create(this);
        }

        public DotNetObjectReference<UnderlyingSource> ObjRef { get; init; }

        [JsonIgnore]
        public Func<ReadableStreamDefaultController, Task>? Pull { get; set; }

        [JSInvokable]
        public async Task InvokePull(IJSObjectReference controller)
        {
            if (Pull is null)
            {
                return;
            }

            await Pull.Invoke(await ReadableStreamDefaultController.CreateAsync(jSRuntime, controller, new() { DisposesJSReference = true }));
        }

        public void Dispose()
        {
            ObjRef.Dispose();
            GC.SuppressFinalize(this);
        }
    }

    /// <summary>
    /// <see href="https://streams.spec.whatwg.org/#readablestreamdefaultcontroller">Streams browser specs</see>
    /// </summary>
    public class ReadableStreamDefaultController : IJSCreatable<ReadableStreamDefaultController>
    {
        /// <inheritdoc/>
        public IJSRuntime JSRuntime { get; }
        /// <inheritdoc/>
        public IJSObjectReference JSReference { get; }
        /// <inheritdoc/>
        public bool DisposesJSReference { get; }

        /// <inheritdoc/>
        public static async Task<ReadableStreamDefaultController> CreateAsync(IJSRuntime jSRuntime, IJSObjectReference jSReference)
        {
            return await CreateAsync(jSRuntime, jSReference, new());
        }

        /// <inheritdoc/>
        public static Task<ReadableStreamDefaultController> CreateAsync(IJSRuntime jSRuntime, IJSObjectReference jSReference, CreationOptions options)
        {
            return Task.FromResult(new ReadableStreamDefaultController(jSRuntime, jSReference, options));
        }

        /// <inheritdoc cref="CreateAsync(IJSRuntime, IJSObjectReference, CreationOptions)"/>
        protected ReadableStreamDefaultController(IJSRuntime jSRuntime, IJSObjectReference jSReference, CreationOptions options)
        {
            JSRuntime = jSRuntime;
            JSReference = jSReference;
            DisposesJSReference = options.DisposesJSReference;
        }

        /// <summary>
        /// The desired size to fill the controlled stream's internal queue.
        /// </summary>
        /// <returns>Negative values means that the queue is overfull.</returns>
        public async Task<double?> GetDesiredSizeAsync()
        {
            return await JSReference.GetValueAsync<double?>("desiredSize");
        }

        /// <summary>
        /// Enqueues the chunk in the controlled stream.
        /// </summary>
        /// <param name="chunk">An <see cref="ValueReference"/> supplied as the BYOB.</param>
        public async Task EnqueueAsync(object chunk)
        {
            await JSReference.InvokeVoidAsync("enqueue", chunk);
        }

        public async ValueTask DisposeAsync()
        {
            await IJSWrapper.DisposeJSReference(this);
            GC.SuppressFinalize(this);
        }
    }
}
