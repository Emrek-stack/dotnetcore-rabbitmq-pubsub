using System;
using Microsoft.Extensions.DependencyInjection;

namespace Rabbitmq.ServiceBase
{
    public class WindowsServiceBaseRunner
    {
        public void Run()
        {
            // create service collection
            var serviceCollection = new ServiceCollection();
            Startup.ConfigureServices(serviceCollection);

            // create service provider
            var serviceProvider = serviceCollection.BuildServiceProvider();
        }
    }
}