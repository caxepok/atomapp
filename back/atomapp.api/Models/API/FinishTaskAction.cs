using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace atomapp.api.Models.API
{
    /// <summary>
    /// Запрос завершения задачи
    /// </summary>
    public class FinishTaskAction : TaskActionBase
    {
        public string Comment { get; set; }
        public long TaskId { get; internal set; }
    }
}
