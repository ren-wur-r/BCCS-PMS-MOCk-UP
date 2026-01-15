using System.Threading.Tasks;
using ServiceLibrary.Core.Employee.DB.ProjectStone.Format;

namespace ServiceLibrary.Core.Employee.DB.ProjectStone.Service;

/// <summary>核心-員工-專案里程碑-資料庫服務介面</summary>
public interface ICoEmpProjectStoneDbService
{
    /// <summary>核心-員工-專案里程碑-資料庫-新增</summary>
    public Task<CoEmpPsDbAddRspMdl> AddAsync(CoEmpPsDbAddReqMdl reqObj);

    /// <summary>核心-員工-專案里程碑-資料庫-更新</summary>
    public Task<CoEmpPsDbUpdateRspMdl> UpdateAsync(CoEmpPsDbUpdateReqMdl reqObj);

    /// <summary>核心-員工-專案里程碑-資料庫-更新多筆</summary>
    public Task<CoEmpPsDbUpdateManyRspMdl> UpdateManyAsync(CoEmpPsDbUpdateManyReqMdl reqObj);

    /// <summary>核心-員工-專案里程碑-資料庫-移除</summary>
    public Task<CoEmpPsDbRemoveRspMdl> RemoveAsync(CoEmpPsDbRemoveReqMdl reqObj);

    /// <summary>核心-員工-專案里程碑-取得</summary>
    public Task<CoEmpPsDbGetRspMdl> GetAsync(CoEmpPsDbGetReqMdl reqObj);

    /// <summary>核心-員工-專案里程碑-取得員工專案ID</summary>
    public Task<CoEmpPsDbGetEmployeeProjectIdRspMdl> GetEmployeeProjectIdAsync(CoEmpPsDbGetEmployeeProjectIdReqMdl reqObj);

    /// <summary>核心-員工-專案里程碑-取得[名稱]</summary>
    public Task<CoEmpPsDbGetNameRspMdl> GetNameAsync(CoEmpPsDbGetNameReqMdl reqObj);

    /// <summary>核心-員工-專案里程碑-取得多筆</summary>
    public Task<CoEmpPsDbGetManyRspMdl> GetManyAsync(CoEmpPsDbGetManyReqMdl reqObj);

    /// <summary>核心-員工-專案里程碑-取得多筆[名稱]</summary>
    public Task<CoEmpPsDbGetManyNameRspMdl> GetManyNameAsync(CoEmpPsDbGetManyNameReqMdl reqObj);

    #region 管理者後台-基本

    /// <summary>核心-員工-專案里程碑-資料庫-取得多筆從[管理者後台-基本]</summary>
    public Task<CoEmpPsDbGetManyFromMbsBscRspMdl> GetManyFromMbsBscAsync(CoEmpPsDbGetManyFromMbsBscReqMdl reqObj);

    #endregion

}