namespace IntegrationTests;

public class Uint32ArrayTest : JSInteropBlazorTest
{
    [Test]
    public async Task CreateAsync_WithNoArguments_Succeeds()
    {
        // Arrange
        AfterRenderAsync = async () =>
        {
            Uint32Array array = await Uint32Array.CreateAsync(EvaluationContext.JSRuntime);
            return array;
        };

        // Act
        await OnAfterRerenderAsync();

        // Assert
        _ = EvaluationContext.Result.Should().BeOfType<Uint32Array>();
    }

    [Test]
    public async Task CreateAsync_WithLength_CreatesArrayWithLength()
    {
        // Arrange
        AfterRenderAsync = async () =>
        {
            await using Uint32Array array = await Uint32Array.CreateAsync(EvaluationContext.JSRuntime, 4);
            long length = await array.GetLengthAsync();
            return length;
        };

        // Act
        await OnAfterRerenderAsync();

        // Assert
        _ = EvaluationContext.Result.Should().BeOfType<long>()
            .Which.Should().Be(4);
    }

    [Test]
    public async Task CreateAsync_WithArrayBuffer_CreatesArrayFromBuffer()
    {
        // Arrange
        AfterRenderAsync = async () =>
        {
            await using Uint16Array originalArray = await Uint16Array.CreateAsync(EvaluationContext.JSRuntime, 4);
            await originalArray.FillAsync(1, 0, 3);
            await using IArrayBuffer arrayBuffer = await originalArray.GetBufferAsync();

            await using Uint32Array array = await Uint32Array.CreateAsync(EvaluationContext.JSRuntime, arrayBuffer);
            uint firstIndex = await array.AtAsync(0);
            uint secondIndex = await array.AtAsync(1);
            return (firstIndex, secondIndex);
        };

        // Act
        await OnAfterRerenderAsync();

        // Assert
        _ = EvaluationContext.Result.Should().BeOfType<(uint, uint)>()
            .Which.Should().Be((ushort.MaxValue + 2, 1));
    }

    [Test]
    public async Task CreateAsync_WithArrayBufferAndByteOffset_CreatesArrayFromBuffer()
    {
        // Arrange
        AfterRenderAsync = async () =>
        {
            await using Uint16Array originalArray = await Uint16Array.CreateAsync(EvaluationContext.JSRuntime, 8);
            await originalArray.FillAsync(1);
            await using IArrayBuffer arrayBuffer = await originalArray.GetBufferAsync();

            await using Uint32Array array = await Uint32Array.CreateAsync(EvaluationContext.JSRuntime, arrayBuffer, 4);
            uint sum = 0;
            for(int i = 0; i < 3; i++)
            {
                sum += await array.AtAsync(i);
            }
            return sum;
        };

        // Act
        await OnAfterRerenderAsync();

        // Assert
        _ = EvaluationContext.Result.Should().BeOfType<uint>()
            .Which.Should().Be((ushort.MaxValue * 3) + (2 * 3));
    }

    [Test]
    public async Task CreateAsync_WithArrayBufferByteOffsetAndLength_CreatesArrayFromBuffer()
    {
        // Arrange
        AfterRenderAsync = async () =>
        {
            await using Uint16Array originalArray = await Uint16Array.CreateAsync(EvaluationContext.JSRuntime, 8);
            await originalArray.FillAsync(1);
            await using IArrayBuffer arrayBuffer = await originalArray.GetBufferAsync();

            await using Uint32Array array = await Uint32Array.CreateAsync(EvaluationContext.JSRuntime, arrayBuffer, 4, 2);
            uint result = await array.AtAsync(0);

            return result;
        };

        // Act
        await OnAfterRerenderAsync();

        // Assert
        _ = EvaluationContext.Result.Should().BeOfType<uint>()
            .Which.Should().Be(ushort.MaxValue + 1 + 1);
    }

    [Test]
    public async Task CreateAsync_WithTypedArray_CreatesArrayFromTypedArray()
    {
        // Arrange
        AfterRenderAsync = async () =>
        {
            await using Uint32Array originalArray = await Uint32Array.CreateAsync(EvaluationContext.JSRuntime, 2);
            await originalArray.FillAsync(1, 0, 1);
            await originalArray.FillAsync(2, 1, 2);

            await using Uint32Array array = await Uint32Array.CreateAsync(EvaluationContext.JSRuntime, originalArray);
            uint firstIndex = await array.AtAsync(0);
            uint secondIndex = await array.AtAsync(1);
            return (firstIndex, secondIndex);
        };

        // Act
        await OnAfterRerenderAsync();

        // Assert
        _ = EvaluationContext.Result.Should().BeOfType<(uint, uint)>()
            .Which.Should().Be((1, 2));
    }

