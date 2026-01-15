using System.Threading.Tasks;
using ServiceLibrary.Core.Manager.DB.Product.Format;

namespace ServiceLibrary.Core.Manager.DB.Product.Service;

/// <summary>核心-管理者-產品-資料庫服務</summary>
public interface ICoMgrProductDbService
{
    /// <summary>核心-管理者-產品-資料庫-是否存在</summary>
    public Task<CoMgrPrdDbExistRspMdl> ExistAsync(CoMgrPrdDbExistReqMdl reqObj);

    /// <summary>核心-管理者-產品-資料庫-新增</summary>
    public Task<CoMgrPrdDbAddRspMdl> AddAsync(CoMgrPrdDbAddReqMdl reqObj);

    /// <summary>核心-管理者-產品-資料庫-更新</summary>
    public Task<CoMgrPrdDbUpdateRspMdl> UpdateAsync(CoMgrPrdDbUpdateReqMdl reqObj);

    /// <summary>核心-管理者-產品-資料庫-移除</summary>
    public Task<CoMgrPrdDbRemoveRspMdl> RemoveAsync(CoMgrPrdDbRemoveReqMdl reqObj);

    /// <summary>核心-管理者-產品-資料庫-取得</summary>
    public Task<CoMgrPrdDbGetRspMdl> GetAsync(CoMgrPrdDbGetReqMdl reqObj);

    /// <summary>核心-管理者-產品-資料庫-取得多筆</summary>
    public Task<CoMgrPrdDbGetManyRspMdl> GetManyAsync(CoMgrPrdDbGetManyReqMdl reqObj);

    /// <summary>核心-管理者-產品-資料庫-取得多筆[名稱]</summary>
    public Task<CoMgrPrdDbGetManyNameRspMdl> GetManyNameAsync(CoMgrPrdDbGetManyNameReqMdl reqObj);

    /// <summary>核心-管理者-產品-資料庫-取得多筆從[產品ID]</summary>
    public Task<CoMgrPrdDbGetManyFromManagerProductIDRspMdl> GetManyFromManagerProductIDAsync(CoMgrPrdDbGetManyFromManagerProductIDReqMdl reqObj);

    #region ManagerBackSite.System.Product 管理者後台-系統設定-產品

    /// <summary>核心-管理者-產品-資料庫-取得[筆數]從[管理者後台-系統設定-產品]</summary>
    public Task<CoMgrPrdDbGetCountFromMbsSysPrdRspMdl> GetCountFromMbsSysPrdAsync(CoMgrPrdDbGetCountFromMbsSysPrdReqMdl reqObj);

    /// <summary>核心-管理者-產品-資料庫-取得多筆從[管理者後台-系統設定-產品]</summary>
    public Task<CoMgrPrdDbGetManyFromMbsSysPrdRspMdl> GetManyFromMbsSysPrdAsync(CoMgrPrdDbGetManyFromMbsSysPrdReqMdl reqObj);

    #endregion

    #region ManagerBackSite.Marketing.Activity 管理者後台-CRM-活動管理

    /// <summary>核心-管理者-產品-資料庫-取得多筆從[管理者後台-CRM-活動管理]</summary>
    public Task<CoMgrPrdDbGetManyFromMbsMktActRspMdl> GetManyFromMbsMktActAsync(CoMgrPrdDbGetManyFromMbsMktActReqMdl reqObj);

    #endregion

    #region ManagerBackSite.Basic 管理者後台-基本

    /// <summary>核心-管理者-產品-資料庫-取得多筆從[管理者後台-基本]</summary>
    public Task<CoMgrPrdDbGetManyFromMbsBscRspMdl> GetManyFromMbsBscAsync(CoMgrPrdDbGetManyFromMbsBscReqMdl reqObj);

    #endregion
}