using System.ComponentModel.DataAnnotations;

namespace Subscriptionapi.Models
{
    public class LoginModel
    {
        [Required]
        public string email { get; set; }

        [Required]
        public string password { get; set; }


        public string? AccessToken { get; set; }

    }
}
