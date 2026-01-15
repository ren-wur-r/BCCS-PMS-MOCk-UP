using System.Threading.Tasks;
using ServiceLibrary.Core.Manager.DB.ProductSubKind.Format;

namespace ServiceLibrary.Core.Manager.DB.ProductSubKind.Service;

/// <summary>核心-管理者-產品子分類-資料庫服務</summary>
public interface ICoMgrProductSubKindDbService
{
    /// <summary>核心-管理者-產品子分類-資料庫-是否存在</summary>
    public Task<CoMgrPskDbExistRspMdl> ExistAsync(CoMgrPskDbExistReqMdl reqObj);

    /// <summary>核心-管理者-產品子分類-資料庫-取得多筆</summary>
    public Task<CoMgrPskDbGetManyRspMdl> GetManyAsync(CoMgrPskDbGetManyReqMdl reqObj);

    /// <summary>核心-管理者-產品子分類-資料庫-取得</summary>
    public Task<CoMgrPskDbGetRspMdl> GetAsync(CoMgrPskDbGetReqMdl reqObj);

    /// <summary>核心-管理者-產品子分類-資料庫-新增</summary>
    public Task<CoMgrPskDbAddRspMdl> AddAsync(CoMgrPskDbAddReqMdl reqObj);

    /// <summary>核心-管理者-產品子分類-資料庫-更新</summary>
    public Task<CoMgrPskDbUpdateRspMdl> UpdateAsync(CoMgrPskDbUpdateReqMdl reqObj);

    /// <summary>核心-管理者-產品子分類-資料庫-取得多筆[名稱]</summary>
    public Task<CoMgrPskDbGetManyNameRspMdl> GetManyNameAsync(CoMgrPskDbGetManyNameReqMdl reqObj);

    #region ManagerBackSite.Basic.ProductSubKind 管理者後台-基本-產品

    /// <summary>核心-管理者-產品子分類-資料庫-取得多筆從[管理者後台-基本]</summary>
    public Task<CoMgrPskDbGetManyFromMbsBscRspMdl> GetManyFromMbsBscAsync(CoMgrPskDbGetManyFromMbsBscReqMdl reqObj);

    #endregion

    #region ManagerBackSite.System.ProductSubKind 管理者後台-系統設定-產品

    /// <summary>核心-管理者-產品子分類-資料庫-取得[筆數]從[管理者後台-系統設定-產品[子分類]]</summary>
    public Task<CoMgrPskDbGetCountFromMbsSysPrdRspMdl> GetCountFromMbsSysPrdAsync(CoMgrPskDbGetCountFromMbsSysPrdReqMdl reqObj);

    /// <summary>核心-管理者-產品子分類-資料庫-取得多筆從[管理者後台-系統設定-產品[子分類]]</summary>
    public Task<CoMgrPskDbGetManyFromMbsSysPrdRspMdl> GetManyFromMbsSysPrdAsync(CoMgrPskDbGetManyFromMbsSysPrdReqMdl reqObj);

    #endregion
}