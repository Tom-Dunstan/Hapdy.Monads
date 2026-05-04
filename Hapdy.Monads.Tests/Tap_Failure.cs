// ReSharper disable InconsistentNaming
// ReSharper disable CheckNamespace

using Hapdy.Monads.Results.Extensions;

namespace Hapdy.Monads.Results.Testing_Tap;

[TestFixture(TestOf = typeof(Failure<>)
           , TestName = "Failure"
           , Category = "3 - Tap")]
public class Tap_Failure
{
    private static bool _functionWasCalled;

    private static class Values
    {
        public const string ExpectedValue = "Test Failure Message";

        public static int? IntPassedToAction;
    }

    private static class Functions
    {
        public static Action<int> GetAction()
        {
            return value =>
                   {
                       _functionWasCalled         = true;
                       Values.IntPassedToAction = value;
                   };
        }

        public static Func<int, CancellationToken, Task> GetAsyncAction()
        {
            // ReSharper disable once RedundantLambdaParameterType
            // ReSharper disable once UnusedParameter.Local
            return (int value, CancellationToken cancellationToken) =>
                   {
                       _functionWasCalled         = true;
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
        public static void Failure(
            IResult<int> result
          , IResult<int> originalResult)
        {
            Assert.That(result, Is.InstanceOf<Failure<int>>());
            using (Assert.EnterMultipleScope())
            {
                Assert.That(_functionWasCalled,         Is.False);
                Assert.That(Values.IntPassedToAction, Is.Null);
                Assert.That(result,                     Is.EqualTo(originalResult));
            }
        }
    }

    private IResult<int>       _failureResult;
    private Task<IResult<int>> _asyncFailureResult;


    [SetUp]
    public void SetUp()
    {
        _functionWasCalled         = false;
        Values.IntPassedToAction = null;
        _failureResult             = Failure<int>.Create(Values.ExpectedValue);
        _asyncFailureResult        = Task.FromResult(_failureResult);
    }

    [TearDown] public void TearDown() { _asyncFailureResult.Dispose(); }

    [Test]
    public void When_SuccessFunctionExpectsValue_Then_DoesNotRunSuccessFunction()
    {
        // Arrange
        var func = Functions.GetAction();

        // Act
        var result = _failureResult.Tap(func);

        // Assert
        Assertions.Failure(result, _failureResult);
    }

    [Test]
    public async Task When_SuccessFunctionExpectsValue_Then_DoesNotRunSuccessFunctionAsync()
    {
        // Arrange
        var func = Functions.GetAsyncAction();

        // Act
        var result = await _failureResult.Tap(func, CancellationToken.None);

        // Assert
        Assertions.Failure(result, _failureResult);
    }

    [Test]
    public async Task When_AsyncSuccessFunctionExpectsValue_Then_DoesNotRunSuccessFunction()
    {
        // Arrange
        var func = Functions.GetAction();

        // Act
        var result = await _asyncFailureResult.Tap(func);

        // Assert
        Assertions.Failure(result, _failureResult);
    }

    [Test]
    public async Task When_AsyncSuccessFunctionExpectsValue_Then_DoesNotRunSuccessFunctionAsync()
    {
        // Arrange
        var func = Functions.GetAsyncAction();

        // Act
        var result = await _asyncFailureResult.Tap(func, CancellationToken.None);

        // Assert
        Assertions.Failure(result, _failureResult);
    }

    [Test]
    public void When_SuccessFunctionExpectsNoValue_Then_DoesNotRunSuccessFunction()
    {
        // Arrange
        var func = Functions.GetNoParamAction();

        // Act
        var result = _failureResult.Tap(func);

        // Assert
        Assertions.Failure(result, _failureResult);
    }

    [Test]
    public async Task When_SuccessFunctionExpectsNoValue_Then_DoesNotRunSuccessFunctionAsync()
    {
        // Arrange
        var func = Functions.GetNoParamAsyncAction();

        // Act
        var result = await _failureResult.Tap(func, CancellationToken.None);

        // Assert
        Assertions.Failure(result, _failureResult);
    }

    [Test]
    public async Task When_AsyncSuccessFunctionExpectsNoValue_Then_DoesNotRunSuccessFunction()
    {
        // Arrange
        var func = Functions.GetNoParamAction();

        // Act
        var result = await _asyncFailureResult.Tap(func);

        // Assert
        Assertions.Failure(result, _failureResult);
    }

    [Test]
    public async Task When_AsyncSuccessFunctionExpectsNoValue_Then_DoesNotRunSuccessFunctionAsync()
    {
        // Arrange
        var func = Functions.GetNoParamAsyncAction();

        // Act
        var result = await _asyncFailureResult.Tap(func, CancellationToken.None);

        // Assert
        Assertions.Failure(result, _failureResult);
    }
}