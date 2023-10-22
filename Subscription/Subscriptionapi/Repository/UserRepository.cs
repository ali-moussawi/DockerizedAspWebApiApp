using Microsoft.EntityFrameworkCore;
using Subscriptionapi.Models;
using Subscriptionapi.Repository.IRepository;
using System;
using Subscriptionapi.Crypto;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace Subscriptionapi.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;
    
        public UserRepository(ApplicationDbContext context )
        {
            _context = context;
          
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User> GetUserByIdAsync(int userId)
        {
            return await _context.Users.FindAsync(userId);
        }

        public async Task<User> GetUserByUserNameAsync(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task CreateUserAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateUserAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteUserAsync(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Subscription>> GetAllUserSubscriptionsAsync(int userId)
        {
            return await _context.Subscription.Where(s => s.UserId == userId).ToListAsync();
        }








        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _context.Users.Where(u => u.Email == email).FirstOrDefaultAsync();
        }



        public  bool AuthenticateUser(string Email, string Enteredpassword)
        {
            var targetuser =  _context.Users.Where(u=>u.Email == Email.ToLower().Trim()).FirstOrDefault();

            if(targetuser == null)
            {
                return false;
            }

            else
            {
                bool isverified = Hash.VerifyPassword(Enteredpassword, targetuser.PasswordHash);
                return isverified;
            }
             
         
           
        }



      




    }

}