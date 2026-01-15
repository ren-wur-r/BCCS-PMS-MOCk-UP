using System.Threading.Tasks;
using ServiceLibrary.ManagerBackSite.Logical.Basic.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.Basic.Service;

/// <summary>管理者後台-基本-邏輯服務</summary>
public interface IMbsBasicLogicalService
{
    /// <summary>管理者後台-基本-邏輯-登入</summary>
    public Task<MbsBscLgcLoginRspMdl> LoginAsync(MbsBscLgcLoginReqMdl reqObj);

    /// <summary>管理者後台-基本-邏輯-登出</summary>
    public Task<MbsBscLgcLogoutRspMdl> LogoutAsync(MbsBscLgcLogoutReqMdl reqObj);

    /// <summary>管理者後台-基本-邏輯-心跳</summary>
    public Task<MbsBscLgcHeartbeatRspMdl> HeartbeatAsync(MbsBscLgcHeartbeatReqMdl reqObj);

    #region Employee

    /// <summary>管理者後台-基本-邏輯-取得員工資訊</summary>
    public Task<MbsBscLgcGetEmployeeRspMdl> GetEmployeeAsync(MbsBscLgcGetEmployeeReqMdl reqObj);

    /// <summary>管理者後台-基本-邏輯-修改員工密碼</summary>
    public Task<MbsBscLgcUpdatePasswordRspMdl> UpdateEmployeePasswordAsync(MbsBscLgcUpdatePasswordReqMdl reqObj);

    /// <summary>管理者後台-基本-邏輯-取得多筆員工資訊</summary>
    public Task<MbsBscLgcGetManyEmployeeInfoRspMdl> GetManyEmployeeInfoAsync(MbsBscLgcGetManyEmployeeInfoReqMdl reqObj);

    #endregion

    #region EmployeeProjectMember

    /// <summary>管理者後台-基本-邏輯-取得多筆員工專案成員</summary>
    public Task<MbsBscLgcGetManyEmployeeProjectMemberRspMdl> GetManyEmployeeProjectMemberAsync(MbsBscLgcGetManyEmployeeProjectMemberReqMdl reqObj);

    #endregion

    #region EmployeeProject

    /// <summary>管理者後台-基本-邏輯-取得多筆員工專案</summary>
    public Task<MbsBscLgcGetManyProjectRspMdl> GetManyEmployeeProjectAsync(MbsBscLgcGetManyProjectReqMdl reqObj);

    #endregion

    #region EmployeeProjectStone

    /// <summary>管理者後台-基本-邏輯-取得多筆員工專案里程碑</summary>
    public Task<MbsBscLgcGetManyProjectStoneRspMdl> GetManyEmployeeProjectStoneAsync(MbsBscLgcGetManyProjectStoneReqMdl reqObj);

    #endregion

    #region EmployeeProjectStoneJob

    /// <summary>管理者後台-基本-邏輯-取得多筆員工專案里程碑工項</summary>
    public Task<MbsBscLgcGetManyProjectStoneJobRspMdl> GetManyEmployeeProjectStoneJobAsync(MbsBscLgcGetManyProjectStoneJobReqMdl reqObj);

    #endregion

    #region ManagerDepartment

    /// <summary>管理者後台-基本-邏輯-取得多筆管理者部門</summary>
    public Task<MbsBscLgcGetManyDepartmentRspMdl> GetManyManagerDepartmentAsync(MbsBscLgcGetManyDepartmentReqMdl reqObj);

    /// <summary>管理者後台-基本-邏輯-取得全部管理者部門</summary>
    public Task<MbsBscLgcGetAllDepartmentRspMdl> GetAllManagerDepartmentAsync(MbsBscLgcGetAllDepartmentReqMdl reqObj);

    #endregion

    #region ManagerRegion


    /// <summary>管理者後台-基本-邏輯-取得多筆管理者地區</summary>
    public Task<MbsBscLgcGetManyRegionRspMdl> GetManyManagerRegionAsync(MbsBscLgcGetManyRegionReqMdl reqObj);

    /// <summary>管理者後台-基本-邏輯-取得全部管理者地區</summary>
    public Task<MbsBscLgcGetAllRegionRspMdl> GetAllManagerRegionAsync(MbsBscLgcGetAllRegionReqMdl reqObj);

    #endregion

    #region ManagerRole

    /// <summary>管理者後台-基本-邏輯-取得多筆管理者角色</summary>
    public Task<MbsBscLgcGetManyRoleRspMdl> GetManyManagerRoleAsync(MbsBscLgcGetManyRoleReqMdl reqObj);

    #endregion

    #region ManagerRolePermission

