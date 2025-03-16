using FluentValidation;
using ModelLibrary.Entities;

namespace ModelLibrary.Validation;

/// <summary>
///     Provides validation rules for the User entity using FluentValidation.
/// </summary>
public class UserValidator : AbstractValidator<User>
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="UserValidator" /> class.
    ///     Defines validation rules for the User entity properties.
    /// </summary>
    public UserValidator()
    {
        RuleFor(user => user.UserName)
            .NotEmpty().WithMessage("Username is required.");

        RuleFor(user => user.FirstName)
            .NotEmpty().WithMessage("First name is required.");

        RuleFor(user => user.LastName)
            .NotEmpty().WithMessage("Last name is required.");

        RuleFor(user => user.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Invalid email address format.");

        RuleFor(user => user.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(6).WithMessage("Password must be at least 6 characters long.")
            .Matches(@"[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
            .Matches(@"[a-z]").WithMessage("Password must contain at least one lowercase letter.")
            .Matches(@"\d").WithMessage("Password must contain at least one digit.")
            .Matches(@"[\W_]").WithMessage("Password must contain at least one special character.");

        RuleFor(user => user.RePassword)
            .Equal(user => user.Password).WithMessage("Password confirmation does not match the password.");

        RuleFor(user => user.DocumentNumber)
            .NotEmpty().WithMessage("Document number is required.")
            .MaximumLength(20).WithMessage("Document number must be at most 20 characters long.");

        RuleFor(user => user.PhoneNumbers)
            .NotEmpty().WithMessage("At least one phone number is required.")
            .Must(HaveUniquePhoneNumbers).WithMessage("Phone numbers must be unique.");

        RuleFor(user => user.DateOfBirth)
            .Must(BeAtLeast18YearsOld).WithMessage("The user must be at least 18 years old.");
    }

    /// <summary>
    ///     Checks that the user's age is at least 18 years.
    /// </summary>
    /// <param name="dateOfBirth">The date of birth of the user.</param>
    /// <returns>True if the user is at least 18 years old; otherwise, false.</returns>
    private bool BeAtLeast18YearsOld(DateTime dateOfBirth)
    {
        var today = DateTime.Today;
        var age = today.Year - dateOfBirth.Year;
        if (dateOfBirth.Date > today.AddYears(-age))
            age--;

        return age >= 18;
    }

    /// <summary>
    ///     Validates that all phone numbers in the collection are unique.
    /// </summary>
    /// <param name="phoneNumbers">The collection of phone numbers to validate.</param>
    /// <returns>True if all phone numbers are unique; otherwise, false.</returns>
    private bool HaveUniquePhoneNumbers(ICollection<PhoneNumber> phoneNumbers)
    {
        var numbers = phoneNumbers.Select(p => p.Number).ToList();
        return numbers.Distinct().Count() == numbers.Count;
    }
}