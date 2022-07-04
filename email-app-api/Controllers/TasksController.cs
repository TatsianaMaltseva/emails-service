using email_app_api.Models;
using email_app_api.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

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

        [HttpPut]
        [Route("users/{userId}/[controller]/{taskId}")]
        public IActionResult EditTask([FromRoute] int taskId, [FromBody] Task task)
        {
            Task editedTask = taskService.EditTask(taskId, task);
            return editedTask != null
                ? Ok(editedTask)
                : BadRequest("Task data is not valid");
        }

        [HttpGet]
        [Route("users/{userId}/[controller]")]
        public List<Task> GetTasks([FromRoute] int userId)
        {
            return taskService.GetTasks(userId);
        }

        [HttpDelete]
        [Route("users/{userId}/[controller]/{taskId}")]
        public IActionResult DeleteTask([FromRoute] int taskId)
        {
            bool ifWasDeleted = taskService.DeleteTask(taskId);
            return ifWasDeleted
                ? NoContent()
                : BadRequest("Task was not deleted");
        }
    }
}
