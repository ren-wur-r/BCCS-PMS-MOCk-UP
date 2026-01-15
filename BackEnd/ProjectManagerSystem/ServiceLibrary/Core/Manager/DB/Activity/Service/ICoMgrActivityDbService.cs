using System.Threading.Tasks;
using ServiceLibrary.Core.Manager.DB.Activity.Format;

namespace ServiceLibrary.Core.Manager.DB.Activity.Service;

/// <summary>核心-管理者-活動-資料庫服務介面</summary>
public interface ICoMgrActivityDbService
{
    /// <summary>核心-管理者-活動-資料庫-是否存在</summary>
    public Task<CoMgrActivityDbExistRspMdl> ExistAsync(CoMgrActivityDbExistReqMdl reqObj);

    /// <summary>核心-管理者-活動-資料庫-新增</summary>
    public Task<CoMgrActivityDbAddRspMdl> AddAsync(CoMgrActivityDbAddReqMdl reqObj);

    /// <summary>核心-管理者-活動-資料庫-更新</summary>
    public Task<CoMgrActivityDbUpdateRspMdl> UpdateAsync(CoMgrActivityDbUpdateReqMdl reqObj);

    /// <summary>核心-管理者-活動-資料庫-移除</summary>
    public Task<CoMgrActivityDbRemoveRspMdl> RemoveAsync(CoMgrActivityDbRemoveReqMdl reqObj);

    /// <summary>核心-管理者-活動-資料庫-取得</summary>
    public Task<CoMgrActivityDbGetRspMdl> GetAsync(CoMgrActivityDbGetReqMdl reqObj);

    /// <summary>核心-管理者-活動-資料庫-增加商機數</summary>
    public Task<CoMgrActivityDbIncrementEmployeePipelineCountRspMdl> IncrementEmployeePipelineCountAsync(CoMgrActivityDbIncrementEmployeePipelineCountReqMdl reqObj);

    /// <summary>核心-管理者-活動-資料庫-取得最新過往活動從[窗口Email]</summary>
    public Task<CoMgrActivityDbGetLatestPastActivityFromContacterEmailRspMdl> GetLatestPastActivityFromContacterEmailAsync(CoMgrActivityDbGetLatestPastActivityFromContacterEmailReqMdl reqObj);

    /// <summary>核心-管理者-活動-資料庫-取得[筆數]過往活動從[窗口Email]</summary>
    public Task<CoMgrActivityDbGetCountPastActivityFromContacterEmailRspMdl> GetCountPastActivityFromContacterEmailAsync(CoMgrActivityDbGetCountPastActivityFromContacterEmailReqMdl reqObj);

    /// <summary>核心-管理者-活動-資料庫-取得多筆過往活動從[窗口Email]</summary>
    public Task<CoMgrActivityDbGetManyPastActivityFromContacterEmailRspMdl> GetManyPastActivityFromContacterEmailAsync(CoMgrActivityDbGetManyPastActivityFromContacterEmailReqMdl reqObj);

    #region ManagerBackSite.Crm.Activity 管理者後台-CRM-活動管理

    /// <summary>核心-管理者-活動-資料庫-取得[筆數]從[管理者後台-CRM-活動管理]</summary>
    public Task<CoMgrActivityDbGetCountFromMbsCrmActRspMdl> GetCountFromMbsCrmActAsync(CoMgrActivityDbGetCountFromMbsCrmActReqMdl reqObj);

    /// <summary>核心-管理者-活動-資料庫-取得多筆從[管理者後台-CRM-活動管理]</summary>
    public Task<CoMgrActivityDbGetManyFromMbsCrmActRspMdl> GetManyFromMbsCrmActAsync(CoMgrActivityDbGetManyFromMbsCrmActReqMdl reqObj);

    #endregion

    #region ManagerBackSite.Crm.Phone 管理者後台-CRM-電銷管理

    /// <summary>核心-管理者-活動-資料庫-取得[筆數]從[管理者後台-CRM-電銷管理]</summary>
    public Task<CoMgrActivityDbGetCountFromMbsCrmPhnRspMdl> GetCountFromMbsCrmPhnAsync(CoMgrActivityDbGetCountFromMbsCrmPhnReqMdl reqObj);

    /// <summary>核心-管理者-活動-資料庫-取得多筆從[管理者後台-CRM-電銷管理]</summary>
    public Task<CoMgrActivityDbGetManyFromMbsCrmPhnRspMdl> GetManyFromMbsCrmPhnAsync(CoMgrActivityDbGetManyFromMbsCrmPhnReqMdl reqObj);

    #endregion
}