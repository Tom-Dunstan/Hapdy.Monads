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
        /// <param name="param">Parameter to be passed to success function</param>
        /// <param name="cancellationToken">The cancellation token to use</param>
        /// <typeparam name="TParam">Type of parameter to be passed to success function</typeparam>
        /// <typeparam name="TValue">Type of the result value type</typeparam>
        /// <returns>The result of the matched function</returns>
        public async Task<IResult<TValue>> Match<TParam, TValue>(
            Func<TParam, CancellationToken, Task<IResult<TValue>>>      successFunc
          , Func<IFailure<T>, CancellationToken, Task<IResult<TValue>>> failFunc
          , TParam                                                      param
          , CancellationToken                                           cancellationToken)
        {
            var result = await resultTask;
            return await result.Match(successFunc
                                    , failFunc
                                    , param
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

        /// <summary>
        /// Matches a function to a result
        /// </summary>
        /// <param name="successFunc">The function to run on success</param>
        /// <param name="failFunc">The function to run on failure</param>
        /// <param name="param">Parameter to be passed to success function</param>
        /// <typeparam name="TParam">Type of parameter to be passed to success function</typeparam>
        /// <typeparam name="TValue">Type of the result value type</typeparam>
        /// <returns>The result of the matched function</returns>
        public async Task<IResult<TValue>> Match<TParam, TValue>(
            Func<TParam, IResult<TValue>>      successFunc
          , Func<IFailure<T>, IResult<TValue>> failFunc
          , TParam                             param)
        {
            var result = await resultTask;
            return result.Match(successFunc
                              , failFunc
                              , param);
        }
    }

    extension<T>(IResult<T> result)
    {
        private async Task<IResult<TValue>> MatchResult<TValue>(
            Func<T, Task<IResult<TValue>>>           successFunc
          , Func<IFailure<T>, Task<IResult<TValue>>> failFunc)
        {
            return result switch
                   {
                       ISuccess<T> success                   => await successFunc(success.Value).ConfigureAwait(false)
                     , IShortCircuit<T> shortCircuit         => ShortCircuit<TValue>.Create(shortCircuit.Value)
                     , IExceptionFailure<T> exceptionFailure => ExceptionFailure<TValue>.Create(exceptionFailure.Exception)
                     , IFailure<T> failure                   => await failFunc(failure).ConfigureAwait(false)
#pragma warning disable CA2208
                     , _ => ExceptionFailure<TValue>.Create(new ArgumentOutOfRangeException(nameof(result)))
#pragma warning restore CA2208
                   };
        }

        private async Task<IResult<TValue>> MatchResult<TValue>(
            Func<T, IResult<TValue>>                 successFunc
          , Func<IFailure<T>, Task<IResult<TValue>>> failFunc)
        {
            return result switch
                   {
                       ISuccess<T> success                   => successFunc(success.Value)
                     , IShortCircuit<T> shortCircuit         => ShortCircuit<TValue>.Create(shortCircuit.Value)
                     , IExceptionFailure<T> exceptionFailure => ExceptionFailure<TValue>.Create(exceptionFailure.Exception)
                     , IFailure<T> failure                   => await failFunc(failure).ConfigureAwait(false)
#pragma warning disable CA2208
                     , _ => ExceptionFailure<TValue>.Create(new ArgumentOutOfRangeException(nameof(result)))
#pragma warning restore CA2208
                   };
        }

        private IResult<TValue> MatchResult<TValue>(
            Func<T, IResult<TValue>>           successFunc
          , Func<IFailure<T>, IResult<TValue>> failFunc)
        {
            return result switch
                   {
                       ISuccess<T> success                   => successFunc(success.Value)
                     , IShortCircuit<T> shortCircuit         => ShortCircuit<TValue>.Create(shortCircuit.Value)
                     , IExceptionFailure<T> exceptionFailure => ExceptionFailure<TValue>.Create(exceptionFailure.Exception)
                     , IFailure<T> failure                   => failFunc(failure)
#pragma warning disable CA2208
                     , _ => ExceptionFailure<TValue>.Create(new ArgumentOutOfRangeException(nameof(result)))
#pragma warning restore CA2208
                   };
        }

        private async Task<IResult<TValue>> MatchResult<TValue>(
            Func<T, Task<IResult<TValue>>>     successFunc
          , Func<IFailure<T>, IResult<TValue>> failFunc)
        {
            return result switch
                   {
                       ISuccess<T> success                   => await successFunc(success.Value).ConfigureAwait(false)
                     , IShortCircuit<T> shortCircuit         => ShortCircuit<TValue>.Create(shortCircuit.Value)
                     , IExceptionFailure<T> exceptionFailure => ExceptionFailure<TValue>.Create(exceptionFailure.Exception)
                     , IFailure<T> failure                   => failFunc(failure)
#pragma warning disable CA2208
                     , _ => ExceptionFailure<TValue>.Create(new ArgumentOutOfRangeException(nameof(result)))
#pragma warning restore CA2208
                   };
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
            return result.MatchResult(value => RunFunctionWithCatch(successFunc, value)
                                    , failure => RunFailureFunctionWithCatch(failFunc, failure));
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
            return result.MatchResult(_ => RunFunctionNoParamWithCatch(successFunc)
                                    , failure => RunFailureFunctionWithCatch(failFunc, failure));
        }

        /// <summary>
        /// Matches a function to a result
        /// </summary>
        /// <param name="successFunc">The function to run on success</param>
        /// <param name="failFunc">The function to run on failure</param>
        /// <param name="param">Parameter to be passed to success function</param>
        /// <typeparam name="TParam">Type of parameter to be passed to success function</typeparam>
        /// <typeparam name="TValue">Type of the result value type</typeparam>
        /// <returns>The result of the matched function</returns>
        public IResult<TValue> Match<TParam, TValue>(
            Func<TParam, IResult<TValue>>      successFunc
          , Func<IFailure<T>, IResult<TValue>> failFunc
          , TParam                             param)
        {
            return result.MatchResult<T, TValue>(_ => RunParamFunctionWithCatch(successFunc
                                                                              , param)
                                               , failure => RunFailureFunctionWithCatch(failFunc
                                                                                      , failure));
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
            return result.MatchResult(value => RunFunctionWithCatch(successFunc
                                                                  , value
                                                                  , cancellationToken)
                                    , failure => RunFailureFunctionWithCatch(failFunc
                                                                           , failure
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
            return result.MatchResult(_ => RunFunctionNoParamWithCatch(successFunc, cancellationToken)
                                    , failure => RunFailureFunctionWithCatch(failFunc
                                                                           , failure
                                                                           , cancellationToken));
        }

        /// <summary>
        /// Matches a function to a result
        /// </summary>
        /// <param name="successFunc">The function to run on success</param>
        /// <param name="failFunc">The function to run on failure</param>
        /// <param name="param">Parameter to be passed to success function</param>
        /// <param name="cancellationToken">The cancellation token to use</param>
        /// <typeparam name="TParam">Type of parameter to be passed to success function</typeparam>
        /// <typeparam name="TValue">Type of the result value type</typeparam>
        /// <returns>The result of the matched function</returns>
        public Task<IResult<TValue>> Match<TParam, TValue>(
            Func<TParam, CancellationToken, Task<IResult<TValue>>>      successFunc
          , Func<IFailure<T>, CancellationToken, Task<IResult<TValue>>> failFunc
          , TParam                                                      param
          , CancellationToken                                           cancellationToken)
        {
            return result.MatchResult<T, TValue>(_ => RunParamFunctionWithCatch(successFunc
                                                                              , param
                                                                              , cancellationToken)
                                               , failure => RunFailureFunctionWithCatch(failFunc
                                                                                      , failure
                                                                                      , cancellationToken));
        }
    }
}