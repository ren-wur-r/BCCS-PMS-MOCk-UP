using System.Threading.Tasks;
using ServiceLibrary.ManagerBackSite.Logical.System.Department.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.System.Department.Service;

/// <summary>管理者後台-系統設定-部門-邏輯服務</summary>
public interface IMbsSysDepartmentLogicalService
{
    /// <summary>管理者後台-系統設定-部門-邏輯-取得多筆</summary>
    public Task<MbsSysDptLgcGetManyRspMdl> GetManyAsync(MbsSysDptLgcGetManyReqMdl reqObj);

    /// <summary>管理者後台-系統設定-部門-邏輯-新增</summary>
    public Task<MbsSysDptLgcAddRspMdl> AddAsync(MbsSysDptLgcAddReqMdl reqObj);

    /// <summary>管理者後台-系統設定-部門-邏輯-更新</summary>
    public Task<MbsSysDptLgcUpdateRspMdl> UpdateAsync(MbsSysDptLgcUpdateReqMdl reqObj);

    /// <summary>管理者後台-系統設定-部門-邏輯-取得</summary>
    public Task<MbsSysDptLgcGetRspMdl> GetAsync(MbsSysDptLgcGetReqMdl reqObj);
}