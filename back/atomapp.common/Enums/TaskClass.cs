using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace atomapp.common.Enums
{
    /// <summary>
    /// Класс задачи
    /// </summary>
    public enum TaskClass
    {
        /// <summary>
        /// Ознакомиться
        /// </summary>
        Knowledge = 0,
        /// <summary>
        /// Измерить
        /// </summary>
        Measure = 1,
        /// <summary>
        /// Проверить
        /// </summary>
        Check = 2,
        /// <summary>
        /// Заменить
        /// </summary>
        Replace = 3,
        /// <summary>
        /// Поставить
        /// </summary>
        Install = 4
    }
}
