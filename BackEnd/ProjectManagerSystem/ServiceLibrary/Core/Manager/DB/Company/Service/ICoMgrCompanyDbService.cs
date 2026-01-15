using System.Threading.Tasks;
using ServiceLibrary.Core.Manager.DB.Company.Format;

namespace ServiceLibrary.Core.Manager.DB.Company.Service;

/// <summary>核心-管理者-公司-資料庫服務-介面</summary>
public interface ICoMgrCompanyDbService
{
    /// <summary>核心-管理者-公司-資料庫-是否存在</summary>
    public Task<CoMgrComDbExistRspMdl> ExistAsync(CoMgrComDbExistReqMdl reqObj);

    /// <summary>核心-管理者-公司-資料庫-統一編號是否存在</summary>
    public Task<CoMgrComDbUniNumExistRspMdl> UniNumExistAsync(CoMgrComDbUniNumExistReqMdl reqObj);

    /// <summary>核心-管理者-公司-資料庫-取得多筆</summary>
    public Task<CoMgrCmpDbGetManyRspMdl> GetManyAsync(CoMgrCmpDbGetManyReqMdl reqObj);

    /// <summary>核心-管理者-公司-資料庫-取得</summary>
    public Task<CoMgrCmpDbGetRspMdl> GetAsync(CoMgrCmpDbGetReqMdl reqObj);

    /// <summary>核心-管理者-公司-資料庫-取得[名稱]</summary>
    public Task<CoMgrCmpDbGetNameRspMdl> GetNameAsync(CoMgrCmpDbGetNameReqMdl reqObj);

    /// <summary>核心-管理者-公司-資料庫-新增</summary>
    public Task<CoMgrCmpDbAddRspMdl> AddAsync(CoMgrCmpDbAddReqMdl reqObj);

    /// <summary>核心-管理者-公司-資料庫-新增[指定ID]</summary>
    public Task<CoMgrCmpDbAddWithIdRspMdl> AddWithIdAsync(CoMgrCmpDbAddWithIdReqMdl reqObj);

    /// <summary>核心-管理者-公司-資料庫-更新</summary>
    public Task<CoMgrCmpDbUpdateRspMdl> UpdateAsync(CoMgrCmpDbUpdateReqMdl reqObj);

    /// <summary>核心-管理者-公司-資料庫-移除</summary>
    public Task<CoMgrCmpDbRemoveRspMdl> RemoveAsync(CoMgrCmpDbRemoveReqMdl reqObj);

    /// <summary>核心-管理者-公司-資料庫-取得多筆[名稱]</summary>
    public Task<CoMgrCmpDbGetManyNameRspMdl> GetManyNameAsync(CoMgrCmpDbGetManyNameReqMdl reqObj);

    #region ManagerBackSite.Customer.Company 管理者後台-系統設定-客戶

    /// <summary>核心-管理者-公司-資料庫-取得[筆數]從[管理者後台-系統設定-客戶]</summary>
    public Task<CoMgrCmpDbGetCountFromMbsSysCmpRspMdl> GetCountFromMbsSysCmpAsync(CoMgrCmpDbGetCountFromMbsSysCmpReqMdl reqObj);

    /// <summary>核心-管理者-公司-資料庫-取得多筆從[管理者後台-系統設定-客戶]</summary>
    public Task<CoMgrCmpDbGetManyFromMbsSysCmpRspMdl> GetManyFromMbsSysCmpAsync(CoMgrCmpDbGetManyFromMbsSysCmpReqMdl reqObj);

    #endregion

    #region ManagerBackSite.Basic 管理者後台-基本

    /// <summary>核心-管理者-公司-資料庫-取得多筆從[管理者後台-基本]</summary>
    public Task<CoMgrCmpDbGetManyFromMbsBscRspMdl> GetManyFromMbsBscAsync(CoMgrCmpDbGetManyFromMbsBscReqMdl reqObj);

    #endregion
}