using System;
using Microsoft.Extensions.Logging;

namespace ILoggerExample
{
    public class UserProfileService
    {
        private readonly ILogger _logger;

        public UserProfileService(ILoggerFactory loggerFactory)
        {
            // Category Name is structured to pass Module and Class name
            _logger = loggerFactory.CreateLogger("UserProfile.UserProfileService");
        }

        public void LoadUserProfile(string username)
        {
            string className = nameof(UserProfileService) + ".txt";

            // LOG: Request entered tracing inside target file "UserProfileService.txt"
            _logger.LogInformation(new EventId(0, className), "Request entered: LoadUserProfile called for user '{Username}'.", username);

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
                    _logger.LogWarning(new EventId(0, "unauthorized.txt"), "Access Denied: Guest user profile loading is currently disabled.");
                }
                else
                {
                    // LOG: Success log to "log.txt"
                    _logger.LogInformation(new EventId(0, "log.txt"), "User Profile loaded successfully for user '{Username}'. Role: Administrator.", username);
                }
            }
            catch (Exception ex)
            {
                // EXCEPTIONLOG: System exception logged automatically to "UserProfileService.txt"
                _logger.LogError(ex, "System exception caught in module loading profile for '{Username}'.", username);
            }
        }
    }
}
