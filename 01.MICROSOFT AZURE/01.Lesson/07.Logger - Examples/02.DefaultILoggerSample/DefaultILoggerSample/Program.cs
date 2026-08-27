using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DefaultILoggerSample
{

    class Program
    {
        static void Main(string[] args)
        {
            // Host.CreateDefaultBuilder() -> Idhu thaan main magic!
            // Idhu automatic ah Console logging, appsettings.json configuration ellathayum setup pannidum.
            var host = Host.CreateDefaultBuilder(args)
                .ConfigureServices((context, services) =>
                {
                    // Namma business classes ah DI container la add panrom
                    services.AddTransient<UserService>();
                    services.AddTransient<PaymentService>();
                })
                .Build();

            // DI container la irundhu classes ah get panrom
            var userService = host.Services.GetRequiredService<UserService>();
            var paymentService = host.Services.GetRequiredService<PaymentService>();

            // Methods ah call panrom
            userService.CreateUser("JohnDoe");
            paymentService.ProcessPayment(500.00m);

            // Warning test
            paymentService.ProcessPayment(-10.00m);

            // Error test
            paymentService.ProcessPayment(999.00m);

        }
    }
}