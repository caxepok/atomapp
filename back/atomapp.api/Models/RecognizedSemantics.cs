using atomapp.common.Enums;
using System;

namespace atomapp.api.Models
{
    /// <summary>
    /// Распознанные семантические данные
    /// </summary>
    public class RecognizedSemantics
    {
        /// <summary>
        /// Действие, которое хочет выполнить пользователь
        /// </summary>
        public SemanticsAction SemanticsAction { get; set; }
        /// <summary>
        /// Цель, относительно пользователя
        /// </summary>
        public Executors RelativeTarget { get; set; }
        /// <summary>
        /// Абсолютная цель
        /// </summary>
        public string AbsoluteTarget { get; set; }
        /// <summary>
        /// Номер задачи
        /// </summary>
        public long? TaskId { get; set; }
        /// <summary>
        /// Дата окончания задачи
        /// </summary>
        public DateTimeOffset FinishDate { get; set; }
        /// <summary>
        /// Сырой текст
        /// </summary>
        public string Raw { get; set; }
        /// <summary>
        /// Класс задачи
        /// </summary>
        public TaskClass TaskClass { get; set; }
        /// <summary>
        /// Приоритет задачки
        /// </summary>
        public TaskPriority TaskPriority { get; set; }
        /// <summary>
        /// Цель-объект задачки
        /// </summary>
        public string TaskObject { get; set; }
        /// <summary>
        /// Guid аудиозаписи
        /// </summary>
        public string AudioGuid { get; set; }
        /// <summary>
        /// Комментарий
        /// </summary>
        public string Comment { get; set; }
    }

    /// <summary>
    /// Исполнители
    /// </summary>
    public enum Executors
    {
        None = 0,
        AllSub = 1
    }

    /// <summary>
    /// Действие, которое хочет выполнить пользователь
    /// </summary>
    public enum SemanticsAction
    {
        /// <summary>
        /// Создать задачу
        /// </summary>
        MakeTask = 0,
        /// <summary>
        /// Закрыть задачу
        /// </summary>
        FinishTask = 1,
        /// <summary>
        /// Добавление коментария
        /// </summary>
        AddComment = 2,
        /// <summary>
        /// Изменение даты окончания задачи
        /// </summary>
        RescheduleTask = 3,
        /// <summary>
        /// Переместить задачу
        /// </summary>
        MoveTask = 4,
        /// <summary>
        /// Задать процент выполнения задачи
        /// </summary>
        SetTaskExecutionPercent = 5

    }
}
