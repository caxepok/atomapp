using atomapp.api.Infrastructure;
using atomapp.api.Models.API;
using atomapp.api.Models.Database;
using atomapp.api.Services.Interfaces;
using atomapp.common.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace atomapp.api.Services
{
    /// <inheritdoc cref="IWorkplaceService"/>
    public class WorkplaceService : IWorkplaceService
    {
        private readonly ILogger _logger;
        private readonly AtomDBContext _db;

        public WorkplaceService(ILogger<WorkplaceService> logger, AtomDBContext db)
        {
            _logger = logger;
            _db = db;

            // вселенная, прости меня за такое (:
            FillDatabaseWithPredefinedValues().Wait();
        }

        public IEnumerable<Worker> GetWorkerOfWorkplace(long id)
        {
            return _db.Workers.Where(_ => _.WorkplaceId == id);
        }

        public IEnumerable<Worker> GetWorkers()
        {
            return _db.Workers;
        }

        public IEnumerable<Worker> GetSubordinates(long userId)
        {
            // тут мы собираем всех своих подчинённых в кучку
            var user = _db.Workers.Single(_ => _.Id == userId);
            var subRole = GetSubRole(user);

            var myWorkplace = _db.Workplaces.Single(_ => _.Id == user.WorkplaceId);
            var subWorkplaces = _db.Workplaces.Where(_ => _.ParentId == myWorkplace.Id).ToList();
            List<Worker> mySubs = new List<Worker>();
            foreach (var sub in subWorkplaces)
                mySubs.AddRange(_db.Workers.Where(_ => _.WorkplaceId == sub.Id && _.Role == subRole));

            return mySubs;
        }

        public Summary GetSummary(long userId)
        {
            var now = DateTimeOffset.Now;
            var subs = GetSubordinates(userId);
            List<SummaryItem> data = new List<SummaryItem>(subs.Count());
            foreach (var sub in subs)
            {
                var tasks = _db.Tsks.Where(_ => _.ExecutorId == sub.Id).ToList();
                var summaryItem = new SummaryItem()
                {
                    Worker = sub,
                    Active = tasks.Count(_ => _.IsFinished == false),
                    Completed = tasks.Count(_ => _.IsFinished == true),
                    Outdate = tasks.Count(_ => _.IsFinished == false && _.PlannedAt < now)
                };
                data.Add(summaryItem);
            }
            return new Summary() { Data = data };
        }

        public IEnumerable<Workplace> GetWorkplaces()
        {
            var workplaces = _db.Workplaces.Where(_ => _.ParentId == null).OrderBy(_ => _.Id).ToList();
            foreach (var workplace in workplaces)
                GetChildren(workplace);

            return workplaces;
        }

        /// <summary>
        /// Наполняет дочерними записями родительскую запись
        /// </summary>
        private void GetChildren(Workplace parent)
        {
            parent.Children = _db.Workplaces.Where(_ => _.ParentId == parent.Id).OrderBy(_ => _.Id).ToList();
            foreach (var workplace in parent.Children)
                GetChildren(workplace);
        }

        /// <summary>
        /// Возвращает значение низлежащей роли пользователя
        /// </summary>
        private WorkplaceRole GetSubRole(Worker user)
        {
            switch (user.Role)
            {
                case WorkplaceRole.Boss:
                    return WorkplaceRole.Manager;
                case WorkplaceRole.Manager:
                    return WorkplaceRole.Master;
                default:
                    throw new InvalidOperationException("Master have no subroles");
            }
        }

        /// <summary>
        /// Заполнение базы оргструктурой и пользователями
        /// В реале это должно подтягиваться из систем предприятия
        /// </summary>
        private async Task FillDatabaseWithPredefinedValues()
        {
            if (await _db.Workplaces.CountAsync() == 0)
            {
                var ceh1 = new Workplace() { Name = "Цех очистки плутония" };
                var ceh2 = new Workplace() { Name = "Металлургический цех" };
                var ceh3 = new Workplace() { Name = "Цех нарезки болтов" };

                _db.Workplaces.Add(ceh1);
                _db.Workplaces.Add(ceh2);
                _db.Workplaces.Add(ceh3);
                await _db.SaveChangesAsync();

                var korpus1с1 = new Workplace() { Name = "Корпус подготовки сырья", ParentId = ceh1.Id };
                var korpus2с1 = new Workplace() { Name = "Плавильный корпус", ParentId = ceh1.Id };
                var korpus1с2 = new Workplace() { Name = "Корпус деионизации", ParentId = ceh1.Id };
                var korpus2с2 = new Workplace() { Name = "Корпус компрессии", ParentId = ceh1.Id };
                _db.Workplaces.Add(korpus1с1);
                _db.Workplaces.Add(korpus2с1);
                _db.Workplaces.Add(korpus1с2);
                _db.Workplaces.Add(korpus2с2);
                await _db.SaveChangesAsync();

                var uchastok1k1c1 = new Workplace() { Name = "Южный участок", ParentId = korpus1с1.Id };
                var uchastok2k1c1 = new Workplace() { Name = "Северный участок", ParentId = korpus1с1.Id };
                var uchastok3k1c1 = new Workplace() { Name = "Западный участок", ParentId = korpus1с1.Id };
                var uchastok4k1c1 = new Workplace() { Name = "Восточный участок", ParentId = korpus1с1.Id };

                var uchastok1k2c1 = new Workplace() { Name = "Красный участок", ParentId = korpus2с1.Id };
                var uchastok2k2c1 = new Workplace() { Name = "Зелёный участок", ParentId = korpus2с1.Id };
                var uchastok3k2c1 = new Workplace() { Name = "Синий участок", ParentId = korpus2с1.Id };
                var uchastok4k2c1 = new Workplace() { Name = "Жёлтый участок", ParentId = korpus2с1.Id };

                _db.Workplaces.Add(uchastok1k1c1);
                _db.Workplaces.Add(uchastok2k1c1);
                _db.Workplaces.Add(uchastok3k1c1);
                _db.Workplaces.Add(uchastok4k1c1);
                await _db.SaveChangesAsync();

                _db.Workers.Add(new Worker() { WorkplaceId = ceh1.Id, Login = "antuan", Name = "Антуан Беккерель", Role = WorkplaceRole.Boss });

                _db.Workers.Add(new Worker() { WorkplaceId = korpus1с1.Id, Login = "mkuri", Name = "Мария Кюри", Role = WorkplaceRole.Manager });
                _db.Workers.Add(new Worker() { WorkplaceId = korpus1с1.Id, Login = "pkuri", Name = "Пьер Кюри", Role = WorkplaceRole.Manager });
                _db.Workers.Add(new Worker() { WorkplaceId = korpus2с1.Id, Login = "mihail", Name = "Михаил Ломоносов", Role = WorkplaceRole.Manager });
                _db.Workers.Add(new Worker() { WorkplaceId = korpus2с1.Id, Login = "oganesyan", Name = "Юрий Оганесян", Role = WorkplaceRole.Manager });

                _db.Workers.Add(new Worker() { WorkplaceId = uchastok1k1c1.Id, Login = "lippman", Name = "Габриэль Липпман", Role = WorkplaceRole.Master });
                _db.Workers.Add(new Worker() { WorkplaceId = uchastok2k1c1.Id, Login = "gustav", Name = "Густав Кирхгоф", Role = WorkplaceRole.Master });
                _db.Workers.Add(new Worker() { WorkplaceId = uchastok3k1c1.Id, Login = "einstein", Name = "Альберт Эйнштейн", Role = WorkplaceRole.Master });
                _db.Workers.Add(new Worker() { WorkplaceId = uchastok4k1c1.Id, Login = "nbor", Name = "Бор Нильс", Role = WorkplaceRole.Master });

                _db.Workers.Add(new Worker() { WorkplaceId = uchastok1k2c1.Id, Login = "sitdikov", Name = "Ситдиков Денис", Role = WorkplaceRole.Master });
                _db.Workers.Add(new Worker() { WorkplaceId = uchastok2k2c1.Id, Login = "batretdinov", Name = "Батретдинов Ринат", Role = WorkplaceRole.Master });
                _db.Workers.Add(new Worker() { WorkplaceId = uchastok3k2c1.Id, Login = "hai", Name = "Хайдаров Ильназ", Role = WorkplaceRole.Master });
                _db.Workers.Add(new Worker() { WorkplaceId = uchastok4k2c1.Id, Login = "ruslan", Name = "Нугуманов Руслан", Role = WorkplaceRole.Master });

                await _db.SaveChangesAsync();
            }
        }
    }
}
