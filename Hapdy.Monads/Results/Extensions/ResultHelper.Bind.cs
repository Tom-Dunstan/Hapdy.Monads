namespace Hapdy.Monads.Results.Extensions;

public static partial class ResultHelper
{
    extension<T>(Task<IResult<T>> resultTask)
    {
        /// <summary>
        /// Binds a function to a parameter
        /// </summary>
        /// <param name="func">Function to bind to result</param>
        /// <param name="cancellationToken">Cancellation token for asynchronous operation</param>
        /// <typeparam name="TValue">Type of the result value type</typeparam>
        /// <returns>A result of type <typeparamref name="TValue" /></returns>
        public async Task<IResult<TValue>> Bind<TValue>(Func<T, CancellationToken, Task<IResult<TValue>>> func, CancellationToken cancellationToken)
        {
            var result = await resultTask;
            return await result.Bind(func, cancellationToken);
        }

        /// <summary>
        /// Binds a function to a parameter
        /// </summary>
        /// <param name="func">Function to bind to result</param>
        /// <param name="cancellationToken">Cancellation token for asynchronous operation</param>
        /// <typeparam name="TValue">Type of the result value type</typeparam>
        /// <returns>A result of type <typeparamref name="TValue" /></returns>
        public async Task<IResult<TValue>> Bind<TValue>(Func<CancellationToken, Task<IResult<TValue>>> func, CancellationToken cancellationToken)
        {
            var result = await resultTask;
            return await result.Bind(func, cancellationToken);
        }

        /// <summary>
        /// Binds a function to a parameter
        /// </summary>
        /// <param name="func">Function to bind to result</param>
        /// <param name="param">Parameter to pass to function</param>
        /// <param name="cancellationToken">Cancellation token for asynchronous operation</param>
        /// <typeparam name="TParam">Type of parameter</typeparam>
        /// <typeparam name="TValue">Type of the result value type</typeparam>
        /// <returns>A result of type <typeparamref name="TValue" /></returns>
        public async Task<IResult<TValue>> Bind<TParam, TValue>(Func<TParam, CancellationToken, Task<IResult<TValue>>> func, TParam param, CancellationToken cancellationToken)
        {
            var result = await resultTask;
            return await result.Bind(func
                                   , param
                                   , cancellationToken);
        }

        /// <summary>
        /// Binds a function to a parameter
        /// </summary>
        /// <param name="func">Function to bind to result</param>
        /// <typeparam name="TValue">Type of the result value type</typeparam>
        /// <returns>A result of type <typeparamref name="TValue" /></returns>
        public async Task<IResult<TValue>> Bind<TValue>(Func<T, IResult<TValue>> func)
        {
            var result = await resultTask;
            return result.Bind(func);
        }

        /// <summary>
        /// Binds a function to a parameter
        /// </summary>
        /// <param name="func">Function to bind to result</param>
        /// <typeparam name="TValue">Type of the result value type</typeparam>
        /// <returns>A result of type <typeparamref name="TValue" /></returns>
        public async Task<IResult<TValue>> Bind<TValue>(Func<IResult<TValue>> func)
        {
            var result = await resultTask;
            return result.Bind(func);
        }

        /// <summary>
        /// Binds a function to a parameter
        /// </summary>
        /// <param name="func">Function to bind to result</param>
        /// <param name="param">Parameter to pass to function</param>
        /// <typeparam name="TParam">Type of parameter</typeparam>
        /// <typeparam name="TValue">Type of the result value type</typeparam>
        /// <returns>A result of type <typeparamref name="TValue" /></returns>
        public async Task<IResult<TValue>> Bind<TValue, TParam>(Func<TParam, IResult<TValue>> func, TParam param)
        {
            var result = await resultTask;
            return result.Bind(func, param);
        }
    }

    extension<T>(IResult<T> result)
    {
        private IResult<TValue> BindResult<TValue>(Func<T, IResult<TValue>> func)
        {
            return result switch
                   {
                       ISuccess<T> success                   => func(success.Value)
                     , IShortCircuit<T> shortCircuit         => ShortCircuit<TValue>.Create(shortCircuit.Value)
                     , IExceptionFailure<T> exceptionFailure => ExceptionFailure<TValue>.Create(exceptionFailure.Exception)
                     , IFailure<T> failure                   => Failure<TValue>.Create(failure.ErrorMessage)
#pragma warning disable CA2208
                     , _                                     => ExceptionFailure<TValue>.Create(new ArgumentOutOfRangeException(nameof(result)))
#pragma warning restore CA2208
                   };
        }
        
