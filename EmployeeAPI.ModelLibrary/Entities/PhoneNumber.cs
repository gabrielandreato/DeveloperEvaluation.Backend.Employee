using System.ComponentModel.DataAnnotations;
using ModelLibrary.Entities;

/// <summary>
/// Represets a phone number
/// </summary>
public class PhoneNumber
{
    /// <summary>
    /// Unique identifier
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// The phone number as a string (e.g., "+123456789").
    /// </summary>
    [Required]
    [StringLength(20)]
    public string Number { get; set; }

    /// <summary>
    /// Reference to the associated user.
    /// </summary>
    public int UserId { get; set; }

    /// <summary>
    /// User related with the phone number
    /// </summary>
    public User User { get; set; }
}