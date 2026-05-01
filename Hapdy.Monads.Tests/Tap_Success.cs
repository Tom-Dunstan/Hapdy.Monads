// ReSharper disable InconsistentNaming
// ReSharper disable CheckNamespace

using Hapdy.Monads.Results.Extensions;

namespace Hapdy.Monads.Results.Testing_Tap;

[TestFixture(TestOf = typeof(Success<>)
           , TestName = "Success"
           , Category = "3 - Tap")]
public class Tap_Success
{
    [SetUp] public void Setup() { }

    [Test]
    public void When_SuccessFunctionExpectsValue_Then_RunsSuccessFunction()
    {
        // Arrange
        const int testValue                    = 42;
        var       startingResult               = Success<int>.Create(42);
        int?      valuePassedToSuccessFunction = null;

        // ReSharper disable once MoveLocalFunctionAfterJumpStatement
        void SuccessFunction(int value)
        {
            valuePassedToSuccessFunction = value;
        }

        // Act
        var resultAfterBind = startingResult.Tap(SuccessFunction);

        // Assert
        Assert.That(resultAfterBind, Is.InstanceOf<Success<int>>());
        using (Assert.EnterMultipleScope())
        {
            Assert.That(valuePassedToSuccessFunction, Is.EqualTo(testValue));
            Assert.That(resultAfterBind,              Is.EqualTo(startingResult));
        }
    }

    [Test]
    public async Task When_SuccessFunctionExpectsValue_Then_RunsSuccessFunctionAsync()
    {
        // Arrange
        var  startingResult               = Success<int>.Create(42);
        int? valuePassedToSuccessFunction = null;

        // ReSharper disable once MoveLocalFunctionAfterJumpStatement
        Task SuccessFunction(int value, CancellationToken cancellationToken)
        {
            valuePassedToSuccessFunction = value;
            return Task.CompletedTask;
        }

        // Act
        var resultAfterBind = await startingResult.Tap(SuccessFunction, CancellationToken.None);

        // Assert
        Assert.That(resultAfterBind, Is.InstanceOf<Success<int>>());
        using (Assert.EnterMultipleScope())
        {
            Assert.That(valuePassedToSuccessFunction, Is.Not.Null);
            Assert.That(valuePassedToSuccessFunction, Is.EqualTo(42));
            Assert.That(resultAfterBind,              Is.EqualTo(startingResult));
        }
    }

    [Test]
    public async Task When_AsyncSuccessFunctionExpectsValue_Then_RunsSuccessFunction()
    {
        // Arrange
        IResult<int> syncResult                   = Success<int>.Create(42);
        var          startingResult               = Task.FromResult(syncResult);
        int?         valuePassedToSuccessFunction = null;

        // ReSharper disable once MoveLocalFunctionAfterJumpStatement
        void SuccessFunction(int value)
        {
            valuePassedToSuccessFunction = value;
        }

        // Act
        var resultAfterBind = await startingResult.Tap(SuccessFunction);

        // Assert
        Assert.That(resultAfterBind, Is.InstanceOf<Success<int>>());
        using (Assert.EnterMultipleScope())
        {
            Assert.That(valuePassedToSuccessFunction, Is.Not.Null);
            Assert.That(valuePassedToSuccessFunction, Is.EqualTo(42));
            Assert.That(resultAfterBind,              Is.EqualTo(syncResult));
        }
    }

    [Test]
    public async Task When_AsyncSuccessFunctionExpectsValue_Then_RunsSuccessFunctionAsync()
    {
        // Arrange
        IResult<int> syncResult                   = Success<int>.Create(42);
        var       startingResult               = Task.FromResult(syncResult);
        int?      valuePassedToSuccessFunction = null;

        // ReSharper disable once MoveLocalFunctionAfterJumpStatement
        Task SuccessFunction(int value, CancellationToken cancellationToken)
        {
            valuePassedToSuccessFunction = value;
            return Task.CompletedTask;
        }

        // Act
        var resultAfterBind = await startingResult.Tap(SuccessFunction, CancellationToken.None);

        // Assert
        Assert.That(resultAfterBind, Is.InstanceOf<Success<int>>());
        using (Assert.EnterMultipleScope())
        {
            Assert.That(valuePassedToSuccessFunction, Is.Not.Null);
            Assert.That(valuePassedToSuccessFunction, Is.EqualTo(42));
            Assert.That(resultAfterBind,              Is.EqualTo(syncResult));
        }
    }

