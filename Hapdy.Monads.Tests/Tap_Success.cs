// ReSharper disable InconsistentNaming
// ReSharper disable CheckNamespace

using Hapdy.Monads.Results.Extensions;

namespace Hapdy.Monads.Results.Testing_Tap;

[TestFixture(TestOf = typeof(Success<>)
           , TestName = "Success"
           , Category = "3 - Tap")]
public class Tap_Success
{
    private static bool _functionWasCalled;

    private static class Values
    {
        public const           int    Test                = 42;
        public static readonly int?   ExpectedValue       = 42;

        public static int? IntPassedToFunction;
    }

    private static class Errors
    {
        public const           string    ExpectedExceptionMessage = "Test exception message";
        public static readonly Exception ExceptionThrown          = new(ExpectedExceptionMessage);
    }

    private static class Functions
    {
        public static Action<int> GetAction()
        {
            return value =>
                   {
                       _functionWasCalled         = true;
                       Values.IntPassedToFunction = value;
                   };
        }

        public static Func<int, CancellationToken, Task> GetAsyncAction()
        {
            // ReSharper disable once RedundantLambdaParameterType
            // ReSharper disable once UnusedParameter.Local
            return (int value, CancellationToken cancellationToken) =>
                   {
                       _functionWasCalled         = true;
                       Values.IntPassedToFunction = value;
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

        public static Action<int> GetExceptionAction()
        {
            // ReSharper disable UnusedParameter.Local
            return value => throw Errors.ExceptionThrown;
            // ReSharper restore UnusedParameter.Local
        }

        public static Func<int, CancellationToken, Task> GetExceptionAsyncAction()
        {
            // ReSharper disable UnusedParameter.Local
            return (value, cancellationToken) => throw Errors.ExceptionThrown;
            // ReSharper restore UnusedParameter.Local
        }

        public static Action GetNoParamExceptionAction() { return () => throw Errors.ExceptionThrown; }

        public static Func<CancellationToken, Task> GetNoParamExceptionAsyncAction()
        {
            // ReSharper disable UnusedParameter.Local
            return cancellationToken => throw Errors.ExceptionThrown;
            // ReSharper restore UnusedParameter.Local
        }
    }

    private static class Assertions
    {
        public static void Successful(
            IResult<int> result
          , IResult<int> originalResult
          , int?         valuePassed
          , int?         expectedValuePassedToFunction)
        {
            Assert.That(result, Is.InstanceOf<Success<int>>());
            using (Assert.EnterMultipleScope())
            {
                Assert.That(_functionWasCalled, Is.True);
                Assert.That(valuePassed,        Is.EqualTo(expectedValuePassedToFunction));
                Assert.That(result,             Is.EqualTo(originalResult));
            }
        }

        public static void ExceptionThrown(IResult<int> result)
        {
            Assert.That(result, Is.InstanceOf<ExceptionFailure<int>>());
            using (Assert.EnterMultipleScope())
            {
                var exceptionFailure = (ExceptionFailure<int>)result;
                Assert.That(Values.IntPassedToFunction,         Is.Null);
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
        _successResult                = Success<int>.Create(Values.Test);
        _asyncSuccessResult           = Task.FromResult(_successResult);
    }

    [TearDown] public void TearDown() { _asyncSuccessResult.Dispose(); }

    [Test]
    public void When_SuccessFunctionExpectsValue_Then_RunsSuccessFunction()
    {
        // Arrange
        var func = Functions.GetAction();

        // Act
        var result = _successResult.Tap(func);

        // Assert
        Assertions.Successful(result
                                      , _successResult
                                      , Values.IntPassedToFunction
                                      , Values.ExpectedValue);
    }

    [Test]
    public async Task When_SuccessFunctionExpectsValue_Then_RunsSuccessFunctionAsync()
    {
        // Arrange
        var func = Functions.GetAsyncAction();

        // Act
        var result = await _successResult.Tap(func, CancellationToken.None);

        // Assert
        Assertions.Successful(result
                                      , _successResult
                                      , Values.IntPassedToFunction
                                      , Values.ExpectedValue);
    }

    [Test]
    public async Task When_AsyncSuccessFunctionExpectsValue_Then_RunsSuccessFunction()
    {
        // Arrange
        var func = Functions.GetAction();

        // Act
        var result = await _asyncSuccessResult.Tap(func);

        // Assert
        Assertions.Successful(result
                                      , _successResult
                                      , Values.IntPassedToFunction
                                      , Values.ExpectedValue);
    }

    [Test]
    public async Task When_AsyncSuccessFunctionExpectsValue_Then_RunsSuccessFunctionAsync()
    {
        // Arrange
        var func = Functions.GetAsyncAction();

        // Act
        var result = await _asyncSuccessResult.Tap(func, CancellationToken.None);

        // Assert
        Assertions.Successful(result
                                      , _successResult
                                      , Values.IntPassedToFunction
                                      , Values.ExpectedValue);
    }

    [Test]
    public void When_SuccessFunctionExpectsNoValueAndReturnsDifferentType_Then_RunsSuccessFunction()
    {
        // Arrange
        var func = Functions.GetNoParamAction();

        // Act
        var result = _successResult.Tap(func);

        // Assert
        Assertions.Successful(result
                                      , _successResult
                                      , Values.IntPassedToFunction
                                      , null);
    }

    [Test]
    public async Task When_SuccessFunctionExpectsNoValue_Then_RunsSuccessFunctionAsync()
    {
        // Arrange
        var func = Functions.GetNoParamAsyncAction();

        // Act
        var result = await _successResult.Tap(func, CancellationToken.None);

        // Assert
        Assertions.Successful(result
                                      , _successResult
                                      , Values.IntPassedToFunction
                                      , null);
    }

    [Test]
    public async Task When_AsyncSuccessFunctionExpectsNoValue_Then_RunsSuccessFunction()
    {
        // Arrange
        var func = Functions.GetNoParamAction();

        // Act
        var result = await _asyncSuccessResult.Tap(func);

        // Assert
        Assertions.Successful(result
                                      , _successResult
                                      , Values.IntPassedToFunction
                                      , null);
    }

    [Test]
    public async Task When_AsyncSuccessFunctionExpectsNoValue_Then_RunsSuccessFunctionAsync()
    {
        // Arrange
        var func = Functions.GetNoParamAsyncAction();

        // Act
        var result = await _asyncSuccessResult.Tap(func, CancellationToken.None);

        // Assert
        Assertions.Successful(result
                                      , _successResult
                                      , Values.IntPassedToFunction
                                      , null);
    }

    [Test]
    public void When_SuccessFunctionThrowsException_Then_ReturnsExceptionFailure()
    {
        // Arrange
        var func = Functions.GetExceptionAction();

        //Act
        var result = _successResult.Tap(func);

        // Assert
        Assertions.ExceptionThrown(result);
    }

    [Test]
    public async Task When_AsyncSuccessFunctionThrowsException_Then_ReturnsExceptionFailure()
    {
        // Arrange
        var func = Functions.GetExceptionAction();

        //Act
        var result = await _asyncSuccessResult.Tap(func);

        // Assert
        Assertions.ExceptionThrown(result);
    }

    [Test]
    public async Task When_AsyncSuccessFunctionThrowsException_Then_ReturnsExceptionFailureAsync()
    {
        // Arrange
        var func = Functions.GetExceptionAsyncAction();

        //Act
        var result =await  _asyncSuccessResult.Tap(func, CancellationToken.None);

        // Assert
        Assertions.ExceptionThrown(result);
    }

    [Test]
    public async Task When_SuccessFunctionThrowsException_Then_ReturnsExceptionFailureAsync()
    {
        // Arrange
        var func = Functions.GetExceptionAsyncAction();

        //Act
        var result =await  _successResult.Tap(func, CancellationToken.None);

        // Assert
        Assertions.ExceptionThrown(result);
    }

    [Test]
    public void When_SuccessFunctionExpectsNoValueThrowsException_Then_ReturnsExceptionFailure()
    {
        // Arrange
        var func = Functions.GetNoParamExceptionAction();

        //Act
        var result =  _successResult.Tap(func);

        // Assert
        Assertions.ExceptionThrown(result);
    }

    [Test]
    public async Task When_SuccessFunctionExpectsNoValueThrowsException_Then_ReturnsExceptionFailureAsync()
    {
        // Arrange
        var func = Functions.GetNoParamExceptionAsyncAction();

        //Act
        var result = await _successResult.Tap(func, CancellationToken.None);

        // Assert
        Assertions.ExceptionThrown(result);
    }

    [Test]
    public async Task When_AsyncSuccessFunctionExpectsNoValueThrowsException_Then_ReturnsExceptionFailure()
    {
        // Arrange
        var func = Functions.GetNoParamExceptionAction();

        //Act
        var result = await _asyncSuccessResult.Tap(func);

        // Assert
        Assertions.ExceptionThrown(result);
    }

    [Test]
    public async Task When_AsyncSuccessFunctionExpectsNoValueThrowsException_Then_ReturnsExceptionFailureAsync()
    {
        // Arrange
        var func = Functions.GetNoParamExceptionAsyncAction();

        //Act
        var result = await _asyncSuccessResult.Tap(func, CancellationToken.None);

        // Assert
        Assertions.ExceptionThrown(result);
    }
}