using ModelLibrary.Enums;

namespace Employes.Feature.User.Requests;

/// <summary>
///     Represents the data required to create a new user.
/// </summary>
public class CreateUserRequest
{
    /// <summary>
    ///     Gets or sets the username for the new user.
    /// </summary>
    public string UserName { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the password for the new user.
    ///     This should follow best practices for security.
    /// </summary>
    public string Password { get; set; } = string.Empty;
    /// <summary>
    ///     Gets or sets the repeated password for confirmation.
    ///     This should match the Password property.
    /// </summary>
    public string RePassword { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the first name of the user.
    ///     This field is required.
    /// </summary>
    public string FirstName { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the last name of the user.
    ///     This field is required.
    /// </summary>
    public string LastName { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the email of the user.
    ///     This field is required.
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the document number of the user.
    ///     This field is required and must be unique.
    /// </summary>
    public string DocumentNumber { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the phone number(s) for the user.
    ///     A user should have more than one phone number if applicable.
    /// </summary>
    public List<CreatePhoneNumberRequest> PhoneNumbers { get; set; } = [];

    /// <summary>
    ///     Gets or sets the ID of the manager.
    ///     The manager can be another employee.
    /// </summary>
    public int? ManagerId { get; set; }

    /// <summary>
    ///     Gets or sets the date of birth of the user.
    ///     The user must not be a minor.
    /// </summary>
    public DateTime DateOfBirth { get; set; }
    
    /// <summary>
    /// Gets or sets the role of the user within the organization.
    /// </summary>
    public Role Role { get; set; }
}