    [Test]
    public void When_SuccessFunctionExpectsNoValueAndReturnsDifferentType_Then_RunsSuccessFunction()
    {
        // Arrange
        const int    testValue         = 42;
        var          functionWasCalled = false;
        Success<int> startingResult    = new(testValue);

        // ReSharper disable once MoveLocalFunctionAfterJumpStatement
        void SuccessFunction() { functionWasCalled = true; }

        // Act
        var resultAfterBind = startingResult.Tap(SuccessFunction);

        // Assert
        Assert.That(resultAfterBind, Is.InstanceOf<Success<int>>());
        using (Assert.EnterMultipleScope())
        {
            Assert.That(functionWasCalled, Is.True);
            Assert.That(resultAfterBind,   Is.EqualTo(startingResult));
        }
    }

    [Test]
    public async Task When_SuccessFunctionExpectsNoValue_Then_RunsSuccessFunctionAsync()
    {
        // Arrange
        const int    testValue         = 42;
        var          functionWasCalled = false;
        Success<int> startingResult    = new (testValue);

        // ReSharper disable once MoveLocalFunctionAfterJumpStatement
        Task SuccessFunction(CancellationToken cancellationToken)
        {
            functionWasCalled = true;
            return Task.CompletedTask;
        }

        // Act
        var resultAfterBind = await startingResult.Tap(SuccessFunction, CancellationToken.None);

        // Assert
        Assert.That(resultAfterBind, Is.InstanceOf<Success<int>>());
        using (Assert.EnterMultipleScope())
        {
            Assert.That(functionWasCalled, Is.True);
            Assert.That(resultAfterBind,   Is.EqualTo(startingResult));
        }
    }

    [Test]
    public async Task When_AsyncSuccessFunctionExpectsNoValue_Then_RunsSuccessFunction()
    {
        // Arrange
        const int    testValue         = 42;
        var          functionWasCalled = false;
        IResult<int> syncResult        = Success<int>.Create(testValue);
        var          startingResult    = Task.FromResult(syncResult);

        // ReSharper disable once MoveLocalFunctionAfterJumpStatement
        void SuccessFunction() { functionWasCalled = true; }

        // Act
        var resultAfterBind = await startingResult.Tap(SuccessFunction);

        // Assert
        Assert.That(resultAfterBind, Is.InstanceOf<Success<int>>());
        using (Assert.EnterMultipleScope())
        {
            Assert.That(functionWasCalled, Is.True);
            Assert.That(resultAfterBind,   Is.EqualTo(syncResult));
        }
    }

    [Test]
    public async Task When_AsyncSuccessFunctionExpectsNoValue_Then_RunsSuccessFunctionAsync()
    {
        // Arrange
        const int    testValue         = 42;
        var          functionWasCalled = false;
        IResult<int> syncResult        = Success<int>.Create(testValue);
        var          startingResult    = Task.FromResult(syncResult);

        // ReSharper disable once MoveLocalFunctionAfterJumpStatement
        Task SuccessFunction(CancellationToken cancellationToken)
        {
            functionWasCalled = true;
            return Task.CompletedTask;
        }

        // Act
        var resultAfterBind = await startingResult.Tap(SuccessFunction, CancellationToken.None);

        // Assert
        Assert.That(resultAfterBind, Is.InstanceOf<Success<int>>());
        using (Assert.EnterMultipleScope())
        {
            Assert.That(functionWasCalled, Is.True);
            Assert.That(resultAfterBind,   Is.EqualTo(syncResult));
        }
    }

