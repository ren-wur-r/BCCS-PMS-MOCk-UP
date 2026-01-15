using System.Threading.Tasks;
using ServiceLibrary.ManagerBackSite.Logical.System.Role.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.System.Role.Service;

/// <summary>管理者後台-系統設定-角色-邏輯服務</summary>
public interface IMbsSysRoleLogicalService
{
    /// <summary>管理者後台-系統設定-角色-邏輯-取得多筆</summary>
    public Task<MbsSysRolLgcGetManyRspMdl> GetManyAsync(MbsSysRolLgcGetManyReqMdl reqObj);

    /// <summary>管理者後台-系統設定-角色-邏輯-新增</summary>
    public Task<MbsSysRolLgcAddRspMdl> AddAsync(MbsSysRolLgcAddReqMdl reqObj);

    /// <summary>管理者後台-系統設定-角色-邏輯-更新</summary>
    public Task<MbsSysRolLgcUpdateRspMdl> UpdateAsync(MbsSysRolLgcUpdateReqMdl reqObj);

    /// <summary>管理者後台-系統設定-角色-邏輯-取得</summary>
    public Task<MbsSysRolLgcGetRspMdl> GetAsync(MbsSysRolLgcGetReqMdl reqObj);
}