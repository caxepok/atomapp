using atomapp.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace atomapp.Services
{
    public interface IApiService
    {
        Task<Record> UploadRecordAsync(byte[] data);
        Task<IEnumerable<Tsk>> GetMyTasks(int v);
    }

    public class ApiService : IApiService
    {
        public async Task<IEnumerable<Tsk>> GetMyTasks(int v)
        {
            HttpClient client = new HttpClient();
            string json = await client.GetStringAsync("https://atomspeech.germanywestcentral.cloudapp.azure.com/tasks/inbox?userId=9");

            return JsonConvert.DeserializeObject<IEnumerable<Tsk>>(json);
        }

        public async Task<Record> UploadRecordAsync(byte[] data)
        {
            HttpClient client = new HttpClient();
            ByteArrayContent bytes = new ByteArrayContent(data);
            MultipartFormDataContent multiContent = new MultipartFormDataContent();
            multiContent.Add(bytes, "file", "tempRecord.mp3");

            var response = await client.PostAsync("https://atomspeech.germanywestcentral.cloudapp.azure.com/record/upload?userId=9", multiContent);

            if (!response.IsSuccessStatusCode)
                throw new Exception("Ошибка отправки записи на сервер");

            return new Record();
        }
    }
}
