using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Subscriptionapi.Models;
using Subscriptionapi.Services;
using System.Security.Claims;
using Subscriptionapi.Crypto;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Polly;
using System.Text.RegularExpressions;

namespace Subscriptionapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Security : ControllerBase
    {


        private readonly UserService _userService;
        private readonly IConfiguration _configuration;
        public Security(UserService userService, IConfiguration configuration)
        {
            _userService = userService;
            _configuration = configuration;
        }




        [HttpPost("register")]
        public async Task<IActionResult> Register(User registerModel)
        {

            if (!IsValidEmail(registerModel.Email))
            {
                return BadRequest("Please enter valid email");
            }
            if (string.IsNullOrWhiteSpace(registerModel.Username) || string.IsNullOrWhiteSpace(registerModel.PasswordHash) || string.IsNullOrWhiteSpace(registerModel.Email))
            {
                return BadRequest("Please validate all inputs");
            }
           
            if ( await _userService.GetUserByemail(registerModel.Email) != null)
            {
                return BadRequest("Email Already Existed");
            }

            else
            {
                User newuser = registerModel;
                newuser.PasswordHash = Hash.HashPassword(registerModel.PasswordHash);
               await  _userService.CreateUser(newuser);

                return Ok("User Created Successfully");

            }

        }





        [HttpPost("login")]

        public  async Task<IActionResult> Login(LoginModel login)
        {

            if (string.IsNullOrWhiteSpace(login.email) || string.IsNullOrWhiteSpace(login.password))
            {
                return BadRequest("Cannot send empty Fields");

            }


            var isuserAuthenticated =   _userService.AuthenticateUser(login.email, login.password);

            if (!isuserAuthenticated)
            {
                return Unauthorized();
            }
            else
            {

                var targetuser = await  _userService.GetUserByemail(login.email);

                if (targetuser == null)
                {

                    return BadRequest("user not found");
                }
                else
                {
                    var claims = new List<Claim>
                            {
                                new Claim(ClaimTypes.Name, targetuser.Email),
                               new Claim("username", targetuser.Username),
                
                            };

                    var token = GetToken(login);

                   login.AccessToken = token;

                    return Ok(login);
                }

              
            }

         
        }




        private  bool IsValidEmail(string email)
        {
            string pattern = @"^[\w-]+(\.[\w-]+)*@([\w-]+\.)+[a-zA-Z]{2,7}$";
            return Regex.IsMatch(email, pattern);
        }


        private string GetToken(LoginModel login)
        {
            var claims = new List<Claim>
                            {
                  new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                  new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                  new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                  new Claim(ClaimTypes.Name, login.email),

                            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims,
                expires: DateTime.UtcNow.AddHours(3),
                signingCredentials: signIn
                );
            string Token = new JwtSecurityTokenHandler().WriteToken(token);
            return Token;
        }


    }
}
