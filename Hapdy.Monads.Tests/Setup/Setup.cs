// ReSharper disable once CheckNamespace

namespace Hapdy.Monads.Results;

public static class Values
{
    public const int Test = 42;
    public const int ExpectedValue = 84;
    public static readonly int? TestNullable = 42;
    public static readonly int? TestNull = null;
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
    public const string MessageNullable = "Value must be greater than 30";
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

    public static Func<IResult<string>> GetNoParamFunctionToString()
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

    public static Func<int, IResult<TReturn>> GetExceptionFunction<TReturn>()
    {
        return _ => throw Errors.ExceptionThrown;
    }

    public static Func<int, CancellationToken, Task<IResult<TReturn>>> GetExceptionAsyncFunction<TReturn>()
    {
        return (_, _) => throw Errors.ExceptionThrown;
    }

    public static Func<IResult<TReturn>> GetNoParamExceptionFunction<TReturn>()
    {
        return () => throw Errors.ExceptionThrown;
    }

    public static Func<CancellationToken, Task<IResult<TReturn>>> GetNoParamExceptionAsyncFunction<TReturn>()
    {
        return _ => throw Errors.ExceptionThrown;
    }

    public static Action<int> GetAction()
    {
        return value =>
        {
            Values.FunctionWasCalled = true;
            Values.IntPassedToFunction = value;
        };
    }

    public static Func<int, CancellationToken, Task> GetAsyncAction()
    {
        return (value, _) =>
        {
            Values.FunctionWasCalled = true;
            Values.IntPassedToFunction = value;
            return Task.CompletedTask;
        };
    }

    public static Action GetNoParamAction()
    {
        return () => { Values.FunctionWasCalled = true; };
    }

    public static Func<CancellationToken, Task> GetNoParamAsyncAction()
    {
        return _ =>
        {
            Values.FunctionWasCalled = true;
            return Task.CompletedTask;
        };
    }

    public static Action<int> GetExceptionAction()
    {
        return _ => throw Errors.ExceptionThrown;
    }

    public static Func<int, CancellationToken, Task> GetExceptionAsyncAction()
    {
        return (_, _) => throw Errors.ExceptionThrown;
    }

    public static Action GetNoParamExceptionAction()
    {
        return () => throw Errors.ExceptionThrown;
    }

    public static Func<CancellationToken, Task> GetNoParamExceptionAsyncAction()
    {
        return _ => throw Errors.ExceptionThrown;
    }
    
    public static Func<int?, IResult<int>> GetNullableFunction()
     {
         return value =>
         {
             Values.FunctionWasCalled = true;
             return value == null
                 ? Failure<int>.Create(Errors.MessageNullable)
                 : Success<int>.Create(value.Value * 2);
         };
     }

     public static Func<int?, CancellationToken, Task<IResult<int>>> GetNullableFunctionAsync()
     {
         return (value, _) =>
         {
             Values.FunctionWasCalled = true;
             IResult<int> result = value == null
                 ? Failure<int>.Create(Errors.MessageNullable)
                 : Success<int>.Create(value.Value * 2);
             return Task.FromResult(result);
         };
     }

     public static Func<int?, IResult<int>> GetNullableExceptionFunction()
     {
         // ReSharper disable once UnusedParameter.Local
         return value => throw Errors.ExceptionThrown;
     }
     
     public static Func<int?, CancellationToken, Task<IResult<int>>> GetNullableExceptionFunctionAsync()
     {
         return (_, _) => throw Errors.ExceptionThrown;
     }
}