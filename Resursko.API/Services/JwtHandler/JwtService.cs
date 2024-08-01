using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Resursko.API.Services.JwtHandler;

public class JwtService(IConfiguration configuration)
{
    private readonly IConfigurationSection _jwtSettings = configuration.GetSection("JwtSettings");
    private List<Claim> GetUserClaims(User user)
    {
        return new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Name, user.UserName!),
            new Claim(ClaimTypes.Email, user.Email!)
        };
    }

    private SigningCredentials GetUserCredentials()
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings["JwtSecurityKey"]!));
        return new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
    }

    private JwtSecurityToken GenerateTokenOptions(SigningCredentials creds, List<Claim> claims)
    {
        return new JwtSecurityToken(
            issuer: _jwtSettings["JwtIssuer"],
            audience: _jwtSettings["JwtAudience"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(Convert.ToDouble(_jwtSettings["JwtExpiryInMinutes"])),
            signingCredentials: creds
            );
    }

    public string CreateToken(User user)
    {
        var claims = GetUserClaims(user);
        var creds = GetUserCredentials();
        var token = GenerateTokenOptions(creds, claims);
        
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
