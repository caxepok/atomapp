using System;
using System.Collections.Generic;
using System.Text;

namespace atomapp.Models
{
    /// <summary>
    /// Работник
    /// </summary>
    public class Worker
    {
        public long Id { get; set; }
        public long ParentId { get; set; }
        public string Login { get; set; }
        public string Name { get; set; }
    }
}
