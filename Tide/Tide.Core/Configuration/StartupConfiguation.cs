using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tide.Core.Configuration
{
    public abstract class StartupConfiguation
    {
        private static IConfiguration AddConfiguration(IServiceCollection services)
        {
            var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
            .AddEnvironmentVariables();

            var config = builder.Build();
            services.AddSingleton<IConfiguration>(config);
            return config;
        }



        protected abstract void ConfigureServices(IServiceCollection services, IConfiguration configuration);

        public ServiceProvider Build()
        {
            var services = new ServiceCollection();

            var config = AddConfiguration(services);

            ConfigureServices(services, config);

            var provider = services.BuildServiceProvider();

            return provider;
        }

    }
}
