using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography;
using System.Text;
using ModelLibrary.Enums;

namespace ModelLibrary.Entities;

/// <summary>
/// Represents a user entity with additional properties for the application.
/// </summary>
[Table("Users")]
public class User
{
    /// <summary>
    /// Initializes a new instance of the <see cref="User" /> class.
    /// </summary>
    public User()
    {
    }

    /// <summary>
    /// Unique identifier
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Represents the user name
    /// </summary>
    public string UserName { get; set; }
    /// <summary>
    /// Represents the user email
    /// </summary>
    public string Email { get; set; }
    
    /// <summary>
    /// Gets or sets the first name of the user.
    /// This field is required.
    /// </summary>
    [Required]
    public string FirstName { get; set; }

    /// <summary>
    /// Gets or sets the last name of the user.
    /// This field is required.
    /// </summary>
    [Required]
    public string LastName { get; set; }

    /// <summary>
    /// Gets or sets the password for the user.
    /// The password should follow security best practices.
    /// </summary>
    [StringLength(100, MinimumLength = 6, ErrorMessage = "The password must be at least 6 characters long.")]
    public string Password { get; set; }

    /// <summary>
    /// Gets or sets the confirmation password.
    /// This should match the Password property.
    /// </summary>
    [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
    public string RePassword { get; set; }

    /// <summary>
    /// Gets or sets the hash password.
    /// A MD5 hash password.
    /// </summary>
    public string HashPassword { get; set; }

    /// <summary>
    /// Gets or sets the document number of the user.
    /// This field is required and must be unique.
    /// </summary>
    [Required]
    [StringLength(20)]
    public string DocumentNumber { get; set; }

    /// <summary>
    /// Gets or sets the manager ID.
    /// This can refer to another employee who is the manager.
    /// </summary>
    public int? ManagerId { get; set; }

    /// <summary>
    /// Gets or sets the manager entity, representing the user's manager in the organization.
    /// </summary>
    public User? Manager { get; set; }

    /// <summary>
    /// Gets or sets the date of birth of the user.
    /// The user must be at least 18 years old.
    /// </summary>
    [Required]
    [AgeRequirement(MinimumAge = 18, ErrorMessage = "The user must be at least 18 years old.")]
    public DateTime DateOfBirth { get; set; }

    /// <summary>
    /// Gets or sets the role of the user within the organization.
    /// </summary>
    public Role Role { get; set; }

    /// <summary>
    /// Gets or sets the collection of phone numbers associated with the user.
    /// </summary>
    public ICollection<PhoneNumber> PhoneNumbers { get; set; } = [];
    
    public void SetEncryptedPassword()
    {
        var md5EncryptedPassword = MD5.HashData(
            Encoding.UTF8.GetBytes(Password)
        );
        HashPassword = BitConverter.ToString(md5EncryptedPassword);
    }
    
    public void ValidatePassword(string password)
    {
        var md5EncryptedPassword = BitConverter.ToString(MD5.HashData(
            Encoding.UTF8.GetBytes(password)
        ));
        if (md5EncryptedPassword != HashPassword) throw new Exception("Passwords is wrong.");
    }
}

/// <summary>
/// Validation attribute to ensure the minimum age requirement is met.
/// </summary>
public class AgeRequirementAttribute : ValidationAttribute
{
    /// <summary>
    /// Gets or sets the minimum age required.
    /// </summary>
    public int MinimumAge { get; set; }

    /// <summary>
    /// Validates the age requirement.
    /// </summary>
    /// <param name="value">The value to validate.</param>
    /// <param name="validationContext">The validation context.</param>
    /// <returns>
    /// A <see cref="ValidationResult" /> indicating whether the age requirement is met.
    /// </returns>
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value is DateTime dateOfBirth)
        {
            var age = DateTime.Today.Year - dateOfBirth.Year;
            if (dateOfBirth.Date > DateTime.Today.AddYears(-age)) age--;

            if (age < MinimumAge) return new ValidationResult(ErrorMessage);
        }

        return ValidationResult.Success;
    }
}