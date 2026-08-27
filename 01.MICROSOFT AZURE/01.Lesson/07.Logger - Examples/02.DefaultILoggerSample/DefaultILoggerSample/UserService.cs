using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DefaultILoggerSample
{
    public class UserService
    {
        // Constructor la ILogger<T> inject panrom
        private readonly ILogger<UserService> _logger;

        public UserService(ILogger<UserService> logger)
        {
            _logger = logger;
        }

        public void CreateUser(string userName)
        {
            // Information level log
            _logger.LogInformation("Starting user creation process for {UserName}.", userName);

            // Simulating some work
            System.Threading.Thread.Sleep(100);

            _logger.LogInformation("User {UserName} created successfully in the database.", userName);
        }
    }
}
