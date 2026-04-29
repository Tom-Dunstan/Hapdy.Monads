namespace Hapdy.Monads.Results.Extensions;

public static partial class ResultHelper
{
    private static IResult<T> RunFunctionWithCatch<T>(Func<IResult<T>> func)
    {
        try
        {
            return func();
        }
        catch (Exception e) when (e is not OperationCanceledException)
        {
            return new ExceptionFailure<T>(e);
        }
    }

    private static async Task<IResult<T>> RunFunctionWithCatchAsync<T>(Func<Task<IResult<T>>> func)
    {
        try
        {
            return await func();
        }
        catch (Exception e) when (e is not OperationCanceledException)
        {
            return new ExceptionFailure<T>(e);
        }
    }

    private static IResult<T> RunActionWithCatch<T>(IResult<T> result, Action action)
    {
        try
        {
            action();
            return result;
        }
        catch (Exception e) when (e is not OperationCanceledException)
        {
            return new ExceptionFailure<T>(e);
        }
    }

    private static async Task<IResult<T>> RunActionWithCatchAsync<T>(IResult<T> result, Func<Task> action)
    {
        try
        {
            await action();
            return result;
        }
        catch (Exception e) when (e is not OperationCanceledException)
        {
            return new ExceptionFailure<T>(e);
        }
    }
}