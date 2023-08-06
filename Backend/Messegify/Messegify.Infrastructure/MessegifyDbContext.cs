using Messegify.Domain.Entities;
using Messegify.Infrastructure.Error;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;

namespace Messegify.Infrastructure;

public class MessegifyDbContext : DbContext
{
    public MessegifyDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Account> Accounts { get; set; } = null!;
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<ChatRoom> Rooms { get; set; } = null!;
    public DbSet<Message> Messages { get; set; } = null!;
    public DbSet<AccountChatRoom> AccountRooms { get; set; } = null!;
    public DbSet<Contact> Contacts { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .Entity<ChatRoom>()
            .Property(chatRoom => chatRoom.ChatRoomType)
            .HasConversion<string>();

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