using Subscriptionapi.Models;

namespace Subscriptionapi.Repository.IRepository
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User> GetUserByIdAsync(int userId);
        Task<User> GetUserByUserNameAsync(string username);
        Task CreateUserAsync(User user);
        Task UpdateUserAsync(User user);
        Task DeleteUserAsync(int userId);
        Task<IEnumerable<Subscription>> GetAllUserSubscriptionsAsync(int userId);
        Task<User> GetUserByEmailAsync(string email);
  


        bool AuthenticateUser(string Email, string hashedpassword);
    }
}
