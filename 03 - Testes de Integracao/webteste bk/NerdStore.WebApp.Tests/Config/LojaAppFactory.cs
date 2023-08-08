using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Hosting;
using NerdStore.WebApp.MVC;

namespace NerdStore.WebApp.Tests.Config
{
    public class LojaAppFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
    {
        //protected override void ConfigureWebHost(IWebHostBuilder builder)
        //{
        //    builder.UseStartup<TStartup>();
        //    builder.UseEnvironment("Testing");
        //}

        protected override IHostBuilder CreateHostBuilder()
        {
            var builder = Host.CreateDefaultBuilder()
                              .ConfigureWebHostDefaults(x =>
                              {
                                  x.UseStartup <StartupWebTests>().UseTestServer();
                              });
            return builder;
        }
    }
}
