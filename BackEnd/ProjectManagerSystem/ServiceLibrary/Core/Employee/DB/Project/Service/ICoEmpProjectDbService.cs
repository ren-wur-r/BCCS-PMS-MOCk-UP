using System.Threading.Tasks;
using ServiceLibrary.Core.Employee.DB.Project.Format;

namespace ServiceLibrary.Core.Employee.DB.Project.Service;

/// <summary>核心-員工-專案-資料庫服務介面</summary>
public interface ICoEmpProjectDbService
{
    /// <summary>核心-員工-專案-資料庫-是否存在</summary>
    public Task<CoEmpPrjDbExistRspMdl> ExistAsync(CoEmpPrjDbExistReqMdl reqObj);

    /// <summary>核心-員工-專案-資料庫-新增</summary>
    public Task<CoEmpPrjDbAddRspMdl> AddAsync(CoEmpPrjDbAddReqMdl reqObj);

    /// <summary>核心-員工-專案-資料庫-更新</summary>
    public Task<CoEmpPrjDbUpdateRspMdl> UpdateAsync(CoEmpPrjDbUpdateReqMdl reqObj);

    /// <summary>核心-員工-專案-資料庫-更新多筆</summary>
    public Task<CoEmpPrjDbUpdateManyRspMdl> UpdateManyAsync(CoEmpPrjDbUpdateManyReqMdl reqObj);

    /// <summary>核心-員工-專案-資料庫-移除</summary>
    public Task<CoEmpPrjDbRemoveRspMdl> RemoveAsync(CoEmpPrjDbRemoveReqMdl reqObj);

    /// <summary>核心-員工-專案-取得</summary>
    public Task<CoEmpPrjDbGetRspMdl> GetAsync(CoEmpPrjDbGetReqMdl reqObj);

    /// <summary>核心-員工-專案-取得多筆ID</summary>
    public Task<CoEmpPrjDbGetManyIdRspMdl> GetManyIdAsync(CoEmpPrjDbGetManyIdReqMdl reqObj);

    /// <summary>核心-員工-專案-取得多筆[名稱]</summary>
    public Task<CoEmpPrjDbGetManyNameRspMdl> GetManyNameAsync(CoEmpPrjDbGetManyNameReqMdl reqObj);

    #region 管理後台-工作-專案

    /// <summary>核心-員工-專案-取得筆數從[管理後台-工作-專案]</summary>
    public Task<CoEmpPrjDbGetCountFromMbsWrkPrjRspMdl> GetCountFromMbsWrkPrjAsync(CoEmpPrjDbGetCountFromMbsWrkPrjReqMdl reqObj);

    /// <summary>核心-員工-專案-取得多筆從[管理後台-工作-專案]</summary>
    public Task<CoEmpPrjDbGetManyFromMbsWrkPrjRspMdl> GetManyFromMbsWrkPrjAsync(CoEmpPrjDbGetManyFromMbsWrkPrjReqMdl reqObj);

    /// <summary>核心-員工-專案-取得儀錶板從[管理後台-工作-專案]</summary>
    public Task<CoEmpPrjDbGetDashboardFromMbsWrkPrjRspMdl> GetDashboardFromMbsWrkPrjAsync(CoEmpPrjDbGetDashboardFromMbsWrkPrjReqMdl reqObj);

    #endregion

    #region 管理者後台-基本

    /// <summary>核心-員工-專案-取得多筆從[管理者後台-基本]</summary>
    public Task<CoEmpPrjDbGetManyFromMbsBscRspMdl> GetManyFromMbsBscAsync(CoEmpPrjDbGetManyFromMbsBscReqMdl reqObj);

    #endregion

    #region 排程-計時器

    /// <summary>核心-員工-專案-資料庫-取得多筆ID從[排程-計時器]</summary>
    public Task<CoEmpPrjDbGetManyIdFromSchTmrRspMdl> GetManyIdFromSchTmrAsync(CoEmpPrjDbGetManyIdFromSchTmrReqMdl reqObj);

    #endregion

}