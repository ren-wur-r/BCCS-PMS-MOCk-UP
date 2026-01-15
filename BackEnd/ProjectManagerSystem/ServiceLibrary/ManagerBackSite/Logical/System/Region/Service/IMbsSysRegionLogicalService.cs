using System.Threading.Tasks;
using ServiceLibrary.ManagerBackSite.Logical.System.Region.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.System.Region.Service;

/// <summary>管理者後台-系統設定-地區-邏輯服務</summary>
public interface IMbsSysRegionLogicalService
{
    /// <summary>管理者後台-系統設定-地區-邏輯服務-取得多筆</summary>
    public Task<MbsSysRgnLgcGetManyRspMdl> GetManyAsync(MbsSysRgnLgcGetManyReqMdl reqObj);

    /// <summary>管理者後台-系統設定-地區-邏輯服務-新增</summary>
    public Task<MbsSysRgnLgcAddRspMdl> AddAsync(MbsSysRgnLgcAddReqMdl reqObj);

    /// <summary>管理者後台-系統設定-地區-邏輯服務-更新</summary>
    public Task<MbsSysRgnLgcUpdateRspMdl> UpdateAsync(MbsSysRgnLgcUpdateReqMdl reqObj);

    /// <summary>管理者後台-系統設定-地區-邏輯服務-取得</summary>
    public Task<MbsSysRgnLgcGetRspMdl> GetAsync(MbsSysRgnLgcGetReqMdl reqObj);
}