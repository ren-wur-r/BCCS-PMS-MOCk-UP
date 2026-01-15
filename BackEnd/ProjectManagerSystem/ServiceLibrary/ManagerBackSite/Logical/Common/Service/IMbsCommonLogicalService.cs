using System.Threading.Tasks;
using ServiceLibrary.ManagerBackSite.Logical.Common.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.Common.Service;

/// <summary>管理者後台-通用-邏輯服務</summary>
public interface IMbsCommonLogicalService
{
    /// <summary>管理者後台-通用-邏輯-取得登入使用者資訊</summary>
    public Task<MbsCmnLgcGetLoginUserInfoRspMdl> GetLoginUserInfoAsync(MbsCmnLgcGetLoginUserInfoReqMdl reqObj);

    /// <summary>管理者後台-通用-邏輯-登出</summary>
    public Task<MbsCmnLgcLogoutRspMdl> LogoutAsync(MbsCmnLgcLogoutReqMdl reqObj);

    /// <summary>管理者後台-通用-邏輯-檢查與更新專案狀態</summary>
    public Task CheckAndUpdateProjectStatusAsync(MbsCmnLgcCheckAndUpdateProjectStatusReqMdl reqObj);

}