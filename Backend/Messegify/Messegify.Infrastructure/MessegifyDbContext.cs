using Messegify.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Messegify.Infrastructure;

public class MessegifyDbContext : DbContext
{
    public MessegifyDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Account> Accounts { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Room> Rooms { get; set; }
    public DbSet<Message> Messages { get; set; }
    public DbSet<AccountRoom> AccountRooms { get; set; }
    public DbSet<Friendship> Friendships { get; set; }
}