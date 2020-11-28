using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace atomapp.api.Services.Interfaces
{
    /// <summary>
    /// Сервис преобразования mp3 в wav и opus в mp3
    /// </summary>
    public interface IAudioConverterService
    {
        Task ConvertMp3(string src, string dst);
        Task ConvertOpus(string src, string dst);
    }
}
