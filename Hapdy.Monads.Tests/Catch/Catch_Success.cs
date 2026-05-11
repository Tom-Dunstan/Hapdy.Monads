// ReSharper disable InconsistentNaming
// ReSharper disable CheckNamespace

namespace Hapdy.Monads.Results.Testing_Bind;

[TestFixture(TestOf = typeof(Success<>)
           , TestName = "Success"
           , Category = "6 - Catch")]
[TestFixture]
public class Catch_Success
{
    [SetUp] public static void SetUp() { Values.FunctionRan = false; }


    private static class Values
    {
        public static readonly IResult<int> Success = Success<int>.Create(42);
        public static          bool         FunctionRan;
    }

    private static class Functions
    {
        public static Func<IExceptionFailure<int>, IResult<int>> GetExceptionFunction()
        {
            return exceptionFailure =>
                   {
                       Values.FunctionRan = true;
                       var result = Failure<int>.Create(exceptionFailure.ErrorMessage);
                       return result;
                   };
        }

        public static Func<IExceptionFailure<int>, CancellationToken, Task<IResult<int>>> GetExceptionFunctionAsync()
        {
            return (exceptionFailure, _) =>
                   {
                       Values.FunctionRan = true;
                       IResult<int> result = Failure<int>.Create(exceptionFailure.ErrorMessage);
                       return Task.FromResult(result);
                   };
        }
    }

    private static class Assertions
    {
    }

    [Test]
    public void When__Then_()
    {
        // Arrange


        // Act

        // Assert
    }
}