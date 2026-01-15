using System.Threading.Tasks;
using ServiceLibrary.Core.Employee.DB.Permission.Format;

namespace ServiceLibrary.Core.Employee.DB.Permission.Service;

/// <summary>核心-員工-目錄權限-資料庫服務</summary>
public interface ICoEmpPermissionDbService
{
    /// <summary>核心-員工-目錄權限-資料庫-新增多筆</summary>
    public Task<CoEmpPmnDbAddManyRspMdl> AddManyAsync(CoEmpPmnDbAddManyReqMdl reqObj);

    /// <summary>核心-員工-目錄權限-資料庫-移除多筆</summary>
    public Task<CoEmpPmnDbRemoveManyRspMdl> RemoveManyAsync(CoEmpPmnDbRemoveManyReqMdl reqObj);

    /// <summary>核心-員工-目錄權限-資料庫-取得</summary>
    public Task<CoEmpPmnDbGetRspMdl> GetAsync(CoEmpPmnDbGetReqMdl reqObj);

    /// <summary>核心-員工-目錄權限-資料庫-取得多筆</summary>
    public Task<CoEmpPmnDbGetManyRspMdl> GetManyAsync(CoEmpPmnDbGetManyReqMdl reqObj);

}