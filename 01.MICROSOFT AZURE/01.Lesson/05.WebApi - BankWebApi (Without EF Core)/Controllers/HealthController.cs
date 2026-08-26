using Microsoft.AspNetCore.Mvc;

namespace Bank_Account_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HealthController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetHealth()
        {
            return Ok("Healthy");
        }
    }
}
