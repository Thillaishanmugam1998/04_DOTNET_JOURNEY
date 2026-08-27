using System;
using System.IO;
using System.Threading.Tasks;
using System.Runtime.CompilerServices; // CallerFilePath-ku idhu avaram

namespace CustomLogger
{
    class Program
    {
        static void Main(string[] args)
        {
            // 1. Initialize the logger
            Logger.Initialize();

            // 2. Different classes la irundhu log call panrom
            var userService = new UserService();
            userService.CreateUser();

            var paymentService = new PaymentService();
            paymentService.ProcessPayment();

            // 3. Parallel ah test panrom (Thread safety check)
            Parallel.For(0, 5, i =>
            {
                Logger.WriteLog($"Parallel task {i} executed."); 
            });

        }
    }
}