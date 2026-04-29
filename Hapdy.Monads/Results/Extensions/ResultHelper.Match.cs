// ReSharper disable MemberCanBePrivate.Global
namespace Hapdy.Monads.Results.Extensions;

public static partial class ResultHelper
{
    extension<T>(Task<IResult<T>> resultTask)
    {
        /// <summary>
        /// Matches a function to a result
        /// </summary>
        /// <param name="successFunc">The function to run on success</param>
        /// <param name="failFunc">The function to run on failure</param>
        /// <param name="cancellationToken">The cancellation token to use</param>
        /// <typeparam name="TValue">Type of the result value type</typeparam>
        /// <returns>The result of the matched function</returns>
        public async Task<IResult<TValue>> Match<TValue>(
            Func<T, CancellationToken, Task<IResult<TValue>>>           successFunc
          , Func<IFailure<T>, CancellationToken, Task<IResult<TValue>>> failFunc
          , CancellationToken                                           cancellationToken)
        {
            var result = await resultTask;
            return await result.Match(successFunc
                                    , failFunc
                                    , cancellationToken);
        }

        /// <summary>
        /// Matches a function to a result
        /// </summary>
        /// <param name="successFunc">The function to run on success</param>
        /// <param name="failFunc">The function to run on failure</param>
        /// <param name="cancellationToken">The cancellation token to use</param>
        /// <typeparam name="TValue">Type of the result value type</typeparam>
        /// <returns>The result of the matched function</returns>
        public async Task<IResult<TValue>> Match<TValue>(
            Func<CancellationToken, Task<IResult<TValue>>>              successFunc
          , Func<IFailure<T>, CancellationToken, Task<IResult<TValue>>> failFunc
          , CancellationToken                                           cancellationToken)
        {
            var result = await resultTask;
            return await result.Match(successFunc
                                    , failFunc
                                    , cancellationToken);
        }

        /// <summary>
        /// Matches a function to a result
        /// </summary>
        /// <param name="successFunc">The function to run on success</param>
        /// <param name="failFunc">The function to run on failure</param>
        /// <typeparam name="TValue">Type of the result value type</typeparam>
        /// <returns>The result of the matched function</returns>
        public async Task<IResult<TValue>> Match<TValue>(
            Func<T, IResult<TValue>>           successFunc
          , Func<IFailure<T>, IResult<TValue>> failFunc)
        {
            var result = await resultTask;
            return result.Match(successFunc
                              , failFunc);
        }

        /// <summary>
        /// Matches a function to a result
        /// </summary>
        /// <param name="successFunc">The function to run on success</param>
        /// <param name="failFunc">The function to run on failure</param>
        /// <typeparam name="TValue">Type of the result value type</typeparam>
        /// <returns>The result of the matched function</returns>
        public async Task<IResult<TValue>> Match<TValue>(
            Func<IResult<TValue>>              successFunc
          , Func<IFailure<T>, IResult<TValue>> failFunc)
        {
            var result = await resultTask;
            return result.Match(successFunc
                              , failFunc);
        }
    }

    extension<T>(IResult<T> result)
    {
        private async Task<IResult<TValue>> MatchResult<TValue>(
            Func<T, Task<IResult<TValue>>>           successFunc
          , Func<IFailure<T>, Task<IResult<TValue>>> failFunc)
        {
            return await RunFunctionWithCatchAsync(async () => result switch
                                                               {
                                                                   ISuccess<T> success                   => await successFunc(success.Value).ConfigureAwait(true)
                                                                 , IExceptionFailure<T> exceptionFailure => ExceptionFailure<TValue>.Create(exceptionFailure.Exception)
                                                                 , IFailure<T> failure                   => await failFunc(failure).ConfigureAwait(true)
#pragma warning disable CA2208
                                                                 , _ => ExceptionFailure<TValue>.Create(new ArgumentOutOfRangeException(nameof(result)))
#pragma warning restore CA2208
                                                               });
        }

