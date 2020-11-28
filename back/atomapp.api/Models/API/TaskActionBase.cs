using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace atomapp.api.Models.API
{
    /// <summary>
    /// Базовый класс операции с задачей
    /// </summary>
    public abstract class TaskActionBase
    {
        public long CreatorId { get; set; }
        public string AudioGuid { get; set; }
    }
}
