using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Messegify.Application.Configuration;
using Messegify.Application.Services;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Application.Services.Tests;

public class JwtServiceTests
{
    private readonly JwtConfiguration _jwtConfiguration = new JwtConfiguration
    {
        SecretKey = "SUUUUPER_SEKRET_OWO",
        Expires = 60,
        Issuer = "your-issuer",
        Audience = "your-audience"
    };

    [Fact]
    public void GenerateSymmetricJwtToken_ReturnsValidToken()
    {
        // Arrange
        var claimsIdentity = new ClaimsIdentity(new Claim[]
        {
            new Claim(ClaimTypes.Name, "testuser")
        });

        var jwtService = new JwtService(Options.Create(_jwtConfiguration));

        // Act
        var token = jwtService.GenerateSymmetricJwtToken(claimsIdentity);

        // Assert
        Assert.NotNull(token);
        Assert.True(token.Length > 0);

        var tokenHandler = new JwtSecurityTokenHandler();
        var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = _jwtConfiguration.Issuer,
            ValidateAudience = true,
            ValidAudience = _jwtConfiguration.Audience,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtConfiguration.SecretKey))
        }, out _);

        Assert.NotNull(principal);
        Assert.Equal("testuser", principal.Identity.Name);
    }
}