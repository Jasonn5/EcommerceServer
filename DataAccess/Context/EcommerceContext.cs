using Authentication.DataAccess.Context;
using DataAccess.Model;
using DataAccess.Model.Product;
using Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Context
{
    public class EcommerceContext : AuthContext
    {
        public EcommerceContext(DbContextOptions<EcommerceContext> options) : base(options)
        {
        }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Sale> Sales { get; set; }
        public DbSet<SaleDetail> SaleDetails { get; set; }
        public DbSet<Stock> Stocks { get; set; }
        public DbSet<Product> Products { get; set; }

        // CUSTOM
        public virtual DbSet<OrderOrderDetails> OrderOrderDetails { get; set; }
        public virtual DbSet<ProductGetView> ProductGetViews { get; set; }
        public virtual DbSet<SaleSaleDetailsCreditClientSeller> SaleSaleDetailsCreditClientSellers { get; set; }
        public virtual DbSet<SaleSaleDetails> SaleSaleDetails { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Order>()
                .HasMany(o => o.OrderDetails)
                .WithOne(od => od.Order);

            modelBuilder.Entity<OrderDetail>()
                .HasOne(od => od.Order)
                .WithMany(o => o.OrderDetails);

            modelBuilder.Entity<Sale>()
                .HasMany(o => o.SaleDetails)
                .WithOne(sd => sd.Sale);

            modelBuilder.Entity<SaleDetail>()
                .HasOne(sd => sd.Sale)
                .WithMany(s => s.SaleDetails);

            base.OnModelCreating(modelBuilder);
        }

        public override int SaveChanges()
        {
            return base.SaveChanges();
        }
    }
}
