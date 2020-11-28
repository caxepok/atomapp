using atomapp.common.Enums;
using System;

namespace atomapp.api.Models.Database
{
    /// <summary>
    /// Шаблон задачи, которую можно создать
    /// </summary>
    public class TskTemplate
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public TaskClass Class { get; set; }

        /// <summary>
        /// Периодичность создания экземпляров задач из шаблона (если класс задачи периодичный)
        /// </summary>
        public TimeSpan Period { get; set; }
    }
}
