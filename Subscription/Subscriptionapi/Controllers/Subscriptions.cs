using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Subscriptionapi.Crypto;
using Subscriptionapi.Models;
using Subscriptionapi.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace Subscriptionapi.Controllers
{
    [Route("api/[controller]")]
     [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    public class Subscriptions : ControllerBase
    {



        private readonly UserService _userService;
        private readonly SubscriptionService _subService;
        private readonly IConfiguration _configuration;
        private readonly ILogger<Subscription> _logger;
        public Subscriptions(UserService userService, IConfiguration configuration, SubscriptionService subService,ILogger<Subscription> logger)
        {
            _userService = userService;
            _configuration = configuration;
            _subService = subService;
            _logger = logger;
        }




        [HttpGet("UserSubscriptions")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> UserSubscriptions()
        {
         
     
            string authorizationHeader = Request.Headers["Authorization"].ToString();

            if (string.IsNullOrEmpty(authorizationHeader))
            {
                return BadRequest("Authorization header is missing.");
            }

            if (authorizationHeader.StartsWith("Bearer "))
            {
                string token = authorizationHeader.Substring(7);

                var tokenHandler = new JwtSecurityTokenHandler();
                var tokenValidationParameters = new TokenValidationParameters
                {
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"])),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                };

                ClaimsPrincipal claimsPrincipal;
                try
                {
                    claimsPrincipal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken validatedToken);
                }
                catch (Exception ex)
                {
                    return BadRequest("Token validation failed: " + ex.Message);
                }


                string userEmail = claimsPrincipal.FindFirst(ClaimTypes.Name).Value;


                try
                {
                var targetuser = await _userService.GetUserByemail(userEmail);

                var subscription = await _subService.GetSubscriptionsByUserId(targetuser.UserId);

                    _logger.LogInformation("Email: "+userEmail+ " Fetched All Subscriptions" );
                    return Ok(subscription);
                }
                catch (Exception ex) {
                    _logger.LogInformation("Error Happend at line 85 in UserSubscriptions " + ex);
                    return BadRequest("Unexpected Error !");
                }
       
                

               
            }

            return BadRequest("Invalid token format.");
        }












        [HttpGet("SubscriptionRemainingDays")]
        [MapToApiVersion("1.0")]
        public async  Task<IActionResult> SubscriptionRemainingDays(int Subscriptionid)
        {

                try
                {
                  var remainingdays = await _subService.CalculateRemainingDays(Subscriptionid);

                if(remainingdays != -1)
                {
                    _logger.LogInformation("Remaining Days :" + remainingdays.ToString());
                    return Ok("Remaining Days are: " + remainingdays);
                }
                else
                {
                    return BadRequest("No Data returned");
                }
                  
                }
                catch (Exception ex)
                {
                    _logger.LogInformation("Error Happend  in SubscriptionRemainingDys " + ex);
                    return BadRequest("Unexpected Error !");
                }

          
        }


        [HttpPost("AddSubscription")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> AddSubscription(Subscription subscription)
        {
            if (subscription.StartDate < DateTime.UtcNow)
            {
                return BadRequest("Start Date Can not be this old");
            }

            if (subscription.EndDate < subscription.StartDate)
            {
                return BadRequest("End Date Cannot be less than start Date");
            }
            // Get the authorization header from the request
            string authorizationHeader = Request.Headers["Authorization"].ToString();

            if (string.IsNullOrEmpty(authorizationHeader))
            {
                return BadRequest("Authorization header is missing.");
            }

            if (authorizationHeader.StartsWith("Bearer "))
            {
                string token = authorizationHeader.Substring(7);

                var tokenHandler = new JwtSecurityTokenHandler();
                var tokenValidationParameters = new TokenValidationParameters
                {
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"])),
                    ValidateIssuer = false, 
                    ValidateAudience = false,
                };

                ClaimsPrincipal claimsPrincipal;
                try
                {
                    claimsPrincipal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken validatedToken);
                }
                catch (Exception ex)
                {
                    return BadRequest("Token validation failed: " + ex.Message);
                }

              
                string userEmail = claimsPrincipal.FindFirst(ClaimTypes.Name).Value;
                try
                {
                    var targetuser = await _userService.GetUserByemail(userEmail);

                    subscription.UserId = targetuser.UserId;
                   await _subService.CreateSubscription(subscription);

                    _logger.LogInformation("Email: " + userEmail + " Creates a new Subscription ");

                    return Ok("Subscription Created Successfully");
                }

                 catch (Exception ex) {
                    _logger.LogInformation("Error Happend in AddSubscription " + ex);
                    return BadRequest("Unexpected Error !");
                }
                
            }

            return BadRequest("Invalid token format.");
        }




        [HttpPut("UpdateSubscription")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> UpdateSubscription(Subscription subscription)
        {
            if (subscription.StartDate < DateTime.UtcNow)
            {
                return BadRequest("Start Date Can not be this old");
            }

            if (subscription.EndDate < subscription.StartDate)
            {
                return BadRequest("End Date Cannot be less than start Date");
            }

            string authorizationHeader = Request.Headers["Authorization"].ToString();

            if (string.IsNullOrEmpty(authorizationHeader))
            {
                return BadRequest("Authorization header is missing.");
            }

            if (authorizationHeader.StartsWith("Bearer "))
            {
                string token = authorizationHeader.Substring(7);

                var tokenHandler = new JwtSecurityTokenHandler();
                var tokenValidationParameters = new TokenValidationParameters
                {
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"])),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                };

                ClaimsPrincipal claimsPrincipal;
                try
                {
                    claimsPrincipal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken validatedToken);
                }
                catch (Exception ex)
                {
                    return BadRequest("Token validation failed: " + ex.Message);
                }


                string userEmail = claimsPrincipal.FindFirst(ClaimTypes.Name).Value;
                try
                {
                    var targetuser = await _userService.GetUserByemail(userEmail);


                    var targetsubscription = await _subService.GetSubscriptionById(subscription.SubscriptionId);

                    if(targetsubscription == null || targetsubscription.UserId != targetuser.UserId)
                    {
                        return BadRequest("subscription not found or you dont have access to update this subscription");
                    }
                    else
                    {

                    targetsubscription.StartDate= subscription.StartDate;
                        targetsubscription.EndDate= subscription.EndDate;
                        targetsubscription.SubscriptionType  = subscription.SubscriptionType;
                        await _subService.SaveChanges();

                    _logger.LogInformation("Email: " + userEmail + " Updates  Subscription  with subid "+ subscription.SubscriptionId);

                    return Ok("Subscription Updated Successfully");
                    }


                }

                catch (Exception ex)
                {
                    _logger.LogInformation("Error Happend in UpdateSubscription " + ex);
                    return BadRequest("Unexpected Error !");
                }

            }

            return BadRequest("Invalid token format.");
        }



        [HttpDelete("DeleteSubscription")]
        [MapToApiVersion("1.0")]

        public async Task<IActionResult> DeleteSubscription(int subscriptionid)
        {

            // Get the authorization header from the request
            string authorizationHeader = Request.Headers["Authorization"].ToString();

            if (string.IsNullOrEmpty(authorizationHeader))
            {
                return BadRequest("Authorization header is missing.");
            }

            if (authorizationHeader.StartsWith("Bearer "))
            {
                string token = authorizationHeader.Substring(7);

                var tokenHandler = new JwtSecurityTokenHandler();
                var tokenValidationParameters = new TokenValidationParameters
                {
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"])),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                };

                ClaimsPrincipal claimsPrincipal;
                try
                {
                    claimsPrincipal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken validatedToken);
                }
                catch (Exception ex)
                {
                    return BadRequest("Token validation failed: " + ex.Message);
                }


                string userEmail = claimsPrincipal.FindFirst(ClaimTypes.Name).Value;
                try
                {
                    var targetuser = await _userService.GetUserByemail(userEmail);

                    await _subService.DeleteSubscription(subscriptionid, targetuser.UserId);

                    _logger.LogInformation("Email: " + userEmail + " Deletes a Subscription ");

                    return Ok("Subscription Deleted Successfully");
                }

                catch (Exception ex)
                {
                    _logger.LogInformation("Error Happend  in DeleteSubscription " + ex);
                    return BadRequest("Unexpected Error !");
                }

            }

            return BadRequest("Invalid token format.");
        }




    }


}

