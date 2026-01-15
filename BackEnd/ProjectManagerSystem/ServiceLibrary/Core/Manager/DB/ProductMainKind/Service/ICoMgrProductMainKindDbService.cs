using System.Threading.Tasks;
using ServiceLibrary.Core.Manager.DB.ProductMainKind.Format;

namespace ServiceLibrary.Core.Manager.DB.ProductMainKind.Service;

/// <summary>核心-管理者-產品主分類-資料庫服務</summary>
public interface ICoMgrProductMainKindDbService
{
    /// <summary>核心-管理者-產品主分類-資料庫-是否存在</summary>
    public Task<CoMgrPmkDbExistRspMdl> ExistAsync(CoMgrPmkDbExistReqMdl reqObj);

    /// <summary>核心-管理者-產品主分類-資料庫-新增</summary>
    public Task<CoMgrPmkDbAddRspMdl> AddAsync(CoMgrPmkDbAddReqMdl reqObj);

    /// <summary>核心-管理者-產品主分類-資料庫-更新</summary>
    public Task<CoMgrPmkDbUpdateRspMdl> UpdateAsync(CoMgrPmkDbUpdateReqMdl reqObj);

    /// <summary>核心-管理者-產品主分類-資料庫-取得</summary>
    public Task<CoMgrPmkDbGetRspMdl> GetAsync(CoMgrPmkDbGetReqMdl reqObj);

    /// <summary>核心-管理者-產品主分類-資料庫-取得多筆</summary>
    public Task<CoMgrPmkDbGetManyRspMdl> GetManyAsync(CoMgrPmkDbGetManyReqMdl reqObj);

    /// <summary>核心-管理者-產品主分類-資料庫-取得多筆[名稱]</summary>
    public Task<CoMgrPmkDbGetManyNameRspMdl> GetManyNameAsync(CoMgrPmkDbGetManyNameReqMdl reqObj);

    #region ManagerBackSite.Basic.ProductMainKind 管理者後台-基本-產品

    /// <summary>核心-管理者-產品主分類-資料庫-取得多筆從[管理者後台-基本]</summary>
    public Task<CoMgrPmkDbGetManyFromMbsBscRspMdl> GetManyFromMbsBscAsync(CoMgrPmkDbGetManyFromMbsBscReqMdl reqObj);

    #endregion

    #region ManagerBackSite.System.ProductMainKind 管理者後台-系統設定-產品

    /// <summary>核心-管理者-產品主分類-資料庫-取得[筆數]從[管理者後台-系統設定-產品]</summary>
    public Task<CoMgrPmkDbGetCountFromMbsSysPrdRspMdl> GetCountFromMbsSysPrdAsync(CoMgrPmkDbGetCountFromMbsSysPrdReqMdl reqObj);

    /// <summary>核心-管理者-產品主分類-資料庫-取得多筆從[管理者後台-系統設定-產品]</summary>
    public Task<CoMgrPmkDbGetManyFromMbsSysPrdRspMdl> GetManyFromMbsSysPrdAsync(CoMgrPmkDbGetManyFromMbsSysPrdReqMdl reqObj);

    #endregion
}