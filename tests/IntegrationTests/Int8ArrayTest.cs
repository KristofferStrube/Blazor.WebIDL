namespace IntegrationTests;

public class Int8ArrayTest(string browserName) : JSInteropBlazorTest(browserName)
{
    [Test]
    public async Task CreateAsync_WithNoArguments_Succeeds()
    {
        // Arrange
        AfterRenderAsync = async () =>
        {
            Int8Array array = await Int8Array.CreateAsync(EvaluationContext.JSRuntime);
            return array;
        };

        // Act
        await OnAfterRerenderAsync();

        // Assert
        _ = EvaluationContext.Result.Should().BeOfType<Int8Array>();
    }

    [Test]
    public async Task CreateAsync_WithLength_CreatesArrayWithLength()
    {
        // Arrange
        AfterRenderAsync = async () =>
        {
            await using Int8Array array = await Int8Array.CreateAsync(EvaluationContext.JSRuntime, 4);
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
            await using Int16Array originalArray = await Int16Array.CreateAsync(EvaluationContext.JSRuntime, 1);
            await originalArray.FillAsync(1);
            await using IArrayBuffer arrayBuffer = await originalArray.GetBufferAsync();

            await using Int8Array array = await Int8Array.CreateAsync(EvaluationContext.JSRuntime, arrayBuffer);
            sbyte firstIndex = await array.AtAsync(0);
            sbyte secondIndex = await array.AtAsync(1);
            return (firstIndex, secondIndex);
        };

        // Act
        await OnAfterRerenderAsync();

        // Assert
        _ = EvaluationContext.Result.Should().BeOfType<(sbyte, sbyte)>()
            .Which.Should().Be((1, 0));
    }

    [Test]
    public async Task CreateAsync_WithArrayBufferAndsbyteOffset_CreatesArrayFromBuffer()
    {
        // Arrange
        AfterRenderAsync = async () =>
        {
            await using Int16Array originalArray = await Int16Array.CreateAsync(EvaluationContext.JSRuntime, 10);
            await originalArray.FillAsync(3);
            await using IArrayBuffer arrayBuffer = await originalArray.GetBufferAsync();

            await using Int8Array array = await Int8Array.CreateAsync(EvaluationContext.JSRuntime, arrayBuffer, 4);
            int sum = 0;
            for(int i = 0; i < 8; i++)
            {
                sum += await array.AtAsync(i);
            }
            return sum;
        };

        // Act
        await OnAfterRerenderAsync();

        // Assert
        _ = EvaluationContext.Result.Should().BeOfType<int>()
            .Which.Should().Be(12);
    }

    [Test]
    public async Task CreateAsync_WithArrayBuffersbyteOffsetAndLength_CreatesArrayFromBuffer()
    {
        // Arrange
        AfterRenderAsync = async () =>
        {
            await using Uint16Array originalArray = await Uint16Array.CreateAsync(EvaluationContext.JSRuntime, 10);
            await originalArray.FillAsync(3);
            await using IArrayBuffer arrayBuffer = await originalArray.GetBufferAsync();

            await using Int8Array array = await Int8Array.CreateAsync(EvaluationContext.JSRuntime, arrayBuffer, 4, 4);
            int sum = 0;
            for (int i = 0; i < 4; i++)
            {
                sum += await array.AtAsync(i);
            }
            return sum;
        };

        // Act
        await OnAfterRerenderAsync();

        // Assert
        _ = EvaluationContext.Result.Should().BeOfType<int>()
            .Which.Should().Be(6);
    }

    [Test]
    public async Task CreateAsync_WithTypedArray_CreatesArrayFromTypedArray()
    {
        // Arrange
        AfterRenderAsync = async () =>
        {
            await using Uint16Array originalArray = await Uint16Array.CreateAsync(EvaluationContext.JSRuntime, 2);
            await originalArray.FillAsync(1, 0, 1);
            await originalArray.FillAsync(2, 1, 2);

            await using Int8Array array = await Int8Array.CreateAsync(EvaluationContext.JSRuntime, originalArray);
            sbyte firstIndex = await array.AtAsync(0);
            sbyte secondIndex = await array.AtAsync(1);
            return (firstIndex, secondIndex);
        };

        // Act
        await OnAfterRerenderAsync();

        // Assert
        _ = EvaluationContext.Result.Should().BeOfType<(sbyte, sbyte)>()
            .Which.Should().Be((1, 2));
    }

