using atomapp.api.Infrastructure;
using atomapp.api.Models;
using atomapp.api.Services.Interfaces;
using atomapp.common.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Speech.Recognition;
using Microsoft.Speech.Recognition.SrgsGrammar;
using System;
using System.IO;
using System.Threading.Tasks;

namespace atomapp.api.Services
{
    /// <inheritdoc cref="ISemanticService"/>
    public class SemanticService : ISemanticService
    {
        private readonly ILogger _logger;
        private readonly AppSettings _appSettings;
        private readonly IAudioConverterService _mp3ConverterService;
        private readonly IPythonService _pythonService;
        private static bool gramarsCompiled;

        public SemanticService(ILogger<SemanticService> logger, IOptions<AppSettings> options, IAudioConverterService mp3ConverterService, IPythonService pythonService)
        {
            _logger = logger;
            _appSettings = options.Value;
            _mp3ConverterService = mp3ConverterService;
            _pythonService = pythonService;
        }

        public async Task<RecognizedSemantics> ProcessAsync(string audioGuid, byte[] bytes, bool isOpus)
        {
            string opusFile = Path.Combine(_appSettings.TempPath, $"{audioGuid}.opus");
            string mp3file = Path.Combine(_appSettings.TempPath, $"{audioGuid}.mp3");
            string wavFile = Path.Combine(_appSettings.TempPath, $"{audioGuid}.wav");

            if (isOpus)
            {
                await File.WriteAllBytesAsync(opusFile, bytes);
                await _mp3ConverterService.ConvertOpus(opusFile, mp3file);
            }
            else
            {
                await File.WriteAllBytesAsync(mp3file, bytes);
            }
            await _mp3ConverterService.ConvertMp3(mp3file, wavFile);

            // compile gramars
            if (!gramarsCompiled)
            {
                foreach (var file in Directory.EnumerateFiles(_appSettings.GrammarsPath, "*.xml"))
                {
                    string fileName = Path.GetFileNameWithoutExtension(file);
                    await using FileStream fs = new FileStream(Path.Combine(_appSettings.GrammarsPath, $"{fileName}.cfg"), FileMode.Create);
                    SrgsGrammarCompiler.Compile(file, fs);
                    await fs.FlushAsync();
                    fs.Close();
                }
                gramarsCompiled = true;
            }

            var semanticsTask = WaitInput(mp3file);
            var rawTask = ExtractRawText(wavFile);

            var semantics = await semanticsTask;
            var raw = await rawTask;

            RecognizedSemantics rs = new RecognizedSemantics() { Raw = raw };
            try
            {
                foreach (var s in semantics.Semantics)
                    _logger.LogInformation("k:{key},v:{value}", s.Key, s.Value.Value);

                rs = ParseSemantics(semantics.Semantics, raw);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Semantincs error {ex.Message}");
            }

            _logger.LogInformation("Semantics result: {0}", semantics.Text);
            _logger.LogInformation("Raw result: {0}", raw);

            rs.AudioGuid = audioGuid;

            return rs;
        }

        /// <summary>
        /// Вычленяет семантику сказанного для понимания типа действия и свойст задачи
        /// </summary>
        private RecognizedSemantics ParseSemantics(SemanticValue semantics, string raw)
        {
            if (!semantics.ContainsKey("action"))
                throw new ApplicationException("Фраза не распознана");

            RecognizedSemantics rs = new RecognizedSemantics() { Raw = raw };
            string action = (string)semantics["action"].Value;
            switch (action)
            {
                case "MakeTask":
                    rs.SemanticsAction = SemanticsAction.MakeTask;
                    if (semantics.ContainsKey("finishAt"))
                        rs.FinishDate = ExtractDate(semantics);
                    rs.RelativeTarget = ExtractExecutors(semantics);
                    rs.TaskClass = ExtractTaskClass(semantics);
                    rs.TaskPriority = ExtractPriority(semantics);
                    rs.TaskObject = ExtractTaskObject(semantics);
                    break;
                case "AddComment":
                    rs.SemanticsAction = SemanticsAction.AddComment;
                    rs.TaskId = ExtractTaskId(semantics);
                    rs.Comment = ExtractComment(rs);
                    break;
                case "FinishTask":
                    rs.SemanticsAction = SemanticsAction.FinishTask;
                    rs.TaskId = ExtractTaskId(semantics);
                    rs.Comment = ExtractComment(rs);
                    break;
                case "MoveTask":
                    rs.SemanticsAction = SemanticsAction.MoveTask;
                    break;
                default:
                    throw new ApplicationException("Фраза не распознана");
            }

            return rs;
        }

