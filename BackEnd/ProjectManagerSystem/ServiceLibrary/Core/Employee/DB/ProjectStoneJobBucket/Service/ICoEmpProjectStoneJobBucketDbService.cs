using System.Threading.Tasks;
using ServiceLibrary.Core.Employee.DB.ProjectStoneJobBucket.Format;

namespace ServiceLibrary.Core.Employee.DB.ProjectStoneJobBucket.Service;

/// <summary>核心-員工-專案里程碑工項清單-資料庫服務介面</summary>
public interface ICoEmpProjectStoneJobBucketDbService
{
    /// <summary>核心-員工-專案里程碑工項清單-資料庫-新增</summary>
    public Task<CoEmpPsjbDbAddRspMdl> AddAsync(CoEmpPsjbDbAddReqMdl reqObj);

    /// <summary>核心-員工-專案里程碑工項清單-資料庫-新增多筆</summary>
    public Task<CoEmpPsjbDbAddManyRspMdl> AddManyAsync(CoEmpPsjbDbAddManyReqMdl reqObj);

    /// <summary>核心-員工-專案里程碑工項清單-資料庫-更新</summary>
    public Task<CoEmpPsjbDbUpdateRspMdl> UpdateAsync(CoEmpPsjbDbUpdateReqMdl reqObj);

    /// <summary>核心-員工-專案里程碑工項清單-資料庫-移除</summary>
    public Task<CoEmpPsjbDbRemoveRspMdl> RemoveAsync(CoEmpPsjbDbRemoveReqMdl reqObj);

    /// <summary>核心-員工-專案里程碑工項清單-資料庫-移除多筆</summary>
    public Task<CoEmpPsjbDbRemoveManyRspMdl> RemoveManyAsync(CoEmpPsjbDbRemoveManyReqMdl reqObj);

    /// <summary>核心-員工-專案里程碑工項清單-資料庫-取得</summary>
    public Task<CoEmpPsjbDbGetRspMdl> GetAsync(CoEmpPsjbDbGetReqMdl reqObj);

    /// <summary>核心-員工-專案里程碑工項清單-資料庫-取得[員工專案ID]</summary>
    public Task<CoEmpPsjbDbGetEmployeeProjectIdRspMdl> GetEmployeeProjectIdAsync(CoEmpPsjbDbGetEmployeeProjectIdReqMdl reqObj);

    /// <summary>核心-員工-專案里程碑工項清單-資料庫-取得多筆</summary>
    public Task<CoEmpPsjbDbGetManyRspMdl> GetManyAsync(CoEmpPsjbDbGetManyReqMdl reqObj);

    /// <summary>核心-員工-專案里程碑工項清單-資料庫-取得多筆從工項ID列表</summary>
    public Task<CoEmpPsjbDbGetManyFromTaskIdListRspMdl> GetManyFromTaskIdListAsync(CoEmpPsjbDbGetManyFromTaskIdListReqMdl reqObj);

    /// <summary>核心-員工-專案里程碑工項清單-資料庫-取得多筆完成狀態從工項ID</summary>
    public Task<CoEmpPsjbDbGetManyIsFinishFromTaskIdRspMdl> GetManyIsFinishFromTaskIdAsync(CoEmpPsjbDbGetManyIsFinishFromTaskIdReqMdl reqObj);
}