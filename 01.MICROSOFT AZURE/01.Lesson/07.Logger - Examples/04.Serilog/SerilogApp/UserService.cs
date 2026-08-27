using Serilog;

namespace SerilogApp;

/// <summary>
/// UserService - Uses Serilog ILogger (injected via constructor).
/// Mirrors the same logic as the CustomLogger version.
/// </summary>
public class UserService
{
    // Serilog logger specific to this class
    private readonly ILogger _logger;

    public UserService(ILogger logger)
    {
        // ForContext stamps every log from this class with SourceContext = "UserService"
        _logger = logger.ForContext<UserService>();
    }

    public void CreateUser()
    {
        _logger.Information("New user created successfully.");
    }
}
