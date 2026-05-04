// ReSharper disable InconsistentNaming
// ReSharper disable CheckNamespace

using Hapdy.Monads.Results.Extensions;

namespace Hapdy.Monads.Results.Testing_Then;

[TestFixture(TestOf = typeof(Failure<>)
           , TestName = "Failure"
           , Category = "2 - Then")]
public class Then_Failure
{
    private static bool _functionWasCalled;

    private static class Values
    {
        public const string ExpectedErrorMessage = "Testing failure result.";
        public const string ExpectedStringValue  = "Expected String Value";
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
                       _functionWasCalled = true;
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
                       _functionWasCalled = true;
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
        public static void Failed<TReturn>(IResult<TReturn> result)
        {
            Assert.That(result, Is.InstanceOf<Failure<TReturn>>());
            using (Assert.EnterMultipleScope())
            {
                Assert.That(_functionWasCalled, Is.False);
                var failureResult = (Failure<TReturn>)result;
                Assert.That(failureResult.ErrorMessage, Is.EqualTo(Values.ExpectedErrorMessage));
            }
        }
    }

    private IResult<int>       _failureResult;
    private Task<IResult<int>> _asyncFailureResult;


    [SetUp]
    public void Setup()
    {
        _functionWasCalled  = false;
        _failureResult      = Failure<int>.Create(Values.ExpectedErrorMessage);
        _asyncFailureResult = Task.FromResult(_failureResult);
    }

    [TearDown] public void TearDown() { _asyncFailureResult.Dispose(); }

    [Test]
    public void When_FailureAndSuccessExpectsValue_Then_DoesNotRunSuccessFunction()
    {
        // Arrange
        var func = Functions.GetFunction();

        // Act
        var result = _failureResult.Then(func);

        // Assert
        Assertions.Failed(result);
    }

    [Test]
    public async Task When_FailureAndSuccessExpectsValue_Then_DoesNotRunSuccessFunctionAsync()
    {
        // Arrange
        var func = Functions.GetAsyncFunction();

        // Act
        var result = await _failureResult.Then(func, CancellationToken.None);

        // Assert
        Assertions.Failed(result);
    }

    [Test]
    public async Task When_AsyncFailureAndSuccessExpectsValue_Then_DoesNotRunSuccessFunction()
    {
        // Arrange
        var func = Functions.GetFunction();

        // Act
        var result = await _asyncFailureResult.Then(func);

        // Assert
        Assertions.Failed(result);
    }

    [Test]
    public async Task When_AsyncFailureAndSuccessExpectsValue_Then_DoesNotRunSuccessFunctionAsync()
    {
        // Arrange
        var func = Functions.GetAsyncFunction();

        // Act
        var result = await _asyncFailureResult.Then(func, CancellationToken.None);

        // Assert
        Assertions.Failed(result);
    }

    [Test]
    public void When_FailureAndSuccessExpectsNoParam_Then_DoesNotRunSuccessFunction()
    {
        // Arrange
        var func = Functions.GetNoParamFunction();

        // Act
        var result = _failureResult.Then(func);

        // Assert
        Assertions.Failed(result);
    }

    [Test]
    public async Task When_FailureAndSuccessExpectsNoParam_Then_DoesNotRunSuccessFunctionAsync()
    {
        // Arrange
        var func = Functions.GetNoParamAsyncFunction();

        // Act
        var result = await _failureResult.Then(func, CancellationToken.None);

        // Assert
        Assertions.Failed(result);
    }

    [Test]
    public async Task When_AsyncFailureAndSuccessExpectsNoParam_Then_DoesNotRunSuccessFunction()
    {
        // Arrange
        var func = Functions.GetNoParamFunction();

        // Act
        var result = await _asyncFailureResult.Then(func);

        // Assert
        Assertions.Failed(result);
    }

    [Test]
    public async Task When_AsyncFailureAndSuccessExpectsNoParam_Then_DoesNotRunSuccessFunctionAsync()
    {
        // Arrange
        var func = Functions.GetNoParamAsyncFunction();

        // Act
        var result = await _asyncFailureResult.Then(func, CancellationToken.None);

        // Assert
        Assertions.Failed(result);
    }
}