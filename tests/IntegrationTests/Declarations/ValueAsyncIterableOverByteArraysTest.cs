using KristofferStrube.Blazor.WebIDL.Declarations;
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
public class ValueAsyncIterableOverByteArraysTest(string browserName) : BlazorTest(browserName)
{
    private ReadableStreamForUint8Array readableStream = default!;

    [SetUp]
    public async Task CreateReadableStream()
    {
        byte i = 1;
        readableStream = await ReadableStreamForUint8Array.CreateAsync(JSRuntime, new UnderlyingSourceForUint8Array(JSRuntime)
        {
            Pull = async (controller) =>
            {
                double? size = await controller.GetDesiredSizeAsync();
                if (size > 0)
                {
                    await using Uint8Array buffer = await Uint8Array.CreateAsync(JSRuntime, (int)size);
                    await buffer.FillAsync(i++);
                    await controller.EnqueueAsync(buffer);
                }
            },
        });
    }

    [Test]
    public async Task ValuesAsync_ShouldReturnAsyncIteratorThatCanReturnValues()
    {
        // Act
        await using AsyncIterator<Uint8Array> iterator = await readableStream.ValuesAsync();

        // Assert
        _ = await iterator.MoveNextAsync();
        Uint8Array firstChunk = iterator.Current;
        byte valueInFirstChunk = await firstChunk.AtAsync(0);

        _ = await iterator.MoveNextAsync();
        Uint8Array secondChunk = iterator.Current;
        byte valueInSecondChunk = await secondChunk.AtAsync(0);

        _ = valueInFirstChunk.Should().Be(1);
        _ = valueInSecondChunk.Should().Be(2);
    }

    [Test]
    public async Task ValuesAsync_WithPreventCancelOptionForStream_ShouldReturnAsyncIteratorThatDoesNotPropagateCancel()
    {
        // Act
        AsyncIterator<Uint8Array> iterator = await readableStream.ValuesAsync(new()
        {
            PreventCancel = true
        });

        // Assert
        _ = await iterator.MoveNextAsync();
        Uint8Array firstChunk = iterator.Current;
        byte valueInFirstChunk = await firstChunk.AtAsync(0);

        await iterator.ReturnAsync();
        await iterator.DisposeAsync();
        iterator = await readableStream.ValuesAsync();

        _ = await iterator.MoveNextAsync();
        Uint8Array secondChunk = iterator.Current;
        byte valueInSecondChunk = await secondChunk.AtAsync(0);

        _ = valueInFirstChunk.Should().Be(1);
        _ = valueInSecondChunk.Should().Be(2);

        await iterator.DisposeAsync();
    }

    [Test]
    public async Task ValuesAsync_ShouldReturnIteratorThatDisposesElements_WhenConfiguredToDisposeThem()
    {
        // Act
        await using AsyncIterator<Uint8Array> iterator = await readableStream.ValuesAsync(
            disposePreviousValueWhenMovingToNextValue: true);

        // Assert
        List<Uint8Array> elements = await iterator.Take(5).ToListAsync();

        _ = elements.Should().AllSatisfy(element =>
        {
            _ = IsDisposed(element.JSReference).Should().BeTrue();
        });
    }

