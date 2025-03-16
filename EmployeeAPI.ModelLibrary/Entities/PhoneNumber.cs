using System.ComponentModel.DataAnnotations;
using ModelLibrary.Entities;

/// <summary>
/// Represets a phone number
/// </summary>
public class PhoneNumber
{
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
    public string UserId { get; set; }

    public User User { get; set; }
}