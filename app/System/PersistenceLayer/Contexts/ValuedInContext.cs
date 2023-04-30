using Microsoft.EntityFrameworkCore;
using ValuedInBE.Chats.Models.Entities;
using ValuedInBE.PersonalValues.Models.Entities;
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
        public DbSet<PersonalValue> Values { get; set; } = null!;
        public DbSet<PersonalValueGroup> ValueGroups { get; set; } = null!;
        public DbSet<UserValue> UserValues { get; set; } = null!;

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
            MapValueModels(modelBuilder);
            MapModelsToTables(modelBuilder);
        }

        private static void MapUserModels(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserCredentials>()
            .HasOne(a => a.UserDetails).WithOne()
            .HasForeignKey<UserDetails>(e => e.UserID).IsRequired();

            modelBuilder.Entity<UserDetails>()
               .HasMany(a => a.UserValues).WithOne()
               .HasForeignKey(a => a.UserId).IsRequired();

            modelBuilder.Entity<UserCredentials>()
                .HasIndex(a => a.Login)
                .IsUnique();

            modelBuilder.Entity<UserValue>()
                .HasKey(e => new { e.UserId, e.ValueId });

            modelBuilder.Entity<UserDetails>()
                .HasMany(a => a.UserValues).WithOne()
                .HasForeignKey(a => a.UserId).IsRequired();
        }

        private static void MapMessagingModels(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Chat>()
            .HasMany(a => a.Messages).WithOne()
            .HasForeignKey(e => e.ChatId).IsRequired();

            modelBuilder.Entity<ChatParticipant>()
                .HasKey(e => new { e.UserId, e.ChatId });
        }

        private static void MapValueModels(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PersonalValueGroup>()
                .HasMany(a=>a.PersonalValues).WithOne()
                .HasForeignKey(a=>a.GroupId).IsRequired();
            
            modelBuilder.Entity<PersonalValue>()
                .HasMany(a=>a.UserValues).WithOne()
                .HasForeignKey(a=>a.ValueId).IsRequired();

            modelBuilder.Entity<PersonalValue>()
                .HasMany(a=>a.UserValues).WithOne()
                .HasForeignKey(a=>a.ValueId).IsRequired();
        }

        private static void MapModelsToTables(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserCredentials>().ToTable("UserCredentials");
            modelBuilder.Entity<UserDetails>().ToTable("UserDetails");
            modelBuilder.Entity<Chat>().ToTable("Chats");
            modelBuilder.Entity<ChatParticipant>().ToTable("ChatParticipants");
            modelBuilder.Entity<ChatMessage>().ToTable("ChatMessages");
            modelBuilder.Entity<PersonalValueGroup>().ToTable("ValueGroups");
            modelBuilder.Entity<PersonalValue>().ToTable("Values");

        }
    }
}