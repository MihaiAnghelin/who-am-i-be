using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using who_am_i_be.Interfaces;

namespace who_am_i_be.Services;

public class TokenEmitterService : ITokenEmitterService
{
    private readonly IConfiguration _configuration;
    
    public TokenEmitterService(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    public string GenerateAuthToken(string userId)
    {
        var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtToken:IssuerSigningKey"]));
        var signingCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
        
        var tokenOptions = new JwtSecurityToken(
            issuer: _configuration["JwtToken:ValidIssuer"],
            audience: _configuration["JwtToken:Audience"],
            claims: new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userId)
            },
            expires: DateTime.Now.AddDays(1),
            signingCredentials: signingCredentials
        );
        
        return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
    }
}