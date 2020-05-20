/*
using Autofac.Extensions.DependencyInjection;
using MicroS_Common.Logging;
using MicroS_Common.Metrics;
using MicroS_Common.Mvc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace MicroS.Services.Operations
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>()
                    .UseLogging()
                    //.UseVault()
                    .UseLockbox()
                    .UseAppMetrics()
                    ;
                });
    }
}
*/