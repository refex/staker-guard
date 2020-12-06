using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Polly;
using Polly.Extensions.Http;
using StakerGuard.Options;
using StakerGuard.Services;
using System;
using System.IO;
using System.Net.Http;

namespace StakerGuard
{
    public class Program
    {
        private static IConfigurationRoot Configuration;

        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureHostConfiguration(configHost =>
                {
                    configHost.SetBasePath(Directory.GetCurrentDirectory());
                    configHost.AddJsonFile("appsettings.json", optional: true);
                    Configuration = configHost.Build();
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.Configure<GuardOptions>(Configuration.GetSection(GuardOptions.SettingsName));
                    services.AddHostedService<Worker>();
                    services.AddHttpClient<IEth2Service, BeaconChainService>()
                                .AddPolicyHandler(GetRetryPolicy());
                });
        }

        static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
                .WaitAndRetryAsync(6, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
        }
    }
}
