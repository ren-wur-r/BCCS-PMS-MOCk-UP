using System.Threading.Tasks;
using ServiceLibrary.Core.Employee.DB.PipelinePhone.Format;

namespace ServiceLibrary.Core.Employee.DB.PipelinePhone.Service;

/// <summary>核心-員工-商機電銷-資料庫服務介面</summary>
public interface ICoEmpPipelinePhoneDbService
{
    /// <summary>核心-員工-商機電銷-資料庫-取得</summary>
    public Task<CoEmpPplPhnDbGetRspMdl> GetAsync(CoEmpPplPhnDbGetReqMdl reqObj);

    /// <summary>核心-員工-商機電銷-資料庫-取得多筆</summary>
    public Task<CoEmpPplPhnDbGetManyRspMdl> GetManyAsync(CoEmpPplPhnDbGetManyReqMdl reqObj);

    /// <summary>核心-員工-商機電銷-資料庫-新增</summary>
    public Task<CoEmpPplPhnDbAddRspMdl> AddAsync(CoEmpPplPhnDbAddReqMdl reqObj);

    /// <summary>核心-員工-商機電銷-資料庫-更新</summary>
    public Task<CoEmpPplPhnDbUpdateRspMdl> UpdateAsync(CoEmpPplPhnDbUpdateReqMdl reqObj);

    /// <summary>核心-員工-商機電銷-資料庫-移除</summary>
    public Task<CoEmpPplPhnDbRemoveRspMdl> RemoveAsync(CoEmpPplPhnDbRemoveReqMdl reqObj);

    /// <summary>核心-員工-商機電銷-資料庫-取得[筆數]從[活動ID]</summary>
    public Task<CoEmpPplPhnDbGetCountFromManagerActivityIDRspMdl> GetCountFromManagerActivityIDAsync(CoEmpPplPhnDbGetCountFromManagerActivityIDReqMdl reqObj);

    /// <summary>核心-員工-商機電銷-資料庫-取得多筆[筆數]從[活動ID]</summary>
    public Task<CoEmpPplPhnDbGetManyCountFromManagerActivityIDRspMdl> GetManyCountFromManagerActivityIDAsync(CoEmpPplPhnDbGetManyCountFromManagerActivityIDReqMdl reqObj);
}
