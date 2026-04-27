namespace Hapdy.Monads.Results.Extensions;

public static partial class ResultHelper
{
    private static async Task<IResult<TValue>> RunFunctionWithCatchAsync<T, TValue>(
        Func<T, CancellationToken, Task<IResult<TValue>>> func
      , T                                                 value
      , CancellationToken                                 cancellationToken)
    {
        try
        {
            return await func(value, cancellationToken);
        }
        catch (Exception e)
        {
            return new ExceptionFailure<TValue>(e);
        }
    }

    private static async Task<IResult<TValue>> RunFunctionWithCatch<T, TValue>(
        Func<T, CancellationToken, Task<IResult<TValue>>> func
      , T                                                 value
      , CancellationToken                                 cancellationToken)
    {
        try
        {
            return await func(value, cancellationToken);
        }
        catch (Exception e)
        {
            return new ExceptionFailure<TValue>(e);
        }
    }

    private static async Task<IResult<TValue>> RunFunctionWithCatch<T, TParam, TValue>(
        Func<T, TParam, CancellationToken, Task<IResult<TValue>>> func
      , T                                                         value
      , TParam                                                    param
      , CancellationToken                                         cancellationToken)
    {
        try
        {
            return await func(value
                            , param
                     ,        cancellationToken);
        }
        catch (Exception e)
        {
            return new ExceptionFailure<TValue>(e);
        }
    }

    private static async Task<IResult<TValue>> RunFunctionNoParamWithCatch<TValue>(
        Func<CancellationToken, Task<IResult<TValue>>> func
      , CancellationToken                              cancellationToken)
    {
        try
        {
            return await func(cancellationToken);
        }
        catch (Exception e)
        {
            return new ExceptionFailure<TValue>(e);
        }
    }

    private static async Task<IResult<TValue>> RunParamFunctionWithCatch<TParam, TValue>(
        Func<TParam, CancellationToken, Task<IResult<TValue>>> func
      , TParam                                                 value
      , CancellationToken                                      cancellationToken)
    {
        try
        {
            return await func(value, cancellationToken);
        }
        catch (Exception e)
        {
            return new ExceptionFailure<TValue>(e);
        }
    }

    private static async Task<IResult<TValue>> RunFailureFunctionWithCatch<T, TValue>(
        Func<IFailure<T>, CancellationToken, Task<IResult<TValue>>> func
      , IFailure<T>                                                 failure
      , CancellationToken                                           cancellationToken)
    {
        try
        {
            return await func(failure, cancellationToken);
        }
        catch (Exception e)
        {
            return new ExceptionFailure<TValue>(e);
        }
    }

    private static IResult<TValue> RunFunctionWithCatch<T, TValue>(
        Func<T, IResult<TValue>> func
      , T                        value)
    {
        try
        {
            return func(value);
        }
        catch (Exception e)
        {
            return new ExceptionFailure<TValue>(e);
        }
    }

    private static IResult<TValue> RunFunctionWithCatch<T, TParam, TValue>(
        Func<T, TParam, IResult<TValue>> func
      , T                                value
      , TParam                           param)
    {
        try
        {
            return func(value, param);
        }
        catch (Exception e)
        {
            return new ExceptionFailure<TValue>(e);
        }
    }

    private static IResult<TValue> RunFunctionNoParamWithCatch<TValue>(
        Func<IResult<TValue>> func)
    {
        try
        {
            return func();
        }
        catch (Exception e)
        {
            return new ExceptionFailure<TValue>(e);
        }
    }

    private static IResult<TValue> RunParamFunctionWithCatch<TParam, TValue>(
        Func<TParam, IResult<TValue>> func
      , TParam                        value)
    {
        try
        {
            return func(value);
        }
        catch (Exception e)
        {
            return new ExceptionFailure<TValue>(e);
        }
    }

    private static IResult<TValue> RunFailureFunctionWithCatch<T, TValue>(
        Func<IFailure<T>, IResult<TValue>> func
      , IFailure<T>                        failure)
    {
        try
        {
            return func(failure);
        }
        catch (Exception e)
        {
            return new ExceptionFailure<TValue>(e);
        }
    }

}