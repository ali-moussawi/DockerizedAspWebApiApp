using Microsoft.AspNetCore.Identity;
using Subscriptionapi.Models;

namespace Subscriptionapi.Crypto
{
    public class Hash
    {


        public static string HashPassword(string password)
        {
            string salt = BCrypt.Net.BCrypt.GenerateSalt(12); // You can configure the number of rounds (12 is a reasonable value)
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password, salt);

            return hashedPassword;
        }

        public static bool VerifyPassword(string enteredPassword, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(enteredPassword, hashedPassword);
        }


    }
}
