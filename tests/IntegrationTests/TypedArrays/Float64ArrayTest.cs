namespace IntegrationTests.TypedArrays;

public class Float64ArrayTest(string browserName) : BlazorTest(browserName)
{
    [Test]
    public async Task CreateAsync_WithNoArguments_Succeeds()
    {
        // Arrange
        await using Float64Array array = await Float64Array.CreateAsync(JSRuntime);
    }

    [Test]
    public async Task CreateAsync_WithLength_CreatesArrayWithLength()
    {
        // Act
        await using Float64Array array = await Float64Array.CreateAsync(JSRuntime, 4);

        // Assert
        long length = await array.GetLengthAsync();
        _ = length.Should().Be(4);
    }

    [Test]
    public async Task CreateAsync_WithArrayBuffer_CreatesArrayFromBuffer()
    {
        // Arrange
        await using Float32Array original = await Float32Array.CreateAsync(JSRuntime, 4);
        await original.FillAsync(1, 1, 2);
        await using IArrayBuffer buffer = await original.GetBufferAsync();

        // Act
        await using Float64Array array = await Float64Array.CreateAsync(JSRuntime, buffer);

        // Assert
        double firstElement = await array.AtAsync(0);
        double secondElement = await array.AtAsync(1);
        _ = firstElement.Should().Be(0.0078125);
        _ = secondElement.Should().Be(0);
    }

    [Test]
    public async Task CreateAsync_WithArrayBufferAndByteOffset_CreatesArrayFromBuffer()
    {
        // Arrange
        await using Float64Array originalArray = await Float64Array.CreateAsync(JSRuntime, 12);
        await originalArray.FillAsync(1, 0, 8);
        await using IArrayBuffer arrayBuffer = await originalArray.GetBufferAsync();

        // Act
        await using Float64Array array = await Float64Array.CreateAsync(JSRuntime, arrayBuffer, 8);

        // Assert
        double sum = 0;
        for (int i = 0; i < 2; i++)
        {
            sum += await array.AtAsync(i);
        }
        _ = sum.Should().Be(2);
    }

    [Test]
    public async Task CreateAsync_WithArrayBufferByteOffsetAndLength_CreatesArrayFromBuffer()
    {
        // Arrange
        await using Float64Array originalArray = await Float64Array.CreateAsync(JSRuntime, 24);
        await originalArray.FillAsync(1);
        await using IArrayBuffer arrayBuffer = await originalArray.GetBufferAsync();

        // Act
        await using Float64Array array = await Float64Array.CreateAsync(JSRuntime, arrayBuffer, 8, 2);

        // Assert
        double firstElement = await array.AtAsync(0);
        _ = firstElement.Should().Be(1);
    }

    [Test]
    public async Task CreateAsync_WithTypedArray_CreatesArrayFromTypedArray()
    {
        // Arrange
        await using Float32Array originalArray = await Float32Array.CreateAsync(JSRuntime, 2);
        await originalArray.FillAsync(1, 0, 1);
        await originalArray.FillAsync(2, 1, 2);

        // Act
        await using Float64Array array = await Float64Array.CreateAsync(JSRuntime, originalArray);

        // Assert
        double firstElement = await array.AtAsync(0);
        double secondElement = await array.AtAsync(1);
        _ = firstElement.Should().Be(1);
        _ = secondElement.Should().Be(2);
    }

    [Test]
    public async Task GetBufferAsync_GetsBuffer()
    {
        // Arrange
        await using Float64Array array = await Float64Array.CreateAsync(JSRuntime, 10);

        // Act
        await using IArrayBuffer buffer = await array.GetBufferAsync();
    }

    [Test]
    public async Task AtAsync_WithPositiveNumber_GetsElementFromStartOfArray()
    {
        // Arrange
        await using Float64Array array = await Float64Array.CreateAsync(JSRuntime, 10);
        await array.FillAsync(10);
        await array.FillAsync(20, 5);

        // Act
        double secondElement = await array.AtAsync(1);

        // Assert
        _ = secondElement.Should().Be(10);
    }

    [Test]
    public async Task AtAsync_WithNegativeNumber_GetsElementFromEndOfArray()
    {
        // Arrange
        await using Float64Array array = await Float64Array.CreateAsync(JSRuntime, 10);
        await array.FillAsync(10);
        await array.FillAsync(20, 5);

        // Act
        double secondElement = await array.AtAsync(-1);

        // Assert
        _ = secondElement.Should().Be(20);
    }

    [Test]
    public async Task FillAsync_WithNoArguments_FillsEntireArray()
    {
        // Arrange
        await using Float64Array array = await Float64Array.CreateAsync(JSRuntime, 10);

        // Act
        await array.FillAsync(10);

        // Assert
        double firstElement = await array.AtAsync(0);
        double lastElement = await array.AtAsync(-1);
        _ = firstElement.Should().Be(10);
        _ = lastElement.Should().Be(10);
    }

    [Test]
    public async Task FillAsync_WithStart_FillsArrayFromStartArgument()
    {
        // Arrange
        await using Float64Array array = await Float64Array.CreateAsync(JSRuntime, 10);

        // Act
        await array.FillAsync(10, 5);

        // Assert
        double firstElement = await array.AtAsync(0);
        double lastElement = await array.AtAsync(-1);
        _ = firstElement.Should().Be(0);
        _ = lastElement.Should().Be(10);
    }

    [Test]
    public async Task FillAsync_WithStartAndEnd_FillsArrayFromStartArgumentToEndArgument()
    {
        // Arrange
        await using Float64Array array = await Float64Array.CreateAsync(JSRuntime, 10);

        // Act
        await array.FillAsync(10, 1, 3);

        // Assert
        double firstElement = await array.AtAsync(0);
        double secondElement = await array.AtAsync(1);
        double thirdElement = await array.AtAsync(2);
        double fourthElement = await array.AtAsync(3);
        _ = firstElement.Should().Be(0);
        _ = secondElement.Should().Be(10);
        _ = thirdElement.Should().Be(10);
        _ = fourthElement.Should().Be(0);
    }

    [Test]
    public async Task GetLengthAsync_GetsLength()
    {
        // Arrange
        await using Float64Array array = await Float64Array.CreateAsync(JSRuntime, 10);

        // Act
        long length = await array.GetLengthAsync();

        // Assert
        _ = length.Should().Be(10);
    }
}
