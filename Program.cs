using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebLn
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureAppConfiguration((hostingContext, config) =>
                    {
                        //var connection = config.Build().GetConnectionString("AppConfig");
                        var connection = Environment.GetEnvironmentVariable("APP_CONFIG_CONN_STRING");
                        config.AddAzureAppConfiguration(options =>
                        {
                            options.Connect(connection)
                                   .ConfigureRefresh(refresh =>
                                   {
                                       refresh.Register("TestApp:Settings:Message", refreshAll: true)
                                      .SetCacheExpiration(new TimeSpan(0, 0, 20));
                                   });
                        });
                    }).UseStartup<Startup>();
                });
    }
}
