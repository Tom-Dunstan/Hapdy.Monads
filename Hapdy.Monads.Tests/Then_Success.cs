// ReSharper disable InconsistentNaming
// ReSharper disable CheckNamespace

using Hapdy.Monads.Results.Extensions;

namespace Hapdy.Monads.Results.Testing_Then;

[TestFixture(TestOf = typeof(Success<>)
           , TestName = "Success"
           , Category = "2 - Then")]
public class Then_Success
{
    [SetUp] public void Setup() { }

    [Test]
    public void When_SuccessAndExpectsValue_Then_RunsSuccessFunction()
    {
        // Arrange
        const int testValue                    = 42;
        var       startingResult               = Success<int>.Create(42);
        int?      valuePassedToSuccessFunction = null;
    
        // ReSharper disable once MoveLocalFunctionAfterJumpStatement
        IResult<int> SuccessFunction(int value)
        {
            valuePassedToSuccessFunction = value;
            return Success<int>.Create(value * 2);
        }
    
        // Act
        var resultAfterThen = startingResult.Then(SuccessFunction);
    
        // Assert
        Assert.That(resultAfterThen,              Is.InstanceOf<Success<int>>());
        using (Assert.EnterMultipleScope())
        {
            Assert.That(valuePassedToSuccessFunction, Is.EqualTo(testValue));
            var successResult = (Success<int>)resultAfterThen;
            Assert.That(successResult.Value, Is.EqualTo(testValue * 2));
        }
    }

    [Test]
    public async Task When_SuccessAndExpectsValue_Then_RunsSuccessFunctionAsync()
    {
        // Arrange
        var  startingResult               = Success<int>.Create(42);
        int? valuePassedToSuccessFunction = null;
    
        // ReSharper disable once MoveLocalFunctionAfterJumpStatement
        Task<IResult<int>> SuccessFunction(int value, CancellationToken cancellationToken)
        {
            valuePassedToSuccessFunction = value;
            return Task.FromResult((IResult<int>)Success<int>.Create(value * 2));
        }
    
        // Act
        var resultAfterThen = await startingResult.Then(SuccessFunction, CancellationToken.None);
    
        // Assert
        Assert.That(resultAfterThen,              Is.InstanceOf<Success<int>>());
        using (Assert.EnterMultipleScope())
        {
            Assert.That(valuePassedToSuccessFunction, Is.Not.Null);
            Assert.That(valuePassedToSuccessFunction, Is.EqualTo(42));
        }
    }

    [Test]
    public async Task When_AsyncSuccessAndExpectsValue_Then_RunsSuccessFunction()
    {
        // Arrange
        var  startingResult               = Task.FromResult((IResult<int>)Success<int>.Create(42));
        int? valuePassedToSuccessFunction = null;

        // ReSharper disable once MoveLocalFunctionAfterJumpStatement
        IResult<int> SuccessFunction(int value)
        {
            valuePassedToSuccessFunction = value;
            return Success<int>.Create(value * 2);
        }

        // Act
        var resultAfterThen = await startingResult.Then(SuccessFunction);

        // Assert
        Assert.That(resultAfterThen, Is.InstanceOf<Success<int>>());
        using (Assert.EnterMultipleScope())
        {
            Assert.That(valuePassedToSuccessFunction, Is.Not.Null);
            Assert.That(valuePassedToSuccessFunction, Is.EqualTo(42));
        }
    }

    [Test]
    public async Task When_AsyncSuccessAndExpectsValue_Then_RunsSuccessFunctionAsync()
    {
        // Arrange
        var  startingResult               = Task.FromResult((IResult<int>)Success<int>.Create(42));
        int? valuePassedToSuccessFunction = null;
    
        // ReSharper disable once MoveLocalFunctionAfterJumpStatement
        Task<IResult<int>> SuccessFunction(int value, CancellationToken cancellationToken)
        {
            valuePassedToSuccessFunction = value;
            return Task.FromResult((IResult<int>)Success<int>.Create(value * 2));
        }
    
        // Act
        var resultAfterThen = await startingResult.Then(SuccessFunction, CancellationToken.None);
    
        // Assert
        Assert.That(resultAfterThen,              Is.InstanceOf<Success<int>>());
        using (Assert.EnterMultipleScope())
        {
            Assert.That(valuePassedToSuccessFunction, Is.Not.Null);
            Assert.That(valuePassedToSuccessFunction, Is.EqualTo(42));
        }
    }

