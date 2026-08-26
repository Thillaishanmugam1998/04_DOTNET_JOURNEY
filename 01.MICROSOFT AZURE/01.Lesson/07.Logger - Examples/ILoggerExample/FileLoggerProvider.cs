using System;
using System.IO;
using Microsoft.Extensions.Logging;

namespace ILoggerExample
{
    // 1. Custom ILogger Implementation
    public class FileLogger : ILogger
    {
        private readonly string _categoryName;
        private readonly string _baseDir;
        private static readonly object _lock = new object();

        public FileLogger(string categoryName, string baseDir)
        {
            _categoryName = categoryName;
            _baseDir = baseDir;
        }

        // Scope is not implemented
        public IDisposable? BeginScope<TState>(TState state) where TState : notnull => null;

        public bool IsEnabled(LogLevel logLevel) => true;

        public void Log<TState>(
            LogLevel logLevel, 
            EventId eventId, 
            TState state, 
            Exception? exception, 
            Func<TState, Exception?, string> formatter)
        {
            if (!IsEnabled(logLevel)) return;

            // 1. Extract ModuleName and ClassName from categoryName
            // Category name structured as: "ModuleName.ClassName" (e.g. "Authentication.LoginService")
            string moduleName = "";
            string className = "UnknownService";

            if (!string.IsNullOrEmpty(_categoryName))
            {
                string[] parts = _categoryName.Split('.');
                if (parts.Length >= 2)
                {
                    moduleName = parts[0];
                    className = parts[1];
                }
                else
                {
                    className = _categoryName;
                }
            }

            // 2. Determine Log Type folder
            string logType = "LOG";
            if (exception != null)
            {
                logType = "EXCEPTIONLOG";
            }
            else if (logLevel == LogLevel.Warning || logLevel == LogLevel.Error)
            {
                logType = "ERRORLOG";
            }

            // 3. Determine File Name
            // If exception is logged, always write to the class-specific text file
            // Otherwise, read eventId.Name (defaulting to "log.txt")
            string fileName = "log.txt";
            if (exception != null)
            {
                fileName = className + ".txt";
            }
            else if (!string.IsNullOrEmpty(eventId.Name))
            {
                fileName = eventId.Name;
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

            // 5. Format the message
            string message = formatter(state, exception);
            string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            string logEntry = $"[{timestamp}] [{logLevel.ToString().ToUpper()}] {message}";

            if (exception != null)
            {
                logEntry += Environment.NewLine + $"Exception: {exception.Message}" + Environment.NewLine + $"Stack Trace: {exception.StackTrace}";
            }

            string targetFile = Path.Combine(targetFolder, fileName);

            // 6. Write thread-safe
            lock (_lock)
            {
                try
                {
                    File.AppendAllText(targetFile, logEntry + Environment.NewLine);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[FATAL] FileLogger failed writing to file '{targetFile}': {ex.Message}");
                }
            }
        }
    }

    // 2. Custom ILoggerProvider Implementation
    public class FileLoggerProvider : ILoggerProvider
    {
        private readonly string _baseDir;

        public FileLoggerProvider(string baseDir)
        {
            _baseDir = baseDir;
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new FileLogger(categoryName, _baseDir);
        }

        public void Dispose() { }
    }

    // 3. Extension Method
    public static class FileLoggerExtensions
    {
        public static ILoggingBuilder AddFile(this ILoggingBuilder builder, string baseDir)
        {
            builder.AddProvider(new FileLoggerProvider(baseDir));
            return builder;
        }
    }
}
