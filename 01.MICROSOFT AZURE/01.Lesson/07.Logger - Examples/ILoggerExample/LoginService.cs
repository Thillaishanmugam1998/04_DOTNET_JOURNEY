using System;
using Microsoft.Extensions.Logging;

namespace ILoggerExample
{
    public class LoginService
    {
        private readonly ILogger _logger;
        private readonly UserProfileService _userProfileService;

        public LoginService(ILoggerFactory loggerFactory)
        {
            // Category Name is structured to pass Module and Class name
            _logger = loggerFactory.CreateLogger("Authentication.LoginService");
            _userProfileService = new UserProfileService(loggerFactory);
        }

        public void Login(string username, string password)
        {
            string className = nameof(LoginService) + ".txt";

            // LOG: Request entered tracing inside target file "LoginService.txt"
            _logger.LogInformation(new EventId(0, className), "Request entered: Login method called with username '{Username}'.", username);

            try
            {
                // Scenario test: Throw database exception if username is "throw"
                if (username == "throw")
                {
                    throw new InvalidOperationException("Failed to reach database server. Timeout after 15 seconds.");
                }

                // Login Validation
                if (username == "admin" || username == "error-user" || username == "guest")
                {
                    if (password == "admin123")
                    {
                        // LOG: Success logged to "log.txt"
                        _logger.LogInformation(new EventId(0, "log.txt"), "Login success: User '{Username}' authenticated successfully.", username);

                        // Call the module service
                        _userProfileService.LoadUserProfile(username);
                    }
                    else
                    {
                        // ERRORLOG: Wrong credentials validation log to "wrong_users.txt"
                        _logger.LogWarning(new EventId(0, "wrong_users.txt"), "Login failed: Incorrect password for existing user '{Username}'.", username);
                    }
                }
                else
                {
                    // ERRORLOG: Non-existent user validation log to "wrong_users.txt"
                    _logger.LogWarning(new EventId(0, "wrong_users.txt"), "Login failed: Username '{Username}' does not exist.", username);
                }
            }
            catch (Exception ex)
            {
                // EXCEPTIONLOG: System exception logged automatically to "LoginService.txt"
                _logger.LogError(ex, "System exception caught in login procedure for user '{Username}'.", username);
            }
        }
    }
}
