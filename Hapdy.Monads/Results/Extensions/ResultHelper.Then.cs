// ReSharper disable MemberCanBePrivate.Global
namespace Hapdy.Monads.Results.Extensions;

public static partial class ResultHelper
{
    extension<T>(Task<IResult<T>> resultTask)
    {
        /// <summary>
        /// Next bound function to run
        /// </summary>
        /// <param name="func">Function to bind to result</param>
        /// <param name="cancellationToken">Cancellation token for asynchronous operation</param>
        /// <typeparam name="TValue">Type of the result value type</typeparam>
        /// <returns>A result of type <typeparamref name="TValue" /></returns>
        public async Task<IResult<TValue>> Then<TValue>(Func<T, CancellationToken, Task<IResult<TValue>>> func, CancellationToken cancellationToken)
        {
            var result = await resultTask;
            return await result.Then(func, cancellationToken);
        }

        /// <summary>
        /// Next bound function to run
        /// </summary>
        /// <param name="func">Function to bind to result</param>
        /// <param name="cancellationToken">Cancellation token for asynchronous operation</param>
        /// <typeparam name="TValue">Type of the result value type</typeparam>
        /// <returns>A result of type <typeparamref name="TValue" /></returns>
        public async Task<IResult<TValue>> Then<TValue>(Func<CancellationToken, Task<IResult<TValue>>> func, CancellationToken cancellationToken)
        {
            var result = await resultTask;
            return await result.Then(func, cancellationToken);
        }

        /// <summary>
        /// Next bound function to run
        /// </summary>
        /// <param name="func">Function to bind to result</param>
        /// <typeparam name="TValue">Type of the result value type</typeparam>
        /// <returns>A result of type <typeparamref name="TValue" /></returns>
        public async Task<IResult<TValue>> Then<TValue>(Func<T, IResult<TValue>> func)
        {
            var result = await resultTask;
            return result.Then(func);
        }

        /// <summary>
        /// Next bound function to run
        /// </summary>
        /// <param name="func">Function to bind to result</param>
        /// <typeparam name="TValue">Type of the result value type</typeparam>
        /// <returns>A result of type <typeparamref name="TValue" /></returns>
        public async Task<IResult<TValue>> Then<TValue>(Func<IResult<TValue>> func)
        {
            var result = await resultTask;
            return result.Then(func);
        }

    }

    extension<T>(IResult<T> result)
    {
        /// <summary>
        /// Next bound function to run
        /// </summary>
        /// <param name="func">Function to bind to result</param>
        /// <returns>A result of type <typeparamref name="TValue" /></returns>
        public IResult<TValue> Then<TValue>(Func<T, IResult<TValue>> func) { return result.Bind(func); }

        /// <summary>
        /// Next bound function to run
        /// </summary>
        /// <param name="func">Function to bind to result</param>
        /// <typeparam name="TValue">Type of the result value type</typeparam>
        /// <returns>A result of type <typeparamref name="TValue" /></returns>
        public IResult<TValue> Then<TValue>(Func<IResult<TValue>> func) { return result.Bind(func); }

        /// <summary>
        /// Next bound function to run
        /// </summary>
        /// <param name="func">Function to bind to result</param>
        /// <param name="cancellationToken">Cancellation token for bound async function</param>
        /// <typeparam name="TValue">Type of the result value type</typeparam>
        /// <returns>A result of type <typeparamref name="TValue" /></returns>
        public async Task<IResult<TValue>> Then<TValue>(Func<T, CancellationToken, Task<IResult<TValue>>> func, CancellationToken cancellationToken) { return await result.Bind(func, cancellationToken); }

        /// <summary>
        /// Next bound function to run
        /// </summary>
        /// <param name="func">Function to bind to result</param>
        /// <param name="cancellationToken">Cancellation token for bound async function</param>
        /// <typeparam name="TValue">Type of the result value type</typeparam>
        /// <returns>A result of type <typeparamref name="TValue" /></returns>
        public async Task<IResult<TValue>> Then<TValue>(Func<CancellationToken, Task<IResult<TValue>>> func, CancellationToken cancellationToken) { return await result.Bind(func, cancellationToken); }
    }
}