using Authentication.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Authentication.DataAccess.Context
{
    public abstract class AuthContext : IdentityDbContext
    {
        public AuthContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasData(new
                {
                    Id = 1,
                    Username = "admin",
                    FirstName = "Admin",
                    LastName = "Admin",
                    GivenName = "Michi",
                    Email = "michisoft@gmail.com",
                    IsEnabled = true
                });

            base.OnModelCreating(modelBuilder);
        }
    }
}
