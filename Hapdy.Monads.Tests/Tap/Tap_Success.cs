// ReSharper disable InconsistentNaming
// ReSharper disable CheckNamespace

using Hapdy.Monads.Results.Extensions;

namespace Hapdy.Monads.Results.Testing_Tap;

[TestFixture(TestOf = typeof(Success<>)
    , TestName = "Success"
    , Category = "3 - Tap")]
public class Tap_Success
{
    private static class Results
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        public static IResult<int> SuccessResult;
        public static Task<IResult<int>> AsyncSuccessResult;
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    }

    private static class Assertions
    {
        public static void Successful(
            IResult<int> result
            , IResult<int> originalResult
            , int? valuePassed
            , int? expectedValuePassedToFunction)
        {
            Assert.That(result, Is.InstanceOf<Success<int>>());
            using (Assert.EnterMultipleScope())
            {
                Assert.That(Values.FunctionWasCalled, Is.True);
                Assert.That(valuePassed, Is.EqualTo(expectedValuePassedToFunction));
                Assert.That(result, Is.EqualTo(originalResult));
            }
        }

        public static void ExceptionThrown(IResult<int> result)
        {
            Assert.That(result, Is.InstanceOf<ExceptionFailure<int>>());
            using (Assert.EnterMultipleScope())
            {
                var exceptionFailure = (ExceptionFailure<int>)result;
                Assert.That(Values.IntPassedToFunction, Is.Null);
                Assert.That(exceptionFailure.Exception, Is.EqualTo(Errors.ExceptionThrown));
                Assert.That(exceptionFailure.Exception.Message, Is.EqualTo(Errors.ExpectedExceptionMessage));
            }
        }
    }

    [SetUp]
    public void SetUp()
    {
        Values.Initialise();
        Results.SuccessResult = Success<int>.Create(Values.Test);
        Results.AsyncSuccessResult = Task.FromResult(Results.SuccessResult);
    }

    [TearDown]
    public void TearDown()
    {
        Results.AsyncSuccessResult.Dispose();
    }

    [Test]
    public void When_SuccessFunctionExpectsValue_Then_RunsSuccessFunction()
    {
        // Arrange
        var func = Functions.GetAction();

        // Act
        var result = Results.SuccessResult.Tap(func);

        // Assert
        Assertions.Successful(result
            , Results.SuccessResult
            , Values.IntPassedToFunction
            , Values.Test);
    }

    [Test]
    public async Task When_SuccessFunctionExpectsValue_Then_RunsSuccessFunctionAsync()
    {
        // Arrange
        var func = Functions.GetAsyncAction();

        // Act
        var result = await Results.SuccessResult.Tap(func, CancellationToken.None);

        // Assert
        Assertions.Successful(result
            , Results.SuccessResult
            , Values.IntPassedToFunction
            , Values.Test);
    }

    [Test]
    public async Task When_AsyncSuccessFunctionExpectsValue_Then_RunsSuccessFunction()
    {
        // Arrange
        var func = Functions.GetAction();

        // Act
        var result = await Results.AsyncSuccessResult.Tap(func);

        // Assert
        Assertions.Successful(result
            , Results.SuccessResult
            , Values.IntPassedToFunction
            , Values.Test);
    }

    [Test]
    public async Task When_AsyncSuccessFunctionExpectsValue_Then_RunsSuccessFunctionAsync()
    {
        // Arrange
        var func = Functions.GetAsyncAction();

        // Act
        var result = await Results.AsyncSuccessResult.Tap(func, CancellationToken.None);

        // Assert
        Assertions.Successful(result
            , Results.SuccessResult
            , Values.IntPassedToFunction
            , Values.Test);
    }

    [Test]
    public void When_SuccessFunctionExpectsNoValueAndReturnsDifferentType_Then_RunsSuccessFunction()
    {
        // Arrange
        var func = Functions.GetNoParamAction();

        // Act
        var result = Results.SuccessResult.Tap(func);

        // Assert
        Assertions.Successful(result
            , Results.SuccessResult
            , Values.IntPassedToFunction
            , null);
    }

    [Test]
    public async Task When_SuccessFunctionExpectsNoValue_Then_RunsSuccessFunctionAsync()
    {
        // Arrange
        var func = Functions.GetNoParamAsyncAction();

        // Act
        var result = await Results.SuccessResult.Tap(func, CancellationToken.None);

        // Assert
        Assertions.Successful(result
            , Results.SuccessResult
            , Values.IntPassedToFunction
            , null);
    }

    [Test]
    public async Task When_AsyncSuccessFunctionExpectsNoValue_Then_RunsSuccessFunction()
    {
        // Arrange
        var func = Functions.GetNoParamAction();

        // Act
        var result = await Results.AsyncSuccessResult.Tap(func);

        // Assert
        Assertions.Successful(result
            , Results.SuccessResult
            , Values.IntPassedToFunction
            , null);
    }

    [Test]
    public async Task When_AsyncSuccessFunctionExpectsNoValue_Then_RunsSuccessFunctionAsync()
    {
        // Arrange
        var func = Functions.GetNoParamAsyncAction();

        // Act
        var result = await Results.AsyncSuccessResult.Tap(func, CancellationToken.None);

        // Assert
        Assertions.Successful(result
            , Results.SuccessResult
            , Values.IntPassedToFunction
            , null);
    }

    [Test]
    public void When_SuccessFunctionThrowsException_Then_ReturnsExceptionFailure()
    {
        // Arrange
        var func = Functions.GetExceptionAction();

        //Act
        var result = Results.SuccessResult.Tap(func);

        // Assert
        Assertions.ExceptionThrown(result);
    }

    [Test]
    public async Task When_AsyncSuccessFunctionThrowsException_Then_ReturnsExceptionFailure()
    {
        // Arrange
        var func = Functions.GetExceptionAction();

        //Act
        var result = await Results.AsyncSuccessResult.Tap(func);

        // Assert
        Assertions.ExceptionThrown(result);
    }

    [Test]
    public async Task When_AsyncSuccessFunctionThrowsException_Then_ReturnsExceptionFailureAsync()
    {
        // Arrange
        var func = Functions.GetExceptionAsyncAction();

        //Act
        var result = await Results.AsyncSuccessResult.Tap(func, CancellationToken.None);

        // Assert
        Assertions.ExceptionThrown(result);
    }

    [Test]
    public async Task When_SuccessFunctionThrowsException_Then_ReturnsExceptionFailureAsync()
    {
        // Arrange
        var func = Functions.GetExceptionAsyncAction();

        //Act
        var result = await Results.SuccessResult.Tap(func, CancellationToken.None);

        // Assert
        Assertions.ExceptionThrown(result);
    }

    [Test]
    public void When_SuccessFunctionExpectsNoValueThrowsException_Then_ReturnsExceptionFailure()
    {
        // Arrange
        var func = Functions.GetNoParamExceptionAction();

        //Act
        var result = Results.SuccessResult.Tap(func);

        // Assert
        Assertions.ExceptionThrown(result);
    }

    [Test]
    public async Task When_SuccessFunctionExpectsNoValueThrowsException_Then_ReturnsExceptionFailureAsync()
    {
        // Arrange
        var func = Functions.GetNoParamExceptionAsyncAction();

        //Act
        var result = await Results.SuccessResult.Tap(func, CancellationToken.None);

        // Assert
        Assertions.ExceptionThrown(result);
    }

    [Test]
    public async Task When_AsyncSuccessFunctionExpectsNoValueThrowsException_Then_ReturnsExceptionFailure()
    {
        // Arrange
        var func = Functions.GetNoParamExceptionAction();

        //Act
        var result = await Results.AsyncSuccessResult.Tap(func);

        // Assert
        Assertions.ExceptionThrown(result);
    }

    [Test]
    public async Task When_AsyncSuccessFunctionExpectsNoValueThrowsException_Then_ReturnsExceptionFailureAsync()
    {
        // Arrange
        var func = Functions.GetNoParamExceptionAsyncAction();

        //Act
        var result = await Results.AsyncSuccessResult.Tap(func, CancellationToken.None);

        // Assert
        Assertions.ExceptionThrown(result);
    }
}