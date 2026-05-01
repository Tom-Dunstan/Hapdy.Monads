// ReSharper disable InconsistentNaming
// ReSharper disable CheckNamespace

namespace Hapdy.Monads.Results.Testing_Success;

[TestFixture(TestOf = typeof(Failure<>)
           , TestName = "Failure"
           , Category = "0 - Results")]
public class Failure
{
    [SetUp] public void Setup() { }

    [Test]
    public void When_TestingIsSuccess_Then_ReturnsFalse()
    {
        // Arrange
        var result = Failure<int>.Create("Testing failure result.");

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