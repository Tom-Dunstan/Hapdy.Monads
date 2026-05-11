// ReSharper disable InconsistentNaming
// ReSharper disable CheckNamespace

using Hapdy.Monads.Results.Extensions;

namespace Hapdy.Monads.Results.Testing_Bind;

[TestFixture(TestOf = typeof(Failure<>)
           , TestName = "Failure"
           , Category = "1 - Bind")]
public class Bind_Failure
{
    [SetUp]
    public void Setup()
    {
        Values.Initialise();
        Results.FailureResult      = Failure<int>.Create(Errors.ExpectedExceptionMessage);
        Results.AsyncFailureResult = Task.FromResult(Results.FailureResult);
    }

    [TearDown] public void TearDown() { Results.AsyncFailureResult.Dispose(); }

    private static class Results
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value
        public static IResult<int>       FailureResult;
        public static Task<IResult<int>> AsyncFailureResult;
#pragma warning restore CS0649 // Field is never assigned to, and will always have its default value
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    }

    private static class Assertions
    {
        public static void Failed<TReturn>(IResult<TReturn> result)
        {
            Assert.That(result, Is.InstanceOf<Failure<TReturn>>());
            using (Assert.EnterMultipleScope())
            {
                Assert.That(Values.FunctionWasCalled, Is.False);
                var failureResult = (Failure<TReturn>)result;
                Assert.That(failureResult.ErrorMessage, Is.EqualTo(Errors.ExpectedExceptionMessage));
            }
        }
    }

    [Test]
    public void When_FailureAndSuccessExpectsValue_Then_DoesNotRunSuccessFunction()
    {
        // Arrange
        var func = Functions.GetFunction();

        // Act
        var result = Results.FailureResult.Bind(func);

        // Assert
        Assertions.Failed(result);
    }

    [Test]
    public async Task When_FailureAndSuccessExpectsValue_Then_DoesNotRunSuccessFunctionAsync()
    {
        // Arrange
        var func = Functions.GetAsyncFunction();

        // Act
        var result = await Results.FailureResult.Bind(func, CancellationToken.None);

        // Assert
        Assertions.Failed(result);
    }

    [Test]
    public async Task When_AsyncFailureAndSuccessExpectsValue_Then_DoesNotRunSuccessFunction()
    {
        // Arrange
        var func = Functions.GetFunction();

        // Act
        var result = await Results.AsyncFailureResult.Bind(func);

        // Assert
        Assertions.Failed(result);
    }

    [Test]
    public async Task When_AsyncFailureAndSuccessExpectsValue_Then_DoesNotRunSuccessFunctionAsync()
    {
        // Arrange
        var func = Functions.GetAsyncFunction();

        // Act
        var result = await Results.AsyncFailureResult.Bind(func, CancellationToken.None);

        // Assert
        Assertions.Failed(result);
    }

    [Test]
    public void When_FailureAndSuccessExpectsNoParam_Then_DoesNotRunSuccessFunction()
    {
        // Arrange
        var func = Functions.GetNoParamFunctionToString();

        // Act
        var result = Results.FailureResult.Bind(func);

        // Assert
        Assertions.Failed(result);
    }

    [Test]
    public async Task When_FailureAndSuccessExpectsNoParam_Then_DoesNotRunSuccessFunctionAsync()
    {
        // Arrange
        var func = Functions.GetNoParamAsyncFunction();

        // Act
        var result = await Results.FailureResult.Bind(func, CancellationToken.None);

        // Assert
        Assertions.Failed(result);
    }

    [Test]
    public async Task When_AsyncFailureAndSuccessExpectsNoParam_Then_DoesNotRunSuccessFunction()
    {
        // Arrange
        var func = Functions.GetNoParamFunctionToString();

        // Act
        var result = await Results.AsyncFailureResult.Bind(func);

        // Assert
        Assertions.Failed(result);
    }

    [Test]
    public async Task When_AsyncFailureAndSuccessExpectsNoParam_Then_DoesNotRunSuccessFunctionAsync()
    {
        // Arrange
        var func = Functions.GetNoParamAsyncFunction();

        // Act
        var result = await Results.AsyncFailureResult.Bind(func, CancellationToken.None);

        // Assert
        Assertions.Failed(result);
    }
}