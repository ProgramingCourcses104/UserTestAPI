using Microsoft.EntityFrameworkCore;
using UserTestAPI.Interfaces;
using UserTestAPI.Models;

namespace UserTestAPI.DB
{
    public class AppDbContext : DbContext , IAppDbContext
    {

        public AppDbContext(DbContextOptions options) : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure your entity relationships and constraints here
            // modelBuilder.Entity<YourEntity>().HasOne(...).WithMany(...);

            base.OnModelCreating(modelBuilder);
        }

        public void Save() {

            SaveChanges();
        }

    }
}
