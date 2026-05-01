// ReSharper disable InconsistentNaming
// ReSharper disable CheckNamespace

using Hapdy.Monads.Results.Extensions;

namespace Hapdy.Monads.Results.Testing_Tap;

[TestFixture(TestOf = typeof(Failure<>)
           , TestName = "Failure"
           , Category = "3 - Tap")]
public class Tap_Failure
{
    [SetUp] public void Setup() { }

    [Test]
    public void When_SuccessFunctionExpectsValue_Then_DoesNotRunSuccessFunction()
    {
        // Arrange
        const string testErrorMessage         = "Testing binding failure result.";
        var          startingResult           = Failure<int>.Create(testErrorMessage);
        var          successFunctionWasCalled = false;

        // ReSharper disable once MoveLocalFunctionAfterJumpStatement
        void SuccessFunction(int value) { successFunctionWasCalled = true; }

        // Act
        var resultAfterBind = startingResult.Tap(SuccessFunction);

        // Assert
        Assert.That(resultAfterBind, Is.InstanceOf<Failure<int>>());
        using (Assert.EnterMultipleScope())
        {
            Assert.That(successFunctionWasCalled, Is.False);
            var failureResult = (Failure<int>)resultAfterBind;
            Assert.That(failureResult.ErrorMessage, Is.EqualTo(testErrorMessage));
            Assert.That(resultAfterBind,            Is.EqualTo(startingResult));
        }
    }

    [Test]
    public async Task When_SuccessFunctionExpectsValue_Then_DoesNotRunSuccessFunctionAsync()
    {
        // Arrange
        const string testErrorMessage         = "Testing binding failure result.";
        var          startingResult           = Failure<int>.Create(testErrorMessage);
        var          successFunctionWasCalled = false;

        // ReSharper disable once MoveLocalFunctionAfterJumpStatement
        Task SuccessFunction(int value, CancellationToken cancellationToken)
        {
            successFunctionWasCalled = true;
            return Task.CompletedTask;
        }

        // Act
        var resultAfterBind = await startingResult.Tap(SuccessFunction, CancellationToken.None);

        // Assert
        Assert.That(resultAfterBind, Is.InstanceOf<Failure<int>>());
        using (Assert.EnterMultipleScope())
        {
            Assert.That(successFunctionWasCalled, Is.False);
            var failureResult = (Failure<int>)resultAfterBind;
            Assert.That(failureResult.ErrorMessage, Is.EqualTo(testErrorMessage));
            Assert.That(resultAfterBind,            Is.EqualTo(startingResult));
        }
    }

    [Test]
    public async Task When_AsyncSuccessFunctionExpectsValue_Then_DoesNotRunSuccessFunction()
    {
        // Arrange
        const string testErrorMessage         = "Testing binding failure result.";
        IResult<int> syncResult               = Failure<int>.Create(testErrorMessage);
        var          startingResult           = Task.FromResult(syncResult);
        var          successFunctionWasCalled = false;

        // ReSharper disable once MoveLocalFunctionAfterJumpStatement
        void SuccessFunction(int value) { successFunctionWasCalled = true; }

        // Act
        var resultAfterBind = await startingResult.Tap(SuccessFunction);

        // Assert
        Assert.That(resultAfterBind, Is.InstanceOf<Failure<int>>());
        using (Assert.EnterMultipleScope())
        {
            Assert.That(successFunctionWasCalled, Is.False);
            var failureResult = (Failure<int>)resultAfterBind;
            Assert.That(failureResult.ErrorMessage, Is.EqualTo(testErrorMessage));
            Assert.That(resultAfterBind,            Is.EqualTo(syncResult));
        }
    }

    [Test]
    public async Task When_AsyncSuccessFunctionExpectsValue_Then_DoesNotRunSuccessFunctionAsync()
    {
        // Arrange
        const string testErrorMessage         = "Testing binding failure result.";
        IResult<int> syncResult               = Failure<int>.Create(testErrorMessage);
        var          startingResult           = Task.FromResult(syncResult);
        var          successFunctionWasCalled = false;

        // ReSharper disable once MoveLocalFunctionAfterJumpStatement
        Task SuccessFunction(int value, CancellationToken cancellationToken)
        {
            successFunctionWasCalled = true;
            return Task.CompletedTask;
        }

        // Act
        var resultAfterBind = await startingResult.Tap(SuccessFunction, CancellationToken.None);

        // Assert
        Assert.That(resultAfterBind, Is.InstanceOf<Failure<int>>());
        using (Assert.EnterMultipleScope())
        {
            Assert.That(successFunctionWasCalled, Is.False);
            var failureResult = (Failure<int>)resultAfterBind;
            Assert.That(failureResult.ErrorMessage, Is.EqualTo(testErrorMessage));
            Assert.That(resultAfterBind,            Is.EqualTo(syncResult));
        }
    }

