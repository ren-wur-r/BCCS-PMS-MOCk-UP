using System.Threading.Tasks;
using ServiceLibrary.Core.Employee.DB.PipelineContacter.Format;

namespace ServiceLibrary.Core.Employee.DB.PipelineContacter.Service;

/// <summary>核心-員工-商機窗口-資料庫服務介面</summary>
public interface ICoEmpPipelineContacterDbService
{
    /// <summary>核心-員工-商機窗口-資料庫-取得</summary>
    public Task<CoEmpPplContDbGetRspMdl> GetAsync(CoEmpPplContDbGetReqMdl reqObj);

    /// <summary>核心-員工-商機窗口-資料庫-取得多筆</summary>
    public Task<CoEmpPplContDbGetManyRspMdl> GetManyAsync(CoEmpPplContDbGetManyReqMdl reqObj);

    /// <summary>核心-員工-商機窗口-資料庫-取得[筆數]</summary>
    public Task<CoEmpPplContDbGetCountRspMdl> GetCountAsync(CoEmpPplContDbGetCountReqMdl reqObj);

    /// <summary>核心-員工-商機窗口-資料庫-新增</summary>
    public Task<CoEmpPplContDbAddRspMdl> AddAsync(CoEmpPplContDbAddReqMdl reqObj);

    /// <summary>核心-員工-商機窗口-資料庫-新增多筆</summary>
    public Task<CoEmpPplContDbAddManyRspMdl> AddManyAsync(CoEmpPplContDbAddManyReqMdl reqObj);

    /// <summary>核心-員工-商機窗口-資料庫-更新</summary>
    public Task<CoEmpPplContDbUpdateRspMdl> UpdateAsync(CoEmpPplContDbUpdateReqMdl reqObj);

    /// <summary>核心-員工-商機窗口-資料庫-移除</summary>
    public Task<CoEmpPplContDbRemoveRspMdl> RemoveAsync(CoEmpPplContDbRemoveReqMdl reqObj);

    /// <summary>核心-員工-商機窗口-資料庫-移除多筆</summary>
    public Task<CoEmpPplContDbRemoveManyRspMdl> RemoveManyAsync(CoEmpPplContDbRemoveManyReqMdl reqObj);

    /// <summary>核心-員工-商機窗口-資料庫-更新多筆[是否為主要商機窗口]</summary>
    public Task<CoEmpPplContDbUpdateManyIsPrimaryRspMdl> UpdateManyIsPrimaryAsync(CoEmpPplContDbUpdateManyIsPrimaryReqMdl reqObj);

    /// <summary>核心-員工-商機窗口-資料庫-移除多筆根據公司ID不匹配</summary>
    public Task<CoEmpPplContDbRemoveManyByCompanyIdMismatchRspMdl> RemoveManyByCompanyIdMismatchAsync(CoEmpPplContDbRemoveManyByCompanyIdMismatchReqMdl reqObj);
}
