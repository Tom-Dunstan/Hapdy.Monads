// ReSharper disable InconsistentNaming
// ReSharper disable CheckNamespace

using Hapdy.Monads.Results.Extensions;

namespace Hapdy.Monads.Results.Testing_Then;

[TestFixture(TestOf = typeof(Success<>)
           , TestName = "Success"
           , Category = "2 - Then")]
public class Then_Success
{
    private static bool _functionWasCalled;

    private static class Values
    {
        public const int    Test                = 42;
        public const int    ExpectedValue       = 84;
        public const string ExpectedStringValue = "Expected String Value";

        public static int? IntPassedToFunction;
        public static int? StringPassedToFunction;
    }

    private static class Errors
    {
        public const           string    Message                  = "Value must be greater than 30";
        public const           string    ExpectedExceptionMessage = "Test exception message";
        public static readonly Exception ExceptionThrown          = new(ExpectedExceptionMessage);
    }

    private static class Functions
    {
        public static Func<int, IResult<int>> GetFunction()
        {
            return value =>
                   {
                       _functionWasCalled         = true;
                       Values.IntPassedToFunction = value;
                       IResult<int> result = value > 30
                                                 ? Success<int>.Create(value * 2)
                                                 : Failure<int>.Create(Errors.Message);
                       return result;
                   };
        }

        public static Func<int, CancellationToken, Task<IResult<int>>> GetAsyncFunction()
        {
            // ReSharper disable once RedundantLambdaParameterType
            // ReSharper disable once UnusedParameter.Local
            return (int value, CancellationToken cancellationToken) =>
                   {
                       _functionWasCalled         = true;
                       Values.IntPassedToFunction = value;
                       IResult<int> result = value > 30
                                                 ? Success<int>.Create(value * 2)
                                                 : Failure<int>.Create(Errors.Message);
                       return Task.FromResult(result);
                   };
        }

        public static Func<IResult<string>> GetNoParamFunction()
        {
            return () =>
                   {
                       _functionWasCalled = true;
                       IResult<string> result = Success<string>.Create(Values.ExpectedStringValue);
                       return result;
                   };
        }

        public static Func<CancellationToken, Task<IResult<string>>> GetNoParamAsyncFunction()
        {
            // ReSharper disable once RedundantLambdaParameterType
            // ReSharper disable once UnusedParameter.Local
            return (CancellationToken cancellationToken) =>
                   {
                       _functionWasCalled = true;
                       IResult<string> result = Success<string>.Create(Values.ExpectedStringValue);
                       return Task.FromResult(result);
                   };
        }

        public static Func<int, IResult<string>> GetExceptionFunction()
        {
            // ReSharper disable UnusedParameter.Local
            return value => throw Errors.ExceptionThrown;
            // ReSharper restore UnusedParameter.Local
        }

        public static Func<int, CancellationToken, Task<IResult<string>>> GetExceptionAsyncFunction()
        {
            // ReSharper disable UnusedParameter.Local
            return (value, cancellationToken) => throw Errors.ExceptionThrown;
            // ReSharper restore UnusedParameter.Local
        }

        public static Func<IResult<string>> GetNoParamExceptionFunction() { return () => throw Errors.ExceptionThrown; }

        public static Func<CancellationToken, Task<IResult<string>>> GetNoParamExceptionAsyncFunction()
        {
            // ReSharper disable UnusedParameter.Local
            return cancellationToken => throw Errors.ExceptionThrown;
            // ReSharper restore UnusedParameter.Local
        }
    }

    private static class Assertions
    {
        public static void Successful<T, TReturn>(
            IResult<TReturn> result
          , T?               valuePassed
          , T?               expectedValuePassedToFunction
          , TReturn          expectedResultValue)
        {
            Assert.That(result, Is.InstanceOf<Success<TReturn>>());
            using (Assert.EnterMultipleScope())
            {
                Assert.That(_functionWasCalled, Is.True);
                Assert.That(valuePassed,        Is.EqualTo(expectedValuePassedToFunction));
                var successResult = (Success<TReturn>)result;
                Assert.That(successResult.Value, Is.EqualTo(expectedResultValue));
            }
        }

        public static void ExceptionThrown<TReturn>(IResult<TReturn> result)
        {
            Assert.That(result, Is.InstanceOf<ExceptionFailure<TReturn>>());
            using (Assert.EnterMultipleScope())
            {
                var exceptionFailure = (ExceptionFailure<TReturn>)result;
                Assert.That(exceptionFailure.Exception,         Is.Not.Null);
                Assert.That(exceptionFailure.Exception,         Is.EqualTo(Errors.ExceptionThrown));
                Assert.That(exceptionFailure.Exception.Message, Is.EqualTo(Errors.ExpectedExceptionMessage));
            }
        }
    }

    private IResult<int>       _successResult;
    private Task<IResult<int>> _asyncSuccessResult;


    [SetUp]
    public void SetUp()
    {
        _functionWasCalled            = false;
        Values.IntPassedToFunction    = null;
        Values.StringPassedToFunction = null;
        _successResult                = Success<int>.Create(Values.Test);
        _asyncSuccessResult           = Task.FromResult(_successResult);
    }

    [TearDown] public void TearDown() { _asyncSuccessResult.Dispose(); }

    [Test]
    public void When_SuccessFunctionExpectsValue_Then_RunsSuccessFunction()
    {
        // Arrange
        var func = Functions.GetFunction();

        // Act
        var result = _successResult.Then(func);

        // Assert
        Assertions.Successful(result
                            , Values.Test
                            , Values.IntPassedToFunction
                            , Values.ExpectedValue);
    }

