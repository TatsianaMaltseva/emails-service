using Microsoft.AspNetCore.Mvc;

namespace email_app_api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : Controller
    {
        public UsersController()
        {
        }

        [HttpPost]
        public LoginResponse Get()
        {
            //update last time active
            LoginResponse loginResponse = new LoginResponse()
            {
                UserId = 10,
                Role = "Admin"
            };
            return loginResponse;
        }
    }
}
