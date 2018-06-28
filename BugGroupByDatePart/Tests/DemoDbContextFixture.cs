using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using BugGroupByDatePart.Classes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using Xunit;

namespace BugGroupByDatePart.Tests
{
    public class DemoDbContextFixture : IDisposable
    {
        // Fixture ID
        public const string ID = "DemoDbContext";

        public DemoDbContext Context { get; private set; }
        public ILoggerFactory LoggerFactory { get; private set; }
        public IConfigurationRoot Configuration { get; private set; }

        private static IConfigurationRoot LoadConfiguration()
        {
            var path = AppDomain.CurrentDomain.BaseDirectory;
            var builder = new ConfigurationBuilder()
                .SetBasePath(path)
                .AddJsonFile("appsettings.json", optional: false)
                .AddEnvironmentVariables();

            return builder.Build();
        }

        private static ServiceProvider BuildDI(string connectString)
        {
            var services = new ServiceCollection();
            services.AddScoped<ILoggerFactory, LoggerFactory>();
            services.AddScoped(typeof(ILogger<>), typeof(Logger<>));
            services.AddDbContext<DemoDbContext>(options =>
            {
                options
                    .UseSqlServer(connectString)
                    .EnableSensitiveDataLogging(false); // true => log sql
            });

            var serviceProvider = services.BuildServiceProvider();

            // add logger config
            var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
            loggerFactory.AddNLog();
            NLog.LogManager.LoadConfiguration("nlog.config");

            return serviceProvider;
        }

        public DemoDbContextFixture()
        {
            Configuration = LoadConfiguration();

            string connectString = Configuration.GetConnectionString("DemoDbConnection");
            var serviceProvider = BuildDI(connectString);

            Context = serviceProvider.GetRequiredService<DemoDbContext>();
            LoggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
        }

        public void Dispose()
        {
            if (Context != null)
            {
                Context.Dispose();
            }
            NLog.LogManager.Shutdown();
        }
    }

    /// <summary>
    /// This class has no code, and is never created. Its purpose is simply
    /// to be the place to apply [CollectionDefinition] and all the
    /// ICollectionFixture<> interfaces.
    /// </summary>
    [CollectionDefinition(DemoDbContextFixture.ID)]
    public class DatabaseCollection : ICollectionFixture<DemoDbContextFixture>
    {

    }
}
