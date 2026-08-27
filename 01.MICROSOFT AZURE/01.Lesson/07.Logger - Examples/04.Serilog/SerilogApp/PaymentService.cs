using Serilog;

namespace SerilogApp;

/// <summary>
/// PaymentService - Uses Serilog ILogger (injected via constructor).
/// Mirrors the same logic as the CustomLogger version.
/// </summary>
public class PaymentService
{
    // Serilog logger specific to this class
    private readonly ILogger _logger;

    public PaymentService(ILogger logger)
    {
        // ForContext stamps every log from this class with SourceContext = "PaymentService"
        _logger = logger.ForContext<PaymentService>();
    }

    public void ProcessPayment()
    {
        _logger.Information("Payment of $100 processed.");
    }
}
