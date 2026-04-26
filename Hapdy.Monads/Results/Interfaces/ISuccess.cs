namespace Hapdy.Monads.Results;

/// <summary>
/// A monadic result type for a successful operation
/// </summary>
/// <typeparam name="T">The type of the result value</typeparam>
public interface ISuccess<out T> : IResult<T>
{
    /// <summary>
    /// The result value
    /// </summary>
    T Value { get; }
}