using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Rabbitmq.Subscriber
{
    class Program
    {
        public static void Main(string[] args)
        {
            // create service collection
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            // create service provider
            var serviceProvider = serviceCollection.BuildServiceProvider();

            // entry to run app
            serviceProvider.GetService<App>().Run();
        }

        private static void ConfigureServices(IServiceCollection serviceCollection)
        {
            // Add logging
            serviceCollection.AddSingleton(new LoggerFactory()
                .AddConsole()
                .AddSerilog()
                .AddDebug());
            serviceCollection.AddLogging();

            // Build configuration
            var configuration = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", false)
                .Build();


            // Initialize serilog logger
            Log.Logger = new LoggerConfiguration()

                .MinimumLevel.Debug()
                .Enrich.FromLogContext()
                .CreateLogger();

            // Add access to generic IConfigurationRoot
            serviceCollection.AddSingleton(configuration);

            // add services
            //serviceCollection.AddTransient<ITestService, TestService>();

            // add app
            serviceCollection.AddTransient<App>();
        }
    }
}
