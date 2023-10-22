using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Text.Json.Serialization;

namespace Subscriptionapi.Models
{
    public class Subscription
    {

        [Key]
        [ValidateNever]
        [BindNever]
        public int SubscriptionId { get; set; }

      
        [ValidateNever]
        [BindNever]
        [ForeignKey("User")]
        public int UserId { get; set; }
        [ValidateNever]
        [BindNever]
        [JsonIgnore]
        public User User { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Required]
        [MaxLength(50)]
        public string SubscriptionType { get; set; }
    }
}
