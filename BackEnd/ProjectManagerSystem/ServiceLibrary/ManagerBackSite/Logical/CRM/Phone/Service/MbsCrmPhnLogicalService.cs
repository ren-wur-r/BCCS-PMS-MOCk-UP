using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using ApiModelLibrary.ManagerBackSite.Common;
using CommonLibrary.CmnExcel.Service;
using CommonLibrary.CmnFolderFile.Service;
using DataModelLibrary.Database.AtomEmployeePipelineProduct;
using DataModelLibrary.Database.AtomEmployeePipelineSaler;
using DataModelLibrary.Database.AtomManagerContacter;
using DataModelLibrary.Database.AtomMenu;
using DataModelLibrary.Database.AtomPermissionKind;
using DataModelLibrary.Database.AtomPipeline;
using Microsoft.Extensions.Logging;
using ServiceLibrary.Core.Employee.DB.Info.Format;
using ServiceLibrary.Core.Employee.DB.Info.Service;
using ServiceLibrary.Core.Employee.DB.Pipeline.Format;
using ServiceLibrary.Core.Employee.DB.Pipeline.Service;
using ServiceLibrary.Core.Employee.DB.PipelineBooking.Service;
using ServiceLibrary.Core.Employee.DB.PipelineContacter.Format;
using ServiceLibrary.Core.Employee.DB.PipelineContacter.Service;
using ServiceLibrary.Core.Employee.DB.PipelineOriginal.Format;
using ServiceLibrary.Core.Employee.DB.PipelineOriginal.Service;
using ServiceLibrary.Core.Employee.DB.PipelinePhone.Format;
using ServiceLibrary.Core.Employee.DB.PipelinePhone.Service;
using ServiceLibrary.Core.Employee.DB.PipelineProduct.Format;
using ServiceLibrary.Core.Employee.DB.PipelineProduct.Service;
using ServiceLibrary.Core.Employee.DB.PipelineSaler.Format;
using ServiceLibrary.Core.Employee.DB.PipelineSaler.Service;
using ServiceLibrary.Core.Employee.DB.PipelineSalerTracking.Format;
using ServiceLibrary.Core.Employee.DB.PipelineSalerTracking.Service;
using ServiceLibrary.Core.Employee.Logical.Service;
using ServiceLibrary.Core.Manager.DB.Activity.Format;
using ServiceLibrary.Core.Manager.DB.Activity.Service;
using ServiceLibrary.Core.Manager.DB.ActivityExpense.Format;
using ServiceLibrary.Core.Manager.DB.ActivityExpense.Service;
using ServiceLibrary.Core.Manager.DB.ActivityProduct.Format;
using ServiceLibrary.Core.Manager.DB.ActivityProduct.Service;
using ServiceLibrary.Core.Manager.DB.ActivitySponsor.Format;
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
using ServiceLibrary.Core.Manager.DB.Product.Format;
using ServiceLibrary.Core.Manager.DB.Product.Service;
using ServiceLibrary.Core.Manager.DB.ProductMainKind.Format;
using ServiceLibrary.Core.Manager.DB.ProductMainKind.Service;
using ServiceLibrary.Core.Manager.DB.ProductSpecification.Format;
using ServiceLibrary.Core.Manager.DB.ProductSpecification.Service;
using ServiceLibrary.Core.Manager.DB.ProductSubKind.Format;
using ServiceLibrary.Core.Manager.DB.ProductSubKind.Service;
using ServiceLibrary.ManagerBackSite.Logical.Common.Format;
using ServiceLibrary.ManagerBackSite.Logical.Common.Service;
using ServiceLibrary.ManagerBackSite.Logical.CRM.Phone.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.CRM.Phone.Service;

/// <summary>管理者後台-CRM-電銷管理-邏輯服務</summary>
public class MbsCrmPhnLogicalService : IMbsCrmPhnLogicalService
{
    /// <summary>logger</summary>
    private readonly ILogger<MbsCrmPhnLogicalService> _logger;

    #region CommonLibrary

    /// <summary>通用-Excel-服務</summary>
    private readonly ICmnExcelService _cmnExcel;

    /// <summary>通用-資料夾檔案-服務</summary>
    private readonly ICmnFolderFileService _cmnFolderFile;

    #endregion

    #region Core Employee

    /// <summary>核心-員工-資訊-資料庫服務</summary>
    private readonly ICoEmpInfoDbService _coEmpInfoDb;

    /// <summary>核心-員工-商機-資料庫服務</summary>
    private readonly ICoEmpPipelineDbService _coEmpPipelineDb;

    /// <summary>核心-員工-商機Booking-資料庫服務</summary>
    private readonly ICoEmpPipelineBookingDbService _coEmpPipelineBookingDb;

    /// <summary>核心-員工-商機窗口-資料庫服務</summary>
    private readonly ICoEmpPipelineContacterDbService _coEmpPipelineContacterDb;

    /// <summary>核心-員工-商機原始資料-資料庫服務</summary>
    private readonly ICoEmpPipelineOriginalDbService _coEmpPipelineOriginalDb;

    /// <summary>核心-員工-商機電銷紀錄-資料庫服務</summary>
    private readonly ICoEmpPipelinePhoneDbService _coEmpPipelinePhoneDb;

    /// <summary>核心-員工-商機產品-資料庫服務</summary>
    private readonly ICoEmpPipelineProductDbService _coEmpPipelineProductDb;

    /// <summary>核心-員工-商機業務-資料庫服務</summary>
    private readonly ICoEmpPipelineSalerDbService _coEmpPipelineSalerDb;

    /// <summary>核心-員工-商機業務開發紀錄-資料庫服務</summary>
    private readonly ICoEmpPipelineSalerTrackingDbService _coEmpPipelineSalerTrackingDb;

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
    public MbsCrmPhnLogicalService(
        ILogger<MbsCrmPhnLogicalService> logger,
        // CommonLibrary
        ICmnExcelService cmnExcel,
        ICmnFolderFileService cmnFolderFile,
        // Core Employee
        ICoEmpInfoDbService coEmpInfoDb,
        ICoEmpPipelineDbService coEmpPipelineDb,
        ICoEmpPipelineBookingDbService coEmpPipelineBookingDb,
        ICoEmpPipelineContacterDbService coEmpPipelineContacterDb,
        ICoEmpPipelineOriginalDbService coEmpPipelineOriginalDb,
        ICoEmpPipelinePhoneDbService coEmpPipelinePhoneDb,
        ICoEmpPipelineProductDbService coEmpPipelineProductDb,
        ICoEmpPipelineSalerDbService coEmpPipelineSalerDb,
        ICoEmpPipelineSalerTrackingDbService coEmpPipelineSalerTrackingDb,
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
        // Core Logical
        ICoEmployeeLogicalService coEmployeeLogical,
        // ManagerBackSite
        IMbsCommonLogicalService mbsCommonLogical)
    {
        this._logger = logger;
        // CommonLibrary
        this._cmnExcel = cmnExcel;
        this._cmnFolderFile = cmnFolderFile;
        // Core Employee
        this._coEmpInfoDb = coEmpInfoDb;
        this._coEmpPipelineDb = coEmpPipelineDb;
        this._coEmpPipelineBookingDb = coEmpPipelineBookingDb;
        this._coEmpPipelineContacterDb = coEmpPipelineContacterDb;
        this._coEmpPipelineOriginalDb = coEmpPipelineOriginalDb;
        this._coEmpPipelinePhoneDb = coEmpPipelinePhoneDb;
        this._coEmpPipelineProductDb = coEmpPipelineProductDb;
        this._coEmpPipelineSalerDb = coEmpPipelineSalerDb;
        this._coEmpPipelineSalerTrackingDb = coEmpPipelineSalerTrackingDb;
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
        // Core Logical
        this._coEmployeeLogical = coEmployeeLogical;
        // ManagerBackSite
        this._mbsCommonLogical = mbsCommonLogical;

        // This
        this._atomMenu = DbAtomMenuEnum.CrmPhone;
    }

    #region 活動

    /// <summary>管理者後台-CRM-電銷管理-邏輯服務-取得多筆活動</summary>
    public async Task<MbsCrmPhnLgcGetManyActivityRspMdl> GetManyActivityAsync(MbsCrmPhnLgcGetManyActivityReqMdl reqObj)
    {
        var rspObj = new MbsCrmPhnLgcGetManyActivityRspMdl
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
        if (mbsCmnLgcGetLoginUserInfoRspObj.AtomPermissionKindIdView != DbAtomPermissionKindEnum.Everyone)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");

            // 權限不足
            rspObj.ErrorCode = MbsErrorCodeEnum.PermissionDenied;
            return rspObj;
        }

        #endregion

        // db, 核心-管理者-活動-資料庫-取得[筆數]從[管理者後台-CRM-電銷管理]
        var coMgrActivityDbGetCountFromMbsCrmPhnReqObj = new CoMgrActivityDbGetCountFromMbsCrmPhnReqMdl()
        {
            ManagerActivityStartTime = reqObj.ManagerActivityStartTime,
            ManagerActivityEndTime = reqObj.ManagerActivityEndTime,
            ManagerActivityKind = reqObj.ManagerActivityKind,
            ManagerActivityName = reqObj.ManagerActivityName,
        };
        var coMgrActivityDbGetCountFromMbsCrmPhnRspObj = await this._coMgrActivityDb.GetCountFromMbsCrmPhnAsync(coMgrActivityDbGetCountFromMbsCrmPhnReqObj);
        if (coMgrActivityDbGetCountFromMbsCrmPhnRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");

            return rspObj;
        }

        // 判斷筆數是否為0
        if (coMgrActivityDbGetCountFromMbsCrmPhnRspObj.Count == 0)
        {
            rspObj.ErrorCode = MbsErrorCodeEnum.Success;
            rspObj.ManagerActivityList = new();
            rspObj.TotalCount = 0;
            return rspObj;
        }

        // db, 核心-管理者-活動-資料庫-取得多筆從[管理者後台-CRM-電銷管理]
        var coMgrActivityDbGetManyFromMbsCrmPhnReqObj = new CoMgrActivityDbGetManyFromMbsCrmPhnReqMdl()
        {
            ManagerActivityStartTime = reqObj.ManagerActivityStartTime,
            ManagerActivityEndTime = reqObj.ManagerActivityEndTime,
            ManagerActivityKind = reqObj.ManagerActivityKind,
            ManagerActivityName = reqObj.ManagerActivityName,
            PageIndex = reqObj.PageIndex,
            PageSize = reqObj.PageSize
        };
        var coMgrActivityDbGetManyFromMbsCrmPhnRspObj = await this._coMgrActivityDb.GetManyFromMbsCrmPhnAsync(coMgrActivityDbGetManyFromMbsCrmPhnReqObj);
        if (coMgrActivityDbGetManyFromMbsCrmPhnRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");

            return rspObj;
        }

