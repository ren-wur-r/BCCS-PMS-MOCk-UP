using System.Threading.Tasks;
using ServiceLibrary.Core.Manager.DB.Region.Format;

namespace ServiceLibrary.Core.Manager.DB.Region.Service;

/// <summary>核心-管理者-地區-資料庫服務</summary>
public interface ICoMgrRegionDbService
{
    /// <summary>核心-管理者-地區-資料庫-是否存在</summary>
    public Task<CoMgrRgnDbExistRspMdl> ExistAsync(CoMgrRgnDbExistReqMdl reqObj);

    /// <summary>核心-管理者-地區-資料庫-新增</summary>
    public Task<CoMgrRgnDbAddRspMdl> AddAsync(CoMgrRgnDbAddReqMdl reqObj);

    /// <summary>核心-管理者-地區-資料庫-新增[指定ID]</summary>
    public Task<CoMgrRgnDbAddWithIdRspMdl> AddWithIdAsync(CoMgrRgnDbAddWithIdReqMdl reqObj);

    /// <summary>核心-管理者-地區-資料庫-更新</summary>
    public Task<CoMgrRgnDbUpdateRspMdl> UpdateAsync(CoMgrRgnDbUpdateReqMdl reqObj);

    /// <summary>核心-管理者-地區-資料庫-取得</summary>
    public Task<CoMgrRgnDbGetRspMdl> GetAsync(CoMgrRgnDbGetReqMdl reqObj);

    /// <summary>核心-管理者-地區-資料庫-取得[名稱]</summary>
    public Task<CoMgrRgnDbGetNameRspMdl> GetNameAsync(CoMgrRgnDbGetNameReqMdl reqObj);

    /// <summary>核心-管理者-地區-資料庫-取得多筆</summary>
    public Task<CoMgrRgnDbGetManyRspMdl> GetManyAsync(CoMgrRgnDbGetManyReqMdl reqObj);

    #region ManagerBackSite.Basic.Region 管理者後台-基本-地區

    /// <summary>核心-管理者-地區-資料庫-取得多筆從[管理者後台-基本]</summary>
    public Task<CoMgrRgnDbGetManyFromMbsBscRspMdl> GetManyFromMbsBscRgnAsync(CoMgrRgnDbGetManyFromMbsBscReqMdl reqObj);

    /// <summary>核心-管理者-地區-資料庫-取得全部從[管理者後台-基本-地區]</summary>
    public Task<CoMgrRgnDbGetAllFromMbsBscRspMdl> GetAllFromMbsBscRgnAsync(CoMgrRgnDbGetAllFromMbsBscReqMdl reqObj);

    #endregion

    #region ManagerBackSite.System.Region 管理者後台-系統-地區

    /// <summary>核心-管理者-地區-資料庫-取得多筆從[管理者後台-系統-地區]</summary>
    public Task<CoMgrRgnDbGetManyFromMbsSysRgnRspMdl> GetManyFromMbsSysRgnAsync(CoMgrRgnDbGetManyFromMbsSysRgnReqMdl reqObj);

    /// <summary>核心-管理者-地區-資料庫-取得[筆數]從[管理者後台-系統-地區]</summary>
    public Task<CoMgrRgnDbGetCountFromMbsSysRgnRspMdl> GetCountFromMbsSysRgnAsync(CoMgrRgnDbGetCountFromMbsSysRgnReqMdl reqObj);

    #endregion

}