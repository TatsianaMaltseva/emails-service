using email_app_api.Models;
using email_app_api.Services;
using Microsoft.AspNetCore.Mvc;

namespace email_app_api.Controllers
{
    [ApiController]
    public class TasksController : Controller
    {
        private readonly TaskService taskService;
        public TasksController(TaskService taskService)
        {
            this.taskService = taskService;
        }

        [HttpPost]
        [Route("users/{userId}/[controller]")]
        public IActionResult AddTask([FromRoute] int userId, [FromBody] Task task)
        {
            Task addedTask = taskService.AddTask(userId, task);
            return addedTask != null
                ? Ok(addedTask)
                : BadRequest("Task data is not valid");
        }
    }
}
