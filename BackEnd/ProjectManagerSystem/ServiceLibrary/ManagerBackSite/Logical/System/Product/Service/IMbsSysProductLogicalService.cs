using System.Threading.Tasks;
using ServiceLibrary.ManagerBackSite.Logical.System.Product.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.System.Product.Service;

/// <summary>管理者後台-系統-產品-邏輯服務</summary>
public interface IMbsSysProductLogicalService
{
    #region Product 產品

    /// <summary>管理者後台-系統-產品-邏輯服務-取得多筆</summary>
    public Task<MbsSysPrdLgcGetManyRspMdl> GetManyAsync(MbsSysPrdLgcGetManyReqMdl reqObj);

    /// <summary>管理者後台-系統-產品-邏輯服務-新增</summary>
    public Task<MbsSysPrdLgcAddRspMdl> AddAsync(MbsSysPrdLgcAddReqMdl reqObj);

    /// <summary>管理者後台-系統-產品-邏輯服務-更新</summary>
    public Task<MbsSysPrdLgcUpdateRspMdl> UpdateAsync(MbsSysPrdLgcUpdateReqMdl reqObj);

    /// <summary>管理者後台-系統-產品-邏輯服務-取得</summary>
    public Task<MbsSysPrdLgcGetRspMdl> GetAsync(MbsSysPrdLgcGetReqMdl reqObj);

    /// <summary>管理者後台-系統-產品-邏輯服務-新增規格</summary>
    public Task<MbsSysPrdLgcAddSpecificationRspMdl> AddSpecificationAsync(MbsSysPrdLgcAddSpecificationReqMdl reqObj);

    /// <summary>管理者後台-系統-產品-邏輯服務-更新規格</summary>
    public Task<MbsSysPrdLgcUpdateSpecificationRspMdl> UpdateSpecificationAsync(MbsSysPrdLgcUpdateSpecificationReqMdl reqObj);

    #endregion

    #region ProductMainKind 產品主分類

    /// <summary>管理者後台-系統-產品-邏輯服務-取得多筆主分類</summary>
    public Task<MbsSysPrdLgcGetManyMainKindRspMdl> GetManyMainKindAsync(MbsSysPrdLgcGetManyMainKindReqMdl reqObj);

    /// <summary>管理者後台-系統-產品-邏輯服務-新增主分類</summary>
    public Task<MbsSysPrdLgcAddMainKindRspMdl> AddMainKindAsync(MbsSysPrdLgcAddMainKindReqMdl reqObj);

    /// <summary>管理者後台-系統-產品-邏輯服務-更新主分類</summary>
    public Task<MbsSysPrdLgcUpdateMainKindRspMdl> UpdateMainKindAsync(MbsSysPrdLgcUpdateMainKindReqMdl reqObj);

    /// <summary>管理者後台-系統-產品-邏輯服務-取得主分類</summary>
    public Task<MbsSysPrdLgcGetMainKindRspMdl> GetMainKindAsync(MbsSysPrdLgcGetMainKindReqMdl reqObj);

    #endregion

    #region ProductSubKind 產品子分類

    /// <summary>管理者後台-系統-產品-邏輯服務-取得多筆子分類</summary>
    public Task<MbsSysPrdLgcGetManySubKindRspMdl> GetManySubKindAsync(MbsSysPrdLgcGetManySubKindReqMdl reqObj);

    /// <summary>管理者後台-系統-產品-邏輯服務-新增子分類</summary>
    public Task<MbsSysPrdLgcAddSubKindRspMdl> AddSubKindAsync(MbsSysPrdLgcAddSubKindReqMdl reqObj);

    /// <summary>管理者後台-系統-產品-邏輯服務-更新子分類</summary>
    public Task<MbsSysPrdLgcUpdateSubKindRspMdl> UpdateSubKindAsync(MbsSysPrdLgcUpdateSubKindReqMdl reqObj);

    /// <summary>管理者後台-系統-產品-邏輯服務-取得子分類</summary>
    public Task<MbsSysPrdLgcGetSubKindRspMdl> GetSubKindAsync(MbsSysPrdLgcGetSubKindReqMdl reqObj);

    #endregion
}