using System.Threading.Tasks;
using ServiceLibrary.Core.Manager.DB.Role.Format;

namespace ServiceLibrary.Core.Manager.DB.Role.Service;

/// <summary>核心-管理者-角色-資料庫服務</summary>
public interface ICoMgrRoleDbService
{
    /// <summary>核心-管理者-角色-資料庫-是否存在</summary>
    public Task<CoMgrRolDbExistRspMdl> ExistAsync(CoMgrRolDbExistReqMdl reqObj);

    /// <summary>核心-管理者-角色-資料庫-新增</summary>
    public Task<CoMgrRolDbAddRspMdl> AddAsync(CoMgrRolDbAddReqMdl reqObj);

    /// <summary>核心-管理者-角色-資料庫-新增[指定ID]</summary>
    public Task<CoMgrRolDbAddWithIdRspMdl> AddWithIdAsync(CoMgrRolDbAddWithIdReqMdl reqObj);

    /// <summary>核心-管理者-角色-資料庫-更新</summary>
    public Task<CoMgrRolDbUpdateRspMdl> UpdateAsync(CoMgrRolDbUpdateReqMdl reqObj);

    /// <summary>核心-管理者-角色-資料庫-移除</summary>
    public Task<CoMgrRolDbRemoveRspMdl> RemoveAsync(CoMgrRolDbRemoveReqMdl reqObj);

    /// <summary>核心-管理者-角色-資料庫-取得</summary>
    public Task<CoMgrRolDbGetRspMdl> GetAsync(CoMgrRolDbGetReqMdl reqObj);

    /// <summary>核心-管理者-角色-資料庫-取得[名稱]</summary>
    public Task<CoMgrRolDbGetNameRspMdl> GetNameAsync(CoMgrRolDbGetNameReqMdl reqObj);

    /// <summary>核心-管理者-角色-資料庫-取得多筆</summary>
    public Task<CoMgrRolDbGetManyRspMdl> GetManyAsync(CoMgrRolDbGetManyReqMdl reqObj);

    #region ManagerBackSite.Basic.Role 管理者後台-基本-角色

    /// <summary>核心-管理者-角色-資料庫-取得多筆從[管理者後台-基本]</summary>
    public Task<CoMgrRolDbGetManyFromMbsBscRspMdl> GetManyFromMbsBscRolAsync(CoMgrRolDbGetManyFromMbsBscReqMdl reqObj);

    #endregion

    #region ManagerBackSite.System.Role 管理者後台-系統-角色

    /// <summary>核心-管理者-角色-資料庫-取得[筆數]從[管理者後台-系統-角色]</summary>
    public Task<CoMgrRolDbGetCountFromMbsSysRolRspMdl> GetCountFromMbsSysRolAsync(CoMgrRolDbGetCountFromMbsSysRolReqMdl reqObj);

    /// <summary>核心-管理者-角色-資料庫-取得多筆從[管理者後台-系統-角色]</summary>
    public Task<CoMgrRolDbGetManyFromMbsSysRolRspMdl> GetManyFromMbsSysRolAsync(CoMgrRolDbGetManyFromMbsSysRolReqMdl reqObj);

    #endregion

}