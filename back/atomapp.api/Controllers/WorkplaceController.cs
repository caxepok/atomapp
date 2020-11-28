using atomapp.api.Infrastructure;
using atomapp.api.Services;
using atomapp.api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        [HttpGet("")]
        public async Task<IActionResult> Get()
        {
            return Ok(await _workplaceService.GetWorkplaces());
        }

        [HttpGet("{id}/worker")]
        public IActionResult GetWorkers(long id)
        {
            return Ok(_workplaceService.GetWorkerOfWorkplace(id));
        }

        [HttpGet("worker")]
        public IActionResult GetWorkers()
        {
            return Ok(_workplaceService.GetWorkers());
        }

        [HttpGet("worker/subordinates")]
        public IActionResult GetSubordinates(long userId)
        {
            return Ok(_workplaceService.GetSubordinates(userId));
        }
    }
}