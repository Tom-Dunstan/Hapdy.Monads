namespace Hapdy.Monads.Results.Extensions;

public static partial class ResultHelper
{
    extension<T>(Task<IResult<T>> resultTask)
    {
        public async Task<IResult<T>> Unbox()
        {
            var result = await resultTask;
            return result switch
                   {
                       IShortCircuit<T> shortCircuit => shortCircuit.Unwrap()
                     , _                             => result
                   };
        }
    }

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