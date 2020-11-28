using atomapp.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace atomapp.Services
{
    public interface IApiService
    {
        Task<Record> UploadRecordAsync(byte[] data);
        void GetMyTasks(int v);
    }

    public class RecordApiService : IApiService
    {
        public void GetMyTasks(int v)
        {
            throw new NotImplementedException();
        }

        public async Task<Record> UploadRecordAsync(byte[] data)
        {
            HttpClient client = new HttpClient();
            ByteArrayContent bytes = new ByteArrayContent(data);
            MultipartFormDataContent multiContent = new MultipartFormDataContent();
            multiContent.Add(bytes, "file", "tempRecord.mp3");

            var response = await client.PostAsync("https://atomspeech.germanywestcentral.cloudapp.azure.com/record/upload?userId=9", multiContent);

            if(response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Record>(json);
            }

            throw new Exception("Ошибка отправки записи на сервер");
        }
    }
}
