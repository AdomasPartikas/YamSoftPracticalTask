using YamSoft.API.Entities;
using YamSoft.API.Enums;

namespace YamSoft.API.Interfaces;

public interface IDatabaseService
{
    Task<User> CreateUserAsync(string username, string hashedPassword);
    Task<bool> UserExistsAsync(string username);
    Task<User> GetUserByUsernameAsync(string username);
    Task<User?> GetUserByIdAsync(int id);
    Task<IEnumerable<User>> GetAllUsersAsync();
    Task UpdateUserAsync(User user);
    Task DeleteUserAsync(int id);

    Task<Product> CreateProductAsync(Product product);
    Task<Product?> GetProductByIdAsync(int id);
    Task<IEnumerable<Product>> GetAllProductsAsync();
    Task<IEnumerable<Product>> GetProductsByNameAsync(string name);
    Task UpdateProductAsync(Product product);
    Task DeleteProductAsync(int id);

    Task<Cart> CreateCartAsync(int userId);
    Task<Cart?> GetCartByUserIdAsync(int userId);
    Task<Cart?> GetCartByIdAsync(int id);
    Task UpdateCartAsync(Cart cart);
    Task DeleteCartAsync(int id);

    Task<CartItem> AddCartItemAsync(int cartId, int productId, int quantity);
    Task<CartItem?> GetCartItemAsync(int cartId, int productId);
    Task<CartItem?> GetCartItemByIdAsync(int cartItemId);
    Task UpdateCartItemAsync(CartItem cartItem);
    Task RemoveCartItemAsync(int cartItemId);
    Task<IEnumerable<CartItem>> GetCartItemsByCartIdAsync(int cartId);

    Task<Notification> CreateNotificationAsync(int userId, NotificationType type, string message);
    Task<Notification?> GetNotificationByIdAsync(int id);
    Task<IEnumerable<Notification>> GetNotificationsByUserIdAsync(int userId);
    Task<IEnumerable<Notification>> GetUnreadNotificationsByUserIdAsync(int userId);
    Task UpdateNotificationAsync(Notification notification);
    Task MarkNotificationAsReadAsync(int id);
    Task DeleteNotificationAsync(int id);
}