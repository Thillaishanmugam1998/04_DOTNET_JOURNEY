using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomLogger
{
    public class UserService
    {
        public void CreateUser()
        {
            // Idhu Log/2026-08-27/UserService.txt la eludhum
            Logger.WriteLog("New user created successfully.");
        }
    }
}
