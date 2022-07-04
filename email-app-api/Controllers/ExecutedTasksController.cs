using email_app_api.Models;
using email_app_api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace email_app_api.Controllers
{
    [ApiController]
    public class ExecutedTasksController : Controller
    {
        private readonly ExecutedTasksService executedTasksService;

        public ExecutedTasksController(ExecutedTasksService executedTasksService)
        {
            this.executedTasksService = executedTasksService;
        }

        [HttpGet]
        [Authorize(Roles = Roles.Admin)]
        [Route("users/{userId}/[controller]")]
        public List<ExecutedTask> GetExecutedTasks([FromRoute] int userId)
        {
            return executedTasksService.GetExecutedTasksForUser(userId);
        }
    }
}
