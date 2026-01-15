using System;
using CommonLibrary.CmnCryptography.CryptoMD5.Service;
using Microsoft.Extensions.DependencyInjection;
using ServiceLibrary.Core.Employee.DB.Info.Service;
using ServiceLibrary.Core.Employee.DB.Permission.Service;
using ServiceLibrary.Core.Employee.DB.Pipeline.Service;
using ServiceLibrary.Core.Employee.DB.PipelineBill.Service;
using ServiceLibrary.Core.Employee.DB.PipelineBooking.Service;
using ServiceLibrary.Core.Employee.DB.PipelineContacter.Service;
using ServiceLibrary.Core.Employee.DB.PipelineDue.Service;
using ServiceLibrary.Core.Employee.DB.PipelineOriginal.Service;
using ServiceLibrary.Core.Employee.DB.PipelinePhone.Service;
using ServiceLibrary.Core.Employee.DB.PipelineProduct.Service;
using ServiceLibrary.Core.Employee.DB.PipelineSaler.Service;
using ServiceLibrary.Core.Employee.DB.PipelineSalerTracking.Service;
using ServiceLibrary.Core.Employee.DB.Project.Service;
using ServiceLibrary.Core.Employee.DB.ProjectContract.Service;
using ServiceLibrary.Core.Employee.DB.ProjectExpense.Service;
using ServiceLibrary.Core.Employee.DB.ProjectMember.Service;
using ServiceLibrary.Core.Employee.DB.ProjectStone.Service;
using ServiceLibrary.Core.Employee.DB.ProjectStoneJob.Service;
using ServiceLibrary.Core.Employee.DB.ProjectStoneJobBucket.Service;
using ServiceLibrary.Core.Employee.DB.ProjectStoneJobExecutor.Service;
using ServiceLibrary.Core.Employee.DB.ProjectStoneJobWork.Service;
using ServiceLibrary.Core.Employee.DB.ProjectStoneJobWorkFile.Service;
using ServiceLibrary.Core.Employee.DB.ProjectWork.Service;
using ServiceLibrary.Core.Employee.Logical.Service;
using ServiceLibrary.Core.Employee.Memory.LoginInfo.Service;

namespace ServiceLibrary.Core.Employee.Extension;

/// <summary>核心-員工-服務擴充</summary>
public static class CoEmployeeServiceCollectionExtensions
{
    /// <summary>加入-核心-員工</summary>
    public static IServiceCollection AddCoreEmployee(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        // common library
        services.AddSingleton<ICmnMd5Service, CmnMd5Service>();

        // db, 排序請依照 DB 資料夾排序
        services.AddScoped<ICoEmpInfoDbService, CoEmpInfoDbService>(); // 核心-員工-資訊-資料庫服務
        services.AddScoped<ICoEmpPermissionDbService, CoEmpPermissionDbService>(); // 核心-員工-權限-資料庫服務
        services.AddScoped<ICoEmpPipelineDbService, CoEmpPipelineDbService>(); // 核心-員工-商機-資料庫服務
        services.AddScoped<ICoEmpPipelineBillDbService, CoEmpPipelineBillDbService>(); // 核心-員工-商機發票紀錄-資料庫服務
        services.AddScoped<ICoEmpPipelineBookingDbService, CoEmpPipelineBookingDbService>(); // 核心-員工-商機Booking-資料庫服務
        services.AddScoped<ICoEmpPipelineContacterDbService, CoEmpPipelineContacterDbService>(); // 核心-員工-商機窗口-資料庫服務
        services.AddScoped<ICoEmpPipelineDueDbService, CoEmpPipelineDueDbService>(); // 核心-員工-商機履約期限-資料庫服務
        services.AddScoped<ICoEmpPipelineOriginalDbService, CoEmpPipelineOriginalDbService>(); // 核心-員工-商機原始資料-資料庫服務
        services.AddScoped<ICoEmpPipelinePhoneDbService, CoEmpPipelinePhoneDbService>(); // 核心-員工-商機電銷-資料庫服務
        services.AddScoped<ICoEmpPipelineProductDbService, CoEmpPipelineProductDbService>(); // 核心-員工-商機產品-資料庫服務
        services.AddScoped<ICoEmpPipelineSalerDbService, CoEmpPipelineSalerDbService>(); // 核心-員工-商機業務-資料庫服務
        services.AddScoped<ICoEmpPipelineSalerTrackingDbService, CoEmpPipelineSalerTrackingDbService>(); // 核心-員工-商機業務開發紀錄-資料庫服務
        services.AddScoped<ICoEmpProjectDbService, CoEmpProjectDbService>(); // 核心-員工-專案-資料庫服務
        services.AddScoped<ICoEmpProjectContractDbService, CoEmpProjectContractDbService>(); // 核心-員工-專案合約-資料庫服務
        services.AddScoped<ICoEmpProjectExpenseDbService, CoEmpProjectExpenseDbService>(); // 核心-員工-專案支出-資料庫服務
        services.AddScoped<ICoEmpProjectMemberDbService, CoEmpProjectMemberDbService>(); // 核心-員工-專案成員-資料庫服務
        services.AddScoped<ICoEmpProjectStoneDbService, CoEmpProjectStoneDbService>(); // 核心-員工-專案里程碑-資料庫服務
        services.AddScoped<ICoEmpProjectStoneJobDbService, CoEmpProjectStoneJobDbService>(); // 核心-員工-專案里程碑工項-資料庫服務
        services.AddScoped<ICoEmpProjectStoneJobBucketDbService, CoEmpProjectStoneJobBucketDbService>(); // 核心-員工-專案里程碑工項清單-資料庫服務
        services.AddScoped<ICoEmpProjectStoneJobExecutorDbService, CoEmpProjectStoneJobExecutorDbService>(); // 核心-員工-專案里程碑工項執行者-資料庫服務
        services.AddScoped<ICoEmpProjectStoneJobWorkDbService, CoEmpProjectStoneJobWorkDbService>(); // 核心-員工-專案里程碑工項工作-資料庫服務
        services.AddScoped<ICoEmpProjectStoneJobWorkFileDbService, CoEmpProjectStoneJobWorkFileDbService>(); // 核心-員工-專案里程碑工項工作檔案-資料庫服務
        services.AddScoped<ICoEmpProjectWorkDbService, CoEmpProjectWorkDbService>(); // 核心-員工-專案工作計劃書-資料庫服務

        // mongo, 排序請依照 Mongo 資料夾排序

        // redis, 排序請依照 Redis 資料夾排序

        // memory, 排序請依照 Memory 資料夾排序
        services.AddSingleton<ICoEmpLoginInfoMemoryService, CoEmpLoginInfoMemoryService>(); // 核心-員工-登入資訊-記憶體服務

        // logical, 排序請依照 Logical 資料夾排序
        services.AddSingleton<ICoEmployeeLogicalService, CoEmployeeLogicalService>(); // 核心-員工-邏輯服務

        // operation

        // initial

        return services;
    }
}