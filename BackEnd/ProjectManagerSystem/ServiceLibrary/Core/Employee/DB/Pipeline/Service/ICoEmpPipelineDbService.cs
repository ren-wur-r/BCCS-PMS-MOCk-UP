using System.Threading.Tasks;
using ServiceLibrary.Core.Employee.DB.Pipeline.Format;

namespace ServiceLibrary.Core.Employee.DB.Pipeline.Service;

/// <summary>核心-員工-商機-資料庫服務</summary>
public interface ICoEmpPipelineDbService
{
    /// <summary>核心-員工-商機-資料庫-是否存在</summary>
    public Task<CoEmpPplDbExistRspMdl> ExistAsync(CoEmpPplDbExistReqMdl reqObj);

    /// <summary>核心-員工-商機-資料庫-取得</summary>
    public Task<CoEmpPplDbGetRspMdl> GetAsync(CoEmpPplDbGetReqMdl reqObj);

    /// <summary>核心-員工-商機-資料庫-取得多筆</summary>
    public Task<CoEmpPplDbGetManyRspMdl> GetManyAsync(CoEmpPplDbGetManyReqMdl reqObj);

    /// <summary>核心-員工-商機-資料庫-取得多筆員工商機ID</summary>
    public Task<CoEmpPplDbGetManyEmployeePipelineIDRspMdl> GetManyEmployeePipelineIDAsync(CoEmpPplDbGetManyEmployeePipelineIDReqMdl reqObj);

    /// <summary>核心-員工-商機-資料庫-新增</summary>
    public Task<CoEmpPplDbAddRspMdl> AddAsync(CoEmpPplDbAddReqMdl reqObj);

    /// <summary>核心-員工-商機-資料庫-新增多筆</summary>
    public Task<CoEmpPplDbAddManyRspMdl> AddManyAsync(CoEmpPplDbAddManyReqMdl reqObj);

    /// <summary>核心-員工-商機-資料庫-更新</summary>
    public Task<CoEmpPplDbUpdateRspMdl> UpdateAsync(CoEmpPplDbUpdateReqMdl reqObj);

    /// <summary>核心-員工-商機-資料庫-批次更新商機狀態</summary>
    public Task<CoEmpPplDbUpdateManyAtomPipelineStatusRspMdl> UpdateManyAtomPipelineStatusAsync(CoEmpPplDbUpdateManyAtomPipelineStatusReqMdl reqObj);

    /// <summary>核心-員工-商機-資料庫-移除</summary>
    public Task<CoEmpPplDbRemoveRspMdl> RemoveAsync(CoEmpPplDbRemoveReqMdl reqObj);

    /// <summary>核心-員工-商機-資料庫-移除多筆</summary>
    public Task<CoEmpPplDbRemoveManyRspMdl> RemoveManyAsync(CoEmpPplDbRemoveManyReqMdl reqObj);

    /// <summary>核心-員工-商機-資料庫-取得多筆[筆數]</summary>
    public Task<CoEmpPplDbGetManyCountRspMdl> GetManyCountAsync(CoEmpPplDbGetManyCountReqMdl reqObj);

    /// <summary>核心-員工-商機-資料庫-取得[筆數]</summary>
    public Task<CoEmpPplDbGetCountRspMdl> GetCountAsync(CoEmpPplDbGetCountReqMdl reqObj);

    #region ManagerBackSite.Crm.Phone 管理者後台-CRM-電銷管理

    /// <summary>核心-員工-商機-資料庫-取得[筆數]從[管理者後台-CRM-電銷管理]</summary>
    public Task<CoEmpPplDbGetCountFromMbsCrmPhnRspMdl> GetCountFromMbsCrmPhnAsync(CoEmpPplDbGetCountFromMbsCrmPhnReqMdl reqObj);

    /// <summary>核心-員工-商機-資料庫-取得多筆[管理者後台-CRM-電銷管理]</summary>
    public Task<CoEmpPplDbGetManyFromMbsCrmPhnRspMdl> GetManyFromMbsCrmPhnAsync(CoEmpPplDbGetManyFromMbsCrmPhnReqMdl reqObj);

    #endregion

    #region ManagerBackSite.Crm.Pipeline 管理者後台-CRM-商機管理

    /// <summary>核心-員工-商機-資料庫-取得[筆數]從[管理者後台-CRM-商機管理]</summary>
    public Task<CoEmpPplDbGetCountFromMbsCrmPipelineRspMdl> GetCountFromMbsCrmPipelineAsync(CoEmpPplDbGetCountFromMbsCrmPipelineReqMdl reqObj);

    /// <summary>核心-員工-商機-資料庫-取得多筆[管理者後台-CRM-商機管理]</summary>
    public Task<CoEmpPplDbGetManyFromMbsCrmPipelineRspMdl> GetManyFromMbsCrmPipelineAsync(CoEmpPplDbGetManyFromMbsCrmPipelineReqMdl reqObj);

    /// <summary>核心-員工-商機-資料庫-取得[管理者後台-CRM-商機管理]</summary>
    public Task<CoEmpPplDbGetFromMbsCrmPipelineRspMdl> GetFromMbsCrmPipelineAsync(CoEmpPplDbGetFromMbsCrmPipelineReqMdl reqObj);

    #endregion
}