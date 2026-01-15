using System.Threading.Tasks;
using ServiceLibrary.Core.Manager.DB.ProductSpecification.Format;

namespace ServiceLibrary.Core.Manager.DB.ProductSpecification.Service;

/// <summary>核心-管理者-產品規格-資料庫服務</summary>
public interface ICoMgrProductSpecificationDbService
{
    /// <summary>核心-管理者-產品規格-資料庫-是否存在</summary>
    public Task<CoMgrPsDbExistRspMdl> ExistAsync(CoMgrPsDbExistReqMdl reqObj);

    /// <summary>核心-管理者-產品規格-資料庫-新增</summary>
    public Task<CoMgrPsDbAddRspMdl> AddAsync(CoMgrPsDbAddReqMdl reqObj);

    /// <summary>核心-管理者-產品規格-資料庫-新增多筆</summary>
    public Task<CoMgrPsDbAddManyRspMdl> AddManyAsync(CoMgrPsDbAddManyReqMdl reqObj);

    /// <summary>核心-管理者-產品規格-資料庫-更新</summary>
    public Task<CoMgrPsDbUpdateRspMdl> UpdateAsync(CoMgrPsDbUpdateReqMdl reqObj);

    /// <summary>核心-管理者-產品規格-資料庫-取得</summary>
    public Task<CoMgrPsDbGetRspMdl> GetAsync(CoMgrPsDbGetReqMdl reqObj);

    /// <summary>核心-管理者-產品規格-資料庫-移除</summary>
    public Task<CoMgrPsDbRemoveRspMdl> RemoveAsync(CoMgrPsDbRemoveReqMdl reqObj);

    /// <summary>核心-管理者-產品規格-資料庫-取得多筆</summary>
    public Task<CoMgrPsDbGetManyRspMdl> GetManyAsync(CoMgrPsDbGetManyReqMdl reqObj);

    /// <summary>核心-管理者-產品規格-資料庫-取得多筆名稱</summary>
    public Task<CoMgrPdtSpcDbGetManyNameRspMdl> GetManyNameAsync(CoMgrPdtSpcDbGetManyNameReqMdl reqObj);

    /// <summary>核心-管理者-產品規格-資料庫-取得多筆從[產品ID]</summary>
    public Task<CoMgrPsDbGetManyFromManagerProductIDRspMdl> GetManyFromManagerProductIDAsync(CoMgrPsDbGetManyFromManagerProductIDReqMdl reqObj);

    #region ManagerBackSite.Basic.ProductSpecification 管理者後台-基本-產品規格

    /// <summary>核心-管理者-產品規格-資料庫-取得多筆從[管理者後台-基本-產品規格]</summary>
    public Task<CoMgrPsDbGetManyFromMbsBscRspMdl> GetManyFromMbsBscAsync(CoMgrPsDbGetManyFromMbsBscReqMdl reqObj);

    #endregion

    #region ManagerBackSite.System.Product 管理者後台-系統設定-產品規格

    /// <summary>核心-管理者-產品規格-資料庫-取得[筆數]從[管理者後台-系統設定-產品]</summary>
    public Task<CoMgrPsDbGetCountFromMbsSysPrdRspMdl> GetCountFromMbsSysPrdAsync(CoMgrPsDbGetCountFromMbsSysPrdReqMdl reqObj);

    /// <summary>核心-管理者-產品規格-資料庫-取得多筆從[管理者後台-系統設定-產品]</summary>
    public Task<CoMgrPsDbGetManyFromMbsSysPrdRspMdl> GetManyFromMbsSysPrdAsync(CoMgrPsDbGetManyFromMbsSysPrdReqMdl reqObj);

    #endregion
}