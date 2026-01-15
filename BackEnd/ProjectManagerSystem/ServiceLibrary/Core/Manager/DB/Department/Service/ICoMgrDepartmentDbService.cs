using System.Threading.Tasks;
using ServiceLibrary.Core.Manager.DB.Department.Format;

namespace ServiceLibrary.Core.Manager.DB.Department.Service;

/// <summary>核心-管理者-部門-資料庫服務</summary>
public interface ICoMgrDepartmentDbService
{
    /// <summary>核心-管理者-部門-資料庫-是否存在</summary>
    public Task<CoMgrDptDbExistRspMdl> ExistAsync(CoMgrDptDbExistReqMdl reqObj);

    /// <summary>核心-管理者-部門-資料庫-新增</summary>
    public Task<CoMgrDptDbAddRspMdl> AddAsync(CoMgrDptDbAddReqMdl reqObj);

    /// <summary>核心-管理者-部門-資料庫-新增[指定ID]</summary>
    public Task<CoMgrDptDbAddWithIdRspMdl> AddWithIdAsync(CoMgrDptDbAddWithIdReqMdl reqObj);

    /// <summary>核心-管理者-部門-資料庫-更新</summary>
    public Task<CoMgrDptDbUpdateRspMdl> UpdateAsync(CoMgrDptDbUpdateReqMdl reqObj);

    /// <summary>核心-管理者-部門-資料庫-移除</summary>
    public Task<CoMgrDptDbRemoveRspMdl> RemoveAsync(CoMgrDptDbRemoveReqMdl reqObj);

    /// <summary>核心-管理者-部門-資料庫-取得[名稱]</summary>
    public Task<CoMgrDptDbGetNameRspMdl> GetNameAsync(CoMgrDptDbGetNameReqMdl reqObj);

    /// <summary>核心-管理者-部門-資料庫-取得多筆</summary>
    public Task<CoMgrDptDbGetManyRspMdl> GetManyAsync(CoMgrDptDbGetManyReqMdl reqObj);

    /// <summary>核心-管理者-部門-資料庫-取得</summary>
    public Task<CoMgrDptDbGetRspMdl> GetAsync(CoMgrDptDbGetReqMdl reqObj);

    #region ManagerBackSite.Basic.Department 管理者後台-基本-部門

    /// <summary>核心-管理者-部門-資料庫-取得多筆從[管理者後台-基本-部門]</summary>
    public Task<CoMgrDptDbGetManyFromMbsBscRspMdl> GetManyFromMbsBscDptAsync(CoMgrDptDbGetManyFromMbsBscReqMdl reqObj);

    /// <summary>核心-管理者-部門-資料庫-取得全部從[管理者後台-基本-部門]</summary>
    public Task<CoMgrDptDbGetAllFromMbsBscRspMdl> GetAllFromMbsBscDptAsync(CoMgrDptDbGetAllFromMbsBscReqMdl reqObj);

    #endregion

    #region ManagerBackSite.System.Department 管理者後台-系統-部門

    /// <summary>核心-管理者-部門-資料庫-取得多筆從[管理者後台-系統-部門]</summary>
    public Task<CoMgrDptDbGetManyFromMbsSysDptRspMdl> GetManyFromMbsSysDptAsync(CoMgrDptDbGetManyFromMbsSysDptReqMdl reqObj);

    /// <summary>核心-管理者-部門-資料庫-取得[筆數]從[管理者後台-系統-部門]</summary>
    public Task<CoMgrDptDbGetCountFromMbsSysDptRspMdl> GetCountFromMbsSysDptAsync(CoMgrDptDbGetCountFromMbsSysDptReqMdl reqObj);

    #endregion

}