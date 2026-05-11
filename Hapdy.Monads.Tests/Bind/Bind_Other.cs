// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming

using Hapdy.Monads.Results.Extensions;

namespace Hapdy.Monads.Results.Testing_Bind;

[TestFixture(TestOf = typeof(IResult<>)
           , TestName = "Other"
           , Category = "1 - Bind")]
[TestFixture]
public class Bind_Other
{
    [SetUp] public void SetUp() { Values.Initialise(); }

    private sealed record TestResult<T> : IResult<T>
    {
        public bool IsSuccess => false;
        public bool IsFailure => false;
    }

    private static class Results
    {
        public static readonly IResult<int> UnknownResult = new TestResult<int>();
    }

    private static class Assertions
    {
        public static void Unknown(IResult<int> result)
        {
            Assert.That(result,                   Is.InstanceOf<ExceptionFailure<int>>());
            Assert.That(Values.FunctionWasCalled, Is.False);
            var exceptionFailure = (ExceptionFailure<int>)result;
            Assert.That(exceptionFailure.Exception, Is.InstanceOf<ArgumentOutOfRangeException>());
        }
    }

    [Test]
    public void When_UnknownResultType_Then_ReturnExceptionFailure()
    {
        // Arrange
        var func = Functions.GetFunction();

        // Act
        var result = Results.UnknownResult.Bind(func);

        // Assert
        Assertions.Unknown(result);
    }

    [Test]
    public async Task When_UnknownResultType_Then_ReturnExceptionFailureAsync()
    {
        // Arrange
        var func = Functions.GetAsyncFunction();

        // Act
        var result = await Results.UnknownResult.Bind(func, CancellationToken.None);

        // Assert
        Assertions.Unknown(result);
    }
}