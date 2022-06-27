using email_app_api.Models;
using email_app_api.Services;
using Microsoft.AspNetCore.Mvc;

namespace email_app_api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : Controller
    {
        private readonly UserService userService;
        public UsersController(
            UserService userService
        ){
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

        public User[] GetUsers()
        {
            return null;
        }
    }
}