    [Test]
    public async Task When_SuccessFunctionExpectsValue_Then_RunsSuccessFunctionAsync()
    {
        // Arrange
        var func = Functions.GetAsyncFunction();

        // Act
        var resultAfterBind = await _successResult.Then(func, CancellationToken.None);

        // Assert
        Assertions.Successful(resultAfterBind
                            , Values.Test
                            , Values.IntPassedToFunction
                            , Values.ExpectedValue);
    }

    [Test]
    public async Task When_AsyncSuccessFunctionExpectsValue_Then_RunsSuccessFunction()
    {
        // Arrange
        var func = Functions.GetFunction();

        // Act
        var result = await _asyncSuccessResult.Then(func);

        // Assert
        Assertions.Successful(result
                            , Values.Test
                            , Values.IntPassedToFunction
                            , Values.ExpectedValue);
    }

    [Test]
    public async Task When_AsyncSuccessFunctionExpectsValue_Then_RunsSuccessFunctionAsync()
    {
        // Arrange
        var func = Functions.GetAsyncFunction();

        // Act
        var result = await _asyncSuccessResult.Then(func, CancellationToken.None);

        // Assert
        Assertions.Successful(result
                            , Values.Test
                            , Values.IntPassedToFunction
                            , Values.ExpectedValue);
    }

    [Test]
    public void When_SuccessFunctionExpectsNoValue_Then_RunsSuccessFunction()
    {
        // Arrange
        var func = Functions.GetNoParamFunction();

        // Act
        var result = _successResult.Then(func);

        // Assert
        Assertions.Successful(result
                            , null
                            , Values.StringPassedToFunction
                            , Values.ExpectedStringValue);
    }

    [Test]
    public async Task When_SuccessFunctionExpectsNoValue_Then_RunsSuccessFunctionAsync()
    {
        // Arrange
        var func = Functions.GetNoParamAsyncFunction();

        // Act
        var result = await _successResult.Then(func, CancellationToken.None);

        // Assert
        Assertions.Successful(result
                            , null
                            , Values.StringPassedToFunction
                            , Values.ExpectedStringValue);
    }

    [Test]
    public async Task When_AsyncSuccessFunctionExpectsNoValue_Then_RunsSuccessFunction()
    {
        // Arrange
        var func = Functions.GetNoParamFunction();

        // Act
        var result = await _asyncSuccessResult.Then(func);

        // Assert
        Assertions.Successful(result
                            , null
                            , Values.StringPassedToFunction
                            , Values.ExpectedStringValue);
    }

    [Test]
    public async Task When_AsyncSuccessFunctionExpectsNoValue_Then_RunsSuccessFunctionAsync()
    {
        // Arrange
        var func = Functions.GetNoParamAsyncFunction();

        // Act
        var result = await _asyncSuccessResult.Then(func, CancellationToken.None);

        // Assert
        Assertions.Successful(result
                            , null
                            , Values.StringPassedToFunction
                            , Values.ExpectedStringValue);
    }

    [Test]
    public void When_SuccessFunctionThrowsException_Then_ReturnsExceptionFailure()
    {
        // Arrange
        var func = Functions.GetExceptionFunction();

        //Act
        var result = _successResult.Then(func);

        // Assert
        Assertions.ExceptionThrown(result);
    }

    [Test]
    public async Task When_AsyncSuccessFunctionThrowsException_Then_ReturnsExceptionFailure()
    {
        // Arrange
        var func = Functions.GetExceptionFunction();

        //Act
        var result = await _asyncSuccessResult.Then(func);

        // Assert
        Assertions.ExceptionThrown(result);
    }

    [Test]
    public async Task When_SuccessFunctionThrowsException_Then_ReturnsExceptionFailureAsync()
    {
        // Arrange
        var func = Functions.GetExceptionAsyncFunction();

        //Act
        var result = await _successResult.Then(func, CancellationToken.None);

        // Assert
        Assertions.ExceptionThrown(result);
    }

    [Test]
    public async Task When_AsyncSuccessFunctionThrowsException_Then_ReturnsExceptionFailureAsync()
    {
        // Arrange
        var func = Functions.GetExceptionAsyncFunction();

        //Act
        var result = await _asyncSuccessResult.Then(func, CancellationToken.None);

        // Assert
        Assertions.ExceptionThrown(result);
    }

    [Test]
    public void When_SuccessFunctionExpectsNoValueThrowsException_Then_ReturnsExceptionFailure()
    {
        // Arrange
        var func = Functions.GetNoParamExceptionFunction();

        //Act
        var result = _successResult.Then(func);

        // Assert
        Assertions.ExceptionThrown(result);
    }

    [Test]
    public async Task When_SuccessFunctionExpectsNoValueThrowsException_Then_ReturnsExceptionFailureAsync()
    {
        // Arrange
        var func = Functions.GetNoParamExceptionAsyncFunction();

        //Act
        var result = await _successResult.Then(func, CancellationToken.None);

        // Assert
        Assertions.ExceptionThrown(result);
    }

    [Test]
    public async Task When_AsyncSuccessFunctionExpectsNoValueThrowsException_Then_ReturnsExceptionFailure()
    {
        // Arrange
        var func = Functions.GetNoParamExceptionFunction();

        //Act
        var result = await _asyncSuccessResult.Then(func);

        // Assert
        Assertions.ExceptionThrown(result);
    }

    [Test]
    public async Task When_AsyncSuccessFunctionExpectsNoValueThrowsException_Then_ReturnsExceptionFailureAsync()
    {
        // Arrange
        var func = Functions.GetNoParamExceptionAsyncFunction();

        //Act
        var result = await _asyncSuccessResult.Then(func, CancellationToken.None);

        // Assert
        Assertions.ExceptionThrown(result);
    }
}