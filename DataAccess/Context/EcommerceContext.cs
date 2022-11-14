using Authentication.DataAccess.Context;
using Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Context
{
    public class EcommerceContext : AuthContext
    {
        public EcommerceContext(DbContextOptions<EcommerceContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public override int SaveChanges()
        {
            return base.SaveChanges();
        }
    }
}
