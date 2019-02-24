using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using Product.WebApi.Extentions;
using Serilog;
using Product.WebApi.Services;

namespace Product.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private IConfiguration _config;
        private IProductsService _service { get; set; }

        public TokenController(IConfiguration config, IProductsService service)
        {
            _config = config;
            _service = service;
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult CreateToken([FromBody] TokenRequest request)
        {
            try
            {
                IActionResult response = Unauthorized();

                var user = Authenticate(request);

                if (!user.IsObjectNull())
                {
                    var tokenString = BuildToken(user);
                    response = Ok(new { token = tokenString });
                }

                return response;
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"{GetType().Name}.{System.Reflection.MethodBase.GetCurrentMethod().Name} exception");
                return StatusCode(500, "Internal server error");
            }
        }

        #region Private
        private string BuildToken(UserModel user)
        {
            var claims = new[] {
                new Claim(JwtRegisteredClaimNames.Sub, user.Name),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                _config["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private UserModel Authenticate(TokenRequest request)
        {
            UserModel user = null;

            var users = _service.GetUsers().Result;
            var currentUser = users.SingleOrDefault(x => x.Name == request.Username);

            if (request.Username == currentUser.Name && request.Password == currentUser.Password)
            {
                user = new UserModel { Name = currentUser.Name, Email = currentUser.Email };
            }

            return user;
        }
        #endregion

        public class TokenRequest
        {
            public string Username { get; set; }
            public string Password { get; set; }
        }

        private class UserModel
        {
            public string Name { get; set; }
            public string Email { get; set; }
        }
    }
}