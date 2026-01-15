using System.Threading.Tasks;
using ServiceLibrary.ManagerBackSite.Logical.System.Employee.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.System.Employee.Service;

/// <summary>管理者後台-系統設定-員工-邏輯服務</summary>
public interface IMbsSysEmployeeLogicalService
{
    /// <summary>管理者後台-系統設定-員工-邏輯-取得多筆</summary>
    public Task<MbsSysEmpLgcGetManyRspMdl> GetManyAsync(MbsSysEmpLgcGetManyReqMdl reqObj);

    /// <summary>管理者後台-系統設定-員工-邏輯-新增</summary>
    public Task<MbsSysEmpLgcAddRspMdl> AddAsync(MbsSysEmpLgcAddReqMdl reqObj);

    /// <summary>管理者後台-系統設定-員工-邏輯-更新</summary>
    public Task<MbsSysEmpLgcUpdateRspMdl> UpdateAsync(MbsSysEmpLgcUpdateReqMdl reqObj);

    /// <summary>管理者後台-系統設定-員工-邏輯-取得</summary>
    public Task<MbsSysEmpLgcGetRspMdl> GetAsync(MbsSysEmpLgcGetReqMdl reqObj);
}