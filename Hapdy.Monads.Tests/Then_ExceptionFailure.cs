// ReSharper disable InconsistentNaming
// ReSharper disable CheckNamespace

using Hapdy.Monads.Results.Extensions;

namespace Hapdy.Monads.Results.Testing_Then;

[TestFixture(TestOf = typeof(Failure<>)
           , TestName = "ExceptionFailure"
           , Category = "2 - Then")]
public class Then_ExceptionFailure
{
    private static bool _functionWasCalled;

    private static class Values
    {
        public const           string    ExpectedStringValue = "Test exception message";
        public static readonly Exception ExpectedException   = new(ExpectedStringValue);

        public static int? IntPassedToFunction;
    }

    private static class Errors
    {
        public const string Message = "Value must be greater than 30";
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
    }

    private static class Assertions
    {
        public static void ExceptionFailure<TReturn>(IResult<TReturn> result)
        {
            Assert.That(result, Is.InstanceOf<ExceptionFailure<TReturn>>());
            using (Assert.EnterMultipleScope())
            {
                Assert.That(_functionWasCalled,         Is.False);
                Assert.That(Values.IntPassedToFunction, Is.Null);
                var exceptionResult = (ExceptionFailure<TReturn>)result;
                Assert.That(exceptionResult.Exception, Is.EqualTo(Values.ExpectedException));
            }
        }
    }

    private IResult<int>       _exceptionFailureResult;
    private Task<IResult<int>> _asyncExceptionFailureResult;


    [SetUp]
    public void SetUp()
    {
        _functionWasCalled           = false;
        Values.IntPassedToFunction   = null;
        _exceptionFailureResult      = ExceptionFailure<int>.Create(Values.ExpectedException);
        _asyncExceptionFailureResult = Task.FromResult(_exceptionFailureResult);
    }

    [TearDown] public void TearDown() { _asyncExceptionFailureResult.Dispose(); }

    [Test]
    public void When_SuccessFunctionExpectsValue_Then_DoesNotRunSuccessFunction()
    {
        // Arrange
        var func = Functions.GetFunction();

        // Act
        var result = _exceptionFailureResult.Then(func);

        // Assert
        Assertions.ExceptionFailure(result);
    }

    [Test]
    public async Task When_SuccessFunctionExpectsValue_Then_DoesNotRunSuccessFunctionAsync()
    {
        // Arrange
        var func = Functions.GetAsyncFunction();

        // Act
        var result = await _exceptionFailureResult.Then(func, CancellationToken.None);

        // Assert
        Assertions.ExceptionFailure(result);
    }

    [Test]
    public async Task When_AsyncSuccessFunctionExpectsValue_Then_DoesNotRunSuccessFunction()
    {
        // Arrange
        var func = Functions.GetFunction();

        // Act
        var result = await _asyncExceptionFailureResult.Then(func);

        // Assert
        Assertions.ExceptionFailure(result);
    }

    [Test]
    public async Task When_AsyncSuccessFunctionExpectsValue_Then_DoesNotRunSuccessFunctionAsync()
    {
        // Arrange
        var func = Functions.GetAsyncFunction();

        // Act
        var result = await _asyncExceptionFailureResult.Then(func, CancellationToken.None);

        // Assert
        Assertions.ExceptionFailure(result);
    }

    [Test]
    public void When_SuccessFunctionExpectsNoValue_Then_DoesNotRunSuccessFunction()
    {
        // Arrange
        var func = Functions.GetNoParamFunction();

        // Act
        var result = _exceptionFailureResult.Then(func);

        // Assert
        Assertions.ExceptionFailure(result);
    }

    [Test]
    public async Task When_SuccessFunctionExpectsNoValue_Then_DoesNotRunSuccessFunctionAsync()
    {
        // Arrange
        var func = Functions.GetNoParamAsyncFunction();

        // Act
        var result = await _exceptionFailureResult.Then(func, CancellationToken.None);

        // Assert
        Assertions.ExceptionFailure(result);
    }

    [Test]
    public async Task When_AsyncSuccessFunctionExpectsNoValue_Then_DoesNotRunSuccessFunction()
    {
        // Arrange
        var func = Functions.GetNoParamFunction();

        // Act
        var result = await _asyncExceptionFailureResult.Then(func);

        // Assert
        Assertions.ExceptionFailure(result);
    }

    [Test]
    public async Task When_AsyncSuccessFunctionExpectsNoValue_Then_DoesNotRunSuccessFunctionAsync()
    {
        // Arrange
        var func = Functions.GetNoParamAsyncFunction();

        // Act
        var result = await _asyncExceptionFailureResult.Then(func, CancellationToken.None);

        // Assert
        Assertions.ExceptionFailure(result);
    }
}