using System.Threading.Tasks;
using ServiceLibrary.Core.Employee.DB.ProjectContract.Format;

namespace ServiceLibrary.Core.Employee.DB.ProjectContract.Service;

/// <summary>核心-員工-專案契約-資料庫服務介面</summary>
public interface ICoEmpProjectContractDbService
{
    /// <summary>核心-員工-專案契約-新增</summary>
    public Task<CoEmpPcDbAddRspMdl> AddAsync(CoEmpPcDbAddReqMdl reqObj);

    /// <summary>核心-員工-專案契約-資料庫-移除多筆</summary>
    public Task<CoEmpPcDbRemoveManyRspMdl> RemoveManyAsync(CoEmpPcDbRemoveManyReqMdl reqObj);

    /// <summary>核心-員工-專案契約-更新多筆</summary>
    public Task<CoEmpPcDbUpdateManyRspMdl> UpdateManyAsync(CoEmpPcDbUpdateManyReqMdl reqObj);

    /// <summary>核心-員工-專案契約-取得多筆ID</summary>
    public Task<CoEmpPcDbGetManyIdRspMdl> GetManyIdAsync(CoEmpPcDbGetManyIdReqMdl reqObj);

    /// <summary>核心-員工-專案契約-取得多筆</summary>
    public Task<CoEmpPcDbGetManyRspMdl> GetManyAsync(CoEmpPcDbGetManyReqMdl reqObj);

}