using Microsoft.EntityFrameworkCore;
using Subscriptionapi.Models;
using Subscriptionapi.Repository.IRepository;
using System.Security.Claims;

namespace Subscriptionapi.Services
{
    public class SubscriptionService
    {
        private readonly ISubscriptionRepository _subscriptionRepository;

        public SubscriptionService(ISubscriptionRepository subscriptionRepository)
        {
            _subscriptionRepository = subscriptionRepository;
        }

        public  async Task<IEnumerable<Subscription>> GetAllSubscriptions()
        {
            return  await _subscriptionRepository.GetAllSubscriptionsAsync();
        }


        public async Task<IEnumerable<Subscription>> GetSubscriptionsByUserId(int userid)
        {
            return  await _subscriptionRepository.GetSubscriptionsByUserIdAsync(userid);
        }

        public async Task<Subscription> GetSubscriptionById(int subscriptionId)
        {
            return await _subscriptionRepository.GetSubscriptionByIdAsync(subscriptionId);
        }

        public async Task CreateSubscription(Subscription subscription)
        {


             await   _subscriptionRepository.CreateSubscriptionAsync(subscription);
        }

        public async Task UpdateSubscription(Subscription subscription)
        {
           
           await  _subscriptionRepository.UpdateSubscriptionAsync(subscription);
        }


        public async Task DeleteSubscription(int subscriptionId, int userid)
        {
           
            await _subscriptionRepository.DeleteSubscriptionAsync(subscriptionId,userid);
        }





        public async Task<int> CalculateRemainingDays(int subscriptionId)
        {



            return  await _subscriptionRepository.CalculateRemainingDaysAsync(subscriptionId);
        }


        public async Task SaveChanges()
        {
           await _subscriptionRepository.SaveAsync();
        }
    }
}