    [Test]
    public void When_SuccessAndExpectsNoValue_Then_RunsSuccessFunction()
    {
        // Arrange
        const int    testValue           = 42;
        const string expectedResultValue = $"Expected Result";
        var          functionWasCalled   = false;
        Success<int> startingResult      = new(testValue);

        // ReSharper disable once MoveLocalFunctionAfterJumpStatement
        IResult<string> SuccessFunction()
        {
            functionWasCalled = true;
            return Success<string>.Create(expectedResultValue);
        }

        // Act
        var resultAfterThen = startingResult.Then(SuccessFunction);

        // Assert
        Assert.That(resultAfterThen, Is.InstanceOf<Success<string>>());
        using (Assert.EnterMultipleScope())
        {
            Assert.That(functionWasCalled, Is.True);
            var successResult = (Success<string>)resultAfterThen;
            Assert.That(successResult.Value, Is.EqualTo(expectedResultValue));
        }
    }

    [Test]
    public async Task When_SuccessAndExpectsNoValue_Then_RunsSuccessFunctionAsync()
    {
        // Arrange
        const int    testValue           = 42;
        const string expectedResultValue = $"Expected Result";
        var          functionWasCalled   = false;
        Success<int> startingResult      = new(testValue);

        // ReSharper disable once MoveLocalFunctionAfterJumpStatement
        Task<IResult<string>> SuccessFunction(CancellationToken cancellationToken)
        {
            functionWasCalled = true;
            return Task.FromResult((IResult<string>)Success<string>.Create(expectedResultValue));
        }

        // Act
        var resultAfterThen = await startingResult.Then(SuccessFunction, CancellationToken.None);

        // Assert
        Assert.That(resultAfterThen, Is.InstanceOf<Success<string>>());
        using (Assert.EnterMultipleScope())
        {
            Assert.That(functionWasCalled, Is.True);
            var successResult = (Success<string>)resultAfterThen;
            Assert.That(successResult.Value, Is.EqualTo(expectedResultValue));
        }
    }

    [Test]
    public async Task When_AsyncSuccessAndExpectsNoValue_Then_RunsSuccessFunction()
    {
        // Arrange
        const int    testValue           = 42;
        const string expectedResultValue = $"Expected Result";
        var          functionWasCalled   = false;
        var          startingResult = Task.FromResult((IResult<int>)Success<int>.Create(testValue));

        // ReSharper disable once MoveLocalFunctionAfterJumpStatement
        IResult<string> SuccessFunction()
        {
            functionWasCalled = true;
            return Success<string>.Create(expectedResultValue);
        }

        // Act
        var resultAfterThen = await startingResult.Then(SuccessFunction);

        // Assert
        Assert.That(resultAfterThen, Is.InstanceOf<Success<string>>());
        using (Assert.EnterMultipleScope())
        {
            Assert.That(functionWasCalled, Is.True);
            var successResult = (Success<string>)resultAfterThen;
            Assert.That(successResult.Value, Is.EqualTo(expectedResultValue));
        }
    }

