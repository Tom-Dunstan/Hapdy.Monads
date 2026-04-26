namespace Hapdy.Monads.Results;

/// <inheritdoc cref="ISuccess{T}" />
public readonly record struct Success<T>(T Value) : ISuccess<T>
{
    public bool IsSuccess => true;

    public bool IsFailure => false;

    public static Success<T> Create(T value) => new(value);
}