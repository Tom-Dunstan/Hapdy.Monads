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
        /// <param name="param">Parameter to pass to function</param>
        /// <param name="cancellationToken">Cancellation token for asynchronous operation</param>
        /// <typeparam name="TParam">Type of parameter</typeparam>
        /// <typeparam name="TValue">Type of the result value type</typeparam>
        /// <returns>A result of type <typeparamref name="TValue" /></returns>
        public async Task<IResult<TValue>> Then<TParam, TValue>(Func<TParam, CancellationToken, Task<IResult<TValue>>> func, TParam param, CancellationToken cancellationToken)
        {
            var result = await resultTask;
            return await result.Then(func
                                   , param
                                   , cancellationToken);
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

        /// <summary>
        /// Next bound function to run
        /// </summary>
        /// <param name="func">Function to bind to result</param>
        /// <param name="param">Parameter to pass to function</param>
        /// <typeparam name="TParam">Type of parameter</typeparam>
        /// <typeparam name="TValue">Type of the result value type</typeparam>
        /// <returns>A result of type <typeparamref name="TValue" /></returns>
        public async Task<IResult<TValue>> Then<TValue, TParam>(Func<TParam, IResult<TValue>> func, TParam param)
        {
            var result = await resultTask;
            return result.Then(func, param);
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
        /// </summary>
        /// <param name="func">Function to bind to result</param>
        /// <param name="param">Parameter object or value to pass to bound function</param>
        /// <typeparam name="TParam">The type of the parameter to pass to the bound function</typeparam>
        /// <typeparam name="TValue">Type of the result value type</typeparam>
        /// <returns>A result of type <typeparamref name="TValue" /></returns>
        public IResult<TValue> Then<TParam, TValue>(Func<TParam, IResult<TValue>> func, TParam param) { return result.Bind(func, param); }

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

        /// <summary>
        /// Next bound function to run
        /// </summary>
        /// <param name="func">Function to bind to result</param>
        /// <param name="param">Parameter object or value to pass to bound function</param>
        /// <param name="cancellationToken">Cancellation token for bound async function</param>
        /// <typeparam name="TParam">The type of the parameter to pass to the bound function</typeparam>
        /// <typeparam name="TValue">Type of the result value type</typeparam>
        /// <returns>A result of type <typeparamref name="TValue" /></returns>
        public async Task<IResult<TValue>> Then<TParam, TValue>(Func<TParam, CancellationToken, Task<IResult<TValue>>> func, TParam param, CancellationToken cancellationToken)
        {
            return await result.Bind(func
                                   , param
                                   , cancellationToken);
        }
    }
}