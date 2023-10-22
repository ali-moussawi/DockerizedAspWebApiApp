using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace Subscriptionapi.Models
{
    public class User
    {

        [Key]
        [BindNever]
        [ValidateNever]
        public int UserId { get; set; }

        [Required]
        [MaxLength(255)]
        public string Username { get; set; }

        [Required]
        [MaxLength(255)]
        public string Email { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        [BindNever]
        [ValidateNever]
        public virtual ICollection<Subscription> Subscriptions { get; set; }
    }
}
