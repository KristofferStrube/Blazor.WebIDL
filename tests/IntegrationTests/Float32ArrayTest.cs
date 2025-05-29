namespace IntegrationTests;

public class Float32ArrayTest(string browserName) : BlazorTest(browserName)
{
    [Test]
    public async Task CreateAsync_WithNoArguments_Succeeds()
    {
        // Act
        Float32Array array = await Float32Array.CreateAsync(JSRuntime);
    }

    [Test]
    public async Task CreateAsync_WithLength_CreatesArrayWithLength()
    {
        // Act
        await using Float32Array array = await Float32Array.CreateAsync(JSRuntime, 4);

        // Assert
        long length = await array.GetLengthAsync();

        // Assert
        _ = length.Should().Be(4);
    }

    [Test]
    public async Task CreateAsync_WithArrayBuffer_CreatesArrayFromBuffer()
    {
        // Arrange
        await using Float64Array originalArray = await Float64Array.CreateAsync(JSRuntime, 4);
        await originalArray.FillAsync(1);
        await using IArrayBuffer arrayBuffer = await originalArray.GetBufferAsync();

        // Act
        await using Float32Array array = await Float32Array.CreateAsync(JSRuntime, arrayBuffer);

        // Assert
        float firstElement = await array.AtAsync(0);
        float secondElement = await array.AtAsync(1);
        _ = firstElement.Should().Be(0);
        _ = secondElement.Should().Be(1.875f);
    }

    [Test]
    public async Task CreateAsync_WithArrayBufferAndByteOffset_CreatesArrayFromBuffer()
    {
        // Arrange
        await using Float64Array originalArray = await Float64Array.CreateAsync(JSRuntime, 8);
        await originalArray.FillAsync(1);
        await using IArrayBuffer arrayBuffer = await originalArray.GetBufferAsync();

        // Act
        await using Float32Array array = await Float32Array.CreateAsync(JSRuntime, arrayBuffer, 4);

        // Assert
        float sum = 0;
        for (int i = 0; i < 3; i++)
        {
            sum += await array.AtAsync(i);
        }
        _ = sum.Should().Be(3.75f);
    }

    [Test]
    public async Task CreateAsync_WithArrayBufferByteOffsetAndLength_CreatesArrayFromBuffer()
    {
        // Arrange
        await using Float64Array originalArray = await Float64Array.CreateAsync(JSRuntime, 8);
        await originalArray.FillAsync(1);
        await using IArrayBuffer arrayBuffer = await originalArray.GetBufferAsync();

        // Act
        await using Float32Array array = await Float32Array.CreateAsync(JSRuntime, arrayBuffer, 4, 2);

        // Assert
        float result = await array.AtAsync(0);
        _ = result.Should().Be(1.875f);
    }

    [Test]
    public async Task CreateAsync_WithTypedArray_CreatesArrayFromTypedArray()
    {
        // Arrange
        await using Float32Array originalArray = await Float32Array.CreateAsync(JSRuntime, 2);
        await originalArray.FillAsync(1, 0, 1);
        await originalArray.FillAsync(2, 1, 2);

        // Act
        await using Float32Array array = await Float32Array.CreateAsync(JSRuntime, originalArray);

        // Assert
        float firstElement = await array.AtAsync(0);
        float secondElement = await array.AtAsync(1);
        _ = firstElement.Should().Be(1);
        _ = secondElement.Should().Be(2);
    }

    [Test]
    public async Task GetBufferAsync_GetsBuffer()
    {
        // Act
        await using Float32Array array = await Float32Array.CreateAsync(JSRuntime, 10);
        await using IArrayBuffer buffer = await array.GetBufferAsync();

        // Assert
        _ = buffer.Should().BeAssignableTo<IArrayBuffer>();
    }

    [Test]
    public async Task AtAsync_WithPositiveNumber_GetsElementFromStartOfArray()
    {
        // Arrange
        await using Float32Array array = await Float32Array.CreateAsync(JSRuntime, 10);
        await array.FillAsync(10);
        await array.FillAsync(20, 5);

        // Act
        float secondElement = await array.AtAsync(1);

        // Assert
        _ = secondElement.Should().Be(10);
    }

    [Test]
    public async Task AtAsync_WithNegativeNumber_GetsElementFromEndOfArray()
    {
        // Arrange
        await using Float32Array array = await Float32Array.CreateAsync(JSRuntime, 10);
        await array.FillAsync(10);
        await array.FillAsync(20, 5);

        // Act
        float secondElement = await array.AtAsync(-1);

        // Assert
        _ = secondElement.Should().Be(20);
    }

    [Test]
    public async Task FillAsync_WithNoArguments_FillsEntireArray()
    {
        // Arrange
        await using Float32Array array = await Float32Array.CreateAsync(JSRuntime, 10);

        // Act
        await array.FillAsync(10);

        // Assert
        float firstElement = await array.AtAsync(0);
        float lastElement = await array.AtAsync(-1);
        _ = firstElement.Should().Be(10);
        _ = lastElement.Should().Be(10);
    }

    [Test]
    public async Task FillAsync_WithStart_FillsArrayFromStartArgument()
    {
        // Arrange
        await using Float32Array array = await Float32Array.CreateAsync(JSRuntime, 10);

        // Act
        await array.FillAsync(10, 5);

        // Assert
        float firstElement = await array.AtAsync(0);
        float lastElement = await array.AtAsync(-1);
        _ = firstElement.Should().Be(0);
        _ = lastElement.Should().Be(10);
    }

    [Test]
    public async Task FillAsync_WithStartAndEnd_FillsArrayFromStartArgumentToEndArgument()
    {
        // Arrange
        await using Float32Array array = await Float32Array.CreateAsync(JSRuntime, 10);

        // Act
        await array.FillAsync(10, 1, 3);

        // Assert
        float firstElement = await array.AtAsync(0);
        float secondElement = await array.AtAsync(1);
        float thirdElement = await array.AtAsync(2);
        float fourthElement = await array.AtAsync(3);

        _ = firstElement.Should().Be(0);
        _ = secondElement.Should().Be(10);
        _ = thirdElement.Should().Be(10);
        _ = fourthElement.Should().Be(0);
    }

    [Test]
    public async Task GetLengthAsync_GetsLength()
    {
        // Arrange
        await using Float32Array array = await Float32Array.CreateAsync(JSRuntime, 10);

        // Act
        long length = await array.GetLengthAsync();

        // Assert
        _ = length.Should().Be(10);
    }
}
