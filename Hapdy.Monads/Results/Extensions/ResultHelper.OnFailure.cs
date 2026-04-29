// ReSharper disable MemberCanBePrivate.Global
namespace Hapdy.Monads.Results.Extensions;

public static partial class ResultHelper
{
    extension<T>(Task<IResult<T>> resultTask)
    {
        /// <summary>
        /// Runs the <paramref name="failFunc"/> if the result is a failure. Otherwise, returns the result.
        /// </summary>
        /// <param name="failFunc">The function to run on failure</param>
        /// <param name="cancellationToken">The cancellation token to use</param>
        /// <returns>The result of the matched function</returns>
        public async Task<IResult<T>> OnFailure(
            Func<CancellationToken, Task<IResult<T>>> failFunc
          , CancellationToken                         cancellationToken)
        {
            var result = await resultTask;
            return await result.OnFailure(failFunc, cancellationToken);
        }

        /// <summary>
        /// Runs the <paramref name="failFunc"/> if the result is a failure. Otherwise, returns the result.
        /// </summary>
        /// <param name="failFunc">The function to run on failure</param>
        /// <param name="cancellationToken">The cancellation token to use</param>
        /// <returns>The result of the matched function</returns>
        public async Task<IResult<T>> OnFailure(
            Func<IFailure<T>, CancellationToken, Task<IResult<T>>> failFunc
          , CancellationToken                                      cancellationToken)
        {
            var result = await resultTask;
            return await result.OnFailure(failFunc, cancellationToken);
        }

        /// <summary>
        /// Runs the <paramref name="failFunc"/> if the result is a failure. Otherwise, returns the result.
        /// </summary>
        /// <param name="failFunc">The function to run on failure</param>
        /// <returns>The result of the short-circuited function</returns>
        public async Task<IResult<T>> OnFailure(
            Func<IResult<T>> failFunc)
        {
            var result = await resultTask;
            return result.OnFailure(failFunc);
        }

        /// <summary>
        /// Runs the <paramref name="failFunc"/> if the result is a failure. Otherwise, returns the result.
        /// </summary>
        /// <param name="failFunc">The function to run on failure</param>
        /// <returns>The result of the short-circuited function</returns>
        public async Task<IResult<T>> OnFailure(
            Func<IFailure<T>, IResult<T>> failFunc)
        {
            var result = await resultTask;
            return result.OnFailure(failFunc);
        }
    }

    extension<T>(IResult<T> result)
    {
        private async Task<IResult<T>> OnFailureResult(Func<IFailure<T>, Task<IResult<T>>> failFunc)
        {
            return await RunFunctionWithCatchAsync(async () => result switch
                                                               {
                                                                   ISuccess<T> success  => success
                                                                 , IExceptionFailure<T> => result
                                                                 , IFailure<T> failure  => await failFunc(failure)
#pragma warning disable CA2208
                                                                 , _ => ExceptionFailure<T>.Create(new ArgumentOutOfRangeException(nameof(result)))
#pragma warning restore CA2208
                                                               });
        }

        private async Task<IResult<T>> OnFailureResult(Func<Task<IResult<T>>> failFunc)
        {
            return await RunFunctionWithCatchAsync(async () => result switch
                                                               {
                                                                   ISuccess<T> success  => success
                                                                 , IExceptionFailure<T> => result
                                                                 , IFailure<T>          => await failFunc()
#pragma warning disable CA2208
                                                                 , _ => ExceptionFailure<T>.Create(new ArgumentOutOfRangeException(nameof(result)))
#pragma warning restore CA2208
                                                               });
        }

        private IResult<T> OnFailureResult(Func<IFailure<T>, IResult<T>> failFunc)
        {
            return RunFunctionWithCatch(() => result switch
                                              {
                                                  ISuccess<T> success  => success
                                                , IExceptionFailure<T> => result
                                                , IFailure<T> failure  => failFunc(failure)
#pragma warning disable CA2208
                                                , _ => ExceptionFailure<T>.Create(new ArgumentOutOfRangeException(nameof(result)))
#pragma warning restore CA2208
                                              });
        }

        private IResult<T> OnFailureResult(Func<IResult<T>> failFunc)
        {
            return RunFunctionWithCatch(() => result switch
                                              {
                                                  ISuccess<T> success  => success
                                                , IExceptionFailure<T> => result
                                                , IFailure<T>          => failFunc()
#pragma warning disable CA2208
                                                , _ => ExceptionFailure<T>.Create(new ArgumentOutOfRangeException(nameof(result)))
#pragma warning restore CA2208
                                              });
        }

        /// <summary>
        /// Runs the <paramref name="failFunc"/> if the result is a failure. Otherwise, returns the result.
        /// </summary>
        /// <param name="failFunc">The function to run on failure</param>
        /// <returns>The result of the short-circuited function</returns>
        public IResult<T> OnFailure(Func<IResult<T>> failFunc) { return result.OnFailureResult(failFunc); }

        /// <summary>
        /// Runs the <paramref name="failFunc"/> if the result is a failure. Otherwise, returns the result.
        /// </summary>
        /// <param name="failFunc">The function to run on failure</param>
        /// <returns>The result of the short-circuited function</returns>
        public IResult<T> OnFailure(Func<IFailure<T>, IResult<T>> failFunc) { return result.OnFailureResult(failFunc); }

        /// <summary>
        /// Runs the <paramref name="failFunc"/> if the result is a failure. Otherwise, returns the result.
        /// </summary>
        /// <param name="failFunc">The function to run on failure</param>
        /// <param name="cancellationToken">The cancellation token to use</param>
        /// <returns>The result of the matched function</returns>
        public Task<IResult<T>> OnFailure(
            Func<IFailure<T>, CancellationToken, Task<IResult<T>>> failFunc
          , CancellationToken                                      cancellationToken)
        {
            return result.OnFailureResult(failure => failFunc(failure
                                                            , cancellationToken));
        }

        /// <summary>
        /// Runs the <paramref name="failFunc"/> if the result is a failure. Otherwise, returns the result.
        /// </summary>
        /// <param name="failFunc">The function to run on failure</param>
        /// <param name="cancellationToken">The cancellation token to use</param>
        /// <returns>The result of the matched function</returns>
        public Task<IResult<T>> OnFailure(
            Func<CancellationToken, Task<IResult<T>>> failFunc
          , CancellationToken                         cancellationToken)
        {
            return result.OnFailureResult(() => failFunc(cancellationToken));
        }
    }
}