    [Test]
    public async Task ValuesAsync_ShouldReturnIteratorThatDoesNotDisposeElements_WhenConfiguredToNotDisposeThem()
    {
        // Act
        await using AsyncIterator<Uint8Array> iterator = await readableStream.ValuesAsync(
            disposePreviousValueWhenMovingToNextValue: false);

        // Assert
        List<Uint8Array> elements = await iterator.Take(5).ToListAsync();

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
    public class ReadableStreamForUint8Array : IJSCreatable<ReadableStreamForUint8Array>, IValueAsyncIterable<ReadableStreamForUint8Array, Uint8Array, ReadableStreamIteratorOptions>
    {
        /// <inheritdoc/>
        public IJSObjectReference JSReference { get; }

        /// <inheritdoc/>
        public IJSRuntime JSRuntime { get; }

        /// <inheritdoc/>
        public bool DisposesJSReference { get; }

        public static async Task<ReadableStreamForUint8Array> CreateAsync(IJSRuntime jSRuntime, UnderlyingSourceForUint8Array underlyingSource)
        {
            IJSObjectReference jSInstance = await jSRuntime.InvokeAsync<IJSObjectReference>("constructReadableStream", underlyingSource);
            return new ReadableStreamForUint8Array(jSRuntime, jSInstance, new() { DisposesJSReference = true });
        }

        /// <inheritdoc/>
        public static async Task<ReadableStreamForUint8Array> CreateAsync(IJSRuntime jSRuntime, IJSObjectReference jSReference)
        {
            return await CreateAsync(jSRuntime, jSReference, new());
        }

        /// <inheritdoc/>
        public static Task<ReadableStreamForUint8Array> CreateAsync(IJSRuntime jSRuntime, IJSObjectReference jSReference, CreationOptions options)
        {
            return Task.FromResult(new ReadableStreamForUint8Array(jSRuntime, jSReference, options));
        }

        protected ReadableStreamForUint8Array(IJSRuntime jSRuntime, IJSObjectReference jSReference, CreationOptions options)
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
    public class UnderlyingSourceForUint8Array : IDisposable
    {
        protected readonly IJSRuntime jSRuntime;

        /// <summary>
        /// Constructs a wrapper instance.
        /// </summary>
        /// <param name="jSRuntime">An <see cref="IJSRuntime"/> instance.</param>
        public UnderlyingSourceForUint8Array(IJSRuntime jSRuntime)
        {
            this.jSRuntime = jSRuntime;
            ObjRef = DotNetObjectReference.Create(this);
        }

        public DotNetObjectReference<UnderlyingSourceForUint8Array> ObjRef { get; init; }

        [JsonIgnore]
        public Func<ReadableByteStreamController, Task>? Pull { get; set; }

        [JSInvokable]
        public async Task InvokePull(IJSObjectReference controller)
        {
            if (Pull is null)
            {
                return;
            }

            await Pull.Invoke(await ReadableByteStreamController.CreateAsync(jSRuntime, controller, new() { DisposesJSReference = true }));
        }

        public void Dispose()
        {
            ObjRef.Dispose();
            GC.SuppressFinalize(this);
        }
    }

    /// <summary>
    /// <see href="https://streams.spec.whatwg.org/#rbs-controller-class">Streams browser specs</see>
    /// </summary>
    public class ReadableByteStreamController : IJSCreatable<ReadableByteStreamController>
    {
        /// <inheritdoc/>
        public IJSRuntime JSRuntime { get; }
        /// <inheritdoc/>
        public IJSObjectReference JSReference { get; }
        /// <inheritdoc/>
        public bool DisposesJSReference { get; }

        /// <inheritdoc/>
        public static async Task<ReadableByteStreamController> CreateAsync(IJSRuntime jSRuntime, IJSObjectReference jSReference)
        {
            return await CreateAsync(jSRuntime, jSReference, new());
        }

        /// <inheritdoc/>
        public static Task<ReadableByteStreamController> CreateAsync(IJSRuntime jSRuntime, IJSObjectReference jSReference, CreationOptions options)
        {
            return Task.FromResult(new ReadableByteStreamController(jSRuntime, jSReference, options));
        }

        /// <inheritdoc cref="CreateAsync(IJSRuntime, IJSObjectReference, CreationOptions)"/>
        protected ReadableByteStreamController(IJSRuntime jSRuntime, IJSObjectReference jSReference, CreationOptions options)
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
        /// <param name="chunk">An <see cref="Uint8Array"/> supplied as the BYOB.</param>
        public async Task EnqueueAsync(Uint8Array chunk)
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
