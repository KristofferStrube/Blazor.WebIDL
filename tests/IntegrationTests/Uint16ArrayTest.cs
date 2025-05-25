namespace IntegrationTests;

public class Uint16ArrayTest(string browserName) : BlazorTest(browserName)
{
    [Test]
    public async Task CreateAsync_WithNoArguments_Succeeds()
    {
        // Act
        Uint16Array array = await Uint16Array.CreateAsync(JSRuntime);
    }

    [Test]
    public async Task CreateAsync_WithLength_CreatesArrayWithLength()
    {
        // Act
        await using Uint16Array array = await Uint16Array.CreateAsync(JSRuntime, 4);

        // Assert
        long length = await array.GetLengthAsync();
        _ = length.Should().Be(4);
    }

    [Test]
    public async Task CreateAsync_WithArrayBuffer_CreatesArrayFromBuffer()
    {
        // Arrange
        await using Uint32Array originalArray = await Uint32Array.CreateAsync(JSRuntime, 1);
        await originalArray.FillAsync(1);
        await using IArrayBuffer arrayBuffer = await originalArray.GetBufferAsync();

        // Act
        await using Uint16Array array = await Uint16Array.CreateAsync(JSRuntime, arrayBuffer);

        // Assert
        ushort firstElement = await array.AtAsync(0);
        ushort secondElement = await array.AtAsync(1);
        _ = firstElement.Should().Be(1);
        _ = secondElement.Should().Be(0);
    }

    [Test]
    public async Task CreateAsync_WithArrayBufferAndByteOffset_CreatesArrayFromBuffer()
    {
        // Arrange
        await using Uint32Array originalArray = await Uint32Array.CreateAsync(JSRuntime, 10);
        await originalArray.FillAsync(3);
        await using IArrayBuffer arrayBuffer = await originalArray.GetBufferAsync();

        // Act
        await using Uint16Array array = await Uint16Array.CreateAsync(JSRuntime, arrayBuffer, 4);

        // Assert
        int sum = 0;
        for (int i = 0; i < 8; i++)
        {
            sum += await array.AtAsync(i);
        }
        _ = sum.Should().Be(12);
    }

    [Test]
    public async Task CreateAsync_WithArrayBufferByteOffsetAndLength_CreatesArrayFromBuffer()
    {
        // Arrange
        await using Uint32Array originalArray = await Uint32Array.CreateAsync(JSRuntime, 10);
        await originalArray.FillAsync(3);
        await using IArrayBuffer arrayBuffer = await originalArray.GetBufferAsync();

        // Act
        await using Uint16Array array = await Uint16Array.CreateAsync(JSRuntime, arrayBuffer, 4, 4);

        // Assert
        int sum = 0;
        for (int i = 0; i < 4; i++)
        {
            sum += await array.AtAsync(i);
        }
        _ = sum.Should().Be(6);
    }

    [Test]
    public async Task CreateAsync_WithTypedArray_CreatesArrayFromTypedArray()
    {
        // Arrange
        await using Uint16Array originalArray = await Uint16Array.CreateAsync(JSRuntime, 2);
        await originalArray.FillAsync(1, 0, 1);
        await originalArray.FillAsync(2, 1, 2);

        // Act
        await using Uint16Array array = await Uint16Array.CreateAsync(JSRuntime, originalArray);

        // Assert
        ushort firstElement = await array.AtAsync(0);
        ushort secondElement = await array.AtAsync(1);
        _ = firstElement.Should().Be(1);
        _ = secondElement.Should().Be(2);
    }

    [Test]
    public async Task GetBufferAsync_GetsBuffer()
    {
        // Arrange
        await using Uint16Array array = await Uint16Array.CreateAsync(JSRuntime, 10);

        // Act
        await using IArrayBuffer buffer = await array.GetBufferAsync();
    }

    [Test]
    public async Task AtAsync_WithPositiveNumber_GetsElementFromStartOfArray()
    {
        // Arrange
        await using Uint16Array array = await Uint16Array.CreateAsync(JSRuntime, 10);
        await array.FillAsync(10);
        await array.FillAsync(20, 5);

        // Act
        ushort secondElement = await array.AtAsync(1);

        // Assert
        _ = secondElement.Should().Be(10);
    }

    [Test]
    public async Task AtAsync_WithNegativeNumber_GetsElementFromEndOfArray()
    {
        // Arrange
        await using Uint16Array array = await Uint16Array.CreateAsync(JSRuntime, 10);
        await array.FillAsync(10);
        await array.FillAsync(20, 5);

        // Act
        ushort lastElement = await array.AtAsync(-1);

        // Assert
        _ = lastElement.Should().Be(20);
    }

    [Test]
    public async Task FillAsync_WithNoArguments_FillsEntireArray()
    {
        // Arrange
        await using Uint16Array array = await Uint16Array.CreateAsync(JSRuntime, 10);

        // Act
        await array.FillAsync(10);

        // Assert
        ushort firstElement = await array.AtAsync(0);
        ushort lastElement = await array.AtAsync(-1);
        _ = firstElement.Should().Be(10);
        _ = lastElement.Should().Be(10);
    }

    [Test]
    public async Task FillAsync_WithStart_FillsArrayFromStartArgument()
    {
        // Arrange
        await using Uint16Array array = await Uint16Array.CreateAsync(JSRuntime, 10);

        // Act
        await array.FillAsync(10, 5);

        // Assert
        ushort firstElement = await array.AtAsync(0);
        ushort lastElement = await array.AtAsync(-1);
        _ = firstElement.Should().Be(0);
        _ = lastElement.Should().Be(10);
    }

    [Test]
    public async Task FillAsync_WithStartAndEnd_FillsArrayFromStartArgumentToEndArgument()
    {
        // Arrange
        await using Uint16Array array = await Uint16Array.CreateAsync(JSRuntime, 10);

        // Act
        await array.FillAsync(10, 1, 3);

        // Assert
        ushort firstElement = await array.AtAsync(0);
        ushort secondElement = await array.AtAsync(1);
        ushort thirdElement = await array.AtAsync(2);
        ushort fourthElement = await array.AtAsync(3);
        _ = firstElement.Should().Be(0);
        _ = secondElement.Should().Be(10);
        _ = thirdElement.Should().Be(10);
        _ = fourthElement.Should().Be(0);
    }

    [Test]
    public async Task GetLengthAsync_GetsLength()
    {
        // Arrange
        await using Uint16Array array = await Uint16Array.CreateAsync(JSRuntime, 10);

        // Act
        long length = await array.GetLengthAsync();

        // Assert
        _ = length.Should().Be(10);
    }
}