    [Test]
    public async Task When_AsyncSuccessAndExpectsNoValue_Then_RunsSuccessFunctionAsync()
    {
        // Arrange
        const int    testValue           = 42;
        const string expectedResultValue = $"Expected Result";
        var          functionWasCalled   = false;
        var          startingResult      = Task.FromResult((IResult<int>)Success<int>.Create(testValue));

        // ReSharper disable once MoveLocalFunctionAfterJumpStatement
        Task<IResult<string>> SuccessFunction(CancellationToken cancellationToken)
        {
            functionWasCalled = true;
            return Task.FromResult((IResult<string>)Success<string>.Create(expectedResultValue));
        }

        // Act
        var resultAfterThen = await startingResult.Then(SuccessFunction, CancellationToken.None);

        // Assert
        Assert.That(resultAfterThen, Is.InstanceOf<Success<string>>());
        using (Assert.EnterMultipleScope())
        {
            Assert.That(functionWasCalled, Is.True);
            var successResult = (Success<string>)resultAfterThen;
            Assert.That(successResult.Value, Is.EqualTo(expectedResultValue));
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
        var resultAfterThen = startingResult.Then(SuccessFunction);
        
        // Assert
        Assert.That(resultAfterThen, Is.InstanceOf<ExceptionFailure<int>>());
        using (Assert.EnterMultipleScope())
        {
            var exceptionFailure = (ExceptionFailure<int>)resultAfterThen;
            Assert.That(exceptionFailure.Exception,    Is.Not.Null);
            Assert.That(exceptionFailure.Exception,    Is.EqualTo(exception));
            Assert.That(exceptionFailure.ErrorMessage, Is.EqualTo("Test Exception"));
        }
        
    }

    [Test]
    public async Task When_AsyncSuccessFunctionThrowsException_Then_ReturnsExceptionFailure()
    {
        // Arrange
        var startingResult = Task.FromResult((IResult<int>)Success<int>.Create(42));
        var exception      = new Exception("Test Exception");

        // ReSharper disable once MoveLocalFunctionAfterJumpStatement
        IResult<int> SuccessFunction(int value) { throw exception; }

        //Act
        var resultAfterBind = await startingResult.Then(SuccessFunction);

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
    public async Task When_SuccessFunctionThrowsException_Then_ReturnsExceptionFailureAsync()
    {
        // Arrange
        var startingResult = Task.FromResult((IResult<int>)Success<int>.Create(42));
        var exception      = new Exception("Test Exception");

        // ReSharper disable once MoveLocalFunctionAfterJumpStatement
        IResult<int> SuccessFunction(int value) { throw exception; }

        //Act
        var resultAfterBind = await startingResult.Then(SuccessFunction);

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
    public async Task When_AsyncSuccessFunctionThrowsException_Then_ReturnsExceptionFailureAsync()
    {
        // Arrange
        IResult<int> syncResult     = Success<int>.Create(42);
        var          startingResult = Task.FromResult(syncResult);
        var          exception      = new Exception("Test Exception");

        // ReSharper disable once MoveLocalFunctionAfterJumpStatement
        IResult<int> SuccessFunction(int value) => throw exception;

        //Act
        var resultAfterBind = await startingResult.Then(SuccessFunction);

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
        IResult<int> SuccessFunction() => throw exception;

        //Act
        var resultAfterBind = startingResult.Then(SuccessFunction);

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
    public async Task When_SuccessFunctionExpectsNoValueThrowsException_Then_ReturnsExceptionFailureAsync()
    {
        // Arrange
        var startingResult = Success<int>.Create(42);
        var exception      = new Exception("Test Exception");

        // ReSharper disable once MoveLocalFunctionAfterJumpStatement
        Task<IResult<int>> SuccessFunction(CancellationToken cancellationToken) => throw exception;

        //Act
        var resultAfterBind = await startingResult.Then(SuccessFunction, CancellationToken.None);

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
    public async Task When_AsyncSuccessFunctionExpectsNoValueThrowsException_Then_ReturnsExceptionFailure()
    {
        // Arrange
        IResult<int> syncResult     = Success<int>.Create(42);
        var          startingResult = Task.FromResult(syncResult);
        var          exception      = new Exception("Test Exception");

        // ReSharper disable once MoveLocalFunctionAfterJumpStatement
        IResult<int> SuccessFunction() => throw exception;

        //Act
        var resultAfterBind = await startingResult.Then(SuccessFunction);

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
    public async Task When_AsyncSuccessFunctionExpectsNoValueThrowsException_Then_ReturnsExceptionFailureAsync()
    {
        // Arrange
        IResult<int> syncResult     = Success<int>.Create(42);
        var          startingResult = Task.FromResult(syncResult);
        var          exception      = new Exception("Test Exception");

        // ReSharper disable once MoveLocalFunctionAfterJumpStatement
        Task<IResult<int>> SuccessFunction(CancellationToken cancellationToken) => throw exception;

        //Act
        var resultAfterBind = await startingResult.Then(SuccessFunction, CancellationToken.None);

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
}