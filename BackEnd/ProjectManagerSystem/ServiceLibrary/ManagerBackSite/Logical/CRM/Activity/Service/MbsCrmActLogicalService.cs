using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using ApiModelLibrary.ManagerBackSite.Common;
using CommonLibrary.CmnEnum.Extension;
using CommonLibrary.CmnExcel.Format;
using CommonLibrary.CmnExcel.Service;
using CommonLibrary.CmnFolderFile.Format;
using CommonLibrary.CmnFolderFile.Service;
using DataModelLibrary.Database.AtomCity;
using DataModelLibrary.Database.AtomManagerActivity;
using DataModelLibrary.Database.AtomManagerCompany;
using DataModelLibrary.Database.AtomManagerContacter;
using DataModelLibrary.Database.AtomMenu;
using DataModelLibrary.Database.AtomPermissionKind;
using DataModelLibrary.Database.AtomPipeline;
using DataModelLibrary.Database.ManagerActivitySurveyQuestion;
using Microsoft.Extensions.Logging;
using ServiceLibrary.Core.Employee.DB.Pipeline.Format;
using ServiceLibrary.Core.Employee.DB.Pipeline.Service;
using ServiceLibrary.Core.Employee.DB.PipelineContacter.Format;
using ServiceLibrary.Core.Employee.DB.PipelineContacter.Service;
using ServiceLibrary.Core.Employee.DB.PipelineOriginal.Format;
using ServiceLibrary.Core.Employee.DB.PipelineOriginal.Service;
using ServiceLibrary.Core.Employee.Logical.Service;
using ServiceLibrary.Core.Manager.DB.Activity.Format;
using ServiceLibrary.Core.Manager.DB.Activity.Service;
using ServiceLibrary.Core.Manager.DB.ActivityExpense.Format;
using ServiceLibrary.Core.Manager.DB.ActivityExpense.Service;
using ServiceLibrary.Core.Manager.DB.ActivityProduct.Format;
using ServiceLibrary.Core.Manager.DB.ActivityProduct.Service;
using ServiceLibrary.Core.Manager.DB.ActivitySponsor.Format;
using ServiceLibrary.Core.Manager.DB.ActivitySponsor.Service;
using ServiceLibrary.Core.Manager.DB.ActivitySurveyAnswerer.Format;
using ServiceLibrary.Core.Manager.DB.ActivitySurveyAnswerer.Service;
using ServiceLibrary.Core.Manager.DB.ActivitySurveyQuestion.Format;
using ServiceLibrary.Core.Manager.DB.ActivitySurveyQuestion.Service;
using ServiceLibrary.Core.Manager.DB.ActivitySurveyQuestionItem.Format;
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
using ServiceLibrary.Core.Manager.DB.ProductSubKind.Format;
using ServiceLibrary.Core.Manager.DB.ProductSubKind.Service;
using ServiceLibrary.ManagerBackSite.Logical.Common.Format;
using ServiceLibrary.ManagerBackSite.Logical.Common.Service;
using ServiceLibrary.ManagerBackSite.Logical.CRM.Activity.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.CRM.Activity.Service;

/// <summary>管理者後台-CRM-活動管理-邏輯服務</summary>
public class MbsCrmActLogicalService : IMbsCrmActLogicalService
{
    /// <summary>logger</summary>
    private readonly ILogger<MbsCrmActLogicalService> _logger;

    #region CommonLibrary

    /// <summary>通用-Excel-服務</summary>
    private readonly ICmnExcelService _cmnExcel;

    /// <summary>通用-資料夾檔案-服務</summary>
    private readonly ICmnFolderFileService _cmnFolderFile;

    #endregion

    #region Core Employee

    /// <summary>核心-員工-商機-資料庫服務</summary>
    private readonly ICoEmpPipelineDbService _coEmpPipelineDb;

    /// <summary>核心-員工-商機窗口-資料庫服務</summary>
    private readonly ICoEmpPipelineContacterDbService _coEmpPipelineContacterDb;

    /// <summary>核心-員工-商機原始-資料庫服務</summary>
    private readonly ICoEmpPipelineOriginalDbService _coEmpPipelineOriginalDb;

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

    /// <summary>核心-管理者-活動問卷回答者-資料庫-服務</summary>
    private readonly ICoMgrActivitySurveyAnswererDbService _coMgrActivitySurveyAnswererDb;

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
    public MbsCrmActLogicalService(
        ILogger<MbsCrmActLogicalService> logger,
        // CommonLibrary
        ICmnExcelService cmnExcel,
        ICmnFolderFileService cmnFolderFile,
        // Core Employee
        ICoEmpPipelineDbService coEmpPipelineDb,
        ICoEmpPipelineContacterDbService coEmpPipelineContacterDb,
        ICoEmpPipelineOriginalDbService coEmpPipelineOriginalDb,
        // Core Manager
        ICoMgrActivityDbService coMgrActivityDb,
        ICoMgrActivityExpenseDbService coMgrActivityExpenseDb,
        ICoMgrActivityProductDbService coMgrActivityProductDb,
        ICoMgrActivitySponsorDbService coMgrActivitySponsorDb,
        ICoMgrActivitySurveyQuestionDbService coMgrActivitySurveyQuestionDb,
        ICoMgrActivitySurveyAnswererDbService coMgrActivitySurveyAnswererDb,
        ICoMgrActivitySurveyQuestionItemDbService coMgrActivitySurveyQuestionItemDb,
        ICoMgrCompanyDbService coMgrCompanyDb,
        ICoMgrCompanyLocationDbService coMgrCompanyLocationDb,
        ICoMgrCompanyMainKindDbService coMgrCompanyMainKindDb,
        ICoMgrCompanySubKindDbService coMgrCompanySubKindDb,
        ICoMgrContacterDbService coMgrContacterDb,
        ICoMgrProductDbService coMgrProductDb,
        ICoMgrProductMainKindDbService coMgrProductMainKindDb,
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
        this._coEmpPipelineDb = coEmpPipelineDb;
        this._coEmpPipelineContacterDb = coEmpPipelineContacterDb;
        this._coEmpPipelineOriginalDb = coEmpPipelineOriginalDb;
        // Core Manager
        this._coMgrActivityDb = coMgrActivityDb;
        this._coMgrActivityExpenseDb = coMgrActivityExpenseDb;
        this._coMgrActivityProductDb = coMgrActivityProductDb;
        this._coMgrActivitySponsorDb = coMgrActivitySponsorDb;
        this._coMgrActivitySurveyQuestionDb = coMgrActivitySurveyQuestionDb;
        this._coMgrActivitySurveyAnswererDb = coMgrActivitySurveyAnswererDb;
        this._coMgrActivitySurveyQuestionItemDb = coMgrActivitySurveyQuestionItemDb;
        this._coMgrCompanyDb = coMgrCompanyDb;
        this._coMgrCompanyLocationDb = coMgrCompanyLocationDb;
        this._coMgrCompanyMainKindDb = coMgrCompanyMainKindDb;
        this._coMgrCompanySubKindDb = coMgrCompanySubKindDb;
        this._coMgrContacterDb = coMgrContacterDb;
        this._coMgrProductDb = coMgrProductDb;
        this._coMgrProductMainKindDb = coMgrProductMainKindDb;
        this._coMgrProductSubKindDb = coMgrProductSubKindDb;
        // Core Logical
        this._coEmployeeLogical = coEmployeeLogical;
        // ManagerBackSite
        this._mbsCommonLogical = mbsCommonLogical;

        // This
        this._atomMenu = DbAtomMenuEnum.CrmActivity;
    }

    #region 活動

    /// <summary>管理者後台-CRM-活動管理-邏輯服務-新增活動</summary>
    public async Task<MbsCrmActLgcAddActivityRspMdl> AddActivityAsync(MbsCrmActLgcAddActivityReqMdl reqObj)
    {
        var rspObj = new MbsCrmActLgcAddActivityRspMdl
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

        // db, 核心-管理者-活動-資料庫-新增
        var coMgrActivityDbAddReqObj = new CoMgrActivityDbAddReqMdl()
        {
            ManagerActivityName = reqObj.ManagerActivityName,
            ManagerActivityKind = reqObj.ManagerAcitivtyKind,
            ManagerActivityStartTime = reqObj.ManagerActivityStartTime,
            ManagerActivityEndTime = reqObj.ManagerActivityEndTime,
            ManagerActivityLocation = reqObj.ManagerActivityLocation,
            ManagerActivityExpectedLeadCount = reqObj.ManagerActivityExpectedLeadCount,
            ManagerActivityContent = reqObj.ManagerActivityContent,
        };
        var coMgrActivityDbAddRspObj = await this._coMgrActivityDb.AddAsync(coMgrActivityDbAddReqObj);
        if (coMgrActivityDbAddRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // 新增管理者活動-產品清單
        if (reqObj.ManagerActivityProductList != null && reqObj.ManagerActivityProductList.Count > 0)
        {
            // db, 核心-管理者-活動產品-資料庫-新增多筆
            var coMgrActivityProductDbAddManyReqObj = new CoMgrActPrdDbAddManyReqMdl()
            {
                ManagerActivityProductList = reqObj.ManagerActivityProductList.Select(x => new CoMgrActPrdDbAddManyReqItemMdl
                {
                    ManagerActivityID = coMgrActivityDbAddRspObj.ManagerActivityID,
                    ManagerProductID = x.ManagerProductID
                }).ToList()
            };
            var coMgrActivityProductDbAddManyRspObj = await this._coMgrActivityProductDb.AddManyAsync(coMgrActivityProductDbAddManyReqObj);
            if (coMgrActivityProductDbAddManyRspObj == default)
            {
                // db, 核心-管理者-活動-資料庫-移除
                var coMgrActivityDbRemoveReqObj = new CoMgrActivityDbRemoveReqMdl()
                {
                    ManagerActivityID = coMgrActivityDbAddRspObj.ManagerActivityID
                };
                var coMgrActivityDbRemoveRspObj = await this._coMgrActivityDb.RemoveAsync(coMgrActivityDbRemoveReqObj);
                if (coMgrActivityDbRemoveRspObj == default)
                {
                    this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                    return rspObj;
                }

                this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                return rspObj;
            }
        }

        // 新增管理者活動-贊助清單
        if (reqObj.ManagerActivitySponsorList != null && reqObj.ManagerActivitySponsorList.Count > 0)
        {
            // db, 核心-管理者-活動贊助商-資料庫-新增多筆
            var coMgrActivitySponsorDbAddManyReqObj = new CoMgrActSpsDbAddManyReqMdl()
            {
                ManagerActivitySponsorList = reqObj.ManagerActivitySponsorList.Select(x => new CoMgrActSpsDbAddManyReqItemMdl
                {
                    ManagerActivityID = coMgrActivityDbAddRspObj.ManagerActivityID,
                    ManagerActivitySponsorName = x.ManagerActivitySponsorName,
                    ManagerActivitySponsorAmount = x.ManagerActivitySponsorAmount
                }).ToList()
            };
            var coMgrActivitySponsorDbAddManyRspObj = await this._coMgrActivitySponsorDb.AddManyAsync(coMgrActivitySponsorDbAddManyReqObj);
            if (coMgrActivitySponsorDbAddManyRspObj == default)
            {
                // db, 核心-管理者-活動-資料庫-移除
                var coMgrActivityDbRemoveReqObj = new CoMgrActivityDbRemoveReqMdl()
                {
                    ManagerActivityID = coMgrActivityDbAddRspObj.ManagerActivityID
                };
                var coMgrActivityDbRemoveRspObj = await this._coMgrActivityDb.RemoveAsync(coMgrActivityDbRemoveReqObj);
                if (coMgrActivityDbRemoveRspObj == default)
                {
                    this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                    return rspObj;
                }

                this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                return rspObj;
            }
        }

        // 新增管理者活動-支出清單
        if (reqObj.ManagerActivityExpenseList != null && reqObj.ManagerActivityExpenseList.Count > 0)
        {
            // db, 核心-管理者-活動支出-資料庫-新增多筆
            var coMgrActivityExpenseDbAddManyReqObj = new CoMgrActExpDbAddManyReqMdl()
            {
                ManagerActivityExpenseList = reqObj.ManagerActivityExpenseList.Select(x => new CoMgrActExpDbAddManyReqItemMdl
                {
                    ManagerActivityID = coMgrActivityDbAddRspObj.ManagerActivityID,
                    ManagerActivityExpenseItem = x.ManagerActivityExpenseItem,
                    ManagerActivityExpenseQuantity = x.ManagerActivityExpenseQuantity,
                    ManagerActivityExpenseTotalAmount = x.ManagerActivityExpenseTotalAmount,
                }).ToList()
            };
            var coMgrActivityExpenseDbAddManyRspObj = await this._coMgrActivityExpenseDb.AddManyAsync(coMgrActivityExpenseDbAddManyReqObj);
            if (coMgrActivityExpenseDbAddManyRspObj == default)
            {
                // db, 核心-管理者-活動-資料庫-移除
                var coMgrActivityDbRemoveReqObj = new CoMgrActivityDbRemoveReqMdl()
                {
                    ManagerActivityID = coMgrActivityDbAddRspObj.ManagerActivityID
                };
                var coMgrActivityDbRemoveRspObj = await this._coMgrActivityDb.RemoveAsync(coMgrActivityDbRemoveReqObj);
                if (coMgrActivityDbRemoveRspObj == default)
                {
                    this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                    return rspObj;
                }

                this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                return rspObj;
            }
        }

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        rspObj.ManagerActivityID = coMgrActivityDbAddRspObj.ManagerActivityID;
        return rspObj;
    }

    /// <summary>管理者後台-CRM-活動管理-邏輯服務-取得活動</summary>
    public async Task<MbsCrmActLgcGetActivityRspMdl> GetActivityAsync(MbsCrmActLgcGetActivityReqMdl reqObj)
    {
        var rspObj = new MbsCrmActLgcGetActivityRspMdl
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

        #region 產品

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
            select new MbsCrmActLgcGetActivityProductRspItemMdl
            {
                ManagerActivityProductID = actp.ManagerActivityProductID,
                ManagerProductID = actp.ManagerProductID,
                ManagerProductName = prd?.ManagerProductName,
                ManagerProductMainKindID = prdm?.ManagerProductMainKindID ?? 0,
                ManagerProductMainKindName = prdm?.ManagerProductMainKindName,
                ManagerProductSubKindID = prds?.ManagerProductSubKindID ?? 0,
                ManagerProductSubKindName = prds?.ManagerProductSubKindName,
            }
        ).ToList();
        rspObj.ManagerActivitySponsorList = coMgrActSpsDbGetManyRspObj.ManagerActivitySponsorList
            .Select(x => new MbsCrmActLgcGetActivitySponsorRspItemMdl
            {
                ManagerActivitySponsorID = x.ManagerActivitySponsorID,
                ManagerActivitySponsorName = x.ManagerActivitySponsorName,
                ManagerActivitySponsorAmount = x.ManagerActivitySponsorAmount
            })
            .ToList();
        rspObj.ManagerActivityExpenseList = coMgrActExpDbGetManyRspObj.ManagerActivityExpenseList
           .Select(x => new MbsCrmActLgcGetActivityExpenseRspItemMdl
           {
               ManagerActivityExpenseID = x.ManagerActivityExpenseID,
               ManagerActivityExpenseItem = x.ManagerActivityExpenseItem,
               ManagerActivityExpenseQuantity = x.ManagerActivityExpenseQuantity,
               ManagerActivityExpenseTotalAmount = x.ManagerActivityExpenseTotalAmount
           })
           .ToList();
        return rspObj;
    }

