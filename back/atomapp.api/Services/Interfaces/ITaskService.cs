using atomapp.api.Models;
using atomapp.api.Models.API;
using atomapp.api.Models.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace atomapp.api.Services.Interfaces
{
    /// <summary>
    /// Интерфейс сервиса управления задачками
    /// </summary>
    public interface ITaskService
    {
        /// <summary>
        /// Создаёт запрос на создание задачи, который должен подтвердить\скорректировать пользователь для создания задачи
        /// </summary>
        MakeTaskAction CreateMakeTaskAction(long userId, long? parentId, RecognizedSemantics result);
        /// <summary>
        /// Создает запрос на завершение задачи, которые должен подтвредить\скорректировать пользтватель
        /// </summary>
        FinishTaskAction CreateFinishTaskAction(long userId, long? taskId, RecognizedSemantics result);
        /// <summary>
        /// Создает запрос на создание коментария, который должен подтвредить\скорреткировать пользователь для создания коммента к задачке
        /// </summary>
        CommentTaskAction CreateCommentAction(long userId, long? taskId, RecognizedSemantics result);
        /// <summary>
        /// Возвращает входящие задачки пользователя
        /// </summary>
        IEnumerable<Tsk> GetInbox(long userId, bool showFinished);
        /// <summary>
        /// Возвращает исходящие задачки пользователя
        /// </summary>
        IEnumerable<Tsk> GetOutbox(long userId, bool showFinished);
        /// <summary>
        /// Возвращает задачу по идентификатору
        /// </summary>
        Tsk GetById(long id);
        /// <summary>
        /// Создаёт задачу
        /// </summary>
        IEnumerable<Tsk> CreateTask(long userId, MakeTaskAction action);
        /// <summary>
        /// Завершает задачу с коментарием
        /// </summary>
        void FinishTask(long id, FinishTaskAction action);
        /// <summary>
        /// Создаёт коментарий к задаче
        /// </summary>
        TskComment AddTaskComment(long id, CommentTaskAction action);
    }
}
