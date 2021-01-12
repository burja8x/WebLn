using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Polly;
using Serilog;
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
            Log.Information("DEMO app configuration");
            return new string[] { "This demo for Azure app configuration...", _settings.Message };
        }

        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            try
            {
                if (id == 0)
                {
                    Policy.Handle<Exception>().RetryForever().Execute(DemoRandomError);
                    return "OK execute retry forever.";
                }
                else if (id == 1)
                {
                    Policy.Handle<Exception>().Retry(10, (e, i) => Log.Information($"Error '{e.Message}' at retry #{i}")).Execute(DemoRandomError);
                    return "OK execute retry n times. log...";
                }
                else if (id == 2)
                {
                    Policy.Handle<Exception>().Retry(10).Execute(DemoRandomError);
                    return "OK execute retry n times.";
                }
                else if (id == 3) {
                    Policy.Handle<Exception>().WaitAndRetry(new[] { TimeSpan.FromMilliseconds(100), TimeSpan.FromMilliseconds(200) }).Execute(DemoRandomError);
                    return "OK execute wait and retry 100ms 200ms.";
                }
                else if (id == 4) {
                    Policy.Handle<Exception>().WaitAndRetry(5, count => TimeSpan.FromSeconds(count)).Execute(DemoRandomError);
                    return "OK execute wait and retry count 5 ....from seconds....";
                }
                else if (id == 5) {
                    Policy.Handle<Exception>().WaitAndRetryForever(count => TimeSpan.FromSeconds(count)).Execute(DemoRandomError);
                    return "OK execute wait (count) and retry forever.";
                }
                else if (id == 6) {
                    Policy.Handle<Exception>().CircuitBreaker(4, TimeSpan.FromSeconds(6)).Execute(DemoRandomError);
                    return "OK execute circuit braker";
                }
            }
            catch (Exception e)
            {
                Log.Error(e, "DEMO fault tolerance");
                return "DEMO Error";
            }
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

        private void DemoRandomError() {
            var random = new Random().Next(0, 6);
            if (random != 5)
            {
                Log.Information($"DEMO: 5 != {random}");
                throw new Exception("DEMO random number is not 5.");
            }
            else {
                Log.Information("DEMO random number is 5 :) !");
                return;
            }
        }
    }
}
