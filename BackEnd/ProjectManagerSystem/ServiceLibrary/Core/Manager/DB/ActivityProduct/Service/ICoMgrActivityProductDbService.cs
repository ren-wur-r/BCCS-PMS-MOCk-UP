using System.Threading.Tasks;
using ServiceLibrary.Core.Manager.DB.ActivityProduct.Format;

namespace ServiceLibrary.Core.Manager.DB.ActivityProduct.Service;

/// <summary>核心-管理者-活動產品-資料庫服務介面</summary>
public interface ICoMgrActivityProductDbService
{
    /// <summary>核心-管理者-活動產品-資料庫-是否存在</summary>
    public Task<CoMgrActPrdDbExistRspMdl> ExistAsync(CoMgrActPrdDbExistReqMdl reqObj);

    /// <summary>核心-管理者-活動產品-資料庫-新增</summary>
    public Task<CoMgrActPrdDbAddRspMdl> AddAsync(CoMgrActPrdDbAddReqMdl reqObj);

    /// <summary>核心-管理者-活動產品-資料庫-新增多筆</summary>
    public Task<CoMgrActPrdDbAddManyRspMdl> AddManyAsync(CoMgrActPrdDbAddManyReqMdl reqObj);

    /// <summary>核心-管理者-活動產品-資料庫-更新</summary>
    public Task<CoMgrActPrdDbUpdateRspMdl> UpdateAsync(CoMgrActPrdDbUpdateReqMdl reqObj);

    /// <summary>核心-管理者-活動產品-資料庫-移除</summary>
    public Task<CoMgrActPrdDbRemoveRspMdl> RemoveAsync(CoMgrActPrdDbRemoveReqMdl reqObj);

    /// <summary>核心-管理者-活動產品-資料庫-取得</summary>
    public Task<CoMgrActPrdDbGetRspMdl> GetAsync(CoMgrActPrdDbGetReqMdl reqObj);

    /// <summary>核心-管理者-活動產品-資料庫-取得多筆</summary>
    public Task<CoMgrActPrdDbGetManyRspMdl> GetManyAsync(CoMgrActPrdDbGetManyReqMdl reqObj);
}
