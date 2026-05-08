// ReSharper disable once CheckNamespace
namespace Hapdy.Monads.Results;

public static class Values
{
    public const int Test = 42;
    public const int ExpectedValue = 84;
    public const string ExpectedStringValue = "Expected String Value";

    public static bool FunctionWasCalled { get; set; }

    public static int? IntPassedToFunction { get; set; }
    public static int? StringPassedToFunction { get; set; }

    public static void Initialise()
    {
        FunctionWasCalled = false;
        IntPassedToFunction = null;
        StringPassedToFunction = null;
    }
}

public static class Errors
{
    public const string Message = "Value must be greater than 30";
    public const string ExpectedExceptionMessage = "Test exception message";
    public static readonly Exception ExceptionThrown = new(ExpectedExceptionMessage);
}

public static class Functions
{
    public static Func<int, IResult<int>> GetFunction()
    {
        return value =>
        {
            Values.FunctionWasCalled = true;
            Values.IntPassedToFunction = value;
            IResult<int> result = value > 30
                ? Success<int>.Create(value * 2)
                : Failure<int>.Create(Errors.Message);
            return result;
        };
    }

    public static Func<int, CancellationToken, Task<IResult<int>>> GetAsyncFunction()
    {
        return (value, _) =>
        {
            Values.FunctionWasCalled = true;
            Values.IntPassedToFunction = value;
            IResult<int> result = value > 30
                ? Success<int>.Create(value * 2)
                : Failure<int>.Create(Errors.Message);
            return Task.FromResult(result);
        };
    }

    public static Func<IResult<string>> GetNoParamFunction()
    {
        return () =>
        {
            Values.FunctionWasCalled = true;
            IResult<string> result = Success<string>.Create(Values.ExpectedStringValue);
            return result;
        };
    }

    public static Func<CancellationToken, Task<IResult<string>>> GetNoParamAsyncFunction()
    {
        return _ =>
        {
            Values.FunctionWasCalled = true;
            IResult<string> result = Success<string>.Create(Values.ExpectedStringValue);
            return Task.FromResult(result);
        };
    }

    public static Func<int, IResult<string>> GetExceptionFunction()
    {
        return _ => throw Errors.ExceptionThrown;
    }

    public static Func<int, CancellationToken, Task<IResult<string>>> GetExceptionAsyncFunction()
    {
        return (_, _) => throw Errors.ExceptionThrown;
    }

    public static Func<IResult<string>> GetNoParamExceptionFunction()
    {
        return () => throw Errors.ExceptionThrown;
    }

    public static Func<CancellationToken, Task<IResult<string>>> GetNoParamExceptionAsyncFunction()
    {
        return _ => throw Errors.ExceptionThrown;
    }
}