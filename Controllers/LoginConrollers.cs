using Todo.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Todo.Repostories;

namespace Todo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserLoginController : ControllerBase
    {

        private readonly IUserRepository _user;

        private IConfiguration _config;

        public UserLoginController(IConfiguration config, IUserRepository user)
        {
            _config = config;
            _user = user;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] UserLogin userLogin)
        {
            var user = await Authenticate(userLogin);

            if (user != null)
            {
                var token = Generate(user);
                return Ok(token);
            }

            return NotFound("User not found");
        }

        private string Generate(user user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            Console.WriteLine(" user Id in claims : " + user.UserId);
            var claims = new[]
            {
                new Claim(ClaimTypes.SerialNumber, user.UserId.ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.UserName),
                new Claim(ClaimTypes.Email, user.Email),

            };

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
              _config["Jwt:Audience"],
              claims,
              expires: DateTime.Now.AddMinutes(15),
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private async Task<user> Authenticate(UserLogin userLogin)
        {

            // var currentUser = UserConstants.Users.FirstOrDefault(o => o.UserName.ToLower() == userLogin.UserName.ToLower() && o.Password == userLogin.Password);
            // if (currentUser != null)
            // {
            //     return currentUser;
            // }

            // return null;

            var currentUser = await _user.GetByUsername(userLogin.UserName);
            Console.WriteLine(currentUser.UserName);
            if (currentUser == null)
            {
                return null;
            }
            Console.WriteLine("db password" + currentUser.Password);
            Console.WriteLine("user password" + userLogin.Password);

            if (currentUser.Password != userLogin.Password)
            {
                return null;
            }
            return currentUser;
        }
    }
}