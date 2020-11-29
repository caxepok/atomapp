using atomapp.api.Models.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace atomapp.api.Models.API
{
    /// <summary>
    /// Сводная информация по сотрудникам
    /// </summary>
    public class Summary
    {
        public IEnumerable<SummaryItem> Data;
    }

    public class SummaryItem
    {
        public Worker Worker { get; set; }
        public int Active { get; set; }
        public int Completed { get; set; }
        public int Outdate { get; set; }
    }
}
