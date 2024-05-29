using Mango.Services.ProductAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace Mango.Services.ProductAPI.Data
{
    public sealed class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }


        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Product>().HasData(new Product
            {
                ProductId = 1,
                Name = "Jurek",
                Price = 15,
                Description = "is a traditional Polish soup that is particularly cherished for its unique sour flavor and hearty ingredients",
                ImageUrl = "https://placehold.co/603x403",
                CategoryName = "Soup"
            });
            modelBuilder.Entity<Product>().HasData(new Product
            {
                ProductId = 2,
                Name = "Pickled Cucumbers",
                Price = 13.99,
                Description = "Traditional Polish dill pickles, fermented with garlic and dill. They are crunchy and tangy, perfect for starting a meal.",
                ImageUrl = "https://placehold.co/602x402",
                CategoryName = "Appetizer"
            });
            modelBuilder.Entity<Product>().HasData(new Product
            {
                ProductId = 3,
                Name = "Sweet Pie",
                Price = 10.99,
                Description = "Fruity, nutty or chocolatey, decked out with pastry, meringue or crumble, these sumptuous desserts are sure to satisfy your sweet tooth.",
                ImageUrl = "https://placehold.co/601x401",
                CategoryName = "Dessert"
            });
            modelBuilder.Entity<Product>().HasData(new Product
            {
                ProductId = 4,
                Name = "Potato Pancakes",
                Price = 15,
                Description = "Crispy potato pancakes, often served with sour cream or apple sauce. They can be plain or mixed with onions and garlic for extra flavor.",
                ImageUrl = "https://placehold.co/600x400",
                CategoryName = "Entree"
            });
        }
    }
}
