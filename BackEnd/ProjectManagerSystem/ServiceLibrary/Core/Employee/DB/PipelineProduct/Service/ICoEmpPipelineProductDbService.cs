using System.Threading.Tasks;
using ServiceLibrary.Core.Employee.DB.PipelineProduct.Format;

namespace ServiceLibrary.Core.Employee.DB.PipelineProduct.Service;

/// <summary>核心-員工-商機產品-資料庫服務介面</summary>
public interface ICoEmpPipelineProductDbService
{
    /// <summary>核心-員工-商機產品-資料庫-是否存在</summary>
    public Task<CoEmpPplPrdDbExistRspMdl> ExistAsync(CoEmpPplPrdDbExistReqMdl reqObj);

    /// <summary>核心-員工-商機產品-資料庫-取得多筆</summary>
    public Task<CoEmpPplPrdDbGetManyRspMdl> GetManyAsync(CoEmpPplPrdDbGetManyReqMdl reqObj);

    /// <summary>核心-員工-商機產品-資料庫-取得</summary>
    public Task<CoEmpPplPrdDbGetRspMdl> GetAsync(CoEmpPplPrdDbGetReqMdl reqObj);

    /// <summary>核心-員工-商機產品-資料庫-新增</summary>
    public Task<CoEmpPplPrdDbAddRspMdl> AddAsync(CoEmpPplPrdDbAddReqMdl reqObj);

    /// <summary>核心-員工-商機產品-資料庫-新增多筆</summary>
    public Task<CoEmpPplPrdDbAddManyRspMdl> AddManyAsync(CoEmpPplPrdDbAddManyReqMdl reqObj);

    /// <summary>核心-員工-商機產品-資料庫-取得數量</summary>
    public Task<CoEmpPplPrdDbGetCountRspMdl> GetCountAsync(CoEmpPplPrdDbGetCountReqMdl reqObj);

    /// <summary>核心-員工-商機產品-資料庫-更新</summary>
    public Task<CoEmpPplPrdDbUpdateRspMdl> UpdateAsync(CoEmpPplPrdDbUpdateReqMdl reqObj);

    /// <summary>核心-員工-商機產品-資料庫-更新多筆公司ID</summary>
    public Task<CoEmpPplPrdDbUpdateManyCompanyIdRspMdl> UpdateManyCompanyIdAsync(CoEmpPplPrdDbUpdateManyCompanyIdReqMdl reqObj);

    /// <summary>核心-員工-商機產品-資料庫-移除</summary>
    public Task<CoEmpPplPrdDbRemoveRspMdl> RemoveAsync(CoEmpPplPrdDbRemoveReqMdl reqObj);

    /// <summary>核心-員工-商機產品-資料庫-移除多筆</summary>
    public Task<CoEmpPplPrdDbRemoveManyRspMdl> RemoveManyAsync(CoEmpPplPrdDbRemoveManyReqMdl reqObj);

    #region 電銷管理
    /// <summary>核心-員工-商機產品-資料庫-取得多筆從[管理者後台-CRM-電銷管理]</summary>
    public Task<CoEmpPplPrdDbGetManyFromPhoneRspMdl> GetManyFromPhoneAsync(CoEmpPplPrdDbGetManyFromPhoneReqMdl reqObj);

    #endregion

    #region ManagerBackSite.Crm.Pipeline 管理者後台-CRM-商機管理

    /// <summary>核心-管理者-商機產品-資料庫-取得多筆從[管理者後台-CRM-商機管理]</summary>
    public Task<CoEmpPplPrdDbGetManyFromCrmPplRspMdl> GetManyFromMbsCrmPplAsync(CoEmpPplPrdDbGetManyFromCrmPplReqMdl reqObj);

    #endregion

    /// <summary>核心-員工-商機產品-資料庫-移除多筆根據公司ID不匹配</summary>
    public Task<CoEmpPplPrdDbRemoveManyByCompanyIdMismatchRspMdl> RemoveManyByCompanyIdMismatchAsync(CoEmpPplPrdDbRemoveManyByCompanyIdMismatchReqMdl reqObj);

}