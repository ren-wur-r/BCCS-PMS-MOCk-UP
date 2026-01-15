using System.Threading.Tasks;
using ServiceLibrary.Core.Employee.DB.ProjectStoneJobWork.Format;

namespace ServiceLibrary.Core.Employee.DB.ProjectStoneJobWork.Service;

/// <summary>核心-員工-專案里程碑工項工作-資料庫服務介面</summary>
public interface ICoEmpProjectStoneJobWorkDbService
{
    /// <summary>核心-員工-專案里程碑工項工作-新增</summary>
    public Task<CoEmpPsjwDbAddRspMdl> AddAsync(CoEmpPsjwDbAddReqMdl reqObj);

    /// <summary>核心-員工-專案里程碑工項工作-更新</summary>
    public Task<CoEmpPsjwDbUpdateRspMdl> UpdateAsync(CoEmpPsjwDbUpdateReqMdl reqObj);

    /// <summary>核心-員工-專案里程碑工項工作-取得單筆</summary>
    public Task<CoEmpPsjwDbGetRspMdl> GetAsync(CoEmpPsjwDbGetReqMdl reqObj);

    /// <summary>核心-員工-專案里程碑工項工作-取得多筆</summary>
    public Task<CoEmpPsjwDbGetManyRspMdl> GetManyAsync(CoEmpPsjwDbGetManyReqMdl reqObj);

    /// <summary>核心-員工-專案里程碑工項工作-移除</summary>
    public Task<CoEmpPsjwDbRemoveRspMdl> RemoveAsync(CoEmpPsjwDbRemoveReqMdl reqObj);

    #region 管理者後台-工作-工項 MbsWrkJob

    /// <summary>核心-員工-專案里程碑工項工作-取得筆數從[管理者後台-工作-工項]</summary>
    public Task<CoEmpPsjwDbGetCountFromMbsWrkJobRspMdl> GetCountFromMbsWrkJobAsync(CoEmpPsjwDbGetCountFromMbsWrkJobReqMdl reqObj);

    /// <summary>核心-員工-專案里程碑工項工作-取得多筆從[管理者後台-工作-工項]</summary>
    public Task<CoEmpPsjwDbGetManyFromMbsWrkJobRspMdl> GetManyFromMbsWrkJobAsync(CoEmpPsjwDbGetManyFromMbsWrkJobReqMdl reqObj);

    #endregion

    #region 管理者後台-工作-工作紀錄 MbsWrkJobRec

    /// <summary>核心-員工-專案里程碑工項工作-取得筆數從[管理者後台-工作-工作紀錄]</summary>
    public Task<CoEmpPsjwDbGetCountFromMbsWrkJobRecRspMdl> GetCountFromMbsWrkJobRecAsync(CoEmpPsjwDbGetCountFromMbsWrkJobRecReqMdl reqObj);

    /// <summary>核心-員工-專案里程碑工項工作-取得多筆從[管理者後台-工作-工作紀錄]</summary>
    public Task<CoEmpPsjwDbGetManyFromMbsWrkJobRecRspMdl> GetManyFromMbsWrkJobRecAsync(CoEmpPsjwDbGetManyFromMbsWrkJobRecReqMdl reqObj);

    #endregion
}