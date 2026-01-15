using System.Threading.Tasks;
using ServiceLibrary.Core.Manager.DB.CompanyAffiliate.Format;

namespace ServiceLibrary.Core.Manager.DB.CompanyAffiliate.Service;

/// <summary>核心-管理者-關係公司-資料庫服務-介面</summary>
public interface ICoMgrCompanyAffiliateDbService
{
    /// <summary>核心-管理者-關係公司-資料庫-取得多筆</summary>
    public Task<CoMgrCmpAffDbGetManyRspMdl> GetManyAsync(CoMgrCmpAffDbGetManyReqMdl reqObj);

    /// <summary>核心-管理者-關係公司-資料庫-取得</summary>
    public Task<CoMgrCmpAffDbGetRspMdl> GetAsync(CoMgrCmpAffDbGetReqMdl reqObj);

    /// <summary>核心-管理者-關係公司-資料庫-新增</summary>
    public Task<CoMgrCmpAffDbAddRspMdl> AddAsync(CoMgrCmpAffDbAddReqMdl reqObj);

    /// <summary>核心-管理者-關係公司-資料庫-新增多筆</summary>
    public Task<CoMgrCmpAffDbAddManyRspMdl> AddManyAsync(CoMgrCmpAffDbAddManyReqMdl reqObj);

    /// <summary>核心-管理者-關係公司-資料庫-更新</summary>
    public Task<CoMgrCmpAffDbUpdateRspMdl> UpdateAsync(CoMgrCmpAffDbUpdateReqMdl reqObj);

    /// <summary>核心-管理者-關係公司-資料庫-依管理者公司ID移除</summary>
    public Task<CoMgrCmpAffDbRemoveByManagerCompanyIDRspMdl> RemoveByManagerCompanyIDAsync(CoMgrCmpAffDbRemoveByManagerCompanyIDReqMdl reqObj);
}