using System;
using CommonLibrary.CmnCryptography.CryptoMD5.Service;
using Microsoft.Extensions.DependencyInjection;
using ServiceLibrary.Core.Employee.DB.Info.Service;
using ServiceLibrary.Core.Employee.DB.Permission.Service;
using ServiceLibrary.Core.Manager.DB.Activity.Service;
using ServiceLibrary.Core.Manager.DB.ActivityExpense.Service;
using ServiceLibrary.Core.Manager.DB.ActivityProduct.Service;
using ServiceLibrary.Core.Manager.DB.ActivitySponsor.Service;
using ServiceLibrary.Core.Manager.DB.ActivitySurveyAnswerer.Service;
using ServiceLibrary.Core.Manager.DB.ActivitySurveyQuestion.Service;
using ServiceLibrary.Core.Manager.DB.ActivitySurveyQuestionItem.Service;
using ServiceLibrary.Core.Manager.DB.Company.Service;
using ServiceLibrary.Core.Manager.DB.CompanyAffiliate.Service;
using ServiceLibrary.Core.Manager.DB.CompanyLocation.Service;
using ServiceLibrary.Core.Manager.DB.CompanyMainKind.Service;
using ServiceLibrary.Core.Manager.DB.CompanySubKind.Service;
using ServiceLibrary.Core.Manager.DB.Contacter.Service;
using ServiceLibrary.Core.Manager.DB.ContacterRating.Service;
using ServiceLibrary.Core.Manager.DB.ContacterRatingReason.Service;
using ServiceLibrary.Core.Manager.DB.Department.Service;
using ServiceLibrary.Core.Manager.DB.Product.Service;
using ServiceLibrary.Core.Manager.DB.ProductMainKind.Service;
using ServiceLibrary.Core.Manager.DB.ProductSpecification.Service;
using ServiceLibrary.Core.Manager.DB.ProductSubKind.Service;
using ServiceLibrary.Core.Manager.DB.Region.Service;
using ServiceLibrary.Core.Manager.DB.Role.Service;
using ServiceLibrary.Core.Manager.DB.RolePermission.Service;

namespace ServiceLibrary.Core.Manager.Extension;

/// <summary>核心-管理者-服務擴充</summary>
public static class CoManagerServiceCollectionExtensions
{
    /// <summary>加入-核心-管理者</summary>
    public static IServiceCollection AddCoreManager(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        // common library
        services.AddSingleton<ICmnMd5Service, CmnMd5Service>();

        // db, 排序請依照 DB 
        services.AddScoped<ICoEmpInfoDbService, CoEmpInfoDbService>(); // 核心-員工-資訊-資料庫服務
        services.AddScoped<ICoEmpPermissionDbService, CoEmpPermissionDbService>(); // 核心-員工-權限-資料庫服務
        services.AddScoped<ICoMgrActivityDbService, CoMgrActivityDbService>(); // 核心-管理者-活動-資料庫服務
        services.AddScoped<ICoMgrActivityProductDbService, CoMgrActivityProductDbService>(); // 核心-管理者-活動產品-資料庫服務
        services.AddScoped<ICoMgrActivityExpenseDbService, CoMgrActivityExpenseDbService>(); // 核心-管理者-活動支出-資料庫服務
        services.AddScoped<ICoMgrActivitySponsorDbService, CoMgrActivitySponsorDbService>(); // 核心-管理者-活動贊助-資料庫服務
        services.AddScoped<ICoMgrActivitySurveyAnswererDbService, CoMgrActivitySurveyAnswererDbService>(); // 核心-管理者-活動問卷回答者-資料庫服務
        services.AddScoped<ICoMgrActivitySurveyQuestionDbService, CoMgrActivitySurveyQuestionDbService>(); // 核心-管理者-活動問卷問題-資料庫服務
        services.AddScoped<ICoMgrActivitySurveyQuestionItemDbService, CoMgrActivitySurveyQuestionItemDbService>(); // 核心-管理者-活動問卷問題項目-資料庫服務
        services.AddScoped<ICoMgrCompanyAffiliateDbService, CoMgrCompanyAffiliateDbService>(); // 核心-管理者-關係公司-資料庫服務
        services.AddScoped<ICoMgrCompanyDbService, CoMgrCompanyDbService>(); // 核心-管理者-公司-資料庫服務
        services.AddScoped<ICoMgrCompanyLocationDbService, CoMgrCompanyLocationDbService>(); // 核心-管理者-公司營業地點-資料庫服務
        services.AddScoped<ICoMgrCompanyMainKindDbService, CoMgrCompanyMainKindDbService>(); // 核心-管理者-公司主分類-資料庫服務
        services.AddScoped<ICoMgrCompanySubKindDbService, CoMgrCompanySubKindDbService>(); // 核心-管理者-公司子分類-資料庫服務
        services.AddScoped<ICoMgrContacterDbService, CoMgrContacterDbService>(); // 核心-管理者-窗口-資料庫服務
        services.AddScoped<ICoMgrContacterRatingDbService, CoMgrContacterRatingDbService>(); // 核心-管理者-窗口開發評等-資料庫服務
        services.AddScoped<ICoMgrContacterRatingReasonDbService, CoMgrContacterRatingReasonDbService>(); // 核心-管理者-窗口開發評等原因-資料庫服務
        services.AddScoped<ICoMgrDepartmentDbService, CoMgrDepartmentDbService>(); // 核心-管理者-部門-資料庫服務
        services.AddScoped<ICoMgrProductDbService, CoMgrProductDbService>(); // 核心-管理者-產品-資料庫服務
        services.AddScoped<ICoMgrProductMainKindDbService, CoMgrProductMainKindDbService>(); // 核心-管理者-產品主分類-資料庫服務
        services.AddScoped<ICoMgrProductSpecificationDbService, CoMgrProductSpecificationDbService>(); // 核心-管理者-產品規格-資料庫服務
        services.AddScoped<ICoMgrProductSubKindDbService, CoMgrProductSubKindDbService>(); // 核心-管理者-產品子分類-資料庫服務
        services.AddScoped<ICoMgrRegionDbService, CoMgrRegionDbService>(); // 核心-管理者-地區-資料庫服務
        services.AddScoped<ICoMgrRoleDbService, CoMgrRoleDbService>(); // 核心-管理者-角色-資料庫服務
        services.AddScoped<ICoMgrRolePermissionDbService, CoMgrRolePermissionDbService>(); // 核心-管理者-角色權限-資料庫服務

        // mongo, 排序請依照 Mongo 資料夾排序

        // redis, 排序請依照 Redis 資料夾排序

        // memory, 排序請依照 Memory 資料夾排序

        // logical  

        // operation

        // initial

        return services;
    }

}
