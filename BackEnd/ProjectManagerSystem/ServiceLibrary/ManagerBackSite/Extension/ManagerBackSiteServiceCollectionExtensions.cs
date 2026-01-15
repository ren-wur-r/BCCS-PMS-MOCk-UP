using System;
using CommonLibrary.CmnExcel.Service;
using CommonLibrary.CmnFireAndForget;
using CommonLibrary.CmnFolderFile.Service;
using CommonLibrary.CmnSMTP.Service;
using Microsoft.Extensions.DependencyInjection;
using ServiceLibrary.ManagerBackSite.Logical.Basic.Service;
using ServiceLibrary.ManagerBackSite.Logical.Common.Service;
using ServiceLibrary.ManagerBackSite.Logical.CRM.Activity.Service;
using ServiceLibrary.ManagerBackSite.Logical.CRM.Phone.Service;
using ServiceLibrary.ManagerBackSite.Logical.CRM.Pipeline.Service;
using ServiceLibrary.ManagerBackSite.Logical.Dashboard.Service;
using ServiceLibrary.ManagerBackSite.Logical.System.Company.Service;
using ServiceLibrary.ManagerBackSite.Logical.System.Contacter.Service;
using ServiceLibrary.ManagerBackSite.Logical.System.Department.Service;
using ServiceLibrary.ManagerBackSite.Logical.System.Employee.Service;
using ServiceLibrary.ManagerBackSite.Logical.System.Product.Service;
using ServiceLibrary.ManagerBackSite.Logical.System.Region.Service;
using ServiceLibrary.ManagerBackSite.Logical.System.Role.Service;
using ServiceLibrary.ManagerBackSite.Logical.Work.Job.Service;
using ServiceLibrary.ManagerBackSite.Logical.Work.Project.Service;
using ServiceLibrary.ManagerBackSite.Logical.PhoneSales.Service;
using ServiceLibrary.ManagerBackSite.Memory.PhoneSales.Service;

namespace ServiceLibrary.ManagerBackSite.Extension;

public static class ManagerBackSiteServiceCollectionExtensions
{
    /// <summary>加入專案-管理者後台</summary>
    public static IServiceCollection AddProjectManagerBackSite(this IServiceCollection services)
    {
        if (services == null)
        {
            throw new ArgumentNullException(nameof(services));
        }

        // common library
        services.AddSingleton<ICmnExcelService, CmnExcelService>(); // 通用-Excel-服務
        services.AddSingleton<ICmnFireAndForgetService, CmnFireAndForgetService>(); // 通用-射後不理服務
        services.AddSingleton<ICmnFolderFileService, CmnFolderFileService>(); // 通用-資料夾檔案-服務
        services.AddScoped<ICmnSmtpService, CmnSmtpService>(); // 通用-SMTP-服務

        // Api

        // hub

        // memory
        services.AddSingleton<IMbsPhsMemoryService, MbsPhsMemoryService>(); // 管理者後台-電銷客戶-記憶體服務

        // lociacl, 排序請依照 Logical 資料夾排序
        services.AddScoped<IMbsBasicLogicalService, MbsBasicLogicalService>(); // 管理者後台-基本-邏輯服務
        services.AddScoped<IMbsCommonLogicalService, MbsCommonLogicalService>(); // 管理者後台-通用-邏輯服務
        services.AddScoped<IMbsSysCmpLogicalService, MbsSysCmpLogicalService>(); // 管理者後台-系統設定-客戶-邏輯服務
        services.AddScoped<IMbsSysCtrLogicalService, MbsSysCtrLogicalService>(); // 管理者後台-客戶名單-客戶窗口-邏輯服務
        services.AddScoped<IMbsCrmActLogicalService, MbsCrmActLogicalService>(); // 管理者後台-CRM-活動管理-邏輯服務
        services.AddScoped<IMbsCrmPhnLogicalService, MbsCrmPhnLogicalService>(); // 管理者後台-CRM-電銷管理-邏輯服務
        services.AddScoped<IMbsCrmPplLogicalService, MbsCrmPplLogicalService>(); // 管理者後台-CRM-商機管理-邏輯服務
        services.AddScoped<IMbsPhsLogicalService, MbsPhsLogicalService>(); // 管理者後台-電銷客戶管理-邏輯服務
        services.AddScoped<IMbsDashboardLogicalService, MbsDashboardLogicalService>(); // 管理者後台-儀表板-邏輯服務
        services.AddScoped<IMbsSysDepartmentLogicalService, MbsSysDepartmentLogicalService>(); // 管理者後台-系統-部門-邏輯服務
        services.AddScoped<IMbsSysEmployeeLogicalService, MbsSysEmployeeLogicalService>(); // 管理者後台-系統-員工-邏輯服務
        services.AddScoped<IMbsSysProductLogicalService, MbsSysProductLogicalService>(); // 管理者後台-系統-產品-邏輯服務
        services.AddScoped<IMbsSysRegionLogicalService, MbsSysRegionLogicalService>(); // 管理者後台-系統-區域-邏輯服務
        services.AddScoped<IMbsSysRoleLogicalService, MbsSysRoleLogicalService>(); // 管理者後台-系統-角色-邏輯服務
        services.AddScoped<IMbsWrkJobLogicalService, MbsWrkJobLogicalService>(); // 管理者後台-工作-工項-邏輯服務
        services.AddScoped<IMbsWrkProjectLogicalService, MbsWrkProjectLogicalService>(); // 管理者後台-工作-專案-邏輯服務

        // BackgroundService


        return services;
    }

}
