using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DefaultILoggerFileLogger
{
    public class UserService
    {
        private readonly ILogger<UserService> _logger;

        // Constructor la ILogger inject aagum
        public UserService(ILogger<UserService> logger)
        {
            _logger = logger;
        }

        public void CreateUser(string userName)
        {
            _logger.LogInformation($"New user '{userName}' created successfully.");
        }
    }
}