    /// <summary>管理者後台-CRM-活動管理-邏輯服務-取得多筆活動</summary>
    public async Task<MbsCrmActLgcGetManyActivityRspMdl> GetManyActivityAsync(MbsCrmActLgcGetManyActivityReqMdl reqObj)
    {
        var rspObj = new MbsCrmActLgcGetManyActivityRspMdl
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

        // db, 核心-管理者-活動-資料庫-取得[筆數]從[管理者後台-CRM-活動管理]
        var coMgrActivityDbGetCountFromMbsCrmActReqObj = new CoMgrActivityDbGetCountFromMbsCrmActReqMdl()
        {
            ManagerActivityStartTime = reqObj.ManagerActivityStartTime,
            ManagerActivityEndTime = reqObj.ManagerActivityEndTime,
            ManagerActivityKind = reqObj.ManagerActivityKind,
            ManagerActivityName = reqObj.ManagerActivityName,
        };
        var coMgrActivityDbGetCountFromMbsCrmActRspObj = await this._coMgrActivityDb.GetCountFromMbsCrmActAsync(coMgrActivityDbGetCountFromMbsCrmActReqObj);
        if (coMgrActivityDbGetCountFromMbsCrmActRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");

            return rspObj;
        }

        // 判斷筆數是否為0
        if (coMgrActivityDbGetCountFromMbsCrmActRspObj.Count == 0)
        {
            rspObj.ErrorCode = MbsErrorCodeEnum.Success;
            rspObj.ManagerActivityList = new();
            rspObj.TotalCount = 0;
            return rspObj;
        }

        // db, 核心-管理者-活動-資料庫-取得多筆從[管理者後台-CRM-活動管理]
        var coMgrActivityDbGetManyFromMbsCrmActReqObj = new CoMgrActivityDbGetManyFromMbsCrmActReqMdl()
        {
            ManagerActivityStartTime = reqObj.ManagerActivityStartTime,
            ManagerActivityEndTime = reqObj.ManagerActivityEndTime,
            ManagerActivityKind = reqObj.ManagerActivityKind,
            ManagerActivityName = reqObj.ManagerActivityName,
            PageIndex = reqObj.PageIndex,
            PageSize = reqObj.PageSize
        };
        var coMgrActivityDbGetManyFromMbsCrmActRspObj = await this._coMgrActivityDb.GetManyFromMbsCrmActAsync(coMgrActivityDbGetManyFromMbsCrmActReqObj);
        if (coMgrActivityDbGetManyFromMbsCrmActRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");

            return rspObj;
        }

        // db, 核心-管理者-活動贊助商-資料庫-取得多筆總贊助金額從[管理者後台-CRM-活動管理]
        var coMgrActSpsDbGetManyTotalSponsorAmountReqObj = new CoMgrActSpsDbGetManyTotalSponsorAmountReqMdl()
        {
            ManagerActivityIDList = coMgrActivityDbGetManyFromMbsCrmActRspObj.ManagerActivityList
                                                                                    .Select(x => x.ManagerActivityID)
                                                                                    .Distinct()
                                                                                    .ToList()
        };
        var coMgrActSpsDbGetManyTotalSponsorAmountRspObj = await this._coMgrActivitySponsorDb.GetManyTotalSponsorAmountFromMbsMktActAsync(coMgrActSpsDbGetManyTotalSponsorAmountReqObj);
        if (coMgrActSpsDbGetManyTotalSponsorAmountRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");

            return rspObj;
        }

        // db, 核心-管理者-活動支出-資料庫-取得多筆總支出額從[管理者後台-CRM-活動管理]
        var coMgrActExpDbGetManyTotalExpenseReqObj = new CoMgrActExpDbGetManyTotalExpenseReqMdl()
        {
            ManagerActivityIDList = coMgrActivityDbGetManyFromMbsCrmActRspObj.ManagerActivityList
                                                                                    .Select(x => x.ManagerActivityID)
                                                                                    .Distinct()
                                                                                    .ToList()
        };
        var coMgrActExpDbGetManyTotalExpenseRspObj = await this._coMgrActivityExpenseDb.GetManyTotalExpenseAsync(coMgrActExpDbGetManyTotalExpenseReqObj);
        if (coMgrActExpDbGetManyTotalExpenseRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // 實際名單數
        // db, 核心-員工-商機-資料庫-取得多筆[筆數]
        var coEmpPplDbGetManyCountReqObj = new CoEmpPplDbGetManyCountReqMdl()
        {
            ManagerActivityIDList = coMgrActivityDbGetManyFromMbsCrmActRspObj.ManagerActivityList
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
            ManagerActivityIDList = coMgrActivityDbGetManyFromMbsCrmActRspObj.ManagerActivityList
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

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        rspObj.ManagerActivityList =
        (
            from act in coMgrActivityDbGetManyFromMbsCrmActRspObj.ManagerActivityList
            from sps in coMgrActSpsDbGetManyTotalSponsorAmountRspObj.ManagerActivitySponsorList
                                .Where(sps => sps.ManagerActivityID == act.ManagerActivityID)
                                .DefaultIfEmpty()
            from exp in coMgrActExpDbGetManyTotalExpenseRspObj.ManagerActivityExpenseList
                                .Where(exp => exp.ManagerActivityID == act.ManagerActivityID)
                                .DefaultIfEmpty()
            from totalPipelineCount in coEmpPplDbGetManyCountRspObj.EmployeePipelineList
                                .Where(count => count.ManagerActivityID == act.ManagerActivityID)
                                .DefaultIfEmpty()
            from toSalesCount in coEmpPplDbGetManyCountByTransferredToSalesRspObj.EmployeePipelineList
                                .Where(count => count.ManagerActivityID == act.ManagerActivityID)
                                .DefaultIfEmpty()
            select new MbsCrmActLgcGetManyActivityRspItemMdl
            {
                ManagerActivityID = act.ManagerActivityID,
                ManagerActivityName = act.ManagerActivityName,
                ManagerActivityKind = act.ManagerActivityKind,
                ManagerActivityStartTime = act.ManagerActivityStartTime,
                ManagerActivityEndTime = act.ManagerActivityEndTime,
                ManagerActivityLocation = act.ManagerActivityLocation,
                ManagerActivityExpectedLeadCount = act.ManagerActivityExpectedLeadCount,
                ManagerActivityRegisteredCount = totalPipelineCount?.Count ?? 0,
                ManagerActivityTransferredToSalesCount = toSalesCount?.Count ?? 0,
                ManagerActivityEmployeePipelineCount = act.ManagerActivityEmployeePipelineCount,
                ManagerActivitySponsorTotalSponsorAmount = sps?.ManagerActivitySponsorTotalSponsorAmount ?? 0,
                ManagerActivityExpenseTotalExpenseAmount = exp?.ManagerActivityExpenseTotalExpenseAmount ?? 0
            }
        )
        .ToList();
        rspObj.TotalCount = coMgrActivityDbGetCountFromMbsCrmActRspObj.Count;
        return rspObj;
    }

    /// <summary>管理者後台-CRM-活動管理-邏輯服務-更新活動</summary>
    public async Task<MbsCrmActLgcUpdateActivityRspMdl> UpdateActivityAsync(MbsCrmActLgcUpdateActivityReqMdl reqObj)
    {
        var rspObj = new MbsCrmActLgcUpdateActivityRspMdl
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

        // 判斷活動時間,活動開始不能編輯
        if (reqObj.ManagerActivityStartTime < DateTimeOffset.UtcNow)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");

            // 活動已開始，無法編輯
            rspObj.ErrorCode = MbsErrorCodeEnum.Fail;
            return rspObj;
        }

        // db, 核心-管理者-活動-資料庫-更新
        var coMgrActivityDbUpdateReqObj = new CoMgrActivityDbUpdateReqMdl()
        {
            ManagerActivityID = reqObj.ManagerActivityID,
            ManagerActivityName = reqObj.ManagerActivityName,
            ManagerActivityStartTime = reqObj.ManagerActivityStartTime,
            ManagerActivityEndTime = reqObj.ManagerActivityEndTime,
            ManagerActivityLocation = reqObj.ManagerActivityLocation,
            ManagerActivityExpectedLeadCount = reqObj.ManagerActivityExpectedLeadCount,
            ManagerActivityContent = reqObj.ManagerActivityContent,
        };
        var coMgrActivityDbUpdateRspObj = await this._coMgrActivityDb.UpdateAsync(coMgrActivityDbUpdateReqObj);
        if (coMgrActivityDbUpdateRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        return rspObj;
    }

    #endregion

    #region 活動產品

    /// <summary>管理者後台-CRM-活動管理-邏輯服務-新增活動產品</summary>
    public async Task<MbsCrmActLgcAddActivityProductRspMdl> AddActivityProductAsync(MbsCrmActLgcAddActivityProductReqMdl reqObj)
    {
        var rspObj = new MbsCrmActLgcAddActivityProductRspMdl
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

        // db, 核心-管理者-活動-資料庫-是否存在
        var coMgrActivityDbExistReqObj = new CoMgrActivityDbExistReqMdl()
        {
            ManagerActivityID = reqObj.ManagerActivityID
        };
        var coMgrActivityDbExistRspObj = await this._coMgrActivityDb.ExistAsync(coMgrActivityDbExistReqObj);
        if (coMgrActivityDbExistRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }
        if (coMgrActivityDbExistRspObj.IsExist == false)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // db, 核心-管理者-產品-資料庫-是否存在
        var coMgrPrdDbExistReqObj = new CoMgrPrdDbExistReqMdl()
        {
            ManagerProductID = reqObj.ManagerProductID
        };
        var coMgrPrdDbExistRspObj = await this._coMgrProductDb.ExistAsync(coMgrPrdDbExistReqObj);
        if (coMgrPrdDbExistRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }
        if (coMgrPrdDbExistRspObj.IsExist == false)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // db, 核心-管理者-活動產品-資料庫-是否存在
        var coMgrActPrdDbExistReqObj = new CoMgrActPrdDbExistReqMdl()
        {
            ManagerActivityID = reqObj.ManagerActivityID,
            ManagerProductID = reqObj.ManagerProductID
        };
        var coMgrActPrdDbExistRspObj = await this._coMgrActivityProductDb.ExistAsync(coMgrActPrdDbExistReqObj);
        if (coMgrActPrdDbExistRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }
        if (coMgrActPrdDbExistRspObj.IsExist)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            rspObj.ErrorCode = MbsErrorCodeEnum.DataAlreadyExists;
            return rspObj;
        }

        // db, 核心-管理者-活動產品-資料庫-新增
        var coMgrActPrdDbAddReqObj = new CoMgrActPrdDbAddReqMdl()
        {
            ManagerActivityID = reqObj.ManagerActivityID,
            ManagerProductID = reqObj.ManagerProductID,
        };
        var coMgrActPrdDbAddRspObj = await this._coMgrActivityProductDb.AddAsync(coMgrActPrdDbAddReqObj);
        if (coMgrActPrdDbAddRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        rspObj.ManagerActivityProductID = coMgrActPrdDbAddRspObj.ManagerActivityProductID;
        return rspObj;
    }

    /// <summary>管理者後台-CRM-活動管理-邏輯服務-更新活動產品</summary>
    public async Task<MbsCrmActLgcUpdateActivityProductRspMdl> UpdateActivityProductAsync(MbsCrmActLgcUpdateActivityProductReqMdl reqObj)
    {
        var rspObj = new MbsCrmActLgcUpdateActivityProductRspMdl
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

        // db, 核心-管理者-產品-資料庫-是否存在
        var coMgrPrdDbExistReqObj = new CoMgrPrdDbExistReqMdl()
        {
            ManagerProductID = reqObj.ManagerProductID
        };
        var coMgrPrdDbExistRspObj = await this._coMgrProductDb.ExistAsync(coMgrPrdDbExistReqObj);
        if (coMgrPrdDbExistRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }
        if (coMgrPrdDbExistRspObj.IsExist == false)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // db, 核心-管理者-活動產品-資料庫-取得
        var coMgrActPrdDbGetReqObj = new CoMgrActPrdDbGetReqMdl()
        {
            ManagerActivityProductID = reqObj.ManagerActivityProductID
        };
        var coMgrActPrdDbGetRspObj = await this._coMgrActivityProductDb.GetAsync(coMgrActPrdDbGetReqObj);
        if (coMgrActPrdDbGetRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // 檢查資料是否重複
        if (reqObj.ManagerProductID.HasValue &&
            coMgrActPrdDbGetRspObj.ManagerProductID != reqObj.ManagerProductID.Value)
        {
            // db, 核心-管理者-活動產品-資料庫-是否存在
            var coMgrActPrdDbExistReqObj = new CoMgrActPrdDbExistReqMdl()
            {
                ManagerActivityID = coMgrActPrdDbGetRspObj.ManagerActivityID,
                ManagerProductID = reqObj.ManagerProductID.Value
            };
            var coMgrActPrdDbExistRspObj = await this._coMgrActivityProductDb.ExistAsync(coMgrActPrdDbExistReqObj);
            if (coMgrActPrdDbExistRspObj == default)
            {
                this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                return rspObj;
            }
            if (coMgrActPrdDbExistRspObj.IsExist)
            {
                this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                rspObj.ErrorCode = MbsErrorCodeEnum.DataAlreadyExists;
                return rspObj;
            }
        }

        // db, 核心-管理者-活動產品-資料庫-更新
        var coMgrActPrdDbUpdateReqObj = new CoMgrActPrdDbUpdateReqMdl()
        {
            ManagerActivityProductID = reqObj.ManagerActivityProductID,
            ManagerProductID = reqObj.ManagerProductID,
        };
        var coMgrActPrdDbUpdateRspObj = await this._coMgrActivityProductDb.UpdateAsync(coMgrActPrdDbUpdateReqObj);
        if (coMgrActPrdDbUpdateRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        return rspObj;
    }

    /// <summary>管理者後台-CRM-活動管理-邏輯服務-刪除活動產品</summary>
    public async Task<MbsCrmActLgcRemoveActivityProductRspMdl> RemoveActivityProductAsync(MbsCrmActLgcRemoveActivityProductReqMdl reqObj)
    {
        var rspObj = new MbsCrmActLgcRemoveActivityProductRspMdl
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

        // db, 核心-管理者-活動產品-資料庫-移除
        var coMgrActPrdDbRemoveReqObj = new CoMgrActPrdDbRemoveReqMdl()
        {
            ManagerActivityProductID = reqObj.ManagerActivityProductID,
        };
        var coMgrActPrdDbRemoveRspObj = await this._coMgrActivityProductDb.RemoveAsync(coMgrActPrdDbRemoveReqObj);
        if (coMgrActPrdDbRemoveRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        return rspObj;
    }

    /// <summary>管理者後台-CRM-活動管理-邏輯服務-取得活動產品</summary>
    public async Task<MbsCrmActLgcGetActivityProductRspMdl> GetActivityProductAsync(MbsCrmActLgcGetActivityProductReqMdl reqObj)
    {
        var rspObj = new MbsCrmActLgcGetActivityProductRspMdl
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

        // db, 核心-管理者-活動產品-資料庫-取得
        var coMgrActPrdDbGetReqObj = new CoMgrActPrdDbGetReqMdl()
        {
            ManagerActivityProductID = reqObj.ManagerActivityProductID,
        };
        var coMgrActPrdDbGetRspObj = await this._coMgrActivityProductDb.GetAsync(coMgrActPrdDbGetReqObj);
        if (coMgrActPrdDbGetRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // db, 核心-管理者-產品-資料庫-取得
        var coMgrPrdDbGetReqObj = new CoMgrPrdDbGetReqMdl()
        {
            ManagerProductID = coMgrActPrdDbGetRspObj.ManagerProductID,
        };
        var coMgrPrdDbGetRspObj = await this._coMgrProductDb.GetAsync(coMgrPrdDbGetReqObj);
        if (coMgrPrdDbGetRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        rspObj.ManagerActivityProductID = coMgrActPrdDbGetRspObj.ManagerActivityProductID;
        rspObj.ManagerProductID = coMgrActPrdDbGetRspObj.ManagerProductID;
        rspObj.ManagerProductMainKindID = coMgrPrdDbGetRspObj.ManagerProductMainKindID;
        rspObj.ManagerProductSubKindID = coMgrPrdDbGetRspObj.ManagerProductSubKindID;
        return rspObj;
    }

    /// <summary>管理者後台-CRM-活動管理-邏輯服務-取得多筆活動產品</summary>
    public async Task<MbsCrmActLgcGetManyActivityProductRspMdl> GetManyActivityProductAsync(MbsCrmActLgcGetManyActivityProductReqMdl reqObj)
    {
        var rspObj = new MbsCrmActLgcGetManyActivityProductRspMdl
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

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
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
            select new MbsCrmActLgcGetManyActivityProductRspItemMdl
            {
                ManagerActivityProductID = actp.ManagerActivityProductID,
                ManagerProductName = prd?.ManagerProductName,
                ManagerProductMainKindName = prdm?.ManagerProductMainKindName,
                ManagerProductSubKindName = prds?.ManagerProductSubKindName,
            }
        ).ToList();
        return rspObj;
    }

    #endregion

    #region 活動贊助

    /// <summary>管理者後台-CRM-活動管理-邏輯服務-新增活動贊助</summary>
    public async Task<MbsCrmActLgcAddActivitySponsorRspMdl> AddActivitySponsorAsync(MbsCrmActLgcAddActivitySponsorReqMdl reqObj)
    {
        var rspObj = new MbsCrmActLgcAddActivitySponsorRspMdl
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

        // db, 核心-管理者-活動-資料庫-是否存在
        var coMgrActivityDbExistReqObj = new CoMgrActivityDbExistReqMdl()
        {
            ManagerActivityID = reqObj.ManagerActivityID
        };
        var coMgrActivityDbExistRspObj = await this._coMgrActivityDb.ExistAsync(coMgrActivityDbExistReqObj);
        if (coMgrActivityDbExistRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }
        if (coMgrActivityDbExistRspObj.IsExist == false)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // db, 核心-管理者-活動贊助-資料庫-是否存在
        var coMgrActSpsDbExistReqObj = new CoMgrActSpsDbExistReqMdl()
        {
            ManagerActivityID = reqObj.ManagerActivityID,
            ManagerActivitySponsorName = reqObj.ManagerActivitySponsorName
        };
        var coMgrActSpsDbExistRspObj = await this._coMgrActivitySponsorDb.ExistAsync(coMgrActSpsDbExistReqObj);
        if (coMgrActSpsDbExistRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }
        if (coMgrActSpsDbExistRspObj.IsExist)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            rspObj.ErrorCode = MbsErrorCodeEnum.DataAlreadyExists;
            return rspObj;
        }

        // db, 核心-管理者-活動贊助-資料庫-新增
        var coMgrActSpsDbAddReqObj = new CoMgrActSpsDbAddReqMdl()
        {
            ManagerActivityID = reqObj.ManagerActivityID,
            ManagerActivitySponsorName = reqObj.ManagerActivitySponsorName,
            ManagerActivitySponsorAmount = reqObj.ManagerActivitySponsorAmount,
        };
        var coMgrActSpsDbAddRspObj = await this._coMgrActivitySponsorDb.AddAsync(coMgrActSpsDbAddReqObj);
        if (coMgrActSpsDbAddRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        rspObj.ManagerActivitySponsorID = coMgrActSpsDbAddRspObj.ManagerActivitySponsorID;
        return rspObj;
    }

    /// <summary>管理者後台-CRM-活動管理-邏輯服務-更新活動贊助</summary>
    public async Task<MbsCrmActLgcUpdateActivitySponsorRspMdl> UpdateActivitySponsorAsync(MbsCrmActLgcUpdateActivitySponsorReqMdl reqObj)
    {
        var rspObj = new MbsCrmActLgcUpdateActivitySponsorRspMdl
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

        // db, 核心-管理者-活動贊助-資料庫-取得
        var coMgrActSpsDbGetReqObj = new CoMgrActSpsDbGetReqMdl()
        {
            ManagerActivitySponsorID = reqObj.ManagerActivitySponsorID
        };
        var coMgrActSpsDbGetRspObj = await this._coMgrActivitySponsorDb.GetAsync(coMgrActSpsDbGetReqObj);
        if (coMgrActSpsDbGetRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // 檢查資料是否重複
        if (!string.IsNullOrEmpty(reqObj.ManagerActivitySponsorName) &&
            reqObj.ManagerActivitySponsorName != coMgrActSpsDbGetRspObj.ManagerActivitySponsorName)
        {
            // db, 核心-管理者-活動贊助-資料庫-是否存在
            var coMgrActSpsDbExistReqObj = new CoMgrActSpsDbExistReqMdl()
            {
                ManagerActivityID = coMgrActSpsDbGetRspObj.ManagerActivityID,
                ManagerActivitySponsorName = reqObj.ManagerActivitySponsorName
            };
            var coMgrActSpsDbExistRspObj = await this._coMgrActivitySponsorDb.ExistAsync(coMgrActSpsDbExistReqObj);
            if (coMgrActSpsDbExistRspObj == default)
            {
                this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                return rspObj;
            }
            if (coMgrActSpsDbExistRspObj.IsExist)
            {
                this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                rspObj.ErrorCode = MbsErrorCodeEnum.DataAlreadyExists;
                return rspObj;
            }
        }

        // db, 核心-管理者-活動贊助-資料庫-更新
        var coMgrActSpsDbUpdateReqObj = new CoMgrActSpsDbUpdateReqMdl()
        {
            ManagerActivitySponsorID = reqObj.ManagerActivitySponsorID,
            ManagerActivitySponsorName = reqObj.ManagerActivitySponsorName,
            ManagerActivitySponsorAmount = reqObj.ManagerActivitySponsorAmount,
        };
        var coMgrActSpsDbUpdateRspObj = await this._coMgrActivitySponsorDb.UpdateAsync(coMgrActSpsDbUpdateReqObj);
        if (coMgrActSpsDbUpdateRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        return rspObj;
    }

    /// <summary>管理者後台-CRM-活動管理-邏輯服務-刪除活動贊助</summary>
    public async Task<MbsCrmActLgcRemoveActivitySponsorRspMdl> RemoveActivitySponsorAsync(MbsCrmActLgcRemoveActivitySponsorReqMdl reqObj)
    {
        var rspObj = new MbsCrmActLgcRemoveActivitySponsorRspMdl
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

        // db, 核心-管理者-活動贊助-資料庫-移除
        var coMgrActSpsDbRemoveReqObj = new CoMgrActSpsDbRemoveReqMdl()
        {
            ManagerActivitySponsorID = reqObj.ManagerActivitySponsorID,
        };
        var coMgrActSpsDbRemoveRspObj = await this._coMgrActivitySponsorDb.RemoveAsync(coMgrActSpsDbRemoveReqObj);
        if (coMgrActSpsDbRemoveRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        return rspObj;
    }

    /// <summary>管理者後台-CRM-活動管理-邏輯服務-取得活動贊助</summary>
    public async Task<MbsCrmActLgcGetActivitySponsorRspMdl> GetActivitySponsorAsync(MbsCrmActLgcGetActivitySponsorReqMdl reqObj)
    {
        var rspObj = new MbsCrmActLgcGetActivitySponsorRspMdl
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

        // db, 核心-管理者-活動贊助-資料庫-取得
        var coMgrActSpsDbGetReqObj = new CoMgrActSpsDbGetReqMdl()
        {
            ManagerActivitySponsorID = reqObj.ManagerActivitySponsorID,
        };
        var coMgrActSpsDbGetRspObj = await this._coMgrActivitySponsorDb.GetAsync(coMgrActSpsDbGetReqObj);
        if (coMgrActSpsDbGetRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        rspObj.ManagerActivitySponsorName = coMgrActSpsDbGetRspObj.ManagerActivitySponsorName;
        rspObj.ManagerActivitySponsorAmount = coMgrActSpsDbGetRspObj.ManagerActivitySponsorAmount;
        return rspObj;
    }

    /// <summary>管理者後台-CRM-活動管理-邏輯服務-取得多筆活動贊助</summary>
    public async Task<MbsCrmActLgcGetManyActivitySponsorRspMdl> GetManyActivitySponsorAsync(MbsCrmActLgcGetManyActivitySponsorReqMdl reqObj)
    {
        var rspObj = new MbsCrmActLgcGetManyActivitySponsorRspMdl
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

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        rspObj.ManagerActivitySponsorList = coMgrActSpsDbGetManyRspObj.ManagerActivitySponsorList
            .Select(x => new MbsCrmActLgcGetManyActivitySponsorRspItemMdl
            {
                ManagerActivitySponsorID = x.ManagerActivitySponsorID,
                ManagerActivitySponsorName = x.ManagerActivitySponsorName,
                ManagerActivitySponsorAmount = x.ManagerActivitySponsorAmount
            })
            .ToList();
        return rspObj;
    }

    #endregion

    #region 活動支出

    /// <summary>管理者後台-CRM-活動管理-邏輯服務-新增活動支出</summary>
    public async Task<MbsCrmActLgcAddActivityExpenseRspMdl> AddActivityExpenseAsync(MbsCrmActLgcAddActivityExpenseReqMdl reqObj)
    {
        var rspObj = new MbsCrmActLgcAddActivityExpenseRspMdl
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

        // db, 核心-管理者-活動支出-資料庫-是否存在
        var coMgrActExpDbExistReqObj = new CoMgrActExpDbExistReqMdl()
        {
            ManagerActivityID = reqObj.ManagerActivityID,
            ManagerActivityExpenseItem = reqObj.ManagerActivityExpenseItem.Trim()
        };
        var coMgrActExpDbExistRspObj = await this._coMgrActivityExpenseDb.ExistAsync(coMgrActExpDbExistReqObj);
        if (coMgrActExpDbExistRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }
        if (coMgrActExpDbExistRspObj.IsExist)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            rspObj.ErrorCode = MbsErrorCodeEnum.DataAlreadyExists;
            return rspObj;
        }

        // db, 核心-管理者-活動支出-資料庫-新增
        var coMgrActExpDbAddReqObj = new CoMgrActExpDbAddReqMdl()
        {
            ManagerActivityID = reqObj.ManagerActivityID,
            ManagerActivityExpenseItem = reqObj.ManagerActivityExpenseItem,
            ManagerActivityExpenseQuantity = reqObj.ManagerActivityExpenseQuantity,
            ManagerActivityExpenseTotalAmount = reqObj.ManagerActivityExpenseTotalAmount,
        };
        var coMgrActExpDbAddRspObj = await this._coMgrActivityExpenseDb.AddAsync(coMgrActExpDbAddReqObj);
        if (coMgrActExpDbAddRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        rspObj.ManagerActivityExpenseID = coMgrActExpDbAddRspObj.ManagerActivityExpenseID;
        return rspObj;
    }

    /// <summary>管理者後台-CRM-活動管理-邏輯服務-更新活動支出</summary>
    public async Task<MbsCrmActLgcUpdateActivityExpenseRspMdl> UpdateActivityExpenseAsync(MbsCrmActLgcUpdateActivityExpenseReqMdl reqObj)
    {
        var rspObj = new MbsCrmActLgcUpdateActivityExpenseRspMdl
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

        // db, 核心-管理者-活動支出-資料庫-取得
        var coMgrActExpDbGetReqObj = new CoMgrActExpDbGetReqMdl()
        {
            ManagerActivityExpenseID = reqObj.ManagerActivityExpenseID
        };
        var coMgrActExpDbGetRspObj = await this._coMgrActivityExpenseDb.GetAsync(coMgrActExpDbGetReqObj);
        if (coMgrActExpDbGetRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // 檢查資料是否重複
        if (!string.IsNullOrEmpty(reqObj.ManagerActivityExpenseItem) &&
            reqObj.ManagerActivityExpenseItem.Trim() != coMgrActExpDbGetRspObj.ManagerActivityExpenseItem.Trim())
        {
            // db, 核心-管理者-活動支出-資料庫-是否存在
            var coMgrActExpDbExistReqObj = new CoMgrActExpDbExistReqMdl()
            {
                ManagerActivityID = coMgrActExpDbGetRspObj.ManagerActivityID,
                ManagerActivityExpenseItem = reqObj.ManagerActivityExpenseItem.Trim()
            };
            var coMgrActExpDbExistRspObj = await this._coMgrActivityExpenseDb.ExistAsync(coMgrActExpDbExistReqObj);
            if (coMgrActExpDbExistRspObj == default)
            {
                this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                return rspObj;
            }
            if (coMgrActExpDbExistRspObj.IsExist)
            {
                this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                rspObj.ErrorCode = MbsErrorCodeEnum.DataAlreadyExists;
                return rspObj;
            }
        }

        // db, 核心-管理者-活動支出-資料庫-更新
        var coMgrActExpDbUpdateReqObj = new CoMgrActExpDbUpdateReqMdl()
        {
            ManagerActivityExpenseID = reqObj.ManagerActivityExpenseID,
            ManagerActivityExpenseItem = reqObj.ManagerActivityExpenseItem,
            ManagerActivityExpenseQuantity = reqObj.ManagerActivityExpenseQuantity,
            ManagerActivityExpenseTotalAmount = reqObj.ManagerActivityExpenseTotalAmount,
        };
        var coMgrActExpDbUpdateRspObj = await this._coMgrActivityExpenseDb.UpdateAsync(coMgrActExpDbUpdateReqObj);
        if (coMgrActExpDbUpdateRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        return rspObj;
    }

    /// <summary>管理者後台-CRM-活動管理-邏輯服務-刪除活動支出</summary>
    public async Task<MbsCrmActLgcRemoveActivityExpenseRspMdl> RemoveActivityExpenseAsync(MbsCrmActLgcRemoveActivityExpenseReqMdl reqObj)
    {
        var rspObj = new MbsCrmActLgcRemoveActivityExpenseRspMdl
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

        // db, 核心-管理者-活動支出-資料庫-移除
        var coMgrActExpDbRemoveReqObj = new CoMgrActExpDbRemoveReqMdl()
        {
            ManagerActivityExpenseID = reqObj.ManagerActivityExpenseID,
        };
        var coMgrActExpDbRemoveRspObj = await this._coMgrActivityExpenseDb.RemoveAsync(coMgrActExpDbRemoveReqObj);
        if (coMgrActExpDbRemoveRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        return rspObj;
    }

    /// <summary>管理者後台-CRM-活動管理-邏輯服務-取得活動支出</summary>
    public async Task<MbsCrmActLgcGetActivityExpenseRspMdl> GetActivityExpenseAsync(MbsCrmActLgcGetActivityExpenseReqMdl reqObj)
    {
        var rspObj = new MbsCrmActLgcGetActivityExpenseRspMdl
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

        // db, 核心-管理者-活動支出-資料庫-取得
        var coMgrActExpDbGetReqObj = new CoMgrActExpDbGetReqMdl()
        {
            ManagerActivityExpenseID = reqObj.ManagerActivityExpenseID,
        };
        var coMgrActExpDbGetRspObj = await this._coMgrActivityExpenseDb.GetAsync(coMgrActExpDbGetReqObj);
        if (coMgrActExpDbGetRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        rspObj.ManagerActivityExpenseID = coMgrActExpDbGetRspObj.ManagerActivityExpenseID;
        rspObj.ManagerActivityExpenseItem = coMgrActExpDbGetRspObj.ManagerActivityExpenseItem;
        rspObj.ManagerActivityExpenseQuantity = coMgrActExpDbGetRspObj.ManagerActivityExpenseQuantity;
        rspObj.ManagerActivityExpenseTotalAmount = coMgrActExpDbGetRspObj.ManagerActivityExpenseTotalAmount;
        return rspObj;
    }

    /// <summary>管理者後台-CRM-活動管理-邏輯服務-取得多筆活動支出</summary>
    public async Task<MbsCrmActLgcGetManyActivityExpenseRspMdl> GetManyActivityExpenseAsync(MbsCrmActLgcGetManyActivityExpenseReqMdl reqObj)
    {
        var rspObj = new MbsCrmActLgcGetManyActivityExpenseRspMdl
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

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        rspObj.ManagerActivityExpenseList = coMgrActExpDbGetManyRspObj.ManagerActivityExpenseList
            .Select(x => new MbsCrmActLgcGetManyActivityExpenseRspItemMdl
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

    /// <summary>管理者後台-CRM-活動管理-邏輯服務-取得多筆活動名單</summary>
    public async Task<MbsCrmActLgcGetManyActivityEmployeePipelineRspMdl> GetManyActivityEmployeePipelineAsync(MbsCrmActLgcGetManyActivityEmployeePipelineReqMdl reqObj)
    {
        var rspObj = new MbsCrmActLgcGetManyActivityEmployeePipelineRspMdl
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

        // db, 核心-員工-商機原始資料-資料庫-取得[筆數]從[管理者後台-CRM-活動管理]
        var coEmpPplOgnDbGetCountFromMbsCrmActReqObj = new CoEmpPplOgnDbGetCountFromMbsCrmActReqMdl()
        {
            ManagerActivityID = reqObj.ManagerActivityID,
            AtomPipelineStatus = reqObj.AtomPipelineStatus,
            EmployeePipelineOriginalManagerCompanyName = reqObj.ManagerCompanyName,
            EmployeePipelineOriginalManagerContacterName = reqObj.ManagerContacterName,
            EmployeePipelineOriginalManagerContacterEmail = reqObj.ManagerContacterEmail
        };
        var coEmpPplOgnDbGetCountFromMbsCrmActRspObj = await this._coEmpPipelineOriginalDb.GetCountFromMbsCrmActAsync(coEmpPplOgnDbGetCountFromMbsCrmActReqObj);
        if (coEmpPplOgnDbGetCountFromMbsCrmActRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // 判斷筆數是否為0
        if (coEmpPplOgnDbGetCountFromMbsCrmActRspObj.Count == 0)
        {
            rspObj.ErrorCode = MbsErrorCodeEnum.Success;
            rspObj.EmployeePipelineList = new();
            rspObj.TotalCount = 0;
            return rspObj;
        }

        // db, 核心-員工-商機原始資料-資料庫-取得多筆從[管理者後台-CRM-活動管理]
        var coEmpPplOgnDbGetManyFromMbsCrmActReqObj = new CoEmpPplOgnDbGetManyFromMbsCrmActReqMdl()
        {
            ManagerActivityID = reqObj.ManagerActivityID,
            AtomPipelineStatus = reqObj.AtomPipelineStatus,
            EmployeePipelineOriginalManagerCompanyName = reqObj.ManagerCompanyName,
            EmployeePipelineOriginalManagerContacterName = reqObj.ManagerContacterName,
            EmployeePipelineOriginalManagerContacterEmail = reqObj.ManagerContacterEmail,
            PageIndex = reqObj.PageIndex,
            PageSize = reqObj.PageSize
        };
        var coEmpPplOgnDbGetManyFromMbsCrmActRspObj = await this._coEmpPipelineOriginalDb.GetManyFromMbsCrmActAsync(coEmpPplOgnDbGetManyFromMbsCrmActReqObj);
        if (coEmpPplOgnDbGetManyFromMbsCrmActRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        rspObj.EmployeePipelineList = coEmpPplOgnDbGetManyFromMbsCrmActRspObj.EmployeePipelineOriginalList
            .Select(ppl => new MbsCrmActLgcGetManyActivityEmployeePipelineRspItemMdl
            {
                EmployeePipelineID = ppl.EmployeePipelineID,
                EmployeePipelineOriginalManagerCompanyName = ppl.EmployeePipelineOriginalManagerCompanyName,
                EmployeePipelineOriginalManagerContacterDepartment = ppl.EmployeePipelineOriginalManagerContacterDepartment,
                EmployeePipelineOriginalManagerContacterJobTitle = ppl.EmployeePipelineOriginalManagerContacterJobTitle,
                EmployeePipelineOriginalManagerContacterName = ppl.EmployeePipelineOriginalManagerContacterName,
                EmployeePipelineOriginalManagerContacterEmail = ppl.EmployeePipelineOriginalManagerContacterEmail,
                EmployeePipelineOriginalManagerContacterPhone = ppl.EmployeePipelineOriginalManagerContacterPhone,
                EmployeePipelineOriginalManagerContacterTelephone = ppl.EmployeePipelineOriginalManagerContacterTelephone,
                AtomPipelineStatus = ppl.AtomPipelineStatus
            }).ToList();
        rspObj.TotalCount = coEmpPplOgnDbGetCountFromMbsCrmActRspObj.Count;
        return rspObj;
    }

    /// <summary>管理者後台-CRM-活動管理-邏輯服務-取得活動名單</summary>
    public async Task<MbsCrmActLgcGetActivityEmployeePipelineRspMdl> GetActivityEmployeePipelineAsync(MbsCrmActLgcGetActivityEmployeePipelineReqMdl reqObj)
    {
        var rspObj = new MbsCrmActLgcGetActivityEmployeePipelineRspMdl
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

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        // 狀態
        rspObj.AtomPipelineStatus = coEmpPplDbGetRspObj.AtomPipelineStatus;
        // 客戶
        rspObj.ManagerCompanyUnifiedNumber = coEmpPplOgnDbGetRspObj.ManagerCompanyUnifiedNumber;
        rspObj.ManagerCompanyName = coEmpPplOgnDbGetRspObj.ManagerCompanyName;
        rspObj.AtomEmployeeRange = coEmpPplOgnDbGetRspObj.AtomEmployeeRange;
        rspObj.AtomCustomerGrade = coEmpPplOgnDbGetRspObj.AtomCustomerGrade;
        rspObj.ManagerCompanyMainKindName = coEmpPplOgnDbGetRspObj.ManagerCompanyMainKindName;
        rspObj.ManagerCompanySubKindName = coEmpPplOgnDbGetRspObj.ManagerCompanySubKindName;
        rspObj.AtomCity = coEmpPplOgnDbGetRspObj.AtomCity;
        rspObj.ManagerCompanyLocationAddress = coEmpPplOgnDbGetRspObj.ManagerCompanyLocationAddress;
        rspObj.ManagerCompanyLocationTelephone = coEmpPplOgnDbGetRspObj.ManagerCompanyLocationTelephone;
        rspObj.ManagerCompanyLocationStatus = coEmpPplOgnDbGetRspObj.ManagerCompanyLocationStatus;
        // 窗口
        rspObj.ManagerContacterName = coEmpPplOgnDbGetRspObj.ManagerContacterName;
        rspObj.ManagerContacterEmail = coEmpPplOgnDbGetRspObj.ManagerContacterEmail;
        rspObj.ManagerContacterPhone = coEmpPplOgnDbGetRspObj.ManagerContacterPhone;
        rspObj.ManagerContacterDepartment = coEmpPplOgnDbGetRspObj.ManagerContacterDepartment;
        rspObj.ManagerContacterJobTitle = coEmpPplOgnDbGetRspObj.ManagerContacterJobTitle;
        rspObj.ManagerContacterTelephone = coEmpPplOgnDbGetRspObj.ManagerContacterTelephone;
        rspObj.ManagerContacterIsConsent = coEmpPplOgnDbGetRspObj.ManagerContacterIsConsent;
        rspObj.ManagerContacterStatus = coEmpPplOgnDbGetRspObj.ManagerContacterStatus;
        rspObj.AtomRatingKind = coEmpPplOgnDbGetRspObj.AtomRatingKind;
        // 報名狀態
        rspObj.TeamsMeetingDuration = coEmpPplOgnDbGetRspObj.TeamsMeetingDuration;
        rspObj.TeamsRole = coEmpPplOgnDbGetRspObj.TeamsRole;
        return rspObj;
    }

    /// <summary>管理者後台-CRM-活動管理-邏輯服務-新增活動名單</summary>
    public async Task<MbsCrmActLgcAddActivityEmployeePipelineRspMdl> AddActivityEmployeePipelineAsync(MbsCrmActLgcAddActivityEmployeePipelineReqMdl reqObj)
    {
        var rspObj = new MbsCrmActLgcAddActivityEmployeePipelineRspMdl
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

        // db, 核心-管理者-活動-資料庫-取得
        var coMgrActivityDbGetReqObj = new CoMgrActivityDbGetReqMdl()
        {
            ManagerActivityID = reqObj.ManagerActivityID
        };
        var coMgrActivityDbGetRspObj = await this._coMgrActivityDb.GetAsync(coMgrActivityDbGetReqObj);
        if (coMgrActivityDbGetRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // 檢查實體活動 - AtomPipelineStatus限定[-]
        if (coMgrActivityDbGetRspObj.ManagerActivityKind == DbAtomManagerActivityKindEnum.PhysicalActivity &&
            reqObj.AtomPipelineStatus != DbAtomPipelineStatusEnum.Hyphen)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // 檢查線上活動 - AtomPipelineStatus限定[-]or已開啟or已點擊
        if (coMgrActivityDbGetRspObj.ManagerActivityKind == DbAtomManagerActivityKindEnum.OnlineActivity &&
            reqObj.AtomPipelineStatus != DbAtomPipelineStatusEnum.Hyphen &&
            reqObj.AtomPipelineStatus != DbAtomPipelineStatusEnum.Opened &&
            reqObj.AtomPipelineStatus != DbAtomPipelineStatusEnum.Clicked)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // db, 核心-管理者-公司-資料庫-取得
        var coMgrCmpDbGetReqObj = new CoMgrCmpDbGetReqMdl()
        {
            ManagerCompanyID = reqObj.ManagerCompanyID
        };
        var coMgrCmpDbGetRspObj = await this._coMgrCompanyDb.GetAsync(coMgrCmpDbGetReqObj);
        if (coMgrCmpDbGetRspObj == default)
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

        // db, 核心-管理者-公司主分類-資料庫-取得[名稱]
        var coMgrCmpMainKdDbGetNameReqObj = new CoMgrCmpMainKdDbGetNameReqMdl()
        {
            ManagerCompanyMainKindID = coMgrCmpDbGetRspObj.ManagerCompanyMainKindID
        };
        var coMgrCmpMainKdDbGetNameRspObj = await this._coMgrCompanyMainKindDb.GetNameAsync(coMgrCmpMainKdDbGetNameReqObj);
        if (coMgrCmpMainKdDbGetNameRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // db, 核心-管理者-公司子分類-資料庫-取得[名稱]
        var coMgrCmpSubKdDbGetNameReqObj = new CoMgrCmpSubKdDbGetNameReqMdl()
        {
            ManagerCompanySubKindID = coMgrCmpDbGetRspObj.ManagerCompanySubKindID
        };
        var coMgrCmpSubKdDbGetNameRspObj = await this._coMgrCompanySubKindDb.GetNameAsync(coMgrCmpSubKdDbGetNameReqObj);
        if (coMgrCmpSubKdDbGetNameRspObj == default)
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

        // db, 核心-員工-商機-資料庫-新增
        var coEmpPplDbAddReqObj = new CoEmpPplDbAddReqMdl()
        {
            ManagerActivityID = reqObj.ManagerActivityID,
            ManagerCompanyID = reqObj.ManagerCompanyID,
            ManagerCompanyLocationID = reqObj.ManagerCompanyLocationID,
            AtomPipelineStatus = reqObj.AtomPipelineStatus
        };
        var coEmpPplDbAddRspObj = await this._coEmpPipelineDb.AddAsync(coEmpPplDbAddReqObj);
        if (coEmpPplDbAddRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // db, 核心-員工-商機窗口-資料庫-新增
        var coEmpPplContDbAddReqObj = new CoEmpPplContDbAddReqMdl()
        {
            EmployeePipelineID = coEmpPplDbAddRspObj.EmployeePipelineID,
            ManagerContacterID = coMgrCttDbGetRspObj.ManagerContacterID,
            EmployeePipelineContacterIsPrimary = true
        };
        var coEmpPplContDbAddRspObj = await this._coEmpPipelineContacterDb.AddAsync(coEmpPplContDbAddReqObj);
        if (coEmpPplContDbAddRspObj == default)
        {
            // db, 核心-員工-商機-資料庫-移除
            var coEmpPplDbRemoveReqObj = new CoEmpPplDbRemoveReqMdl()
            {
                EmployeePipelineID = coEmpPplDbAddRspObj.EmployeePipelineID
            };
            var coEmpPplDbRemoveRspObj = await this._coEmpPipelineDb.RemoveAsync(coEmpPplDbRemoveReqObj);
            if (coEmpPplDbRemoveRspObj == default)
            {
                this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                return rspObj;
            }

            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // db, 核心-員工-商機原始資料-資料庫-新增
        var coEmpPplOgnDbAddReqObj = new CoEmpPplOgnDbAddReqMdl()
        {
            EmployeePipelineID = coEmpPplDbAddRspObj.EmployeePipelineID,
            ManagerCompanyUnifiedNumber = coMgrCmpDbGetRspObj.ManagerCompanyUnifiedNumber?.Trim(),
            ManagerCompanyName = coMgrCmpDbGetRspObj.ManagerCompanyName?.Trim(),
            AtomEmployeeRange = coMgrCmpDbGetRspObj.AtomEmployeeRange,
            AtomCustomerGrade = coMgrCmpDbGetRspObj.AtomCustomerGrade,
            ManagerCompanyMainKindName = coMgrCmpMainKdDbGetNameRspObj.ManagerCompanyMainKindName?.Trim(),
            ManagerCompanySubKindName = coMgrCmpSubKdDbGetNameRspObj.ManagerCompanySubKindName?.Trim(),
            AtomCity = coMgrCmpLocDbGetRspObj.AtomCity,
            ManagerCompanyLocationAddress = coMgrCmpLocDbGetRspObj.ManagerCompanyLocationAddress?.Trim(),
            ManagerCompanyLocationTelephone = coMgrCmpLocDbGetRspObj.ManagerCompanyLocationTelephone?.Trim(),
            ManagerCompanyLocationStatus = coMgrCmpLocDbGetRspObj.ManagerCompanyLocationStatus,
            ManagerContacterName = coMgrCttDbGetRspObj.ManagerContacterName?.Trim(),
            ManagerContacterEmail = coMgrCttDbGetRspObj.ManagerContacterEmail?.Trim(),
            ManagerContacterPhone = coMgrCttDbGetRspObj.ManagerContacterPhone?.Trim(),
            ManagerContacterDepartment = coMgrCttDbGetRspObj.ManagerContacterDepartment?.Trim(),
            ManagerContacterJobTitle = coMgrCttDbGetRspObj.ManagerContacterJobTitle?.Trim(),
            ManagerContacterTelephone = coMgrCttDbGetRspObj.ManagerContacterTelephone?.Trim(),
            ManagerContacterIsConsent = coMgrCttDbGetRspObj.ManagerContacterIsConsent,
            ManagerContacterStatus = coMgrCttDbGetRspObj.ManagerContacterStatus,
            AtomRatingKind = coMgrCttDbGetRspObj.AtomRatingKind
        };
        var coEmpPplOgnDbAddRspObj = await this._coEmpPipelineOriginalDb.AddAsync(coEmpPplOgnDbAddReqObj);
        if (coEmpPplOgnDbAddRspObj == default)
        {
            // db, 核心-員工-商機-資料庫-移除
            var coEmpPplDbRemoveReqObj = new CoEmpPplDbRemoveReqMdl()
            {
                EmployeePipelineID = coEmpPplDbAddRspObj.EmployeePipelineID
            };
            var coEmpPplDbRemoveRspObj = await this._coEmpPipelineDb.RemoveAsync(coEmpPplDbRemoveReqObj);
            if (coEmpPplDbRemoveRspObj == default)
            {
                this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                return rspObj;
            }

            // db, 核心-員工-商機窗口-資料庫-移除
            var coEmpPplCttDbRemoveReqObj = new CoEmpPplContDbRemoveReqMdl()
            {
                EmployeePipelineContacterID = coEmpPplContDbAddRspObj.EmployeePipelineContacterID
            };
            var coEmpPplCttDbRemoveRspObj = await this._coEmpPipelineContacterDb.RemoveAsync(coEmpPplCttDbRemoveReqObj);
            if (coEmpPplCttDbRemoveRspObj == default)
            {
                this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                return rspObj;
            }
        }

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        rspObj.EmployeePipelineID = coEmpPplDbAddRspObj.EmployeePipelineID;
        return rspObj;
    }

    /// <summary>管理者後台-CRM-活動管理-邏輯服務-更新多筆活動名單</summary>
    public async Task<MbsCrmActLgcUpdateManyActivityEmployeePipelineRspMdl> UpdateManyActivityEmployeePipelineAsync(MbsCrmActLgcUpdateManyActivityEmployeePipelineReqMdl reqObj)
    {
        var rspObj = new MbsCrmActLgcUpdateManyActivityEmployeePipelineRspMdl
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

        // 目前限制只能已轉電銷
        if (reqObj.AtomPipelineStatus != DbAtomPipelineStatusEnum.TransferredToSales)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // 允許轉給電銷的狀態清單
        var allowedStatusList = new List<DbAtomPipelineStatusEnum>
        {
            DbAtomPipelineStatusEnum.Hyphen,
            DbAtomPipelineStatusEnum.Opened,
            DbAtomPipelineStatusEnum.Clicked,
        };

        // db, 核心-員工-商機-資料庫-取得多筆
        var coEmpPplDbGetManyReqObj = new CoEmpPplDbGetManyReqMdl()
        {
            EmployeePipelineIDList = reqObj.EmployeePipelineIDList,
        };
        var coEmpPplDbGetManyRspObj = await this._coEmpPipelineDb.GetManyAsync(coEmpPplDbGetManyReqObj);
        if (coEmpPplDbGetManyRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // 檢查所有商機的當前狀態是否都在允許範圍內
        var hasInvalidStatus = coEmpPplDbGetManyRspObj.EmployeePipelineList
            .Any(x => !allowedStatusList.Contains(x.AtomPipelineStatus));

        if (hasInvalidStatus)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // db, 核心-員工-商機-資料庫-批次更新商機狀態
        var coEmpPplDbUpdateManyReqObj = new CoEmpPplDbUpdateManyAtomPipelineStatusReqMdl()
        {
            EmployeePipelineIDList = reqObj.EmployeePipelineIDList,
            AtomPipelineStatus = reqObj.AtomPipelineStatus
        };
        var coEmpPplDbUpdateManyRspObj = await this._coEmpPipelineDb.UpdateManyAtomPipelineStatusAsync(coEmpPplDbUpdateManyReqObj);
        if (coEmpPplDbUpdateManyRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        return rspObj;
    }

    /// <summary>管理者後台-CRM-活動管理-邏輯服務-刪除多筆活動名單</summary>
    public async Task<MbsCrmActLgcRemoveManyActivityEmployeePipelineRspMdl> RemoveManyActivityEmployeePipelineAsync(MbsCrmActLgcRemoveManyActivityEmployeePipelineReqMdl reqObj)
    {
        var rspObj = new MbsCrmActLgcRemoveManyActivityEmployeePipelineRspMdl
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

        // db, 核心-員工-商機-資料庫-取得多筆
        var coEmpPplDbGetManyReqObj = new CoEmpPplDbGetManyReqMdl()
        {
            EmployeePipelineIDList = reqObj.EmployeePipelineIDList
        };
        var coEmpPplDbGetManyRspObj = await this._coEmpPipelineDb.GetManyAsync(coEmpPplDbGetManyReqObj);
        if (coEmpPplDbGetManyRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // 沒資料
        if (coEmpPplDbGetManyRspObj.EmployeePipelineList == default ||
            coEmpPplDbGetManyRspObj.EmployeePipelineList.Count == 0)
        {
            rspObj.ErrorCode = MbsErrorCodeEnum.Success;
            return rspObj;
        }

        // 刪除只限制商機狀態 = 連字號 / eDM已開啟 / eDM已點擊
        if (coEmpPplDbGetManyRspObj.EmployeePipelineList
                    .Any(x => x.AtomPipelineStatus != DbAtomPipelineStatusEnum.Hyphen &&
                               x.AtomPipelineStatus != DbAtomPipelineStatusEnum.Opened &&
                               x.AtomPipelineStatus != DbAtomPipelineStatusEnum.Clicked))
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // db, 核心-員工-商機-資料庫-移除多筆
        var coEmpPplDbRemoveManyReqObj = new CoEmpPplDbRemoveManyReqMdl()
        {
            EmployeePipelineIDList = reqObj.EmployeePipelineIDList
        };
        var coEmpPplDbRemoveManyRspObj = await this._coEmpPipelineDb.RemoveManyAsync(coEmpPplDbRemoveManyReqObj);
        if (coEmpPplDbRemoveManyRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // db, 核心-員工-商機窗口-資料庫-移除多筆
        var coEmpPplContDbRemoveManyReqObj = new CoEmpPplContDbRemoveManyReqMdl()
        {
            EmployeePipelineIDList = reqObj.EmployeePipelineIDList
        };
        var coEmpPplContDbRemoveManyRspObj = await this._coEmpPipelineContacterDb.RemoveManyAsync(coEmpPplContDbRemoveManyReqObj);
        if (coEmpPplContDbRemoveManyRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // db, 核心-員工-商機原始資料-資料庫-移除多筆
        var coEmpPplOgnDbRemoveManyReqObj = new CoEmpPplOgnDbRemoveManyReqMdl()
        {
            EmployeePipelineIDList = reqObj.EmployeePipelineIDList
        };
        var coEmpPplOgnDbRemoveManyRspObj = await this._coEmpPipelineOriginalDb.RemoveManyAsync(coEmpPplOgnDbRemoveManyReqObj);
        if (coEmpPplOgnDbRemoveManyRspObj == default)
        {
            // db, 核心-員工-商機-資料庫-新增多筆
            var coEmpPplDbAddManyReqObj = new CoEmpPplDbAddManyReqMdl()
            {
                EmployeePipelineList = coEmpPplDbGetManyRspObj.EmployeePipelineList
                                            .Select(x => new CoEmpPplDbAddManyReqItemMdl
                                            {
                                                ManagerActivityID = x.ManagerActivityID,
                                                ManagerCompanyID = x.ManagerCompanyID,
                                                ManagerCompanyLocationID = x.ManagerCompanyLocationID,
                                                AtomPipelineStatus = x.AtomPipelineStatus
                                            }).ToList()
            };
            var coEmpPplDbAddManyRspObj = await this._coEmpPipelineDb.AddManyAsync(coEmpPplDbAddManyReqObj);
            if (coEmpPplDbAddManyRspObj == default)
            {
                this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                return rspObj;
            }

            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        return rspObj;
    }

    /// <summary>管理者後台-CRM-活動管理-邏輯服務-取得多筆客戶過往活動</summary>
    public async Task<MbsCrmActLgcGetManyPastActivityRspMdl> GetManyPastActivityAsync(MbsCrmActLgcGetManyPastActivityReqMdl reqObj)
    {
        var rspObj = new MbsCrmActLgcGetManyPastActivityRspMdl
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
            ? new MbsCrmActLgcGetManyPastActivityRspItemMdl
            {
                ManagerActivityName = coMgrActivityDbGetLatestPastActivityFromEmployeePipelineRspObj.ManagerActivityName,
                ManagerActivityStartTime = coMgrActivityDbGetLatestPastActivityFromEmployeePipelineRspObj.ManagerActivityStartTime,
                ManagerActivityEndTime = coMgrActivityDbGetLatestPastActivityFromEmployeePipelineRspObj.ManagerActivityEndTime
            }
            : null;
        rspObj.PastActivityList = coMgrActivityDbGetManyPastActivityFromEmployeePipelineRspObj.ManagerActivityList
            .Select(x => new MbsCrmActLgcGetManyPastActivityRspItemMdl
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

    #region 活動問卷

    /// <summary>管理者後台-CRM-活動管理-邏輯服務-新增活動問卷問題</summary>
    public async Task<MbsCrmActLgcAddActivitySurveyQuestionRspMdl> AddActivitySurveyQuestionAsync(MbsCrmActLgcAddActivitySurveyQuestionReqMdl reqObj)
    {
        var rspObj = new MbsCrmActLgcAddActivitySurveyQuestionRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail,
            ManagerActivitySurveyQuestionIDList = new List<int>()
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

        // 新增問題
        foreach (var questionItem in reqObj.ManagerActivitySurveyQuestionList)
        {
            // db, 核心-管理者-活動問卷問題-資料庫-新增
            var coMgrAsqDbAddReqObj = new CoMgrAsqDbAddReqMdl()
            {
                ManagerActivityID = questionItem.ManagerActivityID,
                ManagerActivitySurveyQuestionKind = questionItem.ManagerActivitySurveyQuestionKind,
                ManagerActivitySurveyQuestionTitle = questionItem.ManagerActivitySurveyQuestionTitle,
                ManagerActivitySurveyQuestionContent = questionItem.ManagerActivitySurveyQuestionContent,
                IsOther = questionItem.IsOther,
                ManagerActivitySurveyQuestionSort = questionItem.ManagerActivitySurveyQuestionSort,
            };
            var coMgrAsqDbAddRspObj = await this._coMgrActivitySurveyQuestionDb.AddAsync(coMgrAsqDbAddReqObj);
            if (coMgrAsqDbAddRspObj == default)
            {
                this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                return rspObj;
            }

            rspObj.ManagerActivitySurveyQuestionIDList.Add(coMgrAsqDbAddRspObj.ManagerActivitySurveyQuestionID);

            // db, 核心-管理者-活動問卷問題項目-資料庫-新增多筆
            if (questionItem.ManagerActivitySurveyQuestionKind is DbManagerActivitySurveyQuestionKindEnum.Single or DbManagerActivitySurveyQuestionKindEnum.Multiple)
            {
                if (questionItem.ManagerActivitySurveyQuestionItemList == null || questionItem.ManagerActivitySurveyQuestionItemList.Count < 2)
                {
                    // recovery
                    //db, 核心-管理者-活動問卷問題-資料庫-移除
                    var coMgrAsqDbRemoveReqObj = new CoMgrAsqDbRemoveReqMdl()
                    {
                        ManagerActivitySurveyQuestionID = coMgrAsqDbAddRspObj.ManagerActivitySurveyQuestionID
                    };
                    await this._coMgrActivitySurveyQuestionDb.RemoveAsync(coMgrAsqDbRemoveReqObj);

                    this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                    return rspObj;
                }

                var coMgrActSqiDbAddManyReqObj = new CoMgrActSqiDbAddManyReqMdl()
                {
                    ManagerActivitySurveyQuestionItemList = questionItem.ManagerActivitySurveyQuestionItemList
                    .Select(x => new CoMgrActSqiDbAddManyReqItemMdl
                    {
                        ManagerActivityID = questionItem.ManagerActivityID,
                        ManagerActivitySurveyQuestionID = coMgrAsqDbAddRspObj.ManagerActivitySurveyQuestionID,
                        ManagerActivitySurveyQuestionItemName = x.ManagerActivitySurveyQuestionItemName,
                        ManagerActivitySurveyQuestionItemSort = x.ManagerActivitySurveyQuestionItemSort,
                    })
                    .ToList()
                };
                var coMgrActSqiDbAddManyRspObj = await this._coMgrActivitySurveyQuestionItemDb.AddManyAsync(coMgrActSqiDbAddManyReqObj);
                if (coMgrActSqiDbAddManyRspObj == default)
                {
                    // recovery
                    //db, 核心-管理者-活動問卷問題-資料庫-移除
                    var coMgrAsqDbRemoveReqObj = new CoMgrAsqDbRemoveReqMdl()
                    {
                        ManagerActivitySurveyQuestionID = coMgrAsqDbAddRspObj.ManagerActivitySurveyQuestionID
                    };
                    await this._coMgrActivitySurveyQuestionDb.RemoveAsync(coMgrAsqDbRemoveReqObj);

                    this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                    return rspObj;
                }
            }
        }

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        return rspObj;
    }

    /// <summary>管理者後台-CRM-活動管理-邏輯服務-取得活動問卷問題</summary>
    public async Task<MbsCrmActLgcGetActivitySurveyQuestionRspMdl> GetActivitySurveyQuestionAsync(MbsCrmActLgcGetActivitySurveyQuestionReqMdl reqObj)
    {
        var rspObj = new MbsCrmActLgcGetActivitySurveyQuestionRspMdl
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

        // db, 核心-管理者-活動問卷問題-資料庫-取得多筆從[管理者活動ID]
        var questionDbRspObj = await this._coMgrActivitySurveyQuestionDb.GetManyFromActIdAsync(new CoMgrAsqDbGetManyFromActIdReqMdl
        {
            ManagerActivityID = reqObj.ManagerActivityID
        });
        if (questionDbRspObj?.ManagerActivitySurveyQuestionList == null)
        {
            rspObj.ErrorCode = MbsErrorCodeEnum.Fail;
            return rspObj;
        }

        var questionList = new List<MbsCrmActLgcGetActivitySurveyQuestionRspItemMdl>();

        foreach (var question in questionDbRspObj.ManagerActivitySurveyQuestionList)
        {
            // db, 核心-管理者-活動問卷問題項目-資料庫-取得多筆從[管理者活動問卷問題ID]
            var questionItemDbRspObj = await this._coMgrActivitySurveyQuestionItemDb.GetManyFromAsqAsync(new CoMgrActSqiDbGetManyFromAsqReqMdl
            {
                ManagerActivitySurveyQuestionID = question.ManagerActivitySurveyQuestionID
            });

            var questionItemList = questionItemDbRspObj.ManagerActivitySurveyQuestionItemList
                .Select(x => new MbsCrmActLgcGetActivitySurveyQuestionRspItemQuestionItemMdl()
                {
                    ManagerActivitySurveyQuestionItemID = x.ManagerActivitySurveyQuestionItemID,
                    ManagerActivitySurveyQuestionItemName = x.ManagerActivitySurveyQuestionItemName,
                    ManagerActivitySurveyQuestionItemSort = x.ManagerActivitySurveyQuestionItemSort
                })
                .ToList();

            questionList.Add(new MbsCrmActLgcGetActivitySurveyQuestionRspItemMdl
            {
                ManagerActivitySurveyQuestionID = question.ManagerActivitySurveyQuestionID,
                ManagerActivitySurveyQuestionKind = question.ManagerActivitySurveyQuestionKind,
                ManagerActivitySurveyQuestionTitle = question.ManagerActivitySurveyQuestionTitle,
                ManagerActivitySurveyQuestionContent = question.ManagerActivitySurveyQuestionContent,
                IsOther = question.IsOther,
                ManagerActivitySurveyQuestionSort = question.ManagerActivitySurveyQuestionSort,
                ManagerActivitySurveyQuestionItemList = questionItemList
            });
        }

        rspObj.ManagerActivitySurveyQuestionList = questionList;
        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        return rspObj;
    }

    /// <summary>管理者後台-CRM-活動管理-邏輯服務-更新活動問卷問題</summary>
    public async Task<MbsCrmActLgcUpdateActivitySurveyQuestionRspMdl> UpdateActivitySurveyQuestionAsync(MbsCrmActLgcUpdateActivitySurveyQuestionReqMdl reqObj)
    {
        var rspObj = new MbsCrmActLgcUpdateActivitySurveyQuestionRspMdl
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
            return rspObj;
        }

        //判斷權限
        if (mbsCmnLgcGetLoginUserInfoRspObj.AtomPermissionKindIdEdit != DbAtomPermissionKindEnum.Everyone)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        #endregion

        // 判斷問卷問題是否需要更新
        if (reqObj.ManagerActivitySurveyQuestionList != null)
        {
            // db, 核心-管理者-活動問卷問題項目-資料庫-移除多筆從[管理者活動ID]
            var coMgrActSqiDbRemoveManyByActIdReqObj = new CoMgrActSqiDbRemoveManyByActIdReqMdl()
            {
                ManagerActivityID = reqObj.ManagerActivityID
            };
            var coMgrActSqiDbRemoveManyByActIdRspObj = await this._coMgrActivitySurveyQuestionItemDb.RemoveManyByActIdAsync(coMgrActSqiDbRemoveManyByActIdReqObj);
            if (coMgrActSqiDbRemoveManyByActIdRspObj == default)
            {
                this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                return rspObj;
            }

            // db, 核心-管理者-活動問卷問題-資料庫-移除多筆
            var coMgrAsqDbRemoveManyReqObj = new CoMgrAsqDbRemoveManyReqMdl()
            {
                ManagerActivityID = reqObj.ManagerActivityID
            };
            var coMgrAsqDbRemoveManyRspObj = await this._coMgrActivitySurveyQuestionDb.RemoveManyAsync(coMgrAsqDbRemoveManyReqObj);
            if (coMgrAsqDbRemoveManyRspObj == default)
            {
                this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                return rspObj;
            }

            // 新增問題
            foreach (var question in reqObj.ManagerActivitySurveyQuestionList)
            {
                // db, 核心-管理者-活動問卷問題-資料庫-新增
                var coMgrAsqDbAddReqObj = new CoMgrAsqDbAddReqMdl()
                {
                    ManagerActivityID = reqObj.ManagerActivityID,
                    ManagerActivitySurveyQuestionKind = question.ManagerActivitySurveyQuestionKind,
                    ManagerActivitySurveyQuestionTitle = question.ManagerActivitySurveyQuestionTitle,
                    ManagerActivitySurveyQuestionContent = question.ManagerActivitySurveyQuestionContent,
                    IsOther = question.IsOther,
                    ManagerActivitySurveyQuestionSort = question.ManagerActivitySurveyQuestionSort,
                };
                var coMgrAsqDbAddRspObj = await this._coMgrActivitySurveyQuestionDb.AddAsync(coMgrAsqDbAddReqObj);
                if (coMgrAsqDbAddRspObj == default)
                {
                    this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                    return rspObj;
                }

                // db, 核心-管理者-活動問卷問題項目-資料庫-新增多筆
                if (question.ManagerActivitySurveyQuestionKind is DbManagerActivitySurveyQuestionKindEnum.Single or DbManagerActivitySurveyQuestionKindEnum.Multiple)
                {
                    if (question.ManagerActivitySurveyQuestionItemList == null || question.ManagerActivitySurveyQuestionItemList.Count < 2)
                    {
                        this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                        return rspObj;
                    }

                    var coMgrActSqiDbAddManyReqObj = new CoMgrActSqiDbAddManyReqMdl()
                    {
                        ManagerActivitySurveyQuestionItemList = question.ManagerActivitySurveyQuestionItemList
                        .Select(x => new CoMgrActSqiDbAddManyReqItemMdl
                        {
                            ManagerActivityID = reqObj.ManagerActivityID,
                            ManagerActivitySurveyQuestionID = coMgrAsqDbAddRspObj.ManagerActivitySurveyQuestionID,
                            ManagerActivitySurveyQuestionItemName = x.ManagerActivitySurveyQuestionItemName,
                            ManagerActivitySurveyQuestionItemSort = x.ManagerActivitySurveyQuestionItemSort,
                        })
                        .ToList()
                    };
                    var coMgrActSqiDbAddManyRspObj = await this._coMgrActivitySurveyQuestionItemDb.AddManyAsync(coMgrActSqiDbAddManyReqObj);
                    if (coMgrActSqiDbAddManyRspObj == default)
                    {
                        this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                        return rspObj;
                    }
                }
            }
        }

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        return rspObj;
    }

    /// <summary>管理者後台-CRM-活動管理-邏輯服務-刪除活動問卷問題</summary>
    public async Task<MbsCrmActLgcRemoveActivitySurveyQuestionRspMdl> RemoveActivitySurveyQuestionAsync(MbsCrmActLgcRemoveActivitySurveyQuestionReqMdl reqObj)
    {
        var rspObj = new MbsCrmActLgcRemoveActivitySurveyQuestionRspMdl
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

        // db, 核心-管理者-活動問卷問題-資料庫-移除
        var coMgrAsqDbRemoveReqObj = new CoMgrAsqDbRemoveReqMdl()
        {
            ManagerActivitySurveyQuestionID = reqObj.ManagerActivitySurveyQuestionID,
        };
        var coMgrAsqDbRemoveRspObj = await this._coMgrActivitySurveyQuestionDb.RemoveAsync(coMgrAsqDbRemoveReqObj);
        if (coMgrAsqDbRemoveRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // db, 核心-管理者-活動問卷問題項目-資料庫-移除多筆
        var coMgrActSqiDbRemoveManyReqObj = new CoMgrActSqiDbRemoveManyReqMdl()
        {
            ManagerActivitySurveyQuestionID = reqObj.ManagerActivitySurveyQuestionID,
        };
        var coMgrActSqiDbRemoveManyRspObj = await this._coMgrActivitySurveyQuestionItemDb.RemoveManyAsync(coMgrActSqiDbRemoveManyReqObj);
        if (coMgrActSqiDbRemoveManyRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        #endregion

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        return rspObj;
    }

    /// <summary>管理者後台-CRM-活動管理-邏輯服務-取得多筆活動問卷回答者</summary>
    public async Task<MbsCrmActLgcGetManyActivitySurveyAnswererRspMdl> GetManyActivitySurveyAnswererAsync(MbsCrmActLgcGetManyActivitySurveyAnswererReqMdl reqObj)
    {
        var rspObj = new MbsCrmActLgcGetManyActivitySurveyAnswererRspMdl
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
            return rspObj;
        }

        //判斷權限
        if (mbsCmnLgcGetLoginUserInfoRspObj.AtomPermissionKindIdView != DbAtomPermissionKindEnum.Everyone)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        #endregion

        // db, 核心-管理者-活動問卷回答者-資料庫-取得筆數從[管理者活動ID]
        var coMgrAsaDbGetCountFromActivityIdReqObj = new CoMgrAsaDbGetCountFromActivityIdReqMdl()
        {
            ManagerActivityID = reqObj.ManagerActivityID,
            ManagerActivitySurveyAnswererCompanyName = reqObj.ManagerActivitySurveyAnswererCompanyName,
            ManagerActivitySurveyAnswererContacterName = reqObj.ManagerActivitySurveyAnswererContacterName,
            ManagerActivitySurveyAnswererContacterEmail = reqObj.ManagerActivitySurveyAnswererContacterEmail,
        };
        var coMgrAsaDbGetCountFromActivityIdRspObj = await this._coMgrActivitySurveyAnswererDb.GetCountFromActivityIdAsync(coMgrAsaDbGetCountFromActivityIdReqObj);
        if (coMgrAsaDbGetCountFromActivityIdRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        // 判斷筆數是否為0
        if (coMgrAsaDbGetCountFromActivityIdRspObj.Count == 0)
        {
            // 回傳空
            rspObj.ErrorCode = MbsErrorCodeEnum.Success;
            rspObj.ManagerActivitySurveyAnswererList = new List<MbsCrmActLgcGetManyActivitySurveyAnswererRspItemMdl>();
            rspObj.TotalCount = 0;
            return rspObj;
        }

        // db, 核心-管理者-活動問卷回答者-資料庫-取得多筆從[管理者活動ID]
        var coMgrAsaDbGetManyFromActivityIdReqObj = new CoMgrAsaDbGetManyFromActivityIdReqMdl()
        {
            ManagerActivityID = reqObj.ManagerActivityID,
            ManagerActivitySurveyAnswererCompanyName = reqObj.ManagerActivitySurveyAnswererCompanyName,
            ManagerActivitySurveyAnswererContacterName = reqObj.ManagerActivitySurveyAnswererContacterName,
            ManagerActivitySurveyAnswererContacterEmail = reqObj.ManagerActivitySurveyAnswererContacterEmail,
            PageIndex = reqObj.PageIndex,
            PageSize = reqObj.PageSize,
        };
        var coMgrAsaDbGetManyFromActivityIdRspObj = await this._coMgrActivitySurveyAnswererDb.GetManyFromActivityIdAsync(coMgrAsaDbGetManyFromActivityIdReqObj);
        if (coMgrAsaDbGetManyFromActivityIdRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        rspObj.ManagerActivitySurveyAnswererList = coMgrAsaDbGetManyFromActivityIdRspObj.ManagerActivitySurveyAnswererList
        .Select(x => new MbsCrmActLgcGetManyActivitySurveyAnswererRspItemMdl()
        {
            ManagerActivitySurveyAnswererID = x.ManagerActivitySurveyAnswererID,
            ManagerActivitySurveyAnswererCompanyName = x.ManagerActivitySurveyAnswererCompanyName,
            ManagerActivitySurveyAnswererContacterName = x.ManagerActivitySurveyAnswererContacterName,
            ManagerActivitySurveyAnswererContacterEmail = x.ManagerActivitySurveyAnswererContacterEmail,
            ManagerActivitySurveyAnswererCompanyScaleID = x.ManagerActivitySurveyAnswererCompanyScaleID,
            ManagerActivitySurveyAnswererCompanyBudgetID = x.ManagerActivitySurveyAnswererCompanyBudgetID,
            ManagerActivitySurveyAnswererCompanyPurchaseID = x.ManagerActivitySurveyAnswererCompanyPurchaseID,
        })
        .ToList();
        rspObj.TotalCount = coMgrAsaDbGetCountFromActivityIdRspObj.Count;
        return rspObj;
    }

    #endregion

    #region eDM

    /// <summary>管理者後台-CRM-活動管理-邏輯服務-下載eDM範本</summary>
    public async Task<MbsCrmActLgcDownloadEdmTemplateRspMdl> DownloadEdmTemplateAsync(MbsCrmActLgcDownloadEdmTemplateReqMdl reqObj)
    {
        var rspObj = new MbsCrmActLgcDownloadEdmTemplateRspMdl
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

        // common, 通用-資料夾檔案-合併路徑
        var cmnFolderFileCombinePathReqObj = new CmnFolderFileCombinePathReqMdl()
        {
            PathList = new List<string>()
            {
                AppContext.BaseDirectory,
                "File",
                "EdmSample.xlsx",
            },
        };
        var cmnFolderFileCombinePathRspObj = this._cmnFolderFile.CombinePath(cmnFolderFileCombinePathReqObj);
        if (cmnFolderFileCombinePathRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        rspObj.LocalAbsoluteFileName = cmnFolderFileCombinePathRspObj.ResultPath;
        rspObj.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        rspObj.FileName = "EdmSample.xlsx";
        return rspObj;
    }

    /// <summary>管理者後台-CRM-活動管理-邏輯服務-匯入eDM</summary>
    public async Task<MbsCrmActLgcImportEdmRspMdl> ImportEdmAsync(MbsCrmActLgcImportEdmReqMdl reqObj)
    {
        var rspObj = new MbsCrmActLgcImportEdmRspMdl
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

        // common, 通用-Excel-讀取ByIFormFile
        var cmnExcelReadXlsxByIFormFilReqObj = new CmnExcelReadXlsxByIFormFilReqMdl()
        {
            FormFile = reqObj.EdmFile,
        };
        var cmnExcelReadXlsxByIFormFilRspObj = this._cmnExcel.ReadXlsxByIFormFile(cmnExcelReadXlsxByIFormFilReqObj);
        if (cmnExcelReadXlsxByIFormFilRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            rspObj.ErrorCode = MbsErrorCodeEnum.FileFormatError;
            return rspObj;
        }

        // 判斷欄位是否一致
        if (cmnExcelReadXlsxByIFormFilRspObj.DataTable.Columns.Count != 18)
        {
            // 不一致
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            rspObj.ErrorCode = MbsErrorCodeEnum.FileFormatError;
            return rspObj;
        }

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

        /* 規格
        以下欄位都以方式處理 

        -若沒資料，=> 新增 
        -若有資料，欄位空白 => 更新 
        -若有資料，欄位有值 => 不處理 
    
        功能需求： 
        - 需要將不處理或有問題的資料回傳給前端顯示出來 
        - 匯入資料部分，跑迴圈處理, 將每一筆資料進行檢查,失敗的紀錄並回傳錯誤列表
        */

        var errorDataList = new List<MbsCrmActLgcImportEdmRspItemMdl>();
        // 沒資料需要匯入
        var excelDataRowList = cmnExcelReadXlsxByIFormFilRspObj.DataTable
                        .AsEnumerable()
                        .Skip(1); // 跳過表頭
        if (excelDataRowList != null && excelDataRowList.Count() == 0)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            rspObj.ErrorCode = MbsErrorCodeEnum.FileContentError;
            return rspObj;
        }

        // db, 核心-員工-商機原始資料-資料庫-取得多筆從[Email列表]
        var coEmpPplOgnDbGetManyByEmailListReqObj = new CoEmpPplOgnDbGetManyByEmailListReqMdl()
        {
            ManagerActivityID = reqObj.ManagerActivityID,
            ManagerContacterEmailList = excelDataRowList
                                            .Where(row => !string.IsNullOrWhiteSpace(row[0]?.ToString().Trim()))
                                            .Select(row => row[0]?.ToString().Trim())
                                            .ToList()
        };
        var coEmpPplOgnDbGetManyByEmailListRspObj = await this._coEmpPipelineOriginalDb.GetManyByEmailListAsync(coEmpPplOgnDbGetManyByEmailListReqObj);
        if (coEmpPplOgnDbGetManyByEmailListRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        foreach (var excelDataRowItem in excelDataRowList)
        {
            var item = this.ConvertEdmDataRowToRspItem(excelDataRowItem);
            if (item == null)
            {
                errorDataList.Add(item);
                continue;
            }

            // 檢查 公司名稱or姓名or電子信箱 其中一個沒值則不處理,並回傳錯誤的資料列表
            var contacterName = item.EdmFirstName?.Trim() + item.EdmLastName?.Trim();
            if (string.IsNullOrWhiteSpace(item.EdmCompanyName?.Trim()) ||
                string.IsNullOrWhiteSpace(contacterName) ||
                string.IsNullOrWhiteSpace(item.EdmContacterEmail?.Trim()))
            {
                errorDataList.Add(item);
                continue;
            }

            // 透過excel email取得所有商機原始資料列表,判斷更新or新增
            var existingItem = coEmpPplOgnDbGetManyByEmailListRspObj.EmployeePipelineOriginalList
                                                        .FirstOrDefault(x => x.ManagerContacterEmail == item.EdmContacterEmail);
            if (existingItem != null)
            {
                // db, 核心-員工-商機原始資料-資料庫-更新
                var coEmpPplOgnDbUpdateReqObj = new CoEmpPplOgnDbUpdateReqMdl
                {
                    // 檢查欄位有沒有值, 欄位空白 => 更新, 欄位有值 => 不處理         
                    EmployeePipelineID = existingItem.EmployeePipelineID,
                    // 公司縣市
                    AtomCity = existingItem.AtomCity.HasValue == false
                                        ? item.EdmCompanyAddress.ExtractCityFromAddress<DbAtomCityEnum>()
                                        : null,
                    // 公司名稱
                    ManagerCompanyName = string.IsNullOrWhiteSpace(existingItem.ManagerCompanyName) == false
                                        ? item.EdmCompanyName
                                        : null,
                    // 公司電話
                    ManagerCompanyLocationTelephone = string.IsNullOrWhiteSpace(existingItem.ManagerCompanyLocationTelephone) == false
                                        ? item.EdmCompanyPhone
                                        : null,
                    // 客戶主類別
                    ManagerCompanyMainKindName = string.IsNullOrWhiteSpace(existingItem.ManagerCompanyMainKindName) == false
                                        ? item.EdmCompanyMainKind
                                        : null,
                    // 客戶子類別
                    ManagerCompanySubKindName = string.IsNullOrWhiteSpace(existingItem.ManagerCompanySubKindName) == false
                                        ? item.EdmCompanySubKind
                                        : null,
                    // 客戶營業地點狀態
                    ManagerCompanyLocationStatus = DbAtomManagerCompanyStatusEnum.Unclear,
                    // 窗口姓名
                    ManagerContacterName = string.IsNullOrWhiteSpace(existingItem.ManagerContacterName) == false
                                        ? contacterName
                                        : null,
                    // 窗口Email
                    ManagerContacterEmail = string.IsNullOrWhiteSpace(existingItem.ManagerContacterEmail) == false
                                        ? item.EdmContacterEmail
                                        : null,
                    // 窗口手機
                    ManagerContacterPhone = string.IsNullOrWhiteSpace(existingItem.ManagerContacterPhone) == false
                                        ? item.EdmContacterPhone
                                        : null,
                    // 窗口電話
                    ManagerContacterTelephone = string.IsNullOrWhiteSpace(existingItem.ManagerContacterTelephone) == false
                                        ? item.EdmContacterTelephone
                                        : null,
                    // 窗口部門
                    ManagerContacterDepartment = string.IsNullOrWhiteSpace(existingItem.ManagerContacterDepartment) == false
                                        ? item.EdmContacterDepartment
                                        : null,
                    // 窗口職稱
                    ManagerContacterJobTitle = string.IsNullOrWhiteSpace(existingItem.ManagerContacterJobTitle) == false
                                        ? item.EdmContacterJobTitle
                                        : null,
                    // 窗口狀態
                    ManagerContacterStatus = null
                };
                var coEmpPplOgnDbUpdateRspObj = await this._coEmpPipelineOriginalDb.UpdateAsync(coEmpPplOgnDbUpdateReqObj);
                if (coEmpPplOgnDbUpdateRspObj == default)
                {
                    errorDataList.Add(item);

                    this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                    continue;
                }
            }
            else
            {
                // db, 核心-員工-商機-資料庫-新增
                var coEmpPplDbAddReqObj = new CoEmpPplDbAddReqMdl()
                {
                    ManagerActivityID = reqObj.ManagerActivityID,
                    AtomPipelineStatus = DbAtomPipelineStatusEnum.Opened
                };
                var coEmpPplDbAddRspObj = await this._coEmpPipelineDb.AddAsync(coEmpPplDbAddReqObj);
                if (coEmpPplDbAddRspObj == default)
                {
                    errorDataList.Add(item);

                    this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                    continue;
                }

                // db, 核心-員工-商機原始資料-資料庫-新增
                var coEmpPplOgnDbAddReqObj = new CoEmpPplOgnDbAddReqMdl()
                {
                    EmployeePipelineID = coEmpPplDbAddRspObj.EmployeePipelineID,
                    // 公司縣市
                    AtomCity = item.EdmCompanyAddress.ExtractCityFromAddress<DbAtomCityEnum>(),
                    // 公司名稱
                    ManagerCompanyName = item.EdmCompanyName,
                    // 公司電話
                    ManagerCompanyLocationTelephone = item.EdmCompanyPhone,
                    // 客戶主類別
                    ManagerCompanyMainKindName = item.EdmCompanyMainKind,
                    // 客戶子類別
                    ManagerCompanySubKindName = item.EdmCompanySubKind,
                    // 客戶營業地點狀態
                    ManagerCompanyLocationStatus = DbAtomManagerCompanyStatusEnum.Unclear,
                    // 窗口姓名
                    ManagerContacterName = contacterName,
                    // 窗口Email
                    ManagerContacterEmail = item.EdmContacterEmail,
                    // 窗口手機
                    ManagerContacterPhone = item.EdmContacterPhone,
                    // 窗口電話
                    ManagerContacterTelephone = item.EdmContacterTelephone,
                    // 窗口部門
                    ManagerContacterDepartment = item.EdmContacterDepartment,
                    // 窗口職稱
                    ManagerContacterJobTitle = item.EdmContacterJobTitle,
                    // 窗口狀態
                    ManagerContacterStatus = DbAtomManagerContacterStatusEnum.Unknown,
                    // 窗口開發評等
                    AtomRatingKind = DbAtomManagerContacterRatingKindEnum.Whitelist
                };
                var coEmpPplOgnDbAddRspObj = await this._coEmpPipelineOriginalDb.AddAsync(coEmpPplOgnDbAddReqObj);
                if (coEmpPplOgnDbAddRspObj == default)
                {
                    errorDataList.Add(item);

                    // db, 核心-員工-商機-資料庫-移除
                    var coEmpPplDbRemoveReqObj = new CoEmpPplDbRemoveReqMdl()
                    {
                        EmployeePipelineID = coEmpPplDbAddRspObj.EmployeePipelineID
                    };
                    var coEmpPplDbRemoveRspObj = await this._coEmpPipelineDb.RemoveAsync(coEmpPplDbRemoveReqObj);
                    if (coEmpPplDbRemoveRspObj == default)
                    {
                        this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                        continue;
                    }

                    this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                    continue;
                }
            }
        }

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        rspObj.ErrorDataList = errorDataList;
        return rspObj;
    }

    /// <summary>管理者後台-CRM-活動管理-邏輯服務-匯入eDM-DataRow to RspItem </summary>
    private MbsCrmActLgcImportEdmRspItemMdl ConvertEdmDataRowToRspItem(DataRow reqObj)
    {
        try
        {
            if (reqObj == null)
            {
                return null;
            }

            return new MbsCrmActLgcImportEdmRspItemMdl
            {
                EdmContacterEmail = reqObj[0]?.ToString().Trim(),
                EdmFirstName = reqObj[1]?.ToString().Trim(),
                EdmLastName = reqObj[2]?.ToString().Trim(),
                EdmContacterTelephone = reqObj[3]?.ToString().Trim(),
                EdmContacterPhone = reqObj[4]?.ToString().Trim(),
                EdmCompanyName = reqObj[5]?.ToString().Trim(),
                EdmContacterJobTitle = reqObj[6]?.ToString().Trim(),
                EdmCompanyPhone = reqObj[7]?.ToString().Trim(),
                EdmCompanyFax = reqObj[8]?.ToString().Trim(),
                EdmCompanyAddress = reqObj[9]?.ToString().Trim(),
                EdmRemark = reqObj[10]?.ToString().Trim(),
                EdmContacterDepartment = reqObj[11]?.ToString().Trim(),
                EdmCompanyMainKind = reqObj[12]?.ToString().Trim(),
                EdmCompanySubKind = reqObj[13]?.ToString().Trim(),
                EdmAccountSales = reqObj[14]?.ToString().Trim(),
                EdmRegion = reqObj[15]?.ToString().Trim(),
                EdmCreatedDate = reqObj[16]?.ToString().Trim(),
                EdmDevice = reqObj[17]?.ToString().Trim()
            };
        }
        catch (Exception ex)
        {
            this._logger.LogError(
                $"reqObj: {JsonSerializer.Serialize(reqObj)}" +
                $", ex: {ex?.Message}" +
                $", ex.InnerException: {ex?.InnerException?.Message}");
            return null;
        }
    }

    #endregion

    #region Teams

    /// <summary>管理者後台-CRM-活動管理-邏輯服務-下載Teams範本</summary>
    public async Task<MbsCrmActLgcDownloadTeamsTemplateRspMdl> DownloadTeamsTemplateAsync(MbsCrmActLgcDownloadTeamsTemplateReqMdl reqObj)
    {
        var rspObj = new MbsCrmActLgcDownloadTeamsTemplateRspMdl
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

        // common, 通用-資料夾檔案-合併路徑
        var cmnFolderFileCombinePathReqObj = new CmnFolderFileCombinePathReqMdl()
        {
            PathList = new List<string>()
            {
                AppContext.BaseDirectory,
                "File",
                "TeamsSample.xlsx",
            },
        };
        var cmnFolderFileCombinePathRspObj = this._cmnFolderFile.CombinePath(cmnFolderFileCombinePathReqObj);
        if (cmnFolderFileCombinePathRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        rspObj.LocalAbsoluteFileName = cmnFolderFileCombinePathRspObj.ResultPath;
        rspObj.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        rspObj.FileName = "TeamsSample.xlsx";
        return rspObj;
    }

    /// <summary>管理者後台-CRM-活動管理-邏輯服務-匯入Teams</summary>
    public async Task<MbsCrmActLgcImportTeamsRspMdl> ImportTeamsAsync(MbsCrmActLgcImportTeamsReqMdl reqObj)
    {
        var rspObj = new MbsCrmActLgcImportTeamsRspMdl
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

        // common, 通用-Excel-讀取ByIFormFile
        var cmnExcelReadXlsxByIFormFilReqObj = new CmnExcelReadXlsxByIFormFilReqMdl()
        {
            FormFile = reqObj.TeamsFile,
        };
        var cmnExcelReadXlsxByIFormFilRspObj = this._cmnExcel.ReadXlsxByIFormFile(cmnExcelReadXlsxByIFormFilReqObj);
        if (cmnExcelReadXlsxByIFormFilRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            rspObj.ErrorCode = MbsErrorCodeEnum.FileFormatError;
            return rspObj;
        }

        // 判斷欄位是否一致
        if (cmnExcelReadXlsxByIFormFilRspObj.DataTable.Columns.Count != 19)
        {
            // 不一致
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            rspObj.ErrorCode = MbsErrorCodeEnum.FileFormatError;
            return rspObj;
        }

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

        /* 規格
        以下欄位都以方式處理 

        -若沒資料，=> 新增 
        -若有資料，欄位空白 => 更新 
        -若有資料，欄位有值 => 不處理 
    
        功能需求： 
        - 需要將不處理或有問題的資料回傳給前端顯示出來 
        - 匯入資料部分，跑迴圈處理, 將每一筆資料進行檢查,失敗的紀錄並回傳錯誤列表
        */

        var errorDataList = new List<MbsCrmActLgcImportTeamsRspItemMdl>();
        // 沒資料需要匯入
        var excelDataRowList = cmnExcelReadXlsxByIFormFilRspObj.DataTable
                        .AsEnumerable()
                        .Skip(1); // 跳過表頭
        if (excelDataRowList != null && excelDataRowList.Count() == 0)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            rspObj.ErrorCode = MbsErrorCodeEnum.FileContentError;
            return rspObj;
        }

        // db, 核心-員工-商機原始資料-資料庫-取得多筆從[Email列表]
        var coEmpPplOgnDbGetManyByEmailListReqObj = new CoEmpPplOgnDbGetManyByEmailListReqMdl()
        {
            ManagerActivityID = reqObj.ManagerActivityID,
            ManagerContacterEmailList = excelDataRowList
                                        .Where(row => !string.IsNullOrWhiteSpace(row[9]?.ToString().Trim()))
                                        .Select(row => row[9]?.ToString().Trim())
                                        .ToList()
        };
        var coEmpPplOgnDbGetManyByEmailListRspObj = await this._coEmpPipelineOriginalDb.GetManyByEmailListAsync(coEmpPplOgnDbGetManyByEmailListReqObj);
        if (coEmpPplOgnDbGetManyByEmailListRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        foreach (var excelDataRowItem in excelDataRowList)
        {
            var item = this.ConvertTeamsDataRowToRspItem(excelDataRowItem);
            if (item == null)
            {
                errorDataList.Add(item);
                continue;
            }

            // 檢查email (@bccs.com)的都不處理
            if (!string.IsNullOrWhiteSpace(item.TeamsContacterRegisterEmail) &&
                item.TeamsContacterRegisterEmail.Contains("@bccs.com"))
            {
                continue;
            }

            // 檢查 公司名稱or姓名or電子信箱 其中一個沒值則不處理,並回傳錯誤的資料列表
            var contacterName = item.TeamsContacterRegisterFirstName?.Trim() + item.TeamsContacterRegisterLastName?.Trim();
            if (string.IsNullOrWhiteSpace(item.TeamsCompanyName?.Trim()) ||
                string.IsNullOrWhiteSpace(contacterName) ||
                string.IsNullOrWhiteSpace(item.TeamsContacterRegisterEmail?.Trim()))
            {
                errorDataList.Add(item);
                continue;
            }

            // 透過excel email取得所有商機原始資料列表,判斷更新or新增
            var existingItem = coEmpPplOgnDbGetManyByEmailListRspObj.EmployeePipelineOriginalList
                                                        .FirstOrDefault(x => x.ManagerContacterEmail == item.TeamsContacterRegisterEmail);
            if (existingItem != null)
            {
                // 若為空資料或連字號或已開啟，則填入「已點擊」
                if (existingItem.AtomPipelineStatus == DbAtomPipelineStatusEnum.Hyphen ||
                    existingItem.AtomPipelineStatus == DbAtomPipelineStatusEnum.Opened)
                {
                    // db, 核心-員工-商機-資料庫-更新
                    var coEmpPplDbUpdateReqObj = new CoEmpPplDbUpdateReqMdl()
                    {
                        ManagerActivityID = reqObj.ManagerActivityID,
                        EmployeePipelineID = existingItem.EmployeePipelineID,
                        AtomPipelineStatus = DbAtomPipelineStatusEnum.Clicked
                    };
                    var coEmpPplDbUpdateRspObj = await this._coEmpPipelineDb.UpdateAsync(coEmpPplDbUpdateReqObj);
                    if (coEmpPplDbUpdateRspObj == default)
                    {
                        errorDataList.Add(item);

                        this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                        continue;
                    }
                }

                // db, 核心-員工-商機原始資料-資料庫-更新
                var coEmpPplOgnDbUpdateReqObj = new CoEmpPplOgnDbUpdateReqMdl
                {
                    // 檢查欄位有沒有值, 欄位空白 => 更新, 欄位有值 => 不處理         
                    // 公司縣市
                    //AtomCity = null,
                    // 公司名稱                    
                    ManagerCompanyName = string.IsNullOrWhiteSpace(existingItem.ManagerCompanyName) == false
                                        ? item.TeamsCompanyName
                                        : null,
                    // 公司電話
                    ManagerCompanyLocationTelephone = string.IsNullOrWhiteSpace(existingItem.ManagerCompanyLocationTelephone) == false
                                                        ? item.TeamsCompanyTelephone
                                                        : null,
                    // 客戶主類別
                    //ManagerCompanyMainKindName = null,
                    // 客戶子類別
                    //ManagerCompanySubKindName = null,
                    // 客戶營業地點狀態
                    //ManagerCompanyLocationStatus = null,
                    // 窗口姓名
                    ManagerContacterName = string.IsNullOrWhiteSpace(existingItem.ManagerContacterName) == false
                                        ? contacterName
                                        : null,
                    // 窗口Email
                    ManagerContacterEmail = string.IsNullOrWhiteSpace(existingItem.ManagerContacterEmail) == false
                                        ? item.TeamsContacterRegisterEmail
                                        : null,
                    // 窗口手機
                    ManagerContacterPhone = string.IsNullOrWhiteSpace(existingItem.ManagerContacterPhone) == false
                                        ? item.TeamsContacterPhone
                                        : null,
                    // 窗口電話
                    ManagerContacterTelephone = string.IsNullOrWhiteSpace(existingItem.ManagerContacterTelephone) == false
                                                        ? item.TeamsCompanyTelephone
                                                        : null,
                    // 窗口部門
                    ManagerContacterDepartment = string.IsNullOrWhiteSpace(existingItem.ManagerContacterDepartment) == false
                                        ? item.TeamsContacterDepartment
                                        : null,
                    // 窗口職稱
                    ManagerContacterJobTitle = string.IsNullOrWhiteSpace(existingItem.ManagerContacterJobTitle) == false
                                        ? item.TeamsContacterJobTitle
                                        : null,
                    // 窗口是否個資同意
                    //ManagerContacterIsConsent = existingItem.ManagerContacterIsConsent == null
                    //                  ? item.TeamsContacterIsConsent
                    //                  : null,
                    // 窗口狀態
                    //ManagerContacterStatus = DbAtomManagerContacterStatusEnum.Unknown,
                    // Teams註冊狀態
                    TeamsRegistrationStatus = string.IsNullOrWhiteSpace(existingItem.TeamsRegistrationStatus) == false
                                        ? item.TeamsRegistrationStatus
                                        : null,
                    // Teams註冊時間
                    TeamsRegistrationTime = existingItem.TeamsRegistrationTime.HasValue == false
                                        ? item.TeamsRegistrationTime
                                        : null,
                    // Teams上次離開時間
                    TeamsLastLeaveTime = existingItem.TeamsLastLeaveTime.HasValue == false
                                        ? item.TeamsLastLeaveTime
                                        : null,
                    // Teams首次加入時間
                    TeamsFirstJoinTime = existingItem.TeamsFirstJoinTime.HasValue == false
                                        ? item.TeamsFirstJoinTime
                                        : null,
                    // Teams會議持續時間
                    TeamsMeetingDuration = string.IsNullOrWhiteSpace(existingItem.TeamsMeetingDuration) == false
                                        ? item.TeamsMeetingDuration
                                        : null,
                    // Teams角色
                    TeamsRole = string.IsNullOrWhiteSpace(existingItem.TeamsRole) == false
                                        ? item.TeamsRole
                                        : null
                };
                var coEmpPplOgnDbUpdateRspObj = await this._coEmpPipelineOriginalDb.UpdateAsync(coEmpPplOgnDbUpdateReqObj);
                if (coEmpPplOgnDbUpdateRspObj == default)
                {
                    errorDataList.Add(item);

                    this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                    continue;
                }
            }
            else
            {
                // db, 核心-員工-商機-資料庫-新增
                var coEmpPplDbAddReqObj = new CoEmpPplDbAddReqMdl()
                {
                    ManagerActivityID = reqObj.ManagerActivityID,
                    AtomPipelineStatus = DbAtomPipelineStatusEnum.Clicked
                };
                var coEmpPplDbAddRspObj = await this._coEmpPipelineDb.AddAsync(coEmpPplDbAddReqObj);
                if (coEmpPplDbAddRspObj == default)
                {
                    errorDataList.Add(item);

                    this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                    continue;
                }

                // db, 核心-員工-商機原始資料-資料庫-新增
                var coEmpPplOgnDbAddReqObj = new CoEmpPplOgnDbAddReqMdl()
                {
                    EmployeePipelineID = coEmpPplDbAddRspObj.EmployeePipelineID,
                    // 公司縣市
                    AtomCity = null,
                    // 公司名稱
                    ManagerCompanyName = item.TeamsCompanyName,
                    // 公司電話
                    ManagerCompanyLocationTelephone = item.TeamsCompanyTelephone,
                    // 客戶主類別
                    ManagerCompanyMainKindName = null,
                    // 客戶子類別
                    ManagerCompanySubKindName = null,
                    // 客戶營業地點狀態
                    ManagerCompanyLocationStatus = DbAtomManagerCompanyStatusEnum.Unclear,
                    // 窗口姓名
                    ManagerContacterName = contacterName,
                    // 窗口Email
                    ManagerContacterEmail = item.TeamsContacterRegisterEmail,
                    // 窗口手機
                    ManagerContacterPhone = item.TeamsContacterPhone,
                    // 窗口電話
                    ManagerContacterTelephone = item.TeamsCompanyTelephone,
                    // 窗口部門
                    ManagerContacterDepartment = item.TeamsContacterDepartment,
                    // 窗口職稱
                    ManagerContacterJobTitle = item.TeamsContacterJobTitle,
                    // 窗口是否個資同意
                    ManagerContacterIsConsent = item.TeamsContacterIsConsent,
                    // 窗口狀態
                    ManagerContacterStatus = DbAtomManagerContacterStatusEnum.Unknown,
                    // 窗口開發評等
                    AtomRatingKind = DbAtomManagerContacterRatingKindEnum.Whitelist,
                    // Teams註冊狀態
                    TeamsRegistrationStatus = item.TeamsRegistrationStatus,
                    // Teams註冊時間
                    TeamsRegistrationTime = item.TeamsRegistrationTime,
                    // Teams上次離開時間
                    TeamsLastLeaveTime = item.TeamsLastLeaveTime,
                    // Teams首次加入時間
                    TeamsFirstJoinTime = item.TeamsFirstJoinTime,
                    // Teams會議持續時間
                    TeamsMeetingDuration = item.TeamsMeetingDuration,
                    // Teams角色
                    TeamsRole = item.TeamsRole,
                };
                var coEmpPplOgnDbAddRspObj = await this._coEmpPipelineOriginalDb.AddAsync(coEmpPplOgnDbAddReqObj);
                if (coEmpPplOgnDbAddRspObj == default)
                {
                    errorDataList.Add(item);

                    // db, 核心-員工-商機-資料庫-移除
                    var coEmpPplDbRemoveReqObj = new CoEmpPplDbRemoveReqMdl()
                    {
                        EmployeePipelineID = coEmpPplDbAddRspObj.EmployeePipelineID
                    };
                    var coEmpPplDbRemoveRspObj = await this._coEmpPipelineDb.RemoveAsync(coEmpPplDbRemoveReqObj);
                    if (coEmpPplDbRemoveRspObj == default)
                    {
                        this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                        continue;
                    }

                    this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
                    continue;
                }
            }
        }

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        rspObj.ErrorDataList = errorDataList;
        return rspObj;
    }

    /// <summary>管理者後台-CRM-活動管理-邏輯服務-匯入Teams-DataRow to RspItem </summary>
    private MbsCrmActLgcImportTeamsRspItemMdl ConvertTeamsDataRowToRspItem(DataRow reqObj)
    {
        try
        {
            if (reqObj == null)
            {
                return null;
            }

            return new MbsCrmActLgcImportTeamsRspItemMdl
            {
                TeamsName = reqObj[0]?.ToString().Trim(),
                TeamsFirstJoinTime = string.IsNullOrWhiteSpace(reqObj[1]?.ToString().Trim())
                                                ? null : DateTimeOffset.TryParse(reqObj[1]?.ToString().Trim(), out var teamsFirstJoinTime)
                                                ? teamsFirstJoinTime.ToUniversalTime() : null,
                TeamsLastLeaveTime = string.IsNullOrWhiteSpace(reqObj[2]?.ToString().Trim())
                                                ? null : DateTimeOffset.TryParse(reqObj[2]?.ToString().Trim(), out var teamsLastLeaveTime)
                                                ? teamsLastLeaveTime.ToUniversalTime() : null,
                TeamsMeetingDuration = reqObj[3]?.ToString().Trim(),
                TeamsEmail = reqObj[4]?.ToString().Trim(),
                TeamsParticipantId = reqObj[5]?.ToString().Trim(),
                TeamsRole = reqObj[6]?.ToString().Trim(),
                TeamsContacterRegisterLastName = reqObj[7]?.ToString().Trim(),
                TeamsContacterRegisterFirstName = reqObj[8]?.ToString().Trim(),
                TeamsContacterRegisterEmail = reqObj[9]?.ToString().Trim(),
                TeamsRegistrationTime = string.IsNullOrWhiteSpace(reqObj[10]?.ToString().Trim())
                                                ? null : DateTimeOffset.TryParse(reqObj[10]?.ToString().Trim(), out var teamsRegistrationTime)
                                                ? teamsRegistrationTime.ToUniversalTime() : null,
                TeamsRegistrationStatus = reqObj[11]?.ToString().Trim(),
                TeamsCompanyName = reqObj[12]?.ToString().Trim(),
                TeamsContacterDepartment = reqObj[13]?.ToString().Trim(),
                TeamsContacterJobTitle = reqObj[14]?.ToString().Trim(),
                TeamsCompanyTelephone = reqObj[15]?.ToString().Trim(),
                TeamsContacterPhone = reqObj[16]?.ToString().Trim(),
                TeamsActivityInfoSource = reqObj[17]?.ToString().Trim(),
                TeamsContacterIsConsent = string.IsNullOrWhiteSpace(reqObj[18]?.ToString().Trim())
                                                ? false : reqObj[18]?.ToString().Trim().ToLower() == "yes"
            };
        }
        catch (Exception ex)
        {
            this._logger.LogError(
                $"reqObj: {JsonSerializer.Serialize(reqObj)}" +
                $", ex: {ex?.Message}" +
                $", ex.InnerException: {ex?.InnerException?.Message}");
            return null;
        }
    }

    #endregion
}