namespace Hapdy.Monads.Results;

/// <summary>
/// A monadic result type for a successful operation that will short-circuit further operations
/// </summary>
/// <typeparam name="T">The type of the result value</typeparam>
public interface IShortCircuit<out T> : IResult<T>
{
    /// <summary>
    /// Boxed result value
    /// </summary>
    object Value { get; }

    /// <summary>
    /// Method to unwrap the result value
    /// </summary>
    /// <returns></returns>
    IResult<T> Unwrap();
}