        private async Task<IResult<TValue>> MatchResult<TValue>(
            Func<T, IResult<TValue>>                 successFunc
          , Func<IFailure<T>, Task<IResult<TValue>>> failFunc)
        {
            return await RunFunctionWithCatchAsync(async () => result switch
                                                               {
                                                                   ISuccess<T> success                   => successFunc(success.Value)
                                                                 , IExceptionFailure<T> exceptionFailure => ExceptionFailure<TValue>.Create(exceptionFailure.Exception)
                                                                 , IFailure<T> failure                   => await failFunc(failure).ConfigureAwait(false)
#pragma warning disable CA2208
                                                                 , _ => ExceptionFailure<TValue>.Create(new ArgumentOutOfRangeException(nameof(result)))
#pragma warning restore CA2208
                                                               });
        }

        private async Task<IResult<TValue>> MatchResult<TValue>(
            Func<T, Task<IResult<TValue>>>     successFunc
          , Func<IFailure<T>, IResult<TValue>> failFunc)
        {
            return await RunFunctionWithCatchAsync(async () => result switch
                                                               {
                                                                   ISuccess<T> success                   => await successFunc(success.Value).ConfigureAwait(false)
                                                                 , IExceptionFailure<T> exceptionFailure => ExceptionFailure<TValue>.Create(exceptionFailure.Exception)
                                                                 , IFailure<T> failure                   => failFunc(failure)
#pragma warning disable CA2208
                                                                 , _ => ExceptionFailure<TValue>.Create(new ArgumentOutOfRangeException(nameof(result)))
#pragma warning restore CA2208
                                                               });
        }

        private IResult<TValue> MatchResult<TValue>(
            Func<T, IResult<TValue>>           successFunc
          , Func<IFailure<T>, IResult<TValue>> failFunc)
        {
            return RunFunctionWithCatch(() => result switch
                                              {
                                                  ISuccess<T> success                   => successFunc(success.Value)
                                                , IExceptionFailure<T> exceptionFailure => ExceptionFailure<TValue>.Create(exceptionFailure.Exception)
                                                , IFailure<T> failure                   => failFunc(failure)
#pragma warning disable CA2208
                                                , _ => ExceptionFailure<TValue>.Create(new ArgumentOutOfRangeException(nameof(result)))
#pragma warning restore CA2208
                                              });
        }

        /// <summary>
        /// Matches a function to a result
        /// </summary>
        /// <param name="successFunc">The function to run on success</param>
        /// <param name="failFunc">The function to run on failure</param>
        /// <typeparam name="TValue">Type of the result value type</typeparam>
        /// <returns>The result of the matched function</returns>
        public IResult<TValue> Match<TValue>(
            Func<T, IResult<TValue>>           successFunc
          , Func<IFailure<T>, IResult<TValue>> failFunc)
        {
            return result.MatchResult(successFunc
                                    , failFunc);
        }

        /// <summary>
        /// Matches a function to a result
        /// </summary>
        /// <param name="successFunc">The function to run on success</param>
        /// <param name="failFunc">The function to run on failure</param>
        /// <typeparam name="TValue">Type of the result value type</typeparam>
        /// <returns>The result of the matched function</returns>
        public IResult<TValue> Match<TValue>(
            Func<IResult<TValue>>              successFunc
          , Func<IFailure<T>, IResult<TValue>> failFunc)
        {
            return result.MatchResult(_ => successFunc()
                                    , failFunc);
        }

        /// <summary>
        /// Matches a function to a result
        /// </summary>
        /// <param name="successFunc">The function to run on success</param>
        /// <param name="failFunc">The function to run on failure</param>
        /// <param name="cancellationToken"></param>
        /// <typeparam name="TValue">Type of the result value type</typeparam>
        /// <returns>The result of the matched function</returns>
        public Task<IResult<TValue>> Match<TValue>(
            Func<T, CancellationToken, Task<IResult<TValue>>> successFunc
          , Func<IFailure<T>, IResult<TValue>>                failFunc
          , CancellationToken                                 cancellationToken)
        {
            return result.MatchResult(value => successFunc(value, cancellationToken)
                                    , failFunc);
        }

