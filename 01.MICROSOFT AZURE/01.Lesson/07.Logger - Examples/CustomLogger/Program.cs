using System;

namespace CustomLoggerExample
{
    class Program
    {
        static void Main(string[] args)
        {
            CustomLogger logger = new CustomLogger();
            LoginService loginService = new LoginService(logger);

            // Running Scenario 1: Successful Login & UserProfile Module Call
            loginService.Login("admin", "admin123");

            // Running Scenario 2: Failed Login (Logical Validation Error)
            loginService.Login("thillai", "wrongpass");

            // Running Scenario 3: Exception during Login method
            loginService.Login("throw", "anypassword");

            // Running Scenario 4: Successful Login but Exception inside UserProfile Module
            loginService.Login("error-user", "admin123");

            // Running Scenario 5: Successful Login but Logical Error inside UserProfile Module
            loginService.Login("guest", "admin123");
        }
    }
}
