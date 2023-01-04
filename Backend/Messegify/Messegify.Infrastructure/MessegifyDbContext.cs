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
    public DbSet<ChatRoom> Rooms { get; set; }
    public DbSet<Message> Messages { get; set; }
    public DbSet<AccountRoom> AccountRooms { get; set; }
    public DbSet<Contact> Friendships { get; set; }

    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //  TODO: Use fluent API plz
        
        // modelBuilder.Entity<AccountRoom>()
        //     .HasOne(e => e.Account)
        //     .WithMany(e => e.AccountRooms);
        //
        // modelBuilder.Entity<Account>()
        //     .HasMany(e => e.AccountRooms)
        //     .WithOne(e => e.Account)
        //     .HasForeignKey(e => e.AccountId);
        
        base.OnModelCreating(modelBuilder);
    }
}