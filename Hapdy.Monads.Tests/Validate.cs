// ReSharper disable InconsistentNaming
// ReSharper disable CheckNamespace

namespace Hapdy.Monads.Results.Testing_Validate;

[TestFixture(TestOf = typeof(IResult)
           , TestName = "Validate"
           , Category = "5 - Validate")]
[TestFixture]
public class Validate
{
    private bool _mapFunctionWasCalled;

    private const    int  TestValue         = 42;
    private readonly int? TestNullableValue = 42;
    private readonly int? TestNullValue     = null;
    private const    int  ExpectedValue     = 84;
    
    private const    string    ErrorMessage             = "Value must be greater than 30";
    private const    string    ErrorMessageNullable     = "Value must be greater than 30";
    private const    string    ExpectedExceptionMessage = "Test exception message";
    private readonly Exception _exception               = new(ExpectedExceptionMessage);
    
    [SetUp]
    public void Setup()
    {
        _mapFunctionWasCalled = false;
    }
    
    private Func<int, IResult<int>> GetMappingFunction()
    {
        return value =>
               {
                   _mapFunctionWasCalled = true;
                   _mapFunctionWasCalled = true;
                   return value > 30
                              ? Success<int>.Create(value * 2)
                              : Failure<int>.Create(ErrorMessage);
               };
    }

    private Func<int, CancellationToken, Task<IResult<int>>> GetMappingFunctionAsync()
    {
        // ReSharper disable once RedundantLambdaParameterType
        // ReSharper disable once UnusedParameter.Local
        return (int value, CancellationToken cancellationToken) =>
               {
                   _mapFunctionWasCalled = true;
                   IResult<int> result = value > 30
                                             ? Success<int>.Create(value * 2)
                                             : Failure<int>.Create(ErrorMessage);
                   return Task.FromResult(result);
               };
    }

    private Func<int?, IResult<int>> GetNullableMappingFunction()
    {
        return value =>
               {
                   _mapFunctionWasCalled = true;
                   return value == null 
                              ? Failure<int>.Create(ErrorMessageNullable) 
                              : Success<int>.Create(value.Value * 2);
               };
    }

    private Func<int?, CancellationToken, Task<IResult<int>>> GetNullableMappingFunctionAsync()
    {
        // ReSharper disable once RedundantLambdaParameterType
        // ReSharper disable once UnusedParameter.Local
        return (int? value, CancellationToken cancellationToken) =>
               {
                   _mapFunctionWasCalled = true;
                   IResult<int> result = value == null
                                             ? Failure<int>.Create(ErrorMessageNullable)
                                             : Success<int>.Create(value.Value * 2);
                   return Task.FromResult(result);
               };
    }

    private Func<int, IResult<int>> GetExceptionMappingFunction()
    {
        // ReSharper disable once UnusedParameter.Local
        return value => throw _exception;
    }

    private Func<int, CancellationToken, Task<IResult<int>>> GetExceptionMappingFunctionAsync()
    {
        // ReSharper disable once RedundantLambdaParameterType
        // ReSharper disable UnusedParameter.Local
        return (int value, CancellationToken cancellationToken) => throw _exception;
        // ReSharper restore UnusedParameter.Local
    }

    private Func<int?, IResult<int>> GetNullableExceptionMappingFunction()
    {
        // ReSharper disable once UnusedParameter.Local
        return value => throw _exception;
    }

    private Func<int?, CancellationToken, Task<IResult<int>>> GetNullableExceptionMappingFunctionAsync()
    {
        // ReSharper disable once RedundantLambdaParameterType
        // ReSharper disable UnusedParameter.Local
        return (int? value, CancellationToken cancellationToken) => throw _exception;
        // ReSharper restore UnusedParameter.Local
    }

    private void AssertSuccessResult(IResult<int> result)
    {
        Assert.That(result, Is.InstanceOf<Success<int>>());
        using (Assert.EnterMultipleScope())
        {
            Assert.That(_mapFunctionWasCalled, Is.True);
            var successResult = (Success<int>)result;
            Assert.That(successResult.Value, Is.EqualTo(ExpectedValue));
        }
    }
    
