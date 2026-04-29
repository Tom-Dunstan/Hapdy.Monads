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
        /// <returns>A validated result</returns>
        public async Task<IResult<T>> Validate(
            Func<T, IResult<T>> validationFunc)
        {
            var result = await resultTask;
            return result.Validate(validationFunc);
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
        /// <param name="cancellationToken">Cancellation token for asynchronous operations</param>
        /// <returns>A validated result</returns>
        public Task<IResult<T>> Validate(
            Func<T, CancellationToken, Task<IResult<T>>> validationFunc
          , CancellationToken                            cancellationToken)
        {
            return result.Bind(validationFunc, cancellationToken);
        }
    }
}