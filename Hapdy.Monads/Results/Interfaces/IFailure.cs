namespace Hapdy.Monads.Results;

/// <summary>
/// A monadic result type for a failed operation
/// </summary>
public interface IFailure<out T> : IResult<T>
{
    /// <summary>
    /// The error message of a failed result
    /// </summary>
    string ErrorMessage { get; }
}