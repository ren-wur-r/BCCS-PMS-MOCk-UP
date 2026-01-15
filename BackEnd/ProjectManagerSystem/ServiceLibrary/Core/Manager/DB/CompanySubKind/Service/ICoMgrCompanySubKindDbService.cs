using System.Threading.Tasks;
using ServiceLibrary.Core.Manager.DB.CompanySubKind.Format;

namespace ServiceLibrary.Core.Manager.DB.CompanySubKind.Service;

/// <summary>核心-管理者-公司子分類-資料庫服務</summary>
public interface ICoMgrCompanySubKindDbService
{
    /// <summary>核心-管理者-公司子分類-資料庫-取得多筆</summary>
    public Task<CoMgrCmpSubKdDbGetManyRspMdl> GetManyAsync(CoMgrCmpSubKdDbGetManyReqMdl reqObj);

    /// <summary>核心-管理者-公司子分類-資料庫-取得</summary>
    public Task<CoMgrCmpSubKdDbGetRspMdl> GetAsync(CoMgrCmpSubKdDbGetReqMdl reqObj);

    /// <summary>核心-管理者-公司子分類-資料庫-新增</summary>
    public Task<CoMgrCmpSubKdDbAddRspMdl> AddAsync(CoMgrCmpSubKdDbAddReqMdl reqObj);

    /// <summary>核心-管理者-公司子分類-資料庫-新增[指定ID]</summary>
    public Task<CoMgrCmpSubKdDbAddWithIdRspMdl> AddWithIdAsync(CoMgrCmpSubKdDbAddWithIdReqMdl reqObj);

    /// <summary>核心-管理者-公司子分類-資料庫-更新</summary>
    public Task<CoMgrCmpSubKdDbUpdateRspMdl> UpdateAsync(CoMgrCmpSubKdDbUpdateReqMdl reqObj);

    /// <summary>核心-管理者-公司子分類-資料庫-取得[名稱]</summary>
    public Task<CoMgrCmpSubKdDbGetNameRspMdl> GetNameAsync(CoMgrCmpSubKdDbGetNameReqMdl reqObj);

    /// <summary>核心-管理者-公司子分類-資料庫-取得多筆[名稱]</summary>
    public Task<CoMgrCmpSubKdDbGetManyNameRspMdl> GetManyNameAsync(CoMgrCmpSubKdDbGetManyNameReqMdl reqObj);

    #region ManagerBackSite.Customer.Company 管理者後台-系統設定-客戶

    /// <summary>核心-管理者-公司子分類-資料庫-取得[筆數]從[管理者後台-系統設定-客戶]</summary>
    public Task<CoMgrCmpSubKdDbGetCountFromMbsSysCmpRspMdl> GetCountFromMbsSysCmpAsync(CoMgrCmpSubKdDbGetCountFromMbsSysCmpReqMdl reqObj);

    /// <summary>核心-管理者-公司子分類-資料庫-取得多筆從[管理者後台-系統設定-客戶]</summary>
    public Task<CoMgrCmpSubKdDbGetManyFromMbsSysCmpRspMdl> GetManyFromMbsSysCmpAsync(CoMgrCmpSubKdDbGetManyFromMbsSysCmpReqMdl reqObj);

    #endregion

    #region ManagerBackSite.Basic 管理者後台-基本

    /// <summary>核心-管理者-公司子分類-資料庫-取得多筆從[管理者後台-基本]</summary>
    public Task<CoMgrCmpSubKdDbGetManyFromMbsBscRspMdl> GetManyFromMbsBscAsync(CoMgrCmpSubKdDbGetManyFromMbsBscReqMdl reqObj);

    #endregion
}