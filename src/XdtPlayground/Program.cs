using System;
using System.Net.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using XdtPlayground.Monaco;

namespace XdtPlayground
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);

            //  builder.Logging.AddProvider(new CustomLoggingProvider());
            //builder.Services.AddLogging(cfg =>
            //{
            //    cfg.ClearProviders();
            //    cfg.SetMinimumLevel(LogLevel.Trace);
            //});

            builder.Services.AddMonaco();
            builder.RootComponents.Add<App>("app");

          //  builder.Services.AddTransient(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
            Console.WriteLine(builder.HostEnvironment.BaseAddress);
            await builder.Build().RunAsync();
        }
    }
}
