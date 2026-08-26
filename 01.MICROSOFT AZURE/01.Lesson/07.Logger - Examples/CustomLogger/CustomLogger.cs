using System;
using System.IO;

namespace CustomLoggerExample
{
    public class CustomLogger
    {
        private readonly string _baseDir;
        private static readonly object _lock = new object();

        public CustomLogger()
        {
            // Set base directory to the application's running folder
            _baseDir = AppDomain.CurrentDomain.BaseDirectory;
        }

        // Writes Info Logs
        public void LogInfo(string? moduleName, string fileName, string message)
        {
            WriteToLogFile("LOG", moduleName, fileName, message);
        }

        // Writes Logical Error Logs (e.g., failed validations, wrong credentials)
        public void LogError(string? moduleName, string fileName, string message)
        {
            WriteToLogFile("ERRORLOG", moduleName, fileName, message);
        }

        // Writes System Exception Logs (e.g., try-catch exceptions)
        public void LogException(string? moduleName, string fileName, Exception exception)
        {
            string errorMessage = $"{exception.Message}{Environment.NewLine}Stack Trace: {exception.StackTrace}";
            WriteToLogFile("EXCEPTIONLOG", moduleName, fileName, errorMessage);
        }

        // Core logging logic to build dynamic folder structures
        private void WriteToLogFile(string logType, string? moduleName, string fileName, string message)
        {
            string currentDate = DateTime.Now.ToString("yyyy-MM-dd");
            string targetFolder;

            // 1. Build directory path dynamically:
            // If moduleName is null: BASE_DIR / LOG_TYPE / CURRENT_DATE /
            // If moduleName has value: BASE_DIR / LOG_TYPE / MODULE_NAME / CURRENT_DATE /
            if (string.IsNullOrEmpty(moduleName))
            {
                targetFolder = Path.Combine(_baseDir, logType, currentDate);
            }
            else
            {
                targetFolder = Path.Combine(_baseDir, logType, moduleName, currentDate);
            }

            // 2. Ensure target directory exists
            if (!Directory.Exists(targetFolder))
            {
                Directory.CreateDirectory(targetFolder);
            }

            string targetFile = Path.Combine(targetFolder, fileName);
            string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            string formattedLog = $"[{timestamp}] [{logType}] {message}";



            // 4. Write to file safely (using lock for thread-safety)
            lock (_lock)
            {
                try
                {
                    File.AppendAllText(targetFile, formattedLog + Environment.NewLine);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[FATAL ERROR] Failed writing to file '{targetFile}': {ex.Message}");
                }
            }
        }
    }
}
