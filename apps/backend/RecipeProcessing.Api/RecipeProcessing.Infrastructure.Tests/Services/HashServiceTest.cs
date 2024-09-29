using System;
using System.IO;
using System.Text;
using RecipeProcessing.Infrastructure.Services;
using Xunit;

namespace RecipeProcessing.Infrastructure.Tests.Services;

public class HashServiceTests
{
    private readonly HashService _hashService = new();

    [Fact]
    public void ComputeFromStream_ShouldThrowArgumentNullException_WhenStreamIsNull()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => _hashService.ComputeFromStream(null));
    }

    [Fact]
    public void ComputeFromStream_ShouldThrowArgumentException_WhenStreamIsEmpty()
    {
        // Arrange
        using var emptyStream = new MemoryStream();

        // Act & Assert
        Assert.Throws<ArgumentException>(() => _hashService.ComputeFromStream(emptyStream));
    }

    [Fact]
    public void ComputeFromStream_ShouldReturnCorrectHash_WhenStreamIsValid()
    {
        // Arrange
        var inputString = "Test data";
        using var inputStream = new MemoryStream(Encoding.UTF8.GetBytes(inputString));

        // Act
        var result = _hashService.ComputeFromStream(inputStream);

        // Assert
        var expectedHash = "e27c8214be8b7cf5bccc7c08247e3cb0c1514a48ee1f63197fe4ef3ef51d7e6f";
        Assert.Equal(expectedHash, result);
    }
}