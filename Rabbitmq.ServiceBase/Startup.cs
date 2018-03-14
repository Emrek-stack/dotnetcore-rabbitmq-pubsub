using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Rabbitmq.ServiceBase
{
    public class Startup
    {
        internal static void ConfigureServices(IServiceCollection serviceCollection)
        {
            // add logging
            serviceCollection.AddSingleton(new LoggerFactory()
                .AddConsole()
                .AddDebug());
            serviceCollection.AddLogging();

            // build configuration
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false)
                .Build();

            serviceCollection.AddOptions();
            //serviceCollection.Configure<AppSettings>(configuration.GetSection("Configuration"));
            ConfigureConsole(configuration);

            // add services
           // serviceCollection.AddTransient<ITestService, TestService>();

            // add app
            //serviceCollection.AddTransient<App>();
        }

        private static void ConfigureConsole(IConfigurationRoot configuration)
        {
            System.Console.Title = configuration.GetSection("Configuration:ConsoleTitle").Value;
        }
    }
}