        // 實際名單數
        // db, 核心-員工-商機-資料庫-取得多筆[筆數]
        var coEmpPplDbGetManyCountReqObj = new CoEmpPplDbGetManyCountReqMdl()
        {
            ManagerActivityIDList = coMgrActivityDbGetManyFromMbsCrmPhnRspObj.ManagerActivityList
                                                                        .Select(x => x.ManagerActivityID)
                                                                        .Distinct()
                                                                        .ToList()
        };
        var coEmpPplDbGetManyCountRspObj = await this._coEmpPipelineDb.GetManyCountAsync(coEmpPplDbGetManyCountReqObj);
        if (coEmpPplDbGetManyCountRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // 已轉電銷數
        // db, 核心-員工-商機-資料庫-取得多筆[筆數]by已轉電銷
        var coEmpPplDbGetManyCountByTransferredToSalesReqObj = new CoEmpPplDbGetManyCountReqMdl()
        {
            ManagerActivityIDList = coMgrActivityDbGetManyFromMbsCrmPhnRspObj.ManagerActivityList
                                                                        .Select(x => x.ManagerActivityID)
                                                                        .Distinct()
                                                                        .ToList(),
            AtomPipelineStatus = DbAtomPipelineStatusEnum.TransferredToSales
        };
        var coEmpPplDbGetManyCountByTransferredToSalesRspObj = await this._coEmpPipelineDb.GetManyCountAsync(coEmpPplDbGetManyCountByTransferredToSalesReqObj);
        if (coEmpPplDbGetManyCountByTransferredToSalesRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // db, 核心-員工-商機電銷-資料庫-取得多筆[筆數]從[活動ID]
        var coEmpPplPhnDbGetManyCountFromManagerActivityIDReqObj = new CoEmpPplPhnDbGetManyCountFromManagerActivityIDReqMdl()
        {
            ManagerActivityIDList = coMgrActivityDbGetManyFromMbsCrmPhnRspObj.ManagerActivityList
                                                                        .Select(x => x.ManagerActivityID)
                                                                        .Distinct()
                                                                        .ToList()
        };
        var coEmpPplPhnDbGetManyCountFromManagerActivityIDRspObj = await this._coEmpPipelinePhoneDb.GetManyCountFromManagerActivityIDAsync(coEmpPplPhnDbGetManyCountFromManagerActivityIDReqObj);
        if (coEmpPplPhnDbGetManyCountFromManagerActivityIDRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        rspObj.ManagerActivityList =
        (
            from act in coMgrActivityDbGetManyFromMbsCrmPhnRspObj.ManagerActivityList
            from totalPipelineCount in coEmpPplDbGetManyCountRspObj.EmployeePipelineList
                                .Where(count => count.ManagerActivityID == act.ManagerActivityID)
                                .DefaultIfEmpty()
            from toSalesCount in coEmpPplDbGetManyCountByTransferredToSalesRspObj.EmployeePipelineList
                                .Where(count => count.ManagerActivityID == act.ManagerActivityID)
                                .DefaultIfEmpty()
            from phone in coEmpPplPhnDbGetManyCountFromManagerActivityIDRspObj.EmployeePipelinePhoneList
                                .Where(count => count.ManagerActivityID == act.ManagerActivityID)
                                .DefaultIfEmpty()
            select new MbsCrmPhnLgcGetManyActivityRspItemMdl
            {
                ManagerActivityID = act.ManagerActivityID,
                ManagerActivityName = act.ManagerActivityName,
                ManagerActivityKind = act.ManagerActivityKind,
                ManagerActivityStartTime = act.ManagerActivityStartTime,
                ManagerActivityEndTime = act.ManagerActivityEndTime,
                ManagerActivityRegisteredCount = totalPipelineCount != null ? totalPipelineCount.Count : 0,
                ManagerActivityTransferredToSalesCount = toSalesCount != null ? toSalesCount.Count : 0,
                ManagerActivityPhoneCount = phone != null ? phone.Count : 0,
                ManagerActivityEmployeePipelineCount = act.ManagerActivityEmployeePipelineCount,
            }
        ).ToList();
        rspObj.TotalCount = coMgrActivityDbGetCountFromMbsCrmPhnRspObj.Count;
        return rspObj;
    }

    /// <summary>管理者後台-CRM-電銷管理-邏輯服務-取得單筆活動</summary>
    public async Task<MbsCrmPhnLgcGetActivityRspMdl> GetActivityAsync(MbsCrmPhnLgcGetActivityReqMdl reqObj)
    {
        var rspObj = new MbsCrmPhnLgcGetActivityRspMdl
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
        if (mbsCmnLgcGetLoginUserInfoRspObj.AtomPermissionKindIdView != DbAtomPermissionKindEnum.Everyone)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");

            // 權限不足
            rspObj.ErrorCode = MbsErrorCodeEnum.PermissionDenied;
            return rspObj;
        }

        #endregion

        // db, 核心-管理者-活動-資料庫-取得
        var coMgrActivityDbGetReqObj = new CoMgrActivityDbGetReqMdl()
        {
            ManagerActivityID = reqObj.ManagerActivityID,
        };
        var coMgrActivityDbGetRspObj = await this._coMgrActivityDb.GetAsync(coMgrActivityDbGetReqObj);
        if (coMgrActivityDbGetRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // 實際名單數
        // db, 核心-員工-商機-資料庫-取得[筆數]
        var coEmpPplDbGetCountReqObj = new CoEmpPplDbGetCountReqMdl()
        {
            ManagerActivityID = reqObj.ManagerActivityID
        };
        var coEmpPplDbGetCountRspObj = await this._coEmpPipelineDb.GetCountAsync(coEmpPplDbGetCountReqObj);
        if (coEmpPplDbGetCountRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // 已轉電銷數
        // db, 核心-員工-商機-資料庫-取得[筆數]by已轉電銷
        var coEmpPplDbGetCountByTransferredToSalesReqObj = new CoEmpPplDbGetCountReqMdl()
        {
            ManagerActivityID = reqObj.ManagerActivityID,
            AtomPipelineStatus = DbAtomPipelineStatusEnum.TransferredToSales
        };
        var coEmpPplDbGetCountByTransferredToSalesRspObj = await this._coEmpPipelineDb.GetCountAsync(coEmpPplDbGetCountByTransferredToSalesReqObj);
        if (coEmpPplDbGetCountByTransferredToSalesRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // db, 核心-員工-商機-資料庫-取得多筆員工商機ID
        var coEmpPplDbGetManyEmployeePipelineIDReqObj = new CoEmpPplDbGetManyEmployeePipelineIDReqMdl()
        {
            ManagerActivityID = reqObj.ManagerActivityID
        };
        var coEmpPplDbGetManyEmployeePipelineIDRspObj = await this._coEmpPipelineDb.GetManyEmployeePipelineIDAsync(coEmpPplDbGetManyEmployeePipelineIDReqObj);
        if (coEmpPplDbGetManyEmployeePipelineIDRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // db, 核心-員工-商機電銷-資料庫-取得[筆數]從[活動ID]
        var coEmpPplDbGetCountByManagerActivityIDReqObj = new CoEmpPplPhnDbGetCountFromManagerActivityIDReqMdl()
        {
            ManagerActivityID = reqObj.ManagerActivityID
        };
        var coEmpPplDbGetCountByManagerActivityIDRspObj = await this._coEmpPipelinePhoneDb.GetCountFromManagerActivityIDAsync(coEmpPplDbGetCountByManagerActivityIDReqObj);
        if (coEmpPplDbGetCountByManagerActivityIDRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // db, 核心-管理者-活動產品-資料庫-取得多筆
        var coMgrActPrdDbGetManyReqObj = new CoMgrActPrdDbGetManyReqMdl()
        {
            ManagerActivityID = reqObj.ManagerActivityID,
        };
        var coMgrActPrdDbGetManyRspObj = await this._coMgrActivityProductDb.GetManyAsync(coMgrActPrdDbGetManyReqObj);
        if (coMgrActPrdDbGetManyRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        #region 產品

        // db, 核心-管理者-產品-資料庫-取得多筆從[管理者後台-CRM-活動管理]
        var coMgrPrdDbGetManyFromMbsMktActReqObj = new CoMgrPrdDbGetManyFromMbsMktActReqMdl()
        {
            ManagerProductIDList = coMgrActPrdDbGetManyRspObj.ManagerActivityProductList
                                        .Select(x => x.ManagerProductID)
                                        .Distinct()
                                        .ToList()
        };
        var coMgrPrdDbGetManyFromMbsMktActRspObj = await this._coMgrProductDb.GetManyFromMbsMktActAsync(coMgrPrdDbGetManyFromMbsMktActReqObj);
        if (coMgrPrdDbGetManyFromMbsMktActRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // db, 核心-管理者-產品主分類-資料庫-取得多筆[名稱]
        var coMgrPmkDbGetManyNameReqObj = new CoMgrPmkDbGetManyNameReqMdl
        {
            ManagerProductMainKindIdList = coMgrPrdDbGetManyFromMbsMktActRspObj.ManagerProductList
                                        .Select(x => x.ManagerProductMainKindID)
                                        .Distinct()
                                        .ToList()
        };
        var coMgrPmkDbGetManyNameRspObj = await this._coMgrProductMainKindDb.GetManyNameAsync(coMgrPmkDbGetManyNameReqObj);
        if (coMgrPmkDbGetManyNameRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // db, 核心-管理者-產品子分類-資料庫-取得多筆[名稱]
        var coMgrPskDbGetManyNameReqObj = new CoMgrPskDbGetManyNameReqMdl
        {
            ManagerProductSubKindIDList = coMgrPrdDbGetManyFromMbsMktActRspObj.ManagerProductList
                                        .Select(x => x.ManagerProductSubKindID)
                                        .Distinct()
                                        .ToList()
        };
        var coMgrPskDbGetManyNameRspObj = await this._coMgrProductSubKindDb.GetManyNameAsync(coMgrPskDbGetManyNameReqObj);
        if (coMgrPskDbGetManyNameRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        #endregion

        #region 贊助

        // db, 核心-管理者-活動贊助-資料庫-取得多筆
        var coMgrActSpsDbGetManyReqObj = new CoMgrActSpsDbGetManyReqMdl()
        {
            ManagerActivityID = reqObj.ManagerActivityID,
        };
        var coMgrActSpsDbGetManyRspObj = await this._coMgrActivitySponsorDb.GetManyAsync(coMgrActSpsDbGetManyReqObj);
        if (coMgrActSpsDbGetManyRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        #endregion

        #region 支出

        // db, 核心-管理者-活動支出-資料庫-取得多筆
        var coMgrActExpDbGetManyReqObj = new CoMgrActExpDbGetManyReqMdl()
        {
            ManagerActivityID = reqObj.ManagerActivityID,
        };
        var coMgrActExpDbGetManyRspObj = await this._coMgrActivityExpenseDb.GetManyAsync(coMgrActExpDbGetManyReqObj);
        if (coMgrActExpDbGetManyRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        #endregion

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        rspObj.ManagerActivityName = coMgrActivityDbGetRspObj.ManagerActivityName;
        rspObj.ManagerActivityKind = coMgrActivityDbGetRspObj.ManagerActivityKind;
        rspObj.ManagerActivityStartTime = coMgrActivityDbGetRspObj.ManagerActivityStartTime;
        rspObj.ManagerActivityEndTime = coMgrActivityDbGetRspObj.ManagerActivityEndTime;
        rspObj.ManagerActivityLocation = coMgrActivityDbGetRspObj.ManagerActivityLocation;
        rspObj.ManagerActivityExpectedLeadCount = coMgrActivityDbGetRspObj.ManagerActivityExpectedLeadCount;
        rspObj.ManagerActivityContent = coMgrActivityDbGetRspObj.ManagerActivityContent;
        rspObj.ManagerActivityRegisteredCount = coEmpPplDbGetCountRspObj.Count;
        rspObj.ManagerActivityTransferredToSalesCount = coEmpPplDbGetCountByTransferredToSalesRspObj.Count;
        rspObj.ManagerActivityEmployeePipelineCount = coMgrActivityDbGetRspObj.ManagerActivityEmployeePipelineCount;
        rspObj.ManagerActivityPhoneCount = coEmpPplDbGetCountByManagerActivityIDRspObj.Count;
        rspObj.ManagerActivityProductList =
        (
            from actp in coMgrActPrdDbGetManyRspObj.ManagerActivityProductList
            from prd in coMgrPrdDbGetManyFromMbsMktActRspObj.ManagerProductList
                                .Where(x => x.ManagerProductID == actp.ManagerProductID)
                                .DefaultIfEmpty()
            from prdm in coMgrPmkDbGetManyNameRspObj.ManagerProductMainKindList
                                .Where(x => x.ManagerProductMainKindID == prd.ManagerProductMainKindID)
                                .DefaultIfEmpty()
            from prds in coMgrPskDbGetManyNameRspObj.ManagerProductSubKindList
                                .Where(x => x.ManagerProductSubKindID == prd.ManagerProductSubKindID)
                                .DefaultIfEmpty()
            select new MbsCrmPhnLgcGetActivityProductRspItemMdl
            {
                ManagerActivityProductID = actp.ManagerActivityProductID,
                ManagerProductName = prd?.ManagerProductName,
                ManagerProductMainKindName = prdm?.ManagerProductMainKindName,
                ManagerProductSubKindName = prds?.ManagerProductSubKindName,
            }
        ).ToList();
        rspObj.ManagerActivitySponsorList = coMgrActSpsDbGetManyRspObj.ManagerActivitySponsorList
            .Select(x => new MbsCrmPhnLgcGetActivitySponsorRspItemMdl
            {
                ManagerActivitySponsorID = x.ManagerActivitySponsorID,
                ManagerActivitySponsorName = x.ManagerActivitySponsorName,
                ManagerActivitySponsorAmount = x.ManagerActivitySponsorAmount
            })
            .ToList();
        rspObj.ManagerActivityExpenseList = coMgrActExpDbGetManyRspObj.ManagerActivityExpenseList
           .Select(x => new MbsCrmPhnLgcGetActivityExpenseRspItemMdl
           {
               ManagerActivityExpenseID = x.ManagerActivityExpenseID,
               ManagerActivityExpenseItem = x.ManagerActivityExpenseItem,
               ManagerActivityExpenseQuantity = x.ManagerActivityExpenseQuantity,
               ManagerActivityExpenseTotalAmount = x.ManagerActivityExpenseTotalAmount
           })
           .ToList();
        return rspObj;
    }

    #endregion

    #region 活動名單

    /// <summary>管理者後台-CRM-電銷管理-邏輯服務-取得多筆活動名單</summary>
    public async Task<MbsCrmPhnLgcGetManyActivityEmployeePipelineRspMdl> GetManyActivityEmployeePipelineAsync(MbsCrmPhnLgcGetManyActivityEmployeePipelineReqMdl reqObj)
    {
        var rspObj = new MbsCrmPhnLgcGetManyActivityEmployeePipelineRspMdl
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
        if (mbsCmnLgcGetLoginUserInfoRspObj.AtomPermissionKindIdView != DbAtomPermissionKindEnum.Everyone)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");

            // 權限不足
            rspObj.ErrorCode = MbsErrorCodeEnum.PermissionDenied;
            return rspObj;
        }

        #endregion

        // db, 核心-員工-商機-資料庫-取得[筆數]從[管理者後台-CRM-電銷管理]
        var coEmpPplDbGetCountFromMbsCrmPhnReqObj = new CoEmpPplDbGetCountFromMbsCrmPhnReqMdl()
        {
            ManagerActivityID = reqObj.ManagerActivityID,
            AtomPipelineStatus = reqObj.AtomPipelineStatus,
            ManagerCompanyName = reqObj.ManagerCompanyName,
            ManagerContacterName = reqObj.ManagerContacterName,
            ManagerContacterEmail = reqObj.ManagerContacterEmail
        };
        var coEmpPplDbGetCountFromMbsCrmPhnRspObj = await this._coEmpPipelineDb.GetCountFromMbsCrmPhnAsync(coEmpPplDbGetCountFromMbsCrmPhnReqObj);
        if (coEmpPplDbGetCountFromMbsCrmPhnRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // 判斷筆數是否為0
        if (coEmpPplDbGetCountFromMbsCrmPhnRspObj.Count == 0)
        {
            rspObj.ErrorCode = MbsErrorCodeEnum.Success;
            rspObj.EmployeePipelineList = new();
            rspObj.TotalCount = 0;
            return rspObj;
        }

        // db, 核心-員工-商機-資料庫-取得多筆[管理者後台-CRM-電銷管理]
        var coEmpPplDbGetManyFromMbsCrmPhnReqObj = new CoEmpPplDbGetManyFromMbsCrmPhnReqMdl
        {
            ManagerActivityID = reqObj.ManagerActivityID,
            AtomPipelineStatus = reqObj.AtomPipelineStatus,
            ManagerCompanyName = reqObj.ManagerCompanyName,
            ManagerContacterName = reqObj.ManagerContacterName,
            ManagerContacterEmail = reqObj.ManagerContacterEmail,
            PageIndex = reqObj.PageIndex,
            PageSize = reqObj.PageSize
        };
        var coEmpPplDbGetManyFromMbsCrmPhnRspObj = await this._coEmpPipelineDb.GetManyFromMbsCrmPhnAsync(coEmpPplDbGetManyFromMbsCrmPhnReqObj);
        if (coEmpPplDbGetManyFromMbsCrmPhnRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // db, 核心-員工-商機業務開發紀錄-資料庫-取得多筆開發時間從商機ID列表
        var coEmpPplSlrTrkDbGetManyTrackingTimeFromPipelineIdListReqObj = new CoEmpPplSlrTrkDbGetManyTrackingTimeFromPipelineIdListReqMdl
        {
            EmployeePipelineIDList = coEmpPplDbGetManyFromMbsCrmPhnRspObj.EmployeePipelineList.Select(x => x.EmployeePipelineID).ToList()
        };
        var coEmpPplSlrTrkDbGetManyTrackingTimeFromPipelineIdListRspObj = await this._coEmpPipelineSalerTrackingDb.GetManyTrackingTimeFromPipelineIdListAsync(coEmpPplSlrTrkDbGetManyTrackingTimeFromPipelineIdListReqObj);
        if (coEmpPplSlrTrkDbGetManyTrackingTimeFromPipelineIdListRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        rspObj.EmployeePipelineList = (
            from p in coEmpPplDbGetManyFromMbsCrmPhnRspObj.EmployeePipelineList
            from t in coEmpPplSlrTrkDbGetManyTrackingTimeFromPipelineIdListRspObj.EmployeePipelineSalerTrackingTimeList
                .Where(x => x.EmployeePipelineID == p.EmployeePipelineID)
                .DefaultIfEmpty()
            select new MbsCrmPhnLgcGetManyEmployeePipelineRspItemMdl
            {
                EmployeePipelineID = p.EmployeePipelineID,
                HasCompany = p.HasCompany,
                ManagerCompanyID = p.ManagerCompanyID,
                ManagerCompanyName = p.ManagerCompanyName,
                HasContacter = p.HasContacter,
                EmployeePipelineContacterID = p.EmployeePipelineContacterID,
                ManagerContacterDepartment = p.ManagerContacterDepartment,
                ManagerContacterJobTitle = p.ManagerContacterJobTitle,
                ManagerContacterName = p.ManagerContacterName,
                ManagerContacterEmail = p.ManagerContacterEmail,
                ManagerContacterPhone = p.ManagerContacterPhone,
                ManagerContacterTelephone = p.ManagerContacterTelephone,
                AtomPipelineStatus = p.AtomPipelineStatus,
                EmployeePipelineSalerTrackingTime = t != null && t.TrackingTimeList != null && t.TrackingTimeList.Count > 0
                                                    ? t.TrackingTimeList.FirstOrDefault()
                                                    : null
            }
        ).ToList();
        rspObj.TotalCount = coEmpPplDbGetCountFromMbsCrmPhnRspObj.Count;
        return rspObj;
    }

    /// <summary>管理者後台-CRM-電銷管理-邏輯服務-取得活動名單</summary>
    public async Task<MbsCrmPhnLgcGetActivityEmployeePipelineRspMdl> GetActivityEmployeePipelineAsync(MbsCrmPhnLgcGetActivityEmployeePipelineReqMdl reqObj)
    {
        var rspObj = new MbsCrmPhnLgcGetActivityEmployeePipelineRspMdl
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
        if (mbsCmnLgcGetLoginUserInfoRspObj.AtomPermissionKindIdView != DbAtomPermissionKindEnum.Everyone)
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

        var company = coEmpPplDbGetRspObj.ManagerCompanyID.HasValue
                ? new MbsCrmPhnLgcGetActivityEmployeePipelineRspCompanyItemMdl()
                : null;

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

        // db, 核心-員工-商機原始資料-資料庫-取得
        var coEmpPplOgnDbGetReqObj = new CoEmpPplOgnDbGetReqMdl()
        {
            EmployeePipelineID = reqObj.EmployeePipelineID
        };
        var coEmpPplOgnDbGetRspObj = await this._coEmpPipelineOriginalDb.GetAsync(coEmpPplOgnDbGetReqObj);
        if (coEmpPplOgnDbGetRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // db, 核心-員工-商機電銷-資料庫-取得多筆
        var coEmpPplPhnDbGetManyReqObj = new CoEmpPplPhnDbGetManyReqMdl()
        {
            EmployeePipelineID = reqObj.EmployeePipelineID
        };
        var coEmpPplPhnDbGetManyRspObj = await this._coEmpPipelinePhoneDb.GetManyAsync(coEmpPplPhnDbGetManyReqObj);
        if (coEmpPplPhnDbGetManyRspObj == default)
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

        // db, 核心-員工-商機業務-資料庫-取得多筆
        var coEmpPplSlrDbGetManyReqObj = new CoEmpPplSlrDbGetManyReqMdl()
        {
            EmployeePipelineID = reqObj.EmployeePipelineID
        };
        var coEmpPplSlrDbGetManyRspObj = await this._coEmpPipelineSalerDb.GetManyAsync(coEmpPplSlrDbGetManyReqObj);
        if (coEmpPplSlrDbGetManyRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // 查詢員工名稱 From EmployeeID
        var queryEmployeeNameList = new List<int>();
        if (coEmpPplPhnDbGetManyRspObj.EmployeePipelinePhoneList != null)
        {
            // 電銷員工ID
            queryEmployeeNameList.AddRange(coEmpPplPhnDbGetManyRspObj.EmployeePipelinePhoneList
                                                .Select(x => x.EmployeePipelinePhoneCreateEmployeeID)
                                                .Distinct()
                                                .ToList());
        }
        if (coEmpPplSlrDbGetManyRspObj.EmployeePipelineSalerList != null)
        {
            // 指派員工ID
            queryEmployeeNameList.AddRange(coEmpPplSlrDbGetManyRspObj.EmployeePipelineSalerList
                                                .Select(x => x.EmployeePipelineSalerCreateEmployeeID)
                                                .Distinct()
                                                .ToList());
            // 業務員工ID                                                    
            queryEmployeeNameList.AddRange(coEmpPplSlrDbGetManyRspObj.EmployeePipelineSalerList
                                                .Select(x => x.EmployeePipelineSalerEmployeeID)
                                                .Distinct()
                                                .ToList());
        }

        // db, 核心-員工-資訊-資料庫-取得多筆[名稱]
        var coEmpInfDbGetManyNameReqObj = new CoEmpInfDbGetManyNameReqMdl()
        {
            EmployeeIdList = queryEmployeeNameList.Distinct().ToList()
        };
        var coEmpInfDbGetManyNameRspObj = await this._coEmpInfoDb.GetManyNameAsync(coEmpInfDbGetManyNameReqObj);
        if (coEmpInfDbGetManyNameRspObj == default)
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
        if (coEmpPplPhnDbGetManyRspObj.EmployeePipelinePhoneList != null)
        {
            managerContacterIDList.AddRange(coEmpPplPhnDbGetManyRspObj.EmployeePipelinePhoneList
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

        // db, 核心-員工-商機產品-資料庫-取得多筆從[管理者後台-CRM-電銷管理]
        var coEmpPplPrdDbGetManyFromPhoneReqObj = new CoEmpPplPrdDbGetManyFromPhoneReqMdl()
        {
            EmployeePipelineID = reqObj.EmployeePipelineID
        };
        var coEmpPplPrdDbGetManyFromPhoneRspObj = await this._coEmpPipelineProductDb.GetManyFromPhoneAsync(coEmpPplPrdDbGetManyFromPhoneReqObj);
        if (coEmpPplPrdDbGetManyFromPhoneRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        // 狀態
        rspObj.AtomPipelineStatus = coEmpPplDbGetRspObj.AtomPipelineStatus;
        // 原始客戶資訊
        rspObj.OriginalCompany = new MbsCrmPhnLgcGetActivityEmployeePipelineRspOriginalCompanyItemMdl
        {
            ManagerCompanyUnifiedNumber = coEmpPplOgnDbGetRspObj.ManagerCompanyUnifiedNumber,
            ManagerCompanyName = coEmpPplOgnDbGetRspObj.ManagerCompanyName,
            AtomEmployeeRange = coEmpPplOgnDbGetRspObj.AtomEmployeeRange,
            AtomCustomerGrade = coEmpPplOgnDbGetRspObj.AtomCustomerGrade,
            ManagerCompanyMainKindName = coEmpPplOgnDbGetRspObj.ManagerCompanyMainKindName,
            ManagerCompanySubKindName = coEmpPplOgnDbGetRspObj.ManagerCompanySubKindName,
            AtomCity = coEmpPplOgnDbGetRspObj.AtomCity,
            ManagerCompanyLocationAddress = coEmpPplOgnDbGetRspObj.ManagerCompanyLocationAddress,
            ManagerCompanyLocationTelephone = coEmpPplOgnDbGetRspObj.ManagerCompanyLocationTelephone,
            ManagerCompanyLocationStatus = coEmpPplOgnDbGetRspObj.ManagerCompanyLocationStatus
        };
        // 客戶資訊
        rspObj.HasCompany = coEmpPplDbGetRspObj.ManagerCompanyID.HasValue;
        rspObj.Company = company;
        // 原始窗口資訊
        rspObj.OriginalContacter = new MbsCrmPhnLgcGetActivityEmployeePipelineRspOriginalContacterItemMdl
        {
            ManagerContacterName = coEmpPplOgnDbGetRspObj.ManagerContacterName,
            ManagerContacterEmail = coEmpPplOgnDbGetRspObj.ManagerContacterEmail,
            ManagerContacterPhone = coEmpPplOgnDbGetRspObj.ManagerContacterPhone,
            ManagerContacterDepartment = coEmpPplOgnDbGetRspObj.ManagerContacterDepartment,
            ManagerContacterJobTitle = coEmpPplOgnDbGetRspObj.ManagerContacterJobTitle,
            ManagerContacterTelephone = coEmpPplOgnDbGetRspObj.ManagerContacterTelephone,
            ManagerContacterIsConsent = coEmpPplOgnDbGetRspObj.ManagerContacterIsConsent,
            ManagerContacterStatus = coEmpPplOgnDbGetRspObj.ManagerContacterStatus,
            AtomRatingKind = coEmpPplOgnDbGetRspObj.AtomRatingKind
        };
        // 窗口資訊
        rspObj.ContacterList = (
            from epc in coEmpPplContDbGetManyRspObj.EmployeePipelineContacterList
            from mc in coMgrCttDbGetManyRspObj.ManagerContacterList
                        .Where(mc => mc.ManagerContacterID == epc.ManagerContacterID)
                        .DefaultIfEmpty()
            select new MbsCrmPhnLgcGetActivityEmployeePipelineRspContacterItemMdl
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
        // 產品列表
        rspObj.ProductList = (
            from epp in coEmpPplPrdDbGetManyFromPhoneRspObj.EmployeePipelineProductList
            select new MbsCrmPhnLgcGetActivityEmployeePipelineRspProductItemMdl
            {
                EmployeePipelineProductID = epp.EmployeePipelineProductID,
                ManagerProductID = epp.ManagerProductID,
                ManagerProductName = epp.ManagerProductName,
                ManagerProductMainKindID = epp.ManagerProductMainKindID,
                ManagerProductMainKindName = epp.ManagerProductMainKindName,
                ManagerProductSubKindID = epp.ManagerProductSubKindID,
                ManagerProductSubKindName = epp.ManagerProductSubKindName,
                ManagerProductSpecificationID = epp.ManagerProductSpecificationID,
                ManagerProductSpecificationName = epp.ManagerProductSpecificationName,
                ManagerProductSpecificationSellPrice = epp.ManagerProductSpecificationSellPrice,
            }
        ).ToList();
        // 報名狀態
        rspObj.TeamsMeetingDuration = coEmpPplOgnDbGetRspObj?.TeamsMeetingDuration?.Trim();
        rspObj.TeamsRole = coEmpPplOgnDbGetRspObj?.TeamsRole?.Trim();
        // 電銷紀錄
        rspObj.PhoneList = (
            from ep in coEmpPplPhnDbGetManyRspObj.EmployeePipelinePhoneList
            from mc in coMgrCttDbGetManyRspObj.ManagerContacterList
                        .Where(mc => mc.ManagerContacterID == ep.ManagerContacterID)
                        .DefaultIfEmpty()
            from e in coEmpInfDbGetManyNameRspObj.EmployeeList
                        .Where(e => e.EmployeeID == ep.EmployeePipelinePhoneCreateEmployeeID)
                        .DefaultIfEmpty()
            select new MbsCrmPhnLgcGetActivityEmployeePipelineRspPhoneItemMdl
            {
                EmployeePipelinePhoneID = ep.EmployeePipelinePhoneID,
                EmployeePipelinePhoneRecordTime = ep.EmployeePipelinePhoneRecordTime,
                ManagerContacterName = mc?.ManagerContacterName,
                EmployeePipelinePhoneTitle = ep.EmployeePipelinePhoneTitle,
                EmployeePipelinePhoneRemark = ep.EmployeePipelinePhoneRemark,
                EmployeePipelinePhoneCreateEmployeeName = e?.EmployeeName
            }
        ).ToList();
        // 指派業務紀錄
        rspObj.SalerList = (
            from ep in coEmpPplSlrDbGetManyRspObj.EmployeePipelineSalerList
            from saler in coEmpInfDbGetManyNameRspObj.EmployeeList
                        .Where(e => e.EmployeeID == ep.EmployeePipelineSalerEmployeeID)
                        .DefaultIfEmpty()
            from employee in coEmpInfDbGetManyNameRspObj.EmployeeList
                        .Where(e => e.EmployeeID == ep.EmployeePipelineSalerCreateEmployeeID)
                        .DefaultIfEmpty()
            select new MbsCrmPhnLgcGetActivityEmployeePipelineRspSalerItemMdl
            {
                EmployeePipelineSalerID = ep.EmployeePipelineSalerID,
                EmployeePipelineSalerEmployeeName = saler?.EmployeeName,
                EmployeePipelineSalerReplyTime = ep.EmployeePipelineSalerReplyTime,
                EmployeePipelineSalerStatus = ep.EmployeePipelineSalerStatus,
                EmployeePipelineSalerCreateEmployeeName = employee?.EmployeeName,
                EmployeePipelineSalerCreateTime = ep.EmployeePipelineSalerCreateTime,
                EmployeePipelineSalerRemark = ep.EmployeePipelineSalerRemark
            }
        ).ToList();

        return rspObj;
    }

    /// <summary>管理者後台-CRM-電銷管理-邏輯服務-更新活動名單狀態</summary>
    public async Task<MbsCrmPhnLgcUpdateActivityEmployeePipelineStatusRspMdl> UpdateActivityEmployeePipelineStatusAsync(MbsCrmPhnLgcUpdateActivityEmployeePipelineStatusReqMdl reqObj)
    {
        var rspObj = new MbsCrmPhnLgcUpdateActivityEmployeePipelineStatusRspMdl
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
        if (mbsCmnLgcGetLoginUserInfoRspObj.AtomPermissionKindIdEdit != DbAtomPermissionKindEnum.Everyone)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");

            // 權限不足
            rspObj.ErrorCode = MbsErrorCodeEnum.PermissionDenied;
            return rspObj;
        }

        #endregion

        // db, 核心-員工-商機-資料庫-是否存在
        var coEmpPplDbExistReqObj = new CoEmpPplDbExistReqMdl()
        {
            EmployeePipelineID = reqObj.EmployeePipelineID
        };
        var coEmpPplDbExistRspObj = await this._coEmpPipelineDb.ExistAsync(coEmpPplDbExistReqObj);
        if (coEmpPplDbExistRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

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

        // 檢查當前狀態是否為已轉電銷
        if (coEmpPplDbGetRspObj.AtomPipelineStatus != DbAtomPipelineStatusEnum.TransferredToSales)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // db, 核心-員工-商機-資料庫-更新
        var coEmpPplDbUpdateReqObj = new CoEmpPplDbUpdateReqMdl()
        {
            EmployeePipelineID = reqObj.EmployeePipelineID,
            AtomPipelineStatus = DbAtomPipelineStatusEnum.CompletedSales
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

    /// <summary>管理者後台-CRM-電銷管理-邏輯服務-取得多筆客戶過往活動</summary>
    public async Task<MbsCrmPhnLgcGetManyPastActivityRspMdl> GetManyPastActivityAsync(MbsCrmPhnLgcGetManyPastActivityReqMdl reqObj)
    {
        var rspObj = new MbsCrmPhnLgcGetManyPastActivityRspMdl
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
        if (mbsCmnLgcGetLoginUserInfoRspObj.AtomPermissionKindIdView != DbAtomPermissionKindEnum.Everyone)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");

            // 權限不足
            rspObj.ErrorCode = MbsErrorCodeEnum.PermissionDenied;
            return rspObj;
        }

        #endregion

        // db, 核心-員工-商機原始資料-資料庫-取得
        var coEmpPplOgnDbGetReqObj = new CoEmpPplOgnDbGetReqMdl()
        {
            EmployeePipelineID = reqObj.EmployeePipelineID
        };
        var coEmpPplOgnDbGetRspObj = await this._coEmpPipelineOriginalDb.GetAsync(coEmpPplOgnDbGetReqObj);
        if (coEmpPplOgnDbGetRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

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

        // db, 核心-管理者-活動-資料庫-取得[筆數]過往活動從[窗口Email]
        var coMgrActivityDbGetPastActivityCountFromEmployeePipelineReqObj = new CoMgrActivityDbGetCountPastActivityFromContacterEmailReqMdl()
        {
            EmployeePipelineOriginalManagerContacterEmail = coEmpPplOgnDbGetRspObj.ManagerContacterEmail,
            FilterManagerActivityID = coEmpPplDbGetRspObj.ManagerActivityID
        };
        var coMgrActivityDbGetPastActivityCountFromEmployeePipelineRspObj = await this._coMgrActivityDb.GetCountPastActivityFromContacterEmailAsync(coMgrActivityDbGetPastActivityCountFromEmployeePipelineReqObj);
        if (coMgrActivityDbGetPastActivityCountFromEmployeePipelineRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // db, 核心-管理者-活動-資料庫-取得最新過往活動從[窗口Email]
        var coMgrActivityDbGetLatestPastActivityFromEmployeePipelineReqObj = new CoMgrActivityDbGetLatestPastActivityFromContacterEmailReqMdl()
        {
            EmployeePipelineOriginalManagerContacterEmail = coEmpPplOgnDbGetRspObj.ManagerContacterEmail,
            FilterManagerActivityID = coEmpPplDbGetRspObj.ManagerActivityID
        };
        var coMgrActivityDbGetLatestPastActivityFromEmployeePipelineRspObj = await this._coMgrActivityDb.GetLatestPastActivityFromContacterEmailAsync(coMgrActivityDbGetLatestPastActivityFromEmployeePipelineReqObj);

        // db, 核心-管理者-活動-資料庫-取得多筆過往活動從[窗口Email]
        var coMgrActivityDbGetManyPastActivityFromEmployeePipelineReqObj = new CoMgrActivityDbGetManyPastActivityFromContacterEmailReqMdl()
        {
            EmployeePipelineOriginalManagerContacterEmail = coEmpPplOgnDbGetRspObj.ManagerContacterEmail,
            FilterManagerActivityID = coEmpPplDbGetRspObj.ManagerActivityID,
            PageIndex = reqObj.PageIndex,
            PageSize = reqObj.PageSize
        };
        var coMgrActivityDbGetManyPastActivityFromEmployeePipelineRspObj = await this._coMgrActivityDb.GetManyPastActivityFromContacterEmailAsync(coMgrActivityDbGetManyPastActivityFromEmployeePipelineReqObj);
        if (coMgrActivityDbGetManyPastActivityFromEmployeePipelineRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        rspObj.LatestPastActivity = coMgrActivityDbGetLatestPastActivityFromEmployeePipelineRspObj != default
            ? new MbsCrmPhnLgcGetManyPastActivityRspItemMdl
            {
                ManagerActivityName = coMgrActivityDbGetLatestPastActivityFromEmployeePipelineRspObj.ManagerActivityName,
                ManagerActivityStartTime = coMgrActivityDbGetLatestPastActivityFromEmployeePipelineRspObj.ManagerActivityStartTime,
                ManagerActivityEndTime = coMgrActivityDbGetLatestPastActivityFromEmployeePipelineRspObj.ManagerActivityEndTime
            }
            : null;
        rspObj.PastActivityList = coMgrActivityDbGetManyPastActivityFromEmployeePipelineRspObj.ManagerActivityList
            .Select(x => new MbsCrmPhnLgcGetManyPastActivityRspItemMdl
            {
                ManagerActivityName = x.ManagerActivityName,
                ManagerActivityStartTime = x.ManagerActivityStartTime,
                ManagerActivityEndTime = x.ManagerActivityEndTime
            })
            .ToList();
        rspObj.TotalCount = coMgrActivityDbGetPastActivityCountFromEmployeePipelineRspObj.Count;
        return rspObj;
    }

    #endregion

    #region 客戶

    /// <summary>管理者後台-CRM-電銷管理-邏輯服務-取得客戶</summary>
    public async Task<MbsCrmPhnLgcGetEmployeePipelineCompanyRspMdl> GetEmployeePipelineCompanyAsync(MbsCrmPhnLgcGetEmployeePipelineCompanyReqMdl reqObj)
    {
        var rspObj = new MbsCrmPhnLgcGetEmployeePipelineCompanyRspMdl
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
        if (mbsCmnLgcGetLoginUserInfoRspObj.AtomPermissionKindIdView != DbAtomPermissionKindEnum.Everyone)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");

            // 權限不足
            rspObj.ErrorCode = MbsErrorCodeEnum.PermissionDenied;
            return rspObj;
        }

        #endregion

        // db, 核心-員工-商機原始資料-資料庫-取得
        var coEmpPplOgnDbGetReqObj = new CoEmpPplOgnDbGetReqMdl()
        {
            EmployeePipelineID = reqObj.EmployeePipelineID
        };
        var coEmpPplOgnDbGetRspObj = await this._coEmpPipelineOriginalDb.GetAsync(coEmpPplOgnDbGetReqObj);
        if (coEmpPplOgnDbGetRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

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

        var company = coEmpPplDbGetRspObj.ManagerCompanyID.HasValue
                            ? new MbsCrmPhnLgcGetEmployeePipelineCompanyRspItemMdl()
                            : null;
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
            company.AtomCity = coMgrCmpLocDbGetRspObj.AtomCity;
            company.ManagerCompanyLocationName = coMgrCmpLocDbGetRspObj.ManagerCompanyLocationName;
            company.ManagerCompanyLocationAddress = coMgrCmpLocDbGetRspObj.ManagerCompanyLocationAddress;
            company.ManagerCompanyLocationTelephone = coMgrCmpLocDbGetRspObj.ManagerCompanyLocationTelephone;
            company.ManagerCompanyLocationStatus = coMgrCmpLocDbGetRspObj.ManagerCompanyLocationStatus;
        }

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        // 原始客戶
        rspObj.OriginalCompany = new MbsCrmPhnLgcGetEmployeePipelineCompanyRspItemMdl
        {
            ManagerCompanyUnifiedNumber = coEmpPplOgnDbGetRspObj.ManagerCompanyUnifiedNumber,
            ManagerCompanyName = coEmpPplOgnDbGetRspObj.ManagerCompanyName,
            AtomEmployeeRange = coEmpPplOgnDbGetRspObj.AtomEmployeeRange,
            AtomCustomerGrade = coEmpPplOgnDbGetRspObj.AtomCustomerGrade,
            ManagerCompanyMainKindName = coEmpPplOgnDbGetRspObj.ManagerCompanyMainKindName,
            ManagerCompanySubKindName = coEmpPplOgnDbGetRspObj.ManagerCompanySubKindName,
            AtomCity = coEmpPplOgnDbGetRspObj.AtomCity,
            ManagerCompanyLocationAddress = coEmpPplOgnDbGetRspObj.ManagerCompanyLocationAddress,
            ManagerCompanyLocationTelephone = coEmpPplOgnDbGetRspObj.ManagerCompanyLocationTelephone,
            ManagerCompanyLocationStatus = coEmpPplOgnDbGetRspObj.ManagerCompanyLocationStatus
        };
        // 當前客戶
        rspObj.Company = company;
        return rspObj;
    }

    /// <summary>管理者後台-CRM-電銷管理-邏輯服務-更新名單客戶</summary>
    public async Task<MbsCrmPhnLgcUpdateEmployeePipelineCompanyRspMdl> UpdateEmployeePipelineCompanyAsync(MbsCrmPhnLgcUpdateEmployeePipelineCompanyReqMdl reqObj)
    {
        var rspObj = new MbsCrmPhnLgcUpdateEmployeePipelineCompanyRspMdl
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
        if (mbsCmnLgcGetLoginUserInfoRspObj.AtomPermissionKindIdEdit != DbAtomPermissionKindEnum.Everyone)
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

        // 匯入活動產品,只會匯入一次,先判斷有沒有產品資訊
        // db, 核心-員工-商機產品-資料庫-取得數量
        var coEmpPplPrdDbGetCountReqObj = new CoEmpPplPrdDbGetCountReqMdl
        {
            EmployeePipelineID = reqObj.EmployeePipelineID
        };
        var coEmpPplPrdDbGetCountRspObj = await this._coEmpPipelineProductDb.GetCountAsync(coEmpPplPrdDbGetCountReqObj);
        if (coEmpPplPrdDbGetCountRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        if (coEmpPplDbGetRspObj.ManagerActivityID.HasValue &&
            coEmpPplPrdDbGetCountRspObj.Count == 0)
        {
            // db, 核心-管理者-活動產品-資料庫-取得多筆
            var coMgrActPrdDbGetManyReqObj = new CoMgrActPrdDbGetManyReqMdl()
            {
                ManagerActivityID = coEmpPplDbGetRspObj.ManagerActivityID.Value
            };
            var coMgrActPrdDbGetManyRspObj = await this._coMgrActivityProductDb.GetManyAsync(coMgrActPrdDbGetManyReqObj);
            if (coMgrActPrdDbGetManyRspObj == default)
            {
                this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                return rspObj;
            }

            // 匯入活動產品

            // db, 核心-管理者-產品-資料庫-取得多筆從[產品ID]
            var coMgrPrdDbGetManyFromManagerProductIDReqObj = new CoMgrPrdDbGetManyFromManagerProductIDReqMdl
            {
                ManagerProductIDList = coMgrActPrdDbGetManyRspObj.ManagerActivityProductList
                                            .Select(p => p.ManagerProductID)
                                            .Distinct()
                                            .ToList(),
                ManagerProductIsEnable = true
            };
            var coMgrPrdDbGetManyFromManagerProductIDRspObj = await this._coMgrProductDb.GetManyFromManagerProductIDAsync(coMgrPrdDbGetManyFromManagerProductIDReqObj);
            if (coMgrPrdDbGetManyFromManagerProductIDRspObj == default)
            {
                this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                return rspObj;
            }

            // 活動產品沒有規格資料,這邊暫時先取得產品的第一筆規格資料
            // db, 核心-管理者-產品規格-資料庫-取得多筆從[產品ID]     
            var coMgrPsDbGetManyFromManagerProductIDReqObj = new CoMgrPsDbGetManyFromManagerProductIDReqMdl()
            {
                ManagerProductIDList = coMgrActPrdDbGetManyRspObj.ManagerActivityProductList
                                            .Select(p => p.ManagerProductID)
                                            .Distinct()
                                            .ToList(),
                ManagerProductSpecificationIsEnable = true
            };
            var coMgrPsDbGetManyFromManagerProductIDRspObj = await this._coMgrProductSpecificationDb.GetManyFromManagerProductIDAsync(coMgrPsDbGetManyFromManagerProductIDReqObj);
            if (coMgrPsDbGetManyFromManagerProductIDRspObj == default)
            {
                this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                return rspObj;
            }
            // 依照產品ID分組,且只取第一筆產品規格
            var groupedSpecifications = coMgrPsDbGetManyFromManagerProductIDRspObj.ManagerProductSpecificationList
                .GroupBy(p => p.ManagerProductID)
                .Select(g => g.FirstOrDefault())
                .ToList();

            // db, 核心-員工-商機產品-資料庫-新增多筆
            var coEmpPplPrdDbAddManyReqObj = new CoEmpPplPrdDbAddManyReqMdl()
            {
                EmployeePipelineProductList = (
                    from ap in coMgrActPrdDbGetManyRspObj.ManagerActivityProductList
                    from p in coMgrPrdDbGetManyFromManagerProductIDRspObj.ManagerProductList
                                    .Where(p => p.ManagerProductID == ap.ManagerProductID)
                    from gp in groupedSpecifications
                                    .Where(g => g.ManagerProductID == ap.ManagerProductID)
                    select new CoEmpPplPrdDbAddManyReqItemMdl()
                    {
                        EmployeePipelineID = reqObj.EmployeePipelineID,
                        ManagerCompanyID = reqObj.ManagerCompanyID,
                        ManagerProductID = ap.ManagerProductID,
                        ManagerProductMainKindID = p.ManagerProductMainKindID,
                        ManagerProductSubKindID = p.ManagerProductSubKindID,
                        ManagerProductSpecificationID = gp.ManagerProductSpecificationID,
                        EmployeePipelineProductSellPrice = gp.ManagerProductSpecificationSellPrice,
                        EmployeePipelineProductClosingPrice = 0, // 業務才會知道售價
                        EmployeePipelineProductCostPrice = gp.ManagerProductSpecificationCostPrice,
                        EmployeePipelineProductCount = 1,
                        EmployeePipelineProductPurchaseKindID = DbAtomEmployeePipelineProductPurchaseKindEnum.NotSelected,
                        EmployeePipelineProductContractKindID = DbAtomEmployeePipelineProductContractKindEnum.NotSelected,
                        EmployeePipelineProductContractText = string.Empty
                    }
                ).ToList()
            };
            var coEmpPplPrdDbAddManyRspObj = await this._coEmpPipelineProductDb.AddManyAsync(coEmpPplPrdDbAddManyReqObj);
            if (coEmpPplPrdDbAddManyRspObj == default)
            {
                this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                return rspObj;
            }
        }

        // db, 核心-員工-商機產品-資料庫-更新多筆公司ID
        var coEmpPplPrdDbUpdateManyCompanyIdReqObj = new CoEmpPplPrdDbUpdateManyCompanyIdReqMdl()
        {
            EmployeePipelineID = reqObj.EmployeePipelineID,
            ManagerCompanyID = reqObj.ManagerCompanyID
        };
        var coEmpPplPrdDbUpdateManyCompanyIdRspObj = await this._coEmpPipelineProductDb.UpdateManyCompanyIdAsync(coEmpPplPrdDbUpdateManyCompanyIdReqObj);
        if (coEmpPplPrdDbUpdateManyCompanyIdRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        return rspObj;
    }

    #endregion

    #region 窗口

    /// <summary>管理者後台-CRM-電銷管理-邏輯服務-取得原始窗口</summary>
    public async Task<MbsCrmPhnLgcGetOriginalEmployeePipelineContacterRspMdl> GetOriginalEmployeePipelineContacterAsync(MbsCrmPhnLgcGetOriginalEmployeePipelineContacterReqMdl reqObj)
    {
        var rspObj = new MbsCrmPhnLgcGetOriginalEmployeePipelineContacterRspMdl
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
        if (mbsCmnLgcGetLoginUserInfoRspObj.AtomPermissionKindIdView != DbAtomPermissionKindEnum.Everyone)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");

            // 權限不足
            rspObj.ErrorCode = MbsErrorCodeEnum.PermissionDenied;
            return rspObj;
        }

        #endregion

        // db, 核心-員工-商機原始資料-資料庫-取得
        var coEmpPplOgnDbGetReqObj = new CoEmpPplOgnDbGetReqMdl()
        {
            EmployeePipelineID = reqObj.EmployeePipelineID
        };
        var coEmpPplOgnDbGetRspObj = await this._coEmpPipelineOriginalDb.GetAsync(coEmpPplOgnDbGetReqObj);
        if (coEmpPplOgnDbGetRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        // 原始窗口資訊
        rspObj.ManagerContacterName = coEmpPplOgnDbGetRspObj.ManagerContacterName;
        rspObj.ManagerContacterEmail = coEmpPplOgnDbGetRspObj.ManagerContacterEmail;
        rspObj.ManagerContacterPhone = coEmpPplOgnDbGetRspObj.ManagerContacterPhone;
        rspObj.ManagerContacterDepartment = coEmpPplOgnDbGetRspObj.ManagerContacterDepartment;
        rspObj.ManagerContacterJobTitle = coEmpPplOgnDbGetRspObj.ManagerContacterJobTitle;
        rspObj.ManagerContacterTelephone = coEmpPplOgnDbGetRspObj.ManagerContacterTelephone;
        rspObj.ManagerContacterIsConsent = coEmpPplOgnDbGetRspObj.ManagerContacterIsConsent;
        rspObj.ManagerContacterStatus = coEmpPplOgnDbGetRspObj.ManagerContacterStatus;
        rspObj.AtomRatingKind = coEmpPplOgnDbGetRspObj.AtomRatingKind;
        return rspObj;
    }

    /// <summary>管理者後台-CRM-電銷管理-邏輯服務-新增名單窗口</summary>
    public async Task<MbsCrmPhnLgcAddEmployeePipelineContacterRspMdl> AddEmployeePipelineContacterAsync(MbsCrmPhnLgcAddEmployeePipelineContacterReqMdl reqObj)
    {
        var rspObj = new MbsCrmPhnLgcAddEmployeePipelineContacterRspMdl
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
        if (mbsCmnLgcGetLoginUserInfoRspObj.AtomPermissionKindIdCreate != DbAtomPermissionKindEnum.Everyone)
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

    /// <summary>管理者後台-CRM-電銷管理-邏輯服務-更新名單窗口</summary>
    public async Task<MbsCrmPhnLgcUpdateEmployeePipelineContacterRspMdl> UpdateEmployeePipelineContacterAsync(MbsCrmPhnLgcUpdateEmployeePipelineContacterReqMdl reqObj)
    {
        var rspObj = new MbsCrmPhnLgcUpdateEmployeePipelineContacterRspMdl
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
        if (mbsCmnLgcGetLoginUserInfoRspObj.AtomPermissionKindIdEdit != DbAtomPermissionKindEnum.Everyone)
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

    /// <summary>管理者後台-CRM-電銷管理-邏輯服務-刪除名單窗口</summary>
    public async Task<MbsCrmPhnLgcRemoveEmployeePipelineContacterRspMdl> RemoveEmployeePipelineContacterAsync(MbsCrmPhnLgcRemoveEmployeePipelineContacterReqMdl reqObj)
    {
        var rspObj = new MbsCrmPhnLgcRemoveEmployeePipelineContacterRspMdl
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
        if (mbsCmnLgcGetLoginUserInfoRspObj.AtomPermissionKindIdDelete != DbAtomPermissionKindEnum.Everyone)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");

            // 權限不足
            rspObj.ErrorCode = MbsErrorCodeEnum.PermissionDenied;
            return rspObj;
        }

        #endregion

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

    /// <summary>管理者後台-CRM-電銷管理-邏輯服務-取得多筆名單窗口</summary>
    public async Task<MbsCrmPhnLgcGetManyEmployeePipelineContacterRspMdl> GetManyEmployeePipelineContacterAsync(MbsCrmPhnLgcGetManyEmployeePipelineContacterReqMdl reqObj)
    {
        var rspObj = new MbsCrmPhnLgcGetManyEmployeePipelineContacterRspMdl
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
        if (mbsCmnLgcGetLoginUserInfoRspObj.AtomPermissionKindIdView != DbAtomPermissionKindEnum.Everyone)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");

            // 權限不足
            rspObj.ErrorCode = MbsErrorCodeEnum.PermissionDenied;
            return rspObj;
        }

        #endregion

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
            select new MbsCrmPhnLgcGetManyEmployeePipelineContacterRspItemMdl
            {
                EmployeePipelineContacterID = epc.EmployeePipelineContacterID,
                ManagerContacterID = mc.ManagerContacterID,
                ManagerContacterName = mc.ManagerContacterName,
                ManagerContacterEmail = mc.ManagerContacterEmail,
                ManagerContacterPhone = mc.ManagerContacterPhone,
                ManagerContacterDepartment = mc.ManagerContacterDepartment,
                ManagerContacterJobTitle = mc.ManagerContacterJobTitle,
                ManagerContacterTelephone = mc.ManagerContacterTelephone,
                ManagerContacterIsConsent = mc.ManagerContacterIsConsent,
                ManagerContacterStatus = mc.ManagerContacterStatus,
                AtomRatingKind = mc.AtomRatingKind,
                EmployeePipelineContacterIsPrimary = epc.EmployeePipelineContacterIsPrimary,
                ManagerContacterRemark = mc.ManagerContacterRemark
            }
        ).ToList();
        return rspObj;
    }

    #endregion

    #region 指派業務紀錄

    /// <summary>管理者後台-CRM-電銷管理-邏輯服務-指派業務</summary>
    public async Task<MbsCrmPhnLgcAddEmployeePipelineSalerRspMdl> AddEmployeePipelineSalerAsync(MbsCrmPhnLgcAddEmployeePipelineSalerReqMdl reqObj)
    {
        var rspObj = new MbsCrmPhnLgcAddEmployeePipelineSalerRspMdl
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
        if (mbsCmnLgcGetLoginUserInfoRspObj.AtomPermissionKindIdCreate != DbAtomPermissionKindEnum.Everyone)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");

            // 權限不足
            rspObj.ErrorCode = MbsErrorCodeEnum.PermissionDenied;
            return rspObj;
        }

        #endregion

        // 檢查指派業務
        // db, 核心-員工-資訊-資料庫-取得
        var coEmpInfDbGetReqObj = new CoEmpInfDbGetReqMdl()
        {
            EmployeeID = reqObj.EmployeePipelineSalerEmployeeID
        };
        var coEmpInfDbGetRspObj = await this._coEmpInfoDb.GetAsync(coEmpInfDbGetReqObj);
        if (coEmpInfDbGetRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

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

        // 檢查是否已經有指派過業務
        if (coEmpPplDbGetRspObj.EmployeePipelineSalerEmployeeID.HasValue)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // [名單公司]與[名單窗口]都需必填,才可以指派業務
        if (coEmpPplDbGetRspObj.ManagerCompanyID.HasValue == false)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }
        // db, 核心-員工-商機窗口-資料庫-取得[筆數]
        var coEmpPplContDbGetCountReqObj = new CoEmpPplContDbGetCountReqMdl()
        {
            EmployeePipelineID = coEmpPplDbGetRspObj.EmployeePipelineID
        };
        var coEmpPplContDbGetCountRspObj = await this._coEmpPipelineContacterDb.GetCountAsync(coEmpPplContDbGetCountReqObj);
        if (coEmpPplContDbGetCountRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }
        if (coEmpPplContDbGetCountRspObj.Count == 0)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // db, 核心-員工-商機業務-資料庫-是否存在
        var coEmpPplSlrDbExistsReqObj = new CoEmpPplSlrDbExistsReqMdl()
        {
            EmployeePipelineID = reqObj.EmployeePipelineID,
            EmployeePipelineSalerEmployeeID = reqObj.EmployeePipelineSalerEmployeeID,
            EmployeePipelineSalerStatus = DbAtomEmployeePipelineSalerStatusEnum.Pending
        };
        var coEmpPplSlrDbExistsRspObj = await this._coEmpPipelineSalerDb.ExistsAsync(coEmpPplSlrDbExistsReqObj);
        if (coEmpPplSlrDbExistsRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }
        if (coEmpPplSlrDbExistsRspObj.IsExist)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            rspObj.ErrorCode = MbsErrorCodeEnum.DataAlreadyExists;
            return rspObj;
        }

        // db, 核心-員工-商機-資料庫-更新
        var coEmpPplDbUpdateReqObj = new CoEmpPplDbUpdateReqMdl()
        {
            EmployeePipelineID = reqObj.EmployeePipelineID,
            AtomPipelineStatus = DbAtomPipelineStatusEnum.TransferredToBusiness,
            EmployeePipelineSalerEmployeeID = reqObj.EmployeePipelineSalerEmployeeID
        };
        var coEmpPplDbUpdateRspObj = await this._coEmpPipelineDb.UpdateAsync(coEmpPplDbUpdateReqObj);
        if (coEmpPplDbUpdateRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // db, 核心-員工-商機業務-資料庫-新增
        var coEmpPplSlrDbAddReqObj = new CoEmpPplSlrDbAddReqMdl()
        {
            EmployeePipelineID = reqObj.EmployeePipelineID,
            EmployeePipelineSalerEmployeeID = reqObj.EmployeePipelineSalerEmployeeID,
            EmployeePipelineSalerStatus = DbAtomEmployeePipelineSalerStatusEnum.Pending,
            CreateEmployeeID = mbsCmnLgcGetLoginUserInfoRspObj.EmployeeID
        };
        var coEmpPplSlrDbAddRspObj = await this._coEmpPipelineSalerDb.AddAsync(coEmpPplSlrDbAddReqObj);
        if (coEmpPplSlrDbAddRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        return rspObj;
    }

    /// <summary>管理者後台-CRM-電銷管理-邏輯服務-取得多筆指派業務</summary>
    public async Task<MbsCrmPhnLgcGetManyEmployeePipelineSalerRspMdl> GetManyEmployeePipelineSalerAsync(MbsCrmPhnLgcGetManyEmployeePipelineSalerReqMdl reqObj)
    {
        var rspObj = new MbsCrmPhnLgcGetManyEmployeePipelineSalerRspMdl
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
        if (mbsCmnLgcGetLoginUserInfoRspObj.AtomPermissionKindIdView != DbAtomPermissionKindEnum.Everyone)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");

            // 權限不足
            rspObj.ErrorCode = MbsErrorCodeEnum.PermissionDenied;
            return rspObj;
        }

        #endregion

        // db, 核心-員工-商機業務-資料庫-取得多筆
        var coEmpPplSlrDbGetManyReqObj = new CoEmpPplSlrDbGetManyReqMdl()
        {
            EmployeePipelineID = reqObj.EmployeePipelineID
        };
        var coEmpPplSlrDbGetManyRspObj = await this._coEmpPipelineSalerDb.GetManyAsync(coEmpPplSlrDbGetManyReqObj);
        if (coEmpPplSlrDbGetManyRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // 查詢員工名稱 From EmployeeID
        var queryEmployeeNameList = new List<int>();
        if (coEmpPplSlrDbGetManyRspObj.EmployeePipelineSalerList != null)
        {
            // 指派員工ID
            queryEmployeeNameList.AddRange(coEmpPplSlrDbGetManyRspObj.EmployeePipelineSalerList
                                                .Select(x => x.EmployeePipelineSalerCreateEmployeeID)
                                                .Distinct()
                                                .ToList());
            // 業務員工ID                                                    
            queryEmployeeNameList.AddRange(coEmpPplSlrDbGetManyRspObj.EmployeePipelineSalerList
                                                .Select(x => x.EmployeePipelineSalerEmployeeID)
                                                .Distinct()
                                                .ToList());
        }

        // db, 核心-員工-資訊-資料庫-取得多筆[名稱]
        var coEmpInfDbGetManyNameReqObj = new CoEmpInfDbGetManyNameReqMdl()
        {
            EmployeeIdList = queryEmployeeNameList.Distinct().ToList()
        };
        var coEmpInfDbGetManyNameRspObj = await this._coEmpInfoDb.GetManyNameAsync(coEmpInfDbGetManyNameReqObj);
        if (coEmpInfDbGetManyNameRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        rspObj.EmployeePipelineSalerList = (
            from s in coEmpPplSlrDbGetManyRspObj.EmployeePipelineSalerList
            from ce in coEmpInfDbGetManyNameRspObj.EmployeeList
                        .Where(ce => ce.EmployeeID == s.EmployeePipelineSalerCreateEmployeeID)
                        .DefaultIfEmpty()
            from se in coEmpInfDbGetManyNameRspObj.EmployeeList
                        .Where(se => se.EmployeeID == s.EmployeePipelineSalerEmployeeID)
                        .DefaultIfEmpty()
            select new MbsCrmPhnLgcGetManyEmployeePipelineSalerRspItemMdl
            {
                EmployeePipelineSalerID = s.EmployeePipelineSalerID,
                EmployeePipelineSalerStatus = s.EmployeePipelineSalerStatus,
                EmployeePipelineSalerCreateTime = s.EmployeePipelineSalerCreateTime,
                EmployeePipelineSalerCreateEmployeeName = ce?.EmployeeName,
                EmployeePipelineSalerReplyTime = s.EmployeePipelineSalerReplyTime,
                EmployeePipelineSalerEmployeeName = se?.EmployeeName,
                EmployeePipelineSalerRemark = s.EmployeePipelineSalerRemark
            }
        ).ToList();
        return rspObj;
    }

    #endregion

    #region 電銷紀錄

    /// <summary>管理者後台-CRM-電銷管理-邏輯服務-新增電銷紀錄</summary>
    public async Task<MbsCrmPhnLgcAddEmployeePipelinePhoneRspMdl> AddEmployeePipelinePhoneAsync(MbsCrmPhnLgcAddEmployeePipelinePhoneReqMdl reqObj)
    {
        var rspObj = new MbsCrmPhnLgcAddEmployeePipelinePhoneRspMdl
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
        if (mbsCmnLgcGetLoginUserInfoRspObj.AtomPermissionKindIdCreate != DbAtomPermissionKindEnum.Everyone)
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

        // 檢查窗口-公司跟商機-公司是否為相同
        if (coMgrCttDbGetRspObj.ManagerCompanyID != coEmpPplDbGetRspObj.ManagerCompanyID)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // [名單公司]需必填,才可以新增電銷紀錄
        if (coEmpPplDbGetRspObj.ManagerCompanyID.HasValue == false)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
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
        // [名單窗口]需必填且判斷窗口ID是否已新增,才可以新增電銷紀錄
        if (coEmpPplContDbGetManyRspObj.EmployeePipelineContacterList.Count == 0)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // db, 核心-員工-商機電銷-資料庫-新增
        var coEmpPplPhnDbAddReqObj = new CoEmpPplPhnDbAddReqMdl()
        {
            EmployeePipelineID = reqObj.EmployeePipelineID,
            EmployeePipelinePhoneRecordTime = reqObj.EmployeePipelinePhoneRecordTime,
            ManagerContacterID = reqObj.ManagerContacterID,
            EmployeePipelinePhoneTitle = reqObj.EmployeePipelinePhoneTitle,
            EmployeePipelinePhoneRemark = reqObj.EmployeePipelinePhoneRemark,
            EmployeePipelinePhoneCreateEmployeeID = mbsCmnLgcGetLoginUserInfoRspObj.EmployeeID
        };
        var coEmpPplPhnDbAddRspObj = await this._coEmpPipelinePhoneDb.AddAsync(coEmpPplPhnDbAddReqObj);
        if (coEmpPplPhnDbAddRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        return rspObj;
    }

    /// <summary>管理者後台-CRM-電銷管理-邏輯服務-取得多筆電銷紀錄</summary>
    public async Task<MbsCrmPhnLgcGetManyEmployeePipelinePhoneRspMdl> GetManyEmployeePipelinePhoneAsync(MbsCrmPhnLgcGetManyEmployeePipelinePhoneReqMdl reqObj)
    {
        var rspObj = new MbsCrmPhnLgcGetManyEmployeePipelinePhoneRspMdl
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
        if (mbsCmnLgcGetLoginUserInfoRspObj.AtomPermissionKindIdView != DbAtomPermissionKindEnum.Everyone)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");

            // 權限不足
            rspObj.ErrorCode = MbsErrorCodeEnum.PermissionDenied;
            return rspObj;
        }

        #endregion

        // db, 核心-員工-商機電銷-資料庫-取得多筆
        var coEmpPplPhnDbGetManyReqObj = new CoEmpPplPhnDbGetManyReqMdl()
        {
            EmployeePipelineID = reqObj.EmployeePipelineID
        };
        var coEmpPplPhnDbGetManyRspObj = await this._coEmpPipelinePhoneDb.GetManyAsync(coEmpPplPhnDbGetManyReqObj);
        if (coEmpPplPhnDbGetManyRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // db, 核心-管理者-窗口-資料庫服務-取得多筆[名稱]
        var coMgrCttDbGetManyNameReqObj = new CoMgrCttDbGetManyNameReqMdl()
        {
            ManagerContacterIDList = coEmpPplPhnDbGetManyRspObj.EmployeePipelinePhoneList
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

        // 查詢員工名稱 From EmployeeID
        var queryEmployeeNameList = new List<int>();
        if (coEmpPplPhnDbGetManyRspObj.EmployeePipelinePhoneList != null)
        {
            // 電銷員工ID
            queryEmployeeNameList.AddRange(coEmpPplPhnDbGetManyRspObj.EmployeePipelinePhoneList
                                                .Select(x => x.EmployeePipelinePhoneCreateEmployeeID)
                                                .Distinct()
                                                .ToList());
        }

        // db, 核心-員工-資訊-資料庫-取得多筆[名稱]
        var coEmpInfDbGetManyNameReqObj = new CoEmpInfDbGetManyNameReqMdl()
        {
            EmployeeIdList = queryEmployeeNameList.Distinct().ToList()
        };
        var coEmpInfDbGetManyNameRspObj = await this._coEmpInfoDb.GetManyNameAsync(coEmpInfDbGetManyNameReqObj);
        if (coEmpInfDbGetManyNameRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        rspObj.EmployeePipelinePhoneList = (
            from ep in coEmpPplPhnDbGetManyRspObj.EmployeePipelinePhoneList
            from mc in coMgrCttDbGetManyNameRspObj.ManagerContacterList
                        .Where(mc => mc.ManagerContacterID == ep.ManagerContacterID)
                        .DefaultIfEmpty()
            from e in coEmpInfDbGetManyNameRspObj.EmployeeList
                        .Where(e => e.EmployeeID == ep.EmployeePipelinePhoneCreateEmployeeID)
                        .DefaultIfEmpty()
            select new MbsCrmPhnLgcGetManyEmployeePipelinePhoneRspItemMdl
            {
                EmployeePipelinePhoneID = ep.EmployeePipelinePhoneID,
                EmployeePipelinePhoneRecordTime = ep.EmployeePipelinePhoneRecordTime,
                ManagerContacterName = mc?.ManagerContacterName,
                EmployeePipelinePhoneTitle = ep.EmployeePipelinePhoneTitle,
                EmployeePipelinePhoneRemark = ep.EmployeePipelinePhoneRemark,
                EmployeePipelinePhoneCreateEmployeeName = e?.EmployeeName
            }
        ).ToList();
        return rspObj;
    }

    #endregion

    #region 電銷產品

    /// <summary>管理者後台-CRM-電銷管理-邏輯服務-取得多筆電銷產品</summary>
    public async Task<MbsCrmPhnLgcGetManyEmployeePipelineProductRspMdl> GetManyEmployeePipelineProductAsync(MbsCrmPhnLgcGetManyEmployeePipelineProductReqMdl reqObj)
    {
        var rspObj = new MbsCrmPhnLgcGetManyEmployeePipelineProductRspMdl
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
        if (mbsCmnLgcGetLoginUserInfoRspObj.AtomPermissionKindIdView != DbAtomPermissionKindEnum.Everyone)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");

            // 權限不足
            rspObj.ErrorCode = MbsErrorCodeEnum.PermissionDenied;
            return rspObj;
        }

        #endregion

        // db, 核心-員工-商機產品-資料庫-取得多筆從[管理者後台-CRM-電銷管理]
        var coEmpPplPrdDbGetManyFromPhoneReqObj = new CoEmpPplPrdDbGetManyFromPhoneReqMdl()
        {
            EmployeePipelineID = reqObj.EmployeePipelineID
        };
        var coEmpPplPrdDbGetManyFromPhoneRspObj = await this._coEmpPipelineProductDb.GetManyFromPhoneAsync(coEmpPplPrdDbGetManyFromPhoneReqObj);
        if (coEmpPplPrdDbGetManyFromPhoneRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        rspObj.EmployeePipelineProductList = (
            from epp in coEmpPplPrdDbGetManyFromPhoneRspObj.EmployeePipelineProductList
            select new MbsCrmPhnLgcGetManyEmployeePipelineProductRspItemMdl
            {
                EmployeePipelineProductID = epp.EmployeePipelineProductID,
                ManagerProductID = epp.ManagerProductID,
                ManagerProductName = epp.ManagerProductName,
                ManagerProductMainKindID = epp.ManagerProductMainKindID,
                ManagerProductMainKindName = epp.ManagerProductMainKindName,
                ManagerProductSubKindID = epp.ManagerProductSubKindID,
                ManagerProductSubKindName = epp.ManagerProductSubKindName,
                ManagerProductSpecificationID = epp.ManagerProductSpecificationID,
                ManagerProductSpecificationName = epp.ManagerProductSpecificationName,
                ManagerProductSpecificationSellPrice = epp.ManagerProductSpecificationSellPrice,
            }
        )
        .OrderBy(x => x.EmployeePipelineProductID)
        .ToList();
        return rspObj;
    }

    /// <summary>管理者後台-CRM-電銷管理-邏輯服務-取得電銷產品</summary>
    public async Task<MbsCrmPhnLgcGetEmployeePipelineProductRspMdl> GetEmployeePipelineProductAsync(MbsCrmPhnLgcGetEmployeePipelineProductReqMdl reqObj)
    {
        var rspObj = new MbsCrmPhnLgcGetEmployeePipelineProductRspMdl
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
        if (mbsCmnLgcGetLoginUserInfoRspObj.AtomPermissionKindIdView != DbAtomPermissionKindEnum.Everyone)
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
        return rspObj;
    }

    /// <summary>管理者後台-CRM-電銷管理-邏輯服務-新增電銷產品</summary>
    public async Task<MbsCrmPhnLgcAddEmployeePipelineProductRspMdl> AddEmployeePipelineProductAsync(MbsCrmPhnLgcAddEmployeePipelineProductReqMdl reqObj)
    {
        var rspObj = new MbsCrmPhnLgcAddEmployeePipelineProductRspMdl
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
        if (mbsCmnLgcGetLoginUserInfoRspObj.AtomPermissionKindIdCreate != DbAtomPermissionKindEnum.Everyone)
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
        if (coEmpPplPrdDbExistRspObj.IsExist)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");

            rspObj.ErrorCode = MbsErrorCodeEnum.NameDuplicate;
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

        // db, 核心-管理者-產品-資料庫-取得
        var coMgrPrdDbGetReqObj = new CoMgrPrdDbGetReqMdl()
        {
            ManagerProductID = reqObj.ManagerProductID,
        };
        var coMgrPrdDbGetRspObj = await this._coMgrProductDb.GetAsync(coMgrPrdDbGetReqObj);
        if (coMgrPrdDbGetRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // db, 核心-管理者-產品規格-資料庫-取得
        var coMgrPsDbGetReqObj = new CoMgrPsDbGetReqMdl()
        {
            ManagerProductSpecificationID = reqObj.ManagerProductSpecificationID,
        };
        var coMgrPsDbGetRspObj = await this._coMgrProductSpecificationDb.GetAsync(coMgrPsDbGetReqObj);
        if (coMgrPsDbGetRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // db, 核心-員工-商機產品-資料庫-新增
        var coEmpPplPrdDbAddReqObj = new CoEmpPplPrdDbAddReqMdl()
        {
            EmployeePipelineID = reqObj.EmployeePipelineID,
            ManagerCompanyID = coEmpPplDbGetRspObj.ManagerCompanyID,
            ManagerProductID = reqObj.ManagerProductID,
            ManagerProductSpecificationID = reqObj.ManagerProductSpecificationID,
            EmployeePipelineProductSellPrice = coMgrPsDbGetRspObj.ManagerProductSpecificationSellPrice,
            EmployeePipelineProductClosingPrice = 0,
            EmployeePipelineProductCostPrice = coMgrPsDbGetRspObj.ManagerProductSpecificationCostPrice,
            EmployeePipelineProductCount = 1,
            EmployeePipelineProductPurchaseKindID = DbAtomEmployeePipelineProductPurchaseKindEnum.NotSelected,
            EmployeePipelineProductContractKindID = DbAtomEmployeePipelineProductContractKindEnum.NotSelected,
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

    /// <summary>管理者後台-CRM-電銷管理-邏輯服務-更新電銷產品</summary>
    public async Task<MbsCrmPhnLgcUpdateEmployeePipelineProductRspMdl> UpdateEmployeePipelineProductAsync(MbsCrmPhnLgcUpdateEmployeePipelineProductReqMdl reqObj)
    {
        var rspObj = new MbsCrmPhnLgcUpdateEmployeePipelineProductRspMdl
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
        if (mbsCmnLgcGetLoginUserInfoRspObj.AtomPermissionKindIdEdit != DbAtomPermissionKindEnum.Everyone)
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

        if (reqObj.ManagerProductSpecificationID.HasValue
            && reqObj.ManagerProductSpecificationID.Value != coEmpPplPrdDbGetRspObj.ManagerProductSpecificationID)
        {
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
            if (coEmpPplPrdDbExistRspObj.IsExist)
            {
                this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                rspObj.ErrorCode = MbsErrorCodeEnum.NameDuplicate;
                return rspObj;
            }

            // db, 核心-管理者-產品規格-資料庫-取得
            var coMgrPsDbGetReqObj = new CoMgrPsDbGetReqMdl()
            {
                ManagerProductSpecificationID = reqObj.ManagerProductSpecificationID.Value,
            };
            var coMgrPsDbGetRspObj = await this._coMgrProductSpecificationDb.GetAsync(coMgrPsDbGetReqObj);
            if (coMgrPsDbGetRspObj == default)
            {
                this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                return rspObj;
            }

            // db, 核心-管理者-產品-資料庫-取得
            var coMgrPrdDbGetReqObj = new CoMgrPrdDbGetReqMdl()
            {
                ManagerProductID = coMgrPsDbGetRspObj.ManagerProductID,
            };
            var coMgrPrdDbGetRspObj = await this._coMgrProductDb.GetAsync(coMgrPrdDbGetReqObj);
            if (coMgrPrdDbGetRspObj == default)
            {
                this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                return rspObj;
            }

            // db, 核心-員工-商機產品-資料庫-更新
            var coEmpPplPrdDbUpdateReqObj = new CoEmpPplPrdDbUpdateReqMdl()
            {
                EmployeePipelineProductID = reqObj.EmployeePipelineProductID,
                ManagerProductID = coMgrPsDbGetRspObj.ManagerProductID,
                ManagerProductSpecificationID = reqObj.ManagerProductSpecificationID.Value,
                EmployeePipelineProductSellPrice = coMgrPsDbGetRspObj.ManagerProductSpecificationSellPrice,
                EmployeePipelineProductClosingPrice = 0,
                EmployeePipelineProductCostPrice = coMgrPsDbGetRspObj.ManagerProductSpecificationCostPrice,
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

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        return rspObj;
    }

    /// <summary>管理者後台-CRM-電銷管理-邏輯服務-刪除電銷產品</summary>
    public async Task<MbsCrmPhnLgcRemoveEmployeePipelineProductRspMdl> RemoveEmployeePipelineProductAsync(MbsCrmPhnLgcRemoveEmployeePipelineProductReqMdl reqObj)
    {
        var rspObj = new MbsCrmPhnLgcRemoveEmployeePipelineProductRspMdl
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
        if (mbsCmnLgcGetLoginUserInfoRspObj.AtomPermissionKindIdDelete != DbAtomPermissionKindEnum.Everyone)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");

            // 權限不足
            rspObj.ErrorCode = MbsErrorCodeEnum.PermissionDenied;
            return rspObj;
        }

        #endregion

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
}