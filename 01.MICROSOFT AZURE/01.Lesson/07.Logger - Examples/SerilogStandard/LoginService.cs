using System;
using Serilog;

namespace SerilogStandard
{
    public class LoginService
    {
        // Standard practice: ForContext<T>() automatically enriches logs with "SourceContext": "SerilogStandard.LoginService"
        private readonly ILogger _logger = Log.ForContext<LoginService>();
        private readonly UserProfileService _userProfileService;

        public LoginService()
        {
            _userProfileService = new UserProfileService();
        }

        public void Login(string username, string password)
        {
            // Tracing method entry using structured parameters
            _logger.Information("Request entered: Login method called for username {Username}.", username);

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
                        // SUCCESS: Log success information
                        _logger.Information("Login success: User '{Username}' authenticated successfully.", username);

                        // Call the module service
                        _userProfileService.LoadUserProfile(username);
                    }
                    else
                    {
                        // FAILURE: Log incorrect credentials as Warning (Logical Error)
                        _logger.Warning("Login failed: Incorrect password for existing user '{Username}'.", username);
                    }
                }
                else
                {
                    // FAILURE: Log non-existent user as Warning (Logical Error)
                    _logger.Warning("Login failed: Username '{Username}' does not exist.", username);
                }
            }
            catch (Exception ex)
            {
                // EXCEPTION: Log exception with full stack trace using standard Error level
                _logger.Error(ex, "System exception caught in login procedure for user '{Username}'.", username);
            }
        }
    }
}
