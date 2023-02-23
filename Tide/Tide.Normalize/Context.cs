using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
//using Microsoft.Extensions.Configuration.Binder;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tide.Core.Configuration;
using Tide.Data.Ef;

namespace Tide.Normalize
{
    internal static class Context
    {
        private class Builder : StartupConfiguation
        {
            protected override void ConfigureServices(IServiceCollection services, IConfiguration configuration)
            {
                services.AddDbContext<TideContext>(options =>
             options.UseSqlServer(
                 configuration.GetConnectionString(configuration.GetConnectionString("DefaultConnection")!)));
            }
        }


        static Context()
        {
            Builder bld = new();
            _provider = bld.Build();
            _connectionString = GetService<IConfiguration>().GetConnectionString("DefaultConnection")!;
            _path = GetService<IConfiguration>()?["IO"]?.ToString()!;
            _folders = GetService<IConfiguration>().GetSection("Folders").Get<List<int>>()!;
        }

        private static readonly ServiceProvider _provider;

        private static readonly string _connectionString;
        private static readonly string _path;
        private static readonly IReadOnlyCollection<int> _folders;

        public static string Path => _path;
        public static IReadOnlyCollection<int> Folders => _folders;
        public static IService GetService<IService>() where IService : notnull => _provider.GetRequiredService<IService>();
        public static TideContext Db
        {
            get
            {
                var optionsBuilder = new DbContextOptionsBuilder<TideContext>();
                optionsBuilder.UseSqlServer(_connectionString);
                return new TideContext(optionsBuilder.Options);
            }
        }
    }
}
