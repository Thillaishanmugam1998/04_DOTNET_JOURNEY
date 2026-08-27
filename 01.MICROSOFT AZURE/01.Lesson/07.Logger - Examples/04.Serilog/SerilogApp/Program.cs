using Serilog;

namespace SerilogApp;

class Program
{
    static void Main(string[] args)
    {
        // ── 1. Configure Serilog ────────────────────────────────────────────
        // WriteTo.Console  → Logs appear in the terminal (colour-coded)
        // WriteTo.File     → Logs written to Log/<date>/app.txt (same folder style as CustomLogger)
        //                    rollingInterval: RollingInterval.Day → new file every day
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.Console(
                outputTemplate: "[{Timestamp:HH:mm:ss.fff} {Level:u3}] [{SourceContext}] {Message:lj}{NewLine}{Exception}")
            .WriteTo.File(
                path: "Log/.txt",
                rollingInterval: RollingInterval.Day,
                outputTemplate: "[{Timestamp:HH:mm:ss.fff} {Level:u3}] [{SourceContext}] {Message:lj}{NewLine}{Exception}")
            .CreateLogger();

        // ── 2. Inject the global Serilog logger into each service ──────────
        var userService    = new UserService(Log.Logger);
        var paymentService = new PaymentService(Log.Logger);

        // ── 3. Call service methods (same as CustomLogger sample) ──────────
        userService.CreateUser();
        paymentService.ProcessPayment();

        // ── 4. Parallel task test (Thread-safety check) ────────────────────
        Parallel.For(0, 5, i =>
        {
            // Log.Logger is thread-safe in Serilog
            Log.Information("Parallel task {TaskId} executed.", i);
        });

        // ── 5. Flush & close before app exits ─────────────────────────────
        Log.CloseAndFlush();
    }
}
