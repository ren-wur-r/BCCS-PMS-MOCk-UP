using System.Threading.Tasks;
using ServiceLibrary.Core.Manager.DB.Contacter.Format;

namespace ServiceLibrary.Core.Manager.DB.Contacter.Service;

/// <summary>核心-管理者-窗口-資料庫服務</summary>
public interface ICoMgrContacterDbService
{
    /// <summary>核心-管理者-窗口-資料庫服務-是否存在</summary>
    public Task<CoMgrCttDbExistRspMdl> ExistAsync(CoMgrCttDbExistReqMdl reqObj);

    /// <summary>核心-管理者-窗口-資料庫-同公司Email是否存在</summary>
    public Task<CoMgrCttDbExistByCompanyEmailRspMdl> ExistByCompanyEmailAsync(CoMgrCttDbExistByCompanyEmailReqMdl reqObj);

    /// <summary>核心-管理者-窗口-資料庫服務-取得</summary>
    public Task<CoMgrCttDbGetRspMdl> GetAsync(CoMgrCttDbGetReqMdl reqObj);

    /// <summary>核心-管理者-窗口-資料庫服務-取得多筆</summary>
    public Task<CoMgrCttDbGetManyRspMdl> GetManyAsync(CoMgrCttDbGetManyReqMdl reqObj);

    /// <summary>核心-管理者-窗口-資料庫服務-取得多筆[名稱]</summary>
    public Task<CoMgrCttDbGetManyNameRspMdl> GetManyNameAsync(CoMgrCttDbGetManyNameReqMdl reqObj);

    /// <summary>核心-管理者-窗口-資料庫服務-新增</summary>
    public Task<CoMgrCttDbAddRspMdl> AddAsync(CoMgrCttDbAddReqMdl reqObj);

    /// <summary>核心-管理者-窗口-資料庫服務-更新</summary>
    public Task<CoMgrCttDbUpdateRspMdl> UpdateAsync(CoMgrCttDbUpdateReqMdl reqObj);

    /// <summary>核心-管理者-窗口-資料庫服務-移除</summary>
    public Task<CoMgrCttDbRemoveRspMdl> RemoveAsync(CoMgrCttDbRemoveReqMdl reqObj);

    #region ManagerBackSite.Customer.Company 管理者後台-系統設定-窗口

    /// <summary>核心-管理者-窗口-資料庫服務-取得[筆數]從[管理者後台-系統設定-窗口]</summary>
    public Task<CoMgrCttDbGetCountFromMbsSysCtrRspMdl> GetCountFromMbsSysCtrAsync(CoMgrCttDbGetCountFromMbsSysCtrReqMdl reqObj);

    /// <summary>核心-管理者-窗口-資料庫服務-取得多筆[管理者後台-系統設定-窗口]</summary>
    public Task<CoMgrCttDbGetManyFromMbsSysCtrRspMdl> GetManyFromMbsSysCtrAsync(CoMgrCttDbGetManyFromMbsSysCtrReqMdl reqObj);

    #endregion    

    #region ManagerBackSite.Basic 管理者後台-基本

    /// <summary>核心-管理者-窗口-資料庫-取得多筆從[管理者後台-基本]</summary>
    public Task<CoMgrCttDbGetManyFromMbsBscRspMdl> GetManyFromMbsBscAsync(CoMgrCttDbGetManyFromMbsBscReqMdl reqObj);

    #endregion
}
