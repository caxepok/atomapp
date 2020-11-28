using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace atomapp.common.Enums
{
    /// <summary>
    /// Роль в иерархии оргуструтуры
    /// </summary>
    public enum WorkplaceRole
    {
        /// <summary>
        /// Начальник
        /// </summary>
        Boss = 0,
        /// <summary>
        /// Иненер технологи (менеджер)
        /// </summary>
        Manager = 1,
        /// <summary>
        /// Мастер
        /// </summary>
        Master = 2
    }
}
