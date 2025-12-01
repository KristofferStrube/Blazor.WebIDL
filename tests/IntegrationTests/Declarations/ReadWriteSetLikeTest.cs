using FluentAssertions.Execution;
using Microsoft.JSInterop;

namespace IntegrationTests.Declarations;

public class ReadWriteSetLikeTest(string browserName) : BlazorTest(browserName)
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
    public async Task ClearAsync_ShouldClearTheSet()
    {
        // Act
        await set.ClearAsync();

        // Assert
        ulong size = await set.GetSizeAsync();
        _ = size.Should().Be(0);
    }

    [Test]
    public async Task AddAsync_ShouldExpandTheSetWith_WhenTheElementIsNotAlreadyInSet()
    {
        // Arrange
        await using AbstractRange newRange = await CreateAbstractRange(JSRuntime, 0, 1);

        // Act
        await set.AddAsync(newRange);

        // Assert
        ulong size = await set.GetSizeAsync();
        _ = size.Should().Be(3);
    }

    [Test]
    public async Task AddAsync_ShouldNotExpandTheSetWith_WhenTheElementIsAlreadyInSet()
    {
        // Act
        await set.AddAsync(range1);

        // Assert
        ulong size = await set.GetSizeAsync();
        _ = size.Should().Be(2);
    }

    [Test]
    public async Task RemoveAsync_ShouldRemove_WhenTheElementIsInTheSet()
    {
        // Act
        bool removed = await set.DeleteAsync(range1);

        // Assert
        bool range1IsInSet = await set.HasAsync(range1);
        using AssertionScope scope = new();

        _ = removed.Should().BeTrue();
        _ = range1IsInSet.Should().BeFalse();
    }

    [Test]
    public async Task RemoveAsync_ShouldDoNothing_WhenTheElementIsNotInTheSet()
    {
        // Arrange
        await using AbstractRange newRange = await CreateAbstractRange(JSRuntime, 0, 1);

        // Act
        bool removed = await set.DeleteAsync(newRange);

        // Assert
        ulong size = await set.GetSizeAsync();
        using AssertionScope scope = new();

        _ = removed.Should().BeFalse();
        _ = size.Should().Be(2);
    }

    [IJSWrapperConverter]
    public class Highlight : IJSCreatable<Highlight>, IReadWriteSetlike<Highlight, AbstractRange>
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
