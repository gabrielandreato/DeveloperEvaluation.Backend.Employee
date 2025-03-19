using System.ComponentModel.DataAnnotations;
using ModelLibrary.Enums;

namespace Employes.Feature.User.Response;
/// <summary>
/// Represents a response to return users
/// </summary>
public class UserResponse
{
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
    public string FirstName { get; set; }

    /// <summary>
    /// Gets or sets the last name of the user.
    /// This field is required.
    /// </summary>
    public string LastName { get; set; }
    

    /// <summary>
    /// Gets or sets the document number of the user.
    /// This field is required and must be unique.
    /// </summary>
    public string DocumentNumber { get; set; }

    /// <summary>
    /// Gets or sets the manager ID.
    /// This can refer to another employee who is the manager.
    /// </summary>
    public int? ManagerId { get; set; }

    /// <summary>
    /// Gets or sets the manager entity, representing the user's manager in the organization.
    /// </summary>
    public ModelLibrary.Entities.User? Manager { get; set; }

    /// <summary>
    /// Gets or sets the date of birth of the user.
    /// The user must be at least 18 years old.
    /// </summary>
    public DateTime DateOfBirth { get; set; }

    /// <summary>
    /// Gets or sets the role of the user within the organization.
    /// </summary>
    public Role Role { get; set; }

    /// <summary>
    /// Gets or sets the collection of phone numbers associated with the user.
    /// </summary>
    public ICollection<PhoneNumberResponse> PhoneNumbers { get; set; }
}