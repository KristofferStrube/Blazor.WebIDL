using FluentAssertions.Execution;
using Microsoft.JSInterop;
namespace IntegrationTests.Declarations;

public class ReadWriteMapLikeTest(string browserName) : BlazorTest(browserName)
{
    private HighlightRegistry map = default!;

    protected override Dictionary<string, object>? FirefoxUserPrefs => new()
    {
        ["dom.customHighlightAPI.enabled"] = true
    };

    [SetUp]
    public async Task CreateMap()
    {
        IJSObjectReference jSInstance = await JSRuntime.GetValueAsync<IJSObjectReference>("CSS.highlights");

        await using Highlight highlight1 = await Highlight.CreateAsync(JSRuntime);
        await using Highlight highlight2 = await Highlight.CreateAsync(JSRuntime);

        await jSInstance.InvokeVoidAsync("set", "highlight1", highlight1);
        await jSInstance.InvokeVoidAsync("set", "highlight2", highlight2);

        map = await HighlightRegistry.CreateAsync(JSRuntime, jSInstance, new() { DisposesJSReference = true });
    }

    [Test]
    public async Task SetAsync_ShouldExpandTheMap_WhenTheKeyWasNotAlreadyInTheMap()
    {
        // Arrange
        await using Highlight newHighlight = await Highlight.CreateAsync(JSRuntime);

        // Act
        await map.SetAsync("newHightlight", newHighlight);

        // Assert
        ulong size = await map.GetSizeAsync();
        _ = size.Should().Be(3);
    }

    [Test]
    public async Task SetAsync_ShouldReplaceValue_WhenTheKeyWasAlreadyInTheMap()
    {
        // Arrange
        await using Highlight newHighlight = await Highlight.CreateAsync(JSRuntime);

        // Act
        await map.SetAsync("highlight1", newHighlight);

        // Assert
        using AssertionScope scope = new();

        ulong size = await map.GetSizeAsync();
        _ = size.Should().Be(2);

        await using Highlight highlightNowInMap = (await map.GetAsync("highlight1"))!;

        bool newHighlightIsSameAsInMap = await JSRuntime.InvokeAsync<bool>("Object.is", newHighlight, highlightNowInMap);
        _ = newHighlightIsSameAsInMap.Should().BeTrue();
    }

    [Test]
    public async Task ClearAsync_ShouldClearTheMap()
    {
        // Act
        await map.ClearAsync();

        // Assert
        ulong size = await map.GetSizeAsync();
        _ = size.Should().Be(0);
    }

    [Test]
    public async Task DeleteAsync_ShouldRemoveTheEntry_WhenTheKeyIsInTheMap()
    {
        // Act
        bool removed = await map.DeleteAsync("highlight1");

        // Assert
        using AssertionScope scope = new();

        _ = removed.Should().BeTrue();

        ulong size = await map.GetSizeAsync();
        _ = size.Should().Be(1);

        Highlight? highlightAtKeyInMap = await map.GetAsync("highglight1");
        _ = highlightAtKeyInMap.Should().BeNull();
    }

    [Test]
    public async Task DeleteAsync_ShouldDoNothing_WhenTheKeyIsNotInTheMap()
    {
        // Act
        bool removed = await map.DeleteAsync("another-highlight");

        // Assert
        using AssertionScope scope = new();

        _ = removed.Should().BeFalse();

        ulong size = await map.GetSizeAsync();
        _ = size.Should().Be(2);
    }

    [IJSWrapperConverter]
    public class HighlightRegistry : IJSCreatable<HighlightRegistry>, IReadWriteMapLike<HighlightRegistry, string, Highlight>
    {
        /// <inheritdoc/>
        public IJSObjectReference JSReference { get; }

        /// <inheritdoc/>
        public IJSRuntime JSRuntime { get; }

        /// <inheritdoc/>
        public bool DisposesJSReference { get; }

        /// <inheritdoc/>
        public static async Task<HighlightRegistry> CreateAsync(IJSRuntime jSRuntime, IJSObjectReference jSReference)
        {
            return await CreateAsync(jSRuntime, jSReference, new());
        }

        /// <inheritdoc/>
        public static Task<HighlightRegistry> CreateAsync(IJSRuntime jSRuntime, IJSObjectReference jSReference, CreationOptions options)
        {
            return Task.FromResult(new HighlightRegistry(jSRuntime, jSReference, options));
        }

        protected HighlightRegistry(IJSRuntime jSRuntime, IJSObjectReference jSReference, CreationOptions options)
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
    public class Highlight : IJSCreatable<Highlight>
    {
        /// <inheritdoc/>
        public IJSObjectReference JSReference { get; }

        /// <inheritdoc/>
        public IJSRuntime JSRuntime { get; }

        /// <inheritdoc/>
        public bool DisposesJSReference { get; }

        public static async Task<Highlight> CreateAsync(IJSRuntime jSRuntime)
        {
            IJSObjectReference highlight1 = await jSRuntime.InvokeConstructorAsync("Highlight");

            return new Highlight(jSRuntime, highlight1, new() { DisposesJSReference = true });
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
}
