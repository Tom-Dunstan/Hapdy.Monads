// ReSharper disable MemberCanBePrivate.Global
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
    }

    extension<T>(IResult<T> result)
    {
        private IResult<TValue> BindResult<TValue>(Func<T, IResult<TValue>> func)
        {
            return RunFunctionWithCatch(() => result switch
                                              {
                                                  ISuccess<T> success                   => func(success.Value)
                                                , IExceptionFailure<T> exceptionFailure => ExceptionFailure<TValue>.Create(exceptionFailure.Exception)
                                                , IFailure<T> failure                   => Failure<TValue>.Create(failure.ErrorMessage)
#pragma warning disable CA2208
                                                , _ => ExceptionFailure<TValue>.Create(new ArgumentOutOfRangeException(nameof(result)))
#pragma warning restore CA2208
                                              });
        }

        private async Task<IResult<TValue>> BindResultAsync<TValue>(Func<T, Task<IResult<TValue>>> func)
        {
            return await RunFunctionWithCatchAsync(async () => result switch
                                                               {
                                                                   ISuccess<T> success                   => await func(success.Value).ConfigureAwait(false)
                                                                 , IExceptionFailure<T> exceptionFailure => ExceptionFailure<TValue>.Create(exceptionFailure.Exception)
                                                                 , IFailure<T> failure                   => Failure<TValue>.Create(failure.ErrorMessage)
#pragma warning disable CA2208
                                                                 , _ => ExceptionFailure<TValue>.Create(new ArgumentOutOfRangeException(nameof(result)))
#pragma warning restore CA2208
                                                               });
        }

        /// <summary>
        /// Binds a function to a parameter
        /// </summary>
        /// <param name="func">Function to bind to result</param>
        /// <returns>A result of type <typeparamref name="TValue" /></returns>
        public IResult<TValue> Bind<TValue>(Func<T, IResult<TValue>> func) { return result.BindResult(func); }

        /// <summary>
        /// Binds a function to a parameter
        /// </summary>
        /// <param name="func">Function to bind to result</param>
        /// <typeparam name="TValue">Type of the result value type</typeparam>
        /// <returns>A result of type <typeparamref name="TValue" /></returns>
        public IResult<TValue> Bind<TValue>(Func<IResult<TValue>> func) { return result.BindResult(_ => func()); }

        /// <summary>
        /// Binds a function to a parameter
        /// </summary>
        /// <param name="func">Function to bind to result</param>
        /// <param name="cancellationToken">Cancellation token for the bound async function</param>
        /// <typeparam name="TValue">Type of the result value type</typeparam>
        /// <returns>A result of type <typeparamref name="TValue" /></returns>
        public Task<IResult<TValue>> Bind<TValue>(
            Func<T, CancellationToken, Task<IResult<TValue>>> func
          , CancellationToken                                 cancellationToken)
        {
            return result.BindResultAsync<T, TValue>(value => func(value
                                                                 , cancellationToken));
        }

        /// <summary>
        /// Binds a function to a parameter
        /// </summary>
        /// <param name="func">Function to bind to result</param>
        /// <param name="cancellationToken">Cancellation token for the bound async function</param>
        /// <typeparam name="TValue">Type of the result value type</typeparam>
        /// <returns>A result of type <typeparamref name="TValue" /></returns>
        public Task<IResult<TValue>> Bind<TValue>(
            Func<CancellationToken, Task<IResult<TValue>>> func
          , CancellationToken                              cancellationToken)
        {
            return result.BindResultAsync(_ => func(cancellationToken));
        }
    }
}