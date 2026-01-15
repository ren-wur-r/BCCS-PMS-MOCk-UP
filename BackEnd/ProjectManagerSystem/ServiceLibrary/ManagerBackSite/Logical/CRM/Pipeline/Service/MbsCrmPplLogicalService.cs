using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using ApiModelLibrary.ManagerBackSite.Common;
using CommonLibrary.CmnEnum.Extension;
using CommonLibrary.CmnExcel.Service;
using CommonLibrary.CmnFolderFile.Service;
using CommonLibrary.CmnSMTP.Config;
using CommonLibrary.CmnSMTP.Format;
using CommonLibrary.CmnSMTP.Service;
using DataModelLibrary.Database.AtomEmployeePipelineBill;
using DataModelLibrary.Database.AtomEmployeePipelineSaler;
using DataModelLibrary.Database.AtomEmployeeProjectStatus;
using DataModelLibrary.Database.AtomManagerContacter;
using DataModelLibrary.Database.AtomMenu;
using DataModelLibrary.Database.AtomPermissionKind;
using DataModelLibrary.Database.AtomPipeline;
using DataModelLibrary.Database.Employee;
using DataModelLibrary.Database.EmployeeProjectMember;
using DataModelLibrary.Database.EmployeePipeline;
using DataModelLibrary.GlobalSystem.Config;
using DataModelLibrary.GlobalSystem.Env;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using ServiceLibrary.Core.Employee.DB.Info.Format;
using ServiceLibrary.Core.Employee.DB.Info.Service;
using ServiceLibrary.Core.Employee.DB.Pipeline.Format;
using ServiceLibrary.Core.Employee.DB.Pipeline.Service;
using ServiceLibrary.Core.Employee.DB.PipelineBill.Format;
using ServiceLibrary.Core.Employee.DB.PipelineBill.Service;
using ServiceLibrary.Core.Employee.DB.PipelineBooking.Service;
using ServiceLibrary.Core.Employee.DB.PipelineContacter.Format;
using ServiceLibrary.Core.Employee.DB.PipelineContacter.Service;
using ServiceLibrary.Core.Employee.DB.PipelineDue.Format;
using ServiceLibrary.Core.Employee.DB.PipelineDue.Service;
using ServiceLibrary.Core.Employee.DB.PipelinePhone.Service;
using ServiceLibrary.Core.Employee.DB.PipelineProduct.Format;
using ServiceLibrary.Core.Employee.DB.PipelineProduct.Service;
using ServiceLibrary.Core.Employee.DB.PipelineSaler.Format;
using ServiceLibrary.Core.Employee.DB.PipelineSaler.Service;
using ServiceLibrary.Core.Employee.DB.PipelineSalerTracking.Format;
using ServiceLibrary.Core.Employee.DB.PipelineSalerTracking.Service;
using ServiceLibrary.Core.Employee.DB.Project.Format;
using ServiceLibrary.Core.Employee.DB.Project.Service;
using ServiceLibrary.Core.Employee.DB.ProjectContract.Format;
using ServiceLibrary.Core.Employee.DB.ProjectContract.Service;
using ServiceLibrary.Core.Employee.DB.ProjectMember.Format;
using ServiceLibrary.Core.Employee.DB.ProjectMember.Service;
using ServiceLibrary.Core.Employee.DB.ProjectWork.Format;
using ServiceLibrary.Core.Employee.DB.ProjectWork.Service;
using ServiceLibrary.Core.Employee.Logical.Service;
using ServiceLibrary.Core.Manager.DB.Activity.Format;
using ServiceLibrary.Core.Manager.DB.Activity.Service;
using ServiceLibrary.Core.Manager.DB.ActivityExpense.Service;
using ServiceLibrary.Core.Manager.DB.ActivityProduct.Service;
using ServiceLibrary.Core.Manager.DB.ActivitySponsor.Service;
using ServiceLibrary.Core.Manager.DB.ActivitySurveyQuestion.Service;
using ServiceLibrary.Core.Manager.DB.ActivitySurveyQuestionItem.Service;
using ServiceLibrary.Core.Manager.DB.Company.Format;
using ServiceLibrary.Core.Manager.DB.Company.Service;
using ServiceLibrary.Core.Manager.DB.CompanyLocation.Format;
using ServiceLibrary.Core.Manager.DB.CompanyLocation.Service;
using ServiceLibrary.Core.Manager.DB.CompanyMainKind.Format;
using ServiceLibrary.Core.Manager.DB.CompanyMainKind.Service;
using ServiceLibrary.Core.Manager.DB.CompanySubKind.Format;
using ServiceLibrary.Core.Manager.DB.CompanySubKind.Service;
using ServiceLibrary.Core.Manager.DB.Contacter.Format;
using ServiceLibrary.Core.Manager.DB.Contacter.Service;
using ServiceLibrary.Core.Manager.DB.Department.Format;
using ServiceLibrary.Core.Manager.DB.Department.Service;
using ServiceLibrary.Core.Manager.DB.Product.Format;
using ServiceLibrary.Core.Manager.DB.Product.Service;
using ServiceLibrary.Core.Manager.DB.ProductMainKind.Format;
using ServiceLibrary.Core.Manager.DB.ProductMainKind.Service;
using ServiceLibrary.Core.Manager.DB.ProductSpecification.Format;
using ServiceLibrary.Core.Manager.DB.ProductSpecification.Service;
using ServiceLibrary.Core.Manager.DB.ProductSubKind.Format;
using ServiceLibrary.Core.Manager.DB.ProductSubKind.Service;
using ServiceLibrary.Core.Manager.DB.Region.Format;
using ServiceLibrary.Core.Manager.DB.Region.Service;
using ServiceLibrary.Core.Manager.DB.Role.Format;
using ServiceLibrary.Core.Manager.DB.Role.Service;
using ServiceLibrary.ManagerBackSite.Logical.Common.Format;
using ServiceLibrary.ManagerBackSite.Logical.Common.Service;
using ServiceLibrary.ManagerBackSite.Logical.CRM.Pipeline.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.CRM.Pipeline.Service;

/// <summary>管理者後台-CRM-商機管理-邏輯服務</summary>
public class MbsCrmPplLogicalService : IMbsCrmPplLogicalService
{
    /// <summary>logger</summary>
    private readonly ILogger<MbsCrmPplLogicalService> _logger;

    /// <summary>主機環境</summary>
    private readonly IWebHostEnvironment _env;

    #region CommonLibrary

    /// <summary>通用-Excel-服務</summary>
    private readonly ICmnExcelService _cmnExcel;

    /// <summary>通用-資料夾檔案-服務</summary>
    private readonly ICmnFolderFileService _cmnFolderFile;

    /// <summary>通用-SMTP-服務</summary>
    private readonly ICmnSmtpService _cmnSmtp;

    #endregion

    #region GlobalSystem

    /// <summary>全系統-發票設定</summary>
    private readonly GsBillConfig _gsBillConfig;

    /// <summary>全系統-平台設定</summary>
    public GsPlatformConfig _gsPlatformConfig { get; set; }

    /// <summary>全系統-郵件設定</summary>
    public GsSmtpConfig _gsSmtpConfig { get; set; }

    #endregion

    #region Core Employee

    /// <summary>核心-員工-資訊-資料庫服務</summary>
    private readonly ICoEmpInfoDbService _coEmpInfoDb;

    /// <summary>核心-員工-商機-資料庫服務</summary>
    private readonly ICoEmpPipelineDbService _coEmpPipelineDb;

    /// <summary>核心-員工-商機發票紀錄-資料庫服務</summary>
    private readonly ICoEmpPipelineBillDbService _coEmpPipelineBillDb;

    /// <summary>核心-員工-商機Booking-資料庫服務</summary>
    private readonly ICoEmpPipelineBookingDbService _coEmpPipelineBookingDb;

    /// <summary>核心-員工-商機窗口-資料庫服務</summary>
    private readonly ICoEmpPipelineContacterDbService _coEmpPipelineContacterDb;

    /// <summary>核心-員工-商機履約期限-資料庫服務</summary>
    private readonly ICoEmpPipelineDueDbService _coEmpPipelineDueDb;

    /// <summary>核心-員工-商機電銷紀錄-資料庫服務</summary>
    private readonly ICoEmpPipelinePhoneDbService _coEmpPipelinePhoneDb;

    /// <summary>核心-員工-商機產品-資料庫服務</summary>
    private readonly ICoEmpPipelineProductDbService _coEmpPipelineProductDb;

    /// <summary>核心-員工-商機業務-資料庫服務</summary>
    private readonly ICoEmpPipelineSalerDbService _coEmpPipelineSalerDb;

    /// <summary>核心-員工-商機業務開發紀錄-資料庫服務</summary>
    private readonly ICoEmpPipelineSalerTrackingDbService _coEmpPipelineSalerTrackingDb;

    /// <summary>核心-員工-商機專案-資料庫服務</summary>
    private readonly ICoEmpProjectDbService _coEmpProjectDb;

    /// <summary>核心-員工-專案契約-資料庫服務</summary>
    private readonly ICoEmpProjectContractDbService _coEmpProjectContractDb;

    /// <summary>核心-員工-專案成員-資料庫服務</summary>
    private readonly ICoEmpProjectMemberDbService _coEmpProjectMemberDb;

    /// <summary>核心-員工-專案工作計畫書-資料庫服務</summary>
    private readonly ICoEmpProjectWorkDbService _coEmpProjectWorkDb;

    #endregion

    #region Core Manager

    /// <summary>核心-管理者-活動-資料庫-服務</summary>
    private readonly ICoMgrActivityDbService _coMgrActivityDb;

    /// <summary>核心-管理者-活動支出-資料庫-服務</summary>
    private readonly ICoMgrActivityExpenseDbService _coMgrActivityExpenseDb;

    /// <summary>核心-管理者-活動產品-資料庫-服務</summary>
    private readonly ICoMgrActivityProductDbService _coMgrActivityProductDb;

    /// <summary>核心-管理者-活動贊助-資料庫-服務</summary>
    private readonly ICoMgrActivitySponsorDbService _coMgrActivitySponsorDb;

    /// <summary>核心-管理者-活動問卷問題-資料庫-服務</summary>
    private readonly ICoMgrActivitySurveyQuestionDbService _coMgrActivitySurveyQuestionDb;

    /// <summary>核心-管理者-活動問卷問題項目-資料庫-服務</summary>
    private readonly ICoMgrActivitySurveyQuestionItemDbService _coMgrActivitySurveyQuestionItemDb;

    /// <summary>核心-管理者-公司-資料庫-服務</summary>
    private readonly ICoMgrCompanyDbService _coMgrCompanyDb;

    /// <summary>核心-管理者-公司營業地點-資料庫-服務</summary>
    private readonly ICoMgrCompanyLocationDbService _coMgrCompanyLocationDb;

    /// <summary>核心-管理者-公司主分類-資料庫-服務</summary>
    private readonly ICoMgrCompanyMainKindDbService _coMgrCompanyMainKindDb;

    /// <summary>核心-管理者-公司子分類-資料庫-服務</summary>
    private readonly ICoMgrCompanySubKindDbService _coMgrCompanySubKindDb;

    /// <summary>核心-管理者-系統設定-窗口-資料庫-服務</summary>
    private readonly ICoMgrContacterDbService _coMgrContacterDb;

    /// <summary>核心-管理者-產品-資料庫-服務</summary>
    private readonly ICoMgrProductDbService _coMgrProductDb;

    /// <summary>核心-管理者-產品主分類-資料庫-服務</summary>
    private readonly ICoMgrProductMainKindDbService _coMgrProductMainKindDb;

    /// <summary>核心-管理者-產品規格-資料庫-服務</summary>
    private readonly ICoMgrProductSpecificationDbService _coMgrProductSpecificationDb;

    /// <summary>核心-管理者-產品子分類-資料庫-服務</summary>
    private readonly ICoMgrProductSubKindDbService _coMgrProductSubKindDb;

    /// <summary>核心-管理者-區域-資料庫服務</summary>
    private readonly ICoMgrRegionDbService _coMgrRgnDb;

    /// <summary>核心-管理者-角色-資料庫服務</summary>
    private readonly ICoMgrRoleDbService _coMgrRolDb;

    /// <summary>核心-管理者-部門-資料庫服務</summary>
    private readonly ICoMgrDepartmentDbService _coMgrDptDb;

    #endregion

    #region Core Logical

    /// <summary>核心-管理者-邏輯-服務</summary>
    private readonly ICoEmployeeLogicalService _coEmployeeLogical;

    #endregion

    /// <summary>管理者後台-通用-邏輯-服務</summary>
    private readonly IMbsCommonLogicalService _mbsCommonLogical;

    #region This

    /// <summary>原子-管理者後台目錄-列舉</summary>
    private readonly DbAtomMenuEnum _atomMenu;

    #endregion

    /// <summary>建構子</summary>
    public MbsCrmPplLogicalService(
        ILogger<MbsCrmPplLogicalService> logger,
        IWebHostEnvironment env,
        // CommonLibrary
        ICmnExcelService cmnExcel,
        ICmnFolderFileService cmnFolderFile,
        ICmnSmtpService cmnSmtp,
        // GlobalSystem
        GsBillConfig gsBillConfig,
        GsPlatformConfig gsPlatformConfig,
        GsSmtpConfig gsSmtpConfig,
        // Core Employee
        ICoEmpInfoDbService coEmpInfoDb,
        ICoEmpPipelineDbService coEmpPipelineDb,
        ICoEmpPipelineBillDbService coEmpPipelineBillDb,
        ICoEmpPipelineBookingDbService coEmpPipelineBookingDb,
        ICoEmpPipelineContacterDbService coEmpPipelineContacterDb,
        ICoEmpPipelineDueDbService coEmpPipelineDueDb,
        ICoEmpPipelinePhoneDbService coEmpPipelinePhoneDb,
        ICoEmpPipelineProductDbService coEmpPipelineProductDb,
        ICoEmpPipelineSalerDbService coEmpPipelineSalerDb,
        ICoEmpPipelineSalerTrackingDbService coEmpPipelineSalerTrackingDb,
        ICoEmpProjectDbService coEmpProjectDb,
        ICoEmpProjectContractDbService coEmpProjectContractDb,
        ICoEmpProjectMemberDbService coEmpProjectMemberDb,
        ICoEmpProjectWorkDbService coEmpProjectWorkDb,
        // Core Manager
        ICoMgrActivityDbService coMgrActivityDb,
        ICoMgrActivityExpenseDbService coMgrActivityExpenseDb,
        ICoMgrActivityProductDbService coMgrActivityProductDb,
        ICoMgrActivitySponsorDbService coMgrActivitySponsorDb,
        ICoMgrActivitySurveyQuestionDbService coMgrActivitySurveyQuestionDb,
        ICoMgrActivitySurveyQuestionItemDbService coMgrActivitySurveyQuestionItemDb,
        ICoMgrCompanyDbService coMgrCompanyDb,
        ICoMgrCompanyLocationDbService coMgrCompanyLocationDb,
        ICoMgrCompanyMainKindDbService coMgrCompanyMainKindDb,
        ICoMgrCompanySubKindDbService coMgrCompanySubKindDb,
        ICoMgrContacterDbService coMgrContacterDb,
        ICoMgrProductDbService coMgrProductDb,
        ICoMgrProductMainKindDbService coMgrProductMainKindDb,
        ICoMgrProductSpecificationDbService coMgrProductSpecificationDb,
        ICoMgrProductSubKindDbService coMgrProductSubKindDb,
        ICoMgrRegionDbService coMgrRgnDb,
        ICoMgrRoleDbService coMgrRolDb,
        ICoMgrDepartmentDbService coMgrDptDb,
        // Core Logical
        ICoEmployeeLogicalService coEmployeeLogical,
        // ManagerBackSite
        IMbsCommonLogicalService mbsCommonLogical)
    {
        this._logger = logger;
        this._env = env;
        // CommonLibrary
        this._cmnExcel = cmnExcel;
        this._cmnFolderFile = cmnFolderFile;
        this._cmnSmtp = cmnSmtp;
        // GlobalSystem
        this._gsBillConfig = gsBillConfig;
        this._gsPlatformConfig = gsPlatformConfig;
        this._gsSmtpConfig = gsSmtpConfig;
        // Core Employee
        this._coEmpInfoDb = coEmpInfoDb;
        this._coEmpPipelineDb = coEmpPipelineDb;
        this._coEmpPipelineDueDb = coEmpPipelineDueDb;
        this._coEmpPipelineBillDb = coEmpPipelineBillDb;
        this._coEmpPipelineBookingDb = coEmpPipelineBookingDb;
        this._coEmpPipelineContacterDb = coEmpPipelineContacterDb;
        this._coEmpPipelinePhoneDb = coEmpPipelinePhoneDb;
        this._coEmpPipelineProductDb = coEmpPipelineProductDb;
        this._coEmpPipelineSalerDb = coEmpPipelineSalerDb;
        this._coEmpPipelineSalerTrackingDb = coEmpPipelineSalerTrackingDb;
        this._coEmpProjectDb = coEmpProjectDb;
        this._coEmpProjectContractDb = coEmpProjectContractDb;
        this._coEmpProjectMemberDb = coEmpProjectMemberDb;
        this._coEmpProjectWorkDb = coEmpProjectWorkDb;
        // Core Manager
        this._coMgrActivityDb = coMgrActivityDb;
        this._coMgrActivityExpenseDb = coMgrActivityExpenseDb;
        this._coMgrActivityProductDb = coMgrActivityProductDb;
        this._coMgrActivitySponsorDb = coMgrActivitySponsorDb;
        this._coMgrActivitySurveyQuestionDb = coMgrActivitySurveyQuestionDb;
        this._coMgrActivitySurveyQuestionItemDb = coMgrActivitySurveyQuestionItemDb;
        this._coMgrCompanyDb = coMgrCompanyDb;
        this._coMgrCompanyLocationDb = coMgrCompanyLocationDb;
        this._coMgrCompanyMainKindDb = coMgrCompanyMainKindDb;
        this._coMgrCompanySubKindDb = coMgrCompanySubKindDb;
        this._coMgrContacterDb = coMgrContacterDb;
        this._coMgrProductDb = coMgrProductDb;
        this._coMgrProductMainKindDb = coMgrProductMainKindDb;
        this._coMgrProductSpecificationDb = coMgrProductSpecificationDb;
        this._coMgrProductSubKindDb = coMgrProductSubKindDb;
        this._coMgrRgnDb = coMgrRgnDb;
        this._coMgrRolDb = coMgrRolDb;
        this._coMgrDptDb = coMgrDptDb;
        // Core Logical
        this._coEmployeeLogical = coEmployeeLogical;
        // ManagerBackSite
        this._mbsCommonLogical = mbsCommonLogical;

        // This
        this._atomMenu = DbAtomMenuEnum.CrmPipeline;
    }

    #region PipelineProduct 商機產品

