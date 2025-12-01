using FluentAssertions.Execution;
using Microsoft.JSInterop;
using Microsoft.JSInterop.Implementation;
using System.Reflection;

namespace IntegrationTests.Declarations;

public class ReadonlySetLikeTest(string browserName) : BlazorTest(browserName)
{
    private Highlight set = default!;
    private AbstractRange range1 = default!;
    private AbstractRange range2 = default!;

    protected override Dictionary<string, object>? FirefoxUserPrefs => new()
    {
        ["dom.customHighlightAPI.enabled"] = true
    };

    [SetUp]
    public async Task CreateSet()
    {
        range1 = await CreateAbstractRange(JSRuntime, 0, 1);
        range2 = await CreateAbstractRange(JSRuntime, 1, 2);
        set = await Highlight.CreateAsync(JSRuntime, range1, range2);
    }

    [Test]
    public async Task GetSizeAsync_ShouldReturnTheSize()
    {
        // Act
        ulong size = await set.GetSizeAsync();

        // Assert
        _ = size.Should().Be(2);
    }

    [Test]
    public async Task HasAsync_ShouldReturnTrue_WhenKeyExists()
    {
        // Act
        bool exists = await set.HasAsync(range1);

        // Assert
        _ = exists.Should().BeTrue();
    }

    [Test]
    public async Task HasAsync_ShouldReturnFalse_WhenKeyDoesntExist()
    {
        // Arrange
        await using AbstractRange otherRange = await CreateAbstractRange(JSRuntime, 0, 1);

        // Act
        bool exists = await set.HasAsync(otherRange);

        // Assert
        _ = exists.Should().BeFalse();
    }

    [Test]
    public async Task KeysAsync_ShouldReturnAllKeys()
    {
        // Arrange
        List<ulong> startOffsets = [];

        // Act
        await using Iterator<AbstractRange> keyIterator = await set.KeysAsync();

        await foreach(AbstractRange key in keyIterator)
        {
            startOffsets.Add(await key.GetStartOffset());
        }

        // Assert
        _ = startOffsets.Should().BeEquivalentTo([0, 1]);
    }

    [Test]
    public async Task KeysAsync_ShouldHaveDisposedAllKeys_WhenDoneIterating_AndItIsConfiguredToDispose()
    {
        // Act
        await using Iterator<AbstractRange> valueIterator = await set.ValuesAsync(disposePreviousValueWhenMovingToNextValue: true);

        List<AbstractRange> values = await valueIterator.ToListAsync();

        // Assert
        _ = values.Should().AllSatisfy(range => IsDisposed(range.JSReference));
    }

    [Test]
    public async Task KeysAsync_ShouldNotHaveDisposedKeys_WhenDoneIterating_AndItIsConfiguredToNotDispose()
    {
        // Act
        await using Iterator<AbstractRange> valueIterator = await set.ValuesAsync(disposePreviousValueWhenMovingToNextValue: false);

        List<AbstractRange> values = await valueIterator.ToListAsync();

        // Assert
        _ = values.Should().AllSatisfy(range => IsNotDisposed(range.JSReference));
    }

    [Test]
    public async Task ValuesAsync_ShouldReturnAllValues()
    {
        // Arrange
        List<ulong> startOffsets = [];

        // Act
        await using Iterator<AbstractRange> valueIterator = await set.ValuesAsync();

        await foreach (AbstractRange value in valueIterator)
        {
            startOffsets.Add(await value.GetStartOffset());
        }

        // Assert
        _ = startOffsets.Should().BeEquivalentTo([0, 1]);
    }

    [Test]
    public async Task ValuesAsync_ShouldHaveDisposedAllValues_WhenDoneIterating_AndItIsConfiguredToDispose()
    {
        // Act
        await using Iterator<AbstractRange> valueIterator = await set.ValuesAsync(disposePreviousValueWhenMovingToNextValue: true);

        List<AbstractRange> values = await valueIterator.ToListAsync();

        // Assert
        _ = values.Should().AllSatisfy(range => IsDisposed(range.JSReference));
    }

    [Test]
    public async Task ValuesAsync_ShouldNotHaveDisposedValues_WhenDoneIterating_AndItIsConfiguredToNotDispose()
    {
        // Act
        await using Iterator<AbstractRange> valueIterator = await set.ValuesAsync(disposePreviousValueWhenMovingToNextValue: false);

        List<AbstractRange> values = await valueIterator.ToListAsync();

        // Assert
        _ = values.Should().AllSatisfy(range => IsNotDisposed(range.JSReference));
    }

