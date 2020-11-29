using Android.Media;
using atomapp.Models;
using atomapp.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace atomapp.ViewModels
{
    public class MakeRecordPageViewModel : BaseViewModel
    {
        private MediaRecorder recorder;
        private IApiService _apiService;

        private List<Tsk> _myTasks;
        public List<Tsk> MyTasks { get => _myTasks; set => SetProperty(ref _myTasks, value); }

        public MakeRecordPageViewModel(IApiService recordApiService)
        {
            StartRecordCommand = new Command(OnStartRecord);
            StopRecordCommand = new Command(OnStopRecord);
            RefreshCommand = new Command(OnRefresh);

            _apiService = recordApiService;

            Task.Run(() => LoadMyTasks()); 
        }

        public async Task LoadMyTasks()
        {
            MyTasks = (await _apiService.GetMyTasks(9)).ToList();
        }

        public async Task<PermissionStatus> CheckAndRequestLocationPermission()
        {
            var status = await Permissions.CheckStatusAsync<Permissions.Microphone>();

            if (status == PermissionStatus.Granted)
                return status;
            status = await Permissions.RequestAsync<Permissions.Microphone>();

            return status;
        }

        public async Task<PermissionStatus> CheckAndRequestStorageWritePermission()
        {
            var status = await Permissions.CheckStatusAsync<Permissions.StorageWrite>();

            if (status == PermissionStatus.Granted)
                return status;
            status = await Permissions.RequestAsync<Permissions.StorageWrite>();

            return status;
        }

        public Command StartRecordCommand { get; }
        public Command StopRecordCommand { get; }
        public Command RefreshCommand { get; }

        private async void OnStopRecord()
        {
            recorder.Stop();
            recorder.Reset();

            string filePath = Path.Combine(FileSystem.AppDataDirectory, "tempRecord.mp3");
            var bytes = File.ReadAllBytes(filePath);
            await _apiService.UploadRecordAsync(bytes);
        }

        private async void OnStartRecord()
        {
            var permissionStatus = await CheckAndRequestLocationPermission();
            if (permissionStatus != PermissionStatus.Granted)
                return;
            permissionStatus = await CheckAndRequestStorageWritePermission();
            if (permissionStatus != PermissionStatus.Granted)
                return;

            string filePath = Path.Combine(FileSystem.AppDataDirectory, "tempRecord.mp3");
            try
            {
                if (File.Exists(filePath))
                    File.Delete(filePath);
                if (recorder == null)
                    recorder = new MediaRecorder(); // Initial state.

                recorder.SetAudioSource(AudioSource.Mic);
                recorder.SetOutputFormat(OutputFormat.Mpeg4);
                recorder.SetAudioEncoder(AudioEncoder.Aac);
                // Initialized state.
                recorder.SetOutputFile(filePath);
                // DataSourceConfigured state.
                recorder.Prepare(); // Prepared state
                recorder.Start(); // Recording state.
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                Console.Out.WriteLine(ex.StackTrace);
            }
        }

        private async void OnRefresh()
        {
            await LoadMyTasks();
        }
    }
}
