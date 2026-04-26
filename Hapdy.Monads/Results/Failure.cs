namespace Hapdy.Monads.Results;

/// <inheritdoc cref="IFailure{T}" />
public readonly record struct Failure<T>(string ErrorMessage) : IFailure<T>
{
    public bool IsSuccess => false;

    public bool IsFailure => true;

    public static Failure<T> Create(string errorMessage) => new(errorMessage);
}