// ReSharper disable MemberCanBePrivate.Global
namespace Hapdy.Monads.Results.Extensions;

public static partial class ResultHelper
{
    extension<T>(Task<IResult<T>> resultTask)
    {
        /// <summary>
        /// Runs the <paramref name="exceptionFunc"/> if the result is a ExceptionFailure result. Otherwise, returns the result.
        /// </summary>
        /// <param name="exceptionFunc">Function to handle exceptions</param>
        /// <param name="cancellationToken">CancellationToken</param>
        /// <returns>A result after handling exceptions</returns>
        public async Task<IResult<T>> Catch(Func<IExceptionFailure<T>, CancellationToken, Task<IResult<T>>> exceptionFunc, CancellationToken cancellationToken)
        {
            var result = await resultTask;
            return await result.Catch(exceptionFunc, cancellationToken);
        }

        /// <summary>
        /// Runs the <paramref name="exceptionFunc"/> if the result is a ExceptionFailure result. Otherwise, returns the result.
        /// </summary>
        /// <param name="exceptionFunc">Function to handle exceptions</param>
        /// <returns>A result after handling exceptions</returns>
        public async Task<IResult<T>> Catch(Func<IExceptionFailure<T>, IResult<T>> exceptionFunc)
        {
            var result = await resultTask;
            return result.Catch(exceptionFunc);
        }
    }

    extension<T>(IResult<T> result)
    {
        /// <summary>
        /// Runs the <paramref name="exceptionFunc"/> if the result is an ExceptionFailure result. Otherwise, returns the result.
        /// </summary>
        /// <param name="exceptionFunc">Function to handle exceptions</param>
        /// <returns>A result after handling exceptions</returns>
        public IResult<T> Catch(Func<IExceptionFailure<T>, IResult<T>> exceptionFunc)
        {
            return result switch
                   {
                       IExceptionFailure<T> exceptionFailure => exceptionFunc(exceptionFailure)
                     , _                                     => result
                   };
        }

        /// <summary>
        /// Runs the <paramref name="exceptionFunc"/> if the result is an ExceptionFailure result. Otherwise, returns the result.
        /// </summary>
        /// <param name="exceptionFunc">Function to handle exceptions</param>
        /// <param name="cancellationToken">Cancellation token for asynchronous operations</param>
        /// <returns>A result after handling exceptions</returns>
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