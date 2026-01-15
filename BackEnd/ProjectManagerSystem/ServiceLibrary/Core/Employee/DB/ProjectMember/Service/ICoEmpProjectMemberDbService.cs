using System.Threading.Tasks;
using ServiceLibrary.Core.Employee.DB.ProjectMember.Format;

namespace ServiceLibrary.Core.Employee.DB.ProjectMember.Service;

/// <summary>核心-員工-專案成員-資料庫服務介面</summary>
public interface ICoEmpProjectMemberDbService
{
    /// <summary>核心-員工-專案成員-是否存在</summary>
    public Task<CoEmpPmDbExistRspMdl> ExistAsync(CoEmpPmDbExistReqMdl reqObj);

    /// <summary>核心-員工-專案成員-新增</summary>
    public Task<CoEmpPmDbAddRspMdl> AddAsync(CoEmpPmDbAddReqMdl reqObj);

    /// <summary>核心-員工-專案成員-新增多筆</summary>
    public Task<CoEmpPmemDbAddManyRspMdl> AddManyAsync(CoEmpPmemDbAddManyReqMdl reqObj);

    /// <summary>核心-員工-專案成員-移除</summary>
    public Task<CoEmpPmDbRemoveRspMdl> RemoveAsync(CoEmpPmDbRemoveReqMdl reqObj);

    /// <summary>核心-員工-專案成員-取得</summary>
    public Task<CoEmpPmDbGetRspMdl> GetAsync(CoEmpPmDbGetReqMdl reqObj);

    /// <summary>核心-員工-專案成員-取得多筆</summary>
    public Task<CoEmpPmDbGetManyRspMdl> GetManyAsync(CoEmpPmDbGetManyReqMdl reqObj);

    /// <summary>核心-員工-專案成員-取得多筆員工ID</summary>
    public Task<CoEmpPmDbGetManyEmployeeIdRspMdl> GetManyEmployeeIdAsync(CoEmpPmDbGetManyEmployeeIdReqMdl reqObj);

}