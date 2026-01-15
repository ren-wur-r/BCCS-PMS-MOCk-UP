using System.Threading.Tasks;
using ServiceLibrary.Core.Employee.DB.PipelineSalerTracking.Format;

namespace ServiceLibrary.Core.Employee.DB.PipelineSalerTracking.Service;

/// <summary>核心-員工-商機業務開發紀錄-資料庫服務</summary>
public interface ICoEmpPipelineSalerTrackingDbService
{
    /// <summary>核心-員工-商機業務開發紀錄-資料庫-取得</summary>
    public Task<CoEmpPplSlrTrkDbGetRspMdl> GetAsync(CoEmpPplSlrTrkDbGetReqMdl reqObj);

    /// <summary>核心-員工-商機業務開發紀錄-資料庫-取得多筆</summary>
    public Task<CoEmpPplSlrTrkDbGetManyRspMdl> GetManyAsync(CoEmpPplSlrTrkDbGetManyReqMdl reqObj);

    /// <summary>核心-員工-商機業務開發紀錄-資料庫-新增</summary>
    public Task<CoEmpPplSlrTrkDbAddRspMdl> AddAsync(CoEmpPplSlrTrkDbAddReqMdl reqObj);

    /// <summary>核心-員工-商機業務開發紀錄-資料庫-新增多筆</summary>
    public Task<CoEmpPplSlrTrkDbAddManyRspMdl> AddManyAsync(CoEmpPplSlrTrkDbAddManyReqMdl reqObj);

    /// <summary>核心-員工-商機業務開發紀錄-資料庫-更新</summary>
    public Task<CoEmpPplSlrTrkDbUpdateRspMdl> UpdateAsync(CoEmpPplSlrTrkDbUpdateReqMdl reqObj);

    /// <summary>核心-員工-商機業務開發紀錄-資料庫-移除</summary>
    public Task<CoEmpPplSlrTrkDbRemoveRspMdl> RemoveAsync(CoEmpPplSlrTrkDbRemoveReqMdl reqObj);

    /// <summary>核心-員工-商機業務開發紀錄-資料庫-移除多筆</summary>
    public Task<CoEmpPplSlrTrkDbRemoveManyRspMdl> RemoveManyAsync(CoEmpPplSlrTrkDbRemoveManyReqMdl reqObj);

    /// <summary>核心-員工-商機業務開發紀錄-資料庫-取得多筆開發時間從商機ID列表</summary>
    public Task<CoEmpPplSlrTrkDbGetManyTrackingTimeFromPipelineIdListRspMdl> GetManyTrackingTimeFromPipelineIdListAsync(CoEmpPplSlrTrkDbGetManyTrackingTimeFromPipelineIdListReqMdl reqObj);

    #region ManagerBackSite.Crm.Pipeline 管理者後台-CRM-商機管理

    /// <summary>核心-員工-商機業務開發紀錄-資料庫-取得多筆[管理者後台-CRM-商機管理]</summary>
    public Task<CoEmpPplSlrTrkDbGetManyFromMbsCrmPplRspMdl> GetManyFromMbsCrmPplAsync(CoEmpPplSlrTrkDbGetManyFromMbsCrmPplReqMdl reqObj);

    #endregion
}
