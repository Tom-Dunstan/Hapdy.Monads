# Hapdy.Monads
## Description ##
Monad tools for C# coding. Follows the Programming on Rails, and Resulting patterns

## Result Monads ##
Result monad is a monad that represents a computation that may fail, and provides a way to handle errors in a functional way. These tools are based on the Result monad from Haskell, and include extension methods designed to support the Programming on Rails pattern.

- [IResult<T> Interface](#iresult-interface)
  - [Success<T>](#success)
  - [Failure<T>](#failure)
  - [ExceptionFailure<T>](#exceptionfailure)
  - [ShortCircuit<T>](#shortcircuit)
- [Extensions](#extensions)
  - [Bind()](#bind)
  - [Map()](#map)
  - [Match()](#match)
  - [OnFailure()](#onfailure)
  - [Catch()](#catch)
  - [Validate()](#validate)
  - [Then()](#then)
  - [Unbox()](#unbox)


### IResult<T> Interface ###
All monads implement this interface, and is the main expected return type for all monad methods. The included extension methods expect function to return this type.

Has two properties:
- IsSuccess: A boolean indicating the monad represents a successful operation if true.
- IsFailure: A boolean indicating the monad represents a failed operation if true.

### Success<T> ###
A monad that represents a successful computation of an operation.

The Success monad contains the resulting value of the operation, which is of type T.

```csharp
// Example of a successful operation that returns an int of 10
return new Success<int>(10);

// The Create class method can be used to create a monad as well.
return Success<int>.Create(10);
```

### Failure<T> ###
A monad that represents a failed computation of an operation.

The Failure monad contains the error message that represents the returned failure reasoning.

```csharp
// Example of a failed operation that returns an error message
return Failure<int>.Create("Cannot divide by zero");
```

### ExceptionFailure<T> ###
A monad that represents a failed computation of an operation, and wraps an exception.

The Failure monad contains the exception that was thrown during the operation as well as the error message of a Failure monad.

```csharp
// Example of a failed operation that returns an exception

try
{
    return Success<int>.Create(10 / x);
}
catch (DivideByZeroException e)
{
    return ExceptionFailure<int>.Create(e);
}
```

### ShortCircuit<T> ###
A monad that represents a successful completion of an operation, but is not expected to trigger the next operation.

As and example an operation may result in an empty array. This is may not be a failure in a search operation, however the subsequent operations may not be required so the system can short circuit to the end.

The ShortCircuit<T> monad contains the value of the operation similar to the Success monad. However as in must traverse subsequent operations, the value is boxed as an object. As such, to retrieve the value it must be unboxed via the Unbox method.

**Note:** This is an experimental feature, as it breaks separation of concerns.

```csharp
// Example of a successful operation that returns an empty array
return ShortCircuit<int>.Create(new int[]);
```

## Extensions ##

### Bind() ###
Binds the value of the IResult<T> to the next operation.

This is the main method used to chain monads together.

```csharp
// Example of chaining monads together
Success<int> result = Success<int>.Create(5)
                                  .Bind(x => new Success<int>(x * 2));

Console.WriteLine(result.Value); // Prints 10
```

Multiple monads can be chained together, the return of the next function's IResult<T> does not need to be of the same type.

```csharp
// Example of chaining monads together with different return types
IResult<string> result = Success<int>(5).Create
                                        .Bind(x => new Success<string>(x.ToString()));
```

### Map() ###
Can be used to crate an IResult<T> from a value or existing IResult<T>.
```csharp
// Example of creating an IResult<T> from an existing value
var result = IResult.Map<int>(5);
```

It can also take a transform function to map the value of the IResult<T>.
```csharp
// Example of creating an IResult<T> from an existing value
var result = IResult.Map<int>(5, Success<int>.Create(x => x * 2));
```

### Match() ###
Matches the IResult<T> to the appropriate action based on the type of IResult<T>.
```csharp
// Example of matching the IResult<T> to the appropriate action
IResult<string> result = IResult<string> Celebrate(int value) 
    return Success<string>.Create("The value is greater than zero! :)");

IResult<string> result = IResult<string> Morn(Failure<int> failure, int value) 
    return Success<string>.Create($"Oh no! {Failure.ErrorMessage} :(");

IResult<string> result = IResult.Map(5)
                                .Validate(x => x > 0 ? Success<int>.Create(x) : Failure<int>.Create("Value must be greater than zero"))
                                .Match(celebrate,morn);
```

### OnFailure() ###
Executes the provided action if the IResult<T> is a Failure.
```csharp
// Example of executing the provided action if the IResult<T> is a Failure
IResult<int> fail = Failure<int>.Create(
    message: "An error occurred"
);
var result = fail.OnFailure(failure => Console.WriteLine(failure.ErrorMessage));
```

### Catch() ###
Executes the provided action if the IResult<T> is a Failure.
```csharp
// Example of executing the provided action if the IResult<T> is a Failure
IResult<int> result = ExceptionFailure<int>.Create( new DivideByZeroException() );
result.Catch(exceptionFailure => Console.WriteLine(exceptionFailure.Exception.ErrorMessage));
```

### Validate() ###
Validates the IResult<T> against a predicate.
```csharp
// Example of validating the IResult<T> against a predicate
IResult<int> result = Success<int>.Create(5);
var result.Validate(x => x < 0 
                      ? Failure<int>.Create("Value must be greater than zero")
                      : Success<int>.Create(x));
```

### Then() ###
Is an alias for Bind(). Using the Then method is the same as using the Bind method, but creates a semantic flow that mimics English.

```csharp
// Example of using the Then method
IRestult<int> PrintValueOfX = x => Console.WriteLine(x);

var result = IResult.Map(5)
                    .Then(PrintValueOfX);
```

### Unbox() ###
Unboxes the value of a ShortCircuit<T> monad and returns a Success<T>.
```csharp
// Example of unboxing the value of a ShortCircuit<T> monad
ShortCircuit<int> shortCircuit = ShortCircuit<int>.Create(5);
int value = shortCircuit.Unbox().Value;
```