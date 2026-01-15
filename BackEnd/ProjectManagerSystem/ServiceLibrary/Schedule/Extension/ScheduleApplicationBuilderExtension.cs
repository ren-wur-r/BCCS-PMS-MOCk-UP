using System;
using Hangfire;
using Microsoft.AspNetCore.Builder;
using ServiceLibrary.Schedule.Logical.Initial.Service;
using ServiceLibrary.Schedule.Logical.Timer.Service;

namespace ServiceLibrary.Schedule.Extension;

public static class ScheduleApplicationBuilderExtentions
{
    /// <summary>加入專案-排程</summary>
    public static IApplicationBuilder AddProjectSchedule(this IApplicationBuilder app)
    {
        ArgumentNullException.ThrowIfNull(app);

        // Hangfire
        /*
         * 1. Cron expression
         * https://www.freeformatter.com/cron-expression-generator-quartz.html
         * 要去除"年"
         * 
         * 
         * hangfire login with web api 參考資料：
         * https://www.talkingdotnet.com/integrate-hangfire-with-asp-net-core-web-api/
         * https://medium.com/ricos-note/hangfire-dashboard-of-authorization-b41d7135b044
         */

        #region startup

        var jobStart = BackgroundJob.Enqueue<ISchInitialLogicalService>(x => x.StartJobAsync());
        var jobInitRegion = BackgroundJob.ContinueJobWith<ISchInitialLogicalService>(jobStart, x => x.InitializeRegionAsync());
        var jobInitDepartment = BackgroundJob.ContinueJobWith<ISchInitialLogicalService>(jobInitRegion, x => x.InitializeDepartmentAsync());
        var jobInitCompanyBccs = BackgroundJob.ContinueJobWith<ISchInitialLogicalService>(jobInitDepartment, x => x.InitializeCompanyBccsAsync());
        //var jobInitEmployeeAdmin = BackgroundJob.ContinueJobWith<ISchInitialLogicalService>(jobInitCompanyBccs, x => x.InitializeEmployeeAdminAsync());
        var jobInitEmployeeGeneralManager = BackgroundJob.ContinueJobWith<ISchInitialLogicalService>(jobInitCompanyBccs, x => x.InitialEmployeeGeneralManagerAsync());
        var jobEnd = BackgroundJob.ContinueJobWith<ISchInitialLogicalService>(jobInitEmployeeGeneralManager, x => x.EndJobAsync());

        #endregion

        #region schedule

        // 間隔: 每天，16時10分0秒 (0 10 16 ? * *)，台灣時間 00:10:00
        RecurringJob.AddOrUpdate<ISchTimerLogicalService>(
            $"{nameof(ISchTimerLogicalService)}.{nameof(ISchTimerLogicalService.UpdateProjectStatusAsync)}",
            x => x.UpdateProjectStatusAsync(),
            "0 10 16 ? * *",
            new RecurringJobOptions()
            {
                TimeZone = TimeZoneInfo.Utc,
            });

        // 間隔: 每天，16時30分0秒 (0 30 16 ? * *)，台灣時間 00:30:00
        RecurringJob.AddOrUpdate<ISchTimerLogicalService>(
            $"{nameof(ISchTimerLogicalService)}.{nameof(ISchTimerLogicalService.NotifyAbnormalProjectStatusAsync)}",
            x => x.NotifyAbnormalProjectStatusAsync(),
            "0 30 16 ? * *",
            new RecurringJobOptions()
            {
                TimeZone = TimeZoneInfo.Utc,
            });

        #endregion

        return app;
    }
}
