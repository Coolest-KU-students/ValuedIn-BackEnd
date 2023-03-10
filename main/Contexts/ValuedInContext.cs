using Microsoft.EntityFrameworkCore;
using ValuedInBE.Models.Entities.Messaging;
using ValuedInBE.Models.Users;

namespace ValuedInBE.Contexts
{
    public class ValuedInContext : DbContext
    {
        public ValuedInContext()
        {
        }

        public ValuedInContext(DbContextOptions<ValuedInContext> options) : base(options)
        {

        }

        public DbSet<UserCredentials> UserCredentials { get; set; }
        public DbSet<UserDetails> UserDetails { get; set; }
        public DbSet<Chat> Chats { get; set; }
        public DbSet<ChatMessage> ChatMessages { get; set; }
        public DbSet<ChatParticipant> ChatParticipants { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            MapMessagingModels(modelBuilder);
            MapUserModels(modelBuilder);
            MapModelsToTables(modelBuilder);
        }

        private void MapUserModels(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<UserCredentials>()
            .HasOne(a => a.UserDetails).WithOne()
            .HasForeignKey<UserDetails>(e => e.UserID).IsRequired();

            modelBuilder.Entity<UserCredentials>()
                .HasIndex(a => a.Login)
                .IsUnique();
        }

        private void MapMessagingModels(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Chat>()
            .HasMany(a => a.Messages).WithOne()
            .HasForeignKey(e => e.ChatID).IsRequired();

            modelBuilder.Entity<ChatParticipant>()
                .HasKey(e => new { e.UserID, e.ChatID });

        }

        private void MapModelsToTables(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<UserCredentials>().ToTable("UserCredentials");
            modelBuilder.Entity<UserDetails>().ToTable("UserDetails");
            modelBuilder.Entity<Chat>().ToTable("Chats");
            modelBuilder.Entity<ChatParticipant>().ToTable("ChatParticipants");
            modelBuilder.Entity<ChatMessage>().ToTable("ChatMessages");
        }
    }
}