    private void AssertFailureResult(IResult<int> result)
    {
        Assert.That(result, Is.InstanceOf<Failure<int>>());
        using (Assert.EnterMultipleScope())
        {
            Assert.That(_mapFunctionWasCalled, Is.True);
            var failureResult = (Failure<int>)result;
            Assert.That(failureResult.ErrorMessage, Is.EqualTo(ErrorMessageNullable));
        }
    }

    private void AssertExceptionFailureResult(IResult<int> result)
    {
        Assert.That(result, Is.InstanceOf<ExceptionFailure<int>>());
        using (Assert.EnterMultipleScope())
        {
            Assert.That(_mapFunctionWasCalled, Is.False);
            var exceptionFailureResult = (ExceptionFailure<int>)result;
            Assert.That(exceptionFailureResult.Exception,    Is.EqualTo(_exception));
            Assert.That(exceptionFailureResult.ErrorMessage, Is.EqualTo(ExpectedExceptionMessage));
        }
    }

    [Test]
    public void When_ValueWithFunction_Then_ReturnsMappedValue()
    {
        // Arrange
        var mapFunction = GetMappingFunction();

        // Act
        var result = IResult.Validate(TestValue, mapFunction);

        // Assert
        AssertSuccessResult(result);
    }
    
    [Test]
    public async Task When_ValueWithAsyncFunction_Then_ReturnsMappedValue()
    {
        // Arrange
        var mapFunction = GetMappingFunctionAsync();

        // Act
        var result = await IResult.Validate(TestValue, mapFunction, CancellationToken.None);

        // Assert
        AssertSuccessResult(result);
    }

    [Test]
    public void When_NullableValueWithFunction_Then_ReturnsMappedValue()
    {
        // Arrange
        var mapFunction = GetNullableMappingFunction();

        // Act
        var result = IResult<int>.Validate(TestNullableValue, mapFunction);

        // Assert
        AssertSuccessResult(result);
    }
    
    [Test]
    public async Task When_NullableValueWithAsyncFunction_Then_ReturnsMappedValue()
    {
        var mapFunction = GetNullableMappingFunctionAsync();

        // Act
        var result = await IResult<int>.Validate(TestNullableValue, mapFunction, CancellationToken.None);

        // Assert
        AssertSuccessResult(result);
    }

    [Test]
    public void When_InvalidNullValueWithFunction_Then_ReturnsFailedResult()
    {
        // Arrange
        var mapFunction = GetNullableMappingFunction();

        // Act
        var result = IResult<int>.Validate(TestNullValue, mapFunction);

        // Assert
        AssertFailureResult(result);
    }
    
    [Test]
    public async Task When_InvalidNullValueWithAsyncFunction_Then_ReturnsFailedResult()
    {
        // Arrange
        var mapFunction = GetNullableMappingFunctionAsync();

        // Act
        var result = await IResult<int>.Validate(TestNullValue, mapFunction, CancellationToken.None);

        // Assert
        AssertFailureResult(result);
    }

    [Test]
    public void When_ValueWithFunctionThrowsException_Then_ReturnsExceptionFailure()
    {
        // Arrange
        var mapFunction = GetExceptionMappingFunction();

        // Act
        var result = IResult.Validate(TestValue, mapFunction);

        // Assert
        AssertExceptionFailureResult(result);
    }
    
    [Test]
    public async Task When_ValueWithAsyncFunctionThrowsException_Then_ReturnsExceptionFailure()
    {
        // Arrange
        var mapFunction  = GetExceptionMappingFunctionAsync();

        // Act
        var result = await IResult.Validate(TestValue, mapFunction, CancellationToken.None);

        // Assert
        AssertExceptionFailureResult(result);
    }

    [Test]
    public void When_NullableValueWithFunctionThrowsException_Then_ReturnsExceptionFailure()
    {
        // Arrange
        var mapFunction = GetNullableExceptionMappingFunction();

        // Act
        var result = IResult<int>.Validate(TestValue, mapFunction);

        // Assert
        AssertExceptionFailureResult(result);
    }
    
    [Test]
    public async Task When_NullableValueWithAsyncFunctionThrowsException_Then_ReturnsExceptionFailure()
    {
        // Arrange
        var mapFunction  = GetNullableExceptionMappingFunctionAsync();

        // Act
        var result = await IResult<int>.Validate(TestValue, mapFunction, CancellationToken.None);

        // Assert
        AssertExceptionFailureResult(result);
    }
}