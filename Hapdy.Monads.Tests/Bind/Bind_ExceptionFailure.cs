// ReSharper disable InconsistentNaming
// ReSharper disable CheckNamespace

using Hapdy.Monads.Results.Extensions;

namespace Hapdy.Monads.Results.Testing_Bind;

[TestFixture(TestOf = typeof(Failure<>)
           , TestName = "ExceptionFailure"
           , Category = "1 - Bind")]
public class Bind_ExceptionFailure
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
#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        public static IResult<int>       ExceptionFailureResult;
        public static Task<IResult<int>> AsyncExceptionFailureResult;
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
#pragma warning restore CS0649 // Field is never assigned to, and will always have its default value
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
        var result = Results.ExceptionFailureResult.Bind(func);

        // Assert
        Assertions.ExceptionFailure(result);
    }

    [Test]
    public async Task When_SuccessFunctionExpectsValue_Then_DoesNotRunSuccessFunctionAsync()
    {
        // Arrange
        var func = Functions.GetAsyncFunction();

        // Act
        var result = await Results.ExceptionFailureResult.Bind(func, CancellationToken.None);

        // Assert
        Assertions.ExceptionFailure(result);
    }

    [Test]
    public async Task When_AsyncSuccessFunctionExpectsValue_Then_DoesNotRunSuccessFunction()
    {
        // Arrange
        var func = Functions.GetFunction();

        // Act
        var result = await Results.AsyncExceptionFailureResult.Bind(func);

        // Assert
        Assertions.ExceptionFailure(result);
    }

    [Test]
    public async Task When_AsyncSuccessFunctionExpectsValue_Then_DoesNotRunSuccessFunctionAsync()
    {
        // Arrange
        var func = Functions.GetAsyncFunction();

        // Act
        var result = await Results.AsyncExceptionFailureResult.Bind(func, CancellationToken.None);

        // Assert
        Assertions.ExceptionFailure(result);
    }

    [Test]
    public void When_SuccessFunctionExpectsNoValue_Then_DoesNotRunSuccessFunction()
    {
        // Arrange
        var func = Functions.GetNoParamFunctionToString();

        // Act
        var result = Results.ExceptionFailureResult.Bind(func);

        // Assert
        Assertions.ExceptionFailure(result);
    }

    [Test]
    public async Task When_SuccessFunctionExpectsNoValue_Then_DoesNotRunSuccessFunctionAsync()
    {
        // Arrange
        var func = Functions.GetNoParamAsyncFunction();

        // Act
        var result = await Results.ExceptionFailureResult.Bind(func, CancellationToken.None);

        // Assert
        Assertions.ExceptionFailure(result);
    }

    [Test]
    public async Task When_AsyncSuccessFunctionExpectsNoValue_Then_DoesNotRunSuccessFunction()
    {
        // Arrange
        var func = Functions.GetNoParamFunctionToString();

        // Act
        var result = await Results.AsyncExceptionFailureResult.Bind(func);

        // Assert
        Assertions.ExceptionFailure(result);
    }

    [Test]
    public async Task When_AsyncSuccessFunctionExpectsNoValue_Then_DoesNotRunSuccessFunctionAsync()
    {
        // Arrange
        var func = Functions.GetNoParamAsyncFunction();

        // Act
        var result = await Results.AsyncExceptionFailureResult.Bind(func, CancellationToken.None);

        // Assert
        Assertions.ExceptionFailure(result);
    }
}