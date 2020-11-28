using atomapp.api.Services.Interfaces;
using NAudio.Wave;
using System.Threading.Tasks;

namespace atomapp.api.Services
{
    /// <inheritdoc cref="IAudioConverterService"/>
    public class AudioConverterService : IAudioConverterService
    {
        public async Task ConvertMp3(string src, string dst)
        {
            await using var waveFileReader = new Mp3FileReader(src);
            var outFormat = new WaveFormat(waveFileReader.WaveFormat.SampleRate, 1);
            using var resampler = new MediaFoundationResampler(waveFileReader, outFormat);
            WaveFileWriter.CreateWaveFile(dst, resampler);
        }

        public async Task ConvertOpus(string src, string dst)
        {
            MediaFoundationReader reader = new MediaFoundationReader(src);
            using var writer = new NAudio.Lame.LameMP3FileWriter(dst, reader.WaveFormat, NAudio.Lame.LAMEPreset.STANDARD);
            await reader.CopyToAsync(writer);
        }
    }
}
