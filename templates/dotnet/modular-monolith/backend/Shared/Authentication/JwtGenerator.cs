using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Shared.Authentication;

public sealed class JwtGenerator(JwtSettings jwtSettings)
{
    public string GenerateJwtToken(Guid userId)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key));

        var securityToken = new JwtSecurityToken
        (
            jwtSettings.Issuer,
            jwtSettings.Audience,
            expires: DateTime.UtcNow.AddSeconds(jwtSettings.LifetimeInSeconds),
            signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256),
            claims: new Claim[]
            {
                new(ClaimTypes.NameIdentifier, userId.ToString())
            }
        );

        var tokenAsText = new JwtSecurityTokenHandler().WriteToken(securityToken);

        return tokenAsText;
    }
}
