using ServiceLibrary.Core.Employee.Memory.LoginInfo.Format;

namespace ServiceLibrary.Core.Employee.Memory.LoginInfo.Service;


/// <summary>核心-員工-登入資訊-記憶體服務</summary>
public interface ICoEmpLoginInfoMemoryService
{
    /// <summary>核心-員工-登入資訊-記憶體-是否存在</summary>
    public CoEmpLiMemExistRspMdl Exist(CoEmpLiMemExistReqMdl reqObj);

    /// <summary>核心-員工-登入資訊-記憶體-新增</summary>
    public CoEmpLiMemAddRspMdl Add(CoEmpLiMemAddReqMdl reqObj);

    /// <summary>核心-員工-登入資訊-記憶體-更新到期時間</summary>
    public CoEmpLiMemUpdateExpiredTimeRspMdl UpdateExpiredTime(CoEmpLiMemUpdateExpiredTimeReqMdl reqObj);

    /// <summary>核心-員工-登入資訊-記憶體-移除</summary>
    public CoEmpLiMemRemoveRspMdl Remove(CoEmpLiMemRemoveReqMdl reqObj);

    /// <summary>核心-員工-登入資訊-記憶體-移除多筆</summary>
    public CoEmpLiMemRemoveManyRspMdl RemoveMany(CoEmpLiMemRemoveManyReqMdl reqObj);

    /// <summary>核心-員工-登入資訊-記憶體-取得多筆[登入令牌]</summary>
    public CoEmpLiMemGetManyLoginTokenRspMdl GetManyLoginToken(CoEmpLiMemGetManyLoginTokenReqMdl reqObj);

    /// <summary>核心-員工-登入資訊-記憶體-取得全部</summary>
    public CoEmpLiMemGetAllRspMdl GetAll();

    /// <summary>核心-員工-登入資訊-記憶體-取得</summary>
    public CoEmpLiMemGetRspMdl Get(CoEmpLiMemGetReqMdl reqObj);


}