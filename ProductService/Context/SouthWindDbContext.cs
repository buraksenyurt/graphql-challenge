using Microsoft.EntityFrameworkCore;
using ProductService.Model;

namespace ProductService.Context
{
    public class SouthWindDbContext
        : DbContext
    {
        protected readonly IConfiguration Configuration;
        public SouthWindDbContext(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseNpgsql(Configuration.GetConnectionString("SouthWindConStr"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().HasData(
                       new Category { Id = 1, Title = "Kırtasiye Sarf Malzemesi" },
                       new Category { Id = 2, Title = "Defter" },
                       new Category { Id = 3, Title = "Kurşun Kalem" }
            );

            modelBuilder.Entity<Product>().HasData(
                       new Product { Id = 1, Name = "A4 boy kuşe kağıt", CategoryId = 1, StockLevel = 100, UnitPrice = 55.55M },
                       new Product { Id = 2, Name = "B3 boy kuşe kartpostal. Renki desenli", CategoryId = 1, StockLevel = 35, UnitPrice = 18.95M }
            );
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
    }
}