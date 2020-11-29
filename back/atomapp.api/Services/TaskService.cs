using atomapp.api.Infrastructure;
using atomapp.api.Models;
using atomapp.api.Models.API;
using atomapp.api.Models.Database;
using atomapp.api.Services.Interfaces;
using atomapp.common.Enums;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace atomapp.api.Services
{
    /// <inheritdoc cref="ITaskService"/>
    public class TaskService : ITaskService
    {
        private readonly ILogger _logger;
        private readonly IWorkplaceService _workplaceService;
        private readonly AtomDBContext _db;

        public TaskService(ILogger<TaskService> logger, AtomDBContext db, IWorkplaceService workplaceService)
        {
            _logger = logger;
            _db = db;
            _workplaceService = workplaceService;
        }

        // -- get
        public Tsk GetById(long id)
        {
            var task = _db.Tsks.SingleOrDefault(_ => _.Id == id);
            if (task != null)
            {
                task.ChildTasks = _db.Tsks.Where(_ => _.ParentId == task.Id).ToList();
                task.Comments = _db.TskComments.Where(_ => _.TskId == task.Id).ToList();
            }
            return task;
        }

        public IEnumerable<Tsk> GetInbox(long userId, bool showFinished)
        {
            var tasks = _db.Tsks.Where(_ => _.ExecutorId == userId);
            if (!showFinished)
                tasks = tasks.Where(_ => _.IsFinished == false);

            return tasks.OrderBy(_ => _.PlannedAt);
        }

        public IEnumerable<Tsk> GetOutbox(long userId, bool showFinished)
        {
            var tasks = _db.Tsks.Where(_ => _.CreatorId == userId);
            if (!showFinished)
                tasks = tasks.Where(_ => _.IsFinished == false);

            return tasks.OrderBy(_ => _.PlannedAt);
        }

        // -- create 
        public MakeTaskAction CreateMakeTaskAction(long userId, long? parentId, RecognizedSemantics result)
        {
            // тут мы собираем всех своих подчинённых в кучку
            var subordinates = _workplaceService.GetSubordinates(userId);

            MakeTaskAction a = new MakeTaskAction()
            {
                AudioGuid = result.AudioGuid,
                CreatorId = userId,
                Priority = result.TaskPriority,
                PlannedAt = result.FinishDate,
                ParentId = parentId,
                TaskObject = result.TaskObject,
                TaskClass = result.TaskClass
            };

            a.Name = GenerateTaskName(a);

            if (result.RelativeTarget == Executors.AllSub)
                a.ExecutorWorkers = subordinates;

            int idx = result.Raw.IndexOf("описание") + 9;
            a.Description = result.Raw[idx..];

            return a;
        }
        public IEnumerable<Tsk> CreateTask(long userId, MakeTaskAction action)
        {
            var now = DateTimeOffset.Now;
            List<Tsk> tasks = new List<Tsk>();
            // на каждого сотрудника в списке создаём задачку
            foreach (var worker in action.ExecutorWorkers)
            {
                Tsk tsk = new Tsk()
                {
                    Name = action.Name,
                    AudioGuid = action.AudioGuid,
                    CreatorId = userId,
                    Created = now,
                    Description = action.Description,
                    ExecutorId = worker.Id,
                    PlannedAt = action.PlannedAt,
                    Priority = action.Priority ?? TaskPriority.Medium,
                    TaskObject = action.TaskObject,
                    ParentId = action.ParentId
                };
                tasks.Add(tsk);
            }
            _db.AddRange(tasks);
            _db.SaveChanges();

            return tasks;
        }

        // -- finish
        public FinishTaskAction CreateFinishTaskAction(long userId, long? taskId, RecognizedSemantics result)
        {
            if (taskId == null && result.TaskId == null)
                throw new ApplicationException("Номер задачи для закрытия не распознан");

            _logger.LogInformation($"Creating finish task: {userId}, {taskId}");

            var fta = new FinishTaskAction()
            {
                TaskId = taskId ?? result.TaskId.Value,
                CreatorId = userId,
                AudioGuid = result.AudioGuid,
                Comment = result.Comment,
            };

            FinishTask(fta.TaskId, fta);
            return fta;
        }
        public void FinishTask(long id, FinishTaskAction action)
        {
            _logger.LogInformation($"Finishing task: {id}, {action}");

            var task = _db.Tsks.SingleOrDefault(_ => _.Id == id);
            if (task == null)
                return;
            task.IsFinished = true;
            task.FinishComment = action.Comment;
            if (String.IsNullOrEmpty(action.Comment))
            {
                task.FinishAudioGuid = action.AudioGuid;
            }
            task.ExecutionPercent = 100;
            task.FinishedAt = DateTimeOffset.Now;
            _db.SaveChanges();
        }

        // -- comment
        public CommentTaskAction CreateCommentAction(long userId, long? taskId, RecognizedSemantics result)
        {
            if (taskId == null && result.TaskId == null)
                throw new ApplicationException("Номер задачи для коментирования не распознан");

            _logger.LogInformation($"Creating comment: {userId}, {taskId}");

            var worker = _db.Workers.Single(_ => _.Id == userId);
            var cta = new CommentTaskAction()
            {
                CreatorId = userId,
                Comment = result.Comment,
                AudioGuid = result.AudioGuid,
                TaskId = taskId ?? result.TaskId.Value
            };
            return cta;
        }
        public TskComment AddTaskComment(long taskId, CommentTaskAction action)
        {
            _logger.LogInformation("Adding comment to task: {taskId}, {@action}", taskId, action);
            var worker = _db.Workers.SingleOrDefault(_ => _.Id == action.CreatorId);

            TskComment comment = new TskComment()
            {
                AddedAt = DateTimeOffset.Now,
                AudioGuid = action.AudioGuid,
                CreatorId = action.CreatorId,
                CreatorName = worker.Name,
                Text = action.Comment,
                TskId = taskId
            };
            _db.TskComments.Add(comment);
            _db.SaveChanges();

            return comment;
        }

        /// <summary>
        /// Генерит название задачи и действия и объекта действия
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        private string GenerateTaskName(MakeTaskAction a) => a.TaskClass switch
        {
            TaskClass.Knowledge => $"Ознакомиться: {a.TaskObject}",
            TaskClass.Check => $"Проверить: {a.TaskObject}",
            TaskClass.Measure => $"Измерить: {a.TaskObject}",
            TaskClass.Replace => $"Заменить: {a.TaskObject}",
            TaskClass.Install => $"Поставить: {a.TaskObject}",
            _ => a.TaskObject,
        };
    }
}
