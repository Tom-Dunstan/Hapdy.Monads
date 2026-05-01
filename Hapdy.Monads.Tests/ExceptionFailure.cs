// ReSharper disable InconsistentNaming
// ReSharper disable CheckNamespace

namespace Hapdy.Monads.Results.Testing_ExceptionFailure;

[TestFixture(TestOf = typeof(ExceptionFailure<>)
           , TestName = "ExceptionFailure"
           , Category = "0 - Results")]
public class ExceptionFailure
{
    [SetUp] public void Setup() { }

    [Test]
    public void When_TestingIsSuccess_Then_ReturnsFalse()
    {
        // Arrange
        const string testErrorMessage = "Test Error Message";
        var          testException    = new Exception(testErrorMessage);
        var          result           = ExceptionFailure<int>.Create(testException);

        // Act
        var isSuccess = result.IsSuccess;
        var isFailure = result.IsFailure;

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(isSuccess, Is.False);
            Assert.That(isFailure, Is.True);
        });
    }

}