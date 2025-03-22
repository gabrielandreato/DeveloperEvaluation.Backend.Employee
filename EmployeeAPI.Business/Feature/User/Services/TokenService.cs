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

    /// <inheritdoc />
    public string GenerateToken(ModelLibrary.Entities.User user)
    {
        var claims = new[]
        {
            new Claim("username", user.UserName),
            new Claim("id", user.Id.ToString()),
            new Claim("loginTimeStamp", DateTime.UtcNow.ToString()),
            new Claim(ClaimTypes.Role, user.Role.ToString())
        };

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_configuration["SymmetricSecurityKey"]
                                   ?? throw new Exception("SymmetricSecurityKey not found.")));
        var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken
        (
            expires: DateTime.Now.AddHours(8),
            claims: claims,
            signingCredentials: signingCredentials
        );
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}