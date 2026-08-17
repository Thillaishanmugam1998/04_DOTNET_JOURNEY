using Microsoft.AspNetCore.Mvc;
namespace ProductManagementApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HealthController : ControllerBase
    {
        private readonly ILogger<HealthController> _logger;
        public HealthController(ILogger<HealthController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Get()
        {
            _logger.LogInformation("Health check endpoint was called successfully.");

            return Ok(new
            {
                Status = "Healthy",
                Message = "Product Management API is running successfully."
            });
        }
    }
}