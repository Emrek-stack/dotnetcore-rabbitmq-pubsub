using Microsoft.Extensions.DependencyInjection;

namespace Rabbitmq.Publisher
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
            // add services
            //serviceCollection.AddTransient<ITestService, TestService>();

            // add app
            serviceCollection.AddTransient<App>();
        }
    }
}
