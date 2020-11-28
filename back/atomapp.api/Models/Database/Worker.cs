using atomapp.common.Enums;

namespace atomapp.api.Models.Database
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
        public WorkplaceRole Role { get; set; }
        public long WorkplaceId { get; set; }
    }
}
