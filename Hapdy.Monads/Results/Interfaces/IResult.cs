namespace Hapdy.Monads.Results;

/// <summary>
/// A monadic result type
/// </summary>
public interface IResult
{
    /// <summary>
    /// Indicates whether the result is a success
    /// </summary>
    bool IsSuccess { get; }

    /// <summary>
    /// Indicates whether the result is a failure
    /// </summary>
    bool IsFailure { get; }

    /// <summary>
    /// Map a value to a result
    /// </summary>
    /// <param name="value">Value to be mapped</param>
    /// <typeparam name="T">Type of value</typeparam>
    /// <returns>Result of mapping</returns>
    public static IResult<T> Map<T>(T value) => Success<T>.Create(value);

    /// <summary>
    /// Map a value to a result
    /// </summary>
    /// <param name="value">Value to be mapped</param>
    /// <param name="func">Function to map value to result</param>
    /// <returns>Result of mapping</returns>
    public static IResult<TValue> Map<T, TValue>(
        T?                        value
      , Func<T?, IResult<TValue>> func)
        where T : notnull
    {
        try
        {
            return func(value);
        }
        catch (Exception e)
        {
            return new ExceptionFailure<TValue>(e);
        }
    }

    /// <summary>
    /// Map a value to a result
    /// </summary>
    /// <param name="value">Value to be mapped</param>
    /// <param name="func">Function to map value to result</param>
    /// <returns>Result of mapping</returns>
    public static async Task<IResult<TValue>> Map<T, TValue>(
        T?                              value
      , Func<T?, Task<IResult<TValue>>> func)
        where T : notnull
    {
        try
        {
            return await func(value);
        }
        catch (Exception e)
        {
            return new ExceptionFailure<TValue>(e);
        }
    }

    /// <summary>
    /// Map a value to a result
    /// </summary>
    /// <param name="value">Value to be mapped</param>
    /// <param name="func">Function to map value to result</param>
    /// <param name="cancellationToken">Cancellation token for asynchronous operations</param>
    /// <returns>Result of mapping</returns>
    public static async Task<IResult<TValue>> Map<T, TValue>(
        T?                                                 value
      , Func<T?, CancellationToken, Task<IResult<TValue>>> func
      , CancellationToken                                  cancellationToken)
        where T : notnull
    {
        try
        {
            return await func(value, cancellationToken);
        }
        catch (Exception e)
        {
            return new ExceptionFailure<TValue>(e);
        }
    }

    /// <summary>
    /// Validates a value/model
    /// </summary>
    /// <param name="value">Value/model to be validated</param>
    /// <param name="func">Validation function</param>
    /// <returns>Result of validation</returns>
    public static IResult<TValue> Validate<T, TValue>(
        T?                        value
      , Func<T?, IResult<TValue>> func)
        where T : notnull
        => Map(value, func);

    /// <summary>
    /// Validates a value/model
    /// </summary>
    /// <param name="value">Value/model to be validated</param>
    /// <param name="func">Validation function</param>
    /// <returns>Result of validation</returns>
    public static Task<IResult<TValue>> Validate<T, TValue>(
        T?                              value
      , Func<T?, Task<IResult<TValue>>> func)
        where T : notnull
        => Map(value, func);

    /// <summary>
    /// Validates a value/model
    /// </summary>
    /// <param name="value">Value/model to be validated</param>
    /// <param name="func">Validation function</param>
    /// <param name="cancellationToken">Cancellation token for asynchronous operations</param>
    /// <returns>Result of validation</returns>
    public static Task<IResult<TValue>> Validate<T, TValue>(
        T?                                                 value
      , Func<T?, CancellationToken, Task<IResult<TValue>>> func
      , CancellationToken                                  cancellationToken)
        where T : notnull
        => Map(value
             , func
             , cancellationToken);
}

/// <inheritdoc />
public interface IResult<out T> : IResult
{
    /// <summary>
    /// Map a value to a result asynchronously
    /// </summary>
    /// <param name="value">Value to be mapped</param>
    /// <param name="func">Function to map value to result</param>
    /// <typeparam name="TValue">Type of value</typeparam>
    /// <returns>Result of mapping</returns>
    public static async Task<IResult<T>> Map<TValue>(TValue value, Func<TValue, Task<IResult<T>>> func)
    {
        try
        {
            return await func(value);
        }
        catch (Exception e)
        {
            return new ExceptionFailure<T>(e);
        }
    }

    /// <summary>
    /// Map a value to a result
    /// </summary>
    /// <param name="value">Value to be mapped</param>
    /// <param name="func">Function to map value to result</param>
    /// <returns>Result of mapping</returns>
    public static async Task<IResult<T>> Map(T value, Func<T, Task<IResult<T>>> func) { return await Map<T>(value, func); }

    /// <summary>
    /// Map a value to a result
    /// </summary>
    /// <param name="value">Value to be mapped</param>
    /// <param name="func">Function to map value to result</param>
    /// <typeparam name="TValue">Type of value</typeparam>
    /// <returns>Result of mapping</returns>
    public static IResult<T> Map<TValue>(TValue value, Func<TValue, IResult<T>> func)
    {
        try
        {
            return func(value);
        }
        catch (Exception e)
        {
            return new ExceptionFailure<T>(e);
        }
    }

    /// <summary>
    /// Map a value to a result
    /// </summary>
    /// <param name="value">Value to be mapped</param>
    /// <param name="func">Function to map value to result</param>
    /// <returns>Result of mapping</returns>
    public static IResult<T> Map(T value, Func<T, IResult<T>> func) { return Map<T>(value, func); }

    /// <summary>
    /// Map a value to a result
    /// </summary>
    /// <param name="value">Value to be mapped</param>
    /// <returns>Result of mapping</returns>
    public static IResult<T> Map(T value) => IResult.Map(value);

    /// <summary>
    /// Validates a value/model
    /// </summary>
    /// <param name="value">Value/model to be validated</param>
    /// <param name="func">Validation function</param>
    /// <returns>Result of validation</returns>
    public static IResult<T> Validate(T value, Func<T, IResult<T>> func) => Map(value, func);

    /// <summary>
    /// Validates a value/model
    /// </summary>
    /// <param name="value">Value/model to be validated</param>
    /// <param name="func">Validation function</param>
    /// <returns>Result of validation</returns>
    public static IResult<T> Validate<TValue>(TValue value, Func<TValue, IResult<T>> func) => Map(value, func);

    /// <summary>
    /// Validates a value/model
    /// </summary>
    /// <param name="value">Value/model to be validated</param>
    /// <param name="func">Validation function</param>
    /// <returns>Result of validation</returns>
    public static Task<IResult<T>> Validate(T value, Func<T, Task<IResult<T>>> func) => Map(value, func);

    /// <summary>
    /// Validates a value/model
    /// </summary>
    /// <param name="value">Value/model to be validated</param>
    /// <param name="func">Validation function</param>
    /// <returns>Result of validation</returns>
    public static Task<IResult<T>> Validate<TValue>(TValue value, Func<TValue, Task<IResult<T>>> func) => Map(value, func);
}