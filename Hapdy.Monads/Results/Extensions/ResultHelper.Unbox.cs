// ReSharper disable MemberCanBePrivate.Global
namespace Hapdy.Monads.Results.Extensions;

public static partial class ResultHelper
{
    extension<T>(Task<IResult<T>> resultTask)
    {
        /// <summary>
        /// Unboxes a ShortCircuit result from a Task to a Success result of the same type
        /// </summary>
        /// <returns>Unboxed result</returns>
        public async Task<IResult<T>> Unbox()
        {
            var result = await resultTask;
            return result.Unbox();
        }
    }

    /// <summary>
    /// Unboxes a ShortCircuit result from a Task to a Success result of the same type
    /// </summary>
    /// <returns>Unboxed result</returns>
    extension<T>(IResult<T> result)
    {
        public IResult<T> Unbox()
        {
            return result switch
                   {
                       IShortCircuit<T> shortCircuit => shortCircuit.Unwrap()
                     , _                             => result
                   };
        }
    }
}