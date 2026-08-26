using System;
using Serilog;

namespace SerilogExample
{
    public class UserProfileService
    {
        private readonly ILogger _logger;

        public UserProfileService()
        {
            // Set contextual logger with module name prefix
            _logger = Log.ForContext("SourceContext", "UserProfile.UserProfileService");
        }

        public void LoadUserProfile(string username)
        {
            string className = nameof(UserProfileService) + ".txt";

            // LOG: Request entered tracing inside target file "UserProfileService.txt"
            _logger.ForContext("FileName", className)
                   .Information("Request entered: LoadUserProfile called for user '{Username}'.", username);

            try
            {
                // Scenario test: Throw null exception for error-user
                if (username == "error-user")
                {
                    throw new NullReferenceException("UserProfile data structure is null on database fetch.");
                }

                // Logical Validation test within Module
                if (username == "guest")
                {
                    // ERRORLOG: Access Denied logical error logged to "unauthorized.txt"
                    _logger.ForContext("FileName", "unauthorized.txt")
                           .Warning("Access Denied: Guest user profile loading is currently disabled.");
                }
                else
                {
                    // LOG: Success log to "log.txt"
                    _logger.ForContext("FileName", "log.txt")
                           .Information("User Profile loaded successfully for user '{Username}'. Role: Administrator.", username);
                }
            }
            catch (Exception ex)
            {
                // EXCEPTIONLOG: System exception logged automatically to "UserProfileService.txt"
                _logger.Error(ex, "System exception caught in module loading profile for '{Username}'.", username);
            }
        }
    }
}
