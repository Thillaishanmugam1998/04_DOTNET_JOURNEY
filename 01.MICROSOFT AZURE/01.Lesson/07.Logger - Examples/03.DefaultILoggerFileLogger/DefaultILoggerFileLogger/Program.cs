using System;
using System.IO;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DefaultILoggerFileLogger
{
    public static class FileLoggerExtensions
    {
        public static ILoggingBuilder AddSimpleFileLogger(this ILoggingBuilder builder)
        {
            builder.AddProvider(new SimpleFileLoggerProvider());
            return builder;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // 1. Dependency Injection Container setup
            var services = new ServiceCollection();

            // Namma custom file logger ah add panrom
            services.AddLogging(builder =>
            {
                builder.AddSimpleFileLogger(); // Namma extension method
                builder.SetMinimumLevel(LogLevel.Information);
            });

            // Business classes ah add panrom
            services.AddTransient<UserService>();
            services.AddTransient<PaymentService>();

            // Build the service provider
            var serviceProvider = services.BuildServiceProvider();

            // 2. Classes ah resolve panni use panrom
            var userService = serviceProvider.GetRequiredService<UserService>();
            var paymentService = serviceProvider.GetRequiredService<PaymentService>();

            userService.CreateUser("JohnDoe");
            paymentService.ProcessPayment(500.00m);

            Console.WriteLine("Logs ellam sariya eludhi aayiduchu! Bin/Debug/Log folder ah check pannunga.");
            Console.ReadLine();
        }
    }
}