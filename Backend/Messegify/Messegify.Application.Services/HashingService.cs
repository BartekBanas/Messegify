namespace Messegify.Application.Services;

public interface IHashingService
{
    string HashPassword(string password);
}

public class HashingService : IHashingService
{
    public string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }
}