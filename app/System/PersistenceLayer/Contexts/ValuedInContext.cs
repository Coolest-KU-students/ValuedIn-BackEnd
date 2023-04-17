using Microsoft.EntityFrameworkCore;
using ValuedInBE.Chats.Models.Entities;
using ValuedInBE.Users.Models.Entities;

namespace ValuedInBE.System.PersistenceLayer.Contexts
{
    public class ValuedInContext : DbContext
    {
        public DbSet<UserCredentials> UserCredentials { get; set; } = null!;
        public DbSet<UserDetails> UserDetails { get; set; } = null!;
        public DbSet<Chat> Chats { get; set; } = null!;
        public DbSet<ChatMessage> ChatMessages { get; set; } = null!;
        public DbSet<ChatParticipant> ChatParticipants { get; set; } = null!;

        public ValuedInContext()
        {
        }

        public ValuedInContext(DbContextOptions<ValuedInContext> options) : base(options)
        {
        }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            MapMessagingModels(modelBuilder);
            MapUserModels(modelBuilder);
            MapModelsToTables(modelBuilder);
        }

        private static void MapUserModels(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserCredentials>()
            .HasOne(a => a.UserDetails).WithOne()
            .HasForeignKey<UserDetails>(e => e.UserID).IsRequired();

            modelBuilder.Entity<UserCredentials>()
                .HasIndex(a => a.Login)
                .IsUnique();
        }

        private static void MapMessagingModels(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Chat>()
            .HasMany(a => a.Messages).WithOne()
            .HasForeignKey(e => e.ChatId).IsRequired();

            modelBuilder.Entity<ChatParticipant>()
                .HasKey(e => new { e.UserId, e.ChatId });
        }

        private static void MapModelsToTables(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserCredentials>().ToTable("UserCredentials");
            modelBuilder.Entity<UserDetails>().ToTable("UserDetails");
            modelBuilder.Entity<Chat>().ToTable("Chats");
            modelBuilder.Entity<ChatParticipant>().ToTable("ChatParticipants");
            modelBuilder.Entity<ChatMessage>().ToTable("ChatMessages");
        }
    }
}