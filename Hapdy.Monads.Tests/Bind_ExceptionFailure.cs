// ReSharper disable InconsistentNaming
// ReSharper disable CheckNamespace

using Hapdy.Monads.Results.Extensions;

namespace Hapdy.Monads.Results.Testing_Bind;

[TestFixture(TestOf = typeof(Failure<>)
           , TestName = "ExceptionFailure"
           , Category = "1 - Bind")]
public class Bind_ExceptionFailure
{
    [SetUp] public void Setup() { }

    [Test]
    public void When_SuccessFunctionExpectsValue_Then_DoesNotRunuccessFunction()
    {
        // Arrange
        const string testErrorMessage         = "Testing binding failure result.";
        var          testException            = new Exception(testErrorMessage);
        var          startingResult           = ExceptionFailure<int>.Create(testException);
        var          successFunctionWasCalled = false;

        // ReSharper disable once MoveLocalFunctionAfterJumpStatement
        IResult<int> SuccessFunction(int value)
        {
            successFunctionWasCalled = true;
            return Success<int>.Create(value * 2);
        }

        // Act
        var resultAfterBind = startingResult.Bind(SuccessFunction);

        // Assert
        Assert.That(resultAfterBind, Is.InstanceOf<ExceptionFailure<int>>());
        using (Assert.EnterMultipleScope())
        {
            Assert.That(successFunctionWasCalled, Is.False);
            var exceptionFailureResult = (ExceptionFailure<int>)resultAfterBind;
            Assert.That(exceptionFailureResult.Exception,    Is.EqualTo(testException));
            Assert.That(exceptionFailureResult.ErrorMessage, Is.EqualTo(testErrorMessage));
        }
    }

    [Test]
    public async Task When_SuccessFunctionExpectsValue_Then_DoesNotRunuccessFunctionAsync()
    {
        // Arrange
        const string testErrorMessage         = "Testing binding failure result.";
        var          testException            = new Exception(testErrorMessage);
        var          startingResult           = ExceptionFailure<int>.Create(testException);
        var          successFunctionWasCalled = false;

        // ReSharper disable once MoveLocalFunctionAfterJumpStatement
        Task<IResult<int>> SuccessFunction(int value, CancellationToken cancellationToken)
        {
            successFunctionWasCalled = true;
            return Task.FromResult((IResult<int>)Success<int>.Create(value * 2));
        }

        // Act
        var resultAfterBind = await startingResult.Bind(SuccessFunction, CancellationToken.None);

        // Assert
        Assert.That(resultAfterBind, Is.InstanceOf<ExceptionFailure<int>>());
        using (Assert.EnterMultipleScope())
        {
            Assert.That(successFunctionWasCalled, Is.False);
            var exceptionFailure = (ExceptionFailure<int>)resultAfterBind;
            Assert.That(exceptionFailure.Exception,    Is.EqualTo(testException));
            Assert.That(exceptionFailure.ErrorMessage, Is.EqualTo(testErrorMessage));
        }
    }

    [Test]
    public async Task When_AsyncSuccessFunctionExpectsValue_Then_DoesNotRunuccessFunction()
    {
        // Arrange
        const string testErrorMessage         = "Testing binding failure result.";
        var          testException            = new Exception(testErrorMessage);
        var          startingResult           = Task.FromResult((IResult<int>)ExceptionFailure<int>.Create(testException));
        var          successFunctionWasCalled = false;

        // ReSharper disable once MoveLocalFunctionAfterJumpStatement
        IResult<int> SuccessFunction(int value)
        {
            successFunctionWasCalled = true;
            return Success<int>.Create(value * 2);
        }

        // Act
        var resultAfterBind = await startingResult.Bind(SuccessFunction);

        // Assert
        Assert.That(resultAfterBind, Is.InstanceOf<ExceptionFailure<int>>());
        using (Assert.EnterMultipleScope())
        {
            Assert.That(successFunctionWasCalled, Is.False);
            var exceptionFailureResult = (ExceptionFailure<int>)resultAfterBind;
            Assert.That(exceptionFailureResult.Exception,    Is.EqualTo(testException));
            Assert.That(exceptionFailureResult.ErrorMessage, Is.EqualTo(testErrorMessage));
        }
    }

    [Test]
    public async Task When_AsyncSuccessFunctionExpectsValue_Then_DoesNotRunuccessFunctionAsync()
    {
        // Arrange
        const string testErrorMessage         = "Testing binding failure result.";
        var          testException            = new Exception(testErrorMessage);
        var          startingResult           = Task.FromResult((IResult<int>)ExceptionFailure<int>.Create(testException));
        var          successFunctionWasCalled = false;

        // ReSharper disable once MoveLocalFunctionAfterJumpStatement
        Task<IResult<int>> SuccessFunction(int value, CancellationToken cancellationToken)
        {
            successFunctionWasCalled = true;
            return Task.FromResult((IResult<int>)Success<int>.Create(value * 2));
        }

        // Act
        var resultAfterBind = await startingResult.Bind(SuccessFunction, CancellationToken.None);

        // Assert
        Assert.That(resultAfterBind, Is.InstanceOf<ExceptionFailure<int>>());
        using (Assert.EnterMultipleScope())
        {
            Assert.That(successFunctionWasCalled, Is.False);
            var exceptionFailureResult = (ExceptionFailure<int>)resultAfterBind;
            Assert.That(exceptionFailureResult.Exception,    Is.EqualTo(testException));
            Assert.That(exceptionFailureResult.ErrorMessage, Is.EqualTo(testErrorMessage));
        }
    }

