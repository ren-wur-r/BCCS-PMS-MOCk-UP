using System.Threading.Tasks;
using ServiceLibrary.Core.Manager.DB.RolePermission.Format;

namespace ServiceLibrary.Core.Manager.DB.RolePermission.Service;

/// <summary>核心-管理者-角色權限-資料庫服務</summary>
public interface ICoMgrRolePermissionDbService
{
    /// <summary>核心-管理者-角色權限-資料庫-新增多筆</summary>
    public Task<CoMgrRpDbAddManyRspMdl> AddManyAsync(CoMgrRpDbAddManyReqMdl reqObj);

    /// <summary>核心-管理者-角色權限-資料庫-移除多筆</summary>
    public Task<CoMgrRpDbRemoveManyRspMdl> RemoveManyAsync(CoMgrRpDbRemoveManyReqMdl reqObj);

    /// <summary>核心-管理者-角色權限-資料庫-取得</summary>
    public Task<CoMgrRpDbGetRspMdl> GetAsync(CoMgrRpDbGetReqMdl reqObj);

    /// <summary>核心-管理者-角色權限-資料庫-取得多筆</summary>
    public Task<CoMgrRpDbGetManyRspMdl> GetManyAsync(CoMgrRpDbGetManyReqMdl reqObj);

    #region ManagerBackSite.Basic.RolePermission 管理者後台-基本-角色權限

    /// <summary>核心-管理者-角色權限-資料庫-取得多筆從[管理者後台-基本-角色權限]</summary>
    public Task<CoMgrRpDbGetManyFromMbsBscRspMdl> GetManyFromMbsBscRpAsync(CoMgrRpDbGetManyFromMbsBscReqMdl reqObj);

    #endregion
}