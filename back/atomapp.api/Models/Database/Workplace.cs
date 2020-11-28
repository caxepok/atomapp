using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace atomapp.api.Models.Database
{
    /// <summary>
    /// Рабочее место
    /// </summary>
    public class Workplace
    {
        public long Id { get; set; }
        public long? ParentId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        [NotMapped]
        public Workplace Parent { get; set; }
        [NotMapped]
        public IEnumerable<Workplace> Children { get; set; }
    }
}
