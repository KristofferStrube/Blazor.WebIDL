using Microsoft.JSInterop;
using Microsoft.JSInterop.Implementation;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace IntegrationTests.Declarations;

public class ReadonlyMapLikeTest(string browserName) : BlazorTest(browserName)
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
    public async Task GetSizeAsync_ShouldReturnTheSize()
    {
        // Act
        ulong size = await map.GetSizeAsync();

        // Assert
        _ = size.Should().Be(2);
    }

    [Test]
    public async Task HasAsync_ShouldReturnTrue_WhenKeyExists()
    {
        // Act
        bool exists = await map.HasAsync("highlight1");

        // Assert
        _ = exists.Should().BeTrue();
    }

    [Test]
    public async Task HasAsync_ShouldReturnFalse_WhenKeyDoesntExist()
    {
        // Act
        bool exists = await map.HasAsync("some-other-highlight");

        // Assert
        _ = exists.Should().BeFalse();
    }

    [Test]
    public async Task KeysAsync_ShouldReturnAllKeys()
    {
        // Act
        await using Iterator<string> keyIterator = await map.KeysAsync();

        List<string> keys = await keyIterator.ToListAsync();

        // Assert
        _ = keys.Should().BeEquivalentTo(["highlight1", "highlight2"]);
    }

    [Test]
    public async Task ValuesAsync_ShouldReturnAllValues()
    {
        // Arrange
        List<HighlightType> types = [];

        // Act
        await using Iterator<Highlight> valueIterator = await map.ValuesAsync();
        await foreach (Highlight highligh in valueIterator)
        {
            types.Add(await highligh.GetTypeAsync());
        }

        // Assert
        _ = types.Should().AllBeEquivalentTo(HighlightType.Highlight);
    }

    [Test]
    public async Task ValuesAsync_ShouldHaveDisposedAllValues_WhenDoneIterating_AndItIsConfiguredToDispose()
    {
        // Act
        await using Iterator<Highlight> valueIterator = await map.ValuesAsync(disposePreviousValueWhenMovingToNextValue: true);

        List<Highlight> values = await valueIterator.ToListAsync();

        // Assert
        _ = values.Should().AllSatisfy(highlight => IsDisposed(highlight.JSReference));
    }

    [Test]
    public async Task ValuesAsync_ShouldNotHaveDisposedValues_WhenDoneIterating_AndItIsConfiguredToNotDispose()
    {
        // Act
        await using Iterator<Highlight> valueIterator = await map.ValuesAsync(disposePreviousValueWhenMovingToNextValue: false);

        List<Highlight> values = await valueIterator.ToListAsync();

        // Assert
        _ = values.Should().AllSatisfy(highlight => IsNotDisposed(highlight.JSReference));
    }

    [Test]
    public async Task EntriesAsync_ShouldReturnAllEntries()
    {
        // Act
        await using Iterator<string, Highlight> entriesIterator = await map.EntriesAsync();

        List<string> keys = await entriesIterator.Select(kvp => kvp.Key).ToListAsync();

        // Assert
        _ = keys.Should().BeEquivalentTo(["highlight1", "highlight2"]);
    }

    [Test]
    public async Task EntriesAsync_ShouldHaveDisposedAllValues_WhenDoneIterating_AndItIsConfiguredToDispose()
    {
        // Act
        await using Iterator<string, Highlight> entriesIterator = await map.EntriesAsync(disposePreviousValueWhenMovingToNextValue: true);

        List<Highlight> highlights = await entriesIterator.Select(kvp => kvp.Value).ToListAsync();

        // Assert
        _ = highlights.Should().AllSatisfy(highlight => IsDisposed(highlight.JSReference));
    }

    [Test]
    public async Task EntriesAsync_ShouldNotHaveDisposedValues_WhenDoneIterating_AndItIsConfiguredToNotDispose()
    {
        // Act
        await using Iterator<string, Highlight> entriesIterator = await map.EntriesAsync(disposePreviousValueWhenMovingToNextValue: false);

        List<Highlight> highlights = await entriesIterator.Select(kvp => kvp.Value).ToListAsync();

        // Assert
        _ = highlights.Should().AllSatisfy(highlight => IsNotDisposed(highlight.JSReference));
    }

    [Test]
    public async Task GetAsync_ShouldGetTheValue_WhenItExists()
    {
        // Act
        await using Highlight? value = await map.GetAsync("highlight1");

        // Assert
        _ = value.Should().NotBeNull();
    }

    [Test]
    public async Task GetAsync_ShouldReturnNull_WhenItDoesNotExist()
    {
        // Act
        await using Highlight? value = await map.GetAsync("some-other-highlight");

        // Assert
        _ = value.Should().BeNull();
    }

    [Test]
    public async Task ForEachAsync_ShouldCallBackOnceForEachElement_WhenCalledWithNoArgFunc()
    {
        // Arrange
        int count = 0;

        // Act
        await map.ForEachAsync(async () =>
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
        List<HighlightType> types = [];

        // Act
        await map.ForEachAsync(async (Highlight highlight) =>
        {
            types.Add(await highlight.GetTypeAsync());
        });

        // Assert
        // We need to check multiple times as ForEachAsync doesn't finish the values it iterates sequentually.
        Assert.That(() => types is [HighlightType.Highlight, HighlightType.Highlight], 
            Is.True.After(delayInMilliseconds: 1000, 100));
    }

    [Test]
    public async Task ForEachAsync_ShouldHaveDisposedAllValues_WhenCalledWithOneArgFunc_AndItIsConfiguredToDispose()
    {
        // Arrange
        List<Highlight> highlights = [];

        // Act
        await map.ForEachAsync(async (Highlight highlight) =>
        {
            highlights.Add(highlight);
        }, disposeValueWhenFunctionHasBeenInvoked: true);

        // Assert
        // We need to check multiple times as ForEachAsync doesn't finish the values it iterates sequentually.
        Assert.That(() => highlights.Count is 2 && highlights.All(highlight => IsDisposed(highlight.JSReference)),
            Is.True.After(delayInMilliseconds: 1000, 100));
    }

    [Test]
    public async Task ForEachAsync_ShouldNotHaveDisposedValues_WhenCalledWithOneArgFunc_AndItIsConfiguredToNotDispose()
    {
        // Arrange
        List<Highlight> highlights = [];

        // Act
        await map.ForEachAsync(async (Highlight highlight) =>
        {
            highlights.Add(highlight);
        }, disposeValueWhenFunctionHasBeenInvoked: false);

        // Assert
        // We need to check multiple times as ForEachAsync doesn't finish the values it iterates sequentually.
        Assert.That(() => highlights.Count is 2 && highlights.All(highlight => IsNotDisposed(highlight.JSReference)),
            Is.True.After(delayInMilliseconds: 1000, 100));
    }

    [Test]
    public async Task ForEachAsync_ShouldHaveDisposedAllValues_WhenCalledWithTwoArgFunc_AndItIsConfiguredToDispose()
    {
        // Arrange
        List<Highlight> highlights = [];

        // Act
        await map.ForEachAsync(async (Highlight highlight, string key) =>
        {
            highlights.Add(highlight);
        }, disposeKeyAndValueWhenFunctionHasBeenInvoked: true);

        // Assert
        // We need to check multiple times as ForEachAsync doesn't finish the values it iterates sequentually.
        Assert.That(() => highlights.Count is 2 && highlights.All(highlight => IsDisposed(highlight.JSReference)),
            Is.True.After(delayInMilliseconds: 1000, 100));
    }

    [Test]
    public async Task ForEachAsync_ShouldNotHaveDisposedValues_WhenCalledWithTwoArgFunc_AndItIsConfiguredToNotDispose()
    {
        // Arrange
        List<Highlight> highlights = [];

        // Act
        await map.ForEachAsync(async (Highlight highlight, string key) =>
        {
            highlights.Add(highlight);
        }, disposeKeyAndValueWhenFunctionHasBeenInvoked: false);

        // Assert
        // We need to check multiple times as ForEachAsync doesn't finish the values it iterates sequentually.
        Assert.That(() => highlights.Count is 2 && highlights.All(highlight => IsNotDisposed(highlight.JSReference)),
            Is.True.After(delayInMilliseconds: 1000, 100));
    }

    [Test]
    public async Task ForEachAsync_ShouldReturnValuesAndKeys_WhenCalledWithTwoArgFunc()
    {
        // Arrange
        List<HighlightType> types = [];
        List<string> keys = [];

        // Act
        await map.ForEachAsync(async (Highlight highlight, string key) =>
        {
            types.Add(await highlight.GetTypeAsync());
            keys.Add(key);
        });

        // Assert
        // We need to check multiple times as ForEachAsync doesn't finish the values it iterates sequentually.
        Assert.That(() => types is [HighlightType.Highlight, HighlightType.Highlight] && keys is ["highlight1", "highlight2"],
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
    public class HighlightRegistry : IJSCreatable<HighlightRegistry>, IReadonlyMapLike<HighlightRegistry, string, Highlight>
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

        public async Task<HighlightType> GetTypeAsync()
        {
            return await JSReference.GetValueAsync<HighlightType>("type");
        }

        public async ValueTask DisposeAsync()
        {
            await IJSWrapper.DisposeJSReference(this);
        }
    }

    [JsonConverter(typeof(HighlightTypeConverter))]
    public enum HighlightType
    {
        Highlight,
        SpellingError,
        GrammarError,
    }

    public class HighlightTypeConverter : JsonConverter<HighlightType>
    {
        public override HighlightType Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return reader.GetString() switch
            {
                "highlight" => HighlightType.Highlight,
                "spelling-error" => HighlightType.SpellingError,
                "grammar-error" => HighlightType.GrammarError,
                var value => throw new ArgumentException($"Value '{value}' was not a valid {nameof(HighlightType)}."),
            };
        }

        public override void Write(Utf8JsonWriter writer, HighlightType value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}