    /// <summary>管理者後台-基本-邏輯-取得多筆角色權限從[管理者後台-基本-角色ID]</summary>
    public Task<MbsBscLgcGetManyRolePermissionFromRoleIdRspMdl> GetManyRolePermissionFromRoleAsync(MbsBscLgcGetManyRolePermissionFromRoleIdReqMdl reqObj);

    #endregion

    #region ManagerProduct

    /// <summary>管理者後台-基本-邏輯-取得多筆管理者產品主分類</summary>
    public Task<MbsBscLgcGetManyProductMainKindRspMdl> GetManyManagerProductMainKindAsync(MbsBscLgcGetManyProductMainKindReqMdl reqObj);

    /// <summary>管理者後台-基本-邏輯-取得多筆管理者產品子分類</summary>
    public Task<MbsBscLgcGetManyProductSubKindRspMdl> GetManyManagerProductSubKindAsync(MbsBscLgcGetManyProductSubKindReqMdl reqObj);

    /// <summary>管理者後台-基本-邏輯-取得多筆產品</summary>
    public Task<MbsBscLgcGetManyProductRspMdl> GetManyProductAsync(MbsBscLgcGetManyProductReqMdl reqObj);

    /// <summary>管理者後台-基本-邏輯-取得多筆產品規格</summary>
    public Task<MbsBscLgcGetManyProductSpecificationRspMdl> GetManyProductSpecificationAsync(MbsBscLgcGetManyProductSpecificationReqMdl reqObj);

    #endregion

    #region ManagerContacterRatingReason

    /// <summary>管理者後台-基本-邏輯-取得多筆窗口開發評等原因</summary>
    public Task<MbsBscLgcGetManyContacterRatingReasonRspMdl> GetManyManagerContacterRatingReasonAsync(MbsBscLgcGetManyContacterRatingReasonReqMdl reqObj);

    #endregion

    #region ManagerCompany

    /// <summary>管理者後台-基本-邏輯-取得多筆管理者公司</summary>
    public Task<MbsBscLgcGetManyCompanyRspMdl> GetManyManagerCompanyAsync(MbsBscLgcGetManyCompanyReqMdl reqObj);

    /// <summary>管理者後台-基本-邏輯-取得多筆公司名稱從[商機原始]</summary>
    public Task<MbsBscLgcGetManyCompanyNameFromPipelineOriginalRspMdl> GetManyManagerCompanyNameFromPipelineOriginalAsync(MbsBscLgcGetManyCompanyNameFromPipelineOriginalReqMdl reqObj);

    /// <summary>管理者後台-基本-邏輯-取得管理者公司</summary>
    public Task<MbsBscLgcGetCompanyRspMdl> GetManagerCompanyAsync(MbsBscLgcGetCompanyReqMdl reqObj);

    #endregion

    #region ManagerCompanyMainKind

    /// <summary>管理者後台-基本-邏輯-取得多筆管理者公司主分類</summary>
    public Task<MbsBscLgcGetManyCompanyMainKindRspMdl> GetManyManagerCompanyMainKindAsync(MbsBscLgcGetManyCompanyMainKindReqMdl reqObj);

    #endregion

    #region ManagerCompanySubKind

    /// <summary>管理者後台-基本-邏輯-取得多筆公司子分類</summary>
    public Task<MbsBscLgcGetManyCompanySubKindRspMdl> GetManyManagerCompanySubKindAsync(MbsBscLgcGetManyCompanySubKindReqMdl reqObj);

    #endregion

    #region ManagerContacter

    /// <summary>管理者後台-基本-邏輯-取得多筆管理者窗口</summary>
    public Task<MbsBscLgcGetManyManagerContacterRspMdl> GetManyManagerContacterAsync(MbsBscLgcGetManyManagerContacterReqMdl reqObj);

    /// <summary>管理者後台-基本-邏輯-取得管理者窗口</summary>
    public Task<MbsBscLgcGetManagerContacterRspMdl> GetManagerContacterAsync(MbsBscLgcGetManagerContacterReqMdl reqObj);

    #endregion

    #region ManagerCompanyLocation

    /// <summary>管理者後台-基本-邏輯-取得多筆管理者公司營業地點</summary>
    public Task<MbsBscLgcGetManyCompanyLocationRspMdl> GetManyManagerCompanyLocationAsync(MbsBscLgcGetManyCompanyLocationReqMdl reqObj);

    /// <summary>管理者後台-基本-邏輯-取得管理者公司營業地點</summary>
    public Task<MbsBscLgcGetCompanyLocationRspMdl> GetManagerCompanyLocationAsync(MbsBscLgcGetCompanyLocationReqMdl reqObj);

    #endregion
}
