// ReSharper disable InconsistentNaming
// ReSharper disable CheckNamespace

using Hapdy.Monads.Results.Extensions;

namespace Hapdy.Monads.Results.Testing_Then;

[TestFixture(TestOf = typeof(Failure<>)
           , TestName = "Failure"
           , Category = "2 - Then")]
public class Then_Failure
{
    [SetUp] public void Setup() { }

    [Test]
    public void When_FailureAndSuccessExpectsValue_Then_DoesNotRunSuccessFunction()
    {
        // Arrange
        const string testErrorMessage         = "Testing binding failure result.";
        var          startingResult           = Failure<int>.Create(testErrorMessage);
        var          successFunctionWasCalled = false;

        // ReSharper disable once MoveLocalFunctionAfterJumpStatement
        IResult<int> SuccessFunction(int value)
        {
            successFunctionWasCalled = true;
            return Success<int>.Create(value * 2);
        }

        // Act
        var resultAfterThen = startingResult.Then(SuccessFunction);

        // Assert
        Assert.That(resultAfterThen, Is.InstanceOf<Failure<int>>());
        using (Assert.EnterMultipleScope())
        {
            Assert.That(successFunctionWasCalled, Is.False);
            var failureResult = (Failure<int>)resultAfterThen;
            Assert.That(failureResult.ErrorMessage, Is.EqualTo(testErrorMessage));
        }
    }

    [Test]
    public async Task When_FailureAndSuccessExpectsValue_Then_DoesNotRunSuccessFunctionAsync()
    {
        // Arrange
        const string testErrorMessage         = "Testing binding failure result.";
        var          startingResult           = Failure<int>.Create(testErrorMessage);
        var          successFunctionWasCalled = false;

        // ReSharper disable once MoveLocalFunctionAfterJumpStatement
        Task<IResult<int>> SuccessFunction(int value, CancellationToken cancellationToken)
        {
            successFunctionWasCalled = true;
            return Task.FromResult((IResult<int>)Success<int>.Create(value * 2));
        }

        // Act
        var resultAfterThen = await startingResult.Then(SuccessFunction, CancellationToken.None);

        // Assert
        Assert.That(resultAfterThen, Is.InstanceOf<Failure<int>>());
        using (Assert.EnterMultipleScope())
        {
            Assert.That(successFunctionWasCalled, Is.False);
            var failureResult = (Failure<int>)resultAfterThen;
            Assert.That(failureResult.ErrorMessage, Is.EqualTo(testErrorMessage));
        }
    }

    [Test]
    public async Task When_AsyncFailureAndSuccessExpectsValue_Then_DoesNotRunSuccessFunction()
    {
        // Arrange
        const string testErrorMessage         = "Testing binding failure result.";
        var          startingResult           = Task.FromResult((IResult<int>)Failure<int>.Create(testErrorMessage));
        var          successFunctionWasCalled = false;

        // ReSharper disable once MoveLocalFunctionAfterJumpStatement
        IResult<int> SuccessFunction(int value)
        {
            successFunctionWasCalled = true;
            return Success<int>.Create(value * 2);
        }

        // Act
        var resultAfterThen = await startingResult.Then(SuccessFunction);

        // Assert
        Assert.That(resultAfterThen, Is.InstanceOf<Failure<int>>());
        using (Assert.EnterMultipleScope())
        {
            Assert.That(successFunctionWasCalled, Is.False);
            var failureResult = (Failure<int>)resultAfterThen;
            Assert.That(failureResult.ErrorMessage, Is.EqualTo(testErrorMessage));
        }
    }

    [Test]
    public async Task When_AsyncFailureAndSuccessExpectsValue_Then_DoesNotRunSuccessFunctionAsync()
    {
        // Arrange
        const string testErrorMessage         = "Testing binding failure result.";
        var          startingResult           = Task.FromResult((IResult<int>)Failure<int>.Create(testErrorMessage));
        var          successFunctionWasCalled = false;

        // ReSharper disable once MoveLocalFunctionAfterJumpStatement
        Task<IResult<int>> SuccessFunction(int value, CancellationToken cancellationToken)
        {
            successFunctionWasCalled = true;
            return Task.FromResult((IResult<int>)Success<int>.Create(value * 2));
        }

        // Act
        var resultAfterThen = await startingResult.Then(SuccessFunction, CancellationToken.None);

        // Assert
        Assert.That(resultAfterThen, Is.InstanceOf<Failure<int>>());
        using (Assert.EnterMultipleScope())
        {
            Assert.That(successFunctionWasCalled, Is.False);
            var failureResult = (Failure<int>)resultAfterThen;
            Assert.That(failureResult.ErrorMessage, Is.EqualTo(testErrorMessage));
        }
    }

