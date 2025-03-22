namespace Employes.Feature.User.Requests;

public class CreatePhoneNumberRequest
{
    /// <summary>
    /// The phone number as a string (e.g., "+123456789").
    /// </summary>
    public string Number { get; set; }
}