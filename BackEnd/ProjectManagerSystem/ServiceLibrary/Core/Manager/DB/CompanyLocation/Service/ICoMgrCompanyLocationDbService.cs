using System.Threading.Tasks;
using ServiceLibrary.Core.Manager.DB.CompanyLocation.Format;

namespace ServiceLibrary.Core.Manager.DB.CompanyLocation.Service;

/// <summary>核心-管理者-公司營業地點-資料庫服務-介面</summary>
public interface ICoMgrCompanyLocationDbService
{
    /// <summary>核心-管理者-公司營業地點-資料庫-是否存在</summary>
    Task<CoMgrCmpLocDbExistRspMdl> ExistAsync(CoMgrCmpLocDbExistReqMdl reqObj);

    /// <summary>核心-管理者-公司營業地點-資料庫-取得多筆</summary>
    Task<CoMgrCmpLocDbGetManyRspMdl> GetManyAsync(CoMgrCmpLocDbGetManyReqMdl reqObj);

    /// <summary>核心-管理者-公司營業地點-資料庫-取得</summary>
    Task<CoMgrCmpLocDbGetRspMdl> GetAsync(CoMgrCmpLocDbGetReqMdl reqObj);

    /// <summary>核心-管理者-公司營業地點-資料庫-新增</summary>
    Task<CoMgrCmpLocDbAddRspMdl> AddAsync(CoMgrCmpLocDbAddReqMdl reqObj);

    /// <summary>核心-管理者-公司營業地點-資料庫-新增多筆</summary>
    Task<CoMgrCmpLocDbAddManyRspMdl> AddManyAsync(CoMgrCmpLocDbAddManyReqMdl reqObj);

    /// <summary>核心-管理者-公司營業地點-資料庫-更新</summary>
    Task<CoMgrCmpLocDbUpdateRspMdl> UpdateAsync(CoMgrCmpLocDbUpdateReqMdl reqObj);

    #region ManagerBackSite.Basic 管理者後台-基本

    /// <summary>核心-管理者-公司營業地點-資料庫-取得多筆從[管理者後台-基本]</summary>
    public Task<CoMgrCmpLocDbGetManyFromMbsBscRspMdl> GetManyFromMbsBscAsync(CoMgrCmpLocDbGetManyFromMbsBscReqMdl reqObj);

    #endregion
}