using atomapp.api.Models;
using System.Threading.Tasks;

namespace atomapp.api.Services.Interfaces
{
    /// <summary>
    /// Сервис распознавания семантики
    /// </summary>
    public interface ISemanticService
    {
        /// <summary>
        /// Обработка аудиосообщения
        /// </summary>
        /// <param name="guid">guid аудиосообщения</param>
        /// <param name="bytes">данные</param>
        /// <param name="isOpus">true - кодек Opus, false - mp3</param>
        /// <returns>распознанная семантика + сырой текст</returns>
        Task<RecognizedSemantics> ProcessAsync(string guid, byte[] bytes, bool isOpus);
    }
}
