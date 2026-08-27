using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomLogger
{
    public class PaymentService
    {
        public void ProcessPayment()
        {
            // Idhu Log/2026-08-27/PaymentService.txt la eludhum
            Logger.WriteLog("Payment of $100 processed.");
        }
    }
}
