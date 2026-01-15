using System;
using Hangfire;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using ServiceLibrary.ManagerBackSite.Logical.Schedule.Service;
using ServiceLibrary.ManagerBackSite.Memory.PhoneSales.Service;
namespace ServiceLibrary.ManagerBackSite.Extension;

public static class ManagerBackSiteApplicationBuilderExtensions
{
    /// <summary>加入專案-公司後台</summary>
    public static IApplicationBuilder AddProjectManagerBackSite(this IApplicationBuilder app)
    {
        if (app == null)
        {
            throw new ArgumentNullException(nameof(app));
        }

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

        // start up

        #region start up

        var mbsPhsMemoryService = app.ApplicationServices.GetRequiredService<IMbsPhsMemoryService>();
        Console.WriteLine($"MbsPhsMemoryService initialized");

        #endregion

        #region schedule

        // 管理者後台-排程-登出到期時間
        // 間隔: 每0,6,12,18,24,30,36,42,48,54分 第0秒
        RecurringJob.AddOrUpdate<IMbsScheduleLogicalService>(
            $"{nameof(IMbsScheduleLogicalService)}.{nameof(IMbsScheduleLogicalService.LogoutExpiredTimeAsync)}",
            x => x.LogoutExpiredTimeAsync(),
            "0 0,6,12,18,24,30,36,42,48,54 * ? * *",
            new RecurringJobOptions()
            {
                TimeZone = TimeZoneInfo.Utc,
            });

        #endregion

        return app;
    }
}
