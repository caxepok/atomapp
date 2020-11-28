using atomapp.api.Models.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace atomapp.api.Services.Interfaces
{
    /// <summary>
    /// Сервис управления данными оргструктуры и работников
    /// </summary>
    public interface IWorkplaceService
    {
        Task<IEnumerable<Workplace>> GetWorkplaces();
        IEnumerable<Worker> GetWorkerOfWorkplace(long id);
        IEnumerable<Worker> GetWorkers();
        IEnumerable<Worker> GetSubordinates(long userId);
    }
}
