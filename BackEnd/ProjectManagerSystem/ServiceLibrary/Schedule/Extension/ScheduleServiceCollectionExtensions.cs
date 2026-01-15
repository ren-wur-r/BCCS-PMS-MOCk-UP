using System;
using CommonLibrary.CmnSMTP.Service;
using Microsoft.Extensions.DependencyInjection;
using ServiceLibrary.Schedule.Logical.Initial.Service;
using ServiceLibrary.Schedule.Logical.Timer.Service;

namespace ServiceLibrary.Schedule.Extension;

public static class ScheduleServiceCollectionExtensions
{
    /// <summary>加入專案-排程</summary>
    public static IServiceCollection AddProjectSchedule(this IServiceCollection services)
    {
        if (services == null)
        {
            throw new ArgumentNullException(nameof(services));
        }

        // common library 
        services.AddScoped<ICmnSmtpService, CmnSmtpService>(); // 通用-SMTP-服務


        // Api

        // hub

        // logical, 排序請依照 Logical 資料夾排序
        services.AddScoped<ISchInitialLogicalService, SchInitialLogicalService>(); // 排程-初始化-邏輯服務
        services.AddScoped<ISchTimerLogicalService, SchTimerLogicalService>(); // 排程-計時器-邏輯服務
                                                                               // 
                                                                               // BackgroundService

        return services;
    }
}