    [Test]
    public void When_SuccessFunctionThrowsException_Then_ReturnsExceptionFailure()
    {
        // Arrange
        var startingResult = Success<int>.Create(42);
        var exception      = new Exception("Test Exception");

        // ReSharper disable once MoveLocalFunctionAfterJumpStatement
        IResult<int> SuccessFunction(int value) => throw exception;

        //Act
        var resultAfterBind = startingResult.Bind(SuccessFunction);

        // Assert
        Assert.That(resultAfterBind, Is.InstanceOf<ExceptionFailure<int>>());
        using (Assert.EnterMultipleScope())
        {
            var exceptionFailure = (ExceptionFailure<int>)resultAfterBind;
            Assert.That(exceptionFailure.Exception, Is.Not.Null);
            Assert.That(exceptionFailure.Exception, Is.EqualTo(exception));
        }

    }

    [Test]
    public async Task When_AsyncSuccessFunctionThrowsException_Then_ReturnsExceptionFailure()
    {
        // Arrange
        IResult<int> syncResult = Success<int>.Create(42);
        var startingResult = Task.FromResult(syncResult);
        var exception      = new Exception("Test Exception");

        // ReSharper disable once MoveLocalFunctionAfterJumpStatement
        void SuccessFunction(int value) { throw exception; }

        //Act
        var resultAfterBind = await startingResult.Tap(SuccessFunction);

        // Assert
        Assert.That(resultAfterBind, Is.InstanceOf<ExceptionFailure<int>>());
        using (Assert.EnterMultipleScope())
        {
            var exceptionFailure = (ExceptionFailure<int>)resultAfterBind;
            Assert.That(exceptionFailure.Exception, Is.Not.Null);
            Assert.That(exceptionFailure.Exception, Is.EqualTo(exception));
            Assert.That(exceptionFailure.ErrorMessage, Is.EqualTo("Test Exception"));
        }
    }

    [Test]
    public async Task When_AsyncSuccessFunctionThrowsException_Then_ReturnsExceptionFailureAsync()
    {
        // Arrange
        IResult<int> syncResult     = Success<int>.Create(42);
        var          startingResult = Task.FromResult(syncResult);
        var          exception      = new Exception("Test Exception");

        // ReSharper disable once MoveLocalFunctionAfterJumpStatement
        Task SuccessFunction(int value, CancellationToken cancellationToken) => throw exception;

        //Act
        var resultAfterBind = await startingResult.Tap(SuccessFunction, CancellationToken.None);

        // Assert
        Assert.That(resultAfterBind, Is.InstanceOf<ExceptionFailure<int>>());
        using (Assert.EnterMultipleScope())
        {
            var exceptionFailure = (ExceptionFailure<int>)resultAfterBind;
            Assert.That(exceptionFailure.Exception,    Is.Not.Null);
            Assert.That(exceptionFailure.Exception,    Is.EqualTo(exception));
            Assert.That(exceptionFailure.ErrorMessage, Is.EqualTo("Test Exception"));
        }
    }

    [Test]
    public async Task When_SuccessFunctionThrowsException_Then_ReturnsExceptionFailureAsync()
    {
        // Arrange
        var          startingResult = Success<int>.Create(42);
        var          exception      = new Exception("Test Exception");

        // ReSharper disable once MoveLocalFunctionAfterJumpStatement
        Task SuccessFunction(int value, CancellationToken cancellationToken) => throw exception;

        //Act
        var resultAfterBind = await startingResult.Tap(SuccessFunction, CancellationToken.None);

        // Assert
        Assert.That(resultAfterBind, Is.InstanceOf<ExceptionFailure<int>>());
        using (Assert.EnterMultipleScope())
        {
            var exceptionFailure = (ExceptionFailure<int>)resultAfterBind;
            Assert.That(exceptionFailure.Exception,    Is.Not.Null);
            Assert.That(exceptionFailure.Exception,    Is.EqualTo(exception));
            Assert.That(exceptionFailure.ErrorMessage, Is.EqualTo("Test Exception"));
        }
    }

