namespace Employes.Feature.User.Requests;

/// <summary>
///     Represents data required to login in the system.
/// </summary>
public class LoginUserRequest
{
    /// <summary>
    ///     Gets or sets the username for the user.
    ///     This is used as a unique identifier in the system.
    /// </summary>
    public string UserName { get; set; }

    /// <summary>
    ///     Gets or sets the password for the user.
    ///     The password should follow security best practices.
    /// </summary>
    public string Password { get; set; }
}