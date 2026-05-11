// ReSharper disable InconsistentNaming
// ReSharper disable CheckNamespace

using Hapdy.Monads.Results.Extensions;

namespace Hapdy.Monads.Results.Testing_Validate;

[TestFixture(TestOf = typeof(IResult)
           , TestName = "Validate"
           , Category = "5 - Validate")]
[TestFixture]
public class Validate
{
    [SetUp]
    public void Setup()
    {
        Values.Initialise();
        Results.SuccessResult             = Success<int>.Create(Values.Test);
        Results.AsyncSuccessResult        = Task.FromResult(Results.SuccessResult);
        Results.InvalidSuccessResult      = Success<int>.Create(Values.Invalid);
        Results.AsyncInvalidSuccessResult = Task.FromResult(Results.InvalidSuccessResult);
    }

    [TearDown]
    public void TearDown()
    {
        Results.AsyncSuccessResult.Dispose();
        Results.AsyncInvalidSuccessResult.Dispose();
    }

    private static class Results
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        public static IResult<int>       SuccessResult;
        public static Task<IResult<int>> AsyncSuccessResult;
        public static IResult<int>       InvalidSuccessResult;
        public static Task<IResult<int>> AsyncInvalidSuccessResult;
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    }


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

        public static void AssertFailureResult(IResult<int> result, string expectedMessage)
        {
            Assert.That(result, Is.InstanceOf<Failure<int>>());
            using (Assert.EnterMultipleScope())
            {
                Assert.That(Values.FunctionWasCalled, Is.True);
                var failureResult = (Failure<int>)result;
                Assert.That(failureResult.ErrorMessage, Is.EqualTo(expectedMessage));
            }
        }

        public static void AssertExceptionFailureResult(IResult<int> result)
        {
            Assert.That(result, Is.InstanceOf<ExceptionFailure<int>>());
            using (Assert.EnterMultipleScope())
            {
                Assert.That(Values.FunctionWasCalled, Is.False);
                var exceptionFailureResult = (ExceptionFailure<int>)result;
                Assert.That(exceptionFailureResult.Exception,    Is.EqualTo(Errors.ExceptionThrown));
                Assert.That(exceptionFailureResult.ErrorMessage, Is.EqualTo(Errors.ExpectedExceptionMessage));
            }
        }
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
        Assertions.AssertFailureResult(result, Errors.MessageNullable);
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
        Assertions.AssertFailureResult(result, Errors.MessageNullable);
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

    /*****/
    [Test]
    public void When_SuccessResultWithFunction_Then_ReturnsMappedValue()
    {
        // Arrange
        var mapFunction = Functions.GetFunction();

        // Act
        var result = Results.SuccessResult.Validate(mapFunction);

        // Assert
        Assertions.AssertSuccessResult(result);
    }

    [Test]
    public async Task When_SuccessResultWithAsyncFunction_Then_ReturnsMappedValue()
    {
        // Arrange
        var mapFunction = Functions.GetAsyncFunction();

        // Act
        var result = await Results.SuccessResult.Validate(mapFunction
                                                        , CancellationToken.None);

        // Assert
        Assertions.AssertSuccessResult(result);
    }

    [Test]
    public async Task When_AsyncSuccessResultWithFunction_Then_ReturnsMappedValue()
    {
        // Arrange
        var mapFunction = Functions.GetFunction();

        // Act
        var result = await Results.AsyncSuccessResult.Validate(mapFunction);

        // Assert
        Assertions.AssertSuccessResult(result);
    }

    [Test]
    public async Task When_AsyncSuccessResultWithAsyncFunction_Then_ReturnsMappedValue()
    {
        // Arrange
        var mapFunction = Functions.GetAsyncFunction();

        // Act
        var result = await Results.AsyncSuccessResult.Validate(mapFunction
                                                             , CancellationToken.None);

        // Assert
        Assertions.AssertSuccessResult(result);
    }

    [Test]
    public void When_InvalidSuccessResultWithFunction_Then_ReturnsFailedResult()
    {
        // Arrange
        var mapFunction = Functions.GetFunction();

        // Act
        var result = Results.InvalidSuccessResult.Validate(mapFunction);

        // Assert
        Assertions.AssertFailureResult(result, Errors.Message);
    }

    [Test]
    public async Task When_InvalidSuccessResultWithAsyncFunction_Then_ReturnsFailedResult()
    {
        // Arrange
        var mapFunction = Functions.GetAsyncFunction();

        // Act
        var result = await Results.InvalidSuccessResult.Validate(mapFunction
                                                               , CancellationToken.None);

        // Assert
        Assertions.AssertFailureResult(result, Errors.Message);
    }

    [Test]
    public async Task When_InvalidAsyncSuccessResultWithFunction_Then_ReturnsFailedResult()
    {
        // Arrange
        var mapFunction = Functions.GetFunction();

        // Act
        var result = await Results.AsyncInvalidSuccessResult.Validate(mapFunction);

        // Assert
        Assertions.AssertFailureResult(result, Errors.Message);
    }

    [Test]
    public async Task When_InvalidAsyncSuccessResultWithAsyncFunction_Then_ReturnsFailedResult()
    {
        // Arrange
        var mapFunction = Functions.GetAsyncFunction();

        // Act
        var result = await Results.AsyncInvalidSuccessResult.Validate(mapFunction
                                                                    , CancellationToken.None);

        // Assert
        Assertions.AssertFailureResult(result, Errors.Message);
    }

    [Test]
    public void When_SuccessResultWithFunctionThrowsException_Then_ReturnsExceptionFailure()
    {
        // Arrange
        var mapFunction = Functions.GetExceptionFunction<int>();

        // Act
        var result = Results.SuccessResult.Validate(mapFunction);

        // Assert
        Assertions.AssertExceptionFailureResult(result);
    }

    [Test]
    public async Task When_SuccessResultWithAsyncFunctionThrowsException_Then_ReturnsExceptionFailure()
    {
        // Arrange
        var mapFunction = Functions.GetExceptionAsyncFunction<int>();

        // Act
        var result = await Results.SuccessResult.Validate(mapFunction
                                                        , CancellationToken.None);

        // Assert
        Assertions.AssertExceptionFailureResult(result);
    }

    [Test]
    public async Task When_AsyncSuccessResultWithFunctionThrowsException_Then_ReturnsExceptionFailure()
    {
        // Arrange
        var mapFunction = Functions.GetExceptionFunction<int>();

        // Act
        var result = await Results.AsyncSuccessResult.Validate(mapFunction);

        // Assert
        Assertions.AssertExceptionFailureResult(result);
    }

    [Test]
    public async Task When_AsyncSuccessResultWithAsyncFunctionThrowsException_Then_ReturnsExceptionFailure()
    {
        // Arrange
        var mapFunction = Functions.GetExceptionAsyncFunction<int>();

        // Act
        var result = await Results.AsyncSuccessResult.Validate(mapFunction
                                                             , CancellationToken.None);

        // Assert
        Assertions.AssertExceptionFailureResult(result);
    }
}