    [Test]
    public void When_SuccessFunctionExpectsNoValue_Then_DoesNotRunuccessFunction()
    {
        // Arrange
        const string testErrorMessage         = "Testing binding failure result.";
        var          testException            = new Exception(testErrorMessage);
        var          startingResult           = ExceptionFailure<int>.Create(testException);
        var          successFunctionWasCalled = false;

        // ReSharper disable once MoveLocalFunctionAfterJumpStatement
        IResult<int> SuccessFunction()
        {
            successFunctionWasCalled = true;
            return Success<int>.Create(0);
        }

        // Act
        var resultAfterBind = startingResult.Bind(SuccessFunction);

        // Assert
        Assert.That(resultAfterBind, Is.InstanceOf<ExceptionFailure<int>>());
        using (Assert.EnterMultipleScope())
        {
            Assert.That(successFunctionWasCalled, Is.False);
            var exceptionFailureResult = (ExceptionFailure<int>)resultAfterBind;
            Assert.That(exceptionFailureResult.Exception,    Is.EqualTo(testException));
            Assert.That(exceptionFailureResult.ErrorMessage, Is.EqualTo(testErrorMessage));
        }
    }

    [Test]
    public async Task When_SuccessFunctionExpectsNoValue_Then_DoesNotRunuccessFunctionAsync()
    {
        // Arrange
        const string testErrorMessage         = "Testing binding failure result.";
        var          testException            = new Exception(testErrorMessage);
        var          startingResult           = ExceptionFailure<int>.Create(testException);
        var          successFunctionWasCalled = false;

        // ReSharper disable once MoveLocalFunctionAfterJumpStatement
        Task<IResult<int>> SuccessFunction(CancellationToken cancellationToken)
        {
            successFunctionWasCalled = true;
            return Task.FromResult((IResult<int>)Success<int>.Create(0));
        }

        // Act
        var resultAfterBind = await startingResult.Bind(SuccessFunction
                                                      , CancellationToken.None);

        // Assert
        Assert.That(resultAfterBind, Is.InstanceOf<ExceptionFailure<int>>());
        using (Assert.EnterMultipleScope())
        {
            Assert.That(successFunctionWasCalled, Is.False);
            var exceptionFailureResult = (ExceptionFailure<int>)resultAfterBind;
            Assert.That(exceptionFailureResult.Exception,    Is.EqualTo(testException));
            Assert.That(exceptionFailureResult.ErrorMessage, Is.EqualTo(testErrorMessage));
        }
    }

    [Test]
    public async Task When_AsyncSuccessFunctionExpectsNoValue_Then_DoesNotRunuccessFunction()
    {
        // Arrange
        const string testErrorMessage         = "Testing binding failure result.";
        var          testException            = new Exception(testErrorMessage);
        var          startingResult           = Task.FromResult((IResult<int>)ExceptionFailure<int>.Create(testException));
        var          successFunctionWasCalled = false;

        // ReSharper disable once MoveLocalFunctionAfterJumpStatement
        IResult<int> SuccessFunction()
        {
            successFunctionWasCalled = true;
            return Success<int>.Create(0);
        }

        // Act
        var resultAfterBind = await startingResult.Bind(SuccessFunction);

        // Assert
        Assert.That(resultAfterBind, Is.InstanceOf<ExceptionFailure<int>>());
        using (Assert.EnterMultipleScope())
        {
            Assert.That(successFunctionWasCalled, Is.False);
            var exceptionFailureResult = (ExceptionFailure<int>)resultAfterBind;
            Assert.That(exceptionFailureResult.Exception,    Is.EqualTo(testException));
            Assert.That(exceptionFailureResult.ErrorMessage, Is.EqualTo(testErrorMessage));
        }
    }

    [Test]
    public async Task When_AsyncSuccessFunctionExpectsNoValue_Then_DoesNotRunuccessFunctionAsync()
    {
        // Arrange
        const string testErrorMessage         = "Testing binding failure result.";
        var          testException            = new Exception(testErrorMessage);
        var          startingResult           = Task.FromResult((IResult<int>)ExceptionFailure<int>.Create(testException));
        var          successFunctionWasCalled = false;

        // ReSharper disable once MoveLocalFunctionAfterJumpStatement
        Task<IResult<int>> SuccessFunction(CancellationToken cancellationToken)
        {
            successFunctionWasCalled = true;
            return Task.FromResult((IResult<int>)Success<int>.Create(0));
        }

        // Act
        var resultAfterBind = await startingResult.Bind(SuccessFunction
                                                      , CancellationToken.None);

        // Assert
        Assert.That(resultAfterBind, Is.InstanceOf<ExceptionFailure<int>>());
        using (Assert.EnterMultipleScope())
        {
            Assert.That(successFunctionWasCalled, Is.False);
            var exceptionFailureResult = (ExceptionFailure<int>)resultAfterBind;
            Assert.That(exceptionFailureResult.Exception,    Is.EqualTo(testException));
            Assert.That(exceptionFailureResult.ErrorMessage, Is.EqualTo(testErrorMessage));
        }
    }

}