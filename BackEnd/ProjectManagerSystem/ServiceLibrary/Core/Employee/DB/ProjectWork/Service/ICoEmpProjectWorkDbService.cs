using System.Threading.Tasks;
using ServiceLibrary.Core.Employee.DB.ProjectWork.Format;

namespace ServiceLibrary.Core.Employee.DB.ProjectWork.Service;

/// <summary>核心-員工-專案工作計劃書-資料庫服務介面</summary>
public interface ICoEmpProjectWorkDbService
{
    /// <summary>核心-員工-專案工作計劃書-新增</summary>
    public Task<CoEmpPwDbAddRspMdl> AddAsync(CoEmpPwDbAddReqMdl reqObj);

    /// <summary>核心-員工-專案工作計劃書-資料庫-移除多筆</summary>
    public Task<CoEmpPwDbRemoveManyRspMdl> RemoveManyAsync(CoEmpPwDbRemoveManyReqMdl reqObj);

    /// <summary>核心-員工-專案工作計劃書-更新多筆</summary>
    public Task<CoEmpPwDbUpdateManyRspMdl> UpdateManyAsync(CoEmpPwDbUpdateManyReqMdl reqObj);

    /// <summary>核心-員工-專案工作計劃書-取得多筆ID</summary>
    public Task<CoEmpPwDbGetManyIdRspMdl> GetManyIdAsync(CoEmpPwDbGetManyIdReqMdl reqObj);

    /// <summary>核心-員工-專案工作計劃書-取得多筆</summary>
    public Task<CoEmpPwDbGetManyRspMdl> GetManyAsync(CoEmpPwDbGetManyReqMdl reqObj);
}