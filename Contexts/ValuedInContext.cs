using Microsoft.EntityFrameworkCore;
using ValuedInBE.Contexts;
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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserCredentials>()
            .HasOne(a => a.UserDetails).WithOne()
            .HasForeignKey<UserDetails>(e => e.Login).IsRequired();

            modelBuilder.Entity<UserCredentials>().ToTable("UserCredentials");
            modelBuilder.Entity<UserDetails>().ToTable("UserDetails");
        }
    }
}