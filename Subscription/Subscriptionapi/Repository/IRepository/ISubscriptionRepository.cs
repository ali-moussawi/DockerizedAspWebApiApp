using Subscriptionapi.Models;

namespace Subscriptionapi.Repository.IRepository
{
    public interface ISubscriptionRepository
    {
        Task<IEnumerable<Subscription>> GetAllSubscriptionsAsync();
        Task<Subscription> GetSubscriptionByIdAsync(int subscriptionId);
        Task<IEnumerable<Subscription>> GetSubscriptionsByUserIdAsync(int userId);
        Task CreateSubscriptionAsync(Subscription subscription);
        Task UpdateSubscriptionAsync(Subscription subscription);
        Task DeleteSubscriptionAsync(int subscriptionId, int userId);
        Task<int> CalculateRemainingDaysAsync(int subscriptionId);
        Task SaveAsync();


    }
}
