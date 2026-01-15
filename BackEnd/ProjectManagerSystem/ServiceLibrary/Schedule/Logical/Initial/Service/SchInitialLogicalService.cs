using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataModelLibrary.Database.AtomEmployeeRange;
using DataModelLibrary.Database.AtomManagerCompany;
using DataModelLibrary.Database.AtomMenu;
using DataModelLibrary.Database.AtomPermissionKind;
using DataModelLibrary.Database.AtomSecurityGrade;
using DataModelLibrary.Database.Employee;
using DataModelLibrary.Database.ManagerCompany;
using DataModelLibrary.Database.ManagerCompanyMainKind;
using DataModelLibrary.Database.ManagerCompanySubKind;
using DataModelLibrary.Database.ManagerDepartment;
using DataModelLibrary.Database.ManagerRegion;
using DataModelLibrary.Database.ManagerRole;
using Microsoft.Extensions.Logging;
using ServiceLibrary.Core.Employee.DB.Info.Format;
using ServiceLibrary.Core.Employee.DB.Info.Service;
using ServiceLibrary.Core.Employee.DB.Permission.Format;
using ServiceLibrary.Core.Employee.DB.Permission.Service;
using ServiceLibrary.Core.Employee.Logical.Format;
using ServiceLibrary.Core.Employee.Logical.Service;
using ServiceLibrary.Core.Manager.DB.Company.Format;
using ServiceLibrary.Core.Manager.DB.Company.Service;
using ServiceLibrary.Core.Manager.DB.CompanyMainKind.Format;
using ServiceLibrary.Core.Manager.DB.CompanyMainKind.Service;
using ServiceLibrary.Core.Manager.DB.CompanySubKind.Format;
using ServiceLibrary.Core.Manager.DB.CompanySubKind.Service;
using ServiceLibrary.Core.Manager.DB.Department.Format;
using ServiceLibrary.Core.Manager.DB.Department.Service;
using ServiceLibrary.Core.Manager.DB.Region.Format;
using ServiceLibrary.Core.Manager.DB.Region.Service;
using ServiceLibrary.Core.Manager.DB.Role.Format;
using ServiceLibrary.Core.Manager.DB.Role.Service;
using ServiceLibrary.Core.Manager.DB.RolePermission.Format;
using ServiceLibrary.Core.Manager.DB.RolePermission.Service;

namespace ServiceLibrary.Schedule.Logical.Initial.Service;

/// <summary>排程-初始化-邏輯服務</summary>
public class SchInitialLogicalService : ISchInitialLogicalService
{
    /// <summary>logger</summary>
    private readonly ILogger<SchInitialLogicalService> _logger;

    #region Core Manager

    /// <summary>核心-管理者-公司-資料庫服務</summary>
    private readonly ICoMgrCompanyDbService _coMgrCompanyDb;

    /// <summary>核心-管理者-公司主分類-資料庫服務</summary>
    private readonly ICoMgrCompanyMainKindDbService _coMgrCompanyMainKindDb;

    /// <summary>核心-管理者-公司子分類-資料庫服務</summary>
    private readonly ICoMgrCompanySubKindDbService _coMgrCompanySubKindDb;

    /// <summary>核心-管理者-部門-資料庫服務</summary>
    private readonly ICoMgrDepartmentDbService _coMgrDepartmentDb;

    /// <summary>核心-管理者-地區-資料庫服務</summary>
    private readonly ICoMgrRegionDbService _coMgrRegionDb;

    /// <summary>核心-管理者-角色-資料庫服務</summary>
    private readonly ICoMgrRoleDbService _coMgrRoleDb;

    /// <summary>核心-管理者-角色權限-資料庫服務</summary>
    private readonly ICoMgrRolePermissionDbService _coMgrRolePermissionDb;


    #endregion

    #region Core Employee

    /// <summary>核心-員工-資訊-資料庫服務</summary>
    private readonly ICoEmpInfoDbService _coEmpInfoDb;

    /// <summary>核心-員工-邏輯服務</summary>
    private readonly ICoEmployeeLogicalService _coEmpLogical;

    /// <summary>核心-員工-目錄權限-資料庫服務</summary>
    private readonly ICoEmpPermissionDbService _coEmpPermissionDb;

    #endregion

    /// <summary>建構式</summary>
    public SchInitialLogicalService(
        ILogger<SchInitialLogicalService> logger,
        // Core Manager
        ICoMgrCompanyDbService coMgrCompanyDb,
        ICoMgrCompanyMainKindDbService coMgrCompanyMainKindDb,
        ICoMgrCompanySubKindDbService coMgrCompanySubKindDb,
        ICoMgrDepartmentDbService coMgrDepartmentDb,
        ICoMgrRegionDbService coMgrRegionDb,
        ICoMgrRoleDbService coMgrRoleDb,
        ICoMgrRolePermissionDbService coMgrRolePermissionDb,
        // Core Employee
        ICoEmpInfoDbService coEmpInfoDb,
        ICoEmployeeLogicalService coEmpLogical,
        ICoEmpPermissionDbService coEmpPermissionDb)
    {
        this._logger = logger;
        // Core Manager
        this._coMgrCompanyDb = coMgrCompanyDb;
        this._coMgrCompanyMainKindDb = coMgrCompanyMainKindDb;
        this._coMgrCompanySubKindDb = coMgrCompanySubKindDb;
        this._coMgrDepartmentDb = coMgrDepartmentDb;
        this._coMgrRegionDb = coMgrRegionDb;
        this._coMgrRoleDb = coMgrRoleDb;
        this._coMgrRolePermissionDb = coMgrRolePermissionDb;
        // Core Employee
        this._coEmpInfoDb = coEmpInfoDb;
        this._coEmpLogical = coEmpLogical;
        this._coEmpPermissionDb = coEmpPermissionDb;
    }

    /// <summary>排程-初始化-邏輯服務-開始任務</summary>
    public async Task StartJobAsync()
    {
        Console.WriteLine("StartJob");
        await Task.CompletedTask;
    }

    /// <summary>排程-初始化-邏輯服務-初始化地區</summary>
    public async Task InitializeRegionAsync()
    {
        #region 全區

        // db, 核心-管理者-地區-資料庫-是否存在
        var coMgrRgnDbExist01ReqObj = new CoMgrRgnDbExistReqMdl()
        {
            ManagerRegionID = DbManagerRegionConst.AllRegion.ID,
        };
        var coMgrRgnDbExist01RspObj = await this._coMgrRegionDb.ExistAsync(coMgrRgnDbExist01ReqObj);
        if (coMgrRgnDbExist01RspObj == default)
        {
            this._logger.LogError(string.Empty);
            return;
        }

        // 沒有的話，新增
        if (coMgrRgnDbExist01RspObj.IsExist == false)
        {
            // db, 核心-管理者-地區-資料庫-新增[指定ID]
            var coMgrRgnDbAddWithId01ReqObj = new CoMgrRgnDbAddWithIdReqMdl
            {
                ManagerRegionId = DbManagerRegionConst.AllRegion.ID,
                ManagerRegionName = DbManagerRegionConst.AllRegion.Name,
                ManagerRegionIsEnable = DbManagerRegionConst.AllRegion.IsEnabled
            };
            var coMgrRgnDbAddWithId01RspObj = await this._coMgrRegionDb.AddWithIdAsync(coMgrRgnDbAddWithId01ReqObj);
            if (coMgrRgnDbAddWithId01RspObj == default)
            {
                this._logger.LogError(string.Empty);
                return;
            }

        }

        #endregion

        #region 無權限

        // db, 核心-管理者-地區-資料庫-是否存在
        var coMgrRgnDbExist02ReqObj = new CoMgrRgnDbExistReqMdl()
        {
            ManagerRegionID = DbManagerRegionConst.Denied.ID,
        };
        var coMgrRgnDbExist02RspObj = await this._coMgrRegionDb.ExistAsync(coMgrRgnDbExist02ReqObj);
        if (coMgrRgnDbExist02RspObj == default)
        {
            this._logger.LogError(string.Empty);
            return;
        }

        // 沒有的話，新增
        if (coMgrRgnDbExist02RspObj.IsExist == false)
        {
            // db, 核心-管理者-地區-資料庫-新增[指定ID] 無權限
            var coMgrRgnDbAddWithId02ReqObj = new CoMgrRgnDbAddWithIdReqMdl
            {
                ManagerRegionId = DbManagerRegionConst.Denied.ID,
                ManagerRegionName = DbManagerRegionConst.Denied.Name,
                ManagerRegionIsEnable = DbManagerRegionConst.Denied.IsEnabled
            };
            var coMgrRgnDbAddWithId02RspObj = await this._coMgrRegionDb.AddWithIdAsync(coMgrRgnDbAddWithId02ReqObj);
            if (coMgrRgnDbAddWithId02RspObj == default)
            {
                this._logger.LogError(string.Empty);
                return;
            }
        }

        #endregion

    }

    /// <summary>排程-初始化-邏輯服務-初始化部門</summary>
    public async Task InitializeDepartmentAsync()
    {
        // db, 核心-管理者-部門-資料庫-是否存在
        var coMgrDptDbExistReqObj = new CoMgrDptDbExistReqMdl()
        {
            ManagerDepartmentID = DbManagerDepartmentConst.GeneralManagerOffice.ID,
        };
        var coMgrDptDbExistRspObj = await this._coMgrDepartmentDb.ExistAsync(coMgrDptDbExistReqObj);
        if (coMgrDptDbExistRspObj == default)
        {
            this._logger.LogError(string.Empty);
            return;
        }

        // 沒有的話，新增
        if (coMgrDptDbExistRspObj.IsExist == false)
        {
            // db, 核心-管理者-部門-資料庫-新增[指定ID]
            var coMgrDptDbAddWithIdPresidentReqObj = new CoMgrDptDbAddWithIdReqMdl
            {
                ManagerDepartmentId = DbManagerDepartmentConst.GeneralManagerOffice.ID,
                ManagerDepartmentName = DbManagerDepartmentConst.GeneralManagerOffice.Name,
                ManagerDepartmentIsEnable = DbManagerDepartmentConst.GeneralManagerOffice.IsEnabled
            };
            var coMgrDptDbAddWithIdPresidentRspObj = await this._coMgrDepartmentDb.AddWithIdAsync(coMgrDptDbAddWithIdPresidentReqObj);
            if (coMgrDptDbAddWithIdPresidentRspObj == default)
            {
                this._logger.LogError(string.Empty);
                return;
            }
        }
    }

