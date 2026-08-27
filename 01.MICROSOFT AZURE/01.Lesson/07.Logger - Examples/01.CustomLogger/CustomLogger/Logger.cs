using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CustomLogger
{
    public class Logger
    {
        private static readonly object _lockObj = new object();
        private static string _basePath;

        // Application run aagura root folder ah initialize panrom
        public static void Initialize()
        {
            // AppDomain.CurrentDomain.BaseDirectory -> App run aagura main folder path
            _basePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Log");

            if (!Directory.Exists(_basePath))
            {
                Directory.CreateDirectory(_basePath);
            }
        }

        // [CallerFilePath] -> Evanga irundhu call varudho adha automatic ah pudichukum
        public static void WriteLog(string message, [CallerFilePath] string callerFilePath = "")
        {
            lock (_lockObj)
            {
                try
                {
                    // 1. File path la irundhu Class/File name ah mathum extract panrom (e.g., UserService)
                    string className = Path.GetFileNameWithoutExtension(callerFilePath);
                    if (string.IsNullOrEmpty(className))
                        className = "UnknownClass"; // Fallback

                    // 2. Current Date folder (e.g., 2026-08-27)
                    string dateFolder = DateTime.Now.ToString("yyyy-MM-dd");
                    string fullFolderPath = Path.Combine(_basePath, dateFolder);

                    // Date folder illana create panrom
                    if (!Directory.Exists(fullFolderPath))
                    {
                        Directory.CreateDirectory(fullFolderPath);
                    }

                    // 3. Final File Path: Log/2026-08-27/UserService.txt
                    string logFilePath = Path.Combine(fullFolderPath, $"{className}.txt");

                    // Folder already date ah hold pannirukrathu, inside time mathum podhal podhum
                    string logEntry = $"[{DateTime.Now:HH:mm:ss.fff}] {message}{Environment.NewLine}";

                    File.AppendAllText(logFilePath, logEntry);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Log Error: {ex.Message}");
                }
            }
        }
    }
}
