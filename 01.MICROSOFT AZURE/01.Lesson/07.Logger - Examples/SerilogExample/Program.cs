using System;
using System.IO;
using Serilog;

namespace SerilogExample
{
    class Program
    {
        static void Main(string[] args)
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;

            // 1. Configure Serilog to use CustomFileSink only (Console sink removed for silent operation)
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Sink(new CustomFileSink(baseDir))
                .CreateLogger();

            // 2. Instantiate login service
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

            // 3. Close and Flush logs
            Log.CloseAndFlush();
        }
    }
}