    /// <summary>排程-初始化-邏輯服務-初始化公司-漢昕</summary>
    public async Task InitializeCompanyBccsAsync()
    {
        // db, 核心-管理者-公司主分類-資料庫-是否存在
        var coMgrCmpMainKdDbGetManyNameReqObj = new CoMgrCmpMainKdDbGetManyNameReqMdl()
        {
            ManagerCompanyMainKindIdList = new List<int>()
            {
                DbManagerCompanyMainKindConst.BCCS.ID,
            },
        };
        var coMgrCmpMainKdDbGetManyNameRspObj = await this._coMgrCompanyMainKindDb.GetManyNameAsync(coMgrCmpMainKdDbGetManyNameReqObj);
        if (coMgrCmpMainKdDbGetManyNameRspObj == default)
        {
            this._logger.LogError(string.Empty);
            return;
        }

        // 沒有 主類別的話，新增
        if (coMgrCmpMainKdDbGetManyNameRspObj.ManagerCompanyMainKindList.Any() == false)
        {
            // db, 核心-管理者-公司主分類-資料庫-新增[指定ID]
            var coMgrCmpMainKdDbAddWithIdReqObj = new CoMgrCmpMainKdDbAddWithIdReqMdl
            {
                ManagerCompanyMainKindId = DbManagerCompanyMainKindConst.BCCS.ID,
                ManagerCompanyMainKindName = DbManagerCompanyMainKindConst.BCCS.Name,
                ManagerCompanyMainKindIsEnable = DbManagerCompanyMainKindConst.BCCS.IsEnabled
            };
            var coMgrCmpMainKdDbAddWithIdRspObj = await this._coMgrCompanyMainKindDb.AddWithIdAsync(coMgrCmpMainKdDbAddWithIdReqObj);
            if (coMgrCmpMainKdDbAddWithIdRspObj == default)
            {
                this._logger.LogError(string.Empty);
                return;
            }
        }

        // db, 核心-管理者-公司子分類-資料庫-是否存在
        var coMgrCmpSubKdDbGetManyNameReqObj = new CoMgrCmpSubKdDbGetManyNameReqMdl()
        {
            ManagerCompanySubKindIdList = new List<int>()
            {
                DbManagerCompanySubKindConst.BCCS.ID,
            },
        };
        var coMgrCmpSubKdDbGetManyNameRspObj = await this._coMgrCompanySubKindDb.GetManyNameAsync(coMgrCmpSubKdDbGetManyNameReqObj);
        if (coMgrCmpSubKdDbGetManyNameRspObj == default)
        {
            this._logger.LogError(string.Empty);
            return;
        }

        // 沒有 子類別的話，新增
        if (coMgrCmpSubKdDbGetManyNameRspObj.ManagerCompanySubKindList.Any() == false)
        {
            // db, 核心-管理者-公司子分類-資料庫-新增[指定ID]
            var coMgrCmpSubKdDbAddWithIdReqObj = new CoMgrCmpSubKdDbAddWithIdReqMdl
            {
                ManagerCompanySubKindId = DbManagerCompanySubKindConst.BCCS.ID,
                ManagerCompanySubKindName = DbManagerCompanySubKindConst.BCCS.Name,
                ManagerCompanyMainKindID = DbManagerCompanySubKindConst.BCCS.ManagerCompanyMainKindID,
                ManagerCompanySubKindIsEnable = DbManagerCompanySubKindConst.BCCS.IsEnabled
            };
            var coMgrCmpSubKdDbAddWithIdRspObj = await this._coMgrCompanySubKindDb.AddWithIdAsync(coMgrCmpSubKdDbAddWithIdReqObj);
            if (coMgrCmpSubKdDbAddWithIdRspObj == default)
            {
                this._logger.LogError(string.Empty);
                return;
            }
        }

        // db, 核心-管理者-公司-資料庫-統一編號是否存在
        var coMgrComDbUniNumExistReqObj = new CoMgrComDbUniNumExistReqMdl()
        {
            UnifiedNumber = DbManagerCompanyConst.BCCS.UnifiedNumber,
        };
        var coMgrComDbUniNumExistRspObj = await this._coMgrCompanyDb.UniNumExistAsync(coMgrComDbUniNumExistReqObj);
        if (coMgrComDbUniNumExistRspObj == default)
        {
            this._logger.LogError(string.Empty);
            return;
        }

        // 沒有  公司 的話，新增
        if (coMgrComDbUniNumExistRspObj.IsExist == false)
        {
            // db, 核心-管理者-公司-資料庫-新增[指定ID]
            var coMgrCmpDbAddWithIdReqObj = new CoMgrCmpDbAddWithIdReqMdl
            {
                ManagerCompanyID = DbManagerCompanyConst.BCCS.ID,
                ManagerCompanyMainKindID = DbManagerCompanyConst.BCCS.ManagerCompanyMainKindID,
                ManagerCompanySubKindID = DbManagerCompanyConst.BCCS.ManagerCompanySubKindID,
                ManagerCompanyUnifiedNumber = DbManagerCompanyConst.BCCS.UnifiedNumber,
                ManagerCompanyName = DbManagerCompanyConst.BCCS.Name,
                AtomManagerCompanyStatus = DbManagerCompanyConst.BCCS.StatusEnum,
                AtomCustomerGrade = DbManagerCompanyConst.BCCS.CustomerGradeEnum,
                AtomSecurityGrade = DbManagerCompanyConst.BCCS.SecurityGradeEnum,
                AtomEmployeeRange = DbManagerCompanyConst.BCCS.EmployeeRangeEnum,
            };
            var coMgrCmpDbAddWithIdRspObj = await this._coMgrCompanyDb.AddWithIdAsync(coMgrCmpDbAddWithIdReqObj);
            if (coMgrCmpDbAddWithIdRspObj == default)
            {
                this._logger.LogError(string.Empty);
                return;
            }
        }
    }

