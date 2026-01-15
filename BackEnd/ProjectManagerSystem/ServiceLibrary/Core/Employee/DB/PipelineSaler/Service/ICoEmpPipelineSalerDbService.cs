using System.Threading.Tasks;
using ServiceLibrary.Core.Employee.DB.PipelineSaler.Format;

namespace ServiceLibrary.Core.Employee.DB.PipelineSaler.Service;

/// <summary>核心-員工-商機業務-資料庫服務介面</summary>
public interface ICoEmpPipelineSalerDbService
{
    /// <summary>核心-員工-商機業務-資料庫-取得</summary>
    public Task<CoEmpPplSlrDbGetRspMdl> GetAsync(CoEmpPplSlrDbGetReqMdl reqObj);

    /// <summary>核心-員工-商機業務-資料庫-取得多筆</summary>
    public Task<CoEmpPplSlrDbGetManyRspMdl> GetManyAsync(CoEmpPplSlrDbGetManyReqMdl reqObj);

    /// <summary>核心-員工-商機業務-資料庫-新增</summary>
    public Task<CoEmpPplSlrDbAddRspMdl> AddAsync(CoEmpPplSlrDbAddReqMdl reqObj);

    /// <summary>核心-員工-商機業務-資料庫-更新</summary>
    public Task<CoEmpPplSlrDbUpdateRspMdl> UpdateAsync(CoEmpPplSlrDbUpdateReqMdl reqObj);

    /// <summary>核心-員工-商機業務-資料庫-是否存在</summary>
    public Task<CoEmpPplSlrDbExistsRspMdl> ExistsAsync(CoEmpPplSlrDbExistsReqMdl reqObj);

    #region ManagerBackSite.Crm.Pipeline 管理者後台-CRM-商機管理

    /// <summary>核心-員工-商機業務-資料庫-取得多筆[管理者後台-CRM-商機管理]</summary>
    public Task<CoEmpPplSlrDbGetManyFromMbsCrmPplRspMdl> GetManyFromMbsCrmPplAsync(CoEmpPplSlrDbGetManyFromMbsCrmPplReqMdl reqObj);

    #endregion
}
