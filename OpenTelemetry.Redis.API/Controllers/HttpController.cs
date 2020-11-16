using System;
using System.Net.Http;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace OpenTelemetry.Redis.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HttpController : ControllerBase
    {
        private readonly ILogger<HttpController> _logger;

        public HttpController(ILogger<HttpController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var http = new HttpClient{ BaseAddress = new Uri("https://github.com/") };
            var html = http.GetAsync("/").Result;

            return Ok(html);
        }
    }
}
