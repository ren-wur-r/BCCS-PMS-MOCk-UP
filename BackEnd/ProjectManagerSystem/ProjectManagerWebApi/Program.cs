using System;
using System.IO;
using ApiModelLibrary;
using CommonLibrary;
using CommonLibrary.CmnHangfire.Filter;
using CommonLibrary.CmnSwagger.SchemaFilter;
using DataModelLibrary;
using DataModelLibrary.EfCore.ProjectManagerMain;
using DataModelLibrary.GlobalSystem.Config;
using DataModelLibrary.GlobalSystem.Env;
using DataModelLibrary.GlobalSystem.EnvVar;
using Hangfire;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using NLog.Extensions.Logging;
using ProjectManagerWebApi.Model;
using ServiceLibrary;
using ServiceLibrary.Core.Employee.Extension;
using ServiceLibrary.Core.Manager.Extension;
using ServiceLibrary.EipV1.Http.Format;
using ServiceLibrary.EipV1.Http.Service;
using ServiceLibrary.ManagerBackSite.Extension;
using ServiceLibrary.Schedule.Extension;
using Swashbuckle.AspNetCore.SwaggerUI;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // 取得環境名稱
        var environmentName = builder.Environment.EnvironmentName;

        #region Logging

        builder.Logging.AddNLog("NLog.config");

        #endregion

        #region Service

        // Add services to the container.
        builder.Services
            .AddControllers()
            .AddJsonOptions(c =>
            {
                c.JsonSerializerOptions.PropertyNamingPolicy = null;
            });

        // project
        builder.Services.AddSingleton<CommonLibraryProject>();
        builder.Services.AddSingleton<DataModelLibraryProject>();
        builder.Services.AddSingleton<ApiModelLibraryProject>();
        builder.Services.AddSingleton<ServiceLibraryProject>();

        // program
        var program = new ProjectManagerProgram();
        builder.Services.AddSingleton(program);

        #region EnvironmentVariable

        // 取得-全系統-環境參數
        var envVar = new GsEnvironmentVariable();
        builder.Services.AddSingleton(envVar);
#if DEBUG
        Console.WriteLine("===============================================================");
        Console.WriteLine($"envVar: {System.Text.Json.JsonSerializer.Serialize(envVar)}");
        Console.WriteLine("===============================================================");
#endif

        #endregion

        #region Config

        // 取得-全系統-ApiAppSetting-設定
        var config = builder.Configuration
            .GetSection(GsApiAppSettingConfig.APP_SETTINGS_SECTION)
            .Get<GsApiAppSettingConfig>();
        config.Database.MainConnection = string.IsNullOrWhiteSpace(envVar.DbMain) == false ? envVar.DbMain : config.Database.MainConnection;
        builder.Services.AddSingleton(config);
        builder.Services.AddSingleton(config.Database);
        builder.Services.AddSingleton(config.PlatformConfig);
        builder.Services.AddSingleton(config.SmtpConfig);
        builder.Services.AddSingleton(config.BillConfig);
        builder.Services.AddSingleton(config.EipV1Config);
#if DEBUG
        Console.WriteLine("===============================================================");
        Console.WriteLine($"config: {System.Text.Json.JsonSerializer.Serialize(config)}");
        Console.WriteLine("===============================================================");
#endif

        #endregion

        #region Swagger

        if (environmentName == GsEnvironmentConst.LOCAL
           || environmentName == GsEnvironmentConst.DEVELOPMENT)
        {
            builder.Services.AddEndpointsApiExplorer();

            // service swagger
            builder.Services.AddSwaggerGen(c =>
            {
                // model summery
                var dataModelLibraryXmlPath = Path.Combine(AppContext.BaseDirectory, "DataModelLibrary.xml");
                var apiModelLibraryXmlPath = Path.Combine(AppContext.BaseDirectory, "ApiModelLibrary.xml");
                var projectXmlPath = Path.Combine(AppContext.BaseDirectory, $"{program.ProjectName}.xml");

                c.SwaggerDoc("v1", new OpenApiInfo { Title = program.ProjectName, Version = "v1" });
                c.IncludeXmlComments(dataModelLibraryXmlPath);
                c.IncludeXmlComments(apiModelLibraryXmlPath);
                c.IncludeXmlComments(projectXmlPath, true);
                c.SchemaFilter<CmnSwaggerEnumSchemaFilter>(dataModelLibraryXmlPath);
                c.SchemaFilter<CmnSwaggerEnumSchemaFilter>(apiModelLibraryXmlPath);
                c.SchemaFilter<CmnSwaggerEnumSchemaFilter>(projectXmlPath);
            });
        }

        #endregion

        #region Handfire

        builder.Services.AddHangfire(hangfireConfig =>
            hangfireConfig
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseInMemoryStorage());
        builder.Services.AddHangfireServer();

        #endregion

        #region Connection, db, mongo, reids, http, rabbitmq

        // db
        builder.Services.AddDbContext<ProjectManagerMainContext>(options =>
        {
            options.UseSqlServer(config.Database.MainConnection);
        });

        // http
        builder.Services.AddHttpClient();
        builder.Services.AddHttpClient(EipV1HttpConst.HTTP_CLIENT_NAME, client =>
        {
            client.BaseAddress = new Uri(config.EipV1Config.BaseUrl);
            client.Timeout = TimeSpan.FromSeconds(10);
            client.DefaultRequestHeaders.Add("Api-Key", config.EipV1Config.ApiKey);
        });

        builder.Services.AddSingleton(config.EipV1Config);
        builder.Services.AddSingleton<IEipV1HttpService, EipV1HttpService>(); // EipV1-Http-服務

        #endregion

        #region Core Character

        //builder.Services.AddProjectGlobalSystem();
        //builder.Services.AddCoreAtom();
        builder.Services.AddCoreManager();
        builder.Services.AddCoreEmployee();
        //builder.Services.AddCoreCompany();

        #endregion

        // project
        builder.Services.AddProjectManagerBackSite();
        builder.Services.AddProjectSchedule();

        #endregion

        #region App

        var app = builder.Build();

        // swagger
        if (environmentName == GsEnvironmentConst.LOCAL
            || environmentName == GsEnvironmentConst.DEVELOPMENT)
        {
            // app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.DocExpansion(DocExpansion.None);
            });

            // 使用Cors
            app.UseCors(builder =>
            {
                builder
                    .SetIsOriginAllowed(isOriginAllowed => true)
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
            });
        }

        app.UseAuthorization();
        app.MapControllers();

        // Hangfire Dashboard
        app.MapHangfireDashboard(
            pattern: "/hangfire-schedule",
            options: new DashboardOptions()
            {
                Authorization = new[] { new CmnHangfireNoAuthFilter() },
                IsReadOnlyFunc = (context) => { return true; },
            });

        // project
        app.AddProjectManagerBackSite();
        app.AddProjectSchedule();

        #endregion

        app.Run();
    }
}