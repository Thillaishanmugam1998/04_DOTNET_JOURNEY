using System;
using Serilog;

namespace SerilogStandard
{
    public class UserProfileService
    {
        // Standard practice: ForContext<T>() automatically enriches logs with "SourceContext": "SerilogStandard.UserProfileService"
        private readonly ILogger _logger = Log.ForContext<UserProfileService>();

        public void LoadUserProfile(string username)
        {
            // Tracing method entry inside Module
            _logger.Information("Request entered: LoadUserProfile called for user {Username}.", username);

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
                    // FAILURE: Log module logical validation warning
                    _logger.Warning("Access Denied: Guest user profile loading is currently disabled for '{Username}'.", username);
                }
                else
                {
                    // SUCCESS: Log success information
                    _logger.Information("User Profile loaded successfully for user '{Username}'. Role: Administrator.", username);
                }
            }
            catch (Exception ex)
            {
                // EXCEPTION: Log exception in module using standard Error level
                _logger.Error(ex, "System exception caught in module loading profile for '{Username}'.", username);
            }
        }
    }
}