    /// <summary>管理者後台-CRM-商機管理-邏輯服務-取得商機產品</summary>
    public async Task<MbsCrmPplLgcGetEmployeePipelineProductRspMdl> GetEmployeePipelineProductAsync(MbsCrmPplLgcGetEmployeePipelineProductReqMdl reqObj)
    {
        var rspObj = new MbsCrmPplLgcGetEmployeePipelineProductRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        #region 判斷登入令牌與權限

        // mbs, 管理者後台-通用-邏輯-取得登入使用者資訊
        var mbsCmnLgcGetLoginUserInfoReqObj = new MbsCmnLgcGetLoginUserInfoReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken,
            AtomMenu = this._atomMenu,
        };
        var mbsCmnLgcGetLoginUserInfoRspObj = await this._mbsCommonLogical.GetLoginUserInfoAsync(mbsCmnLgcGetLoginUserInfoReqObj);
        if (mbsCmnLgcGetLoginUserInfoRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }
        if (mbsCmnLgcGetLoginUserInfoRspObj.ErrorCode != MbsErrorCodeEnum.Success)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            rspObj.ErrorCode = mbsCmnLgcGetLoginUserInfoRspObj.ErrorCode;
            return rspObj;
        }

        // 判斷權限
        if (mbsCmnLgcGetLoginUserInfoRspObj.AtomPermissionKindIdView != DbAtomPermissionKindEnum.Self &&
            mbsCmnLgcGetLoginUserInfoRspObj.AtomPermissionKindIdView != DbAtomPermissionKindEnum.Everyone)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");

            // 權限不足
            rspObj.ErrorCode = MbsErrorCodeEnum.PermissionDenied;
            return rspObj;
        }

        #endregion

        // db, 核心-員工-商機產品-資料庫-取得
        var coEmpPplPrdDbGetReqObj = new CoEmpPplPrdDbGetReqMdl()
        {
            EmployeePipelineProductID = reqObj.EmployeePipelineProductID,
        };
        var coEmpPplPrdDbGetRspObj = await this._coEmpPipelineProductDb.GetAsync(coEmpPplPrdDbGetReqObj);
        if (coEmpPplPrdDbGetRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // db, 核心-員工-商機-資料庫-取得
        var coEmpPplDbGetReqObj = new CoEmpPplDbGetReqMdl()
        {
            EmployeePipelineID = coEmpPplPrdDbGetRspObj.EmployeePipelineID,
        };
        var coEmpPplDbGetRspObj = await this._coEmpPipelineDb.GetAsync(coEmpPplDbGetReqObj);
        if (coEmpPplDbGetRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        if (coEmpPplDbGetRspObj.ManagerCompanyID == null)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // db, 核心-管理者-產品-資料庫-取得
        var coMgrPrdDbGetReqObj = new CoMgrPrdDbGetReqMdl()
        {
            ManagerProductID = coEmpPplPrdDbGetRspObj.ManagerProductID,
        };
        var coMgrPrdDbGetRspObj = await this._coMgrProductDb.GetAsync(coMgrPrdDbGetReqObj);
        if (coMgrPrdDbGetRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // db, 核心-管理者-產品主分類-資料庫-取得
        var coMgrPmkDbGetReqObj = new CoMgrPmkDbGetReqMdl()
        {
            ManagerProductMainKindID = coMgrPrdDbGetRspObj.ManagerProductMainKindID,
        };
        var coMgrPmkDbGetRspObj = await this._coMgrProductMainKindDb.GetAsync(coMgrPmkDbGetReqObj);
        if (coMgrPmkDbGetRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // db, 核心-管理者-產品子分類-資料庫-取得
        var coMgrPskDbGetReqObj = new CoMgrPskDbGetReqMdl()
        {
            ManagerProductSubKindID = coMgrPrdDbGetRspObj.ManagerProductSubKindID,
        };
        var coMgrPskDbGetRspObj = await this._coMgrProductSubKindDb.GetAsync(coMgrPskDbGetReqObj);
        if (coMgrPskDbGetRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // db, 核心-管理者-產品規格-資料庫-取得
        var coMgrPsDbGetReqObj = new CoMgrPsDbGetReqMdl()
        {
            ManagerProductSpecificationID = coEmpPplPrdDbGetRspObj.ManagerProductSpecificationID,
        };
        var coMgrPsDbGetRspObj = await this._coMgrProductSpecificationDb.GetAsync(coMgrPsDbGetReqObj);
        if (coMgrPsDbGetRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        rspObj.EmployeePipelineID = coEmpPplPrdDbGetRspObj.EmployeePipelineID;
        rspObj.ManagerProductMainKindID = coMgrPrdDbGetRspObj.ManagerProductMainKindID;
        rspObj.ManagerProductMainKindName = coMgrPmkDbGetRspObj.ManagerProductMainKindName;
        rspObj.ManagerProductSubKindID = coMgrPrdDbGetRspObj.ManagerProductSubKindID;
        rspObj.ManagerProductSubKindName = coMgrPskDbGetRspObj.ManagerProductSubKindName;
        rspObj.ManagerProductID = coEmpPplPrdDbGetRspObj.ManagerProductID;
        rspObj.ManagerProductName = coMgrPrdDbGetRspObj.ManagerProductName;
        rspObj.ManagerProductSpecificationID = coEmpPplPrdDbGetRspObj.ManagerProductSpecificationID;
        rspObj.ManagerProductSpecificationName = coMgrPsDbGetRspObj.ManagerProductSpecificationName;
        rspObj.EmployeePipelineProductSellPrice = coEmpPplPrdDbGetRspObj.EmployeePipelineProductSellPrice;
        rspObj.EmployeePipelineProductClosingPrice = coEmpPplPrdDbGetRspObj.EmployeePipelineProductClosingPrice;
        rspObj.EmployeePipelineProductCostPrice = coEmpPplPrdDbGetRspObj.EmployeePipelineProductCostPrice;
        rspObj.EmployeePipelineProductCount = coEmpPplPrdDbGetRspObj.EmployeePipelineProductCount;
        rspObj.EmployeePipelineProductPurchaseKindID = coEmpPplPrdDbGetRspObj.EmployeePipelineProductPurchaseKindID;
        rspObj.EmployeePipelineProductContractKindID = coEmpPplPrdDbGetRspObj.EmployeePipelineProductContractKindID;
        rspObj.EmployeePipelineProductContractText = coEmpPplPrdDbGetRspObj.EmployeePipelineProductContractText;
        return rspObj;
    }

    /// <summary>管理者後台-CRM-商機管理-邏輯服務-新增商機產品</summary>
    public async Task<MbsCrmPplLgcAddEmployeePipelineProductRspMdl> AddEmployeePipelineProductAsync(MbsCrmPplLgcAddEmployeePipelineProductReqMdl reqObj)
    {
        var rspObj = new MbsCrmPplLgcAddEmployeePipelineProductRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        #region 判斷登入令牌與權限

        // mbs, 管理者後台-通用-邏輯-取得登入使用者資訊
        var mbsCmnLgcGetLoginUserInfoReqObj = new MbsCmnLgcGetLoginUserInfoReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken,
            AtomMenu = this._atomMenu,
        };
        var mbsCmnLgcGetLoginUserInfoRspObj = await this._mbsCommonLogical.GetLoginUserInfoAsync(mbsCmnLgcGetLoginUserInfoReqObj);
        if (mbsCmnLgcGetLoginUserInfoRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }
        if (mbsCmnLgcGetLoginUserInfoRspObj.ErrorCode != MbsErrorCodeEnum.Success)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            rspObj.ErrorCode = mbsCmnLgcGetLoginUserInfoRspObj.ErrorCode;
            return rspObj;
        }

        // 判斷權限
        if (mbsCmnLgcGetLoginUserInfoRspObj.AtomPermissionKindIdCreate != DbAtomPermissionKindEnum.Self &&
            mbsCmnLgcGetLoginUserInfoRspObj.AtomPermissionKindIdCreate != DbAtomPermissionKindEnum.Everyone)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");

            // 權限不足
            rspObj.ErrorCode = MbsErrorCodeEnum.PermissionDenied;
            return rspObj;
        }

        #endregion

        // db, 核心-員工-商機產品-資料庫-是否存在
        var coEmpPplPrdDbExistReqObj = new CoEmpPplPrdDbExistReqMdl()
        {
            EmployeePipelineID = reqObj.EmployeePipelineID,
            ManagerProductSpecificationID = reqObj.ManagerProductSpecificationID,
        };
        var coEmpPplPrdDbExistRspObj = await this._coEmpPipelineProductDb.ExistAsync(coEmpPplPrdDbExistReqObj);
        if (coEmpPplPrdDbExistRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // db, 核心-員工-商機-資料庫-取得
        var coEmpPplDbGetReqObj = new CoEmpPplDbGetReqMdl()
        {
            EmployeePipelineID = reqObj.EmployeePipelineID,
        };
        var coEmpPplDbGetRspObj = await this._coEmpPipelineDb.GetAsync(coEmpPplDbGetReqObj);
        if (coEmpPplDbGetRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }
        if (coEmpPplDbGetRspObj.ManagerCompanyID == null)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }
        if (coEmpPplDbGetRspObj.EmployeePipelineSalerEmployeeID.HasValue == false)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // 檢查權限
        if (mbsCmnLgcGetLoginUserInfoRspObj.AtomPermissionKindIdCreate == DbAtomPermissionKindEnum.Self &&
            coEmpPplDbGetRspObj.EmployeePipelineSalerEmployeeID.Value != mbsCmnLgcGetLoginUserInfoRspObj.EmployeeID)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            // 權限不足
            rspObj.ErrorCode = MbsErrorCodeEnum.PermissionDenied;
            return rspObj;
        }

        // db, 核心-員工-商機產品-資料庫-新增
        var coEmpPplPrdDbAddReqObj = new CoEmpPplPrdDbAddReqMdl()
        {
            EmployeePipelineID = reqObj.EmployeePipelineID,
            ManagerCompanyID = coEmpPplDbGetRspObj.ManagerCompanyID,
            ManagerProductID = reqObj.ManagerProductID,
            ManagerProductSpecificationID = reqObj.ManagerProductSpecificationID,
            EmployeePipelineProductSellPrice = reqObj.EmployeePipelineProductSellPrice,
            EmployeePipelineProductClosingPrice = reqObj.EmployeePipelineProductClosingPrice,
            EmployeePipelineProductCostPrice = reqObj.EmployeePipelineProductCostPrice,
            EmployeePipelineProductCount = reqObj.EmployeePipelineProductCount,
            EmployeePipelineProductPurchaseKindID = reqObj.EmployeePipelineProductPurchaseKindID,
            EmployeePipelineProductContractKindID = reqObj.EmployeePipelineProductContractKindID,
            EmployeePipelineProductContractText = reqObj.EmployeePipelineProductContractText,
        };
        var coEmpPplPrdDbAddRspObj = await this._coEmpPipelineProductDb.AddAsync(coEmpPplPrdDbAddReqObj);
        if (coEmpPplPrdDbAddRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        rspObj.EmployeePipelineProductID = coEmpPplPrdDbAddRspObj.EmployeePipelineProductID;
        return rspObj;
    }

    /// <summary>管理者後台-CRM-商機管理-邏輯服務-更新商機產品</summary>
    public async Task<MbsCrmPplLgcUpdateEmployeePipelineProductRspMdl> UpdateEmployeePipelineProductAsync(MbsCrmPplLgcUpdateEmployeePipelineProductReqMdl reqObj)
    {
        var rspObj = new MbsCrmPplLgcUpdateEmployeePipelineProductRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        #region 判斷登入令牌與權限

        // mbs, 管理者後台-通用-邏輯-取得登入使用者資訊
        var mbsCmnLgcGetLoginUserInfoReqObj = new MbsCmnLgcGetLoginUserInfoReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken,
            AtomMenu = this._atomMenu,
        };
        var mbsCmnLgcGetLoginUserInfoRspObj = await this._mbsCommonLogical.GetLoginUserInfoAsync(mbsCmnLgcGetLoginUserInfoReqObj);
        if (mbsCmnLgcGetLoginUserInfoRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }
        if (mbsCmnLgcGetLoginUserInfoRspObj.ErrorCode != MbsErrorCodeEnum.Success)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            rspObj.ErrorCode = mbsCmnLgcGetLoginUserInfoRspObj.ErrorCode;
            return rspObj;
        }

        // 判斷權限
        if (mbsCmnLgcGetLoginUserInfoRspObj.AtomPermissionKindIdEdit != DbAtomPermissionKindEnum.Self &&
            mbsCmnLgcGetLoginUserInfoRspObj.AtomPermissionKindIdEdit != DbAtomPermissionKindEnum.Everyone)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");

            // 權限不足
            rspObj.ErrorCode = MbsErrorCodeEnum.PermissionDenied;
            return rspObj;
        }

        #endregion

        // db, 核心-員工-商機產品-資料庫-取得
        var coEmpPplPrdDbGetReqObj = new CoEmpPplPrdDbGetReqMdl()
        {
            EmployeePipelineProductID = reqObj.EmployeePipelineProductID
        };
        var coEmpPplPrdDbGetRspObj = await this._coEmpPipelineProductDb.GetAsync(coEmpPplPrdDbGetReqObj);
        if (coEmpPplPrdDbGetRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // db, 核心-員工-商機-資料庫-取得
        var coEmpPplDbGetReqObj = new CoEmpPplDbGetReqMdl()
        {
            EmployeePipelineID = coEmpPplPrdDbGetRspObj.EmployeePipelineID
        };
        var coEmpPplDbGetRspObj = await this._coEmpPipelineDb.GetAsync(coEmpPplDbGetReqObj);
        if (coEmpPplDbGetRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }
        if (coEmpPplDbGetRspObj.EmployeePipelineSalerEmployeeID.HasValue == false)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // 檢查權限
        if (mbsCmnLgcGetLoginUserInfoRspObj.AtomPermissionKindIdEdit == DbAtomPermissionKindEnum.Self &&
            coEmpPplDbGetRspObj.EmployeePipelineSalerEmployeeID.Value != mbsCmnLgcGetLoginUserInfoRspObj.EmployeeID)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            // 權限不足
            rspObj.ErrorCode = MbsErrorCodeEnum.PermissionDenied;
            return rspObj;
        }

        // 判斷 管理者產品規格ID 是否需要更新
        if (reqObj.ManagerProductSpecificationID.HasValue)
        {
            // db, 核心-管理者-商機產品-資料庫-是否存在
            var CoEmpPplPrdDbExistReqObj = new CoEmpPplPrdDbExistReqMdl()
            {
                EmployeePipelineID = reqObj.EmployeePipelineID,
                ManagerProductSpecificationID = reqObj.ManagerProductSpecificationID,
            };
            var CoEmpPplPrdDbExistRspObj = await this._coEmpPipelineProductDb.ExistAsync(CoEmpPplPrdDbExistReqObj);
            if (CoEmpPplPrdDbExistRspObj == default)
            {
                this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                return rspObj;
            }
            if (CoEmpPplPrdDbExistRspObj.IsExist)
            {
                this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");

                // 管理者產品規格ID 重複
                rspObj.ErrorCode = MbsErrorCodeEnum.NameDuplicate;
                return rspObj;
            }
        }

        // db, 核心-員工-商機產品-資料庫-更新
        var coEmpPplPrdDbUpdateReqObj = new CoEmpPplPrdDbUpdateReqMdl()
        {
            EmployeePipelineProductID = reqObj.EmployeePipelineProductID,
            ManagerProductID = reqObj.ManagerProductID,
            ManagerProductSpecificationID = reqObj.ManagerProductSpecificationID,
            EmployeePipelineProductSellPrice = reqObj.EmployeePipelineProductSellPrice,
            EmployeePipelineProductClosingPrice = reqObj.EmployeePipelineProductClosingPrice,
            EmployeePipelineProductCostPrice = reqObj.EmployeePipelineProductCostPrice,
            EmployeePipelineProductCount = reqObj.EmployeePipelineProductCount,
            EmployeePipelineProductPurchaseKindID = reqObj.EmployeePipelineProductPurchaseKindID,
            EmployeePipelineProductContractKindID = reqObj.EmployeePipelineProductContractKindID,
            EmployeePipelineProductContractText = reqObj.EmployeePipelineProductContractText,
        };
        var coEmpPplPrdDbUpdateRspObj = await this._coEmpPipelineProductDb.UpdateAsync(coEmpPplPrdDbUpdateReqObj);
        if (coEmpPplPrdDbUpdateRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        return rspObj;
    }

    /// <summary>管理者後台-CRM-商機管理-邏輯服務-刪除商機產品</summary>
    public async Task<MbsCrmPplLgcRemoveEmployeePipelineProductRspMdl> RemoveEmployeePipelineProductAsync(MbsCrmPplLgcRemoveEmployeePipelineProductReqMdl reqObj)
    {
        var rspObj = new MbsCrmPplLgcRemoveEmployeePipelineProductRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        #region 判斷登入令牌與權限

        // mbs, 管理者後台-通用-邏輯-取得登入使用者資訊
        var mbsCmnLgcGetLoginUserInfoReqObj = new MbsCmnLgcGetLoginUserInfoReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken,
            AtomMenu = this._atomMenu,
        };
        var mbsCmnLgcGetLoginUserInfoRspObj = await this._mbsCommonLogical.GetLoginUserInfoAsync(mbsCmnLgcGetLoginUserInfoReqObj);
        if (mbsCmnLgcGetLoginUserInfoRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        if (mbsCmnLgcGetLoginUserInfoRspObj.ErrorCode != MbsErrorCodeEnum.Success)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            rspObj.ErrorCode = mbsCmnLgcGetLoginUserInfoRspObj.ErrorCode;
            return rspObj;
        }

        // 判斷權限
        if (mbsCmnLgcGetLoginUserInfoRspObj.AtomPermissionKindIdDelete != DbAtomPermissionKindEnum.Self &&
            mbsCmnLgcGetLoginUserInfoRspObj.AtomPermissionKindIdDelete != DbAtomPermissionKindEnum.Everyone)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");

            // 權限不足
            rspObj.ErrorCode = MbsErrorCodeEnum.PermissionDenied;
            return rspObj;
        }

        #endregion

        // db, 核心-員工-商機產品-資料庫-取得
        var coEmpPplPrdDbGetReqObj = new CoEmpPplPrdDbGetReqMdl()
        {
            EmployeePipelineProductID = reqObj.EmployeePipelineProductID
        };
        var coEmpPplPrdDbGetRspObj = await this._coEmpPipelineProductDb.GetAsync(coEmpPplPrdDbGetReqObj);
        if (coEmpPplPrdDbGetRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // db, 核心-員工-商機-資料庫-取得
        var coEmpPplDbGetReqObj = new CoEmpPplDbGetReqMdl()
        {
            EmployeePipelineID = coEmpPplPrdDbGetRspObj.EmployeePipelineID
        };
        var coEmpPplDbGetRspObj = await this._coEmpPipelineDb.GetAsync(coEmpPplDbGetReqObj);
        if (coEmpPplDbGetRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }
        if (coEmpPplDbGetRspObj.EmployeePipelineSalerEmployeeID.HasValue == false)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // 檢查權限
        if (mbsCmnLgcGetLoginUserInfoRspObj.AtomPermissionKindIdDelete == DbAtomPermissionKindEnum.Self &&
            coEmpPplDbGetRspObj.EmployeePipelineSalerEmployeeID.Value != mbsCmnLgcGetLoginUserInfoRspObj.EmployeeID)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            // 權限不足
            rspObj.ErrorCode = MbsErrorCodeEnum.PermissionDenied;
            return rspObj;
        }

        // db, 核心-員工-商機產品-資料庫-移除
        var coEmpPplPrdDbRemoveReqObj = new CoEmpPplPrdDbRemoveReqMdl()
        {
            EmployeePipelineProductID = reqObj.EmployeePipelineProductID,
        };
        var coEmpPplPrdDbRemoveRspObj = await this._coEmpPipelineProductDb.RemoveAsync(coEmpPplPrdDbRemoveReqObj);
        if (coEmpPplPrdDbRemoveRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        return rspObj;
    }

    #endregion

    #region 名單

    /// <summary>管理者後台-CRM-商機管理-邏輯服務-取得多筆名單</summary>
    public async Task<MbsCrmPplLgcGetManyEmployeePipelineRspMdl> GetManyEmployeePipelineAsync(MbsCrmPplLgcGetManyEmployeePipelineReqMdl reqObj)
    {
        var rspObj = new MbsCrmPplLgcGetManyEmployeePipelineRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        #region 判斷登入令牌與權限

        // mbs, 管理者後台-通用-邏輯-取得登入使用者資訊
        var mbsCmnLgcGetLoginUserInfoReqObj = new MbsCmnLgcGetLoginUserInfoReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken,
            AtomMenu = this._atomMenu,
        };
        var mbsCmnLgcGetLoginUserInfoRspObj = await this._mbsCommonLogical.GetLoginUserInfoAsync(mbsCmnLgcGetLoginUserInfoReqObj);
        if (mbsCmnLgcGetLoginUserInfoRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }
        if (mbsCmnLgcGetLoginUserInfoRspObj.ErrorCode != MbsErrorCodeEnum.Success)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");

            rspObj.ErrorCode = mbsCmnLgcGetLoginUserInfoRspObj.ErrorCode;
            return rspObj;
        }

        // 判斷權限,無權限/自身/所有人 
        if (mbsCmnLgcGetLoginUserInfoRspObj.AtomPermissionKindIdView != DbAtomPermissionKindEnum.Everyone &&
            mbsCmnLgcGetLoginUserInfoRspObj.AtomPermissionKindIdView != DbAtomPermissionKindEnum.Self)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");

            // 權限不足
            rspObj.ErrorCode = MbsErrorCodeEnum.PermissionDenied;
            return rspObj;
        }

        #endregion

        // db, 核心-員工-商機-資料庫-取得[筆數]從[管理者後台-CRM-商機管理]
        var coEmpPplDbGetCountFromMbsCrmPipelineReqObj = new CoEmpPplDbGetCountFromMbsCrmPipelineReqMdl()
        {
            AtomPipelineStatus = reqObj.AtomPipelineStatus,
            ManagerCompanyName = reqObj.ManagerCompanyName,
            ManagerContacterName = reqObj.ManagerContacterName,
            ManagerContacterEmail = reqObj.ManagerContacterEmail,
            // 判斷權限,自身or所有人
            EmployeePipelineSalerEmployeeID =
            mbsCmnLgcGetLoginUserInfoRspObj.AtomPermissionKindIdView == DbAtomPermissionKindEnum.Self
                                                ? mbsCmnLgcGetLoginUserInfoRspObj.EmployeeID
                                                : reqObj.EmployeePipelineSalerEmployeeID,
        };
        var coEmpPplDbGetCountFromMbsCrmPipelineRspObj = await this._coEmpPipelineDb.GetCountFromMbsCrmPipelineAsync(coEmpPplDbGetCountFromMbsCrmPipelineReqObj);
        if (coEmpPplDbGetCountFromMbsCrmPipelineRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // 判斷筆數是否為0
        if (coEmpPplDbGetCountFromMbsCrmPipelineRspObj.Count == 0)
        {
            rspObj.ErrorCode = MbsErrorCodeEnum.Success;
            rspObj.EmployeePipelineList = new();
            rspObj.TotalCount = 0;
            return rspObj;
        }

        // db, 核心-員工-商機-資料庫-取得多筆[管理者後台-CRM-商機管理]
        var coEmpPplDbGetManyFromMbsCrmPipelineReqObj = new CoEmpPplDbGetManyFromMbsCrmPipelineReqMdl()
        {
            AtomPipelineStatus = reqObj.AtomPipelineStatus,
            ManagerCompanyName = reqObj.ManagerCompanyName,
            ManagerContacterName = reqObj.ManagerContacterName,
            ManagerContacterEmail = reqObj.ManagerContacterEmail,
            // 判斷權限,自身or所有人
            EmployeePipelineSalerEmployeeID =
            mbsCmnLgcGetLoginUserInfoRspObj.AtomPermissionKindIdView == DbAtomPermissionKindEnum.Self
                                                ? mbsCmnLgcGetLoginUserInfoRspObj.EmployeeID
                                                : reqObj.EmployeePipelineSalerEmployeeID,
            PageIndex = reqObj.PageIndex,
            PageSize = reqObj.PageSize
        };
        var coEmpPplDbGetManyFromMbsCrmPipelineRspObj = await this._coEmpPipelineDb.GetManyFromMbsCrmPipelineAsync(coEmpPplDbGetManyFromMbsCrmPipelineReqObj);
        if (coEmpPplDbGetManyFromMbsCrmPipelineRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        rspObj.EmployeePipelineList = coEmpPplDbGetManyFromMbsCrmPipelineRspObj.EmployeePipelineList
                .Select(pplOgn => new MbsCrmPplLgcGetManyEmployeePipelineRspItemMdl
                {
                    EmployeePipelineID = pplOgn.EmployeePipelineID,
                    ManagerCompanyName = pplOgn.ManagerCompanyName,
                    ManagerContacterDepartment = pplOgn.ManagerContacterDepartment,
                    ManagerContacterJobTitle = pplOgn.ManagerContacterJobTitle,
                    ManagerContacterName = pplOgn.ManagerContacterName,
                    ManagerContacterEmail = pplOgn.ManagerContacterEmail,
                    ManagerContacterPhone = pplOgn.ManagerContacterPhone,
                    ManagerContacterTelephone = pplOgn.ManagerContacterTelephone,
                    AtomPipelineStatus = pplOgn.AtomPipelineStatus,
                    EmployeePipelineSalerEmployeeName = pplOgn.EmployeePipelineSalerEmployeeName
                }).ToList();
        rspObj.TotalCount = coEmpPplDbGetCountFromMbsCrmPipelineRspObj.Count;
        return rspObj;
    }

    /// <summary>管理者後台-CRM-商機管理-邏輯服務-取得名單</summary>
    public async Task<MbsCrmPplLgcGetEmployeePipelineRspMdl> GetEmployeePipelineAsync(MbsCrmPplLgcGetEmployeePipelineReqMdl reqObj)
    {
        var rspObj = new MbsCrmPplLgcGetEmployeePipelineRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        #region 判斷登入令牌與權限

        // mbs, 管理者後台-通用-邏輯-取得登入使用者資訊
        var mbsCmnLgcGetLoginUserInfoReqObj = new MbsCmnLgcGetLoginUserInfoReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken,
            AtomMenu = this._atomMenu,
        };
        var mbsCmnLgcGetLoginUserInfoRspObj = await this._mbsCommonLogical.GetLoginUserInfoAsync(mbsCmnLgcGetLoginUserInfoReqObj);
        if (mbsCmnLgcGetLoginUserInfoRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }
        if (mbsCmnLgcGetLoginUserInfoRspObj.ErrorCode != MbsErrorCodeEnum.Success)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");

            rspObj.ErrorCode = mbsCmnLgcGetLoginUserInfoRspObj.ErrorCode;
            return rspObj;
        }

        // 判斷權限,無權限/自身/所有人 
        if (mbsCmnLgcGetLoginUserInfoRspObj.AtomPermissionKindIdView != DbAtomPermissionKindEnum.Everyone &&
            mbsCmnLgcGetLoginUserInfoRspObj.AtomPermissionKindIdView != DbAtomPermissionKindEnum.Self)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");

            // 權限不足
            rspObj.ErrorCode = MbsErrorCodeEnum.PermissionDenied;
            return rspObj;
        }

        #endregion

        // db, 核心-員工-商機-資料庫-取得[管理者後台-CRM-商機管理]
        var coEmpPplDbGetFromMbsCrmPipelineReqObj = new CoEmpPplDbGetFromMbsCrmPipelineReqMdl()
        {
            EmployeePipelineID = reqObj.EmployeePipelineID,
            // 判斷權限,自身or所有人
            EmployeePipelineSalerEmployeeID = mbsCmnLgcGetLoginUserInfoRspObj.AtomPermissionKindIdView == DbAtomPermissionKindEnum.Self
                                    ? mbsCmnLgcGetLoginUserInfoRspObj.EmployeeID
                                    : null
        };
        var coEmpPplDbGetFromMbsCrmPipelineRspObj = await this._coEmpPipelineDb.GetFromMbsCrmPipelineAsync(coEmpPplDbGetFromMbsCrmPipelineReqObj);
        if (coEmpPplDbGetFromMbsCrmPipelineRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }
        if (coEmpPplDbGetFromMbsCrmPipelineRspObj.EmployeePipelineSalerEmployeeID.HasValue == false)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // 檢查權限
        if (mbsCmnLgcGetLoginUserInfoRspObj.AtomPermissionKindIdView == DbAtomPermissionKindEnum.Self &&
            coEmpPplDbGetFromMbsCrmPipelineRspObj.EmployeePipelineSalerEmployeeID.Value != mbsCmnLgcGetLoginUserInfoRspObj.EmployeeID)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            // 權限不足
            rspObj.ErrorCode = MbsErrorCodeEnum.PermissionDenied;
            return rspObj;
        }

        // db, 核心-員工-資訊-資料庫-取得[名稱]
        var coEmpInfDbGetNameReqObj = new CoEmpInfDbGetNameReqMdl()
        {
            EmployeeID = coEmpPplDbGetFromMbsCrmPipelineRspObj.EmployeePipelineSalerEmployeeID.Value
        };
        var coEmpInfDbGetNameRspObj = await this._coEmpInfoDb.GetNameAsync(coEmpInfDbGetNameReqObj);
        if (coEmpInfDbGetNameRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        var company = new MbsCrmPplLgcGetEmployeePipelineRspCompanyItemMdl();
        if (coEmpPplDbGetFromMbsCrmPipelineRspObj.ManagerCompanyID.HasValue)
        {
            // db, 核心-管理者-公司-資料庫-取得
            var coMgrCmpDbGetReqObj = new CoMgrCmpDbGetReqMdl()
            {
                ManagerCompanyID = coEmpPplDbGetFromMbsCrmPipelineRspObj.ManagerCompanyID.Value
            };
            var coMgrCmpDbGetRspObj = await this._coMgrCompanyDb.GetAsync(coMgrCmpDbGetReqObj);
            if (coMgrCmpDbGetRspObj == default)
            {
                this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                return rspObj;
            }
            company.ManagerCompanyID = coMgrCmpDbGetRspObj.ManagerCompanyID;
            company.ManagerCompanyUnifiedNumber = coMgrCmpDbGetRspObj.ManagerCompanyUnifiedNumber;
            company.ManagerCompanyName = coMgrCmpDbGetRspObj.ManagerCompanyName;
            company.AtomEmployeeRange = coMgrCmpDbGetRspObj.AtomEmployeeRange;
            company.AtomCustomerGrade = coMgrCmpDbGetRspObj.AtomCustomerGrade;

            // db, 核心-管理者-公司主分類-資料庫-取得
            var coMgrCmpMainKdDbGetReqObj = new CoMgrCmpMainKdDbGetReqMdl()
            {
                ManagerCompanyMainKindID = coMgrCmpDbGetRspObj.ManagerCompanyMainKindID
            };
            var coMgrCmpMainKdDbGetRspObj = await this._coMgrCompanyMainKindDb.GetAsync(coMgrCmpMainKdDbGetReqObj);
            if (coMgrCmpMainKdDbGetRspObj == default)
            {
                this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                return rspObj;
            }
            company.ManagerCompanyMainKindName = coMgrCmpMainKdDbGetRspObj.ManagerCompanyMainKindName;

            // db, 核心-管理者-公司子分類-資料庫-取得
            var coMgrCmpSubKdDbGetReqObj = new CoMgrCmpSubKdDbGetReqMdl()
            {
                ManagerCompanySubKindID = coMgrCmpDbGetRspObj.ManagerCompanySubKindID
            };
            var coMgrCmpSubKdDbGetRspObj = await this._coMgrCompanySubKindDb.GetAsync(coMgrCmpSubKdDbGetReqObj);
            if (coMgrCmpSubKdDbGetRspObj == default)
            {
                this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                return rspObj;
            }
            company.ManagerCompanySubKindName = coMgrCmpSubKdDbGetRspObj.ManagerCompanySubKindName;
        }

        if (coEmpPplDbGetFromMbsCrmPipelineRspObj.ManagerCompanyLocationID.HasValue)
        {
            // db, 核心-管理者-公司營業地點-資料庫-取得
            var coMgrCmpLocDbGetReqObj = new CoMgrCmpLocDbGetReqMdl()
            {
                ManagerCompanyLocationID = coEmpPplDbGetFromMbsCrmPipelineRspObj.ManagerCompanyLocationID.Value
            };
            var coMgrCmpLocDbGetRspObj = await this._coMgrCompanyLocationDb.GetAsync(coMgrCmpLocDbGetReqObj);
            if (coMgrCmpLocDbGetRspObj == default)
            {
                this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                return rspObj;
            }
            company.ManagerCompanyLocationID = coMgrCmpLocDbGetRspObj.ManagerCompanyLocationID;
            company.AtomCity = coMgrCmpLocDbGetRspObj.AtomCity;
            company.ManagerCompanyLocationAddress = coMgrCmpLocDbGetRspObj.ManagerCompanyLocationAddress;
            company.ManagerCompanyLocationTelephone = coMgrCmpLocDbGetRspObj.ManagerCompanyLocationTelephone;
        }

        // db, 核心-員工-商機窗口-資料庫-取得多筆
        var coEmpPplContDbGetManyReqObj = new CoEmpPplContDbGetManyReqMdl()
        {
            EmployeePipelineID = reqObj.EmployeePipelineID
        };
        var coEmpPplContDbGetManyRspObj = await this._coEmpPipelineContacterDb.GetManyAsync(coEmpPplContDbGetManyReqObj);
        if (coEmpPplContDbGetManyRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // db, 核心-管理者-窗口-資料庫服務-取得多筆[名稱]
        var coMgrCttDbGetManyNameReqObj = new CoMgrCttDbGetManyNameReqMdl()
        {
            ManagerContacterIDList = coEmpPplContDbGetManyRspObj.EmployeePipelineContacterList
                                        .Select(x => x.ManagerContacterID)
                                        .Distinct()
                                        .ToList()
        };
        var coMgrCttDbGetManyNameRspObj = await this._coMgrContacterDb.GetManyNameAsync(coMgrCttDbGetManyNameReqObj);
        if (coMgrCttDbGetManyNameRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // db, 核心-管理者-窗口-資料庫服務-取得多筆
        var managerContacterIDList = new List<int>();
        if (coEmpPplContDbGetManyRspObj.EmployeePipelineContacterList != null)
        {
            managerContacterIDList.AddRange(coEmpPplContDbGetManyRspObj.EmployeePipelineContacterList
                                                .Select(x => x.ManagerContacterID)
                                                .Distinct()
                                                .ToList());
        }
        var coMgrCttDbGetManyReqObj = new CoMgrCttDbGetManyReqMdl()
        {
            ManagerContacterIDList = managerContacterIDList
                                        .Distinct()
                                        .ToList()
        };
        var coMgrCttDbGetManyRspObj = await this._coMgrContacterDb.GetManyAsync(coMgrCttDbGetManyReqObj);
        if (coMgrCttDbGetManyRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // db, 核心-員工-商機業務-資料庫-取得多筆[管理者後台-CRM-商機管理]
        var coEmpPplSlrDbGetManyFromMbsCrmPplReqObj = new CoEmpPplSlrDbGetManyFromMbsCrmPplReqMdl()
        {
            EmployeePipelineID = reqObj.EmployeePipelineID,
            EmployeePipelineSalerEmployeeID = coEmpPplDbGetFromMbsCrmPipelineRspObj.EmployeePipelineSalerEmployeeID.Value
        };
        var coEmpPplSlrDbGetManyFromMbsCrmPplRspObj = await this._coEmpPipelineSalerDb.GetManyFromMbsCrmPplAsync(coEmpPplSlrDbGetManyFromMbsCrmPplReqObj);
        if (coEmpPplSlrDbGetManyFromMbsCrmPplRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // 取得待處理的商機業務,且業務員公ID==登入者ID
        var pendingEmployeePipelineSaler = coEmpPplSlrDbGetManyFromMbsCrmPplRspObj.EmployeePipelineSalerList
            .Where(x =>
                x.EmployeePipelineID == reqObj.EmployeePipelineID &&
                x.EmployeePipelineSalerEmployeeID == mbsCmnLgcGetLoginUserInfoRspObj.EmployeeID)
            .OrderByDescending(x => x.EmployeePipelineSalerID)
            .FirstOrDefault();

        // db, 核心-員工-商機業務開發紀錄-資料庫-取得多筆[管理者後台-CRM-商機管理]
        var coEmpPplSlrTrkDbGetManyFromMbsCrmPplReqObj = new CoEmpPplSlrTrkDbGetManyFromMbsCrmPplReqMdl()
        {
            EmployeePipelineID = reqObj.EmployeePipelineID,
        };
        var coEmpPplSlrTrkDbGetManyFromMbsCrmPplRspObj = await this._coEmpPipelineSalerTrackingDb.GetManyFromMbsCrmPplAsync(coEmpPplSlrTrkDbGetManyFromMbsCrmPplReqObj);
        if (coEmpPplSlrTrkDbGetManyFromMbsCrmPplRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // db, 核心-管理者-商機產品-資料庫-取得多筆從[管理者後台-CRM-商機管理]
        var coEmpPplPrdDbGetManyFromCrmPplReqObj = new CoEmpPplPrdDbGetManyFromCrmPplReqMdl()
        {
            EmployeePipelineID = reqObj.EmployeePipelineID,
        };
        var coEmpPplPrdDbGetManyFromCrmPplRspObj = await this._coEmpPipelineProductDb.GetManyFromMbsCrmPplAsync(coEmpPplPrdDbGetManyFromCrmPplReqObj);
        if (coEmpPplPrdDbGetManyFromCrmPplRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // db, 核心-員工-商機履約期限-資料庫-取得多筆
        var coEmpPplDueDbGetManyReqObj = new CoEmpPplDueDbGetManyReqMdl()
        {
            EmployeePipelineID = reqObj.EmployeePipelineID
        };
        var coEmpPplDueDbGetManyRspObj = await this._coEmpPipelineDueDb.GetManyAsync(coEmpPplDueDbGetManyReqObj);
        if (coEmpPplDueDbGetManyRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // db, 核心-員工-商機發票紀錄-資料庫-取得多筆
        var coEmpPplBllDbGetManyReqObj = new CoEmpPplBllDbGetManyReqMdl()
        {
            EmployeePipelineID = reqObj.EmployeePipelineID
        };
        var coEmpPplBllDbGetManyRspObj = await this._coEmpPipelineBillDb.GetManyAsync(coEmpPplBllDbGetManyReqObj);
        if (coEmpPplBllDbGetManyRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // db, 核心-員工-資訊-資料庫-取得
        var coEmpInfoDbGetReqObj = new CoEmpInfDbGetReqMdl()
        {
            EmployeeID = coEmpPplDbGetFromMbsCrmPipelineRspObj.EmployeePipelineSalerEmployeeID.Value
        };
        var coEmpInfoDbGetRspObj = await this._coEmpInfoDb.GetAsync(coEmpInfoDbGetReqObj);
        if (coEmpInfoDbGetRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // db, 核心-管理者-角色-資料庫-取得
        var coMgrRolDbGetReqObj = new CoMgrRolDbGetReqMdl()
        {
            ManagerRoleID = coEmpInfoDbGetRspObj.ManagerRoleID,
        };
        var coMgrRolDbGetRspObj = await this._coMgrRolDb.GetAsync(coMgrRolDbGetReqObj);
        if (coMgrRolDbGetRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // db, 核心-管理者-地區-資料庫-取得[名稱]
        var coMgrRgnDbGetNameReqObj = new CoMgrRgnDbGetNameReqMdl()
        {
            ManagerRegionID = coMgrRolDbGetRspObj.ManagerRegionID,
        };
        var coMgrRgnDbGetNameRspObj = await this._coMgrRgnDb.GetNameAsync(coMgrRgnDbGetNameReqObj);
        if (coMgrRgnDbGetNameRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // db, 核心-管理者-部門-資料庫-取得[名稱]
        var coMgrDptDbGetNameReqObj = new CoMgrDptDbGetNameReqMdl()
        {
            ManagerDepartmentID = coMgrRolDbGetRspObj.ManagerDepartmentID,
        };
        var coMgrDptDbGetNameRspObj = await this._coMgrDptDb.GetNameAsync(coMgrDptDbGetNameReqObj);
        if (coMgrDptDbGetNameRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        // 基本資訊
        rspObj.AtomPipelineStatus = coEmpPplDbGetFromMbsCrmPipelineRspObj.AtomPipelineStatus;
        rspObj.EmployeePipelineSalerEmployeeID = coEmpPplDbGetFromMbsCrmPipelineRspObj.EmployeePipelineSalerEmployeeID.Value;
        rspObj.EmployeePipelineSalerEmployeeName = coEmpInfDbGetNameRspObj.EmployeeName;
        rspObj.EmployeePipelineSalerRegionID = coMgrRolDbGetRspObj.ManagerRegionID;
        rspObj.EmployeePipelineSalerRegionName = coMgrRgnDbGetNameRspObj.ManagerRegionName;
        rspObj.EmployeePipelineSalerDepartmentID = coMgrRolDbGetRspObj.ManagerDepartmentID;
        rspObj.EmployeePipelineSalerDepartmentName = coMgrDptDbGetNameRspObj.ManagerDepartmentName;
        rspObj.StageStatus = new MbsCrmPplLgcGetEmployeePipelineRspStageStatusMdl
        {
            BusinessNeedStatus = coEmpPplDbGetFromMbsCrmPipelineRspObj.BusinessNeedStatus,
            BusinessTimelineStatus = coEmpPplDbGetFromMbsCrmPipelineRspObj.BusinessTimelineStatus,
            BusinessBudgetStatus = coEmpPplDbGetFromMbsCrmPipelineRspObj.BusinessBudgetStatus,
            BusinessPresentationStatus = coEmpPplDbGetFromMbsCrmPipelineRspObj.BusinessPresentationStatus,
            BusinessQuotationStatus = coEmpPplDbGetFromMbsCrmPipelineRspObj.BusinessQuotationStatus,
            BusinessNegotiationStatus = coEmpPplDbGetFromMbsCrmPipelineRspObj.BusinessNegotiationStatus,
            BusinessStageRemark = coEmpPplDbGetFromMbsCrmPipelineRspObj.BusinessStageRemark
        };
        // 客戶資訊
        rspObj.Company = company;

        // 窗口資訊
        rspObj.ContacterList = (
            from epc in coEmpPplContDbGetManyRspObj.EmployeePipelineContacterList
            from mc in coMgrCttDbGetManyRspObj.ManagerContacterList
                        .Where(mc => mc.ManagerContacterID == epc.ManagerContacterID)
                        .DefaultIfEmpty()
            select new MbsCrmPplLgcGetEmployeePipelineRspContacterItemMdl
            {
                ManagerContacterID = epc.ManagerContacterID,
                ManagerContacterName = mc?.ManagerContacterName,
                ManagerContacterEmail = mc?.ManagerContacterEmail,
                ManagerContacterPhone = mc?.ManagerContacterPhone,
                ManagerContacterDepartment = mc?.ManagerContacterDepartment,
                ManagerContacterJobTitle = mc?.ManagerContacterJobTitle,
                ManagerContacterTelephone = mc?.ManagerContacterTelephone,
                ManagerContacterIsConsent = mc?.ManagerContacterIsConsent ?? false,
                ManagerContacterStatus = mc?.ManagerContacterStatus ?? DbAtomManagerContacterStatusEnum.Unknown,
                AtomRatingKind = mc?.AtomRatingKind ?? DbAtomManagerContacterRatingKindEnum.Whitelist,
                EmployeePipelineContacterIsPrimary = epc.EmployeePipelineContacterIsPrimary,
                ManagerContacterRemark = mc?.ManagerContacterRemark
            }
        ).ToList();
        // 指派業務紀錄
        rspObj.PendingEmployeePipelineSaler = (pendingEmployeePipelineSaler != null
                                            && pendingEmployeePipelineSaler != default
                                            && pendingEmployeePipelineSaler.EmployeePipelineSalerStatus == DbAtomEmployeePipelineSalerStatusEnum.Pending)
        ? new MbsCrmPplLgcGetEmployeePipelineRspPendingSalerItemMdl
        {
            EmployeePipelineSalerID = pendingEmployeePipelineSaler.EmployeePipelineSalerID,
            EmployeePipelineSalerEmployeeName = pendingEmployeePipelineSaler.EmployeePipelineSalerEmployeeName,
            EmployeePipelineSalerStatus = pendingEmployeePipelineSaler.EmployeePipelineSalerStatus,
            EmployeePipelineSalerCreateEmployeeName = pendingEmployeePipelineSaler.EmployeePipelineSalerCreateEmployeeName,
            EmployeePipelineSalerCreateTime = pendingEmployeePipelineSaler.EmployeePipelineSalerCreateTime,
            // 接受-判斷權限,自身
            HasAcceptPermissions = pendingEmployeePipelineSaler.EmployeePipelineSalerEmployeeID == mbsCmnLgcGetLoginUserInfoRspObj.EmployeeID,
            // 拒絕-判斷權限,自身
            HasRejectPermissions = pendingEmployeePipelineSaler.EmployeePipelineSalerEmployeeID == mbsCmnLgcGetLoginUserInfoRspObj.EmployeeID,
            // 轉指派-判斷權限,所有人
            HasReassignPermissions = mbsCmnLgcGetLoginUserInfoRspObj.AtomPermissionKindIdView == DbAtomPermissionKindEnum.Everyone
        } : null;
        // 業務記錄列表
        rspObj.EmployeePipelineSalerList = coEmpPplSlrDbGetManyFromMbsCrmPplRspObj.EmployeePipelineSalerList
            .Select(x => new MbsCrmPplLgcGetEmployeePipelineRspSalerItemMdl
            {
                EmployeePipelineSalerID = x.EmployeePipelineSalerID,
                EmployeePipelineSalerStatus = x.EmployeePipelineSalerStatus,
                EmployeePipelineSalerCreateTime = x.EmployeePipelineSalerCreateTime,
                EmployeePipelineSalerCreateEmployeeName = x.EmployeePipelineSalerCreateEmployeeName,
                EmployeePipelineSalerReplyTime = x.EmployeePipelineSalerReplyTime,
                EmployeePipelineSalerEmployeeName = x.EmployeePipelineSalerEmployeeName,
                EmployeePipelineSalerRemark = x.EmployeePipelineSalerRemark
            }).ToList();
        // 業務商機開發紀錄
        rspObj.EmployeePipelineSalerTrackingList = coEmpPplSlrTrkDbGetManyFromMbsCrmPplRspObj.EmployeePipelineSalerTrackingList
            .Select(x => new MbsCrmPplLgcGetEmployeePipelineRspSalerTrackingItemMdl
            {
                EmployeePipelineSalerTrackingID = x.EmployeePipelineSalerTrackingID,
                EmployeePipelineSalerTrackingTime = x.EmployeePipelineSalerTrackingTime,
                ManagerContacterID = x.ManagerContacterID,
                ManagerContacterName = x.ManagerContacterName?.Trim(),
                EmployeePipelineSalerTrackingRemark = x.EmployeePipelineSalerTrackingRemark?.Trim(),
                EmployeePipelineSalerTrackingCreateEmployeeName = x.EmployeePipelineSalerTrackingCreateEmployeeName?.Trim()
            }).ToList();
        // 商機產品
        rspObj.EmployeePipelineProductList = coEmpPplPrdDbGetManyFromCrmPplRspObj.EmployeePipelineProductList
            .Select(x => new MbsCrmPplLgcGetEmployeePipelineRspProductItemMdl
            {
                EmployeePipelineProductID = x.EmployeePipelineProductID,
                ManagerProductID = x.ManagerProductID,
                ManagerProductSpecificationID = x.ManagerProductSpecificationID,
                ManagerProductName = x.ManagerProductName,
                ManagerProductMainKindName = x.ManagerProductMainKindName,
                ManagerProductSubKindName = x.ManagerProductSubKindName,
                ManagerProductSpecificationName = x.ManagerProductSpecificationName,
                EmployeePipelineProductSellPrice = x.EmployeePipelineProductSellPrice,
                EmployeePipelineProductClosingPrice = x.EmployeePipelineProductClosingPrice,
                EmployeePipelineProductCostPrice = x.EmployeePipelineProductCostPrice,
                EmployeePipelineProductGrossProfit = x.EmployeePipelineProductGrossProfit,
                EmployeePipelineProductCount = x.EmployeePipelineProductCount,
                EmployeePipelineProductPurchaseKind = x.EmployeePipelineProductPurchaseKind,
                EmployeePipelineProductContractKind = x.EmployeePipelineProductContractKind,
                EmployeePipelineProductContractText = x.EmployeePipelineProductContractText,
                ManagerProductMainKindCommissionRate = x.ManagerProductMainKindCommissionRate
            }).ToList();
        // 履約期限
        rspObj.EmployeePipelineDueList = coEmpPplDueDbGetManyRspObj.EmployeePipelineDueList?
            .Select(x => new MbsCrmPplLgcGetEmployeePipelineRspDueItemMdl
            {
                EmployeePipelineDueID = x.EmployeePipelineDueID,
                EmployeePipelineDueTime = x.EmployeePipelineDueTime,
                EmployeePipelineDueRemark = x.EmployeePipelineDueRemark
            }).ToList();
        // 發票紀錄
        rspObj.EmployeePipelineBillList = coEmpPplBllDbGetManyRspObj.EmployeePipelineBillList?
            .Select(x => new MbsCrmPplLgcGetEmployeePipelineRspBillItemMdl
            {
                EmployeePipelineBillID = x.EmployeePipelineBillID,
                EmployeePipelineBillPeriodNumber = x.EmployeePipelineBillPeriodNumber,
                EmployeePipelineBillBillTime = x.EmployeePipelineBillBillTime,
                EmployeePipelineBillBillNumber = x.EmployeePipelineBillBillNumber,
                EmployeePipelineBillNoTaxAmount = x.EmployeePipelineBillNoTaxAmount,
                EmployeePipelineBillRemark = x.EmployeePipelineBillRemark,
                EmployeePipelineBillStatus = x.EmployeePipelineBillStatus
            }).ToList();

        return rspObj;
    }

    /// <summary>管理者後台-CRM-商機管理-邏輯服務-更新商機狀態</summary>
    public async Task<MbsCrmPplLgcUpdatePipelineStatusRspMdl> UpdatePipelineStatusAsync(MbsCrmPplLgcUpdatePipelineStatusReqMdl reqObj)
    {
        var rspObj = new MbsCrmPplLgcUpdatePipelineStatusRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        #region 判斷登入令牌與權限

        // mbs, 管理者後台-通用-邏輯-取得登入使用者資訊
        var mbsCmnLgcGetLoginUserInfoReqObj = new MbsCmnLgcGetLoginUserInfoReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken,
            AtomMenu = this._atomMenu,
        };
        var mbsCmnLgcGetLoginUserInfoRspObj = await this._mbsCommonLogical.GetLoginUserInfoAsync(mbsCmnLgcGetLoginUserInfoReqObj);
        if (mbsCmnLgcGetLoginUserInfoRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }
        if (mbsCmnLgcGetLoginUserInfoRspObj.ErrorCode != MbsErrorCodeEnum.Success)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            rspObj.ErrorCode = mbsCmnLgcGetLoginUserInfoRspObj.ErrorCode;
            return rspObj;
        }

        // 判斷權限
        if (mbsCmnLgcGetLoginUserInfoRspObj.AtomPermissionKindIdEdit != DbAtomPermissionKindEnum.Everyone &&
            mbsCmnLgcGetLoginUserInfoRspObj.AtomPermissionKindIdEdit != DbAtomPermissionKindEnum.Self)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");

            // 權限不足
            rspObj.ErrorCode = MbsErrorCodeEnum.PermissionDenied;
            return rspObj;
        }

        #endregion

        // db, 核心-員工-商機-資料庫-取得[管理者後台-CRM-商機管理]
        var coEmpPplDbGetFromMbsCrmPipelineReqObj = new CoEmpPplDbGetFromMbsCrmPipelineReqMdl()
        {
            EmployeePipelineID = reqObj.EmployeePipelineID,
            // 判斷權限,自身or所有人
            EmployeePipelineSalerEmployeeID =
            mbsCmnLgcGetLoginUserInfoRspObj.AtomPermissionKindIdEdit == DbAtomPermissionKindEnum.Self
                                                ? mbsCmnLgcGetLoginUserInfoRspObj.EmployeeID
                                                : null,
        };
        var coEmpPplDbGetFromMbsCrmPipelineRspObj = await this._coEmpPipelineDb.GetFromMbsCrmPipelineAsync(coEmpPplDbGetFromMbsCrmPipelineReqObj);
        if (coEmpPplDbGetFromMbsCrmPipelineRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }
        if (coEmpPplDbGetFromMbsCrmPipelineRspObj.EmployeePipelineSalerEmployeeID.HasValue == false)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // 檢查權限
        if (mbsCmnLgcGetLoginUserInfoRspObj.AtomPermissionKindIdEdit == DbAtomPermissionKindEnum.Self &&
            coEmpPplDbGetFromMbsCrmPipelineRspObj.EmployeePipelineSalerEmployeeID.Value != mbsCmnLgcGetLoginUserInfoRspObj.EmployeeID)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            // 權限不足
            rspObj.ErrorCode = MbsErrorCodeEnum.PermissionDenied;
            return rspObj;
        }

        // 限制AtomPipelineStatus只能Business10Percent至BusinessFailed
        if (reqObj.AtomPipelineStatus.ToInt16() < DbAtomPipelineStatusEnum.Business10Percent.ToInt16() ||
            reqObj.AtomPipelineStatus.ToInt16() > DbAtomPipelineStatusEnum.BusinessFailed.ToInt16())
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        var finalNeedStatus = reqObj.BusinessNeedStatus ?? coEmpPplDbGetFromMbsCrmPipelineRspObj.BusinessNeedStatus;
        var finalTimelineStatus = reqObj.BusinessTimelineStatus ?? coEmpPplDbGetFromMbsCrmPipelineRspObj.BusinessTimelineStatus;
        var finalBudgetStatus = reqObj.BusinessBudgetStatus ?? coEmpPplDbGetFromMbsCrmPipelineRspObj.BusinessBudgetStatus;
        var finalPresentationStatus = reqObj.BusinessPresentationStatus ?? coEmpPplDbGetFromMbsCrmPipelineRspObj.BusinessPresentationStatus;
        var finalQuotationStatus = reqObj.BusinessQuotationStatus ?? coEmpPplDbGetFromMbsCrmPipelineRspObj.BusinessQuotationStatus;
        var finalNegotiationStatus = reqObj.BusinessNegotiationStatus ?? coEmpPplDbGetFromMbsCrmPipelineRspObj.BusinessNegotiationStatus;

        bool IsConfirmed(DbEmployeePipelineStageCheckEnum? status) =>
            status.HasValue && status.Value == DbEmployeePipelineStageCheckEnum.Confirmed;

        bool isStageValid = reqObj.AtomPipelineStatus switch
        {
            DbAtomPipelineStatusEnum.Business10Percent => IsConfirmed(finalNeedStatus),
            DbAtomPipelineStatusEnum.Business30Percent => IsConfirmed(finalNeedStatus)
                                                           && IsConfirmed(finalTimelineStatus)
                                                           && IsConfirmed(finalBudgetStatus)
                                                           && IsConfirmed(finalPresentationStatus),
            DbAtomPipelineStatusEnum.Business70Percent => IsConfirmed(finalPresentationStatus)
                                                           && IsConfirmed(finalQuotationStatus)
                                                           && IsConfirmed(finalNegotiationStatus),
            _ => true
        };

        if (isStageValid == false)
        {
            rspObj.ErrorCode = MbsErrorCodeEnum.PipelineStageConditionNotMet;
            return rspObj;
        }

        var hasStagePayload = reqObj.BusinessNeedStatus.HasValue
                              || reqObj.BusinessTimelineStatus.HasValue
                              || reqObj.BusinessBudgetStatus.HasValue
                              || reqObj.BusinessPresentationStatus.HasValue
                              || reqObj.BusinessQuotationStatus.HasValue
                              || reqObj.BusinessNegotiationStatus.HasValue
                              || reqObj.BusinessStageRemark != null;

        // db, 核心-員工-商機-資料庫-更新
        var coEmpPplDbUpdateReqObj = new CoEmpPplDbUpdateReqMdl()
        {
            EmployeePipelineID = reqObj.EmployeePipelineID,
            AtomPipelineStatus = reqObj.AtomPipelineStatus,
            BusinessNeedStatus = reqObj.BusinessNeedStatus,
            BusinessTimelineStatus = reqObj.BusinessTimelineStatus,
            BusinessBudgetStatus = reqObj.BusinessBudgetStatus,
            BusinessPresentationStatus = reqObj.BusinessPresentationStatus,
            BusinessQuotationStatus = reqObj.BusinessQuotationStatus,
            BusinessNegotiationStatus = reqObj.BusinessNegotiationStatus,
            BusinessStageRemark = reqObj.BusinessStageRemark,
            BusinessStageUpdateEmployeeID = hasStagePayload ? mbsCmnLgcGetLoginUserInfoRspObj.EmployeeID : null,
            UpdateBusinessStageTimestamp = hasStagePayload
        };
        var coEmpPplDbUpdateRspObj = await this._coEmpPipelineDb.UpdateAsync(coEmpPplDbUpdateReqObj);
        if (coEmpPplDbUpdateRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        return rspObj;
    }

    /// <summary>管理者後台-CRM-商機管理-邏輯服務-轉換商機至專案</summary>
    public async Task<MbsCrmPplLgcTransferPipelineToProjectRspMdl> TransferPipelineToProjectAsync(MbsCrmPplLgcTransferPipelineToProjectReqMdl reqObj)
    {
        var rspObj = new MbsCrmPplLgcTransferPipelineToProjectRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        #region 判斷登入令牌與權限

        // mbs, 管理者後台-通用-邏輯-取得登入使用者資訊
        var mbsCmnLgcGetLoginUserInfoReqObj = new MbsCmnLgcGetLoginUserInfoReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken,
            AtomMenu = this._atomMenu,
        };
        var mbsCmnLgcGetLoginUserInfoRspObj = await this._mbsCommonLogical.GetLoginUserInfoAsync(mbsCmnLgcGetLoginUserInfoReqObj);
        if (mbsCmnLgcGetLoginUserInfoRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }
        if (mbsCmnLgcGetLoginUserInfoRspObj.ErrorCode != MbsErrorCodeEnum.Success)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            rspObj.ErrorCode = mbsCmnLgcGetLoginUserInfoRspObj.ErrorCode;
            return rspObj;
        }

        // 判斷權限
        if (mbsCmnLgcGetLoginUserInfoRspObj.AtomPermissionKindIdEdit != DbAtomPermissionKindEnum.Everyone &&
            mbsCmnLgcGetLoginUserInfoRspObj.AtomPermissionKindIdEdit != DbAtomPermissionKindEnum.Self)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            // 權限不足
            rspObj.ErrorCode = MbsErrorCodeEnum.PermissionDenied;
            return rspObj;
        }

        #endregion

        // db, 核心-員工-商機-資料庫-取得[管理者後台-CRM-商機管理]
        var coEmpPplDbGetFromMbsCrmPipelineReqObj = new CoEmpPplDbGetFromMbsCrmPipelineReqMdl()
        {
            EmployeePipelineID = reqObj.EmployeePipelineID,
            // 判斷權限,自身or所有人
            EmployeePipelineSalerEmployeeID =
                mbsCmnLgcGetLoginUserInfoRspObj.AtomPermissionKindIdEdit == DbAtomPermissionKindEnum.Self
                ? mbsCmnLgcGetLoginUserInfoRspObj.EmployeeID
                : null,
        };
        var coEmpPplDbGetFromMbsCrmPipelineRspObj = await this._coEmpPipelineDb.GetFromMbsCrmPipelineAsync(coEmpPplDbGetFromMbsCrmPipelineReqObj);
        if (coEmpPplDbGetFromMbsCrmPipelineRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }
        if (coEmpPplDbGetFromMbsCrmPipelineRspObj.EmployeePipelineSalerEmployeeID.HasValue == false)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // 檢查權限
        if (mbsCmnLgcGetLoginUserInfoRspObj.AtomPermissionKindIdEdit == DbAtomPermissionKindEnum.Self &&
            coEmpPplDbGetFromMbsCrmPipelineRspObj.EmployeePipelineSalerEmployeeID.Value != mbsCmnLgcGetLoginUserInfoRspObj.EmployeeID)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            // 權限不足
            rspObj.ErrorCode = MbsErrorCodeEnum.PermissionDenied;
            return rspObj;
        }

        // 限定商機100%才能轉專案
        if (coEmpPplDbGetFromMbsCrmPipelineRspObj.AtomPipelineStatus != DbAtomPipelineStatusEnum.Business100Percent)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // 取得專案by EmployeePipelineID,有資料則代表已轉專案,回傳錯誤
        // db, 核心-員工-專案-取得
        var coEmpPrjDbGetReqObj = new CoEmpPrjDbGetReqMdl()
        {
            EmployeePipelineID = reqObj.EmployeePipelineID
        };
        var coEmpPrjDbGetRspObj = await this._coEmpProjectDb.GetAsync(coEmpPrjDbGetReqObj);
        if (coEmpPrjDbGetRspObj != default)
        {
            rspObj.ErrorCode = MbsErrorCodeEnum.PipelineAlreadyTransferred;
            return rspObj;
        }

        // db, 核心-員工-專案-資料庫-是否存在（檢查專案代碼是否重複）
        var coEmpPrjDbExistReqObj = new CoEmpPrjDbExistReqMdl()
        {
            EmployeeProjectCode = reqObj.EmployeeProjectCode,
        };
        var coEmpPrjDbExistRspObj = await this._coEmpProjectDb.ExistAsync(coEmpPrjDbExistReqObj);
        if (coEmpPrjDbExistRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }
        if (coEmpPrjDbExistRspObj.IsExist)
        {
            rspObj.ErrorCode = MbsErrorCodeEnum.ProjectCodeDuplicate;
            return rspObj;
        }

        // db, 核心-員工-商機-資料庫-更新狀態為TransferredToProject
        var coEmpPplDbUpdateReqObj = new CoEmpPplDbUpdateReqMdl()
        {
            EmployeePipelineID = reqObj.EmployeePipelineID,
            AtomPipelineStatus = DbAtomPipelineStatusEnum.TransferredToProject,
        };
        var coEmpPplDbUpdateRspObj = await this._coEmpPipelineDb.UpdateAsync(coEmpPplDbUpdateReqObj);
        if (coEmpPplDbUpdateRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // db, 核心-員工-專案-資料庫-新增
        var coEmpPrjDbAddReqObj = new CoEmpPrjDbAddReqMdl()
        {
            EmployeePipelineID = reqObj.EmployeePipelineID,
            ManagerCompanyID = coEmpPplDbGetFromMbsCrmPipelineRspObj.ManagerCompanyID.Value,
            AtomEmployeeProjectStatus = DbAtomEmployeeProjectStatusEnum.NotStarted,
            EmployeeProjectCode = reqObj.EmployeeProjectCode,
            EmployeeProjectName = reqObj.EmployeeProjectName,
            EmployeeProjectStartTime = reqObj.EmployeeProjectStartTime,
            EmployeeProjectEndTime = reqObj.EmployeeProjectEndTime,
        };
        var coEmpPrjDbAddRspObj = await this._coEmpProjectDb.AddAsync(coEmpPrjDbAddReqObj);
        if (coEmpPrjDbAddRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // db, 核心-員工-專案契約-新增
        if (string.IsNullOrWhiteSpace(reqObj.EmployeeProjectContractUrl) == false)
        {
            var coEmpPcDbAddReqObj = new CoEmpPcDbAddReqMdl()
            {
                EmployeeProjectID = coEmpPrjDbAddRspObj.EmployeeProjectID,
                EmployeeProjectContractUrl = reqObj.EmployeeProjectContractUrl,
                EmployeeProjectContractIsNewest = true,
            };
            var coEmpPcDbAddRspObj = await this._coEmpProjectContractDb.AddAsync(coEmpPcDbAddReqObj);
            if (coEmpPcDbAddRspObj == default)
            {
                // db, 核心-員工-商機-資料庫-更新回復Business100Percent
                var coEmpPplDbUpdateBusiness100PercentReqObj = new CoEmpPplDbUpdateReqMdl()
                {
                    EmployeePipelineID = reqObj.EmployeePipelineID,
                    AtomPipelineStatus = DbAtomPipelineStatusEnum.Business100Percent,
                };
                var coEmpPplDbUpdateBusiness100PercentRspObj = await this._coEmpPipelineDb.UpdateAsync(coEmpPplDbUpdateBusiness100PercentReqObj);
                if (coEmpPplDbUpdateBusiness100PercentRspObj == default)
                {
                    this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                    return rspObj;
                }

                // db, 核心-員工-專案-資料庫-移除
                var coEmpPrjDbRemoveReqObj = new CoEmpPrjDbRemoveReqMdl()
                {
                    EmployeeProjectID = coEmpPrjDbAddRspObj.EmployeeProjectID,
                };
                var coEmpPrjDbRemoveRspObj = await this._coEmpProjectDb.RemoveAsync(coEmpPrjDbRemoveReqObj);
                if (coEmpPrjDbRemoveRspObj == default)
                {
                    this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                    return rspObj;
                }

                this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                return rspObj;
            }
        }

        // db, 核心-員工-專案工作計劃書-新增
        if (string.IsNullOrWhiteSpace(reqObj.EmployeeProjectWorkUrl) == false)
        {
            var coEmpPwDbAddReqObj = new CoEmpPwDbAddReqMdl()
            {
                EmployeeProjectID = coEmpPrjDbAddRspObj.EmployeeProjectID,
                EmployeeProjectWorkUrl = reqObj.EmployeeProjectWorkUrl,
                EmployeeProjectWorkIsNewest = true,
            };
            var coEmpPwDbAddRspObj = await this._coEmpProjectWorkDb.AddAsync(coEmpPwDbAddReqObj);
            if (coEmpPwDbAddRspObj == default)
            {
                // db, 核心-員工-商機-資料庫-更新回復Business100Percent
                var coEmpPplDbUpdateBusiness100PercentReqObj = new CoEmpPplDbUpdateReqMdl()
                {
                    EmployeePipelineID = reqObj.EmployeePipelineID,
                    AtomPipelineStatus = DbAtomPipelineStatusEnum.Business100Percent,
                };
                var coEmpPplDbUpdateBusiness100PercentRspObj = await this._coEmpPipelineDb.UpdateAsync(coEmpPplDbUpdateBusiness100PercentReqObj);
                if (coEmpPplDbUpdateBusiness100PercentRspObj == default)
                {
                    this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                    return rspObj;
                }

                // db, 核心-員工-專案-資料庫-移除
                var coEmpPrjDbRemoveReqObj = new CoEmpPrjDbRemoveReqMdl()
                {
                    EmployeeProjectID = coEmpPrjDbAddRspObj.EmployeeProjectID,
                };
                var coEmpPrjDbRemoveRspObj = await this._coEmpProjectDb.RemoveAsync(coEmpPrjDbRemoveReqObj);
                if (coEmpPrjDbRemoveRspObj == default)
                {
                    this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                    return rspObj;
                }

                // db, 核心-員工-專案契約-資料庫-移除多筆
                var coEmpPcDbRemoveReqObj = new CoEmpPcDbRemoveManyReqMdl()
                {
                    EmployeeProjectID = coEmpPrjDbAddRspObj.EmployeeProjectID,
                };
                var coEmpPcDbRemoveRspObj = await this._coEmpProjectContractDb.RemoveManyAsync(coEmpPcDbRemoveReqObj);
                if (coEmpPcDbRemoveRspObj == default)
                {
                    this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                    return rspObj;
                }

                this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                return rspObj;
            }
        }

        // db, 核心-員工-專案成員-新增多筆
        if (reqObj.EmployeeProjectMemberEmployeeList != null && reqObj.EmployeeProjectMemberEmployeeList.Count > 0)
        {
            // 專案成員，輸入的人員 + 老闆
            var employeeProjectMemberList = reqObj.EmployeeProjectMemberEmployeeList
                .Select(x => new CoEmpPmemDbAddManyReqItemMdl()
                {
                    EmployeeProjectID = coEmpPrjDbAddRspObj.EmployeeProjectID,
                    EmployeeID = x.EmployeeID,
                    EmployeeProjectMemberRole = x.EmployeeProjectMemberRole,
                })
                .ToList();
            employeeProjectMemberList.Add(new CoEmpPmemDbAddManyReqItemMdl()
            {
                EmployeeProjectID = coEmpPrjDbAddRspObj.EmployeeProjectID,
                EmployeeID = DbEmployeeConst.GeneralManager.ID,
                EmployeeProjectMemberRole = DbEmployeeProjectMemberRoleEnum.GeneralManager,
            });
            employeeProjectMemberList.OrderBy(x => x.EmployeeProjectMemberRole);

            var coEmpPmemDbAddManyReqObj = new CoEmpPmemDbAddManyReqMdl()
            {
                EmployeeProjectMemberList = employeeProjectMemberList
                    .Select(employee => new CoEmpPmemDbAddManyReqItemMdl
                    {
                        EmployeeProjectID = coEmpPrjDbAddRspObj.EmployeeProjectID,
                        EmployeeID = employee.EmployeeID,
                        EmployeeProjectMemberRole = employee.EmployeeProjectMemberRole
                    })
                    .ToList()
            };
            var coEmpPmemDbAddManyRspObj = await this._coEmpProjectMemberDb.AddManyAsync(coEmpPmemDbAddManyReqObj);
            if (coEmpPmemDbAddManyRspObj == default)
            {
                // db, 核心-員工-商機-資料庫-更新回復Business100Percent
                var coEmpPplDbUpdateBusiness100PercentReqObj = new CoEmpPplDbUpdateReqMdl()
                {
                    EmployeePipelineID = reqObj.EmployeePipelineID,
                    AtomPipelineStatus = DbAtomPipelineStatusEnum.Business100Percent,
                };
                var coEmpPplDbUpdateBusiness100PercentRspObj = await this._coEmpPipelineDb.UpdateAsync(coEmpPplDbUpdateBusiness100PercentReqObj);
                if (coEmpPplDbUpdateBusiness100PercentRspObj == default)
                {
                    this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                    return rspObj;
                }

                // db, 核心-員工-專案-資料庫-移除
                var coEmpPrjDbRemoveReqObj = new CoEmpPrjDbRemoveReqMdl()
                {
                    EmployeeProjectID = coEmpPrjDbAddRspObj.EmployeeProjectID,
                };
                var coEmpPrjDbRemoveRspObj = await this._coEmpProjectDb.RemoveAsync(coEmpPrjDbRemoveReqObj);
                if (coEmpPrjDbRemoveRspObj == default)
                {
                    this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                    return rspObj;
                }

                // db, 核心-員工-專案契約-資料庫-移除多筆
                var coEmpPcDbRemoveReqObj = new CoEmpPcDbRemoveManyReqMdl()
                {
                    EmployeeProjectID = coEmpPrjDbAddRspObj.EmployeeProjectID,
                };
                var coEmpPcDbRemoveRspObj = await this._coEmpProjectContractDb.RemoveManyAsync(coEmpPcDbRemoveReqObj);
                if (coEmpPcDbRemoveRspObj == default)
                {
                    this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                    return rspObj;
                }

                // db, 核心-員工-專案工作計劃書-資料庫-移除多筆
                var coEmpPwDbRemoveReqObj = new CoEmpPwDbRemoveManyReqMdl()
                {
                    EmployeeProjectID = coEmpPrjDbAddRspObj.EmployeeProjectID,
                };
                var coEmpPwDbRemoveRspObj = await this._coEmpProjectWorkDb.RemoveManyAsync(coEmpPwDbRemoveReqObj);
                if (coEmpPwDbRemoveRspObj == default)
                {
                    this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                    return rspObj;
                }

                this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                return rspObj;
            }
        }

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        rspObj.EmployeeProjectID = coEmpPrjDbAddRspObj.EmployeeProjectID;
        return rspObj;
    }

    /// <summary>管理者後台-CRM-商機管理-邏輯服務-新增名單</summary>
    public async Task<MbsCrmPplLgcAddEmployeePipelineRspMdl> AddEmployeePipelineAsync(MbsCrmPplLgcAddEmployeePipelineReqMdl reqObj)
    {
        var rspObj = new MbsCrmPplLgcAddEmployeePipelineRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        #region 判斷登入令牌與權限

        // mbs, 管理者後台-通用-邏輯-取得登入使用者資訊
        var mbsCmnLgcGetLoginUserInfoReqObj = new MbsCmnLgcGetLoginUserInfoReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken,
            AtomMenu = this._atomMenu,
        };
        var mbsCmnLgcGetLoginUserInfoRspObj = await this._mbsCommonLogical.GetLoginUserInfoAsync(mbsCmnLgcGetLoginUserInfoReqObj);
        if (mbsCmnLgcGetLoginUserInfoRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }
        if (mbsCmnLgcGetLoginUserInfoRspObj.ErrorCode != MbsErrorCodeEnum.Success)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            rspObj.ErrorCode = mbsCmnLgcGetLoginUserInfoRspObj.ErrorCode;
            return rspObj;
        }

        // 判斷權限
        if (mbsCmnLgcGetLoginUserInfoRspObj.AtomPermissionKindIdCreate != DbAtomPermissionKindEnum.Self &&
            mbsCmnLgcGetLoginUserInfoRspObj.AtomPermissionKindIdCreate != DbAtomPermissionKindEnum.Everyone)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");

            // 權限不足
            rspObj.ErrorCode = MbsErrorCodeEnum.PermissionDenied;
            return rspObj;
        }

        #endregion

        // 檢查窗口所屬公司ID
        if (reqObj.ContacterList != null && reqObj.ContacterList.Count > 0)
        {
            // db, 核心-管理者-窗口-資料庫服務-取得多筆
            var coMgrCttDbGetManyReqObj = new CoMgrCttDbGetManyReqMdl()
            {
                ManagerContacterIDList = reqObj.ContacterList
                                                    .Select(x => x.ManagerContacterID)
                                                    .Distinct()
                                                    .ToList()
            };
            var coEmpPplContDbGetManyRspObj = await this._coMgrContacterDb.GetManyAsync(coMgrCttDbGetManyReqObj);
            if (coEmpPplContDbGetManyRspObj == default)
            {
                this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                return rspObj;
            }
            // 檢查窗口所屬公司
            if (coEmpPplContDbGetManyRspObj.ManagerContacterList
                                                .Where(x => x.ManagerCompanyID != reqObj.ManagerCompanyID)
                                                .Any())
            {
                this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                return rspObj;
            }

        }

        // db, 核心-員工-商機-資料庫-新增
        var coEmpPplDbAddReqObj = new CoEmpPplDbAddReqMdl()
        {
            ManagerCompanyID = reqObj.ManagerCompanyID,
            ManagerCompanyLocationID = reqObj.ManagerCompanyLocationID,
            AtomPipelineStatus = reqObj.AtomPipelineStatus,
            EmployeePipelineSalerEmployeeID = reqObj.EmployeePipelineSalerEmployeeID,
        };
        var coEmpPplDbAddRspObj = await this._coEmpPipelineDb.AddAsync(coEmpPplDbAddReqObj);
        if (coEmpPplDbAddRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        var employeePipelineID = coEmpPplDbAddRspObj.EmployeePipelineID;

        // 新增商機窗口
        if (reqObj.ContacterList != null && reqObj.ContacterList.Count > 0)
        {
            // db, 核心-員工-商機窗口-資料庫-新增多筆
            var coEmpPplContDbAddManyReqObj = new CoEmpPplContDbAddManyReqMdl()
            {
                EmployeePipelineContacterList = reqObj.ContacterList
                .Select(contacter => new CoEmpPplContDbAddManyReqItemMdl()
                {
                    EmployeePipelineID = employeePipelineID,
                    ManagerContacterID = contacter.ManagerContacterID,
                    EmployeePipelineContacterIsPrimary = contacter.EmployeePipelineContacterIsPrimary,
                }).ToList()
            };
            var coEmpPplContDbAddManyRspObj = await this._coEmpPipelineContacterDb.AddManyAsync(coEmpPplContDbAddManyReqObj);
            if (coEmpPplContDbAddManyRspObj == default)
            {
                // db, 核心-員工-商機-資料庫-移除
                var CoEmpPplDbRemoveReqObj = new CoEmpPplDbRemoveReqMdl()
                {
                    EmployeePipelineID = employeePipelineID
                };
                var coEmpPplDbRemoveRspObj = await this._coEmpPipelineDb.RemoveAsync(CoEmpPplDbRemoveReqObj);
                if (coEmpPplDbRemoveRspObj == default)
                {
                    this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                    return rspObj;
                }

                this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                return rspObj;
            }
        }

        // 新增商機業務開發紀錄
        if (reqObj.SalerTrackingList != null && reqObj.SalerTrackingList.Count > 0)
        {
            // db, 核心-員工-商機業務開發紀錄-資料庫-新增多筆
            var coEmpPplSlrTrkDbAddManyReqObj = new CoEmpPplSlrTrkDbAddManyReqMdl()
            {
                EmployeePipelineSalerTrackingList = reqObj.SalerTrackingList
                .Select(salerTracking => new CoEmpPplSlrTrkDbAddManyReqItemMdl()
                {
                    EmployeePipelineID = employeePipelineID,
                    EmployeePipelineSalerTrackingTime = salerTracking.EmployeePipelineSalerTrackingTime,
                    ManagerContacterID = salerTracking.ManagerContacterID,
                    EmployeePipelineSalerTrackingRemark = salerTracking.EmployeePipelineSalerTrackingRemark,
                }).ToList()
            };
            var coEmpPplSlrTrkDbAddManyRspObj = await this._coEmpPipelineSalerTrackingDb.AddManyAsync(coEmpPplSlrTrkDbAddManyReqObj);
            if (coEmpPplSlrTrkDbAddManyRspObj == default)
            {
                // db, 核心-員工-商機-資料庫-移除
                var CoEmpPplDbRemoveReqObj = new CoEmpPplDbRemoveReqMdl()
                {
                    EmployeePipelineID = employeePipelineID
                };
                var coEmpPplDbRemoveRspObj = await this._coEmpPipelineDb.RemoveAsync(CoEmpPplDbRemoveReqObj);
                if (coEmpPplDbRemoveRspObj == default)
                {
                    this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                    return rspObj;
                }

                // db, 核心-員工-商機窗口-資料庫-移除多筆
                var coEmpPplContDbRemoveManyReqObj = new CoEmpPplContDbRemoveManyReqMdl()
                {
                    EmployeePipelineIDList = new List<int> { employeePipelineID }
                };
                var coEmpPplContDbRemoveManyRspObj = await this._coEmpPipelineContacterDb.RemoveManyAsync(coEmpPplContDbRemoveManyReqObj);
                if (coEmpPplContDbRemoveManyRspObj == default)
                {
                    this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                    return rspObj;
                }

                this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                return rspObj;
            }
        }

        // 新增商機產品
        if (reqObj.ProductList != null && reqObj.ProductList.Count > 0)
        {
            // db, 核心-員工-商機產品-資料庫-新增多筆
            var coEmpPplPrdDbAddManyReqObj = new CoEmpPplPrdDbAddManyReqMdl()
            {
                EmployeePipelineProductList = reqObj.ProductList
                .Select(product => new CoEmpPplPrdDbAddManyReqItemMdl()
                {
                    EmployeePipelineID = employeePipelineID,
                    ManagerCompanyID = reqObj.ManagerCompanyID,
                    ManagerProductID = product.ManagerProductID,
                    ManagerProductSpecificationID = product.ManagerProductSpecificationID,
                    EmployeePipelineProductSellPrice = product.EmployeePipelineProductSellPrice,
                    EmployeePipelineProductClosingPrice = product.EmployeePipelineProductClosingPrice,
                    EmployeePipelineProductCostPrice = product.EmployeePipelineProductCostPrice,
                    EmployeePipelineProductCount = product.EmployeePipelineProductCount,
                    EmployeePipelineProductPurchaseKindID = product.EmployeePipelineProductPurchaseKindID,
                    EmployeePipelineProductContractKindID = product.EmployeePipelineProductContractKindID,
                    EmployeePipelineProductContractText = product.EmployeePipelineProductContractText,
                }).ToList()
            };
            var coEmpPplPrdDbAddManyRspObj = await this._coEmpPipelineProductDb.AddManyAsync(coEmpPplPrdDbAddManyReqObj);
            if (coEmpPplPrdDbAddManyRspObj == default)
            {
                // db, 核心-員工-商機-資料庫-移除
                var CoEmpPplDbRemoveReqObj = new CoEmpPplDbRemoveReqMdl()
                {
                    EmployeePipelineID = employeePipelineID
                };
                var coEmpPplDbRemoveRspObj = await this._coEmpPipelineDb.RemoveAsync(CoEmpPplDbRemoveReqObj);
                if (coEmpPplDbRemoveRspObj == default)
                {
                    this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                    return rspObj;
                }

                // db, 核心-員工-商機窗口-資料庫-移除多筆
                var coEmpPplContDbRemoveManyReqObj = new CoEmpPplContDbRemoveManyReqMdl()
                {
                    EmployeePipelineIDList = new List<int> { employeePipelineID }
                };
                var coEmpPplContDbRemoveManyRspObj = await this._coEmpPipelineContacterDb.RemoveManyAsync(coEmpPplContDbRemoveManyReqObj);
                if (coEmpPplContDbRemoveManyRspObj == default)
                {
                    this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                    return rspObj;
                }

                // db, 核心-員工-商機業務開發紀錄-資料庫-移除多筆
                var coEmpPplSlrTrkDbRemoveManyReqObj = new CoEmpPplSlrTrkDbRemoveManyReqMdl()
                {
                    EmployeePipelineID = employeePipelineID
                };
                var coEmpPplSlrTrkDbRemoveManyRspObj = await this._coEmpPipelineSalerTrackingDb.RemoveManyAsync(coEmpPplSlrTrkDbRemoveManyReqObj);
                if (coEmpPplSlrTrkDbRemoveManyRspObj == default)
                {
                    this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                    return rspObj;
                }

                this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                return rspObj;
            }
        }

        // 新增商機發票紀錄
        if (reqObj.BillList != null && reqObj.BillList.Count > 0)
        {
            // db, 核心-員工-商機發票紀錄-資料庫-新增多筆
            var coEmpPplBllDbAddManyReqObj = new CoEmpPplBllDbAddManyReqMdl()
            {
                EmployeePipelineBillList = reqObj.BillList.Select(bill => new CoEmpPplBllDbAddManyReqItemMdl()
                {
                    EmployeePipelineID = employeePipelineID,
                    EmployeePipelineBillPeriodNumber = bill.EmployeePipelineBillPeriodNumber,
                    EmployeePipelineBillBillTime = bill.EmployeePipelineBillBillTime,
                    EmployeePipelineBillNoTaxAmount = bill.EmployeePipelineBillNoTaxAmount,
                    EmployeePipelineBillRemark = bill.EmployeePipelineBillRemark,
                    EmployeePipelineBillStatus = bill.EmployeePipelineBillStatus,
                }).ToList()
            };
            var coEmpPplBllDbAddManyRspObj = await this._coEmpPipelineBillDb.AddManyAsync(coEmpPplBllDbAddManyReqObj);
            if (coEmpPplBllDbAddManyRspObj == default)
            {
                // db, 核心-員工-商機-資料庫-移除
                var CoEmpPplDbRemoveReqObj = new CoEmpPplDbRemoveReqMdl()
                {
                    EmployeePipelineID = employeePipelineID
                };
                var coEmpPplDbRemoveRspObj = await this._coEmpPipelineDb.RemoveAsync(CoEmpPplDbRemoveReqObj);
                if (coEmpPplDbRemoveRspObj == default)
                {
                    this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                    return rspObj;
                }

                // db, 核心-員工-商機窗口-資料庫-移除多筆
                var coEmpPplContDbRemoveManyReqObj = new CoEmpPplContDbRemoveManyReqMdl()
                {
                    EmployeePipelineIDList = new List<int> { employeePipelineID }
                };
                var coEmpPplContDbRemoveManyRspObj = await this._coEmpPipelineContacterDb.RemoveManyAsync(coEmpPplContDbRemoveManyReqObj);
                if (coEmpPplContDbRemoveManyRspObj == default)
                {
                    this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                    return rspObj;
                }

                // db, 核心-員工-商機業務開發紀錄-資料庫-移除多筆
                var coEmpPplSlrTrkDbRemoveManyReqObj = new CoEmpPplSlrTrkDbRemoveManyReqMdl()
                {
                    EmployeePipelineID = employeePipelineID
                };
                var coEmpPplSlrTrkDbRemoveManyRspObj = await this._coEmpPipelineSalerTrackingDb.RemoveManyAsync(coEmpPplSlrTrkDbRemoveManyReqObj);
                if (coEmpPplSlrTrkDbRemoveManyRspObj == default)
                {
                    this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                    return rspObj;
                }

                // db, 核心-員工-商機產品-資料庫-移除多筆
                var coEmpPplPrdDbRemoveManyReqObj = new CoEmpPplPrdDbRemoveManyReqMdl()
                {
                    EmployeePipelineID = employeePipelineID
                };
                var coEmpPplPrdDbRemoveManyRspObj = await this._coEmpPipelineProductDb.RemoveManyAsync(coEmpPplPrdDbRemoveManyReqObj);
                if (coEmpPplPrdDbRemoveManyRspObj == default)
                {
                    this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                    return rspObj;
                }

                this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                return rspObj;
            }
        }

        // 新增商機履約期限
        if (reqObj.DueList != null && reqObj.DueList.Count > 0)
        {
            // db, 核心-員工-商機履約期限-資料庫-新增多筆
            var coEmpPplDueDbAddManyReqObj = new CoEmpPplDueDbAddManyReqMdl()
            {
                EmployeePipelineDueList = reqObj.DueList.Select(due => new CoEmpPplDueDbAddManyReqItemMdl()
                {
                    EmployeePipelineID = employeePipelineID,
                    EmployeePipelineDueTime = due.EmployeePipelineDueTime,
                    EmployeePipelineDueRemark = due.EmployeePipelineDueRemark,
                }).ToList()
            };
            var coEmpPplDueDbAddManyRspObj = await this._coEmpPipelineDueDb.AddManyAsync(coEmpPplDueDbAddManyReqObj);
            if (coEmpPplDueDbAddManyRspObj == default)
            {
                // db, 核心-員工-商機-資料庫-移除
                var CoEmpPplDbRemoveReqObj = new CoEmpPplDbRemoveReqMdl()
                {
                    EmployeePipelineID = employeePipelineID
                };
                var coEmpPplDbRemoveRspObj = await this._coEmpPipelineDb.RemoveAsync(CoEmpPplDbRemoveReqObj);
                if (coEmpPplDbRemoveRspObj == default)
                {
                    this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                    return rspObj;
                }

                // db, 核心-員工-商機窗口-資料庫-移除多筆
                var coEmpPplContDbRemoveManyReqObj = new CoEmpPplContDbRemoveManyReqMdl()
                {
                    EmployeePipelineIDList = new List<int> { employeePipelineID }
                };
                var coEmpPplContDbRemoveManyRspObj = await this._coEmpPipelineContacterDb.RemoveManyAsync(coEmpPplContDbRemoveManyReqObj);
                if (coEmpPplContDbRemoveManyRspObj == default)
                {
                    this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                    return rspObj;
                }

                // db, 核心-員工-商機業務開發紀錄-資料庫-移除多筆
                var coEmpPplSlrTrkDbRemoveManyReqObj = new CoEmpPplSlrTrkDbRemoveManyReqMdl()
                {
                    EmployeePipelineID = employeePipelineID
                };
                var coEmpPplSlrTrkDbRemoveManyRspObj = await this._coEmpPipelineSalerTrackingDb.RemoveManyAsync(coEmpPplSlrTrkDbRemoveManyReqObj);
                if (coEmpPplSlrTrkDbRemoveManyRspObj == default)
                {
                    this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                    return rspObj;
                }

                // db, 核心-員工-商機產品-資料庫-移除多筆
                var coEmpPplPrdDbRemoveManyReqObj = new CoEmpPplPrdDbRemoveManyReqMdl()
                {
                    EmployeePipelineID = employeePipelineID
                };
                var coEmpPplPrdDbRemoveManyRspObj = await this._coEmpPipelineProductDb.RemoveManyAsync(coEmpPplPrdDbRemoveManyReqObj);
                if (coEmpPplPrdDbRemoveManyRspObj == default)
                {
                    this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                    return rspObj;
                }

                // db, 核心-員工-商機發票紀錄-資料庫-移除多筆
                var coEmpPplBllDbRemoveManyReqObj = new CoEmpPplBllDbRemoveManyReqMdl()
                {
                    EmployeePipelineID = employeePipelineID
                };
                var coEmpPplBllDbRemoveManyRspObj = await this._coEmpPipelineBillDb.RemoveManyAsync(coEmpPplBllDbRemoveManyReqObj);
                if (coEmpPplBllDbRemoveManyRspObj == default)
                {
                    this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                    return rspObj;
                }

                this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                return rspObj;
            }
        }

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        rspObj.EmployeePipelineID = employeePipelineID;
        return rspObj;
    }

    #endregion

    #region 客戶

    /// <summary>管理者後台-CRM-商機管理-邏輯服務-取得客戶</summary>
    public async Task<MbsCrmPplLgcGetEmployeePipelineCompanyRspMdl> GetEmployeePipelineCompanyAsync(MbsCrmPplLgcGetEmployeePipelineCompanyReqMdl reqObj)
    {
        var rspObj = new MbsCrmPplLgcGetEmployeePipelineCompanyRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        #region 判斷登入令牌與權限

        // mbs, 管理者後台-通用-邏輯-取得登入使用者資訊
        var mbsCmnLgcGetLoginUserInfoReqObj = new MbsCmnLgcGetLoginUserInfoReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken,
            AtomMenu = this._atomMenu,
        };
        var mbsCmnLgcGetLoginUserInfoRspObj = await this._mbsCommonLogical.GetLoginUserInfoAsync(mbsCmnLgcGetLoginUserInfoReqObj);
        if (mbsCmnLgcGetLoginUserInfoRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }
        if (mbsCmnLgcGetLoginUserInfoRspObj.ErrorCode != MbsErrorCodeEnum.Success)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");

            rspObj.ErrorCode = mbsCmnLgcGetLoginUserInfoRspObj.ErrorCode;
            return rspObj;
        }

        //判斷權限
        if (mbsCmnLgcGetLoginUserInfoRspObj.AtomPermissionKindIdView != DbAtomPermissionKindEnum.Self &&
            mbsCmnLgcGetLoginUserInfoRspObj.AtomPermissionKindIdView != DbAtomPermissionKindEnum.Everyone)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");

            // 權限不足
            rspObj.ErrorCode = MbsErrorCodeEnum.PermissionDenied;
            return rspObj;
        }

        #endregion

        // db, 核心-員工-商機-資料庫-取得
        var coEmpPplDbGetReqObj = new CoEmpPplDbGetReqMdl()
        {
            EmployeePipelineID = reqObj.EmployeePipelineID
        };
        var coEmpPplDbGetRspObj = await this._coEmpPipelineDb.GetAsync(coEmpPplDbGetReqObj);
        if (coEmpPplDbGetRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }
        if (coEmpPplDbGetRspObj.EmployeePipelineSalerEmployeeID.HasValue == false)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // 檢查權限
        if (mbsCmnLgcGetLoginUserInfoRspObj.AtomPermissionKindIdView == DbAtomPermissionKindEnum.Self &&
            coEmpPplDbGetRspObj.EmployeePipelineSalerEmployeeID.Value != mbsCmnLgcGetLoginUserInfoRspObj.EmployeeID)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            // 權限不足
            rspObj.ErrorCode = MbsErrorCodeEnum.PermissionDenied;
            return rspObj;
        }

        if (coEmpPplDbGetRspObj.ManagerCompanyID.HasValue)
        {
            // db, 核心-管理者-公司-資料庫-取得
            var coMgrCmpDbGetReqObj = new CoMgrCmpDbGetReqMdl()
            {
                ManagerCompanyID = coEmpPplDbGetRspObj.ManagerCompanyID.Value
            };
            var coMgrCmpDbGetRspObj = await this._coMgrCompanyDb.GetAsync(coMgrCmpDbGetReqObj);
            if (coMgrCmpDbGetRspObj == default)
            {
                this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                return rspObj;
            }
            rspObj.ManagerCompanyUnifiedNumber = coMgrCmpDbGetRspObj.ManagerCompanyUnifiedNumber;
            rspObj.ManagerCompanyName = coMgrCmpDbGetRspObj.ManagerCompanyName;
            rspObj.AtomEmployeeRange = coMgrCmpDbGetRspObj.AtomEmployeeRange;
            rspObj.AtomCustomerGrade = coMgrCmpDbGetRspObj.AtomCustomerGrade;

            // db, 核心-管理者-公司主分類-資料庫-取得
            var coMgrCmpMainKdDbGetReqObj = new CoMgrCmpMainKdDbGetReqMdl()
            {
                ManagerCompanyMainKindID = coMgrCmpDbGetRspObj.ManagerCompanyMainKindID
            };
            var coMgrCmpMainKdDbGetRspObj = await this._coMgrCompanyMainKindDb.GetAsync(coMgrCmpMainKdDbGetReqObj);
            if (coMgrCmpMainKdDbGetRspObj == default)
            {
                this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                return rspObj;
            }
            rspObj.ManagerCompanyMainKindName = coMgrCmpMainKdDbGetRspObj.ManagerCompanyMainKindName;

            // db, 核心-管理者-公司子分類-資料庫-取得
            var coMgrCmpSubKdDbGetReqObj = new CoMgrCmpSubKdDbGetReqMdl()
            {
                ManagerCompanySubKindID = coMgrCmpDbGetRspObj.ManagerCompanySubKindID
            };
            var coMgrCmpSubKdDbGetRspObj = await this._coMgrCompanySubKindDb.GetAsync(coMgrCmpSubKdDbGetReqObj);
            if (coMgrCmpSubKdDbGetRspObj == default)
            {
                this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                return rspObj;
            }
            rspObj.ManagerCompanySubKindName = coMgrCmpSubKdDbGetRspObj.ManagerCompanySubKindName;
        }

        if (coEmpPplDbGetRspObj.ManagerCompanyLocationID.HasValue)
        {
            // db, 核心-管理者-公司營業地點-資料庫-取得
            var coMgrCmpLocDbGetReqObj = new CoMgrCmpLocDbGetReqMdl()
            {
                ManagerCompanyLocationID = coEmpPplDbGetRspObj.ManagerCompanyLocationID.Value
            };
            var coMgrCmpLocDbGetRspObj = await this._coMgrCompanyLocationDb.GetAsync(coMgrCmpLocDbGetReqObj);
            if (coMgrCmpLocDbGetRspObj == default)
            {
                this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                return rspObj;
            }
            rspObj.AtomCity = coMgrCmpLocDbGetRspObj.AtomCity;
            rspObj.ManagerCompanyLocationAddress = coMgrCmpLocDbGetRspObj.ManagerCompanyLocationAddress;
            rspObj.ManagerCompanyLocationTelephone = coMgrCmpLocDbGetRspObj.ManagerCompanyLocationTelephone;
            rspObj.ManagerCompanyLocationStatus = coMgrCmpLocDbGetRspObj.ManagerCompanyLocationStatus;
        }

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        return rspObj;
    }

    /// <summary>管理者後台-CRM-商機管理-邏輯服務-更新名單客戶</summary>
    public async Task<MbsCrmPplLgcUpdateEmployeePipelineCompanyRspMdl> UpdateEmployeePipelineCompanyAsync(MbsCrmPplLgcUpdateEmployeePipelineCompanyReqMdl reqObj)
    {
        var rspObj = new MbsCrmPplLgcUpdateEmployeePipelineCompanyRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        #region 判斷登入令牌與權限

        // mbs, 管理者後台-通用-邏輯-取得登入使用者資訊
        var mbsCmnLgcGetLoginUserInfoReqObj = new MbsCmnLgcGetLoginUserInfoReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken,
            AtomMenu = this._atomMenu,
        };
        var mbsCmnLgcGetLoginUserInfoRspObj = await this._mbsCommonLogical.GetLoginUserInfoAsync(mbsCmnLgcGetLoginUserInfoReqObj);
        if (mbsCmnLgcGetLoginUserInfoRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }
        if (mbsCmnLgcGetLoginUserInfoRspObj.ErrorCode != MbsErrorCodeEnum.Success)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");

            rspObj.ErrorCode = mbsCmnLgcGetLoginUserInfoRspObj.ErrorCode;
            return rspObj;
        }

        //判斷權限
        if (mbsCmnLgcGetLoginUserInfoRspObj.AtomPermissionKindIdEdit != DbAtomPermissionKindEnum.Self &&
            mbsCmnLgcGetLoginUserInfoRspObj.AtomPermissionKindIdEdit != DbAtomPermissionKindEnum.Everyone)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");

            // 權限不足
            rspObj.ErrorCode = MbsErrorCodeEnum.PermissionDenied;
            return rspObj;
        }

        #endregion

        // db, 核心-員工-商機-資料庫-取得
        var coEmpPplDbGetReqObj = new CoEmpPplDbGetReqMdl()
        {
            EmployeePipelineID = reqObj.EmployeePipelineID
        };
        var coEmpPplDbGetRspObj = await this._coEmpPipelineDb.GetAsync(coEmpPplDbGetReqObj);
        if (coEmpPplDbGetRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }
        if (coEmpPplDbGetRspObj.EmployeePipelineSalerEmployeeID.HasValue == false)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // 檢查權限
        if (mbsCmnLgcGetLoginUserInfoRspObj.AtomPermissionKindIdEdit == DbAtomPermissionKindEnum.Self &&
            coEmpPplDbGetRspObj.EmployeePipelineSalerEmployeeID.Value != mbsCmnLgcGetLoginUserInfoRspObj.EmployeeID)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            // 權限不足
            rspObj.ErrorCode = MbsErrorCodeEnum.PermissionDenied;
            return rspObj;
        }

        // db, 核心-管理者-公司-資料庫-是否存在
        var coMgrComDbExistReqObj = new CoMgrComDbExistReqMdl()
        {
            ManagerCompanyID = reqObj.ManagerCompanyID
        };
        var coMgrComDbExistRspObj = await this._coMgrCompanyDb.ExistAsync(coMgrComDbExistReqObj);
        if (coMgrComDbExistRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // db, 核心-管理者-公司營業地點-資料庫-取得
        var coMgrCmpLocDbGetReqObj = new CoMgrCmpLocDbGetReqMdl()
        {
            ManagerCompanyLocationID = reqObj.ManagerCompanyLocationID
        };
        var coMgrCmpLocDbGetRspObj = await this._coMgrCompanyLocationDb.GetAsync(coMgrCmpLocDbGetReqObj);
        if (coMgrCmpLocDbGetRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // 檢查公司營業地點跟公司是否有關連
        if (coMgrCmpLocDbGetRspObj.ManagerCompanyID != reqObj.ManagerCompanyID)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // db, 核心-員工-商機-資料庫-更新
        var coEmpPplDbUpdateReqObj = new CoEmpPplDbUpdateReqMdl()
        {
            EmployeePipelineID = reqObj.EmployeePipelineID,
            ManagerCompanyID = reqObj.ManagerCompanyID,
            ManagerCompanyLocationID = reqObj.ManagerCompanyLocationID,
        };
        var coEmpPplDbUpdateRspObj = await this._coEmpPipelineDb.UpdateAsync(coEmpPplDbUpdateReqObj);
        if (coEmpPplDbUpdateRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // 檢查商機窗口跟商機產品的CompanyID是否相同,CompanyID不同則刪除,相同則不更動

        // db, 核心-員工-商機窗口-資料庫-移除多筆根據公司ID不匹配
        var coEmpPplContDbRemoveManyByCompanyIdMismatchReqObj = new CoEmpPplContDbRemoveManyByCompanyIdMismatchReqMdl()
        {
            EmployeePipelineID = reqObj.EmployeePipelineID,
            ManagerCompanyID = reqObj.ManagerCompanyID,
        };
        var coEmpPplContDbRemoveManyByCompanyIdMismatchRspObj = await this._coEmpPipelineContacterDb.RemoveManyByCompanyIdMismatchAsync(coEmpPplContDbRemoveManyByCompanyIdMismatchReqObj);
        if (coEmpPplContDbRemoveManyByCompanyIdMismatchRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // db, 核心-員工-商機產品-資料庫-移除多筆根據公司ID不匹配
        var coEmpPplPrdDbRemoveManyByCompanyIdMismatchReqObj = new CoEmpPplPrdDbRemoveManyByCompanyIdMismatchReqMdl()
        {
            EmployeePipelineID = reqObj.EmployeePipelineID,
            ManagerCompanyID = reqObj.ManagerCompanyID,
        };
        var coEmpPplPrdDbRemoveManyByCompanyIdMismatchRspObj = await this._coEmpPipelineProductDb.RemoveManyByCompanyIdMismatchAsync(coEmpPplPrdDbRemoveManyByCompanyIdMismatchReqObj);
        if (coEmpPplPrdDbRemoveManyByCompanyIdMismatchRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        return rspObj;
    }

    #endregion

    #region 窗口

    /// <summary>管理者後台-CRM-商機管理-邏輯服務-新增名單窗口</summary>
    public async Task<MbsCrmPplLgcAddEmployeePipelineContacterRspMdl> AddEmployeePipelineContacterAsync(MbsCrmPplLgcAddEmployeePipelineContacterReqMdl reqObj)
    {
        var rspObj = new MbsCrmPplLgcAddEmployeePipelineContacterRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        #region 判斷登入令牌與權限

        // mbs, 管理者後台-通用-邏輯-取得登入使用者資訊
        var mbsCmnLgcGetLoginUserInfoReqObj = new MbsCmnLgcGetLoginUserInfoReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken,
            AtomMenu = this._atomMenu,
        };
        var mbsCmnLgcGetLoginUserInfoRspObj = await this._mbsCommonLogical.GetLoginUserInfoAsync(mbsCmnLgcGetLoginUserInfoReqObj);
        if (mbsCmnLgcGetLoginUserInfoRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }
        if (mbsCmnLgcGetLoginUserInfoRspObj.ErrorCode != MbsErrorCodeEnum.Success)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");

            rspObj.ErrorCode = mbsCmnLgcGetLoginUserInfoRspObj.ErrorCode;
            return rspObj;
        }

        //判斷權限
        if (mbsCmnLgcGetLoginUserInfoRspObj.AtomPermissionKindIdCreate != DbAtomPermissionKindEnum.Self &&
            mbsCmnLgcGetLoginUserInfoRspObj.AtomPermissionKindIdCreate != DbAtomPermissionKindEnum.Everyone)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");

            // 權限不足
            rspObj.ErrorCode = MbsErrorCodeEnum.PermissionDenied;
            return rspObj;
        }

        #endregion

        // db, 核心-員工-商機-資料庫-取得
        var coEmpPplDbGetReqObj = new CoEmpPplDbGetReqMdl()
        {
            EmployeePipelineID = reqObj.EmployeePipelineID
        };
        var coEmpPplDbGetRspObj = await this._coEmpPipelineDb.GetAsync(coEmpPplDbGetReqObj);
        if (coEmpPplDbGetRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }
        if (coEmpPplDbGetRspObj.EmployeePipelineSalerEmployeeID.HasValue == false)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // 檢查權限
        if (mbsCmnLgcGetLoginUserInfoRspObj.AtomPermissionKindIdCreate == DbAtomPermissionKindEnum.Self &&
            coEmpPplDbGetRspObj.EmployeePipelineSalerEmployeeID.Value != mbsCmnLgcGetLoginUserInfoRspObj.EmployeeID)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            // 權限不足
            rspObj.ErrorCode = MbsErrorCodeEnum.PermissionDenied;
            return rspObj;
        }

        // db, 核心-管理者-窗口-資料庫服務-取得
        var coMgrCttDbGetReqObj = new CoMgrCttDbGetReqMdl()
        {
            ManagerContacterID = reqObj.ManagerContacterID
        };
        var coMgrCttDbGetRspObj = await this._coMgrContacterDb.GetAsync(coMgrCttDbGetReqObj);
        if (coMgrCttDbGetRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // 檢查窗口-公司跟商機-公司是否為相同
        if (coMgrCttDbGetRspObj.ManagerCompanyID != coEmpPplDbGetRspObj.ManagerCompanyID)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // 主要窗口只能有一筆,批次更新目前的窗口為次要窗口
        if (reqObj.EmployeePipelineContacterIsPrimary == true)
        {
            // db, 核心-員工-商機窗口-資料庫-更新多筆[是否為主要商機窗口]
            var coEmpPplContDbUpdateManyIsPrimaryReqObj = new CoEmpPplContDbUpdateManyIsPrimaryReqMdl()
            {
                EmployeePipelineID = reqObj.EmployeePipelineID,
                EmployeePipelineContacterIsPrimary = false
            };
            var coEmpPplContDbUpdateManyIsPrimaryRspObj = await this._coEmpPipelineContacterDb.UpdateManyIsPrimaryAsync(coEmpPplContDbUpdateManyIsPrimaryReqObj);
            if (coEmpPplContDbUpdateManyIsPrimaryRspObj == default)
            {
                this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                return rspObj;
            }
        }

        // db, 核心-員工-商機窗口-資料庫-新增
        var coEmpPplContDbAddReqObj = new CoEmpPplContDbAddReqMdl()
        {
            EmployeePipelineID = reqObj.EmployeePipelineID,
            ManagerContacterID = reqObj.ManagerContacterID,
            EmployeePipelineContacterIsPrimary = reqObj.EmployeePipelineContacterIsPrimary
        };
        var coEmpPplContDbAddRspObj = await this._coEmpPipelineContacterDb.AddAsync(coEmpPplContDbAddReqObj);
        if (coEmpPplContDbAddRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        return rspObj;
    }

    /// <summary>管理者後台-CRM-商機管理-邏輯服務-更新名單窗口</summary>
    public async Task<MbsCrmPplLgcUpdateEmployeePipelineContacterRspMdl> UpdateEmployeePipelineContacterAsync(MbsCrmPplLgcUpdateEmployeePipelineContacterReqMdl reqObj)
    {
        var rspObj = new MbsCrmPplLgcUpdateEmployeePipelineContacterRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        #region 判斷登入令牌與權限

        // mbs, 管理者後台-通用-邏輯-取得登入使用者資訊
        var mbsCmnLgcGetLoginUserInfoReqObj = new MbsCmnLgcGetLoginUserInfoReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken,
            AtomMenu = this._atomMenu,
        };
        var mbsCmnLgcGetLoginUserInfoRspObj = await this._mbsCommonLogical.GetLoginUserInfoAsync(mbsCmnLgcGetLoginUserInfoReqObj);
        if (mbsCmnLgcGetLoginUserInfoRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }
        if (mbsCmnLgcGetLoginUserInfoRspObj.ErrorCode != MbsErrorCodeEnum.Success)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");

            rspObj.ErrorCode = mbsCmnLgcGetLoginUserInfoRspObj.ErrorCode;
            return rspObj;
        }

        // 判斷權限
        if (mbsCmnLgcGetLoginUserInfoRspObj.AtomPermissionKindIdEdit != DbAtomPermissionKindEnum.Self &&
            mbsCmnLgcGetLoginUserInfoRspObj.AtomPermissionKindIdEdit != DbAtomPermissionKindEnum.Everyone)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");

            // 權限不足
            rspObj.ErrorCode = MbsErrorCodeEnum.PermissionDenied;
            return rspObj;
        }

        #endregion

        // db, 核心-管理者-窗口-資料庫服務-取得
        var coMgrCttDbGetReqObj = new CoMgrCttDbGetReqMdl()
        {
            ManagerContacterID = reqObj.ManagerContacterID
        };
        var coMgrCttDbGetRspObj = await this._coMgrContacterDb.GetAsync(coMgrCttDbGetReqObj);
        if (coMgrCttDbGetRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // db, 核心-員工-商機窗口-資料庫-取得
        var coEmpPplContDbGetReqObj = new CoEmpPplContDbGetReqMdl()
        {
            EmployeePipelineContacterID = reqObj.EmployeePipelineContacterID
        };
        var coEmpPplContDbGetRspObj = await this._coEmpPipelineContacterDb.GetAsync(coEmpPplContDbGetReqObj);
        if (coEmpPplContDbGetRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // db, 核心-員工-商機-資料庫-取得
        var coEmpPplDbGetReqObj = new CoEmpPplDbGetReqMdl()
        {
            EmployeePipelineID = coEmpPplContDbGetRspObj.EmployeePipelineID
        };
        var coEmpPplDbGetRspObj = await this._coEmpPipelineDb.GetAsync(coEmpPplDbGetReqObj);
        if (coEmpPplDbGetRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }
        if (coEmpPplDbGetRspObj.EmployeePipelineSalerEmployeeID.HasValue == false)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // 檢查權限
        if (mbsCmnLgcGetLoginUserInfoRspObj.AtomPermissionKindIdEdit == DbAtomPermissionKindEnum.Self &&
            coEmpPplDbGetRspObj.EmployeePipelineSalerEmployeeID.Value != mbsCmnLgcGetLoginUserInfoRspObj.EmployeeID)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            // 權限不足
            rspObj.ErrorCode = MbsErrorCodeEnum.PermissionDenied;
            return rspObj;
        }

        // 檢查窗口-公司跟商機-公司是否為相同
        if (coMgrCttDbGetRspObj.ManagerCompanyID != coEmpPplDbGetRspObj.ManagerCompanyID)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // 主要窗口只能有一筆,批次更新目前的窗口為次要窗口
        if (reqObj.EmployeePipelineContacterIsPrimary == true)
        {
            // db, 核心-員工-商機窗口-資料庫-更新多筆[是否為主要商機窗口]
            var coEmpPplContDbUpdateManyIsPrimaryReqObj = new CoEmpPplContDbUpdateManyIsPrimaryReqMdl()
            {
                EmployeePipelineID = coEmpPplContDbGetRspObj.EmployeePipelineID,
                EmployeePipelineContacterIsPrimary = false
            };
            var coEmpPplContDbUpdateManyIsPrimaryRspObj = await this._coEmpPipelineContacterDb.UpdateManyIsPrimaryAsync(coEmpPplContDbUpdateManyIsPrimaryReqObj);
            if (coEmpPplContDbUpdateManyIsPrimaryRspObj == default)
            {
                this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                return rspObj;
            }
        }

        // db, 核心-員工-商機窗口-資料庫-更新
        var coEmpPplContDbUpdateReqObj = new CoEmpPplContDbUpdateReqMdl()
        {
            EmployeePipelineContacterID = reqObj.EmployeePipelineContacterID,
            ManagerContacterID = reqObj.ManagerContacterID,
            EmployeePipelineContacterIsPrimary = reqObj.EmployeePipelineContacterIsPrimary
        };
        var coEmpPplContDbUpdateRspObj = await this._coEmpPipelineContacterDb.UpdateAsync(coEmpPplContDbUpdateReqObj);
        if (coEmpPplContDbUpdateRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        return rspObj;
    }

    /// <summary>管理者後台-CRM-商機管理-邏輯服務-刪除名單窗口</summary>
    public async Task<MbsCrmPplLgcRemoveEmployeePipelineContacterRspMdl> RemoveEmployeePipelineContacterAsync(MbsCrmPplLgcRemoveEmployeePipelineContacterReqMdl reqObj)
    {
        var rspObj = new MbsCrmPplLgcRemoveEmployeePipelineContacterRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        #region 判斷登入令牌與權限

        // mbs, 管理者後台-通用-邏輯-取得登入使用者資訊
        var mbsCmnLgcGetLoginUserInfoReqObj = new MbsCmnLgcGetLoginUserInfoReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken,
            AtomMenu = this._atomMenu,
        };
        var mbsCmnLgcGetLoginUserInfoRspObj = await this._mbsCommonLogical.GetLoginUserInfoAsync(mbsCmnLgcGetLoginUserInfoReqObj);
        if (mbsCmnLgcGetLoginUserInfoRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }
        if (mbsCmnLgcGetLoginUserInfoRspObj.ErrorCode != MbsErrorCodeEnum.Success)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");

            rspObj.ErrorCode = mbsCmnLgcGetLoginUserInfoRspObj.ErrorCode;
            return rspObj;
        }

        // 判斷權限
        if (mbsCmnLgcGetLoginUserInfoRspObj.AtomPermissionKindIdDelete != DbAtomPermissionKindEnum.Self &&
            mbsCmnLgcGetLoginUserInfoRspObj.AtomPermissionKindIdDelete != DbAtomPermissionKindEnum.Everyone)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");

            // 權限不足
            rspObj.ErrorCode = MbsErrorCodeEnum.PermissionDenied;
            return rspObj;
        }

        #endregion

        // db, 核心-員工-商機窗口-資料庫-取得
        var coEmpPplContDbGetReqObj = new CoEmpPplContDbGetReqMdl()
        {
            EmployeePipelineContacterID = reqObj.EmployeePipelineContacterID
        };
        var coEmpPplContDbGetRspObj = await this._coEmpPipelineContacterDb.GetAsync(coEmpPplContDbGetReqObj);
        if (coEmpPplContDbGetRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // db, 核心-員工-商機-資料庫-取得
        var coEmpPplDbGetReqObj = new CoEmpPplDbGetReqMdl()
        {
            EmployeePipelineID = coEmpPplContDbGetRspObj.EmployeePipelineID
        };
        var coEmpPplDbGetRspObj = await this._coEmpPipelineDb.GetAsync(coEmpPplDbGetReqObj);
        if (coEmpPplDbGetRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }
        if (coEmpPplDbGetRspObj.EmployeePipelineSalerEmployeeID.HasValue == false)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // 檢查權限
        if (mbsCmnLgcGetLoginUserInfoRspObj.AtomPermissionKindIdDelete == DbAtomPermissionKindEnum.Self &&
            coEmpPplDbGetRspObj.EmployeePipelineSalerEmployeeID.Value != mbsCmnLgcGetLoginUserInfoRspObj.EmployeeID)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            // 權限不足
            rspObj.ErrorCode = MbsErrorCodeEnum.PermissionDenied;
            return rspObj;
        }

        // db, 核心-員工-商機窗口-資料庫-移除
        var coEmpPplContDbRemoveReqObj = new CoEmpPplContDbRemoveReqMdl()
        {
            EmployeePipelineContacterID = reqObj.EmployeePipelineContacterID
        };
        var coEmpPplContDbRemoveRspObj = await this._coEmpPipelineContacterDb.RemoveAsync(coEmpPplContDbRemoveReqObj);
        if (coEmpPplContDbRemoveRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        return rspObj;
    }

    /// <summary>管理者後台-CRM-商機管理-邏輯服務-取得多筆名單窗口</summary>
    public async Task<MbsCrmPplLgcGetManyEmployeePipelineContacterRspMdl> GetManyEmployeePipelineContacterAsync(MbsCrmPplLgcGetManyEmployeePipelineContacterReqMdl reqObj)
    {
        var rspObj = new MbsCrmPplLgcGetManyEmployeePipelineContacterRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        #region 判斷登入令牌與權限

        // mbs, 管理者後台-通用-邏輯-取得登入使用者資訊
        var mbsCmnLgcGetLoginUserInfoReqObj = new MbsCmnLgcGetLoginUserInfoReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken,
            AtomMenu = this._atomMenu,
        };
        var mbsCmnLgcGetLoginUserInfoRspObj = await this._mbsCommonLogical.GetLoginUserInfoAsync(mbsCmnLgcGetLoginUserInfoReqObj);
        if (mbsCmnLgcGetLoginUserInfoRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }
        if (mbsCmnLgcGetLoginUserInfoRspObj.ErrorCode != MbsErrorCodeEnum.Success)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");

            rspObj.ErrorCode = mbsCmnLgcGetLoginUserInfoRspObj.ErrorCode;
            return rspObj;
        }

        // 判斷權限
        if (mbsCmnLgcGetLoginUserInfoRspObj.AtomPermissionKindIdView != DbAtomPermissionKindEnum.Self &&
            mbsCmnLgcGetLoginUserInfoRspObj.AtomPermissionKindIdView != DbAtomPermissionKindEnum.Everyone)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");

            // 權限不足
            rspObj.ErrorCode = MbsErrorCodeEnum.PermissionDenied;
            return rspObj;
        }

        #endregion

        // db, 核心-員工-商機-資料庫-取得
        var coEmpPplDbGetReqObj = new CoEmpPplDbGetReqMdl()
        {
            EmployeePipelineID = reqObj.EmployeePipelineID
        };
        var coEmpPplDbGetRspObj = await this._coEmpPipelineDb.GetAsync(coEmpPplDbGetReqObj);
        if (coEmpPplDbGetRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }
        if (coEmpPplDbGetRspObj.EmployeePipelineSalerEmployeeID.HasValue == false)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // 檢查權限
        if (mbsCmnLgcGetLoginUserInfoRspObj.AtomPermissionKindIdView == DbAtomPermissionKindEnum.Self &&
            coEmpPplDbGetRspObj.EmployeePipelineSalerEmployeeID.Value != mbsCmnLgcGetLoginUserInfoRspObj.EmployeeID)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            // 權限不足
            rspObj.ErrorCode = MbsErrorCodeEnum.PermissionDenied;
            return rspObj;
        }

        // db, 核心-員工-商機窗口-資料庫-取得多筆
        var coEmpPplContDbGetManyReqObj = new CoEmpPplContDbGetManyReqMdl()
        {
            EmployeePipelineID = reqObj.EmployeePipelineID
        };
        var coEmpPplContDbGetManyRspObj = await this._coEmpPipelineContacterDb.GetManyAsync(coEmpPplContDbGetManyReqObj);
        if (coEmpPplContDbGetManyRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // db, 核心-管理者-窗口-資料庫服務-取得多筆
        var coMgrCttDbGetManyReqObj = new CoMgrCttDbGetManyReqMdl()
        {
            ManagerContacterIDList = coEmpPplContDbGetManyRspObj.EmployeePipelineContacterList
                                        .Select(x => x.ManagerContacterID)
                                        .Distinct()
                                        .ToList()
        };
        var coMgrCttDbGetManyRspObj = await this._coMgrContacterDb.GetManyAsync(coMgrCttDbGetManyReqObj);
        if (coMgrCttDbGetManyRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        rspObj.EmployeePipelineContacterList = (
            from epc in coEmpPplContDbGetManyRspObj.EmployeePipelineContacterList
            from mc in coMgrCttDbGetManyRspObj.ManagerContacterList
                        .Where(mc => mc.ManagerContacterID == epc.ManagerContacterID)
            select new MbsCrmPplLgcGetManyEmployeePipelineContacterRspItemMdl
            {
                EmployeePipelineContacterID = epc.EmployeePipelineContacterID,
                ManagerContacterID = mc.ManagerContacterID,
                EmployeePipelineContacterIsPrimary = epc.EmployeePipelineContacterIsPrimary,
                ManagerContacterName = mc.ManagerContacterName,
                ManagerContacterEmail = mc.ManagerContacterEmail,
                ManagerContacterPhone = mc.ManagerContacterPhone,
                ManagerContacterDepartment = mc.ManagerContacterDepartment,
                ManagerContacterJobTitle = mc.ManagerContacterJobTitle,
                ManagerContacterTelephone = mc.ManagerContacterTelephone,
                ManagerContacterIsConsent = mc.ManagerContacterIsConsent,
                ManagerContacterStatus = mc.ManagerContacterStatus,
                AtomRatingKind = mc.AtomRatingKind,
                ManagerContacterRemark = mc.ManagerContacterRemark
            }
        ).ToList();
        return rspObj;
    }

    #endregion

    #region 業務紀錄

    /// <summary>管理者後台-CRM-商機管理-邏輯服務-取得多筆商機業務紀錄</summary>
    public async Task<MbsCrmPplLgcGetManyEmployeePipelineSalerRspMdl> GetManyEmployeePipelineSalerAsync(MbsCrmPplLgcGetManyEmployeePipelineSalerReqMdl reqObj)
    {
        var rspObj = new MbsCrmPplLgcGetManyEmployeePipelineSalerRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        #region 判斷登入令牌與權限

        // mbs, 管理者後台-通用-邏輯-取得登入使用者資訊
        var mbsCmnLgcGetLoginUserInfoReqObj = new MbsCmnLgcGetLoginUserInfoReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken,
            AtomMenu = this._atomMenu,
        };
        var mbsCmnLgcGetLoginUserInfoRspObj = await this._mbsCommonLogical.GetLoginUserInfoAsync(mbsCmnLgcGetLoginUserInfoReqObj);
        if (mbsCmnLgcGetLoginUserInfoRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }
        if (mbsCmnLgcGetLoginUserInfoRspObj.ErrorCode != MbsErrorCodeEnum.Success)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");

            rspObj.ErrorCode = mbsCmnLgcGetLoginUserInfoRspObj.ErrorCode;
            return rspObj;
        }

        // 判斷權限
        if (mbsCmnLgcGetLoginUserInfoRspObj.AtomPermissionKindIdView != DbAtomPermissionKindEnum.Self &&
            mbsCmnLgcGetLoginUserInfoRspObj.AtomPermissionKindIdView != DbAtomPermissionKindEnum.Everyone)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");

            // 權限不足
            rspObj.ErrorCode = MbsErrorCodeEnum.PermissionDenied;
            return rspObj;
        }

        #endregion

        // db, 核心-員工-商機-資料庫-取得
        var coEmpPplDbGetReqObj = new CoEmpPplDbGetReqMdl()
        {
            EmployeePipelineID = reqObj.EmployeePipelineID
        };
        var coEmpPplDbGetRspObj = await this._coEmpPipelineDb.GetAsync(coEmpPplDbGetReqObj);
        if (coEmpPplDbGetRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }
        // 確認商機有業務員工
        if (coEmpPplDbGetRspObj.EmployeePipelineSalerEmployeeID.HasValue == false)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // 如果是Self權限,需要驗證商機的業務員工是否為登入者
        if (mbsCmnLgcGetLoginUserInfoRspObj.AtomPermissionKindIdView == DbAtomPermissionKindEnum.Self &&
            coEmpPplDbGetRspObj.EmployeePipelineSalerEmployeeID.Value != mbsCmnLgcGetLoginUserInfoRspObj.EmployeeID)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");

            // 權限不足
            rspObj.ErrorCode = MbsErrorCodeEnum.PermissionDenied;
            return rspObj;
        }

        // db, 核心-員工-商機業務-資料庫-取得多筆[管理者後台-CRM-商機管理]
        var coEmpPplSlrDbGetManyFromMbsCrmPplReqObj = new CoEmpPplSlrDbGetManyFromMbsCrmPplReqMdl()
        {
            EmployeePipelineID = reqObj.EmployeePipelineID,
            EmployeePipelineSalerStatus = reqObj.EmployeePipelineSalerStatus
        };
        var coEmpPplSlrDbGetManyFromMbsCrmPplRspObj = await this._coEmpPipelineSalerDb.GetManyFromMbsCrmPplAsync(coEmpPplSlrDbGetManyFromMbsCrmPplReqObj);
        if (coEmpPplSlrDbGetManyFromMbsCrmPplRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        rspObj.EmployeePipelineSalerList = coEmpPplSlrDbGetManyFromMbsCrmPplRspObj.EmployeePipelineSalerList
            .Select(x => new MbsCrmPplLgcGetManyEmployeePipelineSalerRspItemMdl
            {
                EmployeePipelineSalerID = x.EmployeePipelineSalerID,
                EmployeePipelineSalerEmployeeName = x.EmployeePipelineSalerEmployeeName,
                EmployeePipelineSalerReplyTime = x.EmployeePipelineSalerReplyTime,
                EmployeePipelineSalerStatus = x.EmployeePipelineSalerStatus,
                EmployeePipelineSalerCreateEmployeeName = x.EmployeePipelineSalerCreateEmployeeName,
                EmployeePipelineSalerRemark = x.EmployeePipelineSalerRemark,
                EmployeePipelineSalerCreateTime = x.EmployeePipelineSalerCreateTime
            })
            .ToList();
        return rspObj;
    }

    /// <summary>管理者後台-CRM-商機管理-邏輯服務-處理商機業務紀錄</summary>
    public async Task<MbsCrmPplLgcHandleEmployeePipelineSalerRspMdl> HandleEmployeePipelineSalerAsync(MbsCrmPplLgcHandleEmployeePipelineSalerReqMdl reqObj)
    {
        var rspObj = new MbsCrmPplLgcHandleEmployeePipelineSalerRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        #region 判斷登入令牌與權限

        // mbs, 管理者後台-通用-邏輯-取得登入使用者資訊
        var mbsCmnLgcGetLoginUserInfoReqObj = new MbsCmnLgcGetLoginUserInfoReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken,
            AtomMenu = this._atomMenu,
        };
        var mbsCmnLgcGetLoginUserInfoRspObj = await this._mbsCommonLogical.GetLoginUserInfoAsync(mbsCmnLgcGetLoginUserInfoReqObj);
        if (mbsCmnLgcGetLoginUserInfoRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }
        if (mbsCmnLgcGetLoginUserInfoRspObj.ErrorCode != MbsErrorCodeEnum.Success)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");

            rspObj.ErrorCode = mbsCmnLgcGetLoginUserInfoRspObj.ErrorCode;
            return rspObj;
        }

        //判斷權限
        if (mbsCmnLgcGetLoginUserInfoRspObj.AtomPermissionKindIdEdit != DbAtomPermissionKindEnum.Self &&
            mbsCmnLgcGetLoginUserInfoRspObj.AtomPermissionKindIdEdit != DbAtomPermissionKindEnum.Everyone)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");

            // 權限不足
            rspObj.ErrorCode = MbsErrorCodeEnum.PermissionDenied;
            return rspObj;
        }

        #endregion

        // db, 核心-員工-商機-資料庫-取得
        var coEmpPplDbGetReqObj = new CoEmpPplDbGetReqMdl()
        {
            EmployeePipelineID = reqObj.EmployeePipelineID
        };
        var coEmpPplDbGetRspObj = await this._coEmpPipelineDb.GetAsync(coEmpPplDbGetReqObj);
        if (coEmpPplDbGetRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }
        // 確認商機有業務員工
        if (coEmpPplDbGetRspObj.EmployeePipelineSalerEmployeeID.HasValue == false)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // 如果是Self權限,需要驗證商機的業務員工是否為登入者
        if (mbsCmnLgcGetLoginUserInfoRspObj.AtomPermissionKindIdEdit == DbAtomPermissionKindEnum.Self &&
            coEmpPplDbGetRspObj.EmployeePipelineSalerEmployeeID.Value != mbsCmnLgcGetLoginUserInfoRspObj.EmployeeID)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");

            // 權限不足
            rspObj.ErrorCode = MbsErrorCodeEnum.PermissionDenied;
            return rspObj;
        }

        // 如果是Self權限,需要驗證商機業務的業務員工是否為登入者
        if (mbsCmnLgcGetLoginUserInfoRspObj.AtomPermissionKindIdEdit == DbAtomPermissionKindEnum.Self &&
            coEmpPplDbGetRspObj.EmployeePipelineSalerEmployeeID != mbsCmnLgcGetLoginUserInfoRspObj.EmployeeID)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");

            // 權限不足
            rspObj.ErrorCode = MbsErrorCodeEnum.PermissionDenied;
            return rspObj;
        }

        // db, 核心-員工-商機業務-資料庫-取得多筆
        var coEmpPplSlrDbGetManyReqObj = new CoEmpPplSlrDbGetManyReqMdl()
        {
            EmployeePipelineID = reqObj.EmployeePipelineID,
        };
        var coEmpPplSlrDbGetManyRspObj = await this._coEmpPipelineSalerDb.GetManyAsync(coEmpPplSlrDbGetManyReqObj);
        if (coEmpPplSlrDbGetManyRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        switch (reqObj.EmployeePipelineSalerStatus)
        {
            case DbAtomEmployeePipelineSalerStatusEnum.Pending:
                {
                    // 尚未回覆
                    this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                    return rspObj;
                }
            case DbAtomEmployeePipelineSalerStatusEnum.Accepted:
                {
                    // 接受               

                    // 取得最後一筆的業務紀錄,確認是否為Pending狀態
                    var lastEmployeePipelineSalerRecord = coEmpPplSlrDbGetManyRspObj.EmployeePipelineSalerList
                        .OrderByDescending(x => x.EmployeePipelineSalerID)
                        .FirstOrDefault();
                    if (lastEmployeePipelineSalerRecord == default ||
                        lastEmployeePipelineSalerRecord.EmployeePipelineSalerStatus != DbAtomEmployeePipelineSalerStatusEnum.Pending)
                    {
                        this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                        return rspObj;
                    }
                    // 檢查業務人員ID是否為登入者ID
                    if (lastEmployeePipelineSalerRecord.EmployeePipelineSalerEmployeeID != mbsCmnLgcGetLoginUserInfoRspObj.EmployeeID)
                    {
                        this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                        return rspObj;
                    }

                    // db, 核心-員工-商機業務-資料庫-新增[接收]
                    var coEmpPplSlrDbAddRejectedReqObj = new CoEmpPplSlrDbAddReqMdl()
                    {
                        EmployeePipelineID = coEmpPplDbGetRspObj.EmployeePipelineID,
                        EmployeePipelineSalerEmployeeID = mbsCmnLgcGetLoginUserInfoRspObj.EmployeeID,
                        EmployeePipelineSalerSalerReplyTime = DateTimeOffset.UtcNow,
                        EmployeePipelineSalerStatus = DbAtomEmployeePipelineSalerStatusEnum.Accepted,
                        CreateEmployeeID = lastEmployeePipelineSalerRecord.EmployeePipelineSalerCreateEmployeeID
                    };
                    var coEmpPplSlrDbAddRejectedRspObj = await this._coEmpPipelineSalerDb.AddAsync(coEmpPplSlrDbAddRejectedReqObj);
                    if (coEmpPplSlrDbAddRejectedRspObj == default)
                    {
                        this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                        return rspObj;
                    }

                    // 商機狀態-商機10%
                    // db, 核心-員工-商機-資料庫-更新
                    var coEmpPplDbUpdateReqObj = new CoEmpPplDbUpdateReqMdl()
                    {
                        EmployeePipelineID = coEmpPplDbGetRspObj.EmployeePipelineID,
                        AtomPipelineStatus = DbAtomPipelineStatusEnum.Business10Percent
                    };
                    var coEmpPplDbUpdateRspObj = await this._coEmpPipelineDb.UpdateAsync(coEmpPplDbUpdateReqObj);
                    if (coEmpPplDbUpdateRspObj == default)
                    {
                        this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                        return rspObj;
                    }

                    // 活動商機數+1
                    if (coEmpPplDbGetRspObj.ManagerActivityID.HasValue)
                    {
                        // db, 核心-管理者-活動-資料庫-增加商機數
                        var incrementEmployeePipelineCountReqObj = new CoMgrActivityDbIncrementEmployeePipelineCountReqMdl()
                        {
                            ManagerActivityID = coEmpPplDbGetRspObj.ManagerActivityID.Value
                        };
                        var incrementEmployeePipelineCountRspObj = await this._coMgrActivityDb.IncrementEmployeePipelineCountAsync(incrementEmployeePipelineCountReqObj);
                        if (incrementEmployeePipelineCountRspObj == default)
                        {
                            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                            return rspObj;
                        }
                    }
                    break;
                }
            case DbAtomEmployeePipelineSalerStatusEnum.Rejected:
                {
                    // 拒絕

                    // 拒絕的備註必填
                    if (string.IsNullOrEmpty(reqObj.EmployeePipelineSalerRemark))
                    {
                        this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                        return rspObj;
                    }

                    // 取得最後一筆的業務紀錄,確認是否為Pending狀態
                    var lastEmployeePipelineSalerRecord = coEmpPplSlrDbGetManyRspObj.EmployeePipelineSalerList
                        .OrderByDescending(x => x.EmployeePipelineSalerID)
                        .FirstOrDefault();
                    if (lastEmployeePipelineSalerRecord == default ||
                        lastEmployeePipelineSalerRecord.EmployeePipelineSalerStatus != DbAtomEmployeePipelineSalerStatusEnum.Pending)
                    {
                        this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                        return rspObj;
                    }
                    // 檢查業務人員ID是否為登入者ID
                    if (lastEmployeePipelineSalerRecord.EmployeePipelineSalerEmployeeID != mbsCmnLgcGetLoginUserInfoRspObj.EmployeeID)
                    {
                        this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                        return rspObj;
                    }

                    // db, 核心-員工-商機業務-資料庫-新增[拒絕]
                    var coEmpPplSlrDbAddRejectedReqObj = new CoEmpPplSlrDbAddReqMdl()
                    {
                        EmployeePipelineID = coEmpPplDbGetRspObj.EmployeePipelineID,
                        EmployeePipelineSalerEmployeeID = mbsCmnLgcGetLoginUserInfoRspObj.EmployeeID,
                        EmployeePipelineSalerSalerReplyTime = DateTimeOffset.UtcNow,
                        EmployeePipelineSalerStatus = DbAtomEmployeePipelineSalerStatusEnum.Rejected,
                        CreateEmployeeID = lastEmployeePipelineSalerRecord.EmployeePipelineSalerCreateEmployeeID,
                        EmployeePipelineSalerRemark = reqObj.EmployeePipelineSalerRemark?.Trim()
                    };
                    var coEmpPplSlrDbAddRejectedRspObj = await this._coEmpPipelineSalerDb.AddAsync(coEmpPplSlrDbAddRejectedReqObj);
                    if (coEmpPplSlrDbAddRejectedRspObj == default)
                    {
                        this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                        return rspObj;
                    }

                    // 商機狀態退回[已完成電銷] & 移除業務員工ID

                    // db, 核心-員工-商機-資料庫-更新
                    var coEmpPplDbUpdateRejectedReqObj = new CoEmpPplDbUpdateReqMdl()
                    {
                        EmployeePipelineID = coEmpPplDbGetRspObj.EmployeePipelineID,
                        AtomPipelineStatus = DbAtomPipelineStatusEnum.CompletedSales,
                        EmployeePipelineSalerEmployeeID = -1,
                    };
                    var coEmpPplDbUpdateRejectedRspObj = await this._coEmpPipelineDb.UpdateAsync(coEmpPplDbUpdateRejectedReqObj);
                    if (coEmpPplDbUpdateRejectedRspObj == default)
                    {
                        this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                        return rspObj;
                    }
                    break;
                }
            case DbAtomEmployeePipelineSalerStatusEnum.Reassigned:
                {
                    // 轉指派業務 (先拒絕,接著新增尚未回覆)

                    // 檢查指派業務ID
                    if (reqObj.EmployeePipelineSalerEmployeeID.HasValue == false)
                    {
                        this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                        return rspObj;
                    }

                    // db, 核心-員工-商機業務-資料庫-新增[拒絕]
                    var coEmpPplSlrDbAddRejectedReqObj = new CoEmpPplSlrDbAddReqMdl()
                    {
                        EmployeePipelineID = coEmpPplDbGetRspObj.EmployeePipelineID,
                        EmployeePipelineSalerEmployeeID = coEmpPplDbGetRspObj.EmployeePipelineSalerEmployeeID.Value,
                        EmployeePipelineSalerSalerReplyTime = DateTimeOffset.UtcNow,
                        EmployeePipelineSalerStatus = DbAtomEmployeePipelineSalerStatusEnum.Rejected,
                        CreateEmployeeID = mbsCmnLgcGetLoginUserInfoRspObj.EmployeeID
                    };
                    var coEmpPplSlrDbAddRejectedRspObj = await this._coEmpPipelineSalerDb.AddAsync(coEmpPplSlrDbAddRejectedReqObj);
                    if (coEmpPplSlrDbAddRejectedRspObj == default)
                    {
                        this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                        return rspObj;
                    }

                    // db, 核心-員工-商機業務-資料庫-新增[尚未回覆]
                    var coEmpPplSlrDbAddReqObj = new CoEmpPplSlrDbAddReqMdl()
                    {
                        EmployeePipelineID = coEmpPplDbGetRspObj.EmployeePipelineID,
                        EmployeePipelineSalerEmployeeID = reqObj.EmployeePipelineSalerEmployeeID.Value,
                        EmployeePipelineSalerStatus = DbAtomEmployeePipelineSalerStatusEnum.Pending,
                        CreateEmployeeID = mbsCmnLgcGetLoginUserInfoRspObj.EmployeeID
                    };
                    var coEmpPplSlrDbAddRspObj = await this._coEmpPipelineSalerDb.AddAsync(coEmpPplSlrDbAddReqObj);
                    if (coEmpPplSlrDbAddRspObj == default)
                    {
                        this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                        return rspObj;
                    }

                    // db, 核心-員工-商機-資料庫-更新[業務員工ID]
                    var coEmpPplDbUpdateRejectedReqObj = new CoEmpPplDbUpdateReqMdl()
                    {
                        EmployeePipelineID = coEmpPplDbGetRspObj.EmployeePipelineID,
                        EmployeePipelineSalerEmployeeID = reqObj.EmployeePipelineSalerEmployeeID.Value,
                    };
                    var coEmpPplDbUpdateRejectedRspObj = await this._coEmpPipelineDb.UpdateAsync(coEmpPplDbUpdateRejectedReqObj);
                    if (coEmpPplDbUpdateRejectedRspObj == default)
                    {
                        this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                        return rspObj;
                    }

                    break;
                }
            default:
                {
                    // 未知
                    this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                    return rspObj;
                }
        }

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        return rspObj;
    }

    #endregion

    #region 商機開發紀錄

    /// <summary>管理者後台-CRM-商機管理-邏輯服務-取得多筆商機業務開發紀錄</summary>
    public async Task<MbsCrmPplLgcGetManyEmployeePipelineSalerTrackingRspMdl> GetManyEmployeePipelineSalerTrackingAsync(MbsCrmPplLgcGetManyEmployeePipelineSalerTrackingReqMdl reqObj)
    {
        var rspObj = new MbsCrmPplLgcGetManyEmployeePipelineSalerTrackingRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        #region 判斷登入令牌與權限

        // mbs, 管理者後台-通用-邏輯-取得登入使用者資訊
        var mbsCmnLgcGetLoginUserInfoReqObj = new MbsCmnLgcGetLoginUserInfoReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken,
            AtomMenu = this._atomMenu,
        };
        var mbsCmnLgcGetLoginUserInfoRspObj = await this._mbsCommonLogical.GetLoginUserInfoAsync(mbsCmnLgcGetLoginUserInfoReqObj);
        if (mbsCmnLgcGetLoginUserInfoRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }
        if (mbsCmnLgcGetLoginUserInfoRspObj.ErrorCode != MbsErrorCodeEnum.Success)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");

            rspObj.ErrorCode = mbsCmnLgcGetLoginUserInfoRspObj.ErrorCode;
            return rspObj;
        }

        //  判斷權限
        if (mbsCmnLgcGetLoginUserInfoRspObj.AtomPermissionKindIdView != DbAtomPermissionKindEnum.Self &&
            mbsCmnLgcGetLoginUserInfoRspObj.AtomPermissionKindIdView != DbAtomPermissionKindEnum.Everyone)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");

            // 權限不足
            rspObj.ErrorCode = MbsErrorCodeEnum.PermissionDenied;
            return rspObj;
        }

        #endregion

        // db, 核心-員工-商機-資料庫-取得
        var coEmpPplDbGetReqObj = new CoEmpPplDbGetReqMdl()
        {
            EmployeePipelineID = reqObj.EmployeePipelineID
        };
        var coEmpPplDbGetRspObj = await this._coEmpPipelineDb.GetAsync(coEmpPplDbGetReqObj);
        if (coEmpPplDbGetRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }
        if (coEmpPplDbGetRspObj.EmployeePipelineSalerEmployeeID.HasValue == false)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // 檢查權限
        if (mbsCmnLgcGetLoginUserInfoRspObj.AtomPermissionKindIdView == DbAtomPermissionKindEnum.Self &&
            coEmpPplDbGetRspObj.EmployeePipelineSalerEmployeeID.Value != mbsCmnLgcGetLoginUserInfoRspObj.EmployeeID)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            // 權限不足
            rspObj.ErrorCode = MbsErrorCodeEnum.PermissionDenied;
            return rspObj;
        }

        // db, 核心-員工-商機業務開發紀錄-資料庫-取得多筆[管理者後台-CRM-商機管理]
        var coEmpPplSlrTrkDbGetManyFromMbsCrmPplReqObj = new CoEmpPplSlrTrkDbGetManyFromMbsCrmPplReqMdl()
        {
            EmployeePipelineID = reqObj.EmployeePipelineID
        };
        var coEmpPplSlrTrkDbGetManyFromMbsCrmPplRspObj = await this._coEmpPipelineSalerTrackingDb.GetManyFromMbsCrmPplAsync(coEmpPplSlrTrkDbGetManyFromMbsCrmPplReqObj);
        if (coEmpPplSlrTrkDbGetManyFromMbsCrmPplRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        rspObj.EmployeePipelineSalerTrackingList = coEmpPplSlrTrkDbGetManyFromMbsCrmPplRspObj.EmployeePipelineSalerTrackingList?
            .Select(x => new MbsCrmPplLgcGetManyEmployeePipelineSalerTrackingRspItemMdl
            {
                EmployeePipelineSalerTrackingID = x.EmployeePipelineSalerTrackingID,
                EmployeePipelineSalerTrackingTime = x.EmployeePipelineSalerTrackingTime,
                ManagerContacterName = x.ManagerContacterName,
                EmployeePipelineSalerTrackingRemark = x.EmployeePipelineSalerTrackingRemark,
                EmployeePipelineSalerTrackingCreateEmployeeName = x.EmployeePipelineSalerTrackingCreateEmployeeName
            })
            .ToList();
        return rspObj;
    }

    /// <summary>管理者後台-CRM-商機管理-邏輯服務-新增商機業務開發紀錄</summary>
    public async Task<MbsCrmPplLgcAddEmployeePipelineSalerTrackingRspMdl> AddEmployeePipelineSalerTrackingAsync(MbsCrmPplLgcAddEmployeePipelineSalerTrackingReqMdl reqObj)
    {
        var rspObj = new MbsCrmPplLgcAddEmployeePipelineSalerTrackingRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        #region 判斷登入令牌與權限

        // mbs, 管理者後台-通用-邏輯-取得登入使用者資訊
        var mbsCmnLgcGetLoginUserInfoReqObj = new MbsCmnLgcGetLoginUserInfoReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken,
            AtomMenu = this._atomMenu,
        };
        var mbsCmnLgcGetLoginUserInfoRspObj = await this._mbsCommonLogical.GetLoginUserInfoAsync(mbsCmnLgcGetLoginUserInfoReqObj);
        if (mbsCmnLgcGetLoginUserInfoRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }
        if (mbsCmnLgcGetLoginUserInfoRspObj.ErrorCode != MbsErrorCodeEnum.Success)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");

            rspObj.ErrorCode = mbsCmnLgcGetLoginUserInfoRspObj.ErrorCode;
            return rspObj;
        }

        //判斷權限
        if (mbsCmnLgcGetLoginUserInfoRspObj.AtomPermissionKindIdCreate != DbAtomPermissionKindEnum.Self &&
            mbsCmnLgcGetLoginUserInfoRspObj.AtomPermissionKindIdCreate != DbAtomPermissionKindEnum.Everyone)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");

            // 權限不足
            rspObj.ErrorCode = MbsErrorCodeEnum.PermissionDenied;
            return rspObj;
        }

        #endregion

        // db, 核心-員工-商機-資料庫-取得
        var coEmpPplDbGetReqObj = new CoEmpPplDbGetReqMdl()
        {
            EmployeePipelineID = reqObj.EmployeePipelineID
        };
        var coEmpPplDbGetRspObj = await this._coEmpPipelineDb.GetAsync(coEmpPplDbGetReqObj);
        if (coEmpPplDbGetRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }
        if (coEmpPplDbGetRspObj.EmployeePipelineSalerEmployeeID.HasValue == false)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // 檢查權限
        if (mbsCmnLgcGetLoginUserInfoRspObj.AtomPermissionKindIdCreate == DbAtomPermissionKindEnum.Self &&
            coEmpPplDbGetRspObj.EmployeePipelineSalerEmployeeID.Value != mbsCmnLgcGetLoginUserInfoRspObj.EmployeeID)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            // 權限不足
            rspObj.ErrorCode = MbsErrorCodeEnum.PermissionDenied;
            return rspObj;
        }

        // db, 核心-管理者-窗口-資料庫服務-取得
        var coMgrCttDbGetReqObj = new CoMgrCttDbGetReqMdl()
        {
            ManagerContacterID = reqObj.ManagerContacterID
        };
        var coMgrCttDbGetRspObj = await this._coMgrContacterDb.GetAsync(coMgrCttDbGetReqObj);
        if (coMgrCttDbGetRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // 檢查窗口-公司跟商機-公司是否為相同
        if (coMgrCttDbGetRspObj.ManagerCompanyID != coEmpPplDbGetRspObj.ManagerCompanyID)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // db, 核心-員工-商機業務開發紀錄-資料庫-新增
        var coEmpPplSlrTrkDbAddReqObj = new CoEmpPplSlrTrkDbAddReqMdl()
        {
            EmployeePipelineID = reqObj.EmployeePipelineID,
            EmployeePipelineSalerTrackingTime = reqObj.EmployeePipelineSalerTrackingTime,
            ManagerContacterID = reqObj.ManagerContacterID,
            EmployeePipelineSalerTrackingRemark = reqObj.EmployeePipelineSalerTrackingRemark,
            EmployeePipelineSalerTrackingCreateEmployeeID = mbsCmnLgcGetLoginUserInfoRspObj.EmployeeID
        };
        var coEmpPplSlrTrkDbAddRspObj = await this._coEmpPipelineSalerTrackingDb.AddAsync(coEmpPplSlrTrkDbAddReqObj);
        if (coEmpPplSlrTrkDbAddRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        rspObj.EmployeePipelineSalerTrackingID = coEmpPplSlrTrkDbAddRspObj.EmployeePipelineSalerTrackingID;
        return rspObj;
    }

    #endregion

    #region 發票紀錄

    /// <summary>管理者後台-CRM-商機管理-邏輯服務-取得商機發票紀錄</summary>
    public async Task<MbsCrmPplLgcGetEmployeePipelineBillRspMdl> GetEmployeePipelineBillAsync(MbsCrmPplLgcGetEmployeePipelineBillReqMdl reqObj)
    {
        var rspObj = new MbsCrmPplLgcGetEmployeePipelineBillRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        #region 判斷登入令牌與權限

        // mbs, 管理者後台-通用-邏輯-取得登入使用者資訊
        var mbsCmnLgcGetLoginUserInfoReqObj = new MbsCmnLgcGetLoginUserInfoReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken,
            AtomMenu = this._atomMenu,
        };
        var mbsCmnLgcGetLoginUserInfoRspObj = await this._mbsCommonLogical.GetLoginUserInfoAsync(mbsCmnLgcGetLoginUserInfoReqObj);
        if (mbsCmnLgcGetLoginUserInfoRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }
        if (mbsCmnLgcGetLoginUserInfoRspObj.ErrorCode != MbsErrorCodeEnum.Success)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");

            rspObj.ErrorCode = mbsCmnLgcGetLoginUserInfoRspObj.ErrorCode;
            return rspObj;
        }

        //判斷權限
        if (mbsCmnLgcGetLoginUserInfoRspObj.AtomPermissionKindIdView != DbAtomPermissionKindEnum.Self &&
            mbsCmnLgcGetLoginUserInfoRspObj.AtomPermissionKindIdView != DbAtomPermissionKindEnum.Everyone)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");

            // 權限不足
            rspObj.ErrorCode = MbsErrorCodeEnum.PermissionDenied;
            return rspObj;
        }

        #endregion

        // db, 核心-員工-商機發票紀錄-資料庫-取得
        var coEmpPplBllDbGetReqObj = new CoEmpPplBllDbGetReqMdl()
        {
            EmployeePipelineBillID = reqObj.EmployeePipelineBillID
        };
        var coEmpPplBllDbGetRspObj = await this._coEmpPipelineBillDb.GetAsync(coEmpPplBllDbGetReqObj);
        if (coEmpPplBllDbGetRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // db, 核心-員工-商機-資料庫-取得
        var coEmpPplDbGetReqObj = new CoEmpPplDbGetReqMdl
        {
            EmployeePipelineID = coEmpPplBllDbGetRspObj.EmployeePipelineID
        };
        var coEmpPplDbGetRspObj = await this._coEmpPipelineDb.GetAsync(coEmpPplDbGetReqObj);
        if (coEmpPplDbGetRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }
        if (coEmpPplDbGetRspObj.EmployeePipelineSalerEmployeeID.HasValue == false)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // 檢查權限
        if (mbsCmnLgcGetLoginUserInfoRspObj.AtomPermissionKindIdView == DbAtomPermissionKindEnum.Self &&
            coEmpPplDbGetRspObj.EmployeePipelineSalerEmployeeID.Value != mbsCmnLgcGetLoginUserInfoRspObj.EmployeeID)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            // 權限不足
            rspObj.ErrorCode = MbsErrorCodeEnum.PermissionDenied;
            return rspObj;
        }

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        rspObj.EmployeePipelineBillID = coEmpPplBllDbGetRspObj.EmployeePipelineBillID;
        rspObj.EmployeePipelineID = coEmpPplBllDbGetRspObj.EmployeePipelineID;
        rspObj.EmployeePipelineBillPeriodNumber = coEmpPplBllDbGetRspObj.EmployeePipelineBillPeriodNumber;
        rspObj.EmployeePipelineBillBillTime = coEmpPplBllDbGetRspObj.EmployeePipelineBillBillTime;
        rspObj.EmployeePipelineBillBillNumber = coEmpPplBllDbGetRspObj.EmployeePipelineBillBillNumber;
        rspObj.EmployeePipelineBillNoTaxAmount = coEmpPplBllDbGetRspObj.EmployeePipelineBillNoTaxAmount;
        rspObj.EmployeePipelineBillStatus = coEmpPplBllDbGetRspObj.EmployeePipelineBillStatus;
        rspObj.EmployeePipelineBillRemark = coEmpPplBllDbGetRspObj.EmployeePipelineBillRemark;
        return rspObj;
    }

    /// <summary>管理者後台-CRM-商機管理-邏輯服務-取得多筆商機發票紀錄</summary>
    public async Task<MbsCrmPplLgcGetManyEmployeePipelineBillRspMdl> GetManyEmployeePipelineBillAsync(MbsCrmPplLgcGetManyEmployeePipelineBillReqMdl reqObj)
    {
        var rspObj = new MbsCrmPplLgcGetManyEmployeePipelineBillRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        #region 判斷登入令牌與權限

        // mbs, 管理者後台-通用-邏輯-取得登入使用者資訊
        var mbsCmnLgcGetLoginUserInfoReqObj = new MbsCmnLgcGetLoginUserInfoReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken,
            AtomMenu = this._atomMenu,
        };
        var mbsCmnLgcGetLoginUserInfoRspObj = await this._mbsCommonLogical.GetLoginUserInfoAsync(mbsCmnLgcGetLoginUserInfoReqObj);
        if (mbsCmnLgcGetLoginUserInfoRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }
        if (mbsCmnLgcGetLoginUserInfoRspObj.ErrorCode != MbsErrorCodeEnum.Success)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");

            rspObj.ErrorCode = mbsCmnLgcGetLoginUserInfoRspObj.ErrorCode;
            return rspObj;
        }

        //判斷權限
        if (mbsCmnLgcGetLoginUserInfoRspObj.AtomPermissionKindIdView != DbAtomPermissionKindEnum.Self &&
            mbsCmnLgcGetLoginUserInfoRspObj.AtomPermissionKindIdView != DbAtomPermissionKindEnum.Everyone)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");

            // 權限不足
            rspObj.ErrorCode = MbsErrorCodeEnum.PermissionDenied;
            return rspObj;
        }

        #endregion

        // db, 核心-員工-商機-資料庫-取得
        var coEmpPplDbGetReqObj = new CoEmpPplDbGetReqMdl
        {
            EmployeePipelineID = reqObj.EmployeePipelineID
        };
        var coEmpPplDbGetRspObj = await this._coEmpPipelineDb.GetAsync(coEmpPplDbGetReqObj);
        if (coEmpPplDbGetRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }
        if (coEmpPplDbGetRspObj.EmployeePipelineSalerEmployeeID.HasValue == false)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // 檢查權限
        if (mbsCmnLgcGetLoginUserInfoRspObj.AtomPermissionKindIdView == DbAtomPermissionKindEnum.Self &&
            coEmpPplDbGetRspObj.EmployeePipelineSalerEmployeeID.Value != mbsCmnLgcGetLoginUserInfoRspObj.EmployeeID)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            // 權限不足
            rspObj.ErrorCode = MbsErrorCodeEnum.PermissionDenied;
            return rspObj;
        }

        // db, 核心-員工-商機發票紀錄-資料庫-取得多筆
        var coEmpPplBllDbGetManyReqObj = new CoEmpPplBllDbGetManyReqMdl()
        {
            EmployeePipelineID = reqObj.EmployeePipelineID
        };
        var coEmpPplBllDbGetManyRspObj = await this._coEmpPipelineBillDb.GetManyAsync(coEmpPplBllDbGetManyReqObj);
        if (coEmpPplBllDbGetManyRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        rspObj.EmployeePipelineBillList = coEmpPplBllDbGetManyRspObj.EmployeePipelineBillList
        .Select(x => new MbsCrmPplLgcGetManyEmployeePipelineBillRspItemMdl
        {
            EmployeePipelineBillID = x.EmployeePipelineBillID,
            EmployeePipelineBillPeriodNumber = x.EmployeePipelineBillPeriodNumber,
            EmployeePipelineBillBillTime = x.EmployeePipelineBillBillTime,
            EmployeePipelineBillNoTaxAmount = x.EmployeePipelineBillNoTaxAmount,
            EmployeePipelineBillRemark = x.EmployeePipelineBillRemark,
        })
        .OrderBy(x => x.EmployeePipelineBillPeriodNumber)
        .ToList();
        return rspObj;
    }

    /// <summary>管理者後台-CRM-商機管理-邏輯服務-新增多筆商機發票紀錄</summary>
    public async Task<MbsCrmPplLgcAddManyEmployeePipelineBillRspMdl> AddManyEmployeePipelineBillAsync(MbsCrmPplLgcAddManyEmployeePipelineBillReqMdl reqObj)
    {
        var rspObj = new MbsCrmPplLgcAddManyEmployeePipelineBillRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        #region 判斷登入令牌與權限

        // mbs, 管理者後台-通用-邏輯-取得登入使用者資訊
        var mbsCmnLgcGetLoginUserInfoReqObj = new MbsCmnLgcGetLoginUserInfoReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken,
            AtomMenu = this._atomMenu,
        };
        var mbsCmnLgcGetLoginUserInfoRspObj = await this._mbsCommonLogical.GetLoginUserInfoAsync(mbsCmnLgcGetLoginUserInfoReqObj);
        if (mbsCmnLgcGetLoginUserInfoRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }
        if (mbsCmnLgcGetLoginUserInfoRspObj.ErrorCode != MbsErrorCodeEnum.Success)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");

            rspObj.ErrorCode = mbsCmnLgcGetLoginUserInfoRspObj.ErrorCode;
            return rspObj;
        }

        //判斷權限
        if (mbsCmnLgcGetLoginUserInfoRspObj.AtomPermissionKindIdCreate != DbAtomPermissionKindEnum.Self &&
            mbsCmnLgcGetLoginUserInfoRspObj.AtomPermissionKindIdCreate != DbAtomPermissionKindEnum.Everyone)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");

            // 權限不足
            rspObj.ErrorCode = MbsErrorCodeEnum.PermissionDenied;
            return rspObj;
        }

        #endregion

        // db, 核心-員工-商機-資料庫-取得
        var coEmpPplDbGetReqObj = new CoEmpPplDbGetReqMdl
        {
            EmployeePipelineID = reqObj.EmployeePipelineID
        };
        var coEmpPplDbGetRspObj = await this._coEmpPipelineDb.GetAsync(coEmpPplDbGetReqObj);
        if (coEmpPplDbGetRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }
        if (coEmpPplDbGetRspObj.EmployeePipelineSalerEmployeeID.HasValue == false)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // 檢查權限
        if (mbsCmnLgcGetLoginUserInfoRspObj.AtomPermissionKindIdCreate == DbAtomPermissionKindEnum.Self &&
            coEmpPplDbGetRspObj.EmployeePipelineSalerEmployeeID.Value != mbsCmnLgcGetLoginUserInfoRspObj.EmployeeID)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            // 權限不足
            rspObj.ErrorCode = MbsErrorCodeEnum.PermissionDenied;
            return rspObj;
        }

        // db, 核心-員工-商機發票紀錄-資料庫-新增多筆
        var coEmpPplBllDbAddManyReqObj = new CoEmpPplBllDbAddManyReqMdl()
        {
            EmployeePipelineBillList = reqObj.EmployeePipelineBillList
            .Select(x => new CoEmpPplBllDbAddManyReqItemMdl()
            {
                EmployeePipelineID = reqObj.EmployeePipelineID,
                EmployeePipelineBillPeriodNumber = x.EmployeePipelineBillPeriodNumber,
                EmployeePipelineBillBillTime = x.EmployeePipelineBillBillTime,
                EmployeePipelineBillNoTaxAmount = x.EmployeePipelineBillNoTaxAmount,
                EmployeePipelineBillRemark = x.EmployeePipelineBillRemark?.Trim(),
                // 未轉專案前發票狀態預設為未結案
                EmployeePipelineBillStatus = DbAtomEmployeePipelineBillStatusEnum.NotCompleted
            })
            .ToList()
        };
        var coEmpPplBllDbAddManyRspObj = await this._coEmpPipelineBillDb.AddManyAsync(coEmpPplBllDbAddManyReqObj);
        if (coEmpPplBllDbAddManyRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        return rspObj;
    }

    /// <summary>管理者後台-CRM-商機管理-邏輯服務-更新商機發票紀錄</summary>
    public async Task<MbsCrmPplLgcUpdateEmployeePipelineRspMdl> UpdateEmployeePipelineBillAsync(MbsCrmPplLgcUpdateEmployeePipelineReqMdl reqObj)
    {
        var rspObj = new MbsCrmPplLgcUpdateEmployeePipelineRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        #region 判斷登入令牌與權限

        // mbs, 管理者後台-通用-邏輯-取得登入使用者資訊
        var mbsCmnLgcGetLoginUserInfoReqObj = new MbsCmnLgcGetLoginUserInfoReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken,
            AtomMenu = this._atomMenu,
        };
        var mbsCmnLgcGetLoginUserInfoRspObj = await this._mbsCommonLogical.GetLoginUserInfoAsync(mbsCmnLgcGetLoginUserInfoReqObj);
        if (mbsCmnLgcGetLoginUserInfoRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }
        if (mbsCmnLgcGetLoginUserInfoRspObj.ErrorCode != MbsErrorCodeEnum.Success)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");

            rspObj.ErrorCode = mbsCmnLgcGetLoginUserInfoRspObj.ErrorCode;
            return rspObj;
        }

        //判斷權限
        if (mbsCmnLgcGetLoginUserInfoRspObj.AtomPermissionKindIdEdit != DbAtomPermissionKindEnum.Self &&
            mbsCmnLgcGetLoginUserInfoRspObj.AtomPermissionKindIdEdit != DbAtomPermissionKindEnum.Everyone)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");

            // 權限不足
            rspObj.ErrorCode = MbsErrorCodeEnum.PermissionDenied;
            return rspObj;
        }

        #endregion

        // db, 核心-員工-商機發票紀錄-資料庫-取得
        var coEmpPplBllDbGetReqObj = new CoEmpPplBllDbGetReqMdl
        {
            EmployeePipelineBillID = reqObj.EmployeePipelineBillID
        };
        var coEmpPplBllDbGetRspObj = await this._coEmpPipelineBillDb.GetAsync(coEmpPplBllDbGetReqObj);
        if (coEmpPplBllDbGetRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // db, 核心-員工-商機-資料庫-取得
        var coEmpPplDbGetReqObj = new CoEmpPplDbGetReqMdl
        {
            EmployeePipelineID = coEmpPplBllDbGetRspObj.EmployeePipelineID
        };
        var coEmpPplDbGetRspObj = await this._coEmpPipelineDb.GetAsync(coEmpPplDbGetReqObj);
        if (coEmpPplDbGetRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }
        if (coEmpPplDbGetRspObj.EmployeePipelineSalerEmployeeID.HasValue == false)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // 檢查權限
        if (mbsCmnLgcGetLoginUserInfoRspObj.AtomPermissionKindIdEdit == DbAtomPermissionKindEnum.Self &&
            coEmpPplDbGetRspObj.EmployeePipelineSalerEmployeeID.Value != mbsCmnLgcGetLoginUserInfoRspObj.EmployeeID)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            // 權限不足
            rspObj.ErrorCode = MbsErrorCodeEnum.PermissionDenied;
            return rspObj;
        }

        // db, 核心-員工-商機發票紀錄-資料庫-更新
        var coEmpPplBllDbUpdateReqObj = new CoEmpPplBllDbUpdateReqMdl()
        {
            EmployeePipelineBillID = reqObj.EmployeePipelineBillID,
            EmployeePipelineBillNumber = reqObj.EmployeePipelineBillNumber,
            EmployeePipelineBillStatus = reqObj.EmployeePipelineBillStatus,
            EmployeePipelineBillRemark = reqObj.EmployeePipelineBillRemark?.Trim(),
        };
        var coEmpPplBllDbUpdateRspObj = await this._coEmpPipelineBillDb.UpdateAsync(coEmpPplBllDbUpdateReqObj);
        if (coEmpPplBllDbUpdateRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        return rspObj;
    }

    /// <summary>管理者後台-CRM-商機管理-邏輯服務-更新多筆商機發票紀錄</summary>
    public async Task<MbsCrmPplLgcUpdateManyEmployeePipelineBillRspMdl> UpdateManyEmployeePipelineBillAsync(MbsCrmPplLgcUpdateManyEmployeePipelineBillReqMdl reqObj)
    {
        var rspObj = new MbsCrmPplLgcUpdateManyEmployeePipelineBillRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        #region 判斷登入令牌與權限

        // mbs, 管理者後台-通用-邏輯-取得登入使用者資訊
        var mbsCmnLgcGetLoginUserInfoReqObj = new MbsCmnLgcGetLoginUserInfoReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken,
            AtomMenu = this._atomMenu,
        };
        var mbsCmnLgcGetLoginUserInfoRspObj = await this._mbsCommonLogical.GetLoginUserInfoAsync(mbsCmnLgcGetLoginUserInfoReqObj);
        if (mbsCmnLgcGetLoginUserInfoRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }
        if (mbsCmnLgcGetLoginUserInfoRspObj.ErrorCode != MbsErrorCodeEnum.Success)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");

            rspObj.ErrorCode = mbsCmnLgcGetLoginUserInfoRspObj.ErrorCode;
            return rspObj;
        }

        //判斷權限
        if (mbsCmnLgcGetLoginUserInfoRspObj.AtomPermissionKindIdEdit != DbAtomPermissionKindEnum.Self &&
            mbsCmnLgcGetLoginUserInfoRspObj.AtomPermissionKindIdEdit != DbAtomPermissionKindEnum.Everyone)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");

            // 權限不足
            rspObj.ErrorCode = MbsErrorCodeEnum.PermissionDenied;
            return rspObj;
        }

        #endregion

        // db, 核心-員工-商機-資料庫-取得
        var coEmpPplDbGetReqObj = new CoEmpPplDbGetReqMdl
        {
            EmployeePipelineID = reqObj.EmployeePipelineID
        };
        var coEmpPplDbGetRspObj = await this._coEmpPipelineDb.GetAsync(coEmpPplDbGetReqObj);
        if (coEmpPplDbGetRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }
        if (coEmpPplDbGetRspObj.EmployeePipelineSalerEmployeeID.HasValue == false)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // 檢查權限
        if (mbsCmnLgcGetLoginUserInfoRspObj.AtomPermissionKindIdEdit == DbAtomPermissionKindEnum.Self &&
            coEmpPplDbGetRspObj.EmployeePipelineSalerEmployeeID.Value != mbsCmnLgcGetLoginUserInfoRspObj.EmployeeID)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            // 權限不足
            rspObj.ErrorCode = MbsErrorCodeEnum.PermissionDenied;
            return rspObj;
        }

        // db, 核心-員工-商機發票紀錄-資料庫-移除多筆
        var coEmpPplBllDbRemoveManyReqObj = new CoEmpPplBllDbRemoveManyReqMdl()
        {
            EmployeePipelineID = reqObj.EmployeePipelineID
        };
        var coEmpPplBllDbRemoveManyRspObj = await this._coEmpPipelineBillDb.RemoveManyAsync(coEmpPplBllDbRemoveManyReqObj);
        if (coEmpPplBllDbRemoveManyRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // db, 核心-員工-商機發票紀錄-資料庫-新增多筆
        var coEmpPplBllDbAddManyReqObj = new CoEmpPplBllDbAddManyReqMdl()
        {
            EmployeePipelineBillList = reqObj.EmployeePipelineBillList
            .Select(x => new CoEmpPplBllDbAddManyReqItemMdl()
            {
                EmployeePipelineID = reqObj.EmployeePipelineID,
                EmployeePipelineBillPeriodNumber = x.EmployeePipelineBillPeriodNumber,
                EmployeePipelineBillBillTime = x.EmployeePipelineBillBillTime,
                EmployeePipelineBillNoTaxAmount = x.EmployeePipelineBillNoTaxAmount,
                EmployeePipelineBillRemark = x.EmployeePipelineBillRemark?.Trim(),
                // 未轉專案前發票狀態預設為未結案
                EmployeePipelineBillStatus = DbAtomEmployeePipelineBillStatusEnum.NotCompleted
            })
            .ToList()
        };
        var coEmpPplBllDbAddManyRspObj = await this._coEmpPipelineBillDb.AddManyAsync(coEmpPplBllDbAddManyReqObj);
        if (coEmpPplBllDbAddManyRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        return rspObj;
    }

    /// <summary>管理者後台-CRM-商機管理-邏輯服務-移除多筆商機發票紀錄</summary>
    public async Task<MbsCrmPplLgcRemoveManyEmployeePipelineBillRspMdl> RemoveManyEmployeePipelineBillAsync(MbsCrmPplLgcRemoveManyEmployeePipelineBillReqMdl reqObj)
    {
        var rspObj = new MbsCrmPplLgcRemoveManyEmployeePipelineBillRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        #region 判斷登入令牌與權限

        // mbs, 管理者後台-通用-邏輯-取得登入使用者資訊
        var mbsCmnLgcGetLoginUserInfoReqObj = new MbsCmnLgcGetLoginUserInfoReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken,
            AtomMenu = this._atomMenu,
        };
        var mbsCmnLgcGetLoginUserInfoRspObj = await this._mbsCommonLogical.GetLoginUserInfoAsync(mbsCmnLgcGetLoginUserInfoReqObj);
        if (mbsCmnLgcGetLoginUserInfoRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }
        if (mbsCmnLgcGetLoginUserInfoRspObj.ErrorCode != MbsErrorCodeEnum.Success)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");

            rspObj.ErrorCode = mbsCmnLgcGetLoginUserInfoRspObj.ErrorCode;
            return rspObj;
        }

        //判斷權限
        if (mbsCmnLgcGetLoginUserInfoRspObj.AtomPermissionKindIdDelete != DbAtomPermissionKindEnum.Self &&
            mbsCmnLgcGetLoginUserInfoRspObj.AtomPermissionKindIdDelete != DbAtomPermissionKindEnum.Everyone)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");

            // 權限不足
            rspObj.ErrorCode = MbsErrorCodeEnum.PermissionDenied;
            return rspObj;
        }

        #endregion

        // db, 核心-員工-商機-資料庫-取得
        var coEmpPplDbGetReqObj = new CoEmpPplDbGetReqMdl
        {
            EmployeePipelineID = reqObj.EmployeePipelineID
        };
        var coEmpPplDbGetRspObj = await this._coEmpPipelineDb.GetAsync(coEmpPplDbGetReqObj);
        if (coEmpPplDbGetRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }
        if (coEmpPplDbGetRspObj.EmployeePipelineSalerEmployeeID.HasValue == false)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // 檢查權限
        if (mbsCmnLgcGetLoginUserInfoRspObj.AtomPermissionKindIdDelete == DbAtomPermissionKindEnum.Self &&
            coEmpPplDbGetRspObj.EmployeePipelineSalerEmployeeID.Value != mbsCmnLgcGetLoginUserInfoRspObj.EmployeeID)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            // 權限不足
            rspObj.ErrorCode = MbsErrorCodeEnum.PermissionDenied;
            return rspObj;
        }

        // db, 核心-員工-商機發票紀錄-資料庫-移除多筆
        var coEmpPplBllDbRemoveManyReqObj = new CoEmpPplBllDbRemoveManyReqMdl()
        {
            EmployeePipelineID = reqObj.EmployeePipelineID
        };
        var coEmpPplBllDbRemoveManyRspObj = await this._coEmpPipelineBillDb.RemoveManyAsync(coEmpPplBllDbRemoveManyReqObj);
        if (coEmpPplBllDbRemoveManyRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        return rspObj;
    }

    /// <summary>管理者後台-CRM-商機管理-邏輯服務-通知開立發票</summary>
    public async Task<MbsCrmPplLgcNotifyBillIssueRspMdl> NotifyBillIssueAsync(MbsCrmPplLgcNotifyBillIssueReqMdl reqObj)
    {
        var rspObj = new MbsCrmPplLgcNotifyBillIssueRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        #region 判斷登入令牌與權限

        // mbs, 管理者後台-通用-邏輯-取得登入使用者資訊
        var mbsCmnLgcGetLoginUserInfoReqObj = new MbsCmnLgcGetLoginUserInfoReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken,
            AtomMenu = this._atomMenu,
        };
        var mbsCmnLgcGetLoginUserInfoRspObj = await this._mbsCommonLogical.GetLoginUserInfoAsync(mbsCmnLgcGetLoginUserInfoReqObj);
        if (mbsCmnLgcGetLoginUserInfoRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }
        if (mbsCmnLgcGetLoginUserInfoRspObj.ErrorCode != MbsErrorCodeEnum.Success)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");

            rspObj.ErrorCode = mbsCmnLgcGetLoginUserInfoRspObj.ErrorCode;
            return rspObj;
        }

        // 判斷權限
        if (mbsCmnLgcGetLoginUserInfoRspObj.AtomPermissionKindIdEdit != DbAtomPermissionKindEnum.Self &&
            mbsCmnLgcGetLoginUserInfoRspObj.AtomPermissionKindIdEdit != DbAtomPermissionKindEnum.Everyone)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");

            rspObj.ErrorCode = MbsErrorCodeEnum.PermissionDenied;
            return rspObj;
        }

        #endregion

        // db, 核心-員工-商機發票紀錄-資料庫-取得
        var coEmpPplBllDbGetReqObj = new CoEmpPplBllDbGetReqMdl
        {
            EmployeePipelineBillID = reqObj.EmployeePipelineBillID
        };
        var coEmpPplBllDbGetRspObj = await this._coEmpPipelineBillDb.GetAsync(coEmpPplBllDbGetReqObj);
        if (coEmpPplBllDbGetRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // 此發票已處理
        if (coEmpPplBllDbGetRspObj.EmployeePipelineBillStatus == DbAtomEmployeePipelineBillStatusEnum.InProgress ||
            coEmpPplBllDbGetRspObj.EmployeePipelineBillStatus == DbAtomEmployeePipelineBillStatusEnum.Completed)
        {
            rspObj.ErrorCode = MbsErrorCodeEnum.Success;
            return rspObj;
        }

        // db, 核心-員工-商機-資料庫-取得
        var coEmpPplDbGetReqObj = new CoEmpPplDbGetReqMdl
        {
            EmployeePipelineID = coEmpPplBllDbGetRspObj.EmployeePipelineID
        };
        var coEmpPplDbGetRspObj = await this._coEmpPipelineDb.GetAsync(coEmpPplDbGetReqObj);
        if (coEmpPplDbGetRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }
        if (coEmpPplDbGetRspObj.EmployeePipelineSalerEmployeeID.HasValue == false)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // 檢查權限
        if (mbsCmnLgcGetLoginUserInfoRspObj.AtomPermissionKindIdEdit == DbAtomPermissionKindEnum.Self &&
            coEmpPplDbGetRspObj.EmployeePipelineSalerEmployeeID.Value != mbsCmnLgcGetLoginUserInfoRspObj.EmployeeID)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            // 權限不足
            rspObj.ErrorCode = MbsErrorCodeEnum.PermissionDenied;
            return rspObj;
        }

        // 限制商機狀態=轉移至專案,才可以通知開立發票   
        if (coEmpPplDbGetRspObj.AtomPipelineStatus != DbAtomPipelineStatusEnum.TransferredToProject)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // 更新發票狀態為處理中
        // db, 核心-員工-商機發票紀錄-資料庫-更新
        var coEmpPplBllDbUpdateReqObj = new CoEmpPplBllDbUpdateReqMdl
        {
            EmployeePipelineBillID = reqObj.EmployeePipelineBillID,
            EmployeePipelineBillStatus = DbAtomEmployeePipelineBillStatusEnum.InProgress
        };
        var coEmpPplBllDbUpdateRspObj = await this._coEmpPipelineBillDb.UpdateAsync(coEmpPplBllDbUpdateReqObj);
        if (coEmpPplBllDbUpdateRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // db, 核心-員工-專案-資料庫-取得從商機ID
        var coEmpPrjDbGetReqObj = new CoEmpPrjDbGetReqMdl
        {
            EmployeePipelineID = coEmpPplBllDbGetRspObj.EmployeePipelineID
        };
        var coEmpPrjDbGetRspObj = await this._coEmpProjectDb.GetAsync(coEmpPrjDbGetReqObj);
        if (coEmpPrjDbGetRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // db, 核心-員工-專案成員-資料庫-取得多筆
        var coEmpPmDbGetManyReqObj = new CoEmpPmDbGetManyReqMdl
        {
            EmployeeProjectID = coEmpPrjDbGetRspObj.EmployeeProjectID
        };
        var coEmpPmDbGetManyRspObj = await this._coEmpProjectMemberDb.GetManyAsync(coEmpPmDbGetManyReqObj);
        if (coEmpPmDbGetManyRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // 判斷專案成員內助理角色人員
        var assistantEmployeeIdList = coEmpPmDbGetManyRspObj.EmployeeProjectMemberList
            .Where(x => x.EmployeeProjectMemberRole == DbEmployeeProjectMemberRoleEnum.Assistant)
            .Select(x => x.EmployeeID)
            .ToList();

        // mail接收人員清單
        var receiverList = new List<string>
        {
            "magrlet@bccs.com.tw",
            "mobby@bccs.com.tw",
        };

        // 查詢專案內助理人員的Email
        if (assistantEmployeeIdList.Any())
        {
            // db, 核心-員工-資料庫-取得多筆Email
            var coEmpInfDbGetManyEmailReqObj = new CoEmpInfDbGetManyEmailReqMdl
            {
                EmployeeIdList = assistantEmployeeIdList
            };
            var coEmpInfDbGetManyEmailRspObj = await this._coEmpInfoDb.GetManyEmailAsync(coEmpInfDbGetManyEmailReqObj);
            if (coEmpInfDbGetManyEmailRspObj == default)
            {
                this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                return rspObj;
            }

            // 加入接收者清單
            var assistantEmails = coEmpInfDbGetManyEmailRspObj.EmployeeList
                .Select(x => x.EmployeeEmail)
                .Where(email => !string.IsNullOrWhiteSpace(email))
                .ToList();

            receiverList.AddRange(assistantEmails);
        }

        #region 發送郵件通知

        // 通知連結
        string notifyUrl = this._gsPlatformConfig.ClientFrontSite + string.Format(this._gsBillConfig.BillNotifyRelativeUri, coEmpPplBllDbGetRspObj.EmployeePipelineID);
        var mailSubject = $"{coEmpPrjDbGetRspObj.EmployeeProjectName} - 發票開立通知 - 期數 {coEmpPplBllDbGetRspObj.EmployeePipelineBillPeriodNumber}";
        var mailBody = $@"
<html>
<body>
    <h2>發票開立通知</h2>
    <p>您好,</p>
    <p>以下發票需要處理:</p>
    <ul>     
        <li>期數: {coEmpPplBllDbGetRspObj.EmployeePipelineBillPeriodNumber}</li>
        <li>開立時間: {coEmpPplBllDbGetRspObj.EmployeePipelineBillBillTime:yyyy-MM-dd}</li>
        <li>未稅金額: {coEmpPplBllDbGetRspObj.EmployeePipelineBillNoTaxAmount}</li>
        <li>備註: {coEmpPplBllDbGetRspObj.EmployeePipelineBillRemark}</li>
    </ul>
    <p>請點擊以下連結進行處理:</p>
    <p><a href=""{notifyUrl}"">
    處理開立發票號碼:{coEmpPplBllDbGetRspObj.EmployeePipelineBillBillNumber}</a></p>
</body>
</html>";

        // 正式環境才發送信件
        if (this._env.EnvironmentName == GsEnvironmentConst.PRODUCTION)
        {
            // smtp, 通用-SMTP-發送
            var cmnSmtpSendReqObj = new CmnSmtpSendReqMdl
            {
                Config = new CmnSmtpConfig
                {
                    Host = this._gsSmtpConfig.Host,
                    Port = this._gsSmtpConfig.Port,
                    EnableSsl = this._gsSmtpConfig.EnableSsl,
                    Username = this._gsSmtpConfig.Username,
                    Password = this._gsSmtpConfig.Password,
                    SenderEmail = this._gsSmtpConfig.FromEmail,
                    SenderName = this._gsSmtpConfig.FromName
                },
                ReceiverList = receiverList,
                Subject = mailSubject,
                Body = mailBody,
                IsHtml = true
            };
            var cmnSmtpSendRspObj = await this._cmnSmtp.SendAsync(cmnSmtpSendReqObj);
            if (cmnSmtpSendRspObj == default)
            {
                this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                return rspObj;
            }
            if (cmnSmtpSendRspObj.IsSuccess == false)
            {
                this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                return rspObj;
            }
        }

        #endregion

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        return rspObj;
    }

    /// <summary>管理者後台-CRM-商機管理-邏輯服務-確認開立發票</summary>
    public async Task<MbsCrmPplLgcConfirmBillIssueRspMdl> ConfirmBillIssueAsync(MbsCrmPplLgcConfirmBillIssueReqMdl reqObj)
    {
        var rspObj = new MbsCrmPplLgcConfirmBillIssueRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        #region 判斷登入令牌與權限

        // mbs, 管理者後台-通用-邏輯-取得登入使用者資訊
        var mbsCmnLgcGetLoginUserInfoReqObj = new MbsCmnLgcGetLoginUserInfoReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken,
            AtomMenu = this._atomMenu,
        };
        var mbsCmnLgcGetLoginUserInfoRspObj = await this._mbsCommonLogical.GetLoginUserInfoAsync(mbsCmnLgcGetLoginUserInfoReqObj);
        if (mbsCmnLgcGetLoginUserInfoRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }
        if (mbsCmnLgcGetLoginUserInfoRspObj.ErrorCode != MbsErrorCodeEnum.Success)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");

            rspObj.ErrorCode = mbsCmnLgcGetLoginUserInfoRspObj.ErrorCode;
            return rspObj;
        }

        // 判斷權限
        if (mbsCmnLgcGetLoginUserInfoRspObj.AtomPermissionKindIdEdit != DbAtomPermissionKindEnum.Self &&
            mbsCmnLgcGetLoginUserInfoRspObj.AtomPermissionKindIdEdit != DbAtomPermissionKindEnum.Everyone)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");

            rspObj.ErrorCode = MbsErrorCodeEnum.PermissionDenied;
            return rspObj;
        }

        #endregion

        // db, 核心-員工-商機發票紀錄-資料庫-取得
        var coEmpPplBllDbGetReqObj = new CoEmpPplBllDbGetReqMdl
        {
            EmployeePipelineBillID = reqObj.EmployeePipelineBillID
        };
        var coEmpPplBllDbGetRspObj = await this._coEmpPipelineBillDb.GetAsync(coEmpPplBllDbGetReqObj);
        if (coEmpPplBllDbGetRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // 此發票已處理
        if (coEmpPplBllDbGetRspObj.EmployeePipelineBillStatus == DbAtomEmployeePipelineBillStatusEnum.Completed)
        {
            rspObj.ErrorCode = MbsErrorCodeEnum.Success;
            return rspObj;
        }
        // 檢查發票狀態
        if (coEmpPplBllDbGetRspObj.EmployeePipelineBillStatus == DbAtomEmployeePipelineBillStatusEnum.NotCompleted ||
            coEmpPplBllDbGetRspObj.EmployeePipelineBillStatus == DbAtomEmployeePipelineBillStatusEnum.Undefined)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // db, 核心-員工-商機-資料庫-取得
        var coEmpPplDbGetReqObj = new CoEmpPplDbGetReqMdl
        {
            EmployeePipelineID = coEmpPplBllDbGetRspObj.EmployeePipelineID
        };
        var coEmpPplDbGetRspObj = await this._coEmpPipelineDb.GetAsync(coEmpPplDbGetReqObj);
        if (coEmpPplDbGetRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }
        if (coEmpPplDbGetRspObj.EmployeePipelineSalerEmployeeID.HasValue == false)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // 檢查權限
        if (mbsCmnLgcGetLoginUserInfoRspObj.AtomPermissionKindIdEdit == DbAtomPermissionKindEnum.Self &&
            coEmpPplDbGetRspObj.EmployeePipelineSalerEmployeeID.Value != mbsCmnLgcGetLoginUserInfoRspObj.EmployeeID)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            // 權限不足
            rspObj.ErrorCode = MbsErrorCodeEnum.PermissionDenied;
            return rspObj;
        }

        // db, 核心-員工-商機發票紀錄-資料庫-更新
        var coEmpPplBllDbUpdateReqObj = new CoEmpPplBllDbUpdateReqMdl
        {
            EmployeePipelineBillID = reqObj.EmployeePipelineBillID,
            EmployeePipelineBillNumber = reqObj.EmployeePipelineBillNumber?.Trim(),
            EmployeePipelineBillRemark = reqObj.EmployeePipelineBillRemark?.Trim(),
            EmployeePipelineBillStatus = DbAtomEmployeePipelineBillStatusEnum.Completed
        };
        var coEmpPplBllDbUpdateRspObj = await this._coEmpPipelineBillDb.UpdateAsync(coEmpPplBllDbUpdateReqObj);
        if (coEmpPplBllDbUpdateRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        return rspObj;
    }

    #endregion

    #region 履約期限通知

    /// <summary>管理者後台-CRM-商機管理-邏輯服務-取得商機履約期限</summary>
    public async Task<MbsCrmPplLgcGetEmployeePipelineDueRspMdl> GetEmployeePipelineDueAsync(MbsCrmPplLgcGetEmployeePipelineDueReqMdl reqObj)
    {
        var rspObj = new MbsCrmPplLgcGetEmployeePipelineDueRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        #region 判斷登入令牌與權限

        // mbs, 管理者後台-通用-邏輯-取得登入使用者資訊
        var mbsCmnLgcGetLoginUserInfoReqObj = new MbsCmnLgcGetLoginUserInfoReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken,
            AtomMenu = this._atomMenu,
        };
        var mbsCmnLgcGetLoginUserInfoRspObj = await this._mbsCommonLogical.GetLoginUserInfoAsync(mbsCmnLgcGetLoginUserInfoReqObj);
        if (mbsCmnLgcGetLoginUserInfoRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }
        if (mbsCmnLgcGetLoginUserInfoRspObj.ErrorCode != MbsErrorCodeEnum.Success)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");

            rspObj.ErrorCode = mbsCmnLgcGetLoginUserInfoRspObj.ErrorCode;
            return rspObj;
        }

        //判斷權限
        if (mbsCmnLgcGetLoginUserInfoRspObj.AtomPermissionKindIdView != DbAtomPermissionKindEnum.Self &&
            mbsCmnLgcGetLoginUserInfoRspObj.AtomPermissionKindIdView != DbAtomPermissionKindEnum.Everyone)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");

            // 權限不足
            rspObj.ErrorCode = MbsErrorCodeEnum.PermissionDenied;
            return rspObj;
        }

        #endregion

        // db, 核心-員工-商機履約期限-資料庫-取得
        var coEmpPplDueDbGetReqObj = new CoEmpPplDueDbGetReqMdl()
        {
            EmployeePipelineDueID = reqObj.EmployeePipelineDueID
        };
        var coEmpPplDueDbGetRspObj = await this._coEmpPipelineDueDb.GetAsync(coEmpPplDueDbGetReqObj);
        if (coEmpPplDueDbGetRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // db, 核心-員工-商機-資料庫-取得
        var coEmpPplDbGetReqObj = new CoEmpPplDbGetReqMdl()
        {
            EmployeePipelineID = coEmpPplDueDbGetRspObj.EmployeePipelineID
        };
        var coEmpPplDbGetRspObj = await this._coEmpPipelineDb.GetAsync(coEmpPplDbGetReqObj);
        if (coEmpPplDbGetRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }
        if (coEmpPplDbGetRspObj.EmployeePipelineSalerEmployeeID.HasValue == false)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // 如果是Self權限,需要驗證商機的業務員工是否為登入者
        if (mbsCmnLgcGetLoginUserInfoRspObj.AtomPermissionKindIdView == DbAtomPermissionKindEnum.Self &&
            coEmpPplDbGetRspObj.EmployeePipelineSalerEmployeeID.Value != mbsCmnLgcGetLoginUserInfoRspObj.EmployeeID)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");

            // 權限不足
            rspObj.ErrorCode = MbsErrorCodeEnum.PermissionDenied;
            return rspObj;
        }

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        rspObj.EmployeePipelineDueID = coEmpPplDueDbGetRspObj.EmployeePipelineDueID;
        rspObj.EmployeePipelineID = coEmpPplDueDbGetRspObj.EmployeePipelineID;
        rspObj.EmployeePipelineDueTime = coEmpPplDueDbGetRspObj.EmployeePipelineDueTime;
        rspObj.EmployeePipelineDueRemark = coEmpPplDueDbGetRspObj.EmployeePipelineDueRemark;
        return rspObj;
    }

    /// <summary>管理者後台-CRM-商機管理-邏輯服務-取得多筆商機履約期限</summary>
    public async Task<MbsCrmPplLgcGetManyEmployeePipelineDueRspMdl> GetManyEmployeePipelineDueAsync(MbsCrmPplLgcGetManyEmployeePipelineDueReqMdl reqObj)
    {
        var rspObj = new MbsCrmPplLgcGetManyEmployeePipelineDueRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        #region 判斷登入令牌與權限

        // mbs, 管理者後台-通用-邏輯-取得登入使用者資訊
        var mbsCmnLgcGetLoginUserInfoReqObj = new MbsCmnLgcGetLoginUserInfoReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken,
            AtomMenu = this._atomMenu,
        };
        var mbsCmnLgcGetLoginUserInfoRspObj = await this._mbsCommonLogical.GetLoginUserInfoAsync(mbsCmnLgcGetLoginUserInfoReqObj);
        if (mbsCmnLgcGetLoginUserInfoRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }
        if (mbsCmnLgcGetLoginUserInfoRspObj.ErrorCode != MbsErrorCodeEnum.Success)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");

            rspObj.ErrorCode = mbsCmnLgcGetLoginUserInfoRspObj.ErrorCode;
            return rspObj;
        }

        // 判斷權限
        if (mbsCmnLgcGetLoginUserInfoRspObj.AtomPermissionKindIdView != DbAtomPermissionKindEnum.Self &&
            mbsCmnLgcGetLoginUserInfoRspObj.AtomPermissionKindIdView != DbAtomPermissionKindEnum.Everyone)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");

            // 權限不足
            rspObj.ErrorCode = MbsErrorCodeEnum.PermissionDenied;
            return rspObj;
        }

        #endregion

        // db, 核心-員工-商機-資料庫-取得
        var coEmpPplDbGetReqObj = new CoEmpPplDbGetReqMdl()
        {
            EmployeePipelineID = reqObj.EmployeePipelineID
        };
        var coEmpPplDbGetRspObj = await this._coEmpPipelineDb.GetAsync(coEmpPplDbGetReqObj);
        if (coEmpPplDbGetRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }
        if (coEmpPplDbGetRspObj.EmployeePipelineSalerEmployeeID.HasValue == false)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // 如果是Self權限,需要驗證商機的業務員工是否為登入者
        if (mbsCmnLgcGetLoginUserInfoRspObj.AtomPermissionKindIdView == DbAtomPermissionKindEnum.Self &&
            coEmpPplDbGetRspObj.EmployeePipelineSalerEmployeeID.Value != mbsCmnLgcGetLoginUserInfoRspObj.EmployeeID)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");

            // 權限不足
            rspObj.ErrorCode = MbsErrorCodeEnum.PermissionDenied;
            return rspObj;
        }

        // db, 核心-員工-商機履約期限-資料庫-取得多筆
        var coEmpPplDueDbGetManyReqObj = new CoEmpPplDueDbGetManyReqMdl()
        {
            EmployeePipelineID = reqObj.EmployeePipelineID
        };
        var coEmpPplDueDbGetManyRspObj = await this._coEmpPipelineDueDb.GetManyAsync(coEmpPplDueDbGetManyReqObj);
        if (coEmpPplDueDbGetManyRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        rspObj.EmployeePipelineDueList = coEmpPplDueDbGetManyRspObj.EmployeePipelineDueList
            .Select(x => new MbsCrmPplLgcGetManyEmployeePipelineDueRspItemMdl
            {
                EmployeePipelineDueID = x.EmployeePipelineDueID,
                EmployeePipelineID = x.EmployeePipelineID,
                EmployeePipelineDueTime = x.EmployeePipelineDueTime,
                EmployeePipelineDueRemark = x.EmployeePipelineDueRemark,
            })
            .ToList();
        return rspObj;
    }

    /// <summary>管理者後台-CRM-商機管理-邏輯服務-新增商機履約期限</summary>
    public async Task<MbsCrmPplLgcAddEmployeePipelineDueRspMdl> AddEmployeePipelineDueAsync(MbsCrmPplLgcAddEmployeePipelineDueReqMdl reqObj)
    {
        var rspObj = new MbsCrmPplLgcAddEmployeePipelineDueRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        #region 判斷登入令牌與權限

        // mbs, 管理者後台-通用-邏輯-取得登入使用者資訊
        var mbsCmnLgcGetLoginUserInfoReqObj = new MbsCmnLgcGetLoginUserInfoReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken,
            AtomMenu = this._atomMenu,
        };
        var mbsCmnLgcGetLoginUserInfoRspObj = await this._mbsCommonLogical.GetLoginUserInfoAsync(mbsCmnLgcGetLoginUserInfoReqObj);
        if (mbsCmnLgcGetLoginUserInfoRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }
        if (mbsCmnLgcGetLoginUserInfoRspObj.ErrorCode != MbsErrorCodeEnum.Success)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");

            rspObj.ErrorCode = mbsCmnLgcGetLoginUserInfoRspObj.ErrorCode;
            return rspObj;
        }

        // 判斷權限
        if (mbsCmnLgcGetLoginUserInfoRspObj.AtomPermissionKindIdCreate != DbAtomPermissionKindEnum.Self &&
            mbsCmnLgcGetLoginUserInfoRspObj.AtomPermissionKindIdCreate != DbAtomPermissionKindEnum.Everyone)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");

            // 權限不足
            rspObj.ErrorCode = MbsErrorCodeEnum.PermissionDenied;
            return rspObj;
        }

        #endregion

        // db, 核心-員工-商機-資料庫-取得
        var coEmpPplDbGetReqObj = new CoEmpPplDbGetReqMdl()
        {
            EmployeePipelineID = reqObj.EmployeePipelineID
        };
        var coEmpPplDbGetRspObj = await this._coEmpPipelineDb.GetAsync(coEmpPplDbGetReqObj);
        if (coEmpPplDbGetRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }
        if (coEmpPplDbGetRspObj.EmployeePipelineSalerEmployeeID.HasValue == false)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // 如果是Self權限,需要驗證商機的業務員工是否為登入者
        if (mbsCmnLgcGetLoginUserInfoRspObj.AtomPermissionKindIdCreate == DbAtomPermissionKindEnum.Self &&
            coEmpPplDbGetRspObj.EmployeePipelineSalerEmployeeID.Value != mbsCmnLgcGetLoginUserInfoRspObj.EmployeeID)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");

            // 權限不足
            rspObj.ErrorCode = MbsErrorCodeEnum.PermissionDenied;
            return rspObj;
        }

        // db, 核心-員工-商機履約期限-資料庫-新增
        var coEmpPplDueDbAddReqObj = new CoEmpPplDueDbAddReqMdl()
        {
            EmployeePipelineID = reqObj.EmployeePipelineID,
            EmployeePipelineDueTime = reqObj.EmployeePipelineDueTime,
            EmployeePipelineDueRemark = reqObj.EmployeePipelineDueRemark,
        };
        var coEmpPplDueDbAddRspObj = await this._coEmpPipelineDueDb.AddAsync(coEmpPplDueDbAddReqObj);
        if (coEmpPplDueDbAddRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        rspObj.EmployeePipelineDueID = coEmpPplDueDbAddRspObj.EmployeePipelineDueID;
        return rspObj;
    }

    /// <summary>管理者後台-CRM-商機管理-邏輯服務-更新商機履約期限</summary>
    public async Task<MbsCrmPplLgcUpdateEmployeePipelineDueRspMdl> UpdateEmployeePipelineDueAsync(MbsCrmPplLgcUpdateEmployeePipelineDueReqMdl reqObj)
    {
        var rspObj = new MbsCrmPplLgcUpdateEmployeePipelineDueRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        #region 判斷登入令牌與權限

        // mbs, 管理者後台-通用-邏輯-取得登入使用者資訊
        var mbsCmnLgcGetLoginUserInfoReqObj = new MbsCmnLgcGetLoginUserInfoReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken,
            AtomMenu = this._atomMenu,
        };
        var mbsCmnLgcGetLoginUserInfoRspObj = await this._mbsCommonLogical.GetLoginUserInfoAsync(mbsCmnLgcGetLoginUserInfoReqObj);
        if (mbsCmnLgcGetLoginUserInfoRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }
        if (mbsCmnLgcGetLoginUserInfoRspObj.ErrorCode != MbsErrorCodeEnum.Success)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");

            rspObj.ErrorCode = mbsCmnLgcGetLoginUserInfoRspObj.ErrorCode;
            return rspObj;
        }

        // 判斷權限
        if (mbsCmnLgcGetLoginUserInfoRspObj.AtomPermissionKindIdEdit != DbAtomPermissionKindEnum.Self &&
            mbsCmnLgcGetLoginUserInfoRspObj.AtomPermissionKindIdEdit != DbAtomPermissionKindEnum.Everyone)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");

            // 權限不足
            rspObj.ErrorCode = MbsErrorCodeEnum.PermissionDenied;
            return rspObj;
        }

        #endregion

        // db, 核心-員工-商機履約期限-資料庫-取得
        var coEmpPplDueDbGetReqObj = new CoEmpPplDueDbGetReqMdl()
        {
            EmployeePipelineDueID = reqObj.EmployeePipelineDueID
        };
        var coEmpPplDueDbGetRspObj = await this._coEmpPipelineDueDb.GetAsync(coEmpPplDueDbGetReqObj);
        if (coEmpPplDueDbGetRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // db, 核心-員工-商機-資料庫-取得
        var coEmpPplDbGetReqObj = new CoEmpPplDbGetReqMdl()
        {
            EmployeePipelineID = coEmpPplDueDbGetRspObj.EmployeePipelineID
        };
        var coEmpPplDbGetRspObj = await this._coEmpPipelineDb.GetAsync(coEmpPplDbGetReqObj);
        if (coEmpPplDbGetRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }
        if (coEmpPplDbGetRspObj.EmployeePipelineSalerEmployeeID.HasValue == false)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // 如果是Self權限,需要驗證商機的業務員工是否為登入者
        if (mbsCmnLgcGetLoginUserInfoRspObj.AtomPermissionKindIdView == DbAtomPermissionKindEnum.Self &&
            coEmpPplDbGetRspObj.EmployeePipelineSalerEmployeeID.Value != mbsCmnLgcGetLoginUserInfoRspObj.EmployeeID)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");

            // 權限不足
            rspObj.ErrorCode = MbsErrorCodeEnum.PermissionDenied;
            return rspObj;
        }

        // db, 核心-員工-商機履約期限-資料庫-更新
        var coEmpPplDueDbUpdateReqObj = new CoEmpPplDueDbUpdateReqMdl()
        {
            EmployeePipelineDueID = reqObj.EmployeePipelineDueID,
            EmployeePipelineDueTime = reqObj.EmployeePipelineDueTime,
            EmployeePipelineDueRemark = reqObj.EmployeePipelineDueRemark,
        };
        var coEmpPplDueDbUpdateRspObj = await this._coEmpPipelineDueDb.UpdateAsync(coEmpPplDueDbUpdateReqObj);
        if (coEmpPplDueDbUpdateRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        return rspObj;
    }

    /// <summary>管理者後台-CRM-商機管理-邏輯服務-刪除商機履約期限</summary>
    public async Task<MbsCrmPplLgcRemoveEmployeePipelineDueRspMdl> RemoveEmployeePipelineDueAsync(MbsCrmPplLgcRemoveEmployeePipelineDueReqMdl reqObj)
    {
        var rspObj = new MbsCrmPplLgcRemoveEmployeePipelineDueRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        #region 判斷登入令牌與權限

        // mbs, 管理者後台-通用-邏輯-取得登入使用者資訊
        var mbsCmnLgcGetLoginUserInfoReqObj = new MbsCmnLgcGetLoginUserInfoReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken,
            AtomMenu = this._atomMenu,
        };
        var mbsCmnLgcGetLoginUserInfoRspObj = await this._mbsCommonLogical.GetLoginUserInfoAsync(mbsCmnLgcGetLoginUserInfoReqObj);
        if (mbsCmnLgcGetLoginUserInfoRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }
        if (mbsCmnLgcGetLoginUserInfoRspObj.ErrorCode != MbsErrorCodeEnum.Success)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");

            rspObj.ErrorCode = mbsCmnLgcGetLoginUserInfoRspObj.ErrorCode;
            return rspObj;
        }

        // 判斷權限
        if (mbsCmnLgcGetLoginUserInfoRspObj.AtomPermissionKindIdDelete != DbAtomPermissionKindEnum.Self &&
            mbsCmnLgcGetLoginUserInfoRspObj.AtomPermissionKindIdDelete != DbAtomPermissionKindEnum.Everyone)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");

            // 權限不足
            rspObj.ErrorCode = MbsErrorCodeEnum.PermissionDenied;
            return rspObj;
        }

        #endregion

        // db, 核心-員工-商機履約期限-資料庫-取得
        var coEmpPplDueDbGetReqObj = new CoEmpPplDueDbGetReqMdl()
        {
            EmployeePipelineDueID = reqObj.EmployeePipelineDueID
        };
        var coEmpPplDueDbGetRspObj = await this._coEmpPipelineDueDb.GetAsync(coEmpPplDueDbGetReqObj);
        if (coEmpPplDueDbGetRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // db, 核心-員工-商機-資料庫-取得
        var coEmpPplDbGetReqObj = new CoEmpPplDbGetReqMdl()
        {
            EmployeePipelineID = coEmpPplDueDbGetRspObj.EmployeePipelineID
        };
        var coEmpPplDbGetRspObj = await this._coEmpPipelineDb.GetAsync(coEmpPplDbGetReqObj);
        if (coEmpPplDbGetRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }
        if (coEmpPplDbGetRspObj.EmployeePipelineSalerEmployeeID.HasValue == false)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // 如果是Self權限,需要驗證商機的業務員工是否為登入者
        if (mbsCmnLgcGetLoginUserInfoRspObj.AtomPermissionKindIdView == DbAtomPermissionKindEnum.Self &&
            coEmpPplDbGetRspObj.EmployeePipelineSalerEmployeeID.Value != mbsCmnLgcGetLoginUserInfoRspObj.EmployeeID)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");

            // 權限不足
            rspObj.ErrorCode = MbsErrorCodeEnum.PermissionDenied;
            return rspObj;
        }

        // db, 核心-員工-商機履約期限-資料庫-移除
        var coEmpPplDueDbRemoveReqObj = new CoEmpPplDueDbRemoveReqMdl()
        {
            EmployeePipelineDueID = reqObj.EmployeePipelineDueID
        };
        var coEmpPplDueDbRemoveRspObj = await this._coEmpPipelineDueDb.RemoveAsync(coEmpPplDueDbRemoveReqObj);
        if (coEmpPplDueDbRemoveRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        return rspObj;
    }

    #endregion
}
