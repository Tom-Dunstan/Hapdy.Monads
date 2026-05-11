// ReSharper disable InconsistentNaming
// ReSharper disable CheckNamespace

namespace Hapdy.Monads.Results.Testing_Map;

[TestFixture(TestOf = typeof(IResult)
    , TestName = "Map"
    , Category = "4 - Map")]
[TestFixture]
public class Map
{

    private static class Assertions
    {
        public static void SuccessResult(IResult<int> result)
        {
            Assert.That(result, Is.InstanceOf<Success<int>>());
            using (Assert.EnterMultipleScope())
            {
                Assert.That(Values.FunctionWasCalled, Is.True);
                var successResult = (Success<int>)result;
                Assert.That(successResult.Value, Is.EqualTo(Values.ExpectedValue));
            }
        }

        public static void FailureResult(IResult<int> result)
        {
            Assert.That(result, Is.InstanceOf<Failure<int>>());
            using (Assert.EnterMultipleScope())
            {
                Assert.That(Values.FunctionWasCalled, Is.True);
                var failureResult = (Failure<int>)result;
                Assert.That(failureResult.ErrorMessage, Is.EqualTo(Errors.MessageNullable));
            }
        }

        public static void ExceptionFailureResult(IResult<int> result)
        {
            Assert.That(result, Is.InstanceOf<ExceptionFailure<int>>());
            using (Assert.EnterMultipleScope())
            {
                Assert.That(Values.FunctionWasCalled, Is.False);
                var exceptionFailureResult = (ExceptionFailure<int>)result;
                Assert.That(exceptionFailureResult.Exception, Is.EqualTo(Errors.ExceptionThrown));
                Assert.That(exceptionFailureResult.ErrorMessage, Is.EqualTo(Errors.ExpectedExceptionMessage));
            }
        }
    }

    [SetUp]
    public void Setup()
    {
        Values.Initialise();
        Values.FunctionWasCalled = false;
    }

    [Test]
    public void When_Value_Then_ReturnMappedSuccessValue()
    {
        // Arrange
        const int testValue = 42;

        // Act
        var result = IResult.Map(testValue);

        // Assert
        Assert.That(result, Is.InstanceOf<Success<int>>());
        using (Assert.EnterMultipleScope())
        {
            var successResult = (Success<int>)result;
            Assert.That(successResult.Value, Is.EqualTo(testValue));
        }
    }

    [Test]
    public void When_ValueWithFunction_Then_ReturnsMappedValue()
    {
        // Arrange
        var func = Functions.GetFunction();

        // Act
        var result = IResult.Map(Values.Test, func);

        // Assert
        Assertions.SuccessResult(result);
    }

    [Test]
    public async Task When_ValueWithAsyncFunction_Then_ReturnsMappedValue()
    {
        // Arrange
        var func = Functions.GetAsyncFunction();

        // Act
        var result = await IResult.Map(Values.Test
            , func
            , CancellationToken.None);

        // Assert
        Assertions.SuccessResult(result);
    }

    [Test]
    public void When_NullableValueWithFunction_Then_ReturnsMappedValue()
    {
        // Arrange
        var func = Functions.GetNullableFunction();

        // Act
        var result = IResult<int>.Map(Values.TestNullable, func);

        // Assert
        Assertions.SuccessResult(result);
    }

    [Test]
    public async Task When_NullableValueWithAsyncFunction_Then_ReturnsMappedValue()
    {
        var func = Functions.GetNullableFunctionAsync();

        // Act
        var result = await IResult<int>.Map(Values.TestNullable
            , func
            , CancellationToken.None);

        // Assert
        Assertions.SuccessResult(result);
    }

    [Test]
    public void When_InvalidNullValueWithFunction_Then_ReturnsFailedResult()
    {
        // Arrange
        var func = Functions.GetNullableFunction();

        // Act
        var result = IResult<int>.Map(Values.TestNull, func);

        // Assert
        Assertions.FailureResult(result);
    }

    [Test]
    public async Task When_InvalidNullValueWithAsyncFunction_Then_ReturnsFailedResult()
    {
        // Arrange
        var func = Functions.GetNullableFunctionAsync();

        // Act
        var result = await IResult<int>.Map(Values.TestNull
            , func
            , CancellationToken.None);

        // Assert
        Assertions.FailureResult(result);
    }

    [Test]
    public void When_ValueWithFunctionThrowsException_Then_ReturnsExceptionFailure()
    {
        // Arrange
        var func = Functions.GetExceptionFunction<int>();

        // Act
        var result = IResult.Map(Values.Test, func);

        // Assert
        Assertions.ExceptionFailureResult(result);
    }

    [Test]
    public async Task When_ValueWithAsyncFunctionThrowsException_Then_ReturnsExceptionFailure()
    {
        // Arrange
        var func = Functions.GetExceptionAsyncFunction<int>();

        // Act
        var result = await IResult.Map(Values.Test
            , func
            , CancellationToken.None);

        // Assert
        Assertions.ExceptionFailureResult(result);
    }

    [Test]
    public void When_NullableValueWithFunctionThrowsException_Then_ReturnsExceptionFailure()
    {
        // Arrange
        var func = Functions.GetNullableExceptionFunction();

        // Act
        var result = IResult<int>.Map(Values.TestNullable, func);

        // Assert
        Assertions.ExceptionFailureResult(result);
    }

    [Test]
    public async Task When_NullableValueWithAsyncFunctionThrowsException_Then_ReturnsExceptionFailure()
    {
        // Arrange
        var func = Functions.GetNullableExceptionFunctionAsync();

        // Act
        var result = await IResult<int>.Map(Values.Test
            , func
            , CancellationToken.None);

        // Assert
        Assertions.ExceptionFailureResult(result);
    }
}