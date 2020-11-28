using atomapp.api.Models.API;
using atomapp.api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace atomapp.api.Controllers
{
    [Route("[controller]")]
    public class TaskController : ControllerBase
    {
        private readonly ILogger<TaskController> _logger;
        private readonly ITaskService _taskService;

        public TaskController(ILogger<TaskController> logger, ITaskService taskService)
        {
            _logger = logger;
            _taskService = taskService;
        }

        /// <summary>
        /// Возвращает задачи, назначенные на пользователя
        /// </summary>
        [HttpGet("inbox")]
        public IActionResult GetInbox(long userId)
        {
            return Ok(_taskService.GetInbox(userId, false));
        }

        /// <summary>
        /// Возвращает задачи, назначенные пользователем
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet("outbox")]
        public IActionResult GetOutbox(long userId)
        {
            return Ok(_taskService.GetOutbox(userId, true));
        }

        /// <summary>
        /// Возвращает задачу по её идентификатору
        /// </summary>
        /// <param name="id">идентификатор задачи</param>
        [HttpGet("{id}")]
        public IActionResult GetById(long id)
        {
            return Ok(_taskService.GetById(id));
        }

        /// <summary>
        /// Добавляет коментарий к задаче
        /// </summary>
        /// <param name="id">идентификатор задачи</param>
        /// <param name="comment">коментарий</param>
        /// <returns></returns>
        [HttpPost("{id}/comment")]
        public IActionResult AddTaskComment(long id, [FromBody] CommentTaskAction action)
        {
            return Ok(_taskService.AddTaskComment(id, action));
        }

        /// <summary>
        /// Создаёт задачу
        /// </summary>
        /// <param name="action">данные задачи</param>
        [HttpPost("")]
        public IActionResult CreateTask(long userId, [FromBody] MakeTaskAction action)
        {
            return Ok(_taskService.CreateTask(userId, action));
        }

        /// <summary>
        /// Завершает задачу
        /// </summary>
        /// <param name="id">идентификатор задачи</param>
        /// <param name="comment">коментарий завершения задачи</param>
        /// <returns></returns>
        [HttpPost("{id}/finish")]
        public IActionResult FinishTask(long id, [FromBody] FinishTaskAction action)
        {
            _taskService.FinishTask(id, action);
            return NoContent();
        }


        // - костыль для редиректов, т.к. фронт и бек хостятся в одном сервисе
        // иначе глюит F5
        [HttpGet("/inbox")]
        public IActionResult RedirectInboxToRoot() =>
            Redirect("/");
        [HttpGet("/outbox")]
        public IActionResult RedirectOutboxToRoot() =>
            Redirect("/");
        [HttpGet("/inbox/{id}")]
        public IActionResult RedirectInboxToRoot(string id) =>
            Redirect("/");
        [HttpGet("/outbox/{id}")]
        public IActionResult RedirectOutboxToRoot(string id) =>
            Redirect("/");
    }
}
