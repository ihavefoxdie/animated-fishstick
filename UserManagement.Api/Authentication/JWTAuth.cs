using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using UserManagement.Api.Entities;

namespace UserManagement.Api.Authentication;

/// <summary>
/// Class for generating JWT tokens
/// </summary>
public sealed class JWTAuth
{
    private readonly IConfiguration _configuration;

    /// <summary>
    /// Constructor for configuration assignment
    /// </summary>
    /// <param name="configuration">Configuration to use</param>
    public JWTAuth(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    /// <summary>
    /// Generates a token based on the User object parameter values (id, nickname, role)
    /// </summary>
    /// <param name="user">User to create a token for</param>
    /// <returns>Generated token</returns>
    /// <exception cref="ArgumentNullException">The exception that is thrown when configuration couldn't be read or JWT required fields are missing.</exception>
    public string CreateToken(User user)
    {
        string secretKey = _configuration["ApplicationSettings:JWT:Secret"] ?? throw new ArgumentNullException("The JWT secret is missing!");
        SymmetricSecurityKey securityKey = new(Encoding.UTF8.GetBytes(secretKey));

        SigningCredentials credentials = new(securityKey, SecurityAlgorithms.HmacSha256);

        SecurityTokenDescriptor tokenDescriptor = new()
        {
            Subject = new ClaimsIdentity(
            [
                new Claim(JwtRegisteredClaimNames.Sub, user.Guid.ToString()),
                new Claim(JwtRegisteredClaimNames.Nickname, user.Login),
                new Claim("role", user.Admin ? "Admin" : "User"),
            ]),
            Expires = DateTime.UtcNow.AddMinutes(_configuration.GetValue<int>("ApplicationSettings:JWT:ExpirationInMinutes")),
            SigningCredentials = credentials,
            Issuer = _configuration["ApplicationSettings:JWT:ValidIssuer"],
            Audience = _configuration["ApplicationSettings:JWT:ValidAudience"]
        };

        JwtSecurityTokenHandler tokenHandler = new();
        JwtSecurityToken token = tokenHandler.CreateJwtSecurityToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
}