        /// <summary>
        /// Вычленяет текст коментария отдельно от текста задания 
        /// </summary>
        private string ExtractComment(RecognizedSemantics rs)
        {
            int idx = rs.Raw.IndexOf("комментарием");
            if (idx != -1)
            {
                idx += 12;
                return rs.Raw[idx..];
            }
            idx = rs.Raw.IndexOf("комментарий");
            if (idx != -1)
            {
                idx += 11;
                return rs.Raw[idx..];
            }
            idx = rs.Raw.IndexOf("комментариям");
            if (idx != -1)
            {
                idx += 12;
                return rs.Raw[idx..];
            }
            idx = rs.Raw.IndexOf("комментарии");
            if (idx != -1)
            {
                idx += 11;
                return rs.Raw[idx..];
            }
            return rs.Raw;
        }

        /// <summary>
        /// Распознаёт идентификтаор задачи
        /// </summary>
        private long? ExtractTaskId(SemanticValue semantics)
        {
            try
            {
                string relativeTarget = (string)semantics["taskId"].Value;
                return Int64.Parse(relativeTarget);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to recognize taskId");
                return null;
            }
        }

        /// <summary>
        /// Класс задачи (что сделать)
        /// </summary>
        private TaskClass ExtractTaskClass(SemanticValue semantics)
        {
            try
            {
                string relativeTarget = (string)semantics["taskClass"].Value;
                switch (relativeTarget)
                {
                    case "Knowledge":
                        return TaskClass.Knowledge;
                    case "Check":
                        return TaskClass.Check;
                    case "Measure":
                        return TaskClass.Measure;
                    case "Replace":
                        return TaskClass.Replace;
                    case "Install":
                        return TaskClass.Install;
                    default:
                        throw new ApplicationException("Ошибка распознавания типа задачи");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to regonize task class");
                throw new ApplicationException("Ошибка распознавания типа задачи", ex);
            }
        }

        /// <summary>
        /// Объект задачи (с чем сделать)
        /// </summary>
        private string ExtractTaskObject(SemanticValue semantics)
        {
            try
            {
                string taskObject = (string)semantics["taskObject"].Value;
                return taskObject;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to regonize task object");
                return String.Empty;
            }
        }

        /// <summary>
        /// Распознаёт исполнителей
        /// </summary>
        private Executors ExtractExecutors(SemanticValue semantics)
        {
            try
            {
                string executors = (string)semantics["target"].Value;
                switch (executors)
                {
                    case "AllSub":
                        return Executors.AllSub;
                    default:
                        return Executors.None;
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to regonize executor");
                return Executors.None;
            }
        }

        /// <summary>
        /// Распознаёт сказанную дату
        /// </summary>
        private DateTimeOffset ExtractDate(SemanticValue semantics)
        {
            try
            {
                int day = Int32.Parse((string)semantics["date"]["day"].Value);
                int month = Int32.Parse((string)semantics["date"]["month"].Value);

                var now = DateTimeOffset.Now;
                var plannedDate = new DateTimeOffset(now.Year, month, day, 0, 0, 0, now.Offset);
                // если срок задним числом
                // значит нам нужен следующий год
                // актуально в конце года :)
                if (plannedDate < now)
                    plannedDate = plannedDate.AddYears(1);
                return plannedDate;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to recognize date");
                throw new ApplicationException("Ошибка распознавания даты", ex);
            }
        }

        /// <summary>
        /// Распознаёт приоритет задачи
        /// </summary>
        private TaskPriority ExtractPriority(SemanticValue semantics)
        {
            try
            {
                string priority = (string)semantics["priority"].Value;
                switch (priority)
                {
                    case "Low":
                        return TaskPriority.Low;
                    case "Normal":
                        return TaskPriority.Medium;
                    case "Hight":
                        return TaskPriority.High;
                    default:
                        return TaskPriority.Medium;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to regonize task priority");
                return TaskPriority.Medium;
            }
        }

        /// <summary>
        /// Обёртка в Task над распознавалкой
        /// </summary>
        private Task<RecognitionResult> WaitInput(string mp3file)
        {
            var tcs = new TaskCompletionSource<RecognitionResult>();

            Grammar grCatalogue = new Grammar(Path.Combine(_appSettings.GrammarsPath, "gramar-catalogue.cfg"), "catalogue");
            Grammar grTasks = new Grammar(Path.Combine(_appSettings.GrammarsPath, "gramar-tasks.cfg"), "makeTask");
            SpeechRecognitionEngine sre = new SpeechRecognitionEngine(new System.Globalization.CultureInfo("ru-RU"));
            //sre.LoadGrammar(grCatalogue);
            sre.LoadGrammar(grTasks);
            sre.SetInputToWaveFile(mp3file);
            sre.SpeechRecognized += (o, args) => tcs.SetResult(args.Result);
            sre.SpeechRecognitionRejected += (o, args) => tcs.TrySetException(new InvalidOperationException("Not recognized"));
            sre.RecognizeAsync(RecognizeMode.Single);
            return tcs.Task;
        }

        /// <summary>
        /// Распознаёт сырой текст сказанного
        /// </summary>
        private Task<string> ExtractRawText(string wavFile)
        {
            string raw = _pythonService.Run(Path.Combine(_appSettings.VoskPath, "test_simple.py"), wavFile);
            return Task.FromResult(raw);
        }
    }
}
