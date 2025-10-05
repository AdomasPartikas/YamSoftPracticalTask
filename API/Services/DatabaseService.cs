using Microsoft.EntityFrameworkCore;
using YamSoft.API.Context;
using YamSoft.API.Entities;
using YamSoft.API.Enums;
using YamSoft.API.Interfaces;

namespace YamSoft.API.Services;

public class DatabaseService(YamSoftDbContext context) : IDatabaseService
{
    public async Task<User> CreateUserAsync(string username, string hashedPassword)
    {
        var user = new User 
        { 
            Username = username, 
            HashedPassword = hashedPassword 
        };
        
        context.Users.Add(user);
        await context.SaveChangesAsync();
        return user;
    }

    public async Task<bool> UserExistsAsync(string username)
    {
        return await context.Users.AnyAsync(u => u.Username == username);
    }

    public async Task<User> GetUserByUsernameAsync(string username)
    {
        return await context.Users
            .Include(u => u.Cart)
                .ThenInclude(c => c!.CartItems)
                    .ThenInclude(ci => ci.Product)
            .Include(u => u.Notifications)
            .FirstOrDefaultAsync(u => u.Username == username) ?? throw new InvalidOperationException("User not found");
    }

    public async Task<User?> GetUserByIdAsync(int id)
    {
        return await context.Users
            .Include(u => u.Cart)
                .ThenInclude(c => c!.CartItems)
                    .ThenInclude(ci => ci.Product)
            .Include(u => u.Notifications)
            .FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<IEnumerable<User>> GetAllUsersAsync()
    {
        return await context.Users
            .Include(u => u.Cart)
            .Include(u => u.Notifications)
            .ToListAsync();
    }

    public async Task UpdateUserAsync(User user)
    {
        context.Users.Update(user);
        await context.SaveChangesAsync();
    }

    public async Task DeleteUserAsync(int id)
    {
        var user = await context.Users.FindAsync(id);
        if (user != null)
        {
            context.Users.Remove(user);
            await context.SaveChangesAsync();
        }
    }

    public async Task<Product> CreateProductAsync(Product product)
    {
        context.Products.Add(product);
        await context.SaveChangesAsync();
        return product;
    }

    public async Task<Product?> GetProductByIdAsync(int id)
    {
        return await context.Products.FindAsync(id);
    }

    public async Task<IEnumerable<Product>> GetAllProductsAsync()
    {
        return await context.Products.ToListAsync();
    }

    public async Task<IEnumerable<Product>> GetProductsByNameAsync(string name)
    {
        return await context.Products
            .Where(p => p.Name.Contains(name))
            .ToListAsync();
    }

    public async Task UpdateProductAsync(Product product)
    {
        context.Products.Update(product);
        await context.SaveChangesAsync();
    }

    public async Task DeleteProductAsync(int id)
    {
        var product = await context.Products.FindAsync(id);
        if (product != null)
        {
            context.Products.Remove(product);
            await context.SaveChangesAsync();
        }
    }

    public async Task<Cart> CreateCartAsync(int userId)
    {
        var cart = new Cart { UserId = userId };
        context.Carts.Add(cart);
        await context.SaveChangesAsync();
        return cart;
    }

    public async Task<Cart?> GetCartByUserIdAsync(int userId)
    {
        return await context.Carts
            .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Product)
            .FirstOrDefaultAsync(c => c.UserId == userId);
    }

    public async Task<Cart?> GetCartByIdAsync(int id)
    {
        return await context.Carts
            .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Product)
            .Include(c => c.User)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task UpdateCartAsync(Cart cart)
    {
        cart.UpdatedAt = DateTime.UtcNow;
        context.Carts.Update(cart);
        await context.SaveChangesAsync();
    }

    public async Task DeleteCartAsync(int id)
    {
        var cart = await context.Carts.FindAsync(id);
        if (cart != null)
        {
            context.Carts.Remove(cart);
            await context.SaveChangesAsync();
        }
    }

