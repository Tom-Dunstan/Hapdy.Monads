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
        /// <returns>The previous result or and ExceptionFailure/></returns>
        public async Task<IResult<T>> Tap(Action action)
        {
            var result = await resultTask;
            return result.Tap(action);
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
        public IResult<T> Tap(Action<T> action) { return result.TapResult(action); }

        /// <summary>
        /// Binds a function to a parameter
        /// </summary>
        /// <param name="action">Action to bind to result</param>
        /// <returns>The previous result or and ExceptionFailure/></returns>
        public IResult<T> Tap(Action action) { return result.TapResult(_ => action()); }

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
            return result.TapResult(value => asyncAction(value, cancellationToken));
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
            return result.TapResult(_ => asyncAction(cancellationToken));
        }
    }
}