namespace Hapdy.Monads.Results;

/// <inheritdoc cref="IExceptionFailure{T}" />
public readonly record struct ExceptionFailure<T>(Exception Exception) : IExceptionFailure<T>
{
    public string ErrorMessage => Exception.Message;

    public bool IsSuccess => false;

    public bool IsFailure => true;

    public static ExceptionFailure<T> Create(Exception exception) => new(exception);
}