    /// <summary>排程-初始化-邏輯服務-初始化員工-Admin</summary>
    public async Task InitializeEmployeeAdminAsync()
    {
        #region 部門 

        // db, 核心-管理者-部門-資料庫-是否存在
        var coMgrDptDbExistReqObj = new CoMgrDptDbExistReqMdl()
        {
            ManagerDepartmentID = DbManagerDepartmentConst.Admin.ID,
        };
        var coMgrDptDbExistRspObj = await this._coMgrDepartmentDb.ExistAsync(coMgrDptDbExistReqObj);
        if (coMgrDptDbExistRspObj == default)
        {
            this._logger.LogError(string.Empty);
            return;
        }

        // 判斷是否存在
        if (coMgrDptDbExistRspObj.IsExist == false)
        {
            // 不存在，新增

            // db, 核心-管理者-部門-資料庫-新增[指定ID]
            var adminDptDbAddWithIdAdminReqObj = new CoMgrDptDbAddWithIdReqMdl
            {
                ManagerDepartmentId = DbManagerDepartmentConst.Admin.ID,
                ManagerDepartmentName = DbManagerDepartmentConst.Admin.Name,
                ManagerDepartmentIsEnable = DbManagerDepartmentConst.Admin.IsEnabled
            };
            var adminDptDbAddWithIdAdminRspObj = await this._coMgrDepartmentDb.AddWithIdAsync(adminDptDbAddWithIdAdminReqObj);
            if (adminDptDbAddWithIdAdminRspObj == default)
            {
                this._logger.LogError(string.Empty);
                return;
            }

        }

        #endregion

        #region 角色

        // db, 核心-管理者-角色-資料庫-是否存在
        var coMgrRolDbExistReqObj = new CoMgrRolDbExistReqMdl()
        {
            ManagerRoleID = DbManagerRoleConst.Admin.ID,
        };
        var coMgrRolDbExistRspObj = await this._coMgrRoleDb.ExistAsync(coMgrRolDbExistReqObj);
        if (coMgrRolDbExistRspObj == default)
        {
            this._logger.LogError(string.Empty);
            return;
        }

        // 判斷是否存在
        if (coMgrRolDbExistRspObj.IsExist == false)
        {
            // 不存在，新增

            // db, 核心-管理者-角色-資料庫-新增[指定ID]
            var coMgrRolDbAddWithIdReqObj = new CoMgrRolDbAddWithIdReqMdl
            {
                ManagerRoleId = DbManagerRoleConst.Admin.ID,
                ManagerRoleName = DbManagerRoleConst.Admin.Name,
                ManagerRegionID = DbManagerRoleConst.Admin.ManagerRegionID,
                ManagerDepartmentID = DbManagerRoleConst.Admin.ManagerDepartmentID,
                ManagerRoleRemark = DbManagerRoleConst.Admin.Remark,
                ManagerRoleIsEnable = DbManagerRoleConst.Admin.IsEnabled,
            };
            var coMgrRolDbAddWithIdRspObj = await this._coMgrRoleDb.AddWithIdAsync(coMgrRolDbAddWithIdReqObj);
            if (coMgrRolDbAddWithIdRspObj == default)
            {
                this._logger.LogError(string.Empty);
                return;
            }

            // db, 核心-管理者-角色權限-資料庫-新增多筆
            var coMgrRpDbAddManyReqObj = new CoMgrRpDbAddManyReqMdl
            {
                ManagerRolePermissionList = new List<CoMgrRpDbAddManyReqItemMdl>()
                {
                     #region ERP

                    // ERP_發票結案 
                    new CoMgrRpDbAddManyReqItemMdl()
                    {
                        ManagerRoleID = DbManagerRoleConst.Admin.ID,
                        AtomMenu  = DbAtomMenuEnum.ErpBill,
                        ManagerRegionID = DbManagerRegionConst.Denied.ID,
                        AtomPermissionKindIdView = DbAtomPermissionKindEnum.Everyone,
                        AtomPermissionKindIdCreate = DbAtomPermissionKindEnum.Everyone,
                        AtomPermissionKindIdEdit = DbAtomPermissionKindEnum.Everyone,
                        AtomPermissionKindIdDelete = DbAtomPermissionKindEnum.Everyone,
                    },
	                #endregion

                    #region 工作

                    // 工作_專案管理
                    new CoMgrRpDbAddManyReqItemMdl()
                    {
                        ManagerRoleID = DbManagerRoleConst.Admin.ID,
                        AtomMenu  = DbAtomMenuEnum.WorkProject,
                        ManagerRegionID = DbManagerRegionConst.Denied.ID,
                        AtomPermissionKindIdView = DbAtomPermissionKindEnum.Everyone,
                        AtomPermissionKindIdCreate = DbAtomPermissionKindEnum.Everyone,
                        AtomPermissionKindIdEdit = DbAtomPermissionKindEnum.Everyone,
                        AtomPermissionKindIdDelete = DbAtomPermissionKindEnum.Everyone
                    },

	                #endregion

                    #region CRM
                
                    // CRM_商機管理 
                    new CoMgrRpDbAddManyReqItemMdl()
                    {
                        ManagerRoleID = DbManagerRoleConst.Admin.ID,
                        AtomMenu  = DbAtomMenuEnum.CrmPipeline,
                        ManagerRegionID = DbManagerRegionConst.AllRegion.ID,
                        AtomPermissionKindIdView = DbAtomPermissionKindEnum.Everyone,
                        AtomPermissionKindIdCreate = DbAtomPermissionKindEnum.Everyone,
                        AtomPermissionKindIdEdit = DbAtomPermissionKindEnum.Everyone,
                        AtomPermissionKindIdDelete = DbAtomPermissionKindEnum.Everyone
                    },

                    // CRM_Booking
                    new CoMgrRpDbAddManyReqItemMdl()
                    {
                        ManagerRoleID = DbManagerRoleConst.Admin.ID,
                        AtomMenu = DbAtomMenuEnum.CrmBooking,
                        ManagerRegionID = DbManagerRegionConst.Denied.ID,
                        AtomPermissionKindIdView = DbAtomPermissionKindEnum.Everyone,
                        AtomPermissionKindIdCreate = DbAtomPermissionKindEnum.Denied,
                        AtomPermissionKindIdEdit = DbAtomPermissionKindEnum.Everyone,
                        AtomPermissionKindIdDelete = DbAtomPermissionKindEnum.Denied,
                    },
                
                    // CRM_電銷管理
                    new CoMgrRpDbAddManyReqItemMdl()
                    {
                        ManagerRoleID = DbManagerRoleConst.Admin.ID,
                        AtomMenu = DbAtomMenuEnum.CrmPhone,
                        ManagerRegionID = DbManagerRegionConst.Denied.ID,
                        AtomPermissionKindIdView = DbAtomPermissionKindEnum.Everyone,
                        AtomPermissionKindIdCreate = DbAtomPermissionKindEnum.Everyone,
                        AtomPermissionKindIdEdit = DbAtomPermissionKindEnum.Everyone,
                        AtomPermissionKindIdDelete = DbAtomPermissionKindEnum.Everyone
                    },

                    // CRM_電銷客戶管理
                    new CoMgrRpDbAddManyReqItemMdl()
                    {
                        ManagerRoleID = DbManagerRoleConst.Admin.ID,
                        AtomMenu = DbAtomMenuEnum.CrmPhoneSales,
                        ManagerRegionID = DbManagerRegionConst.Denied.ID,
                        AtomPermissionKindIdView = DbAtomPermissionKindEnum.Everyone,
                        AtomPermissionKindIdCreate = DbAtomPermissionKindEnum.Everyone,
                        AtomPermissionKindIdEdit = DbAtomPermissionKindEnum.Everyone,
                        AtomPermissionKindIdDelete = DbAtomPermissionKindEnum.Everyone
                    },

                    // CRM_活動管理
                    new CoMgrRpDbAddManyReqItemMdl()
                    {
                        ManagerRoleID = DbManagerRoleConst.Admin.ID,
                        AtomMenu = DbAtomMenuEnum.CrmActivity,
                        ManagerRegionID = DbManagerRegionConst.Denied.ID,
                        AtomPermissionKindIdView = DbAtomPermissionKindEnum.Everyone,
                        AtomPermissionKindIdCreate = DbAtomPermissionKindEnum.Everyone,
                        AtomPermissionKindIdEdit = DbAtomPermissionKindEnum.Everyone,
                        AtomPermissionKindIdDelete = DbAtomPermissionKindEnum.Everyone
                    },

                    #endregion

                    #region 報表分析
                
                    // 報表分析_產品同比
                    new CoMgrRpDbAddManyReqItemMdl()
                    {
                        ManagerRoleID = DbManagerRoleConst.Admin.ID,
                        AtomMenu = DbAtomMenuEnum.ReportProductPeriod,
                        ManagerRegionID = DbManagerRegionConst.Denied.ID,
                        AtomPermissionKindIdView = DbAtomPermissionKindEnum.Everyone,
                        AtomPermissionKindIdCreate = DbAtomPermissionKindEnum.Denied,
                        AtomPermissionKindIdEdit = DbAtomPermissionKindEnum.Denied,
                        AtomPermissionKindIdDelete = DbAtomPermissionKindEnum.Denied,
                    },
                
                    // 報表分析_產品趨勢
                    new CoMgrRpDbAddManyReqItemMdl()
                    {
                        ManagerRoleID = DbManagerRoleConst.Admin.ID,
                        AtomMenu = DbAtomMenuEnum.ReportProductTrend,
                        ManagerRegionID = DbManagerRegionConst.Denied.ID,
                        AtomPermissionKindIdView = DbAtomPermissionKindEnum.Everyone,
                        AtomPermissionKindIdCreate = DbAtomPermissionKindEnum.Denied,
                        AtomPermissionKindIdEdit = DbAtomPermissionKindEnum.Denied,
                        AtomPermissionKindIdDelete = DbAtomPermissionKindEnum.Denied,
                    },

                    // 報表分析_產品分析
                    new CoMgrRpDbAddManyReqItemMdl()
                    {
                        ManagerRoleID = DbManagerRoleConst.Admin.ID,
                        AtomMenu = DbAtomMenuEnum.ReportProductAnalysis,
                        ManagerRegionID = DbManagerRegionConst.Denied.ID,
                        AtomPermissionKindIdView = DbAtomPermissionKindEnum.Everyone,
                        AtomPermissionKindIdCreate = DbAtomPermissionKindEnum.Denied,
                        AtomPermissionKindIdEdit = DbAtomPermissionKindEnum.Denied,
                        AtomPermissionKindIdDelete = DbAtomPermissionKindEnum.Denied,
                    },

                    // 報表分析_窗口分析
                    new CoMgrRpDbAddManyReqItemMdl()
                    {
                        ManagerRoleID = DbManagerRoleConst.Admin.ID,
                        AtomMenu = DbAtomMenuEnum.ReportContacterAnalysis,
                        ManagerRegionID = DbManagerRegionConst.Denied.ID,
                        AtomPermissionKindIdView = DbAtomPermissionKindEnum.Everyone,
                        AtomPermissionKindIdCreate = DbAtomPermissionKindEnum.Denied,
                        AtomPermissionKindIdEdit = DbAtomPermissionKindEnum.Denied,
                        AtomPermissionKindIdDelete = DbAtomPermissionKindEnum.Denied,
                    },

                    // 報表分析_客戶分析
                    new CoMgrRpDbAddManyReqItemMdl()
                    {
                        ManagerRoleID = DbManagerRoleConst.Admin.ID,
                        AtomMenu = DbAtomMenuEnum.ReportCompanyAnalysis,
                        ManagerRegionID = DbManagerRegionConst.Denied.ID,
                        AtomPermissionKindIdView = DbAtomPermissionKindEnum.Everyone,
                        AtomPermissionKindIdCreate = DbAtomPermissionKindEnum.Denied,
                        AtomPermissionKindIdEdit = DbAtomPermissionKindEnum.Denied,
                        AtomPermissionKindIdDelete = DbAtomPermissionKindEnum.Denied,
                    },

                    // 報表分析_活動分析
                    new CoMgrRpDbAddManyReqItemMdl()
                    {
                        ManagerRoleID = DbManagerRoleConst.Admin.ID,
                        AtomMenu = DbAtomMenuEnum.ReportActivityAnalysis,
                        ManagerRegionID = DbManagerRegionConst.Denied.ID,
                        AtomPermissionKindIdView = DbAtomPermissionKindEnum.Everyone,
                        AtomPermissionKindIdCreate = DbAtomPermissionKindEnum.Denied,
                        AtomPermissionKindIdEdit = DbAtomPermissionKindEnum.Denied,
                        AtomPermissionKindIdDelete = DbAtomPermissionKindEnum.Denied,
                    },

                    #endregion

                    #region 系統設定

                    // 系統設定_窗口
                    new CoMgrRpDbAddManyReqItemMdl()
                    {
                        ManagerRoleID = DbManagerRoleConst.Admin.ID,
                        AtomMenu = DbAtomMenuEnum.SystemContacter,
                        ManagerRegionID = DbManagerRegionConst.Denied.ID,
                        AtomPermissionKindIdView = DbAtomPermissionKindEnum.Everyone,
                        AtomPermissionKindIdCreate = DbAtomPermissionKindEnum.Everyone,
                        AtomPermissionKindIdEdit = DbAtomPermissionKindEnum.Everyone,
                        AtomPermissionKindIdDelete = DbAtomPermissionKindEnum.Everyone
                    },

                    // 系統設定_客戶
                    new CoMgrRpDbAddManyReqItemMdl()
                    {
                        ManagerRoleID = DbManagerRoleConst.Admin.ID,
                        AtomMenu = DbAtomMenuEnum.SystemCompany,
                        ManagerRegionID = DbManagerRegionConst.Denied.ID,
                        AtomPermissionKindIdView = DbAtomPermissionKindEnum.Everyone,
                        AtomPermissionKindIdCreate = DbAtomPermissionKindEnum.Everyone,
                        AtomPermissionKindIdEdit = DbAtomPermissionKindEnum.Everyone,
                        AtomPermissionKindIdDelete = DbAtomPermissionKindEnum.Everyone
                    },

                    // 系統設定_產品
                    new CoMgrRpDbAddManyReqItemMdl()
                    {
                        ManagerRoleID = DbManagerRoleConst.Admin.ID,
                        AtomMenu = DbAtomMenuEnum.SystemProduct,
                        ManagerRegionID = DbManagerRegionConst.Denied.ID,
                        AtomPermissionKindIdView = DbAtomPermissionKindEnum.Everyone,
                        AtomPermissionKindIdCreate = DbAtomPermissionKindEnum.Everyone,
                        AtomPermissionKindIdEdit = DbAtomPermissionKindEnum.Everyone,
                        AtomPermissionKindIdDelete = DbAtomPermissionKindEnum.Denied,
                    },

                    // 系統設定_員工
                    new CoMgrRpDbAddManyReqItemMdl()
                    {
                        ManagerRoleID = DbManagerRoleConst.Admin.ID,
                        AtomMenu = DbAtomMenuEnum.SystemEmployee,
                        ManagerRegionID = DbManagerRegionConst.Denied.ID,
                        AtomPermissionKindIdView = DbAtomPermissionKindEnum.Everyone,
                        AtomPermissionKindIdCreate = DbAtomPermissionKindEnum.Everyone,
                        AtomPermissionKindIdEdit = DbAtomPermissionKindEnum.Everyone,
                        AtomPermissionKindIdDelete = DbAtomPermissionKindEnum.Denied,
                    },

                    // 系統設定_角色
                    new CoMgrRpDbAddManyReqItemMdl()
                    {
                        ManagerRoleID = DbManagerRoleConst.Admin.ID,
                        AtomMenu = DbAtomMenuEnum.SystemRole,
                        ManagerRegionID = DbManagerRegionConst.Denied.ID,
                        AtomPermissionKindIdView = DbAtomPermissionKindEnum.Everyone,
                        AtomPermissionKindIdCreate = DbAtomPermissionKindEnum.Everyone,
                        AtomPermissionKindIdEdit = DbAtomPermissionKindEnum.Everyone,
                    },

                    // 系統設定_部門
                    new CoMgrRpDbAddManyReqItemMdl()
                    {
                        ManagerRoleID = DbManagerRoleConst.Admin.ID,
                        AtomMenu = DbAtomMenuEnum.SystemDepartment,
                        ManagerRegionID = DbManagerRegionConst.Denied.ID,
                        AtomPermissionKindIdView = DbAtomPermissionKindEnum.Everyone,
                        AtomPermissionKindIdCreate = DbAtomPermissionKindEnum.Everyone,
                        AtomPermissionKindIdEdit = DbAtomPermissionKindEnum.Everyone,
                        AtomPermissionKindIdDelete = DbAtomPermissionKindEnum.Denied,
                    },

                    // 系統設定_地區
                    new CoMgrRpDbAddManyReqItemMdl()
                    {
                        ManagerRoleID = DbManagerRoleConst.Admin.ID,
                        AtomMenu = DbAtomMenuEnum.SystemRegion,
                        ManagerRegionID = DbManagerRegionConst.Denied.ID,
                        AtomPermissionKindIdView = DbAtomPermissionKindEnum.Everyone,
                        AtomPermissionKindIdCreate = DbAtomPermissionKindEnum.Everyone,
                        AtomPermissionKindIdEdit = DbAtomPermissionKindEnum.Everyone,
                        AtomPermissionKindIdDelete = DbAtomPermissionKindEnum.Denied,
                    },

                    #endregion
                }
            };
            var coMgrRpDbAddManyRspObj = await this._coMgrRolePermissionDb.AddManyAsync(coMgrRpDbAddManyReqObj);
            if (coMgrRpDbAddManyRspObj == default)
            {
                this._logger.LogError(string.Empty);
                return;
            }

        }

        #endregion

        #region 員工 

        // db, 核心-員工-資訊-資料庫-是否存在
        var coEmpInfDbExistReqObj = new CoEmpInfDbExistReqMdl()
        {
            EmployeeID = DbEmployeeConst.Admin.ID,
        };
        var coEmpInfDbExistRspObj = await this._coEmpInfoDb.ExistAsync(coEmpInfDbExistReqObj);
        if (coEmpInfDbExistRspObj == default)
        {
            this._logger.LogError(string.Empty);
            return;
        }

        // 判斷是否存在
        if (coEmpInfDbExistRspObj.IsExist == false)
        {
            // 不存在，新增

            // logocal, 核心-員工-邏輯-取得密文密碼
            var coEmpLgcGetCypherPasswordReqObj = new CoEmpLgcGetCypherPasswordReqMdl
            {
                EmployeePlainPassword = DbEmployeeConst.Admin.Password,
            };
            var coEmpLgcGetCypherPasswordRspObj = this._coEmpLogical.GetCypherPassword(coEmpLgcGetCypherPasswordReqObj);
            if (coEmpLgcGetCypherPasswordRspObj == default)
            {
                this._logger.LogError(string.Empty);
                return;
            }

            // db, 核心-員工-資訊-資料庫-新增[指定ID]
            var coEmpInfDbAddWithIdAdminReqObj = new CoEmpInfDbAddWithIdReqMdl
            {
                EmployeeID = DbEmployeeConst.Admin.ID,
                ManagerRoleID = DbEmployeeConst.Admin.ManagerRoleID,
                EmployeeAccount = DbEmployeeConst.Admin.Account,
                EmployeePassword = coEmpLgcGetCypherPasswordRspObj.EmployeeCypherPassword,
                EmployeeEmail = DbEmployeeConst.Admin.Email,
                EmployeeName = DbEmployeeConst.Admin.Name,
                EmployeeRemark = DbEmployeeConst.Admin.Remark,
                EmployeeIsEnable = DbEmployeeConst.Admin.IsEnabled,
            };
            var coEmpInfDbAddWithIdAdminRspObj = await this._coEmpInfoDb.AddWithIdAsync(coEmpInfDbAddWithIdAdminReqObj);
            if (coEmpInfDbAddWithIdAdminRspObj == default)
            {
                this._logger.LogError(string.Empty);
                return;
            }

            // db, 核心-員工-目錄權限-資料庫-新增多筆
            var coEmpPmnDbAddManyReqObj = new CoEmpPmnDbAddManyReqMdl
            {
                EmployeePermissionList = new List<CoEmpPmnDbAddManyReqItemMdl>()
                {
                    #region ERP

                    // ERP_發票結案 
                    new CoEmpPmnDbAddManyReqItemMdl()
                    {
                        EmployeeID = DbEmployeeConst.Admin.ID,
                        AtomMenu  = DbAtomMenuEnum.ErpBill,
                        ManagerRegionID = DbManagerRegionConst.Denied.ID,
                        AtomPermissionKindIdView = DbAtomPermissionKindEnum.Everyone,
                        AtomPermissionKindIdCreate = DbAtomPermissionKindEnum.Everyone,
                        AtomPermissionKindIdEdit = DbAtomPermissionKindEnum.Everyone,
                        AtomPermissionKindIdDelete = DbAtomPermissionKindEnum.Everyone
                    },
	                #endregion

                    #region 工作

                    // 工作_專案管理
                    new CoEmpPmnDbAddManyReqItemMdl()
                    {
                        EmployeeID = DbEmployeeConst.Admin.ID,
                        AtomMenu  = DbAtomMenuEnum.WorkProject,
                        ManagerRegionID = DbManagerRegionConst.Denied.ID,
                        AtomPermissionKindIdView = DbAtomPermissionKindEnum.Everyone,
                        AtomPermissionKindIdCreate = DbAtomPermissionKindEnum.Everyone,
                        AtomPermissionKindIdEdit = DbAtomPermissionKindEnum.Everyone,
                        AtomPermissionKindIdDelete = DbAtomPermissionKindEnum.Everyone
                    },

	                #endregion

                    #region CRM
                
                    // CRM_商機管理 
                    new CoEmpPmnDbAddManyReqItemMdl()
                    {
                        EmployeeID = DbEmployeeConst.Admin.ID,
                        AtomMenu  = DbAtomMenuEnum.CrmPipeline,
                        ManagerRegionID = DbManagerRegionConst.AllRegion.ID,
                        AtomPermissionKindIdView = DbAtomPermissionKindEnum.Everyone,
                        AtomPermissionKindIdCreate = DbAtomPermissionKindEnum.Everyone,
                        AtomPermissionKindIdEdit = DbAtomPermissionKindEnum.Everyone,
                        AtomPermissionKindIdDelete = DbAtomPermissionKindEnum.Everyone
                    },

                    // CRM_Booking
                    new CoEmpPmnDbAddManyReqItemMdl()
                    {
                        EmployeeID = DbEmployeeConst.Admin.ID,
                        AtomMenu = DbAtomMenuEnum.CrmBooking,
                        ManagerRegionID = DbManagerRegionConst.Denied.ID,
                        AtomPermissionKindIdView = DbAtomPermissionKindEnum.Everyone,
                        AtomPermissionKindIdCreate = DbAtomPermissionKindEnum.Denied,
                        AtomPermissionKindIdEdit = DbAtomPermissionKindEnum.Everyone,
                        AtomPermissionKindIdDelete = DbAtomPermissionKindEnum.Denied,
                    },
                
                    // CRM_電銷管理
                    new CoEmpPmnDbAddManyReqItemMdl()
                    {
                        EmployeeID = DbEmployeeConst.Admin.ID,
                        AtomMenu = DbAtomMenuEnum.CrmPhone,
                        ManagerRegionID = DbManagerRegionConst.Denied.ID,
                        AtomPermissionKindIdView = DbAtomPermissionKindEnum.Everyone,
                        AtomPermissionKindIdCreate = DbAtomPermissionKindEnum.Everyone,
                        AtomPermissionKindIdEdit = DbAtomPermissionKindEnum.Everyone,
                        AtomPermissionKindIdDelete = DbAtomPermissionKindEnum.Everyone
                    },

                    // CRM_電銷客戶管理
                    new CoEmpPmnDbAddManyReqItemMdl()
                    {
                        EmployeeID = DbEmployeeConst.Admin.ID,
                        AtomMenu = DbAtomMenuEnum.CrmPhoneSales,
                        ManagerRegionID = DbManagerRegionConst.Denied.ID,
                        AtomPermissionKindIdView = DbAtomPermissionKindEnum.Everyone,
                        AtomPermissionKindIdCreate = DbAtomPermissionKindEnum.Everyone,
                        AtomPermissionKindIdEdit = DbAtomPermissionKindEnum.Everyone,
                        AtomPermissionKindIdDelete = DbAtomPermissionKindEnum.Everyone
                    },

                    // CRM_活動管理
                    new CoEmpPmnDbAddManyReqItemMdl()
                    {
                        EmployeeID = DbEmployeeConst.Admin.ID,
                        AtomMenu = DbAtomMenuEnum.CrmActivity,
                        ManagerRegionID = DbManagerRegionConst.Denied.ID,
                        AtomPermissionKindIdView = DbAtomPermissionKindEnum.Everyone,
                        AtomPermissionKindIdCreate = DbAtomPermissionKindEnum.Everyone,
                        AtomPermissionKindIdEdit = DbAtomPermissionKindEnum.Everyone,
                        AtomPermissionKindIdDelete = DbAtomPermissionKindEnum.Everyone
                    },

                    #endregion

                    #region 報表分析
                
                    // 報表分析_產品同比
                    new CoEmpPmnDbAddManyReqItemMdl()
                    {
                        EmployeeID = DbEmployeeConst.Admin.ID,
                        AtomMenu = DbAtomMenuEnum.ReportProductPeriod,
                        ManagerRegionID = DbManagerRegionConst.Denied.ID,
                        AtomPermissionKindIdView = DbAtomPermissionKindEnum.Everyone,
                        AtomPermissionKindIdCreate = DbAtomPermissionKindEnum.Denied,
                        AtomPermissionKindIdEdit = DbAtomPermissionKindEnum.Denied,
                        AtomPermissionKindIdDelete = DbAtomPermissionKindEnum.Denied,
                    },
                
                    // 報表分析_產品趨勢
                    new CoEmpPmnDbAddManyReqItemMdl()
                    {
                        EmployeeID = DbEmployeeConst.Admin.ID,
                        AtomMenu = DbAtomMenuEnum.ReportProductTrend,
                        ManagerRegionID = DbManagerRegionConst.Denied.ID,
                        AtomPermissionKindIdView = DbAtomPermissionKindEnum.Everyone,
                        AtomPermissionKindIdCreate = DbAtomPermissionKindEnum.Denied,
                        AtomPermissionKindIdEdit = DbAtomPermissionKindEnum.Denied,
                        AtomPermissionKindIdDelete = DbAtomPermissionKindEnum.Denied,
                    },

                    // 報表分析_產品分析
                    new CoEmpPmnDbAddManyReqItemMdl()
                    {
                        EmployeeID = DbEmployeeConst.Admin.ID,
                        AtomMenu = DbAtomMenuEnum.ReportProductAnalysis,
                        ManagerRegionID = DbManagerRegionConst.Denied.ID,
                        AtomPermissionKindIdView = DbAtomPermissionKindEnum.Everyone,
                        AtomPermissionKindIdCreate = DbAtomPermissionKindEnum.Denied,
                        AtomPermissionKindIdEdit = DbAtomPermissionKindEnum.Denied,
                        AtomPermissionKindIdDelete = DbAtomPermissionKindEnum.Denied,
                    },

                    // 報表分析_窗口分析
                    new CoEmpPmnDbAddManyReqItemMdl()
                    {
                        EmployeeID = DbEmployeeConst.Admin.ID,
                        AtomMenu = DbAtomMenuEnum.ReportContacterAnalysis,
                        ManagerRegionID = DbManagerRegionConst.Denied.ID,
                        AtomPermissionKindIdView = DbAtomPermissionKindEnum.Everyone,
                        AtomPermissionKindIdCreate = DbAtomPermissionKindEnum.Denied,
                        AtomPermissionKindIdEdit = DbAtomPermissionKindEnum.Denied,
                        AtomPermissionKindIdDelete = DbAtomPermissionKindEnum.Denied,
                    },

                    // 報表分析_客戶分析
                    new CoEmpPmnDbAddManyReqItemMdl()
                    {
                        EmployeeID = DbEmployeeConst.Admin.ID,
                        AtomMenu = DbAtomMenuEnum.ReportCompanyAnalysis,
                        ManagerRegionID = DbManagerRegionConst.Denied.ID,
                        AtomPermissionKindIdView = DbAtomPermissionKindEnum.Everyone,
                        AtomPermissionKindIdCreate = DbAtomPermissionKindEnum.Denied,
                        AtomPermissionKindIdEdit = DbAtomPermissionKindEnum.Denied,
                        AtomPermissionKindIdDelete = DbAtomPermissionKindEnum.Denied,
                    },

                    // 報表分析_活動分析
                    new CoEmpPmnDbAddManyReqItemMdl()
                    {
                        EmployeeID = DbEmployeeConst.Admin.ID,
                        AtomMenu = DbAtomMenuEnum.ReportActivityAnalysis,
                        ManagerRegionID = DbManagerRegionConst.Denied.ID,
                        AtomPermissionKindIdView = DbAtomPermissionKindEnum.Everyone,
                        AtomPermissionKindIdCreate = DbAtomPermissionKindEnum.Denied,
                        AtomPermissionKindIdEdit = DbAtomPermissionKindEnum.Denied,
                        AtomPermissionKindIdDelete = DbAtomPermissionKindEnum.Denied,
                    },

                    #endregion

                    #region 系統設定

                    // 系統設定_窗口
                    new CoEmpPmnDbAddManyReqItemMdl()
                    {
                        EmployeeID = DbEmployeeConst.Admin.ID,
                        AtomMenu = DbAtomMenuEnum.SystemContacter,
                        ManagerRegionID = DbManagerRegionConst.Denied.ID,
                        AtomPermissionKindIdView = DbAtomPermissionKindEnum.Everyone,
                        AtomPermissionKindIdCreate = DbAtomPermissionKindEnum.Everyone,
                        AtomPermissionKindIdEdit = DbAtomPermissionKindEnum.Everyone,
                        AtomPermissionKindIdDelete = DbAtomPermissionKindEnum.Everyone
                    },

                    // 系統設定_客戶
                    new CoEmpPmnDbAddManyReqItemMdl()
                    {
                        EmployeeID = DbEmployeeConst.Admin.ID,
                        AtomMenu = DbAtomMenuEnum.SystemCompany,
                        ManagerRegionID = DbManagerRegionConst.Denied.ID,
                        AtomPermissionKindIdView = DbAtomPermissionKindEnum.Everyone,
                        AtomPermissionKindIdCreate = DbAtomPermissionKindEnum.Everyone,
                        AtomPermissionKindIdEdit = DbAtomPermissionKindEnum.Everyone,
                        AtomPermissionKindIdDelete = DbAtomPermissionKindEnum.Everyone
                    },

                    // 系統設定_產品
                    new CoEmpPmnDbAddManyReqItemMdl()
                    {
                        EmployeeID = DbEmployeeConst.Admin.ID,
                        AtomMenu = DbAtomMenuEnum.SystemProduct,
                        ManagerRegionID = DbManagerRegionConst.Denied.ID,
                        AtomPermissionKindIdView = DbAtomPermissionKindEnum.Everyone,
                        AtomPermissionKindIdCreate = DbAtomPermissionKindEnum.Everyone,
                        AtomPermissionKindIdEdit = DbAtomPermissionKindEnum.Everyone,
                        AtomPermissionKindIdDelete = DbAtomPermissionKindEnum.Denied,
                    },

                    // 系統設定_員工
                    new CoEmpPmnDbAddManyReqItemMdl()
                    {
                        EmployeeID = DbEmployeeConst.Admin.ID,
                        AtomMenu = DbAtomMenuEnum.SystemEmployee,
                        ManagerRegionID = DbManagerRegionConst.Denied.ID,
                        AtomPermissionKindIdView = DbAtomPermissionKindEnum.Everyone,
                        AtomPermissionKindIdCreate = DbAtomPermissionKindEnum.Everyone,
                        AtomPermissionKindIdEdit = DbAtomPermissionKindEnum.Everyone,
                        AtomPermissionKindIdDelete = DbAtomPermissionKindEnum.Denied,
                    },

                    // 系統設定_角色
                    new CoEmpPmnDbAddManyReqItemMdl()
                    {
                        EmployeeID = DbEmployeeConst.Admin.ID,
                        AtomMenu = DbAtomMenuEnum.SystemRole,
                        ManagerRegionID = DbManagerRegionConst.Denied.ID,
                        AtomPermissionKindIdView = DbAtomPermissionKindEnum.Everyone,
                        AtomPermissionKindIdCreate = DbAtomPermissionKindEnum.Everyone,
                        AtomPermissionKindIdEdit = DbAtomPermissionKindEnum.Everyone,
                    },

                    // 系統設定_部門
                    new CoEmpPmnDbAddManyReqItemMdl()
                    {
                        EmployeeID = DbEmployeeConst.Admin.ID,
                        AtomMenu = DbAtomMenuEnum.SystemDepartment,
                        ManagerRegionID = DbManagerRegionConst.Denied.ID,
                        AtomPermissionKindIdView = DbAtomPermissionKindEnum.Everyone,
                        AtomPermissionKindIdCreate = DbAtomPermissionKindEnum.Everyone,
                        AtomPermissionKindIdEdit = DbAtomPermissionKindEnum.Everyone,
                        AtomPermissionKindIdDelete = DbAtomPermissionKindEnum.Denied,
                    },

                    // 系統設定_地區
                    new CoEmpPmnDbAddManyReqItemMdl()
                    {
                        EmployeeID = DbEmployeeConst.Admin.ID,
                        AtomMenu = DbAtomMenuEnum.SystemRegion,
                        ManagerRegionID = DbManagerRegionConst.Denied.ID,
                        AtomPermissionKindIdView = DbAtomPermissionKindEnum.Everyone,
                        AtomPermissionKindIdCreate = DbAtomPermissionKindEnum.Everyone,
                        AtomPermissionKindIdEdit = DbAtomPermissionKindEnum.Everyone,
                        AtomPermissionKindIdDelete = DbAtomPermissionKindEnum.Denied,
                    },

                    #endregion
                }
            };
            var coEmpPmnDbAddManyRspObj = await this._coEmpPermissionDb.AddManyAsync(coEmpPmnDbAddManyReqObj);
            if (coEmpPmnDbAddManyRspObj == default)
            {
                this._logger.LogError(string.Empty);
                return;
            }

        }

        // 存在，不處理

        #endregion

    }

