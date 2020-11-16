using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using StackExchange.Redis;

namespace OpenTelemetry.Redis.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RedisController : ControllerBase
    {
        private readonly ILogger<RedisController> _logger;

        public RedisController(ILogger<RedisController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var conn = ConnectionMultiplexer.Connect("localhost:6379");
            var redis = conn.GetDatabase();

            var msg = redis.StringGet("testKey01");
            _logger.LogInformation(msg);

            return Ok(msg.ToString());
        }
    }
}