    [Test]
    public void When_SuccessFunctionExpectsNoValue_Then_DoesNotRunSuccessFunction()
    {
        // Arrange
        const string testErrorMessage         = "Testing binding failure result.";
        var          startingResult           = Failure<int>.Create(testErrorMessage);
        var          successFunctionWasCalled = false;

        // ReSharper disable once MoveLocalFunctionAfterJumpStatement
        void SuccessFunction() { successFunctionWasCalled = true; }

        // Act
        var resultAfterBind = startingResult.Tap(SuccessFunction);

        // Assert
        Assert.That(resultAfterBind, Is.InstanceOf<Failure<int>>());
        using (Assert.EnterMultipleScope())
        {
            Assert.That(successFunctionWasCalled, Is.False);
            var failureResult = (Failure<int>)resultAfterBind;
            Assert.That(failureResult.ErrorMessage, Is.EqualTo(testErrorMessage));
            Assert.That(resultAfterBind,            Is.EqualTo(startingResult));
        }
    }

    [Test]
    public async Task When_SuccessFunctionExpectsNoValue_Then_DoesNotRunSuccessFunctionAsync()
    {
        // Arrange
        const string testErrorMessage         = "Testing binding failure result.";
        var          startingResult           = Failure<int>.Create(testErrorMessage);
        var          successFunctionWasCalled = false;

        // ReSharper disable once MoveLocalFunctionAfterJumpStatement
        Task SuccessFunction(CancellationToken cancellationToken)
        {
            successFunctionWasCalled = true;
            return Task.CompletedTask;
        }

        // Act
        var resultAfterBind = await startingResult.Tap(SuccessFunction
                                                     , CancellationToken.None);

        // Assert
        Assert.That(resultAfterBind, Is.InstanceOf<Failure<int>>());
        using (Assert.EnterMultipleScope())
        {
            Assert.That(successFunctionWasCalled, Is.False);
            var failureResult = (Failure<int>)resultAfterBind;
            Assert.That(failureResult.ErrorMessage, Is.EqualTo(testErrorMessage));
            Assert.That(resultAfterBind,            Is.EqualTo(startingResult));
        }
    }

    [Test]
    public async Task When_AsyncSuccessFunctionExpectsNoValue_Then_DoesNotRunSuccessFunction()
    {
        // Arrange
        const string testErrorMessage         = "Testing binding failure result.";
        IResult<int> syncResult               = Failure<int>.Create(testErrorMessage);
        var          startingResult           = Task.FromResult(syncResult);
        var          successFunctionWasCalled = false;

        // ReSharper disable once MoveLocalFunctionAfterJumpStatement
        void SuccessFunction() { successFunctionWasCalled = true; }

        // Act
        var resultAfterBind = await startingResult.Tap(SuccessFunction);

        // Assert
        Assert.That(resultAfterBind, Is.InstanceOf<Failure<int>>());
        using (Assert.EnterMultipleScope())
        {
            Assert.That(successFunctionWasCalled, Is.False);
            var failureResult = (Failure<int>)resultAfterBind;
            Assert.That(failureResult.ErrorMessage, Is.EqualTo(testErrorMessage));
            Assert.That(resultAfterBind,            Is.EqualTo(syncResult));
        }
    }

    [Test]
    public async Task When_AsyncSuccessFunctionExpectsNoValue_Then_DoesNotRunSuccessFunctionAsync()
    {
        // Arrange
        const string testErrorMessage         = "Testing binding failure result.";
        IResult<int> syncResult               = Failure<int>.Create(testErrorMessage);
        var          startingResult           = Task.FromResult(syncResult);
        var          successFunctionWasCalled = false;

        // ReSharper disable once MoveLocalFunctionAfterJumpStatement
        Task SuccessFunction(CancellationToken cancellationToken)
        {
            successFunctionWasCalled = true;
            return Task.CompletedTask;
        }

        // Act
        var resultAfterBind = await startingResult.Tap(SuccessFunction
                                                     , CancellationToken.None);

        // Assert
        Assert.That(resultAfterBind, Is.InstanceOf<Failure<int>>());
        using (Assert.EnterMultipleScope())
        {
            Assert.That(successFunctionWasCalled, Is.False);
            var failureResult = (Failure<int>)resultAfterBind;
            Assert.That(failureResult.ErrorMessage, Is.EqualTo(testErrorMessage));
            Assert.That(resultAfterBind,            Is.EqualTo(syncResult));
        }
    }
}