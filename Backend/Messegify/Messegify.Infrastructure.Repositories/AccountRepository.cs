using Messegify.Domain.Repositories;

namespace Messegify.Infrastructure.Repositories;

public class AccountRepository : IAccountRepository
{
    private readonly MessegifyDbContext _dbContext;

    public AccountRepository(MessegifyDbContext dbContext)
    {
        _dbContext = dbContext;
    }
}