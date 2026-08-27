using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DefaultILoggerSample
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
            _logger.LogInformation("Processing payment of {Amount:C}", amount);

            if (amount <= 0)
            {
                // Warning level log
                _logger.LogWarning("Invalid payment amount received: {Amount}. Skipping process.", amount);
                return;
            }

            try
            {
                // Simulating an error
                if (amount == 999) throw new Exception("Payment Gateway Timeout");

                _logger.LogInformation("Payment processed successfully.");
            }
            catch (Exception ex)
            {
                // Error level log with Exception object
                _logger.LogError(ex, "Failed to process payment for amount {Amount}.", amount);
            }
        }
    }
}
