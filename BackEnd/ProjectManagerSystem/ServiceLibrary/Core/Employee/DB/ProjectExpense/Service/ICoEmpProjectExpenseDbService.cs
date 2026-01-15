using System.Threading.Tasks;
using ServiceLibrary.Core.Employee.DB.ProjectExpense.Format;

namespace ServiceLibrary.Core.Employee.DB.ProjectExpense.Service;

/// <summary>核心-員工-專案支出-資料庫服務</summary>
public interface ICoEmpProjectExpenseDbService
{
    /// <summary>核心-員工-專案支出-資料庫-取得</summary>
    public Task<CoEmpPrjExpDbGetRspMdl> GetAsync(CoEmpPrjExpDbGetReqMdl reqObj);

    /// <summary>核心-員工-專案支出-資料庫-取得員工專案ID</summary>
    public Task<CoEmpPrjExpDbGetEmployeeProjectIdRspMdl> GetEmployeeProjectIdAsync(CoEmpPrjExpDbGetEmployeeProjectIdReqMdl reqObj);

    /// <summary>核心-員工-專案支出-資料庫-取得多筆</summary>
    public Task<CoEmpPrjExpDbGetManyRspMdl> GetManyAsync(CoEmpPrjExpDbGetManyReqMdl reqObj);

    /// <summary>核心-員工-專案支出-資料庫-新增</summary>
    public Task<CoEmpPrjExpDbAddRspMdl> AddAsync(CoEmpPrjExpDbAddReqMdl reqObj);

    /// <summary>核心-員工-專案支出-資料庫-更新</summary>
    public Task<CoEmpPrjExpDbUpdateRspMdl> UpdateAsync(CoEmpPrjExpDbUpdateReqMdl reqObj);

    /// <summary>核心-員工-專案支出-資料庫-移除</summary>
    public Task<CoEmpPrjExpDbRemoveRspMdl> RemoveAsync(CoEmpPrjExpDbRemoveReqMdl reqObj);
}