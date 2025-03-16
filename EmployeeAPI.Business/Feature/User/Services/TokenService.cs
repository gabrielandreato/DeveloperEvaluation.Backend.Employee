using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Employes.Feature.User.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace Employes.Feature.User.Services;

/// <summary>
///     Provides functionality to generate JSON Web Tokens (JWT) for authentication.
/// </summary>
public class TokenService : ITokenService
{
    private readonly IConfiguration _configuration;

    /// <summary>
    ///     Initializes a new instance of the <see cref="TokenService" /> class.
    /// </summary>
    /// <param name="configuration">The application configuration, typically used to access settings.</param>
    public TokenService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    /// <summary>
    ///     Generates a JWT for a specified user.
    /// </summary>
    /// <param name="user">The user entity for whom the token is being generated.</param>
    /// <returns>A string representation of the generated JWT.</returns>
    /// <exception cref="Exception">Thrown when the symmetric security key is not found in the configuration.</exception>
    public string GenerateToken(ModelLibrary.Entities.User user)
    {
        var claims = new[]
        {
            new Claim("username", user.UserName),
            new Claim("id", user.Id),
            new Claim("loginTimeStamp", DateTime.UtcNow.ToString())
        };

        var chave = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_configuration["SymmetricSecurityKey"]
                                   ?? throw new Exception("SymmetricSecurityKey not found.")));
        var signingCredentials = new SigningCredentials(chave, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken
        (
            expires: DateTime.Now.AddMinutes(10),
            claims: claims,
            signingCredentials: signingCredentials
        );
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}