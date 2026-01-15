using System.Threading.Tasks;
using ServiceLibrary.Core.Employee.DB.ProjectStoneJob.Format;

namespace ServiceLibrary.Core.Employee.DB.ProjectStoneJob.Service;

/// <summary>核心-員工-專案里程碑工項-資料庫服務介面</summary>
public interface ICoEmpProjectStoneJobDbService
{
    /// <summary>核心-員工-專案里程碑工項-資料庫-新增</summary>
    public Task<CoEmpPsjDbAddRspMdl> AddAsync(CoEmpPsjDbAddReqMdl reqObj);

    /// <summary>核心-員工-專案里程碑工項-資料庫-新增多筆</summary>
    public Task<CoEmpPsjDbAddManyRspMdl> AddManyAsync(CoEmpPsjDbAddManyReqMdl reqObj);

    /// <summary>核心-員工-專案里程碑工項-資料庫-更新</summary>
    public Task<CoEmpPsjDbUpdateRspMdl> UpdateAsync(CoEmpPsjDbUpdateReqMdl reqObj);

    /// <summary>核心-員工-專案里程碑工項-資料庫-更新多筆</summary>
    public Task<CoEmpPsjDbUpdateManyRspMdl> UpdateManyAsync(CoEmpPsjDbUpdateManyReqMdl reqObj);

    /// <summary>核心-員工-專案里程碑工項-資料庫-移除</summary>
    public Task<CoEmpPsjDbRemoveRspMdl> RemoveAsync(CoEmpPsjDbRemoveReqMdl reqObj);

    /// <summary>核心-員工-專案里程碑工項-資料庫-移除多筆</summary>
    public Task<CoEmpPsjDbRemoveManyRspMdl> RemoveManyAsync(CoEmpPsjDbRemoveManyReqMdl reqObj);

    /// <summary>核心-員工-專案里程碑工項-資料庫-取得</summary>
    public Task<CoEmpPsjDbGetRspMdl> GetAsync(CoEmpPsjDbGetReqMdl reqObj);

    /// <summary>核心-員工-專案里程碑工項-資料庫-取得多筆</summary>
    public Task<CoEmpPsjDbGetManyRspMdl> GetManyAsync(CoEmpPsjDbGetManyReqMdl reqObj);

    /// <summary>核心-員工-專案里程碑工項-資料庫-取得多筆ID</summary>
    public Task<CoEmpPsjDbGetManyIdRspMdl> GetManyIdAsync(CoEmpPsjDbGetManyIdReqMdl reqObj);

    /// <summary>核心-員工-專案里程碑工項-資料庫-取得多筆狀態從里程碑ID</summary>
    public Task<CoEmpPsjDbGetManyStatusFromStoneIdRspMdl> GetManyStatusFromStoneIdAsync(CoEmpPsjDbGetManyStatusFromStoneIdReqMdl reqObj);

    #region 管理者後台-基本

    /// <summary>核心-員工-專案里程碑工項-資料庫-取得多筆從[管理者後台-基本]</summary>
    public Task<CoEmpPsjDbGetManyFromMbsBscRspMdl> GetManyFromMbsBscAsync(CoEmpPsjDbGetManyFromMbsBscReqMdl reqObj);

    #endregion
}