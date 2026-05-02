// ReSharper disable InconsistentNaming
// ReSharper disable CheckNamespace

namespace Hapdy.Monads.Results.Testing_Success;

[TestFixture(TestOf = typeof(Success<>)
           , TestName = "Success"
           , Category = "0 - Results")]
public class Success
{
    [SetUp] public void Setup() { }

    [Test]
    public void When_TestingIsSuccess_Then_ReturnsTrue()
    {
        // Arrange
        IResult<int> result = Success<int>.Create(42);

        // Act
        var isSuccess = result.IsSuccess;
        var isFailure = result.IsFailure;

        // Assert
        Assert.Multiple(() =>
                        {
                            Assert.That(isSuccess, Is.True);
                            Assert.That(isFailure, Is.False);
                        });
    }

}