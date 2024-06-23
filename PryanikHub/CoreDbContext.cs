using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using PryanikHub.Entities;

namespace PryanikHub
{
    public class CoreDbContext : DbContext
    {
        private readonly DbContextOptions<CoreDbContext> options;

        #region System
        public CoreDbContext()
        {

        }
        #endregion

        private string _connectionString;
        public CoreDbContext(DbContextOptions<CoreDbContext> options, string connectionString = "") : base(options)
        {
            _connectionString = connectionString;
        }

        public DbSet<Order> Orders { get; set; }
        public DbSet<Product> Products { get; set; }

        public DbSet<OrderLine> OrderLines { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // primal keys
            builder.Entity<Order>().HasKey(x => x.OrderId);
            builder.Entity<Product>().HasKey(x => x.ProductId);
            builder.Entity<OrderLine>().HasKey(x => x.OrderLineId);

            // adding Identity constarint, basically auto increment
            builder.Entity<Order>().Property(x => x.OrderId).ValueGeneratedOnAdd();
            builder.Entity<Product>().Property(x => x.ProductId).ValueGeneratedOnAdd();
            builder.Entity<OrderLine>().Property(x => x.OrderLineId).ValueGeneratedOnAdd();

            // relationships
            builder.Entity<Order>().HasMany(x => x.OrderLines).WithOne(x => x.Order).HasForeignKey(x => x.OrderId);
            builder.Entity<OrderLine>().HasOne(x => x.Product);

            Seed(builder);
        }

        private void Seed(ModelBuilder builder)
        {
            Product p1 = new Product 
            {
                ProductId = 1,
                DetailedImagesRefs = new string[2]
                {
                    @"https://upload.wikimedia.org/wikipedia/commons/thumb/a/a2/Prjaniki.jpg/1200px-Prjaniki.jpg",
                    @"https://upload.wikimedia.org/wikipedia/commons/thumb/a/a2/Prjaniki.jpg/1200px-Prjaniki.jpg"
                },

                Description = "Lorem ipsum pryanik lopsum",
                Title = "Pryanik#1",
                PreviewImageRef = @"https://upload.wikimedia.org/wikipedia/commons/thumb/a/a2/Prjaniki.jpg/1200px-Prjaniki.jpg"
            };
            Product p2 = new Product
            {
                ProductId = 2,
                DetailedImagesRefs = new string[2]
                {
                    @"https://upload.wikimedia.org/wikipedia/commons/thumb/a/a2/Prjaniki.jpg/1200px-Prjaniki.jpg",
                    @"https://upload.wikimedia.org/wikipedia/commons/thumb/a/a2/Prjaniki.jpg/1200px-Prjaniki.jpg"
                },

                Description = "Lorem ipsum pryanik lopsum",
                Title = "Pryanik#2",
                PreviewImageRef = @"https://upload.wikimedia.org/wikipedia/commons/thumb/a/a2/Prjaniki.jpg/1200px-Prjaniki.jpg"
            };

            builder.Entity<Product>().HasData(p1, p2);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!String.IsNullOrEmpty(_connectionString))
            {
                ConfigServer(_connectionString, optionsBuilder);
            }
        }

        public void ConfigServer(string connectionString, DbContextOptionsBuilder optionsBuilder)
        {
            Debug.WriteLine($"Setting up connection string for {nameof(CoreDbContext)}");
            optionsBuilder.UseSqlServer(_connectionString);
        }
    }
}
