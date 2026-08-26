using System;
using Microsoft.Extensions.Logging;

namespace ILoggerExample
{
    class Program
    {
        static void Main(string[] args)
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;

            // 1. Create a LoggerFactory with Custom File provider (Console logging removed for silent operation)
            using var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder
                    .AddFilter("Microsoft", LogLevel.Warning)
                    .AddFilter("System", LogLevel.Warning)
                    .AddFile(baseDir); // Register our custom File provider pointing to baseDir
            });

            // 2. Instantiate services passing LoggerFactory
            LoginService loginService = new LoginService(loggerFactory);

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
        }
    }
}