    [Test]
    public async Task GetBufferAsync_GetsBuffer()
    {
        // Arrange
        AfterRenderAsync = async () =>
        {
            await using Uint32Array array = await Uint32Array.CreateAsync(EvaluationContext.JSRuntime, 10);
            await using IArrayBuffer buffer = await array.GetBufferAsync();

            return buffer;
        };

        // Act
        await OnAfterRerenderAsync();

        // Assert
        _ = EvaluationContext.Result.Should().BeAssignableTo<IArrayBuffer>();
    }

    [Test]
    public async Task AtAsync_WithPositiveNumber_GetsElementFromStartOfArray()
    {
        // Arrange
        AfterRenderAsync = async () =>
        {
            await using Uint32Array array = await Uint32Array.CreateAsync(EvaluationContext.JSRuntime, 10);
            await array.FillAsync(10);
            await array.FillAsync(20, 5);

            uint secondElement = await array.AtAsync(1);

            return secondElement;
        };

        // Act
        await OnAfterRerenderAsync();

        // Assert
        _ = EvaluationContext.Result.Should().BeAssignableTo<uint>()
            .Which.Should().Be(10);
    }

    [Test]
    public async Task AtAsync_WithNegativeNumber_GetsElementFromEndOfArray()
    {
        // Arrange
        AfterRenderAsync = async () =>
        {
            await using Uint32Array array = await Uint32Array.CreateAsync(EvaluationContext.JSRuntime, 10);
            await array.FillAsync(10);
            await array.FillAsync(20, 5);

            uint secondElement = await array.AtAsync(-1);

            return secondElement;
        };

        // Act
        await OnAfterRerenderAsync();

        // Assert
        _ = EvaluationContext.Result.Should().BeAssignableTo<uint>()
            .Which.Should().Be(20);
    }

    [Test]
    public async Task FillAsync_WithNoArguments_FillsEntireArray()
    {
        // Arrange
        AfterRenderAsync = async () =>
        {
            await using Uint32Array array = await Uint32Array.CreateAsync(EvaluationContext.JSRuntime, 10);

            await array.FillAsync(10);

            uint firstElement = await array.AtAsync(0);
            uint lastElement = await array.AtAsync(-1);

            return (firstElement, lastElement);
        };

        // Act
        await OnAfterRerenderAsync();

        // Assert
        _ = EvaluationContext.Result.Should().BeAssignableTo<(uint, uint)>()
            .Which.Should().Be((10, 10));
    }

    [Test]
    public async Task FillAsync_WithStart_FillsArrayFromStartArgument()
    {
        // Arrange
        AfterRenderAsync = async () =>
        {
            await using Uint32Array array = await Uint32Array.CreateAsync(EvaluationContext.JSRuntime, 10);

            await array.FillAsync(10, 5);

            uint firstElement = await array.AtAsync(0);
            uint lastElement = await array.AtAsync(-1);

            return (firstElement, lastElement);
        };

        // Act
        await OnAfterRerenderAsync();

        // Assert
        _ = EvaluationContext.Result.Should().BeAssignableTo<(uint, uint)>()
            .Which.Should().Be((0, 10));
    }

    [Test]
    public async Task FillAsync_WithStartAndEnd_FillsArrayFromStartArgumentToEndArgument()
    {
        // Arrange
        AfterRenderAsync = async () =>
        {
            await using Uint32Array array = await Uint32Array.CreateAsync(EvaluationContext.JSRuntime, 10);

            await array.FillAsync(10, 1, 3);

            uint firstElement = await array.AtAsync(0);
            uint secondElement = await array.AtAsync(1);
            uint thirdElement = await array.AtAsync(2);
            uint fourthElement = await array.AtAsync(3);

            return (firstElement, secondElement, thirdElement, fourthElement);
        };

        // Act
        await OnAfterRerenderAsync();

        // Assert
        _ = EvaluationContext.Result.Should().BeAssignableTo<(uint, uint, uint, uint)>()
            .Which.Should().Be((0, 10, 10, 0));
    }

    [Test]
    public async Task GetLengthAsync_GetsLength()
    {
        // Arrange
        AfterRenderAsync = async () =>
        {
            await using Uint32Array array = await Uint32Array.CreateAsync(EvaluationContext.JSRuntime, 10);

            long length = await array.GetLengthAsync();

            return length;
        };

        // Act
        await OnAfterRerenderAsync();

        // Assert
        _ = EvaluationContext.Result.Should().BeAssignableTo<long>()
            .Which.Should().Be(10);
    }
}