    [Test]
    public async Task EntriesAsync_ShouldReturnAllEntries()
    {
        // Arrange
        List<ulong> startOffsetsFromKey = [];
        List<ulong> startOffsetsFromValue = [];

        // Act
        await using Iterator<AbstractRange, AbstractRange> valueIterator = await set.EntriesAsync();

        await foreach ((AbstractRange key, AbstractRange value) in valueIterator)
        {
            startOffsetsFromKey.Add(await key.GetStartOffset());
            startOffsetsFromValue.Add(await value.GetStartOffset());
        }

        // Assert
        using var scope = new AssertionScope();

        _ = startOffsetsFromKey.Should().BeEquivalentTo([0, 1]);
        _ = startOffsetsFromValue.Should().BeEquivalentTo([0, 1]);
    }

    [Test]
    public async Task EntriesAsync_ShouldHaveDisposedAllKeysAndValues_WhenDoneIterating_AndItIsConfiguredToDispose()
    {
        // Act
        await using Iterator<AbstractRange, AbstractRange> entriesIterator = await set.EntriesAsync(disposePreviousValueWhenMovingToNextValue: true);

        List<KeyValuePair<AbstractRange, AbstractRange>> ranges = await entriesIterator.ToListAsync();

        // Assert
        using var scope = new AssertionScope();

        _ = ranges.Should().AllSatisfy(kvp => IsDisposed(kvp.Key.JSReference));
        _ = ranges.Should().AllSatisfy(kvp => IsDisposed(kvp.Value.JSReference));
    }

    [Test]
    public async Task EntriesAsync_ShouldNotHaveDisposedValues_WhenDoneIterating_AndItIsConfiguredToNotDispose()
    {
        // Act
        await using Iterator<AbstractRange, AbstractRange> entriesIterator = await set.EntriesAsync(disposePreviousValueWhenMovingToNextValue: false);

        List<KeyValuePair<AbstractRange, AbstractRange>> ranges = await entriesIterator.ToListAsync();

        // Assert
        using var scope = new AssertionScope();

        _ = ranges.Should().AllSatisfy(kvp => IsNotDisposed(kvp.Key.JSReference));
        _ = ranges.Should().AllSatisfy(kvp => IsNotDisposed(kvp.Value.JSReference));
    }

    [Test]
    public async Task ForEachAsync_ShouldCallBackOnceForEachElement_WhenCalledWithNoArgFunc()
    {
        // Arrange
        int count = 0;

        // Act
        await set.ForEachAsync(async () =>
        {
            count++;
        });

        // Assert
        Assert.That(() => count is 2,
            Is.True.After(delayInMilliseconds: 1000, 100));
    }

    [Test]
    public async Task ForEachAsync_ShouldReturnValues_WhenCalledWithOneArgFunc()
    {
        // Arrange
        List<ulong> startOffsets = [];

        // Act
        await set.ForEachAsync(async (AbstractRange range) =>
        {
            startOffsets.Add(await range.GetStartOffset());
        });

        // Assert
        // We need to check multiple times as ForEachAsync doesn't finish the values it iterates sequentually.
        Assert.That(() => startOffsets is [0, 1],
            Is.True.After(delayInMilliseconds: 1000, 100));
    }

    [Test]
    public async Task ForEachAsync_ShouldHaveDisposedAllValues_WhenCalledWithOneArgFunc_AndItIsConfiguredToDispose()
    {
        // Arrange
        List<AbstractRange> ranges = [];

        // Act
        await set.ForEachAsync(async (AbstractRange range) =>
        {
            ranges.Add(range);
        }, disposeValueWhenFunctionHasBeenInvoked: true);

        // Assert
        // We need to check multiple times as ForEachAsync doesn't finish the values it iterates sequentually.
        Assert.That(() => ranges.Count is 2 && ranges.All(highlight => IsDisposed(highlight.JSReference)),
            Is.True.After(delayInMilliseconds: 1000, 100));
    }

