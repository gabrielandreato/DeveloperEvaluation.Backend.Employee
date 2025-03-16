using EmployesAPI.Test.Entities.User.TestData;
using FluentValidation.TestHelper;
using ModelLibrary.Validation;

namespace EmployesAPI.Test.Entities.User;

/// <summary>
///     Contains unit tests for the UserValidator class.
/// </summary>
public class UserTests
{
    private readonly UserValidator _validator;

    /// <summary>
    ///     Initializes a new instance of the <see cref="UserTests" /> class.
    /// </summary>
    public UserTests()
    {
        _validator = new UserValidator();
    }

    /// <summary>
    ///     Tests that a user with a null username should produce a validation error.
    /// </summary>
    [Fact]
    public void Should_Have_Error_When_UserName_Is_Null()
    {
        // Arrange
        var user = UserTestData.GenerateValidUser();
        user.UserName = null;

        // Act
        var result = _validator.TestValidate(user);

        // Assert
        result.ShouldHaveValidationErrorFor(u => u.UserName);
    }

    /// <summary>
    ///     Tests that a user with an invalid email should produce a validation error.
    /// </summary>
    [Fact]
    public void Should_Have_Error_When_Email_Is_Invalid()
    {
        // Arrange
        var user = UserTestData.GenerateValidUser();
        user.Email = "invalid.email";

        // Act
        var result = _validator.TestValidate(user);

        // Assert
        result.ShouldHaveValidationErrorFor(u => u.Email);
    }

    /// <summary>
    ///     Tests that a user with a weak password should produce a validation error.
    /// </summary>
    [Fact]
    public void Should_Have_Error_When_Password_Is_Weak()
    {
        // Arrange
        var user = UserTestData.GenerateValidUser();
        user.Password = "weakpass1"; // Missing uppercase and special character

        // Act
        var result = _validator.TestValidate(user);

        // Assert
        result.ShouldHaveValidationErrorFor(u => u.Password);
    }

    /// <summary>
    ///     Tests that RePassword not matching Password should produce a validation error.
    /// </summary>
    [Fact]
    public void Should_Have_Error_When_RePassword_Does_Not_Match_Password()
    {
        // Arrange
        var user = UserTestData.GenerateValidUser();
        user.RePassword = "Mismatch1!";

        // Act
        var result = _validator.TestValidate(user);

        // Assert
        result.ShouldHaveValidationErrorFor(u => u.RePassword);
    }

    /// <summary>
    ///     Tests that a fully valid user should have no validation errors.
    /// </summary>
    [Fact]
    public void Should_Not_Have_Error_When_User_Is_Valid()
    {
        // Arrange
        var user = UserTestData.GenerateValidUser();

        // Act
        var result = _validator.TestValidate(user);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
}