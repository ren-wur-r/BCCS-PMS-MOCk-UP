using System;
using System.IO;
using DataModelLibrary.EfCore.ProjectManagerMain;
using DataModelLibrary.GlobalSystem.Config;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ServiceLibrary.Core.Manager.Extension;

namespace ServiceLibraryTest;

public class Startup
{
    public void ConfigureHost(IHostBuilder hostBuilder)
    {
        var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

        hostBuilder.ConfigureHostConfiguration(builder =>
            builder
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
        );
    }

    public void ConfigureServices(IServiceCollection services, HostBuilderContext builder)
    {
        // https://medium.com/tuimm/database-integration-tests-in-local-environment-b5b886de0a8c
        // https://github.com/m6t/redis-in-memory

        var config = builder.Configuration
            .GetSection(GsApiAppSettingConfig.APP_SETTINGS_SECTION)
            .Get<GsApiAppSettingConfig>();
        services.AddSingleton(config);
        services.AddSingleton(config.Database);

        // db
        services.AddDbContext<ProjectManagerMainContext>(options =>
        {
            options.UseSqlServer(config.Database.MainConnection);
        });

        // redis

        // memory

        services.AddCoreManager();
    }

}
