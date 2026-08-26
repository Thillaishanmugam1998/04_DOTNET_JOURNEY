using System;
using System.IO;
using Serilog.Core;
using Serilog.Events;

namespace SerilogExample
{
    public class CustomFileSink : ILogEventSink
    {
        private readonly string _baseDir;
        private static readonly object _lock = new object();

        public CustomFileSink(string baseDir)
        {
            _baseDir = baseDir;
        }

        public void Emit(LogEvent logEvent)
        {
            // 1. Extract SourceContext (structured as "ModuleName.ClassName")
            string moduleName = "";
            string className = "UnknownService";

            if (logEvent.Properties.TryGetValue("SourceContext", out var contextValue) && contextValue is ScalarValue scalarContext)
            {
                string contextStr = scalarContext.Value?.ToString() ?? "";
                string[] parts = contextStr.Split('.');
                if (parts.Length >= 2)
                {
                    moduleName = parts[0];
                    className = parts[1];
                }
                else
                {
                    className = contextStr;
                }
            }

            // 2. Determine Log Type folder
            string logType = "LOG";
            if (logEvent.Exception != null)
            {
                logType = "EXCEPTIONLOG";
            }
            else if (logEvent.Level == LogEventLevel.Warning || logEvent.Level == LogEventLevel.Error || logEvent.Level == LogEventLevel.Fatal)
            {
                logType = "ERRORLOG";
            }

            // 3. Determine File Name
            string fileName = "log.txt";
            if (logEvent.Exception != null)
            {
                fileName = className + ".txt";
            }
            else if (logEvent.Properties.TryGetValue("FileName", out var fileValue) && fileValue is ScalarValue scalarFile)
            {
                fileName = scalarFile.Value?.ToString() ?? "log.txt";
            }

            // 4. Build Path dynamically
            string currentDate = DateTime.Now.ToString("yyyy-MM-dd");
            string targetFolder;

            if (string.IsNullOrEmpty(moduleName))
            {
                targetFolder = Path.Combine(_baseDir, logType, currentDate);
            }
            else
            {
                targetFolder = Path.Combine(_baseDir, logType, moduleName, currentDate);
            }

            // Ensure directory exists
            if (!Directory.Exists(targetFolder))
            {
                Directory.CreateDirectory(targetFolder);
            }

            string targetFile = Path.Combine(targetFolder, fileName);

            // 5. Format message
            string timestamp = logEvent.Timestamp.ToString("yyyy-MM-dd HH:mm:ss.fff");
            string message = logEvent.RenderMessage();
            string logEntry = $"[{timestamp}] [{logEvent.Level.ToString().ToUpper()}] {message}";

            if (logEvent.Exception != null)
            {
                logEntry += Environment.NewLine + $"Exception: {logEvent.Exception.Message}" + Environment.NewLine + $"Stack Trace: {logEvent.Exception.StackTrace}";
            }

            // 6. Write thread-safe
            lock (_lock)
            {
                try
                {
                    File.AppendAllText(targetFile, logEntry + Environment.NewLine);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[FATAL] Serilog CustomFileSink failed writing to file '{targetFile}': {ex.Message}");
                }
            }
        }
    }
}
