namespace Employes.Feature.User.Requests;

public class UpdatePhoneNumberRequest
{
    
    /// <summary>
    /// The phone number as a string (e.g., "+123456789").
    /// </summary>
    public string Number { get; set; }
    
    /// <summary>
    /// Unique identifier
    /// </summary>
    public int Id { get; set; }
    
    /// <summary>
    /// Reference to the associated user.
    /// </summary>
    public string UserId { get; set; }
}