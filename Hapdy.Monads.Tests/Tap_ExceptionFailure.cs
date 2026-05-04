// ReSharper disable InconsistentNaming
// ReSharper disable CheckNamespace

using Hapdy.Monads.Results.Extensions;

namespace Hapdy.Monads.Results.Testing_Tap;

[TestFixture(TestOf = typeof(Failure<>)
           , TestName = "ExceptionFailure"
           , Category = "3 - Tap")]
public class Tap_ExceptionFailure
{
    private static bool _functionWasCalled;

    private static class Values
    {
        public static readonly Exception ExpectedException = new Exception("Test Exception Message");

        public static int? IntPassedToAction;
    }

    private static class Functions
    {
        public static Action<int> GetAction()
        {
            return value =>
                   {
                       _functionWasCalled       = true;
                       Values.IntPassedToAction = value;
                   };
        }

        public static Func<int, CancellationToken, Task> GetAsyncAction()
        {
            // ReSharper disable once RedundantLambdaParameterType
            // ReSharper disable once UnusedParameter.Local
            return (int value, CancellationToken cancellationToken) =>
                   {
                       _functionWasCalled       = true;
                       Values.IntPassedToAction = value;
                       return Task.CompletedTask;
                   };
        }

        public static Action GetNoParamAction()
        {
            return () =>
                   {
                       _functionWasCalled = true;
                   };
        }

        public static Func<CancellationToken, Task> GetNoParamAsyncAction()
        {
            // ReSharper disable once RedundantLambdaParameterType
            // ReSharper disable once UnusedParameter.Local
            return (CancellationToken cancellationToken) =>
                   {
                       _functionWasCalled = true;
                       return Task.CompletedTask;
                   };
        }
    }

    private static class Assertions
    {
        public static void ExceptionFailure(
            IResult<int> result
          , IResult<int> originalResult)
        {
            Assert.That(result, Is.InstanceOf<ExceptionFailure<int>>());
            using (Assert.EnterMultipleScope())
            {
                Assert.That(result,                   Is.EqualTo(originalResult));
                Assert.That(_functionWasCalled,       Is.False);
                Assert.That(Values.IntPassedToAction, Is.Null);
                Assert.That(result,                   Is.EqualTo(originalResult));
            }
        }
    }

    private IResult<int>       _exceptionFailureResult;
    private Task<IResult<int>> _asyncExceptionFailureResult;


    [SetUp]
    public void SetUp()
    {
        _functionWasCalled           = false;
        Values.IntPassedToAction     = null;
        _exceptionFailureResult      = ExceptionFailure<int>.Create(Values.ExpectedException);
        _asyncExceptionFailureResult = Task.FromResult(_exceptionFailureResult);
    }

    [TearDown] public void TearDown() { _asyncExceptionFailureResult.Dispose(); }

    [Test]
    public void When_SuccessFunctionExpectsValue_Then_DoesNotRunSuccessFunction()
    {
        // Arrange
        var func = Functions.GetAction();

        // Act
        var result = _exceptionFailureResult.Tap(func);

        // Assert
        Assertions.ExceptionFailure(result, _exceptionFailureResult);
    }

    [Test]
    public async Task When_SuccessFunctionExpectsValue_Then_DoesNotRunSuccessFunctionAsync()
    {
        // Arrange
        var func = Functions.GetAsyncAction();

        // Act
        var result = await _exceptionFailureResult.Tap(func, CancellationToken.None);

        // Assert
        Assertions.ExceptionFailure(result, _exceptionFailureResult);
    }

    [Test]
    public async Task When_AsyncSuccessFunctionExpectsValue_Then_DoesNotRunSuccessFunction()
    {
        // Arrange
        var func = Functions.GetAction();

        // Act
        var result = await _asyncExceptionFailureResult.Tap(func);

        // Assert
        Assertions.ExceptionFailure(result, _exceptionFailureResult);
    }

    [Test]
    public async Task When_AsyncSuccessFunctionExpectsValue_Then_DoesNotRunSuccessFunctionAsync()
    {
        // Arrange
        var func = Functions.GetAsyncAction();

        // Act
        var result = await _asyncExceptionFailureResult.Tap(func, CancellationToken.None);

        // Assert
        Assertions.ExceptionFailure(result, _exceptionFailureResult);
    }

    [Test]
    public void When_SuccessFunctionExpectsNoValue_Then_DoesNotRunSuccessFunction()
    {
        // Arrange
        var func = Functions.GetNoParamAction();

        // Act
        var result = _exceptionFailureResult.Tap(func);

        // Assert
        Assertions.ExceptionFailure(result, _exceptionFailureResult);
    }

    [Test]
    public async Task When_SuccessFunctionExpectsNoValue_Then_DoesNotRunSuccessFunctionAsync()
    {
        // Arrange
        var func = Functions.GetNoParamAsyncAction();

        // Act
        var result = await _exceptionFailureResult.Tap(func, CancellationToken.None);

        // Assert
        Assertions.ExceptionFailure(result, _exceptionFailureResult);
    }

    [Test]
    public async Task When_AsyncSuccessFunctionExpectsNoValue_Then_DoesNotRunSuccessFunction()
    {
        // Arrange
        var func = Functions.GetNoParamAction();

        // Act
        var result = await _asyncExceptionFailureResult.Tap(func);

        // Assert
        Assertions.ExceptionFailure(result, _exceptionFailureResult);
    }

    [Test]
    public async Task When_AsyncSuccessFunctionExpectsNoValue_Then_DoesNotRunSuccessFunctionAsync()
    {
        // Arrange
        var func = Functions.GetNoParamAsyncAction();

        // Act
        var result = await _asyncExceptionFailureResult.Tap(func, CancellationToken.None);

        // Assert
        Assertions.ExceptionFailure(result, _exceptionFailureResult);
    }
}