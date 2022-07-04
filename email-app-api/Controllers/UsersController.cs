using email_app_api.Models;
using email_app_api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace email_app_api.Controllers
{
    [ApiController]
    public class UsersController : Controller
    {
        private readonly UserService userService;

        public UsersController(UserService userService)
        {
            this.userService = userService;
        }

        [HttpPost]
        [Route("login")]
        public IActionResult Login(LoginRequest loginRequest)
        {
            User user = userService.GetUser(loginRequest.Email, loginRequest.Password);
            if (user == null)
            {
                return Unauthorized("Such user does not exist");
            }

            var claims = new List<Claim>() {
                new Claim("id", user.Id.ToString()),
                new Claim("role", user.Role.ToString())
            };

            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("superSecretKey@345"));
            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
            var tokeOptions = new JwtSecurityToken(
                issuer: "https://localhost:5000",
                audience: "https://localhost:5000",
                claims,
                expires: DateTime.Now.AddMinutes(10),
                signingCredentials: signinCredentials
            );
            var token = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
            return Ok( new { token } );
        }

        [HttpGet]
        [Authorize(Roles = Roles.Admin)]
        [Route("[controller]")]
        public IActionResult GetUsers([FromQuery] int currentUserId)
        {
            List<User> users = userService.GetUsers(currentUserId);
            if (users != null)
            {
                return Ok(users);
            }
            return BadRequest("Not enough rights");
        }
    }
}
