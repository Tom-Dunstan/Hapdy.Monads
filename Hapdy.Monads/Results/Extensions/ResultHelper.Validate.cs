// ReSharper disable MemberCanBePrivate.Global
namespace Hapdy.Monads.Results.Extensions;

public static partial class ResultHelper
{
    extension<T>(Task<IResult<T>> resultTask)
    {
        /// <summary>
        /// Binds a function to a parameter
        /// </summary>
        /// <param name="validationFunc">Function to validate the result</param>
        /// <param name="cancellationToken">CancellationToken</param>
        /// <returns>A validated result</returns>
        public async Task<IResult<T>> Validate(
            Func<T, CancellationToken, Task<IResult<T>>> validationFunc
          , CancellationToken                            cancellationToken)
        {
            var result = await resultTask;
            return await result.Validate(validationFunc, cancellationToken);
        }

        /// <summary>
        /// Binds a function to a parameter
        /// </summary>
        /// <param name="validationFunc">Function to validate the result</param>
        /// <param name="param">Parameter to pass to the validation function</param>
        /// <param name="cancellationToken">CancellationToken</param>
        /// <typeparam name="TParam">Type of the parameter</typeparam>
        /// <returns>A validated result</returns>
        public async Task<IResult<T>> Validate<TParam>(
            Func<T, TParam, CancellationToken, Task<IResult<T>>> validationFunc
          , TParam                                               param
          , CancellationToken                                    cancellationToken)
        {
            var result = await resultTask;
            return await result.Validate(validationFunc
                                       , param
                                       , cancellationToken);
        }

        /// <summary>
        /// Binds a function to a parameter
        /// </summary>
        /// <param name="validationFunc">Function to validate the result</param>
        /// <returns>A validated result</returns>
        public async Task<IResult<T>> Validate(
            Func<T, IResult<T>> validationFunc)
        {
            var result = await resultTask;
            return result.Validate(validationFunc);
        }

        /// <summary>
        /// Binds a function to a parameter
        /// </summary>
        /// <param name="validationFunc">Function to validate the result</param>
        /// <param name="param">Parameter to pass to the validation function</param>
        /// <typeparam name="TParam">Type of the parameter</typeparam>
        /// <returns>A validated result</returns>
        public async Task<IResult<T>> Validate<TParam>(
            Func<T, TParam, IResult<T>> validationFunc
          , TParam                      param)
        {
            var result = await resultTask;
            return result.Validate(validationFunc, param);
        }
    }

    extension<T>(IResult<T> result)
    {
        /// <summary>
        /// Binds a function to a parameter
        /// </summary>
        /// <param name="validationFunc">Function to validate the result</param>
        /// <returns>A validated result</returns>
        public IResult<T> Validate(
            Func<T, IResult<T>> validationFunc)
        {
            return result.Bind(validationFunc);
        }

        /// <summary>
        /// Binds a function to a parameter
        /// </summary>
        /// <param name="validationFunc">Function to validate the result</param>
        /// <param name="param">Parameter to pass to the validation function</param>
        /// <typeparam name="TParam">Type of the parameter</typeparam>
        /// <returns>A validated result</returns>
        public IResult<T> Validate<TParam>(
            Func<T, TParam, IResult<T>> validationFunc
          , TParam                      param)
        {
            return result.Bind(validationFunc, param);
        }

        /// <summary>
        /// Binds a function to a parameter
        /// </summary>
        /// <param name="validationFunc">Function to validate the result</param>
        /// <param name="cancellationToken">Cancellation token for asynchronous operations</param>
        /// <returns>A validated result</returns>
        public Task<IResult<T>> Validate(
            Func<T, CancellationToken, Task<IResult<T>>> validationFunc
          , CancellationToken                            cancellationToken)
        {
            return result.Bind(validationFunc, cancellationToken);
        }

        /// <summary>
        /// Binds a function to a parameter
        /// </summary>
        /// <param name="validationFunc">Function to validate the result</param>
        /// <param name="cancellationToken">Cancellation token for asynchronous operations</param>
        /// <param name="param">Parameter to pass to the validation function</param>
        /// <returns>A validated result</returns>
        public Task<IResult<T>> Validate<TParam>(
            Func<T, TParam, CancellationToken, Task<IResult<T>>> validationFunc
          , TParam                                               param
          , CancellationToken                                    cancellationToken)
        {
            return result.Bind(validationFunc
                             , param
                             , cancellationToken);
        }
    }
}