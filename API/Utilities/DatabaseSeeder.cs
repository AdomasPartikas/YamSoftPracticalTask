using YamSoft.API.Context;
using YamSoft.API.Entities;

namespace YamSoft.API.Utilities;

public static class DatabaseSeeder
{
    public static async Task SeedAsync(YamSoftDbContext context)
    {
        // Ensure database is created
        await context.Database.EnsureCreatedAsync();

        // Check if products already exist
        if (context.Products.Any())
            return; // Database has been seeded

        var products = new List<Product>
        {
            new Product
            {
                Name = "Laptop Pro 15\"",
                Price = 1299.99m,
                Stock = 25,
                Description = "High-performance laptop with 15-inch display, perfect for professionals and power users.",
                ImageUrl = "https://via.placeholder.com/400x300?text=Laptop+Pro+15"
            },
            new Product
            {
                Name = "Wireless Headphones",
                Price = 199.99m,
                Stock = 50,
                Description = "Premium noise-cancelling wireless headphones with 30-hour battery life.",
                ImageUrl = "https://via.placeholder.com/400x300?text=Wireless+Headphones"
            },
            new Product
            {
                Name = "Smartphone X",
                Price = 799.99m,
                Stock = 30,
                Description = "Latest generation smartphone with advanced camera system and all-day battery.",
                ImageUrl = "https://via.placeholder.com/400x300?text=Smartphone+X"
            },
            new Product
            {
                Name = "Gaming Mouse",
                Price = 79.99m,
                Stock = 100,
                Description = "Precision gaming mouse with customizable RGB lighting and programmable buttons.",
                ImageUrl = "https://via.placeholder.com/400x300?text=Gaming+Mouse"
            },
            new Product
            {
                Name = "4K Monitor",
                Price = 399.99m,
                Stock = 20,
                Description = "27-inch 4K UHD monitor with HDR support and ultra-thin bezels.",
                ImageUrl = "https://via.placeholder.com/400x300?text=4K+Monitor"
            },
            new Product
            {
                Name = "Mechanical Keyboard",
                Price = 149.99m,
                Stock = 35,
                Description = "Premium mechanical keyboard with tactile switches and customizable backlighting.",
                ImageUrl = "https://via.placeholder.com/400x300?text=Mechanical+Keyboard"
            },
            new Product
            {
                Name = "Tablet 10\"",
                Price = 499.99m,
                Stock = 40,
                Description = "Lightweight 10-inch tablet perfect for productivity and entertainment on the go.",
                ImageUrl = "https://via.placeholder.com/400x300?text=Tablet+10"
            },
            new Product
            {
                Name = "Smart Watch",
                Price = 299.99m,
                Stock = 60,
                Description = "Advanced smartwatch with health tracking, GPS, and week-long battery life.",
                ImageUrl = "https://via.placeholder.com/400x300?text=Smart+Watch"
            },
            new Product
            {
                Name = "Bluetooth Speaker",
                Price = 89.99m,
                Stock = 75,
                Description = "Portable Bluetooth speaker with 360-degree sound and waterproof design.",
                ImageUrl = "https://via.placeholder.com/400x300?text=Bluetooth+Speaker"
            },
            new Product
            {
                Name = "USB-C Hub",
                Price = 49.99m,
                Stock = 120,
                Description = "Multi-port USB-C hub with 4K HDMI, USB 3.0, and fast charging support.",
                ImageUrl = "https://via.placeholder.com/400x300?text=USB-C+Hub"
            },
            new Product
            {
                Name = "Webcam HD",
                Price = 69.99m,
                Stock = 80,
                Description = "1080p HD webcam with auto-focus and built-in noise-cancelling microphone.",
                ImageUrl = "https://via.placeholder.com/400x300?text=Webcam+HD"
            },
            new Product
            {
                Name = "Power Bank",
                Price = 39.99m,
                Stock = 90,
                Description = "High-capacity power bank with fast charging and multiple device support.",
                ImageUrl = "https://via.placeholder.com/400x300?text=Power+Bank"
            },
            new Product
            {
                Name = "Desk Lamp LED",
                Price = 59.99m,
                Stock = 45,
                Description = "Adjustable LED desk lamp with touch controls and multiple brightness levels.",
                ImageUrl = "https://via.placeholder.com/400x300?text=Desk+Lamp+LED"
            },
            new Product
            {
                Name = "Phone Case",
                Price = 24.99m,
                Stock = 200,
                Description = "Durable phone case with military-grade protection and wireless charging support.",
                ImageUrl = "https://via.placeholder.com/400x300?text=Phone+Case"
            },
            new Product
            {
                Name = "Ergonomic Chair",
                Price = 299.99m,
                Stock = 15,
                Description = "Ergonomic office chair with lumbar support and adjustable height.",
                ImageUrl = "https://via.placeholder.com/400x300?text=Ergonomic+Chair"
            }
        };

        await context.Products.AddRangeAsync(products);
        await context.SaveChangesAsync();
    }
}