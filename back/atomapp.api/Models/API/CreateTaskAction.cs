using atomapp.api.Models.Database;
using atomapp.common.Enums;
using System;
using System.Collections.Generic;

namespace atomapp.api.Models.API
{
    /// <summary>
    /// Запрос на создание задачи
    /// </summary>
    public class MakeTaskAction : TaskActionBase
    {
        public string Action => "task";
        public IEnumerable<Worker> ExecutorWorkers { get; set; }
        public DateTimeOffset PlannedAt { get; set; }
        public TaskPriority? Priority { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public string TaskObject { get; set; }
        public long? ParentId { get; set; }
        public string Error { get; set; }
        public TaskClass TaskClass { get; set; }
    }
}
