using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DefaultILoggerFileLogger
{
    public class PaymentService
    {
        private readonly ILogger<PaymentService> _logger;

        public PaymentService(ILogger<PaymentService> logger)
        {
            _logger = logger;
        }

        public void ProcessPayment(decimal amount)
        {
            _logger.LogInformation($"Payment of {amount:C} processed.");

            _logger.LogError("ex", "Payment calculation failed.");
        }
    }
}
