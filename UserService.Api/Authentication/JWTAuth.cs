using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using UserService.Api.Entities;

namespace UserService.Api.Authorization;

internal sealed class JWTAuth
{
    private readonly IConfiguration _configuration;
    
    public JWTAuth(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string CreateToken(User user)
    {
        string secretKey = _configuration["ApplicationSettings:JWT:Secret"];
        SymmetricSecurityKey securityKey = new(Encoding.UTF8.GetBytes(secretKey));

        SigningCredentials credentials = new(securityKey, SecurityAlgorithms.HmacSha256);

        SecurityTokenDescriptor tokenDescriptor = new()
        {
            Subject = new ClaimsIdentity(
            [
                new Claim(JwtRegisteredClaimNames.Sub, user.Guid.ToString()),
                new Claim(JwtRegisteredClaimNames.Nickname, user.Login),
            ]),
            Expires = DateTime.UtcNow.AddMinutes(_configuration.GetValue<int>("ApplicationSettings:JWT:ExpirationInMinutes")),
            SigningCredentials = credentials,
            //TODO: add an issuer and an audience
            //Issuer
            //Audience
        };

        JwtSecurityTokenHandler tokenHandler = new();
        JwtSecurityToken token = tokenHandler.CreateJwtSecurityToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
}
