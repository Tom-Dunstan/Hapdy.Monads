// ReSharper disable InconsistentNaming
// ReSharper disable CheckNamespace

using Hapdy.Monads.Results.Extensions;

namespace Hapdy.Monads.Results.Testing_Bind;

[TestFixture(TestOf = typeof(Failure<>)
           , TestName = "Failure"
           , Category = "1 - Bind")]
public class Bind_Failure
{
    [SetUp] public void Setup() { }

    [Test]
    public void When_TestingIsFailure_Then_ReturnsTrue()
    {
        // Arrange
        var result = Failure<int>.Create("Testing failure result.");

        // Act
        var isSuccess = result.IsSuccess;
        var isFailure = result.IsFailure;

        // Assert
        using (Assert.EnterMultipleScope())
        {
            Assert.That(isSuccess, Is.False);
            Assert.That(isFailure, Is.True);
        }
    }

    [Test]
    public void When_FailureAndSuccessExpectsValue_Then_DoesNotRunsSuccessFunction()
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
        var resultAfterBind = startingResult.Bind(SuccessFunction);

        // Assert
        Assert.That(resultAfterBind, Is.InstanceOf<Failure<int>>());
        using (Assert.EnterMultipleScope())
        {
            Assert.That(successFunctionWasCalled, Is.False);
            var failureResult = (Failure<int>)resultAfterBind;
            Assert.That(failureResult.ErrorMessage, Is.EqualTo(testErrorMessage));
        }
    }

    [Test]
    public async Task When_FailureAndSuccessExpectsValue_Then_DoesNotRunsSuccessFunctionAsync()
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
        var resultAfterBind = await startingResult.Bind(SuccessFunction, CancellationToken.None);

        // Assert
        Assert.That(resultAfterBind, Is.InstanceOf<Failure<int>>());
        using (Assert.EnterMultipleScope())
        {
            Assert.That(successFunctionWasCalled, Is.False);
            var failureResult = (Failure<int>)resultAfterBind;
            Assert.That(failureResult.ErrorMessage, Is.EqualTo(testErrorMessage));
        }
    }

    [Test]
    public async Task When_AsyncFailureAndSuccessExpectsValue_Then_DoesNotRunsSuccessFunction()
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
        var resultAfterBind = await startingResult.Bind(SuccessFunction);

        // Assert
        Assert.That(resultAfterBind, Is.InstanceOf<Failure<int>>());
        using (Assert.EnterMultipleScope())
        {
            Assert.That(successFunctionWasCalled, Is.False);
            var failureResult = (Failure<int>)resultAfterBind;
            Assert.That(failureResult.ErrorMessage, Is.EqualTo(testErrorMessage));
        }
    }

    [Test]
    public async Task When_AsyncFailureAndSuccessExpectsValue_Then_DoesNotRunsSuccessFunctionAsync()
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
        var resultAfterBind = await startingResult.Bind(SuccessFunction, CancellationToken.None);

        // Assert
        Assert.That(resultAfterBind, Is.InstanceOf<Failure<int>>());
        using (Assert.EnterMultipleScope())
        {
            Assert.That(successFunctionWasCalled, Is.False);
            var failureResult = (Failure<int>)resultAfterBind;
            Assert.That(failureResult.ErrorMessage, Is.EqualTo(testErrorMessage));
        }
    }

    [Test]
    public void When_FailureAndSuccessExpectsNoParam_Then_DoesNotRunsSuccessFunction()
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
        var resultAfterBind = startingResult.Bind(SuccessFunction);

        // Assert
        Assert.That(resultAfterBind, Is.InstanceOf<Failure<int>>());
        using (Assert.EnterMultipleScope())
        {
            Assert.That(successFunctionWasCalled, Is.False);
            var failureResult = (Failure<int>)resultAfterBind;
            Assert.That(failureResult.ErrorMessage, Is.EqualTo(testErrorMessage));
        }
    }

    [Test]
    public async Task When_FailureAndSuccessExpectsNoParam_Then_DoesNotRunsSuccessFunctionAsync()
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
        var resultAfterBind = await startingResult.Bind(SuccessFunction
                                                      , CancellationToken.None);

        // Assert
        Assert.That(resultAfterBind, Is.InstanceOf<Failure<int>>());
        using (Assert.EnterMultipleScope())
        {
            Assert.That(successFunctionWasCalled, Is.False);
            var failureResult = (Failure<int>)resultAfterBind;
            Assert.That(failureResult.ErrorMessage, Is.EqualTo(testErrorMessage));
        }
    }

    [Test]
    public async Task When_AsyncFailureAndSuccessExpectsNoParam_Then_DoesNotRunsSuccessFunction()
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
        var resultAfterBind = await startingResult.Bind(SuccessFunction);

        // Assert
        Assert.That(resultAfterBind, Is.InstanceOf<Failure<int>>());
        using (Assert.EnterMultipleScope())
        {
            Assert.That(successFunctionWasCalled, Is.False);
            var failureResult = (Failure<int>)resultAfterBind;
            Assert.That(failureResult.ErrorMessage, Is.EqualTo(testErrorMessage));
        }
    }

    [Test]
    public async Task When_AsyncFailureAndSuccessExpectsNoParam_Then_DoesNotRunsSuccessFunctionAsync()
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
        var resultAfterBind = await startingResult.Bind(SuccessFunction
                                                      , CancellationToken.None);

        // Assert
        Assert.That(resultAfterBind, Is.InstanceOf<Failure<int>>());
        using (Assert.EnterMultipleScope())
        {
            Assert.That(successFunctionWasCalled, Is.False);
            var failureResult = (Failure<int>)resultAfterBind;
            Assert.That(failureResult.ErrorMessage, Is.EqualTo(testErrorMessage));
        }
    }
   
}