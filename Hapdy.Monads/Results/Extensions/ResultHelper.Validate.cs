namespace Hapdy.Monads.Results.Extensions;

public static partial class ResultHelper
{
    extension<T>(Task<IResult<T>> resultTask)
    {
        public async Task<IResult<T>> Validate(
            Func<T, CancellationToken, Task<IResult<T>>> validationFunc
          , CancellationToken                            cancellationToken)
        {
            var result = await resultTask;
            return await result.Validate(validationFunc, cancellationToken);
        }

        public async Task<IResult<T>> Validate(
            Func<T, IResult<T>> validationFunc
          , CancellationToken   cancellationToken)
        {
            var result = await resultTask;
            return result.Validate(validationFunc, cancellationToken);
        }
    }

    extension<T>(IResult<T> result)
    {
        public IResult<T> Validate(
            Func<T, IResult<T>> validationFunc
          , CancellationToken   cancellationToken)
        {
            return result.Bind(validationFunc);
        }

        public Task<IResult<T>> Validate(
            Func<T, CancellationToken, Task<IResult<T>>> validationFunc
          , CancellationToken                            cancellationToken)
        {
            return result.Bind(validationFunc, cancellationToken);
        }
    }
}