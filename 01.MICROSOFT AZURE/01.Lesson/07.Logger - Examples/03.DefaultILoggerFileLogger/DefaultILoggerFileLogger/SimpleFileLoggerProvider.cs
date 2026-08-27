using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DefaultILoggerFileLogger
{
    [ProviderAlias("SimpleFileLogger")]
    public class SimpleFileLoggerProvider : ILoggerProvider
    {
        private readonly object _lockObj = new object();
        private readonly string _basePath;

        public SimpleFileLoggerProvider()
        {
            // Application run aagura root directory
            _basePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Log");
            if (!Directory.Exists(_basePath))
            {
                Directory.CreateDirectory(_basePath);
            }
        }

        public ILogger CreateLogger(string categoryName)
        {
            // categoryName la full namespace + class name varum (e.g., "MyApp.UserService")
            return new SimpleFileLogger(categoryName, this);
        }

        // Thread-safe ah file la eludhura main method
        public void WriteLog(string categoryName, LogLevel logLevel, string message)
        {
            lock (_lockObj)
            {
                try
                {
                    // 1. Category name la irundhu Class name mathum extract panrom (e.g., "UserService")
                    string className = categoryName.Contains('.')
                        ? categoryName.Split('.').Last()
                        : categoryName;

                    // 2. Date folder create panrom (e.g., Log/2026-08-27)
                    string dateFolder = DateTime.Now.ToString("yyyy-MM-dd");
                    string fullFolderPath = Path.Combine(_basePath, dateFolder);

                    if (!Directory.Exists(fullFolderPath))
                    {
                        Directory.CreateDirectory(fullFolderPath);
                    }

                    // 3. Final file path (e.g., Log/2026-08-27/UserService.txt)
                    string logFilePath = Path.Combine(fullFolderPath, $"{className}.txt");

                    // 4. Log format
                    string logEntry = $"[{DateTime.Now:HH:mm:ss.fff}] [{logLevel}] {message}{Environment.NewLine}";

                    File.AppendAllText(logFilePath, logEntry);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"File Log Error: {ex.Message}");
                }
            }
        }

        public void Dispose() { }
    }
}
