// ReSharper disable InconsistentNaming
// ReSharper disable CheckNamespace

namespace Hapdy.Monads.Results.Testing_Validate;

[TestFixture(TestOf = typeof(IResult)
    , TestName = "Validate"
    , Category = "5 - Validate")]
[TestFixture]
public class Validate
{

    private static class Assertions
    {
        public static void AssertSuccessResult(IResult<int> result)
        {
            Assert.That(result, Is.InstanceOf<Success<int>>());
            using (Assert.EnterMultipleScope())
            {
                Assert.That(Values.FunctionWasCalled, Is.True);
                var successResult = (Success<int>)result;
                Assert.That(successResult.Value, Is.EqualTo(Values.ExpectedValue));
            }
        }

        public static void AssertFailureResult(IResult<int> result)
        {
            Assert.That(result, Is.InstanceOf<Failure<int>>());
            using (Assert.EnterMultipleScope())
            {
                Assert.That(Values.FunctionWasCalled, Is.True);
                var failureResult = (Failure<int>)result;
                Assert.That(failureResult.ErrorMessage, Is.EqualTo(Errors.MessageNullable));
            }
        }

        public static void AssertExceptionFailureResult(IResult<int> result)
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
    }

    [Test]
    public void When_ValueWithFunction_Then_ReturnsMappedValue()
    {
        // Arrange
        var mapFunction = Functions.GetFunction();

        // Act
        var result = IResult.Validate(Values.Test, mapFunction);

        // Assert
        Assertions.AssertSuccessResult(result);
    }

    [Test]
    public async Task When_ValueWithAsyncFunction_Then_ReturnsMappedValue()
    {
        // Arrange
        var mapFunction = Functions.GetAsyncFunction();

        // Act
        var result = await IResult.Validate(Values.Test
            , mapFunction
            , CancellationToken.None);

        // Assert
        Assertions.AssertSuccessResult(result);
    }

    [Test]
    public void When_NullableValueWithFunction_Then_ReturnsMappedValue()
    {
        // Arrange
        var mapFunction = Functions.GetNullableFunction();

        // Act
        var result = IResult<int>.Validate(Values.TestNullable, mapFunction);

        // Assert
        Assertions.AssertSuccessResult(result);
    }

    [Test]
    public async Task When_NullableValueWithAsyncFunction_Then_ReturnsMappedValue()
    {
        var mapFunction = Functions.GetNullableFunctionAsync();

        // Act
        var result = await IResult<int>.Validate(Values.TestNullable
            , mapFunction
            , CancellationToken.None);

        // Assert
        Assertions.AssertSuccessResult(result);
    }

    [Test]
    public void When_InvalidNullValueWithFunction_Then_ReturnsFailedResult()
    {
        // Arrange
        var mapFunction = Functions.GetNullableFunction();

        // Act
        var result = IResult<int>.Validate(Values.TestNull, mapFunction);

        // Assert
        Assertions.AssertFailureResult(result);
    }

    [Test]
    public async Task When_InvalidNullValueWithAsyncFunction_Then_ReturnsFailedResult()
    {
        // Arrange
        var mapFunction = Functions.GetNullableFunctionAsync();

        // Act
        var result = await IResult<int>.Validate(Values.TestNull
            , mapFunction
            , CancellationToken.None);

        // Assert
        Assertions.AssertFailureResult(result);
    }

    [Test]
    public void When_ValueWithFunctionThrowsException_Then_ReturnsExceptionFailure()
    {
        // Arrange
        var mapFunction = Functions.GetExceptionFunction<int>();

        // Act
        var result = IResult.Validate(Values.Test, mapFunction);

        // Assert
        Assertions.AssertExceptionFailureResult(result);
    }

    [Test]
    public async Task When_ValueWithAsyncFunctionThrowsException_Then_ReturnsExceptionFailure()
    {
        // Arrange
        var mapFunction = Functions.GetExceptionAsyncFunction<int>();

        // Act
        var result = await IResult.Validate(Values.Test
            , mapFunction
            , CancellationToken.None);

        // Assert
        Assertions.AssertExceptionFailureResult(result);
    }

    [Test]
    public void When_NullableValueWithFunctionThrowsException_Then_ReturnsExceptionFailure()
    {
        // Arrange
        var mapFunction = Functions.GetNullableExceptionFunction();

        // Act
        var result = IResult<int>.Validate(Values.Test, mapFunction);

        // Assert
        Assertions.AssertExceptionFailureResult(result);
    }

    [Test]
    public async Task When_NullableValueWithAsyncFunctionThrowsException_Then_ReturnsExceptionFailure()
    {
        // Arrange
        var mapFunction = Functions.GetNullableExceptionFunctionAsync();

        // Act
        var result = await IResult<int>.Validate(Values.Test
            , mapFunction
            , CancellationToken.None);

        // Assert
        Assertions.AssertExceptionFailureResult(result);
    }
}