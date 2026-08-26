using System;

namespace CustomLoggerExample
{
    // 1. Auth Service Class
    public class LoginService
    {
        private readonly CustomLogger _logger;
        private readonly UserProfileService _userProfileService;
        private const string ModuleName = "Authentication";

        public LoginService(CustomLogger logger)
        {
            _logger = logger;
            _userProfileService = new UserProfileService(logger);
        }

        public void Login(string username, string password)
        {
            string className = nameof(LoginService) + ".txt";

            // Rule: Log "Request entered" to {ClassName}.txt under LOG/Authentication/{currentdate}/
            _logger.LogInfo(ModuleName, className, $"Request entered: Login method called with username '{username}'.");

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
                        // SUCCESS: Log to LOG/Authentication/{currentdate}/log.txt
                        _logger.LogInfo(ModuleName, "log.txt", $"Login success: User '{username}' authenticated successfully.");

                        // Call the module service
                        _userProfileService.LoadUserProfile(username);
                    }
                    else
                    {
                        // FAILURE: Log wrong credentials logical error to ERRORLOG/Authentication/{currentdate}/wrong_users.txt
                        _logger.LogError(ModuleName, "wrong_users.txt", $"Login failed: Incorrect password for existing user '{username}'.");
                    }
                }
                else
                {
                    // FAILURE: Log logical error for non-existent user
                    _logger.LogError(ModuleName, "wrong_users.txt", $"Login failed: Username '{username}' does not exist.");
                }
            }
            catch (Exception ex)
            {
                // EXCEPTION: Log to EXCEPTIONLOG/Authentication/{currentdate}/LoginService.txt
                _logger.LogException(ModuleName, className, ex);
            }
        }
    }
}
