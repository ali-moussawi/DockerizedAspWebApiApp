using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using Npgsql;
using Subscriptionapi.Models;
using Subscriptionapi.Repository.IRepository;
using System;

namespace Subscriptionapi.Repository
{

    public class SubscriptionRepository : ISubscriptionRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        public SubscriptionRepository(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }


        public async Task<IEnumerable<Subscription>> GetAllSubscriptionsAsync()
        {
            return await _context.Subscription.ToListAsync();
        }

        public async Task<Subscription> GetSubscriptionByIdAsync(int subscriptionId)
        {
            return await _context.Subscription.FindAsync(subscriptionId);
        }

        public async Task<IEnumerable<Subscription>> GetSubscriptionsByUserIdAsync(int userId)
        {
            return await _context.Subscription.Where(s => s.UserId == userId).ToListAsync();
        }

        public async Task CreateSubscriptionAsync(Subscription subscription)
        {
            _context.Subscription.Add(subscription);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateSubscriptionAsync(Subscription subscription)
        {
            _context.Subscription.Update(subscription);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteSubscriptionAsync(int subscriptionId, int userId)
        {
            var subscription = await _context.Subscription
                .Where(s => s.UserId == userId && s.SubscriptionId == subscriptionId)
                .FirstOrDefaultAsync();

            if (subscription != null)
            {
                _context.Subscription.Remove(subscription);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<int> CalculateRemainingDaysAsync(int subscriptionId)
        {
            using (var connection = new NpgsqlConnection(_configuration.GetConnectionString("PostgresConnection")))
            {
                await connection.OpenAsync();

                using (var command = new NpgsqlCommand("SELECT public.calculate_remaining_days(@subscriptionId)", connection))
                {
                    command.Parameters.AddWithValue("subscriptionId", subscriptionId);

                    var result = await command.ExecuteScalarAsync();

                    if (result != null && result != DBNull.Value)
                    {
                        return Convert.ToInt32(result);
                    }
                    else
                    {
                        return -1;
                    }
                }
            }
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }




}

