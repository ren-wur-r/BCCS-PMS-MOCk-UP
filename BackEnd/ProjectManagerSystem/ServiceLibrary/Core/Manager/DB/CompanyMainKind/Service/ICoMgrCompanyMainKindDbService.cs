using System.Threading.Tasks;
using ServiceLibrary.Core.Manager.DB.CompanyMainKind.Format;

namespace ServiceLibrary.Core.Manager.DB.CompanyMainKind.Service;

/// <summary>核心-管理者-公司主分類-資料庫服務</summary>
public interface ICoMgrCompanyMainKindDbService
{
    /// <summary>核心-管理者-公司主分類-資料庫-取得多筆</summary>
    public Task<CoMgrCmpMainKdDbGetManyRspMdl> GetManyAsync(CoMgrCmpMainKdDbGetManyReqMdl reqObj);

    /// <summary>核心-管理者-公司主分類-資料庫-取得</summary>
    public Task<CoMgrCmpMainKdDbGetRspMdl> GetAsync(CoMgrCmpMainKdDbGetReqMdl reqObj);

    /// <summary>核心-管理者-公司主分類-資料庫-新增</summary>
    public Task<CoMgrCmpMainKdDbAddRspMdl> AddAsync(CoMgrCmpMainKdDbAddReqMdl reqObj);

    /// <summary>核心-管理者-公司主分類-資料庫-新增[指定ID]</summary>
    public Task<CoMgrCmpMainKdDbAddWithIdRspMdl> AddWithIdAsync(CoMgrCmpMainKdDbAddWithIdReqMdl reqObj);

    /// <summary>核心-管理者-公司主分類-資料庫-更新</summary>
    public Task<CoMgrCmpMainKdDbUpdateRspMdl> UpdateAsync(CoMgrCmpMainKdDbUpdateReqMdl reqObj);

    /// <summary>核心-管理者-公司主分類-資料庫-檢查名稱重複</summary>
    public Task<CoMgrCmpMainKdDbCheckNameDuplicateRspMdl> CheckNameDuplicateAsync(CoMgrCmpMainKdDbCheckNameDuplicateReqMdl reqObj);

    /// <summary>核心-管理者-公司主分類-資料庫-取得[名稱]</summary>
    public Task<CoMgrCmpMainKdDbGetNameRspMdl> GetNameAsync(CoMgrCmpMainKdDbGetNameReqMdl reqObj);

    /// <summary>核心-管理者-公司主分類-資料庫-取得多筆[名稱]</summary>
    public Task<CoMgrCmpMainKdDbGetManyNameRspMdl> GetManyNameAsync(CoMgrCmpMainKdDbGetManyNameReqMdl reqObj);

    #region ManagerBackSite.Customer.Company 管理者後台-系統設定-客戶

    /// <summary>核心-管理者-公司主分類-資料庫-取得[筆數]從[管理者後台-系統設定-客戶]</summary>
    public Task<CoMgrCmpMainKdDbGetCountFromMbsSysCmpRspMdl> GetCountFromMbsSysCmpAsync(CoMgrCmpMainKdDbGetCountFromMbsSysCmpReqMdl reqObj);

    /// <summary>核心-管理者-公司主分類-資料庫-取得多筆從[管理者後台-系統設定-客戶]</summary>
    public Task<CoMgrCmpMainKdDbGetManyFromMbsSysCmpRspMdl> GetManyFromMbsSysCmpAsync(CoMgrCmpMainKdDbGetManyFromMbsSysCmpReqMdl reqObj);

    #endregion

    #region ManagerBackSite.Basic 管理者後台-基本

    /// <summary>核心-管理者-公司主分類-資料庫-取得多筆從[管理者後台-基本]</summary>
    public Task<CoMgrCmpMainKdDbGetManyFromMbsBscRspMdl> GetManyFromMbsBscAsync(CoMgrCmpMainKdDbGetManyFromMbsBscReqMdl reqObj);

    #endregion
}