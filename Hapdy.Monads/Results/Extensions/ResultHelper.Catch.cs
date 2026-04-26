namespace Hapdy.Monads.Results.Extensions;

public static partial class ResultHelper
{
    extension<T>(Task<IResult<T>> resultTask)
    {
        public async Task<IResult<T>> Catch(Func<IExceptionFailure<T>, CancellationToken, Task<IResult<T>>> exceptionFunc, CancellationToken cancellationToken)
        {
            var result = await resultTask;
            return await result.Catch(exceptionFunc, cancellationToken);
        }

        public async Task<IResult<T>> Catch(Func<IExceptionFailure<T>, IResult<T>> exceptionFunc)
        {
            var result = await resultTask;
            return result.Catch(exceptionFunc);
        }
    }

    extension<T>(IResult<T> result)
    {
        public IResult<T> Catch(Func<IExceptionFailure<T>, IResult<T>> exceptionFunc)
        {
            return result switch
                   {
                       IExceptionFailure<T> exceptionFailure => exceptionFunc(exceptionFailure)
                     , _                                     => result
                   };
        }

        public async Task<IResult<T>> Catch(Func<IExceptionFailure<T>, CancellationToken, Task<IResult<T>>> exceptionFunc, CancellationToken cancellationToken)
        {
            return result switch
                   {
                       IExceptionFailure<T> exceptionFailure => await exceptionFunc(exceptionFailure, cancellationToken)
                     , _                                     => result
                   };
        }
    }
}