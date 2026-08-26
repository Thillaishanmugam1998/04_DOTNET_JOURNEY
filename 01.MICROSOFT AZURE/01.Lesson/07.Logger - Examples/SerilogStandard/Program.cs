using System;
using System.IO;
using Serilog;
using Serilog.Formatting.Compact;

namespace SerilogStandard
{
    class Program
    {
        static void Main(string[] args)
        {
            // Define log files path inside "logs" directory
            string logFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs");
            string textLogFile = Path.Combine(logFolder, "app-log-.txt");
            string jsonLogFile = Path.Combine(logFolder, "app-log-.json");

            // 1. Configure Serilog - PURE Standard Configuration (Console, Plain File, and JSON File)
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                // Write 1: Console logging
                .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] [{SourceContext}] {Message:lj}{NewLine}{Exception}")
                // Write 2: Text file logging (Standard daily rolling file)
                .WriteTo.File(textLogFile, 
                    rollingInterval: RollingInterval.Day,
                    outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss.fff}] [{Level:u3}] [{SourceContext}] {Message:lj}{NewLine}{Exception}")
                // Write 3: JSON structured logging (Using Serilog's built-in CompactJsonFormatter!)
                .WriteTo.File(new CompactJsonFormatter(), jsonLogFile, 
                    rollingInterval: RollingInterval.Day)
                .CreateLogger();

            Log.Information("=== Serilog Standard Demo Started ===");
            Log.Information("Logs are saved to folder: {LogFolder}", logFolder);

            // 2. Instantiate services (Serilog's Log static class is used globally)
            LoginService loginService = new LoginService();

            // Run Scenario 1: Successful Login & UserProfile Module Call
            loginService.Login("admin", "admin123");

            // Run Scenario 2: Failed Login (Logical Validation Error)
            loginService.Login("thillai", "wrongpass");

            // Run Scenario 3: Exception during Login method
            loginService.Login("throw", "anypassword");

            // Run Scenario 4: Successful Login but Exception inside UserProfile Module
            loginService.Login("error-user", "admin123");

            // Run Scenario 5: Successful Login but Logical Error inside UserProfile Module
            loginService.Login("guest", "admin123");

            Log.Information("=== Serilog Standard Demo Completed ===");
            
            // 3. Flush and close the logger to write everything to disk
            Log.CloseAndFlush();

            Console.WriteLine("\n[Console Output] App execution complete. Check the logs/ folder for output files.");
        }
    }
}
