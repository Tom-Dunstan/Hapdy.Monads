// ReSharper disable InconsistentNaming
// ReSharper disable CheckNamespace

using Hapdy.Monads.Results.Extensions;

namespace Hapdy.Monads.Results.Testing_Then;

[TestFixture(TestOf = typeof(Failure<>)
           , TestName = "ExceptionFailure"
           , Category = "2 - Then")]
public class Then_ExceptionFailure
{
    [SetUp]
    public void SetUp()
    {
        Values.Initialise();
        Results.ExceptionFailureResult      = ExceptionFailure<int>.Create(Errors.ExceptionThrown);
        Results.AsyncExceptionFailureResult = Task.FromResult(Results.ExceptionFailureResult);
    }

    [TearDown] public void TearDown() { Results.AsyncExceptionFailureResult.Dispose(); }

    private static class Results
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        public static IResult<int>       ExceptionFailureResult;
        public static Task<IResult<int>> AsyncExceptionFailureResult;
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    }

    private static class Assertions
    {
        public static void ExceptionFailure<TReturn>(IResult<TReturn> result)
        {
            Assert.That(result, Is.InstanceOf<ExceptionFailure<TReturn>>());
            using (Assert.EnterMultipleScope())
            {
                Assert.That(Values.FunctionWasCalled,   Is.False);
                Assert.That(Values.IntPassedToFunction, Is.Null);
                var exceptionResult = (ExceptionFailure<TReturn>)result;
                Assert.That(exceptionResult.Exception, Is.EqualTo(Errors.ExceptionThrown));
            }
        }
    }

    [Test]
    public void When_SuccessFunctionExpectsValue_Then_DoesNotRunSuccessFunction()
    {
        // Arrange
        var func = Functions.GetFunction();

        // Act
        var result = Results.ExceptionFailureResult.Then(func);

        // Assert
        Assertions.ExceptionFailure(result);
    }

    [Test]
    public async Task When_SuccessFunctionExpectsValue_Then_DoesNotRunSuccessFunctionAsync()
    {
        // Arrange
        var func = Functions.GetAsyncFunction();

        // Act
        var result = await Results.ExceptionFailureResult.Then(func, CancellationToken.None);

        // Assert
        Assertions.ExceptionFailure(result);
    }

    [Test]
    public async Task When_AsyncSuccessFunctionExpectsValue_Then_DoesNotRunSuccessFunction()
    {
        // Arrange
        var func = Functions.GetFunction();

        // Act
        var result = await Results.AsyncExceptionFailureResult.Then(func);

        // Assert
        Assertions.ExceptionFailure(result);
    }

    [Test]
    public async Task When_AsyncSuccessFunctionExpectsValue_Then_DoesNotRunSuccessFunctionAsync()
    {
        // Arrange
        var func = Functions.GetAsyncFunction();

        // Act
        var result = await Results.AsyncExceptionFailureResult.Then(func, CancellationToken.None);

        // Assert
        Assertions.ExceptionFailure(result);
    }

    [Test]
    public void When_SuccessFunctionExpectsNoValue_Then_DoesNotRunSuccessFunction()
    {
        // Arrange
        var func = Functions.GetNoParamFunctionToString();

        // Act
        var result = Results.ExceptionFailureResult.Then(func);

        // Assert
        Assertions.ExceptionFailure(result);
    }

    [Test]
    public async Task When_SuccessFunctionExpectsNoValue_Then_DoesNotRunSuccessFunctionAsync()
    {
        // Arrange
        var func = Functions.GetNoParamAsyncFunction();

        // Act
        var result = await Results.ExceptionFailureResult.Then(func, CancellationToken.None);

        // Assert
        Assertions.ExceptionFailure(result);
    }

    [Test]
    public async Task When_AsyncSuccessFunctionExpectsNoValue_Then_DoesNotRunSuccessFunction()
    {
        // Arrange
        var func = Functions.GetNoParamFunctionToString();

        // Act
        var result = await Results.AsyncExceptionFailureResult.Then(func);

        // Assert
        Assertions.ExceptionFailure(result);
    }

    [Test]
    public async Task When_AsyncSuccessFunctionExpectsNoValue_Then_DoesNotRunSuccessFunctionAsync()
    {
        // Arrange
        var func = Functions.GetNoParamAsyncFunction();

        // Act
        var result = await Results.AsyncExceptionFailureResult.Then(func, CancellationToken.None);

        // Assert
        Assertions.ExceptionFailure(result);
    }
}