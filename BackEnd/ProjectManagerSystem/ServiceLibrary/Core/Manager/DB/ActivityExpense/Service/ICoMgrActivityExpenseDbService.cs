using System.Threading.Tasks;
using ServiceLibrary.Core.Manager.DB.ActivityExpense.Format;

namespace ServiceLibrary.Core.Manager.DB.ActivityExpense.Service;

/// <summary>核心-管理者-活動支出-資料庫服務介面</summary>
public interface ICoMgrActivityExpenseDbService
{
    /// <summary>核心-管理者-活動支出-資料庫-是否存在</summary>
    public Task<CoMgrActExpDbExistRspMdl> ExistAsync(CoMgrActExpDbExistReqMdl reqObj);

    /// <summary>核心-管理者-活動支出-資料庫-新增</summary>
    public Task<CoMgrActExpDbAddRspMdl> AddAsync(CoMgrActExpDbAddReqMdl reqObj);

    /// <summary>核心-管理者-活動支出-資料庫-新增多筆</summary>
    public Task<CoMgrActExpDbAddManyRspMdl> AddManyAsync(CoMgrActExpDbAddManyReqMdl reqObj);

    /// <summary>核心-管理者-活動支出-資料庫-更新</summary>
    public Task<CoMgrActExpDbUpdateRspMdl> UpdateAsync(CoMgrActExpDbUpdateReqMdl reqObj);

    /// <summary>核心-管理者-活動支出-資料庫-移除</summary>
    public Task<CoMgrActExpDbRemoveRspMdl> RemoveAsync(CoMgrActExpDbRemoveReqMdl reqObj);

    /// <summary>核心-管理者-活動支出-資料庫-取得</summary>
    public Task<CoMgrActExpDbGetRspMdl> GetAsync(CoMgrActExpDbGetReqMdl reqObj);

    /// <summary>核心-管理者-活動支出-資料庫-取得多筆</summary>
    public Task<CoMgrActExpDbGetManyRspMdl> GetManyAsync(CoMgrActExpDbGetManyReqMdl reqObj);

    #region ManagerBackSite.Marketing.Activity 管理者後台-CRM-活動管理

    /// <summary>核心-管理者-活動支出-資料庫-取得多筆總支出額從[管理者後台-CRM-活動管理]</summary>
    public Task<CoMgrActExpDbGetManyTotalExpenseRspMdl> GetManyTotalExpenseAsync(CoMgrActExpDbGetManyTotalExpenseReqMdl reqObj);

    #endregion
}