    [Test]
    public async Task ForEachAsync_ShouldNotHaveDisposedValues_WhenCalledWithOneArgFunc_AndItIsConfiguredToNotDispose()
    {
        // Arrange
        List<AbstractRange> ranges = [];

        // Act
        await set.ForEachAsync(async (AbstractRange range) =>
        {
            ranges.Add(range);
        }, disposeValueWhenFunctionHasBeenInvoked: false);

        // Assert
        // We need to check multiple times as ForEachAsync doesn't finish the values it iterates sequentually.
        Assert.That(() => ranges.Count is 2 && ranges.All(highlight => IsNotDisposed(highlight.JSReference)),
            Is.True.After(delayInMilliseconds: 1000, 100));
    }

    private static bool IsDisposed(IJSObjectReference reference)
    {
        PropertyInfo disposedProperty = typeof(JSObjectReference).GetProperty("Disposed", BindingFlags.Instance | BindingFlags.NonPublic)!;
        bool value = (bool)disposedProperty.GetValue(reference, null)!;
        return value;
    }

    private static bool IsNotDisposed(IJSObjectReference reference) => !IsDisposed(reference);

    [IJSWrapperConverter]
    public class Highlight : IJSCreatable<Highlight>, IReadonlySetlike<Highlight, AbstractRange>
    {
        /// <inheritdoc/>
        public IJSObjectReference JSReference { get; }

        /// <inheritdoc/>
        public IJSRuntime JSRuntime { get; }

        /// <inheritdoc/>
        public bool DisposesJSReference { get; }

        public static async Task<Highlight> CreateAsync(IJSRuntime jSRuntime, params AbstractRange[] args)
        {
            IJSObjectReference jSInstance = await jSRuntime.InvokeConstructorAsync("Highlight", (object?[]?)args);
            return new(jSRuntime, jSInstance, new() { DisposesJSReference = true });
        }

        /// <inheritdoc/>
        public static async Task<Highlight> CreateAsync(IJSRuntime jSRuntime, IJSObjectReference jSReference)
        {
            return await CreateAsync(jSRuntime, jSReference, new());
        }

        /// <inheritdoc/>
        public static Task<Highlight> CreateAsync(IJSRuntime jSRuntime, IJSObjectReference jSReference, CreationOptions options)
        {
            return Task.FromResult(new Highlight(jSRuntime, jSReference, options));
        }

        protected Highlight(IJSRuntime jSRuntime, IJSObjectReference jSReference, CreationOptions options)
        {
            JSRuntime = jSRuntime;
            JSReference = jSReference;
            DisposesJSReference = options.DisposesJSReference;
        }

        public async ValueTask DisposeAsync()
        {
            await IJSWrapper.DisposeJSReference(this);
        }
    }

    [IJSWrapperConverter]
    public class AbstractRange : IJSCreatable<AbstractRange>
    {
        /// <inheritdoc/>
        public IJSObjectReference JSReference { get; }

        /// <inheritdoc/>
        public IJSRuntime JSRuntime { get; }

        /// <inheritdoc/>
        public bool DisposesJSReference { get; }

        /// <inheritdoc/>
        public static async Task<AbstractRange> CreateAsync(IJSRuntime jSRuntime, IJSObjectReference jSReference)
        {
            return await CreateAsync(jSRuntime, jSReference, new());
        }

        /// <inheritdoc/>
        public static Task<AbstractRange> CreateAsync(IJSRuntime jSRuntime, IJSObjectReference jSReference, CreationOptions options)
        {
            return Task.FromResult(new AbstractRange(jSRuntime, jSReference, options));
        }

        protected AbstractRange(IJSRuntime jSRuntime, IJSObjectReference jSReference, CreationOptions options)
        {
            JSRuntime = jSRuntime;
            JSReference = jSReference;
            DisposesJSReference = options.DisposesJSReference;
        }

        public async Task<ulong> GetStartOffset() => await JSReference.GetValueAsync<ulong>("startOffset");

        public async Task<ulong> GetEndOffset() => await JSReference.GetValueAsync<ulong>("endOffset");

        public async ValueTask DisposeAsync()
        {
            await IJSWrapper.DisposeJSReference(this);
        }
    }

    private static async Task<AbstractRange> CreateAbstractRange(IJSRuntime jSRuntime, ulong startOffset, ulong endOffset)
    {
        await using IJSObjectReference body = await jSRuntime.GetValueAsync<IJSObjectReference>("document.body");

        IJSObjectReference jSRange1 = await jSRuntime.InvokeConstructorAsync("StaticRange",
            new { startContainer = body, startOffset, endContainer = body, endOffset }
        );
        return await AbstractRange.CreateAsync(jSRuntime, jSRange1, new() { DisposesJSReference = true });
    }
}
