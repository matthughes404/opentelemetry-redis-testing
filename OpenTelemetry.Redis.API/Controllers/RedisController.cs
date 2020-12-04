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
        private readonly IConnectionMultiplexer _connection;

        public RedisController(ILogger<RedisController> logger, IConnectionMultiplexer connection)
        {
            _logger = logger;
            _connection = connection;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var redis = _connection.GetDatabase();

            var msg = redis.StringGet("testKey01");
            _logger.LogInformation(msg);

            return Ok(msg.ToString());
        }
    }
}
