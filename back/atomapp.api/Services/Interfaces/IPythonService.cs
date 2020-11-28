using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace atomapp.api.Services.Interfaces
{
    /// <summary>
    /// Сервис для запуска python`а
    /// </summary>
    public interface IPythonService
    {
        /// <summary>
        /// Выполняет python скрипт
        /// </summary>
        /// <param name="cmd">название скрипта</param>
        /// <param name="args">аргументы запуска</param>
        /// <returns>результат выполнения</returns>
        string Run(string cmd, string args);
    }
}