    [Test]
    public void When_FailureAndSuccessExpectsNoParam_Then_DoesNotRunSuccessFunction()
    {
        // Arrange
        const string testErrorMessage         = "Testing binding failure result.";
        var          startingResult           = Failure<int>.Create(testErrorMessage);
        var          successFunctionWasCalled = false;

        // ReSharper disable once MoveLocalFunctionAfterJumpStatement
        IResult<int> SuccessFunction()
        {
            successFunctionWasCalled = true;
            return Success<int>.Create(0);
        }

        // Act
        var resultAfterThen = startingResult.Then(SuccessFunction);

        // Assert
        Assert.That(resultAfterThen, Is.InstanceOf<Failure<int>>());
        using (Assert.EnterMultipleScope())
        {
            Assert.That(successFunctionWasCalled, Is.False);
            var failureResult = (Failure<int>)resultAfterThen;
            Assert.That(failureResult.ErrorMessage, Is.EqualTo(testErrorMessage));
        }
    }

    [Test]
    public async Task When_FailureAndSuccessExpectsNoParam_Then_DoesNotRunSuccessFunctionAsync()
    {
        // Arrange
        const string testErrorMessage         = "Testing binding failure result.";
        var          startingResult           = Failure<int>.Create(testErrorMessage);
        var          successFunctionWasCalled = false;

        // ReSharper disable once MoveLocalFunctionAfterJumpStatement
        Task<IResult<int>> SuccessFunction(CancellationToken cancellationToken)
        {
            successFunctionWasCalled = true;
            return Task.FromResult((IResult<int>)Success<int>.Create(0));
        }

        // Act
        var resultAfterThen = await startingResult.Then(SuccessFunction
                                                      , CancellationToken.None);

        // Assert
        Assert.That(resultAfterThen, Is.InstanceOf<Failure<int>>());
        using (Assert.EnterMultipleScope())
        {
            Assert.That(successFunctionWasCalled, Is.False);
            var failureResult = (Failure<int>)resultAfterThen;
            Assert.That(failureResult.ErrorMessage, Is.EqualTo(testErrorMessage));
        }
    }

    [Test]
    public async Task When_AsyncFailureAndSuccessExpectsNoParam_Then_DoesNotRunSuccessFunction()
    {
        // Arrange
        const string testErrorMessage         = "Testing binding failure result.";
        var          startingResult           = Task.FromResult((IResult<int>)Failure<int>.Create(testErrorMessage));
        var          successFunctionWasCalled = false;

        // ReSharper disable once MoveLocalFunctionAfterJumpStatement
        IResult<int> SuccessFunction()
        {
            successFunctionWasCalled = true;
            return Success<int>.Create(0);
        }

        // Act
        var resultAfterThen = await startingResult.Then(SuccessFunction);

        // Assert
        Assert.That(resultAfterThen, Is.InstanceOf<Failure<int>>());
        using (Assert.EnterMultipleScope())
        {
            Assert.That(successFunctionWasCalled, Is.False);
            var failureResult = (Failure<int>)resultAfterThen;
            Assert.That(failureResult.ErrorMessage, Is.EqualTo(testErrorMessage));
        }
    }

    [Test]
    public async Task When_AsyncFailureAndSuccessExpectsNoParam_Then_DoesNotRunSuccessFunctionAsync()
    {
        // Arrange
        const string testErrorMessage         = "Testing binding failure result.";
        var          startingResult           = Task.FromResult((IResult<int>)Failure<int>.Create(testErrorMessage));
        var          successFunctionWasCalled = false;

        // ReSharper disable once MoveLocalFunctionAfterJumpStatement
        Task<IResult<int>> SuccessFunction(CancellationToken cancellationToken)
        {
            successFunctionWasCalled = true;
            return Task.FromResult((IResult<int>)Success<int>.Create(0));
        }

        // Act
        var resultAfterThen = await startingResult.Then(SuccessFunction
                                                      , CancellationToken.None);

        // Assert
        Assert.That(resultAfterThen, Is.InstanceOf<Failure<int>>());
        using (Assert.EnterMultipleScope())
        {
            Assert.That(successFunctionWasCalled, Is.False);
            var failureResult = (Failure<int>)resultAfterThen;
            Assert.That(failureResult.ErrorMessage, Is.EqualTo(testErrorMessage));
        }
    }
   
}