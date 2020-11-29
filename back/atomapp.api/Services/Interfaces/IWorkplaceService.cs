using atomapp.api.Models.API;
using atomapp.api.Models.Database;
using System.Collections.Generic;

namespace atomapp.api.Services.Interfaces
{
    /// <summary>
    /// Сервис управления данными оргструктуры и работников
    /// </summary>
    public interface IWorkplaceService
    {
        /// <summary>
        /// Возвращает иерархию оргструктуры
        /// </summary>
        IEnumerable<Workplace> GetWorkplaces();
        /// <summary>
        /// Возвращает всех сотрудников
        /// </summary>
        IEnumerable<Worker> GetWorkers();
        /// <summary>
        /// Возвращает сотрудников 
        /// </summary>
        IEnumerable<Worker> GetWorkerOfWorkplace(long userId);
        /// <summary>
        /// Возвращает подчинённых сотрудника
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        IEnumerable<Worker> GetSubordinates(long userId);
        /// <summary>
        /// Возвращает сводную информацию по задачм подчинённых сотрудников
        /// </summary>
        Summary GetSummary(long userId);
    }
}
