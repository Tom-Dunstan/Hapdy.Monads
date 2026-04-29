// ReSharper disable MemberCanBePrivate.Global

namespace Hapdy.Monads.Results.Extensions;

public static partial class ResultHelper
{
    extension<T>(Task<IResult<T>> resultTask)
    {
        /// <summary>
        /// Binds a function to a parameter
        /// </summary>
        /// <param name="action">Action to bind to result</param>
        /// <returns>The previous result or and ExceptionFailure/></returns>
        /// <param name="cancellationToken">Cancellation token for asynchronous operation</param>
        /// <returns>The previous result or and ExceptionFailure/></returns>
        public async Task<IResult<T>> Tap(Func<T, CancellationToken, Task> action, CancellationToken cancellationToken)
        {
            var result = await resultTask;
            return await result.Tap(action, cancellationToken);
        }

        /// <summary>
        /// Binds a function to a parameter
        /// </summary>
        /// <param name="asyncAction">Action to bind to result</param>
        /// <param name="param">Parameter to pass to function</param>
        /// <param name="cancellationToken">Cancellation token for asynchronous operation</param>
        /// <returns>The previous result or and ExceptionFailure/></returns>
        public async Task<IResult<T>> Tap<TParam>(
            Func<T, TParam, CancellationToken, Task> asyncAction
          , TParam                                   param
          , CancellationToken                        cancellationToken)
        {
            var result = await resultTask;
            return await result.Tap(asyncAction
                                  , param
                                  , cancellationToken);
        }

        /// <summary>
        /// Binds a function to a parameter
        /// </summary>
        /// <param name="asyncAction">Action to bind to result</param>
        /// <param name="cancellationToken">Cancellation token for asynchronous operation</param>
        /// <returns>The previous result or and ExceptionFailure/></returns>
        public async Task<IResult<T>> Tap(
            Func<CancellationToken, Task> asyncAction
          , CancellationToken             cancellationToken)
        {
            var result = await resultTask;
            return await result.Tap(asyncAction, cancellationToken);
        }

        /// <summary>
        /// Binds a function to a parameter
        /// </summary>
        /// <param name="asyncAction">Action to bind to result</param>
        /// <param name="param">Parameter to pass to function</param>
        /// <param name="cancellationToken">Cancellation token for asynchronous operation</param>
        /// <typeparam name="TParam">Type of parameter</typeparam>
        /// <returns>The previous result or and ExceptionFailure/></returns>
        public async Task<IResult<T>> Tap<TParam>(
            Func<TParam, CancellationToken, Task> asyncAction
          , TParam                                param
          , CancellationToken                     cancellationToken)
        {
            var result = await resultTask;
            return await result.Tap(asyncAction
                                  , param
                                  , cancellationToken);
        }

        /// <summary>
        /// Binds a function to a parameter
        /// </summary>
        /// <param name="action">Action to bind to result</param>
        /// <returns>The previous result or and ExceptionFailure/></returns>
        public async Task<IResult<T>> Tap(Action<T> action)
        {
            var result = await resultTask;
            return result.Tap(action);
        }

        /// <summary>
        /// Binds a function to a parameter
        /// </summary>
        /// <param name="action">Action to bind to result</param>
        /// <param name="param">Parameter to bind to result</param>
        /// <returns>The previous result or and ExceptionFailure/></returns>
        public async Task<IResult<T>> Tap<TParam>(Action<T, TParam> action, TParam param)
        {
            var result = await resultTask;
            return result.Tap(action, param);
        }

        /// <summary>
        /// Binds a function to a parameter
        /// </summary>
        /// <param name="action">Action to bind to result</param>
        /// <returns>The previous result or and ExceptionFailure/></returns>
        public async Task<IResult<T>> Tap(Action action)
        {
            var result = await resultTask;
            return result.Tap(action);
        }

        /// <summary>
        /// Binds a function to a parameter
        /// </summary>
        /// <param name="action">Action to bind to result</param>
        /// <param name="param">Parameter to pass to function</param>
        /// <typeparam name="TParam">Type of parameter</typeparam>
        /// <returns>The previous result or and ExceptionFailure/></returns>
        public async Task<IResult<T>> Tap<TParam>(Action<TParam> action, TParam param)
        {
            var result = await resultTask;
            return result.Tap(action, param);
        }
    }

    extension<T>(IResult<T> result)
    {
        private IResult<T> TapResult(Action<T> action)
        {
            return RunFunctionWithCatch(() =>
                                        {
                                            switch (result)
                                            {
                                                case ISuccess<T> success: action(success.Value); break;
                                            }

                                            return result;
                                        });
        }

