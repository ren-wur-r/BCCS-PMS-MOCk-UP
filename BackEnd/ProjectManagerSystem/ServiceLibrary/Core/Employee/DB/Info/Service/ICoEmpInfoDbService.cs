using System.Threading.Tasks;
using ServiceLibrary.Core.Employee.DB.Info.Format;

namespace ServiceLibrary.Core.Employee.DB.Info.Service;

/// <summary>核心-員工-資訊-資料庫服務</summary>
public interface ICoEmpInfoDbService
{
    /// <summary>核心-員工-資訊-資料庫-是否存在</summary>
    public Task<CoEmpInfDbExistRspMdl> ExistAsync(CoEmpInfDbExistReqMdl reqObj);

    /// <summary>核心-員工-資訊-資料庫-新增</summary>
    public Task<CoEmpInfDbAddRspMdl> AddAsync(CoEmpInfDbAddReqMdl reqObj);

    /// <summary>核心-員工-資訊-資料庫-新增[指定ID]</summary>
    public Task<CoEmpInfDbAddWithIdRspMdl> AddWithIdAsync(CoEmpInfDbAddWithIdReqMdl reqObj);

    /// <summary>核心-員工-資訊-資料庫-更新</summary>
    public Task<CoEmpInfDbUpdateRspMdl> UpdateAsync(CoEmpInfDbUpdateReqMdl reqObj);

    /// <summary>核心-員工-資訊-資料庫-移除</summary>
    public Task<CoEmpInfDbRemoveRspMdl> RemoveAsync(CoEmpInfDbRemoveReqMdl reqObj);

    /// <summary>核心-員工-資訊-資料庫-取得[ID]</summary>
    public Task<CoEmpInfDbGetIdRspMdl> GetIdAsync(CoEmpInfDbGetIdReqMdl reqObj);

    /// <summary>核心-員工-資訊-資料庫-取得</summary>
    public Task<CoEmpInfDbGetRspMdl> GetAsync(CoEmpInfDbGetReqMdl reqObj);

    /// <summary>核心-員工-資訊-資料庫-取得[筆數]</summary>
    public Task<CoEmpInfDbGetCountRspMdl> GetCountAsync(CoEmpInfDbGetCountReqMdl reqObj);

    /// <summary>核心-員工-資訊-資料庫-取得密碼</summary>
    public Task<CoEmpInfDbGetPasswordRspMdl> GetPasswordAsync(CoEmpInfDbGetPasswordReqMdl reqObj);

    /// <summary>核心-員工-資訊-資料庫-取得多筆[帳號]</summary>
    public Task<CoEmpInfDbGetManyAccountRspMdl> GetManyAccountAsync(CoEmpInfDbGetManyAccountReqMdl reqObj);

    /// <summary>核心-員工-資訊-資料庫-取得[名稱]</summary>
    public Task<CoEmpInfDbGetNameRspMdl> GetNameAsync(CoEmpInfDbGetNameReqMdl reqObj);

    /// <summary>核心-員工-資訊-資料庫-取得多筆[名稱]</summary>
    public Task<CoEmpInfDbGetManyNameRspMdl> GetManyNameAsync(CoEmpInfDbGetManyNameReqMdl reqObj);

    /// <summary>核心-員工-資訊-資料庫-取得多筆[信箱]</summary>
    public Task<CoEmpInfDbGetManyEmailRspMdl> GetManyEmailAsync(CoEmpInfDbGetManyEmailReqMdl reqObj);

    /// <summary>核心-員工-資訊-資料庫-檢查多筆存在</summary>
    public Task<CoEmpInfDbCheckManyExistRspMdl> CheckManyExistAsync(CoEmpInfDbCheckManyExistReqMdl reqObj);

    #region ManagerBackSite.System.Employee 管理者後台-系統-員工

    /// <summary>核心-員工-資訊-資料庫-取得[筆數]從[管理者後台-系統-員工]</summary>
    public Task<CoEmpInfDbGetCountFromMbsSysEmpRspMdl> GetCountFromMbsSysEmpAsync(CoEmpInfDbGetCountFromMbsSysEmpReqMdl reqObj);

    /// <summary>核心-員工-資訊-資料庫-取得多筆[管理者後台-系統-員工]</summary>
    public Task<CoEmpInfDbGetManyFromMbsSysEmpRspMdl> GetManyFromMbsSysEmpAsync(CoEmpInfDbGetManyFromMbsSysEmpReqMdl reqObj);

    #endregion

    #region ManagerBackSite.System.Role 管理者後台-系統-角色

    /// <summary>核心-員工-資訊-資料庫-取得多筆[筆數]依[管理者角色ID]</summary>
    public Task<CoEmpInfDbGetManyCountByManagerRoleIdRspMdl> GetManyCountByManagerRoleIdAsync(CoEmpInfDbGetManyCountByManagerRoleIdReqMdl reqObj);

    /// <summary>核心-員工-資訊-資料庫-取得[筆數]依[管理者角色ID]</summary>
    public Task<CoEmpInfDbGetCountByManagerRoleIdRspMdl> GetCountByManagerRoleIdAsync(CoEmpInfDbGetCountByManagerRoleIdReqMdl reqObj);

    /// <summary>核心-員工-資訊-資料庫-取得多筆[員工ID]依[管理者角色ID]</summary>
    public Task<CoEmpInfDbGetManyIdByManagerRoleIdRspMdl> GetManyIdByManagerRoleIdAsync(CoEmpInfDbGetManyIdByManagerRoleIdReqMdl reqObj);

    #endregion

    #region 複雜查詢

    /// <summary>核心-員工-資訊-資料庫-取得多筆[地區][部門][員工]</summary>
    public Task<CoEmpInfDbGetManyRegionDepartmentEmployeeRspMdl> GetManyRegionDepartmentEmployeeAsync(CoEmpInfDbGetManyRegionDepartmentEmployeeReqMdl reqObj);

    #endregion

    #region ManagerBackSite.Basic 管理者後台-基本

    /// <summary>核心-員工-資訊-資料庫-取得多筆從[管理者後台-基本]</summary>
    public Task<CoEmpInfDbGetManyFromMbsBscRspMdl> GetManyFromMbsBscAsync(CoEmpInfDbGetManyFromMbsBscReqMdl reqObj);

    #endregion
}