using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Messegify.Application.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Messegify.Application.Services;

public interface IJwtService
{
    string GenerateSymmetricJwtToken(ClaimsIdentity claimsIdentity);
}

public class JwtService : IJwtService
{
    private readonly JwtConfiguration _jwtConfiguration;

    public JwtService(IOptions<JwtConfiguration> jwtConfiguration)
    {
        _jwtConfiguration = jwtConfiguration.Value;
    }
    public string GenerateAsymmetricJwtToken(ClaimsIdentity claimsIdentity)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        string stringToken = String.Empty;    
        
        using (var rsa = RSA.Create())
        {
            rsa.ImportRSAPrivateKey(Convert.FromBase64String(_jwtConfiguration.SecretKey), out int _);
            
            var signingCredentials = new SigningCredentials(
                key: new RsaSecurityKey(rsa.ExportParameters(true)),
                algorithm: SecurityAlgorithms.RsaSha256 // Important to use RSA version of the SHA algo 
            );
            
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claimsIdentity,
                Expires = DateTime.UtcNow.AddMinutes(_jwtConfiguration.Expires),
                Issuer = _jwtConfiguration.Issuer,
                Audience = _jwtConfiguration.Audience,
                SigningCredentials = signingCredentials
            };
            
            var token = tokenHandler.CreateToken(tokenDescriptor);
            stringToken = tokenHandler.WriteToken(token);
        }

        return stringToken;
    }
    
    public string GenerateSymmetricJwtToken(ClaimsIdentity claimsIdentity)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_jwtConfiguration.SecretKey);
        
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = claimsIdentity,
            Expires = DateTime.UtcNow.AddMinutes(_jwtConfiguration.Expires),
            Issuer = _jwtConfiguration.Issuer,
            Audience = _jwtConfiguration.Audience,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}