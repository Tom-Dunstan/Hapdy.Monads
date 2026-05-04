// ReSharper disable InconsistentNaming
// ReSharper disable CheckNamespace

namespace Hapdy.Monads.Results.Testing_Validate;

[TestFixture(TestOf = typeof(IResult)
           , TestName = "Validate"
           , Category = "5 - Validate")]
[TestFixture]
public class Validate
{
    private static bool _functionWasCalled;

    private static class Values
    {
        public const           int  TestValue         = 42;
        public static readonly int? TestNullableValue = 42;
        public static readonly int? TestNullValue     = null;
        public const           int  ExpectedValue     = 84;

        public const           string    ErrorMessage             = "Value must be greater than 30";
        public const           string    ErrorMessageNullable     = "Value must be greater than 30";
        public const           string    ExpectedExceptionMessage = "Test exception message";
        public static readonly Exception Exception                = new(ExpectedExceptionMessage);
    }

    [SetUp] public void Setup() { _functionWasCalled = false; }

    private static class Functions
    {
        public static Func<int, IResult<int>> GetFunction()
        {
            return value =>
                   {
                       _functionWasCalled = true;
                       _functionWasCalled = true;
                       return value > 30
                                  ? Success<int>.Create(value * 2)
                                  : Failure<int>.Create(Values.ErrorMessage);
                   };
        }

        public static Func<int, CancellationToken, Task<IResult<int>>> GetFunctionAsync()
        {
            // ReSharper disable once RedundantLambdaParameterType
            // ReSharper disable once UnusedParameter.Local
            return (int value, CancellationToken cancellationToken) =>
                   {
                       _functionWasCalled = true;
                       IResult<int> result = value > 30
                                                 ? Success<int>.Create(value * 2)
                                                 : Failure<int>.Create(Values.ErrorMessage);
                       return Task.FromResult(result);
                   };
        }

        public static Func<int?, IResult<int>> GetNullableFunction()
        {
            return value =>
                   {
                       _functionWasCalled = true;
                       return value == null
                                  ? Failure<int>.Create(Values.ErrorMessageNullable)
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
                                                 ? Failure<int>.Create(Values.ErrorMessageNullable)
                                                 : Success<int>.Create(value.Value * 2);
                       return Task.FromResult(result);
                   };
        }

        public static Func<int, IResult<int>> GetExceptionFunction()
        {
            // ReSharper disable once UnusedParameter.Local
            return value => throw Values.Exception;
        }

        public static Func<int, CancellationToken, Task<IResult<int>>> GetExceptionFunctionAsync()
        {
            // ReSharper disable once RedundantLambdaParameterType
            // ReSharper disable UnusedParameter.Local
            return (int value, CancellationToken cancellationToken) => throw Values.Exception;
            // ReSharper restore UnusedParameter.Local
        }

        public static Func<int?, IResult<int>> GetNullableExceptionFunction()
        {
            // ReSharper disable once UnusedParameter.Local
            return value => throw Values.Exception;
        }

        public static Func<int?, CancellationToken, Task<IResult<int>>> GetNullableExceptionFunctionAsync()
        {
            // ReSharper disable once RedundantLambdaParameterType
            // ReSharper disable UnusedParameter.Local
            return (int? value, CancellationToken cancellationToken) => throw Values.Exception;
            // ReSharper restore UnusedParameter.Local
        }
    }

    private static class Assertions
    {
        public static void AssertSuccessResult(IResult<int> result)
        {
            Assert.That(result, Is.InstanceOf<Success<int>>());
            using (Assert.EnterMultipleScope())
            {
                Assert.That(_functionWasCalled, Is.True);
                var successResult = (Success<int>)result;
                Assert.That(successResult.Value, Is.EqualTo(Values.ExpectedValue));
            }
        }

        public static void AssertFailureResult(IResult<int> result)
        {
            Assert.That(result, Is.InstanceOf<Failure<int>>());
            using (Assert.EnterMultipleScope())
            {
                Assert.That(_functionWasCalled, Is.True);
                var failureResult = (Failure<int>)result;
                Assert.That(failureResult.ErrorMessage, Is.EqualTo(Values.ErrorMessageNullable));
            }
        }

        public static void AssertExceptionFailureResult(IResult<int> result)
        {
            Assert.That(result, Is.InstanceOf<ExceptionFailure<int>>());
            using (Assert.EnterMultipleScope())
            {
                Assert.That(_functionWasCalled, Is.False);
                var exceptionFailureResult = (ExceptionFailure<int>)result;
                Assert.That(exceptionFailureResult.Exception,    Is.EqualTo(Values.Exception));
                Assert.That(exceptionFailureResult.ErrorMessage, Is.EqualTo(Values.ExpectedExceptionMessage));
            }
        }
    }

    [Test]
    public void When_ValueWithFunction_Then_ReturnsMappedValue()
    {
        // Arrange
        var mapFunction = Functions.GetFunction();

        // Act
        var result = IResult.Validate(Values.TestValue, mapFunction);

        // Assert
        Assertions.AssertSuccessResult(result);
    }

    [Test]
    public async Task When_ValueWithAsyncFunction_Then_ReturnsMappedValue()
    {
        // Arrange
        var mapFunction = Functions.GetFunctionAsync();

        // Act
        var result = await IResult.Validate(Values.TestValue
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
        var result = IResult<int>.Validate(Values.TestNullableValue, mapFunction);

        // Assert
        Assertions.AssertSuccessResult(result);
    }

    [Test]
    public async Task When_NullableValueWithAsyncFunction_Then_ReturnsMappedValue()
    {
        var mapFunction = Functions.GetNullableFunctionAsync();

        // Act
        var result = await IResult<int>.Validate(Values.TestNullableValue
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
        var result = IResult<int>.Validate(Values.TestNullValue, mapFunction);

        // Assert
        Assertions.AssertFailureResult(result);
    }

    [Test]
    public async Task When_InvalidNullValueWithAsyncFunction_Then_ReturnsFailedResult()
    {
        // Arrange
        var mapFunction = Functions.GetNullableFunctionAsync();

        // Act
        var result = await IResult<int>.Validate(Values.TestNullValue
                                               , mapFunction
                                               , CancellationToken.None);

        // Assert
        Assertions.AssertFailureResult(result);
    }

    [Test]
    public void When_ValueWithFunctionThrowsException_Then_ReturnsExceptionFailure()
    {
        // Arrange
        var mapFunction = Functions.GetExceptionFunction();

        // Act
        var result = IResult.Validate(Values.TestValue, mapFunction);

        // Assert
        Assertions.AssertExceptionFailureResult(result);
    }

    [Test]
    public async Task When_ValueWithAsyncFunctionThrowsException_Then_ReturnsExceptionFailure()
    {
        // Arrange
        var mapFunction = Functions.GetExceptionFunctionAsync();

        // Act
        var result = await IResult.Validate(Values.TestValue
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
        var result = IResult<int>.Validate(Values.TestValue, mapFunction);

        // Assert
        Assertions.AssertExceptionFailureResult(result);
    }

    [Test]
    public async Task When_NullableValueWithAsyncFunctionThrowsException_Then_ReturnsExceptionFailure()
    {
        // Arrange
        var mapFunction = Functions.GetNullableExceptionFunctionAsync();

        // Act
        var result = await IResult<int>.Validate(Values.TestValue
                                               , mapFunction
                                               , CancellationToken.None);

        // Assert
        Assertions.AssertExceptionFailureResult(result);
    }
}