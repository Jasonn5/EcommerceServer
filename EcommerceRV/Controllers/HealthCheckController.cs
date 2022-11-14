using Microsoft.AspNetCore.Mvc;

namespace EcommerceRV.Controllers
{
    [Route("api/health-check")]
    [ApiController]
    public class HealthCheckController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Service running.");
        }
    }
}
