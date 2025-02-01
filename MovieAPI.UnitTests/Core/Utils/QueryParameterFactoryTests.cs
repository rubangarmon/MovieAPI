using MovieAPI.Core.Utils;

namespace MovieAPI.UnitTests.Core.Utils;

public class QueryParameterFactoryTests
{
    [Fact]
    public void CreateQueryParameterByName_WithValidNameAndPage_ShouldReturnCorrectDictionary()
    {
        // Arrange
        var name = "test";
        var page = 1;
        // Act
        var result = QueryParameterFactory.CreateQueryParameterByName(name, page);
        // Assert
        result.ShouldNotBeNull();
        result.ShouldContainKeyAndValue("query", name);
        result.ShouldContainKeyAndValue("include_adult", false.ToString());
        result.ShouldContainKeyAndValue("language", "en-US");
        result.ShouldContainKeyAndValue("page", page.ToString());
    }

    [Fact]
    public void CreateQueryParameterByName_WithEmptyNameAndPage_ShouldReturnCorrectDictionary()
    {
        // Arrange
        var name = string.Empty;
        var page = 0;
        // Act
        var result = QueryParameterFactory.CreateQueryParameterByName(name, page);
        // Assert
        result.ShouldNotBeNull();
        result.ShouldContainKeyAndValue("query", name);
        result.ShouldContainKeyAndValue("include_adult", false.ToString());
        result.ShouldContainKeyAndValue("language", "en-US");
        result.ShouldContainKeyAndValue("page", page.ToString());
    }

    [Fact]
    public void CreateQueryParameterByName_WithNullNameAndPage_ShouldReturnCorrectDictionary()
    {
        // Arrange
        string name = null;
        var page = 0;
        // Act
        var result = QueryParameterFactory.CreateQueryParameterByName(name, page);
        // Assert
        result.ShouldNotBeNull();
        result.ShouldContainKeyAndValue("query", name);
        result.ShouldContainKeyAndValue("include_adult", false.ToString());
        result.ShouldContainKeyAndValue("language", "en-US");
        result.ShouldContainKeyAndValue("page", page.ToString());
    }

    [Fact]
    public void CreateQueryParameterByName_WithValidNameAndNegativePage_ShouldReturnCorrectDictionary()
    {
        // Arrange
        var name = "test";
        var page = -1;
        // Act
        var result = QueryParameterFactory.CreateQueryParameterByName(name, page);
        // Assert
        result.ShouldNotBeNull();
        result.ShouldContainKeyAndValue("query", name);
        result.ShouldContainKeyAndValue("include_adult", false.ToString());
        result.ShouldContainKeyAndValue("language", "en-US");
        result.ShouldContainKeyAndValue("page", page.ToString());
    }

    [Fact]
    public void CreateQueryParameterByName_WithValidNameAndMaxPage_ShouldReturnCorrectDictionary()
    {
        // Arrange
        var name = "test";
        var page = int.MaxValue;
        // Act
        var result = QueryParameterFactory.CreateQueryParameterByName(name, page);
        // Assert
        result.ShouldNotBeNull();
        result.ShouldContainKeyAndValue("query", name);
        result.ShouldContainKeyAndValue("include_adult", false.ToString());
        result.ShouldContainKeyAndValue("language", "en-US");
        result.ShouldContainKeyAndValue("page", page.ToString());
    }
}
