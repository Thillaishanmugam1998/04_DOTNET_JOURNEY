using System;

namespace CustomLoggerExample
{
    // 2. Module Service Class (Module Name: UserProfile)
    public class UserProfileService
    {
        private readonly CustomLogger _logger;
        private const string ModuleName = "UserProfile";

        public UserProfileService(CustomLogger logger)
        {
            _logger = logger;
        }

        public void LoadUserProfile(string username)
        {
            string className = nameof(UserProfileService) + ".txt";

            // Rule: Log "Request entered" to {ClassName}.txt under LOG/UserProfile/{currentdate}/
            _logger.LogInfo(ModuleName, className, $"Request entered: LoadUserProfile called for user '{username}'.");

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
                    // Logical Error inside Module: Log to ERRORLOG/UserProfile/{currentdate}/unauthorized.txt
                    _logger.LogError(ModuleName, "unauthorized.txt", $"Access Denied: Guest user profile loading is currently disabled.");
                }
                else
                {
                    // Success inside Module: Log to LOG/UserProfile/{currentdate}/log.txt
                    _logger.LogInfo(ModuleName, "log.txt", $"User Profile loaded successfully for user '{username}'. Role: Administrator.");
                }
            }
            catch (Exception ex)
            {
                // EXCEPTION inside Module: Log to EXCEPTIONLOG/UserProfile/{currentdate}/UserProfileService.txt
                _logger.LogException(ModuleName, className, ex);
            }
        }
    }
}
