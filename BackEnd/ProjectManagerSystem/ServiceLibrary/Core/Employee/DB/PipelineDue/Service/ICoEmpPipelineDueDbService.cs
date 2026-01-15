using System.Threading.Tasks;
using ServiceLibrary.Core.Employee.DB.PipelineDue.Format;

namespace ServiceLibrary.Core.Employee.DB.PipelineDue.Service;

/// <summary>核心-員工-商機履約期限-資料庫服務</summary>
public interface ICoEmpPipelineDueDbService
{
    /// <summary>核心-員工-商機履約期限-資料庫-取得</summary>
    public Task<CoEmpPplDueDbGetRspMdl> GetAsync(CoEmpPplDueDbGetReqMdl reqObj);

    /// <summary>核心-員工-商機履約期限-資料庫-取得多筆</summary>
    public Task<CoEmpPplDueDbGetManyRspMdl> GetManyAsync(CoEmpPplDueDbGetManyReqMdl reqObj);

    /// <summary>核心-員工-商機履約期限-資料庫-新增</summary>
    public Task<CoEmpPplDueDbAddRspMdl> AddAsync(CoEmpPplDueDbAddReqMdl reqObj);

    /// <summary>核心-員工-商機履約期限-資料庫-新增多筆</summary>
    public Task<CoEmpPplDueDbAddManyRspMdl> AddManyAsync(CoEmpPplDueDbAddManyReqMdl reqObj);

    /// <summary>核心-員工-商機履約期限-資料庫-更新</summary>
    public Task<CoEmpPplDueDbUpdateRspMdl> UpdateAsync(CoEmpPplDueDbUpdateReqMdl reqObj);

    /// <summary>核心-員工-商機履約期限-資料庫-移除</summary>
    public Task<CoEmpPplDueDbRemoveRspMdl> RemoveAsync(CoEmpPplDueDbRemoveReqMdl reqObj);
}