    [Test]
    public void When_SuccessFunctionExpectsNoValueThrowsException_Then_ReturnsExceptionFailure()
    {
        // Arrange
        var startingResult = Success<int>.Create(42);
        var exception      = new Exception("Test Exception");

        // ReSharper disable once MoveLocalFunctionAfterJumpStatement
        void SuccessFunction() => throw exception;

        //Act
        var resultAfterBind = startingResult.Tap(SuccessFunction);

        // Assert
        Assert.That(resultAfterBind, Is.InstanceOf<ExceptionFailure<int>>());
        using (Assert.EnterMultipleScope())
        {
            var exceptionFailure = (ExceptionFailure<int>)resultAfterBind;
            Assert.That(exceptionFailure.Exception, Is.Not.Null);
            Assert.That(exceptionFailure.Exception, Is.EqualTo(exception));
            Assert.That(exceptionFailure.ErrorMessage, Is.EqualTo("Test Exception"));
        }

    }

    [Test]
    public async Task When_SuccessFunctionExpectsNoValueThrowsException_Then_ReturnsExceptionFailureAsync()
    {
        // Arrange
        var startingResult = Success<int>.Create(42);
        var exception      = new Exception("Test Exception");

        // ReSharper disable once MoveLocalFunctionAfterJumpStatement
        Task SuccessFunction(CancellationToken cancellationToken) => throw exception;

        //Act
        var resultAfterBind = await startingResult.Tap(SuccessFunction, CancellationToken.None);

        // Assert
        Assert.That(resultAfterBind, Is.InstanceOf<ExceptionFailure<int>>());
        using (Assert.EnterMultipleScope())
        {
            var exceptionFailure = (ExceptionFailure<int>)resultAfterBind;
            Assert.That(exceptionFailure.Exception, Is.Not.Null);
            Assert.That(exceptionFailure.Exception, Is.EqualTo(exception));
        }

    }

    [Test]
    public async Task When_AsyncSuccessFunctionExpectsNoValueThrowsException_Then_ReturnsExceptionFailure()
    {
        // Arrange
        IResult<int> syncResult     = Success<int>.Create(42);
        var          startingResult = Task.FromResult(syncResult);
        var          exception      = new Exception("Test Exception");

        // ReSharper disable once MoveLocalFunctionAfterJumpStatement
        void SuccessFunction() => throw exception;

        //Act
        var resultAfterBind = await startingResult.Tap(SuccessFunction);

        // Assert
        Assert.That(resultAfterBind, Is.InstanceOf<ExceptionFailure<int>>());
        using (Assert.EnterMultipleScope())
        {
            var exceptionFailure = (ExceptionFailure<int>)resultAfterBind;
            Assert.That(exceptionFailure.Exception,    Is.Not.Null);
            Assert.That(exceptionFailure.Exception,    Is.EqualTo(exception));
            Assert.That(exceptionFailure.ErrorMessage, Is.EqualTo("Test Exception"));
        }

    }

    [Test]
    public async Task When_AsyncSuccessFunctionExpectsNoValueThrowsException_Then_ReturnsExceptionFailureAsync()
    {
        // Arrange
        IResult<int> syncResult     = Success<int>.Create(42);
        var          startingResult = Task.FromResult(syncResult);
        var          exception      = new Exception("Test Exception");

        // ReSharper disable once MoveLocalFunctionAfterJumpStatement
        Task SuccessFunction(CancellationToken cancellationToken) => throw exception;

        //Act
        var resultAfterBind = await startingResult.Tap(SuccessFunction, CancellationToken.None);

        // Assert
        Assert.That(resultAfterBind, Is.InstanceOf<ExceptionFailure<int>>());
        using (Assert.EnterMultipleScope())
        {
            var exceptionFailure = (ExceptionFailure<int>)resultAfterBind;
            Assert.That(exceptionFailure.Exception, Is.Not.Null);
            Assert.That(exceptionFailure.Exception, Is.EqualTo(exception));
        }

    }
}