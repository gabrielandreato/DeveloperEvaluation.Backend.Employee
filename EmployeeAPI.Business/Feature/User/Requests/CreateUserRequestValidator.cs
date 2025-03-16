using FluentValidation;

namespace Employes.Feature.User.Requests;

/// <summary>
///     Provides validation rules for the CreateUserRequest using FluentValidation.
/// </summary>
public class CreateUserRequestValidator : AbstractValidator<CreateUserRequest>
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="CreateUserRequestValidator" /> class.
    ///     Defines validation rules for the CreateUserRequest properties.
    /// </summary>
    public CreateUserRequestValidator()
    {
        RuleFor(request => request.Username)
            .NotEmpty().WithMessage("Username is required.");
        
        RuleFor(request => request.FirstName)
            .NotEmpty().WithMessage("First name is required.");

        RuleFor(request => request.LastName)
            .NotEmpty().WithMessage("Last name is required.");

        RuleFor(request => request.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Invalid email address format.");

        RuleFor(request => request.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(8).WithMessage("Password must be at least 8 characters long.")
            .Matches(@"[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
            .Matches(@"[a-z]").WithMessage("Password must contain at least one lowercase letter.")
            .Matches(@"[0-9]").WithMessage("Password must contain at least one digit.")
            .Matches(@"[\W_]").WithMessage("Password must contain at least one special character.");

        RuleFor(request => request.RePassword)
            .Equal(request => request.Password).WithMessage("Password confirmation does not match the password.");

        RuleFor(request => request.DocumentNumber)
            .NotEmpty().WithMessage("Document number is required.")
            .MaximumLength(20).WithMessage("Document number must be at most 20 characters long.");

        RuleFor(request => request.Phone)
            .NotEmpty().WithMessage("At least one phone number is required.");

        RuleFor(request => request.DateBirth)
            .NotEmpty().WithMessage("Date of birth is required.")
            .Must(BeAtLeast18YearsOld).WithMessage("The user must be at least 18 years old.");
        
        RuleFor(request => request.Role)
            .IsInEnum().WithMessage("Role must be a valid enumeration value (Employee, Supervisor, or Manager).");
        
    }

    /// <summary>
    ///     Validates that the user is at least 18 years old.
    /// </summary>
    /// <param name="dateOfBirth">The date of birth of the user.</param>
    /// <returns>true if the user is at least 18 years old; otherwise, false.</returns>
    private bool BeAtLeast18YearsOld(DateTime dateOfBirth)
    {
        var today = DateTime.Today;
        var age = today.Year - dateOfBirth.Year;

        if (dateOfBirth.Date > today.AddYears(-age)) age--;
        return age >= 18;
    }
}