namespace Employes.Feature.User.Interfaces;

public interface ITokenService
{
    /// <summary>
    ///     Generates a JWT for a specified user.
    /// </summary>
    /// <param name="user">The user entity for whom the token is being generated.</param>
    /// <returns>A string representation of the generated JWT.</returns>
    /// <exception cref="Exception">Thrown when the symmetric security key is not found in the configuration.</exception>
    public string GenerateToken(ModelLibrary.Entities.User user);
}