    [Test]
    public async Task GetBufferAsync_GetsBuffer()
    {
        // Arrange
        AfterRenderAsync = async () =>
        {
            await using Int8Array array = await Int8Array.CreateAsync(EvaluationContext.JSRuntime, 10);
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
            await using Int8Array array = await Int8Array.CreateAsync(EvaluationContext.JSRuntime, 10);
            await array.FillAsync(10);
            await array.FillAsync(20, 5);

            sbyte secondElement = await array.AtAsync(1);

            return secondElement;
        };

        // Act
        await OnAfterRerenderAsync();

        // Assert
        _ = EvaluationContext.Result.Should().BeAssignableTo<sbyte>()
            .Which.Should().Be(10);
    }

    [Test]
    public async Task AtAsync_WithNegativeNumber_GetsElementFromEndOfArray()
    {
        // Arrange
        AfterRenderAsync = async () =>
        {
            await using Int8Array array = await Int8Array.CreateAsync(EvaluationContext.JSRuntime, 10);
            await array.FillAsync(10);
            await array.FillAsync(20, 5);

            sbyte secondElement = await array.AtAsync(-1);

            return secondElement;
        };

        // Act
        await OnAfterRerenderAsync();

        // Assert
        _ = EvaluationContext.Result.Should().BeAssignableTo<sbyte>()
            .Which.Should().Be(20);
    }

    [Test]
    public async Task FillAsync_WithNoArguments_FillsEntireArray()
    {
        // Arrange
        AfterRenderAsync = async () =>
        {
            await using Int8Array array = await Int8Array.CreateAsync(EvaluationContext.JSRuntime, 10);

            await array.FillAsync(10);

            sbyte firstElement = await array.AtAsync(0);
            sbyte lastElement = await array.AtAsync(-1);

            return (firstElement, lastElement);
        };

        // Act
        await OnAfterRerenderAsync();

        // Assert
        _ = EvaluationContext.Result.Should().BeAssignableTo<(sbyte, sbyte)>()
            .Which.Should().Be((10, 10));
    }

    [Test]
    public async Task FillAsync_WithStart_FillsArrayFromStartArgument()
    {
        // Arrange
        AfterRenderAsync = async () =>
        {
            await using Int8Array array = await Int8Array.CreateAsync(EvaluationContext.JSRuntime, 10);

            await array.FillAsync(10, 5);

            sbyte firstElement = await array.AtAsync(0);
            sbyte lastElement = await array.AtAsync(-1);

            return (firstElement, lastElement);
        };

        // Act
        await OnAfterRerenderAsync();

        // Assert
        _ = EvaluationContext.Result.Should().BeAssignableTo<(sbyte, sbyte)>()
            .Which.Should().Be((0, 10));
    }

    [Test]
    public async Task FillAsync_WithStartAndEnd_FillsArrayFromStartArgumentToEndArgument()
    {
        // Arrange
        AfterRenderAsync = async () =>
        {
            await using Int8Array array = await Int8Array.CreateAsync(EvaluationContext.JSRuntime, 10);

            await array.FillAsync(10, 1, 3);

            sbyte firstElement = await array.AtAsync(0);
            sbyte secondElement = await array.AtAsync(1);
            sbyte thirdElement = await array.AtAsync(2);
            sbyte fourthElement = await array.AtAsync(3);

            return (firstElement, secondElement, thirdElement, fourthElement);
        };

        // Act
        await OnAfterRerenderAsync();

        // Assert
        _ = EvaluationContext.Result.Should().BeAssignableTo<(sbyte, sbyte, sbyte, sbyte)>()
            .Which.Should().Be((0, 10, 10, 0));
    }

    [Test]
    public async Task GetLengthAsync_GetsLength()
    {
        // Arrange
        AfterRenderAsync = async () =>
        {
            await using Int8Array array = await Int8Array.CreateAsync(EvaluationContext.JSRuntime, 10);

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