    /// <summary>排程-初始化-邏輯服務-初始化員工-總經理</summary>
    public async Task InitialEmployeeGeneralManagerAsync()
    {
        #region 部門 

        // db, 核心-管理者-部門-資料庫-是否存在
        var coMgrDptDbExistReqObj = new CoMgrDptDbExistReqMdl()
        {
            ManagerDepartmentID = DbManagerDepartmentConst.GeneralManagerOffice.ID,
        };
        var coMgrDptDbExistRspObj = await this._coMgrDepartmentDb.ExistAsync(coMgrDptDbExistReqObj);
        if (coMgrDptDbExistRspObj == default)
        {
            this._logger.LogError(string.Empty);
            return;
        }

        // 判斷是否存在
        if (coMgrDptDbExistRspObj.IsExist == false)
        {
            // 不存在，新增

            // db, 核心-管理者-部門-資料庫-新增[指定ID]
            var adminDptDbAddWithIdAdminReqObj = new CoMgrDptDbAddWithIdReqMdl
            {
                ManagerDepartmentId = DbManagerDepartmentConst.GeneralManagerOffice.ID,
                ManagerDepartmentName = DbManagerDepartmentConst.GeneralManagerOffice.Name,
                ManagerDepartmentIsEnable = DbManagerDepartmentConst.GeneralManagerOffice.IsEnabled
            };
            var adminDptDbAddWithIdAdminRspObj = await this._coMgrDepartmentDb.AddWithIdAsync(adminDptDbAddWithIdAdminReqObj);
            if (adminDptDbAddWithIdAdminRspObj == default)
            {
                this._logger.LogError(string.Empty);
                return;
            }

        }

        #endregion

        #region 角色

        // db, 核心-管理者-角色-資料庫-是否存在
        var coMgrRolDbExistReqObj = new CoMgrRolDbExistReqMdl()
        {
            ManagerRoleID = DbManagerRoleConst.GeneralManager.ID,
        };
        var coMgrRolDbExistRspObj = await this._coMgrRoleDb.ExistAsync(coMgrRolDbExistReqObj);
        if (coMgrRolDbExistRspObj == default)
        {
            this._logger.LogError(string.Empty);
            return;
        }

        // 判斷是否存在
        if (coMgrRolDbExistRspObj.IsExist == false)
        {
            // 不存在，新增

            // db, 核心-管理者-角色-資料庫-新增[指定ID]
            var coMgrRolDbAddWithIdReqObj = new CoMgrRolDbAddWithIdReqMdl
            {
                ManagerRoleId = DbManagerRoleConst.GeneralManager.ID,
                ManagerRoleName = DbManagerRoleConst.GeneralManager.Name,
                ManagerRegionID = DbManagerRoleConst.GeneralManager.ManagerRegionID,
                ManagerDepartmentID = DbManagerRoleConst.GeneralManager.ManagerDepartmentID,
                ManagerRoleRemark = DbManagerRoleConst.GeneralManager.Remark,
                ManagerRoleIsEnable = DbManagerRoleConst.GeneralManager.IsEnabled,
            };
            var coMgrRolDbAddWithIdRspObj = await this._coMgrRoleDb.AddWithIdAsync(coMgrRolDbAddWithIdReqObj);
            if (coMgrRolDbAddWithIdRspObj == default)
            {
                this._logger.LogError(string.Empty);
                return;
            }

            // db, 核心-管理者-角色權限-資料庫-新增多筆
            var coMgrRpDbAddManyReqObj = new CoMgrRpDbAddManyReqMdl
            {
                ManagerRolePermissionList = new List<CoMgrRpDbAddManyReqItemMdl>()
                {
                     #region ERP

                    // ERP_發票結案 
                    new CoMgrRpDbAddManyReqItemMdl()
                    {
                        ManagerRoleID = DbManagerRoleConst.GeneralManager.ID,
                        AtomMenu  = DbAtomMenuEnum.ErpBill,
                        ManagerRegionID = DbManagerRegionConst.Denied.ID,
                        AtomPermissionKindIdView = DbAtomPermissionKindEnum.Everyone,
                        AtomPermissionKindIdCreate = DbAtomPermissionKindEnum.Everyone,
                        AtomPermissionKindIdEdit = DbAtomPermissionKindEnum.Everyone,
                        AtomPermissionKindIdDelete = DbAtomPermissionKindEnum.Everyone,
                    },
	                #endregion

                    #region 工作

                    // 工作_專案管理
                    new CoMgrRpDbAddManyReqItemMdl()
                    {
                        ManagerRoleID = DbManagerRoleConst.GeneralManager.ID,
                        AtomMenu  = DbAtomMenuEnum.WorkProject,
                        ManagerRegionID = DbManagerRegionConst.Denied.ID,
                        AtomPermissionKindIdView = DbAtomPermissionKindEnum.Everyone,
                        AtomPermissionKindIdCreate = DbAtomPermissionKindEnum.Everyone,
                        AtomPermissionKindIdEdit = DbAtomPermissionKindEnum.Everyone,
                        AtomPermissionKindIdDelete = DbAtomPermissionKindEnum.Everyone
                    },

	                #endregion

                    #region CRM
                
                    // CRM_商機管理 
                    new CoMgrRpDbAddManyReqItemMdl()
                    {
                        ManagerRoleID = DbManagerRoleConst.GeneralManager.ID,
                        AtomMenu  = DbAtomMenuEnum.CrmPipeline,
                        ManagerRegionID = DbManagerRegionConst.AllRegion.ID,
                        AtomPermissionKindIdView = DbAtomPermissionKindEnum.Everyone,
                        AtomPermissionKindIdCreate = DbAtomPermissionKindEnum.Everyone,
                        AtomPermissionKindIdEdit = DbAtomPermissionKindEnum.Everyone,
                        AtomPermissionKindIdDelete = DbAtomPermissionKindEnum.Everyone
                    },

                    // CRM_Booking
                    new CoMgrRpDbAddManyReqItemMdl()
                    {
                        ManagerRoleID = DbManagerRoleConst.GeneralManager.ID,
                        AtomMenu = DbAtomMenuEnum.CrmBooking,
                        ManagerRegionID = DbManagerRegionConst.Denied.ID,
                        AtomPermissionKindIdView = DbAtomPermissionKindEnum.Everyone,
                        AtomPermissionKindIdCreate = DbAtomPermissionKindEnum.Denied,
                        AtomPermissionKindIdEdit = DbAtomPermissionKindEnum.Everyone,
                        AtomPermissionKindIdDelete = DbAtomPermissionKindEnum.Denied,
                    },
                
                    // CRM_電銷管理
                    new CoMgrRpDbAddManyReqItemMdl()
                    {
                        ManagerRoleID = DbManagerRoleConst.GeneralManager.ID,
                        AtomMenu = DbAtomMenuEnum.CrmPhone,
                        ManagerRegionID = DbManagerRegionConst.Denied.ID,
                        AtomPermissionKindIdView = DbAtomPermissionKindEnum.Everyone,
                        AtomPermissionKindIdCreate = DbAtomPermissionKindEnum.Everyone,
                        AtomPermissionKindIdEdit = DbAtomPermissionKindEnum.Everyone,
                        AtomPermissionKindIdDelete = DbAtomPermissionKindEnum.Everyone
                    },

                    // CRM_電銷客戶管理
                    new CoMgrRpDbAddManyReqItemMdl()
                    {
                        ManagerRoleID = DbManagerRoleConst.GeneralManager.ID,
                        AtomMenu = DbAtomMenuEnum.CrmPhoneSales,
                        ManagerRegionID = DbManagerRegionConst.Denied.ID,
                        AtomPermissionKindIdView = DbAtomPermissionKindEnum.Everyone,
                        AtomPermissionKindIdCreate = DbAtomPermissionKindEnum.Everyone,
                        AtomPermissionKindIdEdit = DbAtomPermissionKindEnum.Everyone,
                        AtomPermissionKindIdDelete = DbAtomPermissionKindEnum.Everyone
                    },

                    // CRM_活動管理
                    new CoMgrRpDbAddManyReqItemMdl()
                    {
                        ManagerRoleID = DbManagerRoleConst.GeneralManager.ID,
                        AtomMenu = DbAtomMenuEnum.CrmActivity,
                        ManagerRegionID = DbManagerRegionConst.Denied.ID,
                        AtomPermissionKindIdView = DbAtomPermissionKindEnum.Everyone,
                        AtomPermissionKindIdCreate = DbAtomPermissionKindEnum.Everyone,
                        AtomPermissionKindIdEdit = DbAtomPermissionKindEnum.Everyone,
                        AtomPermissionKindIdDelete = DbAtomPermissionKindEnum.Everyone
                    },

                    #endregion

                    #region 報表分析
                
                    // 報表分析_產品同比
                    new CoMgrRpDbAddManyReqItemMdl()
                    {
                        ManagerRoleID = DbManagerRoleConst.GeneralManager.ID,
                        AtomMenu = DbAtomMenuEnum.ReportProductPeriod,
                        ManagerRegionID = DbManagerRegionConst.Denied.ID,
                        AtomPermissionKindIdView = DbAtomPermissionKindEnum.Everyone,
                        AtomPermissionKindIdCreate = DbAtomPermissionKindEnum.Denied,
                        AtomPermissionKindIdEdit = DbAtomPermissionKindEnum.Denied,
                        AtomPermissionKindIdDelete = DbAtomPermissionKindEnum.Denied,
                    },
                
                    // 報表分析_產品趨勢
                    new CoMgrRpDbAddManyReqItemMdl()
                    {
                        ManagerRoleID = DbManagerRoleConst.GeneralManager.ID,
                        AtomMenu = DbAtomMenuEnum.ReportProductTrend,
                        ManagerRegionID = DbManagerRegionConst.Denied.ID,
                        AtomPermissionKindIdView = DbAtomPermissionKindEnum.Everyone,
                        AtomPermissionKindIdCreate = DbAtomPermissionKindEnum.Denied,
                        AtomPermissionKindIdEdit = DbAtomPermissionKindEnum.Denied,
                        AtomPermissionKindIdDelete = DbAtomPermissionKindEnum.Denied,
                    },

                    // 報表分析_產品分析
                    new CoMgrRpDbAddManyReqItemMdl()
                    {
                        ManagerRoleID = DbManagerRoleConst.GeneralManager.ID,
                        AtomMenu = DbAtomMenuEnum.ReportProductAnalysis,
                        ManagerRegionID = DbManagerRegionConst.Denied.ID,
                        AtomPermissionKindIdView = DbAtomPermissionKindEnum.Everyone,
                        AtomPermissionKindIdCreate = DbAtomPermissionKindEnum.Denied,
                        AtomPermissionKindIdEdit = DbAtomPermissionKindEnum.Denied,
                        AtomPermissionKindIdDelete = DbAtomPermissionKindEnum.Denied,
                    },

                    // 報表分析_窗口分析
                    new CoMgrRpDbAddManyReqItemMdl()
                    {
                        ManagerRoleID = DbManagerRoleConst.GeneralManager.ID,
                        AtomMenu = DbAtomMenuEnum.ReportContacterAnalysis,
                        ManagerRegionID = DbManagerRegionConst.Denied.ID,
                        AtomPermissionKindIdView = DbAtomPermissionKindEnum.Everyone,
                        AtomPermissionKindIdCreate = DbAtomPermissionKindEnum.Denied,
                        AtomPermissionKindIdEdit = DbAtomPermissionKindEnum.Denied,
                        AtomPermissionKindIdDelete = DbAtomPermissionKindEnum.Denied,
                    },

                    // 報表分析_客戶分析
                    new CoMgrRpDbAddManyReqItemMdl()
                    {
                        ManagerRoleID = DbManagerRoleConst.GeneralManager.ID,
                        AtomMenu = DbAtomMenuEnum.ReportCompanyAnalysis,
                        ManagerRegionID = DbManagerRegionConst.Denied.ID,
                        AtomPermissionKindIdView = DbAtomPermissionKindEnum.Everyone,
                        AtomPermissionKindIdCreate = DbAtomPermissionKindEnum.Denied,
                        AtomPermissionKindIdEdit = DbAtomPermissionKindEnum.Denied,
                        AtomPermissionKindIdDelete = DbAtomPermissionKindEnum.Denied,
                    },

                    // 報表分析_活動分析
                    new CoMgrRpDbAddManyReqItemMdl()
                    {
                        ManagerRoleID = DbManagerRoleConst.GeneralManager.ID,
                        AtomMenu = DbAtomMenuEnum.ReportActivityAnalysis,
                        ManagerRegionID = DbManagerRegionConst.Denied.ID,
                        AtomPermissionKindIdView = DbAtomPermissionKindEnum.Everyone,
                        AtomPermissionKindIdCreate = DbAtomPermissionKindEnum.Denied,
                        AtomPermissionKindIdEdit = DbAtomPermissionKindEnum.Denied,
                        AtomPermissionKindIdDelete = DbAtomPermissionKindEnum.Denied,
                    },

                    #endregion

                    #region 系統設定

                    // 系統設定_窗口
                    new CoMgrRpDbAddManyReqItemMdl()
                    {
                        ManagerRoleID = DbManagerRoleConst.GeneralManager.ID,
                        AtomMenu = DbAtomMenuEnum.SystemContacter,
                        ManagerRegionID = DbManagerRegionConst.Denied.ID,
                        AtomPermissionKindIdView = DbAtomPermissionKindEnum.Everyone,
                        AtomPermissionKindIdCreate = DbAtomPermissionKindEnum.Everyone,
                        AtomPermissionKindIdEdit = DbAtomPermissionKindEnum.Everyone,
                        AtomPermissionKindIdDelete = DbAtomPermissionKindEnum.Everyone
                    },

                    // 系統設定_客戶
                    new CoMgrRpDbAddManyReqItemMdl()
                    {
                        ManagerRoleID = DbManagerRoleConst.GeneralManager.ID,
                        AtomMenu = DbAtomMenuEnum.SystemCompany,
                        ManagerRegionID = DbManagerRegionConst.Denied.ID,
                        AtomPermissionKindIdView = DbAtomPermissionKindEnum.Everyone,
                        AtomPermissionKindIdCreate = DbAtomPermissionKindEnum.Everyone,
                        AtomPermissionKindIdEdit = DbAtomPermissionKindEnum.Everyone,
                        AtomPermissionKindIdDelete = DbAtomPermissionKindEnum.Everyone
                    },

                    // 系統設定_產品
                    new CoMgrRpDbAddManyReqItemMdl()
                    {
                        ManagerRoleID = DbManagerRoleConst.GeneralManager.ID,
                        AtomMenu = DbAtomMenuEnum.SystemProduct,
                        ManagerRegionID = DbManagerRegionConst.Denied.ID,
                        AtomPermissionKindIdView = DbAtomPermissionKindEnum.Everyone,
                        AtomPermissionKindIdCreate = DbAtomPermissionKindEnum.Everyone,
                        AtomPermissionKindIdEdit = DbAtomPermissionKindEnum.Everyone,
                        AtomPermissionKindIdDelete = DbAtomPermissionKindEnum.Denied,
                    },

                    // 系統設定_員工
                    new CoMgrRpDbAddManyReqItemMdl()
                    {
                        ManagerRoleID = DbManagerRoleConst.GeneralManager.ID,
                        AtomMenu = DbAtomMenuEnum.SystemEmployee,
                        ManagerRegionID = DbManagerRegionConst.Denied.ID,
                        AtomPermissionKindIdView = DbAtomPermissionKindEnum.Everyone,
                        AtomPermissionKindIdCreate = DbAtomPermissionKindEnum.Everyone,
                        AtomPermissionKindIdEdit = DbAtomPermissionKindEnum.Everyone,
                        AtomPermissionKindIdDelete = DbAtomPermissionKindEnum.Denied,
                    },

                    // 系統設定_角色
                    new CoMgrRpDbAddManyReqItemMdl()
                    {
                        ManagerRoleID = DbManagerRoleConst.GeneralManager.ID,
                        AtomMenu = DbAtomMenuEnum.SystemRole,
                        ManagerRegionID = DbManagerRegionConst.Denied.ID,
                        AtomPermissionKindIdView = DbAtomPermissionKindEnum.Everyone,
                        AtomPermissionKindIdCreate = DbAtomPermissionKindEnum.Everyone,
                        AtomPermissionKindIdEdit = DbAtomPermissionKindEnum.Everyone,
                    },

                    // 系統設定_部門
                    new CoMgrRpDbAddManyReqItemMdl()
                    {
                        ManagerRoleID = DbManagerRoleConst.GeneralManager.ID,
                        AtomMenu = DbAtomMenuEnum.SystemDepartment,
                        ManagerRegionID = DbManagerRegionConst.Denied.ID,
                        AtomPermissionKindIdView = DbAtomPermissionKindEnum.Everyone,
                        AtomPermissionKindIdCreate = DbAtomPermissionKindEnum.Everyone,
                        AtomPermissionKindIdEdit = DbAtomPermissionKindEnum.Everyone,
                        AtomPermissionKindIdDelete = DbAtomPermissionKindEnum.Denied,
                    },

                    // 系統設定_地區
                    new CoMgrRpDbAddManyReqItemMdl()
                    {
                        ManagerRoleID = DbManagerRoleConst.GeneralManager.ID,
                        AtomMenu = DbAtomMenuEnum.SystemRegion,
                        ManagerRegionID = DbManagerRegionConst.Denied.ID,
                        AtomPermissionKindIdView = DbAtomPermissionKindEnum.Everyone,
                        AtomPermissionKindIdCreate = DbAtomPermissionKindEnum.Everyone,
                        AtomPermissionKindIdEdit = DbAtomPermissionKindEnum.Everyone,
                        AtomPermissionKindIdDelete = DbAtomPermissionKindEnum.Denied,
                    },

                    #endregion
                }
            };
            var coMgrRpDbAddManyRspObj = await this._coMgrRolePermissionDb.AddManyAsync(coMgrRpDbAddManyReqObj);
            if (coMgrRpDbAddManyRspObj == default)
            {
                this._logger.LogError(string.Empty);
                return;
            }

        }

        #endregion

        #region 員工 

        // db, 核心-員工-資訊-資料庫-是否存在
        var coEmpInfDbExistReqObj = new CoEmpInfDbExistReqMdl()
        {
            EmployeeID = DbEmployeeConst.GeneralManager.ID,
        };
        var coEmpInfDbExistRspObj = await this._coEmpInfoDb.ExistAsync(coEmpInfDbExistReqObj);
        if (coEmpInfDbExistRspObj == default)
        {
            this._logger.LogError(string.Empty);
            return;
        }

        // 判斷是否存在
        if (coEmpInfDbExistRspObj.IsExist == false)
        {
            // 不存在，新增

            // logocal, 核心-員工-邏輯-取得密文密碼
            var coEmpLgcGetCypherPasswordReqObj = new CoEmpLgcGetCypherPasswordReqMdl
            {
                EmployeePlainPassword = DbEmployeeConst.GeneralManager.Password,
            };
            var coEmpLgcGetCypherPasswordRspObj = this._coEmpLogical.GetCypherPassword(coEmpLgcGetCypherPasswordReqObj);
            if (coEmpLgcGetCypherPasswordRspObj == default)
            {
                this._logger.LogError(string.Empty);
                return;
            }

            // db, 核心-員工-資訊-資料庫-新增[指定ID]
            var coEmpInfDbAddWithIdAdminReqObj = new CoEmpInfDbAddWithIdReqMdl
            {
                EmployeeID = DbEmployeeConst.GeneralManager.ID,
                ManagerRoleID = DbEmployeeConst.GeneralManager.ManagerRoleID,
                EmployeeAccount = DbEmployeeConst.GeneralManager.Account,
                EmployeePassword = coEmpLgcGetCypherPasswordRspObj.EmployeeCypherPassword,
                EmployeeEmail = DbEmployeeConst.GeneralManager.Email,
                EmployeeName = DbEmployeeConst.GeneralManager.Name,
                EmployeeRemark = DbEmployeeConst.GeneralManager.Remark,
                EmployeeIsEnable = DbEmployeeConst.GeneralManager.IsEnabled,
            };
            var coEmpInfDbAddWithIdAdminRspObj = await this._coEmpInfoDb.AddWithIdAsync(coEmpInfDbAddWithIdAdminReqObj);
            if (coEmpInfDbAddWithIdAdminRspObj == default)
            {
                this._logger.LogError(string.Empty);
                return;
            }

            // db, 核心-員工-目錄權限-資料庫-新增多筆
            var coEmpPmnDbAddManyReqObj = new CoEmpPmnDbAddManyReqMdl
            {
                EmployeePermissionList = new List<CoEmpPmnDbAddManyReqItemMdl>()
                {
                    #region ERP

                    // ERP_發票結案 
                    new CoEmpPmnDbAddManyReqItemMdl()
                    {
                        EmployeeID = DbEmployeeConst.GeneralManager.ID,
                        AtomMenu  = DbAtomMenuEnum.ErpBill,
                        ManagerRegionID = DbManagerRegionConst.Denied.ID,
                        AtomPermissionKindIdView = DbAtomPermissionKindEnum.Everyone,
                        AtomPermissionKindIdCreate = DbAtomPermissionKindEnum.Everyone,
                        AtomPermissionKindIdEdit = DbAtomPermissionKindEnum.Everyone,
                        AtomPermissionKindIdDelete = DbAtomPermissionKindEnum.Everyone
                    },
	                #endregion

                    #region 工作

                    // 工作_專案管理
                    new CoEmpPmnDbAddManyReqItemMdl()
                    {
                        EmployeeID = DbEmployeeConst.GeneralManager.ID,
                        AtomMenu  = DbAtomMenuEnum.WorkProject,
                        ManagerRegionID = DbManagerRegionConst.Denied.ID,
                        AtomPermissionKindIdView = DbAtomPermissionKindEnum.Everyone,
                        AtomPermissionKindIdCreate = DbAtomPermissionKindEnum.Everyone,
                        AtomPermissionKindIdEdit = DbAtomPermissionKindEnum.Everyone,
                        AtomPermissionKindIdDelete = DbAtomPermissionKindEnum.Everyone
                    },

	                #endregion

                    #region CRM
                
                    // CRM_商機管理 
                    new CoEmpPmnDbAddManyReqItemMdl()
                    {
                        EmployeeID = DbEmployeeConst.GeneralManager.ID,
                        AtomMenu  = DbAtomMenuEnum.CrmPipeline,
                        ManagerRegionID = DbManagerRegionConst.AllRegion.ID,
                        AtomPermissionKindIdView = DbAtomPermissionKindEnum.Everyone,
                        AtomPermissionKindIdCreate = DbAtomPermissionKindEnum.Everyone,
                        AtomPermissionKindIdEdit = DbAtomPermissionKindEnum.Everyone,
                        AtomPermissionKindIdDelete = DbAtomPermissionKindEnum.Everyone
                    },

                    // CRM_Booking
                    new CoEmpPmnDbAddManyReqItemMdl()
                    {
                        EmployeeID = DbEmployeeConst.GeneralManager.ID,
                        AtomMenu = DbAtomMenuEnum.CrmBooking,
                        ManagerRegionID = DbManagerRegionConst.Denied.ID,
                        AtomPermissionKindIdView = DbAtomPermissionKindEnum.Everyone,
                        AtomPermissionKindIdCreate = DbAtomPermissionKindEnum.Denied,
                        AtomPermissionKindIdEdit = DbAtomPermissionKindEnum.Everyone,
                        AtomPermissionKindIdDelete = DbAtomPermissionKindEnum.Denied,
                    },
                
                    // CRM_電銷管理
                    new CoEmpPmnDbAddManyReqItemMdl()
                    {
                        EmployeeID = DbEmployeeConst.GeneralManager.ID,
                        AtomMenu = DbAtomMenuEnum.CrmPhone,
                        ManagerRegionID = DbManagerRegionConst.Denied.ID,
                        AtomPermissionKindIdView = DbAtomPermissionKindEnum.Everyone,
                        AtomPermissionKindIdCreate = DbAtomPermissionKindEnum.Everyone,
                        AtomPermissionKindIdEdit = DbAtomPermissionKindEnum.Everyone,
                        AtomPermissionKindIdDelete = DbAtomPermissionKindEnum.Everyone
                    },

                    // CRM_電銷客戶管理
                    new CoEmpPmnDbAddManyReqItemMdl()
                    {
                        EmployeeID = DbEmployeeConst.GeneralManager.ID,
                        AtomMenu = DbAtomMenuEnum.CrmPhoneSales,
                        ManagerRegionID = DbManagerRegionConst.Denied.ID,
                        AtomPermissionKindIdView = DbAtomPermissionKindEnum.Everyone,
                        AtomPermissionKindIdCreate = DbAtomPermissionKindEnum.Everyone,
                        AtomPermissionKindIdEdit = DbAtomPermissionKindEnum.Everyone,
                        AtomPermissionKindIdDelete = DbAtomPermissionKindEnum.Everyone
                    },

                    // CRM_活動管理
                    new CoEmpPmnDbAddManyReqItemMdl()
                    {
                        EmployeeID = DbEmployeeConst.GeneralManager.ID,
                        AtomMenu = DbAtomMenuEnum.CrmActivity,
                        ManagerRegionID = DbManagerRegionConst.Denied.ID,
                        AtomPermissionKindIdView = DbAtomPermissionKindEnum.Everyone,
                        AtomPermissionKindIdCreate = DbAtomPermissionKindEnum.Everyone,
                        AtomPermissionKindIdEdit = DbAtomPermissionKindEnum.Everyone,
                        AtomPermissionKindIdDelete = DbAtomPermissionKindEnum.Everyone
                    },

                    #endregion

                    #region 報表分析
                
                    // 報表分析_產品同比
                    new CoEmpPmnDbAddManyReqItemMdl()
                    {
                        EmployeeID = DbEmployeeConst.GeneralManager.ID,
                        AtomMenu = DbAtomMenuEnum.ReportProductPeriod,
                        ManagerRegionID = DbManagerRegionConst.Denied.ID,
                        AtomPermissionKindIdView = DbAtomPermissionKindEnum.Everyone,
                        AtomPermissionKindIdCreate = DbAtomPermissionKindEnum.Denied,
                        AtomPermissionKindIdEdit = DbAtomPermissionKindEnum.Denied,
                        AtomPermissionKindIdDelete = DbAtomPermissionKindEnum.Denied,
                    },
                
                    // 報表分析_產品趨勢
                    new CoEmpPmnDbAddManyReqItemMdl()
                    {
                        EmployeeID = DbEmployeeConst.GeneralManager.ID,
                        AtomMenu = DbAtomMenuEnum.ReportProductTrend,
                        ManagerRegionID = DbManagerRegionConst.Denied.ID,
                        AtomPermissionKindIdView = DbAtomPermissionKindEnum.Everyone,
                        AtomPermissionKindIdCreate = DbAtomPermissionKindEnum.Denied,
                        AtomPermissionKindIdEdit = DbAtomPermissionKindEnum.Denied,
                        AtomPermissionKindIdDelete = DbAtomPermissionKindEnum.Denied,
                    },

                    // 報表分析_產品分析
                    new CoEmpPmnDbAddManyReqItemMdl()
                    {
                        EmployeeID = DbEmployeeConst.GeneralManager.ID,
                        AtomMenu = DbAtomMenuEnum.ReportProductAnalysis,
                        ManagerRegionID = DbManagerRegionConst.Denied.ID,
                        AtomPermissionKindIdView = DbAtomPermissionKindEnum.Everyone,
                        AtomPermissionKindIdCreate = DbAtomPermissionKindEnum.Denied,
                        AtomPermissionKindIdEdit = DbAtomPermissionKindEnum.Denied,
                        AtomPermissionKindIdDelete = DbAtomPermissionKindEnum.Denied,
                    },

                    // 報表分析_窗口分析
                    new CoEmpPmnDbAddManyReqItemMdl()
                    {
                        EmployeeID = DbEmployeeConst.GeneralManager.ID,
                        AtomMenu = DbAtomMenuEnum.ReportContacterAnalysis,
                        ManagerRegionID = DbManagerRegionConst.Denied.ID,
                        AtomPermissionKindIdView = DbAtomPermissionKindEnum.Everyone,
                        AtomPermissionKindIdCreate = DbAtomPermissionKindEnum.Denied,
                        AtomPermissionKindIdEdit = DbAtomPermissionKindEnum.Denied,
                        AtomPermissionKindIdDelete = DbAtomPermissionKindEnum.Denied,
                    },

                    // 報表分析_客戶分析
                    new CoEmpPmnDbAddManyReqItemMdl()
                    {
                        EmployeeID = DbEmployeeConst.GeneralManager.ID,
                        AtomMenu = DbAtomMenuEnum.ReportCompanyAnalysis,
                        ManagerRegionID = DbManagerRegionConst.Denied.ID,
                        AtomPermissionKindIdView = DbAtomPermissionKindEnum.Everyone,
                        AtomPermissionKindIdCreate = DbAtomPermissionKindEnum.Denied,
                        AtomPermissionKindIdEdit = DbAtomPermissionKindEnum.Denied,
                        AtomPermissionKindIdDelete = DbAtomPermissionKindEnum.Denied,
                    },

                    // 報表分析_活動分析
                    new CoEmpPmnDbAddManyReqItemMdl()
                    {
                        EmployeeID = DbEmployeeConst.GeneralManager.ID,
                        AtomMenu = DbAtomMenuEnum.ReportActivityAnalysis,
                        ManagerRegionID = DbManagerRegionConst.Denied.ID,
                        AtomPermissionKindIdView = DbAtomPermissionKindEnum.Everyone,
                        AtomPermissionKindIdCreate = DbAtomPermissionKindEnum.Denied,
                        AtomPermissionKindIdEdit = DbAtomPermissionKindEnum.Denied,
                        AtomPermissionKindIdDelete = DbAtomPermissionKindEnum.Denied,
                    },

                    #endregion

                    #region 系統設定

                    // 系統設定_窗口
                    new CoEmpPmnDbAddManyReqItemMdl()
                    {
                        EmployeeID = DbEmployeeConst.GeneralManager.ID,
                        AtomMenu = DbAtomMenuEnum.SystemContacter,
                        ManagerRegionID = DbManagerRegionConst.Denied.ID,
                        AtomPermissionKindIdView = DbAtomPermissionKindEnum.Everyone,
                        AtomPermissionKindIdCreate = DbAtomPermissionKindEnum.Everyone,
                        AtomPermissionKindIdEdit = DbAtomPermissionKindEnum.Everyone,
                        AtomPermissionKindIdDelete = DbAtomPermissionKindEnum.Everyone
                    },

                    // 系統設定_客戶
                    new CoEmpPmnDbAddManyReqItemMdl()
                    {
                        EmployeeID = DbEmployeeConst.GeneralManager.ID,
                        AtomMenu = DbAtomMenuEnum.SystemCompany,
                        ManagerRegionID = DbManagerRegionConst.Denied.ID,
                        AtomPermissionKindIdView = DbAtomPermissionKindEnum.Everyone,
                        AtomPermissionKindIdCreate = DbAtomPermissionKindEnum.Everyone,
                        AtomPermissionKindIdEdit = DbAtomPermissionKindEnum.Everyone,
                        AtomPermissionKindIdDelete = DbAtomPermissionKindEnum.Everyone
                    },

                    // 系統設定_產品
                    new CoEmpPmnDbAddManyReqItemMdl()
                    {
                        EmployeeID = DbEmployeeConst.GeneralManager.ID,
                        AtomMenu = DbAtomMenuEnum.SystemProduct,
                        ManagerRegionID = DbManagerRegionConst.Denied.ID,
                        AtomPermissionKindIdView = DbAtomPermissionKindEnum.Everyone,
                        AtomPermissionKindIdCreate = DbAtomPermissionKindEnum.Everyone,
                        AtomPermissionKindIdEdit = DbAtomPermissionKindEnum.Everyone,
                        AtomPermissionKindIdDelete = DbAtomPermissionKindEnum.Denied,
                    },

                    // 系統設定_員工
                    new CoEmpPmnDbAddManyReqItemMdl()
                    {
                        EmployeeID = DbEmployeeConst.GeneralManager.ID,
                        AtomMenu = DbAtomMenuEnum.SystemEmployee,
                        ManagerRegionID = DbManagerRegionConst.Denied.ID,
                        AtomPermissionKindIdView = DbAtomPermissionKindEnum.Everyone,
                        AtomPermissionKindIdCreate = DbAtomPermissionKindEnum.Everyone,
                        AtomPermissionKindIdEdit = DbAtomPermissionKindEnum.Everyone,
                        AtomPermissionKindIdDelete = DbAtomPermissionKindEnum.Denied,
                    },

                    // 系統設定_角色
                    new CoEmpPmnDbAddManyReqItemMdl()
                    {
                        EmployeeID = DbEmployeeConst.Admin.ID,
                        AtomMenu = DbAtomMenuEnum.SystemRole,
                        ManagerRegionID = DbManagerRegionConst.Denied.ID,
                        AtomPermissionKindIdView = DbAtomPermissionKindEnum.Everyone,
                        AtomPermissionKindIdCreate = DbAtomPermissionKindEnum.Everyone,
                        AtomPermissionKindIdEdit = DbAtomPermissionKindEnum.Everyone,
                    },

                    // 系統設定_部門
                    new CoEmpPmnDbAddManyReqItemMdl()
                    {
                        EmployeeID = DbEmployeeConst.GeneralManager.ID,
                        AtomMenu = DbAtomMenuEnum.SystemDepartment,
                        ManagerRegionID = DbManagerRegionConst.Denied.ID,
                        AtomPermissionKindIdView = DbAtomPermissionKindEnum.Everyone,
                        AtomPermissionKindIdCreate = DbAtomPermissionKindEnum.Everyone,
                        AtomPermissionKindIdEdit = DbAtomPermissionKindEnum.Everyone,
                        AtomPermissionKindIdDelete = DbAtomPermissionKindEnum.Denied,
                    },

                    // 系統設定_地區
                    new CoEmpPmnDbAddManyReqItemMdl()
                    {
                        EmployeeID = DbEmployeeConst.GeneralManager.ID,
                        AtomMenu = DbAtomMenuEnum.SystemRegion,
                        ManagerRegionID = DbManagerRegionConst.Denied.ID,
                        AtomPermissionKindIdView = DbAtomPermissionKindEnum.Everyone,
                        AtomPermissionKindIdCreate = DbAtomPermissionKindEnum.Everyone,
                        AtomPermissionKindIdEdit = DbAtomPermissionKindEnum.Everyone,
                        AtomPermissionKindIdDelete = DbAtomPermissionKindEnum.Denied,
                    },

                    #endregion
                }
            };
            var coEmpPmnDbAddManyRspObj = await this._coEmpPermissionDb.AddManyAsync(coEmpPmnDbAddManyReqObj);
            if (coEmpPmnDbAddManyRspObj == default)
            {
                this._logger.LogError(string.Empty);
                return;
            }

        }

        // 存在，不處理

        #endregion

    }

    /// <summary>排程-初始化-邏輯服務-結束任務</summary>
    public async Task EndJobAsync()
    {
        Console.WriteLine("EndJob");
        await Task.CompletedTask;
    }

}