namespace Hapdy.Monads.Results;

/// <summary>
/// A monadic result type for an operation that threw an exception
/// </summary>
public interface IExceptionFailure<out T> : IFailure<T>
{
    /// <summary>
    /// The error message of a failed result
    /// </summary>
    Exception Exception { get; }
}