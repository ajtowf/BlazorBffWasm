using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BackendApplication.Controllers
{
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        private readonly ILogger<TestController> _logger;

        public TestController(ILogger<TestController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<Claim> Get()
        {
            return User.Claims.Select(c => new Claim { Type = c.Type, Value = c.Value });
        }

        public class Claim
        {
            public string Type { get; set; } = "";
            public string Value { get; set; } = "";
        }
    }
}