        private async Task<IResult<TValue>> BindResult<TValue>(Func<T, Task<IResult<TValue>>> func)
        {
            return result switch
                   {
                       ISuccess<T> success                   => await func(success.Value).ConfigureAwait(false)
                     , IShortCircuit<T> shortCircuit         => ShortCircuit<TValue>.Create(shortCircuit.Value)
                     , IExceptionFailure<T> exceptionFailure => ExceptionFailure<TValue>.Create(exceptionFailure.Exception)
                     , IFailure<T> failure                   => Failure<TValue>.Create(failure.ErrorMessage)
#pragma warning disable CA2208
                     , _                                     => ExceptionFailure<TValue>.Create(new ArgumentOutOfRangeException(nameof(result)))
#pragma warning restore CA2208
                   };
        }

        /// <summary>
        /// Binds a function to a parameter
        /// </summary>
        /// <param name="func">Function to bind to result</param>
        /// <returns>A result of type <typeparamref name="TValue" /></returns>
        public IResult<TValue> Bind<TValue>(Func<T, IResult<TValue>> func) { return result.BindResult(value => RunFunctionWithCatch(func, value)); }

        /// <summary>
        /// Binds a function to a parameter
        /// </summary>
        /// <param name="func">Function to bind to result</param>
        /// <typeparam name="TValue">Type of the result value type</typeparam>
        /// <returns>A result of type <typeparamref name="TValue" /></returns>
        public IResult<TValue> Bind<TValue>(Func<IResult<TValue>> func) { return result.BindResult(_ => RunFunctionNoParamWithCatch(func)); }

        /// <summary>
        /// Binds a function to a parameter
        /// </summary>
        /// <param name="func">Function to bind to result</param>
        /// <param name="param">Parameter object or value to pass to bound function</param>
        /// <typeparam name="TParam">The type of the parameter to pass to the bound function</typeparam>
        /// <typeparam name="TValue">Type of the result value type</typeparam>
        /// <returns>A result of type <typeparamref name="TValue" /></returns>
        public IResult<TValue> Bind<TParam, TValue>(Func<TParam, IResult<TValue>> func, TParam param)
        {
            return result.BindResult(_ => RunParamFunctionWithCatch(func
                                                                  , param));
        }

        /// <summary>
        /// Binds a function to a parameter
        /// </summary>
        /// <param name="func">Function to bind to result</param>
        /// <param name="cancellationToken">Cancellation token for bound async function</param>
        /// <typeparam name="TValue">Type of the result value type</typeparam>
        /// <returns>A result of type <typeparamref name="TValue" /></returns>
        public async Task<IResult<TValue>> Bind<TValue>(Func<T, CancellationToken, Task<IResult<TValue>>> func
                                                      , CancellationToken cancellationToken)
        {
            return await result.BindResult(value => RunFunctionWithCatch(func
                                                                       , value
                                                                       , cancellationToken));
        }

        /// <summary>
        /// Binds a function to a parameter
        /// </summary>
        /// <param name="func">Function to bind to result</param>
        /// <param name="cancellationToken">Cancellation token for bound async function</param>
        /// <typeparam name="TValue">Type of the result value type</typeparam>
        /// <returns>A result of type <typeparamref name="TValue" /></returns>
        public async Task<IResult<TValue>> Bind<TValue>(
            Func<CancellationToken, Task<IResult<TValue>>> func
          , CancellationToken                              cancellationToken)
        {
            return await result.BindResult(_ => RunFunctionNoParamWithCatch(func, cancellationToken));
        }

        /// <summary>
        /// Binds a function to a parameter
        /// </summary>
        /// <param name="func">Function to bind to result</param>
        /// <param name="param">Parameter object or value to pass to bound function</param>
        /// <param name="cancellationToken">Cancellation token for bound async function</param>
        /// <typeparam name="TParam">The type of the parameter to pass to the bound function</typeparam>
        /// <typeparam name="TValue">Type of the result value type</typeparam>
        /// <returns>A result of type <typeparamref name="TValue" /></returns>
        public async Task<IResult<TValue>> Bind<TParam, TValue>(
            Func<TParam, CancellationToken, Task<IResult<TValue>>> func
          , TParam                                                 param
          , CancellationToken                                      cancellationToken)
        {
            return await result.BindResult(_ => RunParamFunctionWithCatch(func
                                                                        , param
                                                                        , cancellationToken));
        }
    }
}