    public async Task<CartItem> AddCartItemAsync(int cartId, int productId, int quantity)
    {
        var existingCartItem = await GetCartItemAsync(cartId, productId);
        
        if (existingCartItem != null)
        {
            existingCartItem.Quantity += quantity;
            await UpdateCartItemAsync(existingCartItem);
            return existingCartItem;
        }

        var cartItem = new CartItem
        {
            CartId = cartId,
            ProductId = productId,
            Quantity = quantity
        };

        context.CartItems.Add(cartItem);
        await context.SaveChangesAsync();
        
        var cart = await GetCartByIdAsync(cartId);
        if (cart != null)
        {
            await UpdateCartAsync(cart);
        }

        return cartItem;
    }

    public async Task<CartItem?> GetCartItemAsync(int cartId, int productId)
    {
        return await context.CartItems
            .Include(ci => ci.Product)
            .FirstOrDefaultAsync(ci => ci.CartId == cartId && ci.ProductId == productId);
    }

    public async Task<CartItem?> GetCartItemByIdAsync(int cartItemId)
    {
        return await context.CartItems
            .Include(ci => ci.Product)
            .Include(ci => ci.Cart)
            .FirstOrDefaultAsync(ci => ci.Id == cartItemId);
    }

    public async Task UpdateCartItemAsync(CartItem cartItem)
    {
        context.CartItems.Update(cartItem);
        await context.SaveChangesAsync();
        
        var cart = await GetCartByIdAsync(cartItem.CartId);
        if (cart != null)
        {
            await UpdateCartAsync(cart);
        }
    }

    public async Task RemoveCartItemAsync(int cartItemId)
    {
        var cartItem = await context.CartItems.FindAsync(cartItemId);
        if (cartItem != null)
        {
            var cartId = cartItem.CartId;
            context.CartItems.Remove(cartItem);
            await context.SaveChangesAsync();
            
            var cart = await GetCartByIdAsync(cartId);
            if (cart != null)
            {
                await UpdateCartAsync(cart);
            }
        }
    }

    public async Task<IEnumerable<CartItem>> GetCartItemsByCartIdAsync(int cartId)
    {
        return await context.CartItems
            .Include(ci => ci.Product)
            .Where(ci => ci.CartId == cartId)
            .ToListAsync();
    }

    public async Task<Notification> CreateNotificationAsync(int userId, NotificationType type, string message)
    {
        var notification = new Notification
        {
            UserId = userId,
            Type = type,
            Message = message
        };

        context.Notifications.Add(notification);
        await context.SaveChangesAsync();
        return notification;
    }

    public async Task<Notification?> GetNotificationByIdAsync(int id)
    {
        return await context.Notifications
            .Include(n => n.User)
            .FirstOrDefaultAsync(n => n.Id == id);
    }

    public async Task<IEnumerable<Notification>> GetNotificationsByUserIdAsync(int userId)
    {
        return await context.Notifications
            .Where(n => n.UserId == userId)
            .OrderByDescending(n => n.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<Notification>> GetUnreadNotificationsByUserIdAsync(int userId)
    {
        return await context.Notifications
            .Where(n => n.UserId == userId && n.Status != NotificationStatus.Read)
            .OrderByDescending(n => n.CreatedAt)
            .ToListAsync();
    }

    public async Task UpdateNotificationAsync(Notification notification)
    {
        context.Notifications.Update(notification);
        await context.SaveChangesAsync();
    }

    public async Task MarkNotificationAsReadAsync(int id)
    {
        var notification = await context.Notifications.FindAsync(id);
        if (notification != null)
        {
            notification.Status = NotificationStatus.Read;
            notification.ProcessedAt = DateTime.UtcNow;
            await context.SaveChangesAsync();
        }
    }

    public async Task DeleteNotificationAsync(int id)
    {
        var notification = await context.Notifications.FindAsync(id);
        if (notification != null)
        {
            context.Notifications.Remove(notification);
            await context.SaveChangesAsync();
        }
    }
}
