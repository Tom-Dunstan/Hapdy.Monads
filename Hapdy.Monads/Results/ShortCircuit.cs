namespace Hapdy.Monads.Results;

/// <inheritdoc cref="ISuccess{T}" />
public readonly record struct ShortCircuit<T>(object Value) : IShortCircuit<T>
{
    public bool IsSuccess => true;

    public bool IsFailure => false;

    public IResult<T> Unwrap()
    {
        if (Value is T value) return Success<T>.Create(value);

        try
        {
            var valueCast = (T)Value;
            return Success<T>.Create(valueCast);
        }
        catch (Exception e)
        {
            return ExceptionFailure<T>.Create(e);
        }
    }

    public static ShortCircuit<T> Create(object value) => new(value);
}