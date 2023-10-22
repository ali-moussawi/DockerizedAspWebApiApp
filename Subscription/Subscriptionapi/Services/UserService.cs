using Microsoft.EntityFrameworkCore;
using Subscriptionapi.Crypto;
using Subscriptionapi.Models;
using Subscriptionapi.Repository.IRepository;

namespace Subscriptionapi.Services
{
    public class UserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async  Task<IEnumerable<User>> GetAllUsers()
        {
            return  await _userRepository.GetAllUsersAsync();
        }

        public async Task<User> GetUserById(int userId)
        {
            return await _userRepository.GetUserByIdAsync(userId);
        }



        public async Task<User> GetUserByUserName(string username)
        {
            return await  _userRepository.GetUserByUserNameAsync(username);
        }
        public async  Task CreateUser(User user)
        {
           
            await _userRepository.CreateUserAsync(user);
        }

        public  async Task UpdateUser(User user)
        {

            await _userRepository.UpdateUserAsync(user);
        }

        public async Task DeleteUser(int userId)
        {

            await _userRepository.DeleteUserAsync(userId);
        }


        public bool AuthenticateUser(string Email, string Enteredpassword)
        {
           return _userRepository.AuthenticateUser(Email, Enteredpassword);



        }



        public async Task<User> GetUserByemail(string email)
        {
       return await _userRepository.GetUserByEmailAsync(email);
        }
    }
}