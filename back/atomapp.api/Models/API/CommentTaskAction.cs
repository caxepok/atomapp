using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace atomapp.api.Models.API
{
    /// <summary>
    /// Запрос на создание коментария
    /// </summary>
    public class CommentTaskAction : TaskActionBase
    {
        public string Action => "comment";
        public string Comment { get; set; }
        public long TaskId { get; set; }
    }
}
