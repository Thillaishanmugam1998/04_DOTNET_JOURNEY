using System;
using Serilog;

namespace SerilogExample
{
    public class LoginService
    {
        private readonly ILogger _logger;
        private readonly UserProfileService _userProfileService;

        public LoginService()
        {
            // Set contextual logger with module name prefix
            _logger = Log.ForContext("SourceContext", "Authentication.LoginService");
            _userProfileService = new UserProfileService();
        }

        public void Login(string username, string password)
        {
            string className = nameof(LoginService) + ".txt";

            // LOG: Request entered tracing inside target file "LoginService.txt"
            _logger.ForContext("FileName", className)
                   .Information("Request entered: Login method called with username '{Username}'.", username);

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
                        // LOG: Success log to "log.txt"
                        _logger.ForContext("FileName", "log.txt")
                               .Information("Login success: User '{Username}' authenticated successfully.", username);

                        // Call the module service
                        _userProfileService.LoadUserProfile(username);
                    }
                    else
                    {
                        // ERRORLOG: Wrong credentials validation log to "wrong_users.txt"
                        _logger.ForContext("FileName", "wrong_users.txt")
                               .Warning("Login failed: Incorrect password for existing user '{Username}'.", username);
                    }
                }
                else
                {
                    // ERRORLOG: Non-existent user validation log to "wrong_users.txt"
                    _logger.ForContext("FileName", "wrong_users.txt")
                               .Warning("Login failed: Username '{Username}' does not exist.", username);
                }
            }
            catch (Exception ex)
            {
                // EXCEPTIONLOG: System exception logged automatically to "LoginService.txt"
                _logger.Error(ex, "System exception caught in login procedure for user '{Username}'.", username);
            }
        }
    }
}