        /// <summary>
        /// Matches a function to a result
        /// </summary>
        /// <param name="successFunc">The function to run on success</param>
        /// <param name="failFunc">The function to run on failure</param>
        /// <param name="cancellationToken">The cancellation token to use for asynchronous operations</param>
        /// <typeparam name="TValue">Type of the result value type</typeparam>
        /// <returns>The result of the matched function</returns>
        public Task<IResult<TValue>> Match<TValue>(
            Func<CancellationToken, Task<IResult<TValue>>> successFunc
          , Func<IFailure<T>, IResult<TValue>>             failFunc
          , CancellationToken                              cancellationToken)
        {
            return result.MatchResult(_ => successFunc(cancellationToken)
                                    , failFunc);
        }

        /// <summary>
        /// Matches a function to a result
        /// </summary>
        /// <param name="successFunc">The function to run on success</param>
        /// <param name="failFunc">The function to run on failure</param>
        /// <param name="cancellationToken">The cancellation token to use for asynchronous operations</param>
        /// <typeparam name="TValue">Type of the result value type</typeparam>
        /// <returns>The result of the matched function</returns>
        public Task<IResult<TValue>> Match<TValue>(
            Func<T, IResult<TValue>>                                    successFunc
          , Func<IFailure<T>, CancellationToken, Task<IResult<TValue>>> failFunc
          , CancellationToken                                           cancellationToken)
        {
            return result.MatchResult(successFunc
                                    , failure => failFunc(failure, cancellationToken));
        }

        /// <summary>
        /// Matches a function to a result
        /// </summary>
        /// <param name="successFunc">The function to run on success</param>
        /// <param name="failFunc">The function to run on failure</param>
        /// <param name="cancellationToken">The cancellation token to use for asynchronous operations</param>
        /// <typeparam name="TValue">Type of the result value type</typeparam>
        /// <returns>The result of the matched function</returns>
        public Task<IResult<TValue>> Match<TValue>(
            Func<IResult<TValue>>                                       successFunc
          , Func<IFailure<T>, CancellationToken, Task<IResult<TValue>>> failFunc
          , CancellationToken                                           cancellationToken)
        {
            return result.MatchResult(_ => successFunc()
                                    , failure => failFunc(failure, cancellationToken));
        }

        /// <summary>
        /// Matches a function to a result
        /// </summary>
        /// <param name="successFunc">The function to run on success</param>
        /// <param name="failFunc">The function to run on failure</param>
        /// <param name="cancellationToken">The cancellation token to use</param>
        /// <typeparam name="TValue">Type of the result value type</typeparam>
        /// <returns>The result of the matched function</returns>
        public Task<IResult<TValue>> Match<TValue>(
            Func<T, CancellationToken, Task<IResult<TValue>>>           successFunc
          , Func<IFailure<T>, CancellationToken, Task<IResult<TValue>>> failFunc
          , CancellationToken                                           cancellationToken)
        {
            return result.MatchResult(value => successFunc(value
                                                         , cancellationToken)
                                    , failure => failFunc(failure
                                                        , cancellationToken));
        }

        /// <summary>
        /// Matches a function to a result
        /// </summary>
        /// <param name="successFunc">The function to run on success</param>
        /// <param name="failFunc">The function to run on failure</param>
        /// <param name="cancellationToken">The cancellation token to use</param>
        /// <typeparam name="TValue">Type of the result value type</typeparam>
        /// <returns>The result of the matched function</returns>
        public Task<IResult<TValue>> Match<TValue>(
            Func<CancellationToken, Task<IResult<TValue>>>              successFunc
          , Func<IFailure<T>, CancellationToken, Task<IResult<TValue>>> failFunc
          , CancellationToken                                           cancellationToken)
        {
            return result.MatchResult(_ => successFunc(cancellationToken)
                                    , failure => failFunc(failure
                                                        , cancellationToken));
        }
    }
}