        private async Task<IResult<T>> TapResult(Func<T, Task> actionAsync)
        {
            return await RunFunctionWithCatchAsync(async () =>
                                                   {
                                                       switch (result)
                                                       {
                                                           case ISuccess<T> success: await actionAsync(success.Value).ConfigureAwait(true); break;
                                                       }

                                                       return result;
                                                   });
        }

        /// <summary>
        /// Binds a function to a parameter
        /// </summary>
        /// <param name="action">Action to bind to result</param>
        /// <returns>The previous result or and ExceptionFailure/></returns>
        public IResult<T> Tap(Action<T> action) { return result.TapResult(value => RunActionWithCatch(result, () => action(value))); }

        /// <summary>
        /// Binds a function to a parameter
        /// </summary>
        /// <param name="action">Function to bind to result</param>
        /// <param name="param">Parameter to pass to function</param>
        public IResult<T> Tap<TParam>(Action<T, TParam> action, TParam param)
        {
            return result.TapResult(value => RunActionWithCatch(result
                                                              , () => action(value
                                                                           , param)));
        }

        /// <summary>
        /// Binds a function to a parameter
        /// </summary>
        /// <param name="action">Action to bind to result</param>
        /// <returns>The previous result or and ExceptionFailure/></returns>
        public IResult<T> Tap(Action action) { return result.TapResult(_ => RunActionWithCatch(result, action)); }

        /// <summary>
        /// Binds a function to a parameter
        /// </summary>
        /// <param name="action">Action to bind to result</param>
        /// <param name="param">Parameter object or value to pass to bound function</param>
        /// <typeparam name="TParam">The type of the parameter to pass to the bound function</typeparam>
        /// <returns>The previous result or and ExceptionFailure/></returns>
        public IResult<T> Tap<TParam>(Action<TParam> action, TParam param) { return result.TapResult(_ => RunActionWithCatch(result, () => action(param))); }

        /// <summary>
        /// Binds a function to a parameter
        /// </summary>
        /// <param name="asyncAction">Action to bind to result</param>
        /// <param name="cancellationToken">Cancellation token for bound async action</param>
        /// <returns>The previous result or and ExceptionFailure/></returns>
        public Task<IResult<T>> Tap(
            Func<T, CancellationToken, Task> asyncAction
          , CancellationToken                cancellationToken)
        {
            return result.TapResult(value => RunActionWithCatchAsync(result, () => asyncAction(value, cancellationToken)));
        }

        /// <summary>
        /// Binds a function to a parameter
        /// </summary>
        /// <param name="asyncAction">Action to bind to result</param>
        /// <param name="param">Parameter to pass to bound function</param>
        /// <param name="cancellationToken">Cancellation token for bound async action</param>
        /// <typeparam name="TParam">Type of the parameter to pass to bound action</typeparam>
        /// <returns>The previous result or and ExceptionFailure/></returns>
        public Task<IResult<T>> Tap<TParam>(
            Func<T, TParam, CancellationToken, Task> asyncAction
          , TParam                                   param
          , CancellationToken                        cancellationToken)
        {
            return result.TapResult(value => RunActionWithCatchAsync(result
                                                                   , () => asyncAction(value
                                                                                     , param
                                                                                     , cancellationToken)));
        }

        /// <summary>
        /// Binds a function to a parameter
        /// </summary>
        /// <param name="asyncAction">Action to bind to result</param>
        /// <param name="cancellationToken">Cancellation token for bound async action</param>
        /// <returns>The previous result or and ExceptionFailure/></returns>
        public Task<IResult<T>> Tap(
            Func<CancellationToken, Task> asyncAction
          , CancellationToken             cancellationToken)
        {
            return result.TapResult(_ => RunActionWithCatchAsync(result, () => asyncAction(cancellationToken)));
        }

        /// <summary>
        /// Binds a function to a parameter
        /// </summary>
        /// <param name="asyncAction">Action to bind to result</param>
        /// <param name="param">Parameter object or value to pass to bound function</param>
        /// <param name="cancellationToken">Cancellation token for bound async action</param>
        /// <returns>The previous result or and ExceptionFailure/></returns>
        public Task<IResult<T>> Tap<TParam>(
            Func<TParam, CancellationToken, Task> asyncAction
          , TParam                                param
          , CancellationToken                     cancellationToken)
        {
            return result.TapResult(_ => RunActionWithCatchAsync(result
                                                               , () => asyncAction(param
                                                                                 , cancellationToken)));
        }
    }
}