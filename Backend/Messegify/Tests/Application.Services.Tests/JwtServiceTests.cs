using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Messegify.Application.Configuration;
using Messegify.Application.Services;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Xunit;

namespace Application.Services.Tests;

public class JwtServiceTests
{
    private readonly JwtConfiguration _jwtConfiguration;
    private readonly JwtService _jwtService;

    public JwtServiceTests()
    {
        _jwtConfiguration = new JwtConfiguration
        {
            SecretKey = Convert.ToBase64String(new byte[32]),
            Expires = 60,
            Issuer = "your-issuer",
            Audience = "your-audience"
        };
        
        _jwtService = new JwtService(Options.Create(_jwtConfiguration));
    }

    [Fact]
    public void GenerateSymmetricJwtToken_ReturnsValidToken()
    {
        const string testClaimValue = "testUser";
        
        // Arrange
        var claimsIdentity = new ClaimsIdentity(new Claim[]
        {
            new(ClaimTypes.Name, testClaimValue)
        });

        // Act
        var token = _jwtService.GenerateSymmetricJwtToken(claimsIdentity);

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
        Assert.Equal(testClaimValue, principal.Identity!.Name);
    }
}