// ReSharper disable InconsistentNaming
// ReSharper disable CheckNamespace

using Hapdy.Monads.Results.Extensions;

namespace Hapdy.Monads.Results.Testing_Bind;

[TestFixture(TestOf = typeof(Success<>)
    , TestName = "Success"
    , Category = "1 - Bind")]
public class Bind_Success
{
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

    private static class Results
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value
        public static IResult<int> SuccessResult;
        public static Task<IResult<int>> AsyncSuccessResult;
#pragma warning restore CS0649 // Field is never assigned to, and will always have its default value
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    }

    private static class Assertions
    {
        public static void Successful<T, TReturn>(IResult<TReturn> result, T? valuePassed,
            T? expectedValuePassedToFunction, TReturn expectedResultValue)
        {
            Assert.That(result, Is.InstanceOf<Success<TReturn>>());
            using (Assert.EnterMultipleScope())
            {
                Assert.That(Values.FunctionWasCalled, Is.True);
                Assert.That(valuePassed, Is.EqualTo(expectedValuePassedToFunction));
                var successResult = (Success<TReturn>)result;
                Assert.That(successResult.Value, Is.EqualTo(expectedResultValue));
            }
        }

        public static void ExceptionThrown<TReturn>(IResult<TReturn> result)
        {
            Assert.That(result, Is.InstanceOf<ExceptionFailure<TReturn>>());
            using (Assert.EnterMultipleScope())
            {
                var exceptionFailure = (ExceptionFailure<TReturn>)result;
                Assert.That(exceptionFailure.Exception, Is.Not.Null);
                Assert.That(exceptionFailure.Exception, Is.EqualTo(Errors.ExceptionThrown));
                Assert.That(exceptionFailure.Exception.Message, Is.EqualTo(Errors.ExpectedExceptionMessage));
            }
        }
    }

    [Test]
    public void When_SuccessFunctionExpectsValue_Then_RunsSuccessFunction()
    {
        // Arrange
        var func = Functions.GetFunction();

        // Act
        var result = Results.SuccessResult.Bind(func);

        // Assert
        Assertions.Successful(result, Values.Test, Values.IntPassedToFunction, Values.ExpectedValue);
    }

    [Test]
    public async Task When_SuccessFunctionExpectsValue_Then_RunsSuccessFunctionAsync()
    {
        // Arrange
        var func = Functions.GetAsyncFunction();

        // Act
        var resultAfterBind = await Results.SuccessResult.Bind(func, CancellationToken.None);

        // Assert
        Assertions.Successful(resultAfterBind, Values.Test, Values.IntPassedToFunction, Values.ExpectedValue);
    }

    [Test]
    public async Task When_AsyncSuccessFunctionExpectsValue_Then_RunsSuccessFunction()
    {
        // Arrange
        var func = Functions.GetFunction();

        // Act
        var result = await Results.AsyncSuccessResult.Bind(func);

        // Assert
        Assertions.Successful(result, Values.Test, Values.IntPassedToFunction, Values.ExpectedValue);
    }

    [Test]
    public async Task When_AsyncSuccessFunctionExpectsValue_Then_RunsSuccessFunctionAsync()
    {
        // Arrange
        var func = Functions.GetAsyncFunction();

        // Act
        var result = await Results.AsyncSuccessResult.Bind(func, CancellationToken.None);

        // Assert
        Assertions.Successful(result, Values.Test, Values.IntPassedToFunction, Values.ExpectedValue);
    }

    [Test]
    public void When_SuccessFunctionExpectsNoValue_Then_RunsSuccessFunction()
    {
        // Arrange
        var func = Functions.GetNoParamFunction();

        // Act
        var result = Results.SuccessResult.Bind(func);

        // Assert
        Assertions.Successful(result, null, Values.StringPassedToFunction, Values.ExpectedStringValue);
    }

    [Test]
    public async Task When_SuccessFunctionExpectsNoValue_Then_RunsSuccessFunctionAsync()
    {
        // Arrange
        var func = Functions.GetNoParamAsyncFunction();

        // Act
        var result = await Results.SuccessResult.Bind(func, CancellationToken.None);

        // Assert
        Assertions.Successful(result, null, Values.StringPassedToFunction, Values.ExpectedStringValue);
    }

    [Test]
    public async Task When_AsyncSuccessFunctionExpectsNoValue_Then_RunsSuccessFunction()
    {
        // Arrange
        var func = Functions.GetNoParamFunction();

        // Act
        var result = await Results.AsyncSuccessResult.Bind(func);

        // Assert
        Assertions.Successful(result, null, Values.StringPassedToFunction, Values.ExpectedStringValue);
    }

    [Test]
    public async Task When_AsyncSuccessFunctionExpectsNoValue_Then_RunsSuccessFunctionAsync()
    {
        // Arrange
        var func = Functions.GetNoParamAsyncFunction();

        // Act
        var result = await Results.AsyncSuccessResult.Bind(func, CancellationToken.None);

        // Assert
        Assertions.Successful(result, null, Values.StringPassedToFunction, Values.ExpectedStringValue);
    }

    [Test]
    public void When_SuccessFunctionThrowsException_Then_ReturnsExceptionFailure()
    {
        // Arrange
        var func = Functions.GetExceptionFunction();

        //Act
        var result = Results.SuccessResult.Bind(func);

        // Assert
        Assertions.ExceptionThrown(result);
    }

    [Test]
    public async Task When_AsyncSuccessFunctionThrowsException_Then_ReturnsExceptionFailure()
    {
        // Arrange
        var func = Functions.GetExceptionFunction();

        //Act
        var result = await Results.AsyncSuccessResult.Bind(func);

        // Assert
        Assertions.ExceptionThrown(result);
    }

    [Test]
    public async Task When_SuccessFunctionThrowsException_Then_ReturnsExceptionFailureAsync()
    {
        // Arrange
        var func = Functions.GetExceptionAsyncFunction();

        //Act
        var result = await Results.SuccessResult.Bind(func, CancellationToken.None);

        // Assert
        Assertions.ExceptionThrown(result);
    }

    [Test]
    public async Task When_AsyncSuccessFunctionThrowsException_Then_ReturnsExceptionFailureAsync()
    {
        // Arrange
        var func = Functions.GetExceptionAsyncFunction();

        //Act
        var result = await Results.AsyncSuccessResult.Bind(func, CancellationToken.None);

        // Assert
        Assertions.ExceptionThrown(result);
    }

    [Test]
    public void When_SuccessFunctionExpectsNoValueThrowsException_Then_ReturnsExceptionFailure()
    {
        // Arrange
        var func = Functions.GetNoParamExceptionFunction();

        //Act
        var result = Results.SuccessResult.Bind(func);

        // Assert
        Assertions.ExceptionThrown(result);
    }

    [Test]
    public async Task When_SuccessFunctionExpectsNoValueThrowsException_Then_ReturnsExceptionFailureAsync()
    {
        // Arrange
        var func = Functions.GetNoParamExceptionAsyncFunction();

        //Act
        var result = await Results.SuccessResult.Bind(func, CancellationToken.None);

        // Assert
        Assertions.ExceptionThrown(result);
    }

    [Test]
    public async Task When_AsyncSuccessFunctionExpectsNoValueThrowsException_Then_ReturnsExceptionFailure()
    {
        // Arrange
        var func = Functions.GetNoParamExceptionFunction();

        //Act
        var result = await Results.AsyncSuccessResult.Bind(func);

        // Assert
        Assertions.ExceptionThrown(result);
    }

    [Test]
    public async Task When_AsyncSuccessFunctionExpectsNoValueThrowsException_Then_ReturnsExceptionFailureAsync()
    {
        // Arrange
        var func = Functions.GetNoParamExceptionAsyncFunction();

        //Act
        var result = await Results.AsyncSuccessResult.Bind(func, CancellationToken.None);

        // Assert
        Assertions.ExceptionThrown(result);
    }
}