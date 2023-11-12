using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;

namespace Shared.Authentication;

public sealed class JwtAuthService(JwtSettings jwtSettings)
{
    public string GenerateJwtToken(Guid userId, string username)
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
                new(ClaimTypes.Name, username),
                new(ClaimTypes.NameIdentifier, userId.ToString())
            }
        );

        var jwtToken = new JwtSecurityTokenHandler().WriteToken(securityToken);

        return jwtToken;
    }

    public void AppendJwtAuthCookie(HttpContext context, string jwtToken)
    {
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = DateTime.UtcNow.AddSeconds(jwtSettings.LifetimeInSeconds)
        };

        context.Response.Cookies.Append(JwtAuthConstants.Cookie, jwtToken, cookieOptions);
    }
}
