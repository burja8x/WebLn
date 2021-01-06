using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebLn.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class DemoController : Controller
    {
        private readonly Settings _settings;
        public DemoController(IOptionsSnapshot<Settings> settings)
        {
            _settings = settings.Value;
        }

        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            Console.WriteLine("DEMO appconfiguration");
            return new string[] { "This demo for Azure app configuration...", _settings.Message };
        }

        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            if (Microsoft.Extensions.Diagnostics.HealthChecks.RandomHealthCheck.healthy)
            {
                Microsoft.Extensions.Diagnostics.HealthChecks.RandomHealthCheck.healthy = false;
            }
            else
            {
                Microsoft.Extensions.Diagnostics.HealthChecks.RandomHealthCheck.healthy = true;
            }

            return $"RandomHealthCheck .... healthy = {Microsoft.Extensions.Diagnostics.HealthChecks.RandomHealthCheck.healthy}";
        }
    }
}
