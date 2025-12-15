using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Staffinity.Personal.Domain.Modules.Auth.Model;

namespace Staffinity.Personal.Test.Common;

internal static class JwtTestTokenHelper
{
    public const string Issuer = "Staffinity.Personal.Api";
    public const string Audience = "Staffinity.Personal.Client";

    public static string CreateToken(string secret, string role, Guid? subject = null, string? email = null)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, (subject ?? Guid.NewGuid()).ToString()),
            new Claim(JwtRegisteredClaimNames.Email, email ?? "test-user@example.com"),
            new Claim(ClaimTypes.Role, role),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var token = new JwtSecurityToken(
            issuer: Issuer,
            audience: Audience,
            claims: claims,
            notBefore: DateTime.UtcNow,
            expires: DateTime.UtcNow.AddMinutes(30),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
