using atomapp.common.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace atomapp.api.Models.Database
{
    /// <summary>
    /// Экземпляр задачи
    /// </summary>
    public class Tsk
    {
        public long Id { get; set; }
        public long CreatorId { get; set; }
        public long ExecutorId { get; set; }
        public long? ParentId { get; set; }
        public string Description { get; set; }
        public DateTimeOffset Created { get; set; }
        public DateTimeOffset PlannedAt { get; set; }
        public DateTimeOffset? FinishedAt { get; set; }
        public bool IsFinished { get; set; }
        public int ExecutionPercent { get; set; }
        public TaskPriority Priority { get; set; }
        public string TaskObject { get; set; }
        public string AudioGuid { get; set; }
        public string FinishCommend { get; set; }
        public string FinishAudioGuid { get; set; }

        public virtual Worker Creator { get; set; }
        public virtual Worker Executor { get; set; }

        [NotMapped]
        public IEnumerable<Tsk> ChildTasks { get; set; }
        [NotMapped]
        public IEnumerable<TskComment> Comments { get; internal set; }
        public string Name { get; internal set; }
    }
}
