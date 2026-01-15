using System.Threading.Tasks;
using ServiceLibrary.Core.Employee.DB.ProjectStoneJobExecutor.Format;

namespace ServiceLibrary.Core.Employee.DB.ProjectStoneJobExecutor.Service;

/// <summary>核心-員工-專案里程碑工項執行者-資料庫服務介面</summary>
public interface ICoEmpProjectStoneJobExecutorDbService
{
    /// <summary>核心-員工-專案里程碑工項執行者-資料庫-新增</summary>
    public Task<CoEmpPsjeDbAddRspMdl> AddAsync(CoEmpPsjeDbAddReqMdl reqObj);

    /// <summary>核心-員工-專案里程碑工項執行者-資料庫-新增多筆</summary>
    public Task<CoEmpPsjeDbAddManyRspMdl> AddManyAsync(CoEmpPsjeDbAddManyReqMdl reqObj);

    /// <summary>核心-員工-專案里程碑工項執行者-資料庫-移除</summary>
    public Task<CoEmpPsjeDbRemoveRspMdl> RemoveAsync(CoEmpPsjeDbRemoveReqMdl reqObj);

    /// <summary>核心-員工-專案里程碑工項執行者-資料庫-移除多筆</summary>
    public Task<CoEmpPsjeDbRemoveManyRspMdl> RemoveManyAsync(CoEmpPsjeDbRemoveManyReqMdl reqObj);

    /// <summary>核心-員工-專案里程碑工項執行者-取得多筆</summary>
    public Task<CoEmpPsjeDbGetManyRspMdl> GetManyAsync(CoEmpPsjeDbGetManyReqMdl reqObj);

    /// <summary>核心-員工-專案里程碑工項執行者-取得多筆數量從工項ID列表</summary>
    public Task<CoEmpPsjeDbGetManyCountFromTaskIdListRspMdl> GetManyCountFromTaskIdListAsync(CoEmpPsjeDbGetManyCountFromTaskIdListReqMdl reqObj);

    /// <summary>核心-員工-專案里程碑工項執行者-取得數量</summary>
    public Task<CoEmpPsjeDbGetCountRspMdl> GetCountAsync(CoEmpPsjeDbGetCountReqMdl reqObj);

}