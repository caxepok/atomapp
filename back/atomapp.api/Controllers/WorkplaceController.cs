using atomapp.api.Infrastructure;
using atomapp.api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace atomapp.api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WorkplaceController : ControllerBase
    {
        private readonly ILogger<WorkplaceController> _logger;
        private readonly AppSettings _appSettings;
        private readonly IWorkplaceService _workplaceService;

        public WorkplaceController(ILogger<WorkplaceController> logger, IOptions<AppSettings> options, IWorkplaceService workplaceService)
        {
            _logger = logger;
            _appSettings = options.Value;
            _workplaceService = workplaceService;
        }

        /// <summary>
        /// Возвращает оргструктуру
        /// </summary>
        [HttpGet("")]
        public IActionResult Get()
        {
            return Ok(_workplaceService.GetWorkplaces());
        }

        /// <summary>
        /// Возвращает сотрдуников подразделения
        /// </summary>
        /// <param name="id">идентификатор подразделения</param>
        [HttpGet("{id}/worker")]
        public IActionResult GetWorkers(long id)
        {
            return Ok(_workplaceService.GetWorkerOfWorkplace(id));
        }

        /// <summary>
        /// Возвращает список всех сотрудников
        /// </summary>
        [HttpGet("worker")]
        public IActionResult GetWorkers()
        {
            return Ok(_workplaceService.GetWorkers());
        }

        /// <summary>
        /// Возвращае всех подчинённых сотрудника
        /// </summary>
        /// <param name="userId">идентификатор пользователя</param>
        [HttpGet("worker/subordinates")]
        public IActionResult GetSubordinates(long userId)
        {
            return Ok(_workplaceService.GetSubordinates(userId));
        }

        /// <summary>
        /// Возвращает данные для аналитики
        /// </summary>
        /// <param name="userId">идентификатор пользователя</param>
        [HttpGet("worker/summary")]
        public IActionResult Summary(long userId)
        {
            return Ok(_workplaceService.GetSummary(userId));
        }
    }
}