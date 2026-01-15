using System.Threading.Tasks;
using ServiceLibrary.ManagerBackSite.Logical.System.Company.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.System.Company.Service;

/// <summary>管理者後台-系統設定-客戶-邏輯服務</summary>
public interface IMbsSysCmpLogicalService
{
    #region 公司主分類

    /// <summary>管理者後台-系統設定-客戶-邏輯服務-取得多筆公司主分類</summary>
    public Task<MbsSysCmpLgcGetManyCompanyMainKindRspMdl> GetManyCompanyMainKindAsync(MbsSysCmpLgcGetManyCompanyMainKindReqMdl reqObj);

    /// <summary>管理者後台-系統設定-客戶-邏輯服務-取得公司主分類</summary>
    public Task<MbsSysCmpLgcGetCompanyMainKindsRspMdl> GetCompanyMainKindAsync(MbsSysCmpLgcGetCompanyMainKindReqMdl reqObj);

    /// <summary>管理者後台-系統設定-客戶-邏輯服務-新增公司主分類</summary>
    public Task<MbsSysCmpLgcAddCompanyMainKindRspMdl> AddCompanyMainKindAsync(MbsSysCmpLgcAddCompanyMainKindReqMdl reqObj);

    /// <summary>管理者後台-系統設定-客戶-邏輯服務-編輯公司主分類</summary>
    public Task<MbsSysCmpLgcUpdateCompanyMainKindRspMdl> UpdateCompanyMainKindAsync(MbsSysCmpLgcUpdateCompanyMainKindReqMdl reqObj);

    #endregion

    #region 公司子分類

    /// <summary>管理者後台-系統設定-客戶-邏輯服務-取得多筆公司子分類</summary>
    public Task<MbsSysCmpLgcGetManyCompanySubKindRspMdl> GetManyCompanySubKindAsync(MbsSysCmpLgcGetManyCompanySubKindReqMdl reqObj);

    /// <summary>管理者後台-系統設定-客戶-邏輯服務-取得公司子分類</summary>
    public Task<MbsSysCmpLgcGetCompanySubKindRspMdl> GetCompanySubKindAsync(MbsSysCmpLgcGetCompanySubKindReqMdl reqObj);

    /// <summary>管理者後台-系統設定-客戶-邏輯服務-新增公司子分類</summary>
    public Task<MbsSysCmpLgcAddCompanySubKindRspMdl> AddCompanySubKindAsync(MbsSysCmpLgcAddCompanySubKindReqMdl reqObj);

    /// <summary>管理者後台-系統設定-客戶-邏輯服務-編輯公司子分類</summary>
    public Task<MbsSysCmpLgcUpdateCompanySubKindRspMdl> UpdateCompanySubKindAsync(MbsSysCmpLgcUpdateCompanySubKindReqMdl reqObj);

    #endregion

    #region 公司

    /// <summary>管理者後台-系統設定-客戶-邏輯服務-取得多筆公司</summary>
    public Task<MbsSysCmpLgcGetManyCompanyRspMdl> GetManyCompanyAsync(MbsSysCmpLgcGetManyCompanyReqMdl reqObj);

    /// <summary>管理者後台-系統設定-客戶-邏輯服務-取得公司</summary>
    public Task<MbsSysCmpLgcGetCompanyRspMdl> GetCompanyAsync(MbsSysCmpLgcGetCompanyReqMdl reqObj);

    /// <summary>管理者後台-系統設定-客戶-邏輯服務-新增公司</summary>
    public Task<MbsSysCmpLgcAddCompanyRspMdl> AddCompanyAsync(MbsSysCmpLgcAddCompanyReqMdl reqObj);

    /// <summary>管理者後台-系統設定-客戶-邏輯服務-編輯公司</summary>
    public Task<MbsSysCmpLgcUpdateCompanyRspMdl> UpdateCompanyAsync(MbsSysCmpLgcUpdateCompanyReqMdl reqObj);

    #endregion

    #region 公司營業地點

    /// <summary>管理者後台-系統設定-客戶-邏輯服務-取得多筆公司營業地點</summary>
    public Task<MbsSysCmpLgcGetManyCompanyLocationRspMdl> GetManyCompanyLocationAsync(MbsSysCmpLgcGetManyCompanyLocationReqMdl reqObj);

    /// <summary>管理者後台-系統設定-客戶-邏輯服務-取得公司營業地點</summary>
    public Task<MbsSysCmpLgcGetCompanyLocationRspMdl> GetCompanyLocationAsync(MbsSysCmpLgcGetCompanyLocationReqMdl reqObj);

    /// <summary>管理者後台-系統設定-客戶-邏輯服務-新增公司營業地點</summary>
    public Task<MbsSysCmpLgcAddCompanyLocationRspMdl> AddCompanyLocationAsync(MbsSysCmpLgcAddCompanyLocationReqMdl reqObj);

    /// <summary>管理者後台-系統設定-客戶-邏輯服務-編輯公司營業地點</summary>
    public Task<MbsSysCmpLgcUpdateCompanyLocationRspMdl> UpdateCompanyLocationAsync(MbsSysCmpLgcUpdateCompanyLocationReqMdl reqObj);

    #endregion

    #region 關係公司

    /// <summary>管理者後台-系統設定-客戶-邏輯服務-取得多筆關係公司</summary>
    public Task<MbsSysCmpLgcGetManyCompanyAffiliateRspMdl> GetManyCompanyAffiliateAsync(MbsSysCmpLgcGetManyCompanyAffiliateReqMdl reqObj);

    /// <summary>管理者後台-系統設定-客戶-邏輯服務-取得關係公司</summary>
    public Task<MbsSysCmpLgcGetCompanyAffiliateRspMdl> GetCompanyAffiliateAsync(MbsSysCmpLgcGetCompanyAffiliateReqMdl reqObj);

    /// <summary>管理者後台-系統設定-客戶-邏輯服務-新增關係公司</summary>
    public Task<MbsSysCmpLgcAddCompanyAffiliateRspMdl> AddCompanyAffiliateAsync(MbsSysCmpLgcAddCompanyAffiliateReqMdl reqObj);

    /// <summary>管理者後台-系統設定-客戶-邏輯服務-編輯關係公司</summary>
    public Task<MbsSysCmpLgcUpdateCompanyAffiliateRspMdl> UpdateCompanyAffiliateAsync(MbsSysCmpLgcUpdateCompanyAffiliateReqMdl reqObj);

    #endregion
}