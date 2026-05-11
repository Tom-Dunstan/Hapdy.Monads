// ReSharper disable InconsistentNaming
// ReSharper disable CheckNamespace

using Hapdy.Monads.Results.Extensions;

namespace Hapdy.Monads.Results.Testing_Tap;

[TestFixture(TestOf = typeof(Failure<>)
    , TestName = "Failure"
    , Category = "3 - Tap")]
public class Tap_Failure
{
    // private static bool Values.FunctionWasCalled;
    //
    // private static class Values
    // {
    //     public const string ExpectedValue = "Test Failure Message";
    //
    //     public static int? IntPassedToAction;
    // }
    //
    // private static class Functions
    // {
    //     public static Action<int> GetAction()
    //     {
    //         return value =>
    //         {
    //             Values.FunctionWasCalled = true;
    //             Values.IntPassedToAction = value;
    //         };
    //     }
    //
    //     public static Func<int, CancellationToken, Task> GetAsyncAction()
    //     {
    //         return (value, _) =>
    //         {
    //             Values.FunctionWasCalled = true;
    //             Values.IntPassedToAction = value;
    //             return Task.CompletedTask;
    //         };
    //     }
    //
    //     public static Action GetNoParamAction()
    //     {
    //         return () => { Values.FunctionWasCalled = true; };
    //     }
    //
    //     public static Func<CancellationToken, Task> GetNoParamAsyncAction()
    //     {
    //         return _ =>
    //         {
    //             Values.FunctionWasCalled = true;
    //             return Task.CompletedTask;
    //         };
    //     }
    // }
    //

    private static class Results
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        public static IResult<int> FailureResult;
        public static Task<IResult<int>> AsyncFailureResult;
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    }
    
    private static class Assertions
    {
        public static void Failure(
            IResult<int> result
            , IResult<int> originalResult)
        {
            Assert.That(result, Is.InstanceOf<Failure<int>>());
            using (Assert.EnterMultipleScope())
            {
                Assert.That(Values.FunctionWasCalled, Is.False);
                Assert.That(Values.IntPassedToFunction, Is.Null);
                Assert.That(result, Is.EqualTo(originalResult));
            }
        }
    }

    [SetUp]
    public void SetUp()
    {
        Values.Initialise();
        Results.FailureResult = Failure<int>.Create(Errors.ExpectedExceptionMessage);
        Results.AsyncFailureResult = Task.FromResult(Results.FailureResult);
    }

    [TearDown]
    public void TearDown()
    {
        Results.AsyncFailureResult.Dispose();
    }

    [Test]
    public void When_SuccessFunctionExpectsValue_Then_DoesNotRunSuccessFunction()
    {
        // Arrange
        var func = Functions.GetAction();

        // Act
        var result = Results.FailureResult.Tap(func);

        // Assert
        Assertions.Failure(result, Results.FailureResult);
    }

    [Test]
    public async Task When_SuccessFunctionExpectsValue_Then_DoesNotRunSuccessFunctionAsync()
    {
        // Arrange
        var func = Functions.GetAsyncAction();

        // Act
        var result = await Results.FailureResult.Tap(func, CancellationToken.None);

        // Assert
        Assertions.Failure(result, Results.FailureResult);
    }

    [Test]
    public async Task When_AsyncSuccessFunctionExpectsValue_Then_DoesNotRunSuccessFunction()
    {
        // Arrange
        var func = Functions.GetAction();

        // Act
        var result = await Results.AsyncFailureResult.Tap(func);

        // Assert
        Assertions.Failure(result, Results.FailureResult);
    }

    [Test]
    public async Task When_AsyncSuccessFunctionExpectsValue_Then_DoesNotRunSuccessFunctionAsync()
    {
        // Arrange
        var func = Functions.GetAsyncAction();

        // Act
        var result = await Results.AsyncFailureResult.Tap(func, CancellationToken.None);

        // Assert
        Assertions.Failure(result, Results.FailureResult);
    }

    [Test]
    public void When_SuccessFunctionExpectsNoValue_Then_DoesNotRunSuccessFunction()
    {
        // Arrange
        var func = Functions.GetNoParamAction();

        // Act
        var result = Results.FailureResult.Tap(func);

        // Assert
        Assertions.Failure(result, Results.FailureResult);
    }

    [Test]
    public async Task When_SuccessFunctionExpectsNoValue_Then_DoesNotRunSuccessFunctionAsync()
    {
        // Arrange
        var func = Functions.GetNoParamAsyncAction();

        // Act
        var result = await Results.FailureResult.Tap(func, CancellationToken.None);

        // Assert
        Assertions.Failure(result, Results.FailureResult);
    }

    [Test]
    public async Task When_AsyncSuccessFunctionExpectsNoValue_Then_DoesNotRunSuccessFunction()
    {
        // Arrange
        var func = Functions.GetNoParamAction();

        // Act
        var result = await Results.AsyncFailureResult.Tap(func);

        // Assert
        Assertions.Failure(result, Results.FailureResult);
    }

    [Test]
    public async Task When_AsyncSuccessFunctionExpectsNoValue_Then_DoesNotRunSuccessFunctionAsync()
    {
        // Arrange
        var func = Functions.GetNoParamAsyncAction();

        // Act
        var result = await Results.AsyncFailureResult.Tap(func, CancellationToken.None);

        // Assert
        Assertions.Failure(result, Results.FailureResult);
    }
}