using System.Threading.Tasks;
using ServiceLibrary.Core.Employee.DB.PipelineBill.Format;

namespace ServiceLibrary.Core.Employee.DB.PipelineBill.Service;

/// <summary>核心-員工-商機發票紀錄-資料庫服務</summary>
public interface ICoEmpPipelineBillDbService
{
    /// <summary>核心-員工-商機發票紀錄-資料庫-取得</summary>
    public Task<CoEmpPplBllDbGetRspMdl> GetAsync(CoEmpPplBllDbGetReqMdl reqObj);

    /// <summary>核心-員工-商機發票紀錄-資料庫-取得多筆</summary>
    public Task<CoEmpPplBllDbGetManyRspMdl> GetManyAsync(CoEmpPplBllDbGetManyReqMdl reqObj);

    /// <summary>核心-員工-商機發票紀錄-資料庫-新增多筆</summary>
    public Task<CoEmpPplBllDbAddManyRspMdl> AddManyAsync(CoEmpPplBllDbAddManyReqMdl reqObj);

    /// <summary>核心-員工-商機發票紀錄-資料庫-更新</summary>
    public Task<CoEmpPplBllDbUpdateRspMdl> UpdateAsync(CoEmpPplBllDbUpdateReqMdl reqObj);

    /// <summary>核心-員工-商機發票紀錄-資料庫-移除多筆</summary>
    public Task<CoEmpPplBllDbRemoveManyRspMdl> RemoveManyAsync(CoEmpPplBllDbRemoveManyReqMdl reqObj);
}