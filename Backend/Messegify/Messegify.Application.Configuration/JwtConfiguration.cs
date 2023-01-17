namespace Messegify.Application.Configuration;

public class JwtConfiguration
{
    public string Issuer { get; set; } = null!;
    public string Audience { get; set; } = null!;
    public string PublicKey { get; set; } = null!;
    public string PublicKeyInfo { get; set; } = null!;
    public string SecretKey { get; set; } = null!;
    public int Expires { get; set; }
}