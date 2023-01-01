using Messegify.Domain.Entities;

namespace Messegify.Application.Services;

public interface IHashingService
{
    string HashPassword(string password);
    bool VerifyPassword(Account account, string password);
}

public class HashingService : IHashingService
{
    public string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    public bool VerifyPassword(Account account, string password)
    {
        return BCrypt.Net.BCrypt.Verify(password, account.PasswordHash);
    }
}