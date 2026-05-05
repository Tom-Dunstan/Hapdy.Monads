// ReSharper disable InconsistentNaming
// ReSharper disable CheckNamespace

namespace Hapdy.Monads.Results.Testing_Map;

[TestFixture(TestOf = typeof(IResult)
           , TestName = "Map"
           , Category = "4 - Map")]
[TestFixture]
public class Map
{
    private static bool _functionWasCalled;

    private static class Values
    {
        public const           int  Test         = 42;
        public static readonly int? TestNullable = 42;
        public static readonly int? TestNull     = null;
        public const           int  Expected     = 84;
    }

    private static class Errors
    {
        public const           string    Message                  = "Value must be greater than 30";
        public const           string    MessageNullable          = "Value must be greater than 30";
        public const           string    ExpectedExceptionMessage = "Test exception message";
        public static readonly Exception Exception                = new(ExpectedExceptionMessage);
    }

    private static class Functions
    {
        public static Func<int, IResult<int>> GetFunction()
        {
            return value =>
                   {
                       _functionWasCalled = true;
                       return value > 30
                                  ? Success<int>.Create(value * 2)
                                  : Failure<int>.Create(Errors.Message);
                   };
        }

        public static Func<int, CancellationToken, Task<IResult<int>>> GetFunctionAsync()
        {
            // ReSharper disable once UnusedParameter.Local
            return (value, cancellationToken) =>
                   {
                       _functionWasCalled = true;
                       IResult<int> result = value > 30
                                                 ? Success<int>.Create(value * 2)
                                                 : Failure<int>.Create(Errors.Message);
                       return Task.FromResult(result);
                   };
        }

        public static Func<int?, IResult<int>> GetNullableFunction()
        {
            return value =>
                   {
                       _functionWasCalled = true;
                       return value == null
                                  ? Failure<int>.Create(Errors.MessageNullable)
                                  : Success<int>.Create(value.Value * 2);
                   };
        }

        public static Func<int?, CancellationToken, Task<IResult<int>>> GetNullableFunctionAsync()
        {
            // ReSharper disable once RedundantLambdaParameterType
            // ReSharper disable once UnusedParameter.Local
            return (int? value, CancellationToken cancellationToken) =>
                   {
                       _functionWasCalled = true;
                       IResult<int> result = value == null
                                                 ? Failure<int>.Create(Errors.MessageNullable)
                                                 : Success<int>.Create(value.Value * 2);
                       return Task.FromResult(result);
                   };
        }

        public static Func<int, IResult<int>> GetExceptionFunction()
        {
            // ReSharper disable once UnusedParameter.Local
            return value => throw Errors.Exception;
        }

        public static Func<int, CancellationToken, Task<IResult<int>>> GetExceptionFunctionAsync()
        {
            // ReSharper disable UnusedParameter.Local
            return (value, cancellationToken) => throw Errors.Exception;
            // ReSharper restore UnusedParameter.Local
        }

        public static Func<int?, IResult<int>> GetNullableExceptionFunction()
        {
            // ReSharper disable once UnusedParameter.Local
            return value => throw Errors.Exception;
        }

        public static Func<int?, CancellationToken, Task<IResult<int>>> GetNullableExceptionFunctionAsync()
        {
            // ReSharper disable UnusedParameter.Local
            return (value, cancellationToken) => throw Errors.Exception;
            // ReSharper restore UnusedParameter.Local
        }
    }

    private static class Assertions
    {
        public static void SuccessResult(IResult<int> result)
        {
            Assert.That(result, Is.InstanceOf<Success<int>>());
            using (Assert.EnterMultipleScope())
            {
                Assert.That(_functionWasCalled, Is.True);
                var successResult = (Success<int>)result;
                Assert.That(successResult.Value, Is.EqualTo(Values.Expected));
            }
        }

        public static void FailureResult(IResult<int> result)
        {
            Assert.That(result, Is.InstanceOf<Failure<int>>());
            using (Assert.EnterMultipleScope())
            {
                Assert.That(_functionWasCalled, Is.True);
                var failureResult = (Failure<int>)result;
                Assert.That(failureResult.ErrorMessage, Is.EqualTo(Errors.MessageNullable));
            }
        }

        public static void ExceptionFailureResult(IResult<int> result)
        {
            Assert.That(result, Is.InstanceOf<ExceptionFailure<int>>());
            using (Assert.EnterMultipleScope())
            {
                Assert.That(_functionWasCalled, Is.False);
                var exceptionFailureResult = (ExceptionFailure<int>)result;
                Assert.That(exceptionFailureResult.Exception,    Is.EqualTo(Errors.Exception));
                Assert.That(exceptionFailureResult.ErrorMessage, Is.EqualTo(Errors.ExpectedExceptionMessage));
            }
        }
    }

    [SetUp] public void Setup() { _functionWasCalled = false; }

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
        var func = Functions.GetFunctionAsync();

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
        var func = Functions.GetExceptionFunction();

        // Act
        var result = IResult.Map(Values.Test, func);

        // Assert
        Assertions.ExceptionFailureResult(result);
    }

    [Test]
    public async Task When_ValueWithAsyncFunctionThrowsException_Then_ReturnsExceptionFailure()
    {
        // Arrange
        var func = Functions.GetExceptionFunctionAsync();

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
        var result = IResult<int>.Map(Values.Test, func);

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