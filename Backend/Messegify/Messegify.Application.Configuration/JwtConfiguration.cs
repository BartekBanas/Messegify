namespace Messegify.Application.Configuration;

public class JwtConfiguration
{
    public string Issuer { get; set; }
    public string Audience { get; set; }
    public string PublicKey { get; set; }
    public string PublicKeyInfo { get; set; }
    public string SecretKey { get; set; }
    public int Expires { get; set; }
}