using System.Threading.Tasks;
using ServiceLibrary.Core.Employee.DB.PipelineOriginal.Format;

namespace ServiceLibrary.Core.Employee.DB.PipelineOriginal.Service;

/// <summary>核心-員工-商機原始資料-資料庫服務</summary>
public interface ICoEmpPipelineOriginalDbService
{
    /// <summary>核心-員工-商機原始資料-資料庫-取得</summary>
    public Task<CoEmpPplOgnDbGetRspMdl> GetAsync(CoEmpPplOgnDbGetReqMdl reqObj);

    /// <summary>核心-員工-商機原始資料-資料庫-取得多筆</summary>
    public Task<CoEmpPplOgnDbGetManyRspMdl> GetManyAsync(CoEmpPplOgnDbGetManyReqMdl reqObj);

    /// <summary>核心-員工-商機原始資料-資料庫-取得多筆從[Email列表]</summary>
    public Task<CoEmpPplOgnDbGetManyByEmailListRspMdl> GetManyByEmailListAsync(CoEmpPplOgnDbGetManyByEmailListReqMdl reqObj);

    /// <summary>核心-員工-商機原始資料-資料庫-新增</summary>
    public Task<CoEmpPplOgnDbAddRspMdl> AddAsync(CoEmpPplOgnDbAddReqMdl reqObj);

    /// <summary>核心-員工-商機原始資料-資料庫-更新</summary>
    public Task<CoEmpPplOgnDbUpdateRspMdl> UpdateAsync(CoEmpPplOgnDbUpdateReqMdl reqObj);

    /// <summary>核心-員工-商機原始資料-資料庫-移除</summary>
    public Task<CoEmpPplOgnDbRemoveRspMdl> RemoveAsync(CoEmpPplOgnDbRemoveReqMdl reqObj);

    /// <summary>核心-員工-商機原始資料-資料庫-移除多筆</summary>
    public Task<CoEmpPplOgnDbRemoveManyRspMdl> RemoveManyAsync(CoEmpPplOgnDbRemoveManyReqMdl reqObj);

    /// <summary>核心-員工-商機原始資料-資料庫-新增多筆</summary>
    public Task<CoEmpPplOgnDbAddManyRspMdl> AddManyAsync(CoEmpPplOgnDbAddManyReqMdl reqObj);

    #region ManagerBackSite.Basic 管理者後台-基本

    /// <summary>核心-員工-商機原始資料-資料庫-取得多筆公司名稱從[管理者後台-基本]</summary>
    public Task<CoEmpPplOgnDbGetManyMgrComNameFromMbsBscRspMdl> GetManyManagerCompanyNameFromMbsBscAsync(CoEmpPplOgnDbGetManyMgrComNameFromMbsBscReqMdl reqObj);

    #endregion

    #region ManagerBackSite.Crm.Phone 管理者後台-CRM-活動管理

    /// <summary>核心-員工-商機原始資料-資料庫-取得[筆數]從[管理者後台-CRM-活動管理]</summary>
    public Task<CoEmpPplOgnDbGetCountFromMbsCrmActRspMdl> GetCountFromMbsCrmActAsync(CoEmpPplOgnDbGetCountFromMbsCrmActReqMdl reqObj);

    /// <summary>核心-員工-商機原始資料-資料庫-取得多筆[管理者後台-CRM-活動管理]</summary>
    public Task<CoEmpPplOgnDbGetManyFromMbsCrmActRspMdl> GetManyFromMbsCrmActAsync(CoEmpPplOgnDbGetManyFromMbsCrmActReqMdl reqObj);

    #endregion
}
