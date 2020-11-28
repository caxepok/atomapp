using atomapp.api.Infrastructure;
using atomapp.api.Models.API;
using atomapp.api.Services;
using atomapp.api.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace atomapp.api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RecordController : ControllerBase
    {
        private readonly ILogger<RecordController> _logger;
        private readonly AppSettings _appSettings;
        private readonly ISemanticService _semanticService;
        private readonly ITaskService _taskService;

        public RecordController(ILogger<RecordController> logger, IOptions<AppSettings> options, ITaskService taskService, ISemanticService semanticService)
        {
            _logger = logger;
            _appSettings = options.Value;
            _semanticService = semanticService;
            _taskService = taskService;
        }

        [HttpGet("ping")]
        public IActionResult Ping()
        {
            return Ok();
        }

        /// <summary>
        /// Метод для симуляции создания задачи для быстрой отладки фронта
        /// </summary>
        /// <returns></returns>
        [HttpPost("upload/template/{id}")]
        public async Task<IActionResult> UploadTemplate(int id, long userId, long? parentTaskId)
        {
            string guid = Guid.NewGuid().ToString();
            var bytes = System.IO.File.ReadAllBytes(Path.Combine(_appSettings.ExamplesPath, $"template{id}.mp3"));
            try
            {
                var result = await _semanticService.ProcessAsync(guid, bytes, false);
                switch (result.SemanticsAction)
                {
                    case Models.SemanticsAction.MakeTask:
                        return Ok(_taskService.CreateMakeTaskAction(userId, parentTaskId, result));
                    default:
                        throw new NotImplementedException();
                }
            }
            catch (Exception ex)
            {
                return Ok(new MakeTaskAction() { Error = "Сообщение не распознано" });
            }
        }

        /// <summary>
        /// Метод отправки звукового фрагмента на распознавание
        /// </summary>
        /// <param name="userId">идентификатор пользователя</param>
        /// <param name="parentTaskId">родительская задача</param>
        /// <param name="files">звуковой файл</param>
        /// <param name="isOpus">признак что файл в формате opus (так его записывает браузер)</param>
        /// <returns></returns>
        [HttpPost("upload")]
        public async Task<IActionResult> Upload(long userId, long? taskId, long? parentTaskId, bool isOpus = false)
        {
            _logger.LogInformation("Started Speech-To-Text...");

            if (Request.Form.Files.Count == 1)
            {
                string guid = Guid.NewGuid().ToString();

                var streamIn = Request.Form.Files[0].OpenReadStream();
                MemoryStream ms = new MemoryStream();
                streamIn.CopyTo(ms);
                byte[] bytes = ms.ToArray();
                // process mp3 file
                
                try
                {
                    var result = await _semanticService.ProcessAsync(guid, bytes, isOpus);
                    switch (result.SemanticsAction)
                    {
                        case Models.SemanticsAction.MakeTask:
                            return Ok(_taskService.CreateMakeTaskAction(userId, parentTaskId, result));
                        case Models.SemanticsAction.AddComment:
                            return Ok(_taskService.CreateCommentAction(userId, taskId, result));
                        case Models.SemanticsAction.FinishTask:
                            return Ok(_taskService.CreateFinishTaskAction(userId, taskId, result));
                        default:
                            throw new NotImplementedException();
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogInformation(ex, "Not recognized exception");
                    return Ok(new MakeTaskAction() { Error = "Сообщение не распознано" });
                }
            }
            return BadRequest("Audio file required");
        }

        /// <summary>
        /// Возвращет аудиосообщение
        /// </summary>
        /// <param name="guid">идентификатор аудиосообщения</param>
        [HttpGet("download/{guid}")]
        public async Task<IActionResult> Download(string guid)
        {
            string mp3file = Path.Combine(_appSettings.TempPath, $"{guid}.mp3");
            var bytes = await System.IO.File.ReadAllBytesAsync(mp3file);
            return File(bytes, "audio/mpeg");
        }
    }
}
