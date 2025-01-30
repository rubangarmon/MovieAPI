using MovieAPI.Core.Attributes;
using MovieAPI.Core.Helpers;

namespace MovieAPI.UnitTests.Core.Helpers;

public class MediaTypeHelperTests
{
    [Fact]
    public void DetermineMediaTypeUrl_WithMediaTypeUrlAttribute_ShouldReturnCorrectUrl()
    {
        // Arrange & Act
        var result = MediaTypeHelper.DetermineMediaTypeUrl<MockMediaWithAttribute>();

        // Assert
        result.ShouldBe("?movie");
    }

    [Fact]
    public void DetermineMediaTypeUrl_WithoutMediaTypeUrlAttribute_ShouldReturnEmptyString()
    {
        // Arrange & Act
        var result = MediaTypeHelper.DetermineMediaTypeUrl<MockMediaWithoutAttribute>();

        // Assert
        result.ShouldBe(string.Empty);
    }

    // Mock class with attribute
    [MediaTypeUrl("?movie")]
    private class MockMediaWithAttribute { }

    // Mock class without attribute
    private class MockMediaWithoutAttribute { }
}
