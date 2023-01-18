using Messegify.Domain.Entities;
using Messegify.Infrastructure.Error;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;

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
    public DbSet<AccountChatRoom> AccountRooms { get; set; }
    public DbSet<Contact> Contacts { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .Entity<ChatRoom>()
            .Property(chatRoom => chatRoom.ChatRoomType)
            .HasConversion<string>();

        modelBuilder.Entity<Account>()
            .HasAlternateKey(account => account.Email)
            .HasName("AlternateKey_Email");

        modelBuilder.Entity<Account>()
            .HasAlternateKey(account => account.Name)
            .HasName("AlternateKey_AccountName");

        // modelBuilder.Entity<AccountChatRoom>()
        //     .HasOne(e => e.Account)
        //     .WithMany(e => e.AccountRooms);
        //
        // modelBuilder.Entity<Account>()
        //     .HasMany(e => e.AccountRooms)
        //     .WithOne(e => e.Account)
        //     .HasForeignKey(e => e.AccountId);

        base.OnModelCreating(modelBuilder);
    }


    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        try
        {
            return await base.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException dbUpdateException)
        {
            var sqlException = dbUpdateException.InnerException as MySqlException ?? throw dbUpdateException;
            
            // Violation of DISTINCT constraint 
            if (sqlException.Number == 1062)
                throw new ItemDuplicatedErrorException("Item duplicated");

            throw;
        }
    }
}