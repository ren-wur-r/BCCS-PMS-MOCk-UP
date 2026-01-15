using System.Threading.Tasks;
using ServiceLibrary.Core.Employee.DB.ProjectStoneJobWorkFile.Format;

namespace ServiceLibrary.Core.Employee.DB.ProjectStoneJobWorkFile.Service;

/// <summary>核心-員工-專案里程碑工項工作檔案-資料庫服務介面</summary>
public interface ICoEmpProjectStoneJobWorkFileDbService
{
    /// <summary>核心-員工-專案里程碑工項工作檔案-新增</summary>
    public Task<CoEmpPsjwfDbAddRspMdl> AddAsync(CoEmpPsjwfDbAddReqMdl reqObj);

    /// <summary>核心-員工-專案里程碑工項工作檔案-新增多筆</summary>
    public Task<CoEmpPsjwfDbAddManyRspMdl> AddManyAsync(CoEmpPsjwfDbAddManyReqMdl reqObj);

    /// <summary>核心-員工-專案里程碑工項工作檔案-移除多筆</summary>
    public Task<CoEmpPsjwfDbRemoveManyRspMdl> RemoveManyAsync(CoEmpPsjwfDbRemoveManyReqMdl reqObj);

    /// <summary>核心-員工-專案里程碑工項工作檔案-更新</summary>
    public Task<CoEmpPsjwfDbUpdateRspMdl> UpdateAsync(CoEmpPsjwfDbUpdateReqMdl reqObj);

    /// <summary>核心-員工-專案里程碑工項工作檔案-取得多筆</summary>
    public Task<CoEmpPsjwfDbGetManyRspMdl> GetManyAsync(CoEmpPsjwfDbGetManyReqMdl reqObj);

}