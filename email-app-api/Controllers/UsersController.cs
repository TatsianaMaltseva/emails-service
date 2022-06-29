using email_app_api.Models;
using email_app_api.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace email_app_api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : Controller
    {
        private readonly UserService userService;

        public UsersController(UserService userService)
        {
            this.userService = userService;
        }

        [HttpPost]
        public IActionResult Login(LoginRequest loginRequest)
        {
            LoginResponse loginResponse = userService.Login(loginRequest);
            if (loginResponse != null)
            {
                return Ok(loginResponse);
            }
            return Unauthorized("Such user does not exist");
        }

        [HttpGet]
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
