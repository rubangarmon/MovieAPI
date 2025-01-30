using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using MovieAPI.Application.Extensions;
using Shouldly;
using Xunit;


namespace MovieAPI.UnitTests.Application.Extensions;

public class ValidationResultExtensionTests
{
    [Fact]
    public void SendErrorAsValidationProblem_ShouldReturnModelStateDictionary_WithErrors()
    {
        // Arrange
        var validationResult = new ValidationResult(
            [
                new ValidationFailure("Property1", "Error message 1"),
                new ValidationFailure("Property2", "Error message 2")
            ]);

        // Act
        var modelStateDictionary = validationResult.SendErrorAsValidationProblem();

        // Assert
        modelStateDictionary.ShouldNotBeNull();
        modelStateDictionary.IsValid.ShouldBeFalse();
        modelStateDictionary.Count.ShouldBe(2);
        modelStateDictionary.ContainsKey("Property1").ShouldBeTrue();
        modelStateDictionary.ContainsKey("Property2").ShouldBeTrue();
        modelStateDictionary["Property1"]?.Errors[0].ErrorMessage.ShouldBe("Error message 1");
        modelStateDictionary["Property2"]?.Errors[0].ErrorMessage.ShouldBe("Error message 2");
    }

    [Fact]
    public void SendErrorAsValidationProblem_ShouldReturnEmptyModelStateDictionary_WhenNoErrors()
    {
        // Arrange
        var validationResult = new ValidationResult();

        // Act
        var modelStateDictionary = validationResult.SendErrorAsValidationProblem();

        // Assert
        modelStateDictionary.ShouldNotBeNull();
        modelStateDictionary.IsValid.ShouldBeTrue();
        modelStateDictionary.Count.ShouldBe(0);
    }
}
