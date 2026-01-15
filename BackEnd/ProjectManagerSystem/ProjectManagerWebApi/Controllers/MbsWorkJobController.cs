using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using ApiModelLibrary.ManagerBackSite.Common;
using ApiModelLibrary.ManagerBackSite.Controller.Work.Job;
using CommonLibrary.CmnHttpContext.Extension;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ServiceLibrary.ManagerBackSite.Logical.Work.Job.Format;
using ServiceLibrary.ManagerBackSite.Logical.Work.Job.Service;

namespace ProjectManagerWebApi.Controllers;

/// <summary>管理後台-工作-工項-控制器</summary>
[Route("api/[controller]/[action]")]
[ApiController]
public class MbsWorkJobController : ControllerBase
{
    /// <summary>logger</summary>
    private readonly ILogger<MbsWorkJobController> _logger;

    /// <summary>管理者後台-工作-工項-邏輯服務</summary>
    private readonly IMbsWrkJobLogicalService _mbsWrkJobLogical;

    /// <summary>建構式</summary>
    public MbsWorkJobController(
        ILogger<MbsWorkJobController> logger,
        IMbsWrkJobLogicalService mbsWrkJobLogical)
    {
        this._logger = logger;
        this._mbsWrkJobLogical = mbsWrkJobLogical;
    }

    #region 專案里程碑工項

    /// <summary>管理者後台-工作-工項-控制器-取得多筆專案里程碑工項</summary>
    [HttpPost]
    public async Task<MbsWrkJobCtlGetManyProjectStoneJobRspMdl> GetManyProjectStoneJob(MbsWrkJobCtlGetManyProjectStoneJobReqMdl reqObj)
    {
        var rspObj = new MbsWrkJobCtlGetManyProjectStoneJobRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        // logical, 管理者後台-工作-工項-邏輯服務-取得多筆專案里程碑工項
        var logicalReqObj = new MbsWrkJobLgcGetManyProjectStoneJobReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken.Trim(),
            OperatorIP = this.HttpContext.GetClientIP(),
            EmployeeProjectID = reqObj.EmployeeProjectID,
            EmployeeProjectStoneID = reqObj.EmployeeProjectStoneID,
            EmployeeProjectStoneJobID = reqObj.EmployeeProjectStoneJobID,
            EmployeeProjectStoneJobStatus = reqObj.EmployeeProjectStoneJobStatus,
            StartEmployeeProjectStoneJobEndTime = reqObj.StartEmployeeProjectStoneJobEndTime,
            EndEmployeeProjectStoneJobEndTime = reqObj.EndEmployeeProjectStoneJobEndTime,
            PageIndex = reqObj.PageIndex,
            PageSize = reqObj.PageSize,
        };
        var logicalRspObj = await this._mbsWrkJobLogical.GetManyProjectStoneJobAsync(logicalReqObj);
        if (logicalRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }
        if (logicalRspObj.ErrorCode != MbsErrorCodeEnum.Success)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");

            rspObj.ErrorCode = logicalRspObj.ErrorCode;
            return rspObj;
        }

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        rspObj.EmployeeProjectStoneJobList = logicalRspObj.EmployeeProjectStoneJobList
            .Select(x => new MbsWrkJobCtlGetManyProjectStoneJobRspItemJobMdl()
            {
                EmployeeProjectStoneJobID = x.EmployeeProjectStoneJobID,
                EmployeeProjectName = x.EmployeeProjectName,
                EmployeeProjectStoneName = x.EmployeeProjectStoneName,
                EmployeeProjectStoneJobName = x.EmployeeProjectStoneJobName,
                EmployeeProjectStoneJobStatus = x.EmployeeProjectStoneJobStatus,
                EmployeeProjectStoneJobStartTime = x.EmployeeProjectStoneJobStartTime,
                EmployeeProjectStoneJobEndTime = x.EmployeeProjectStoneJobEndTime,
                EmployeeProjectStoneJobExecutorList = x.EmployeeProjectStoneJobExecutorList
                    .Select(y => new MbsWrkJobCtlGetManyProjectStoneJobRspItemExecutorMdl()
                    {
                        EmployeeProjectStoneJobExecutorName = y.EmployeeProjectStoneJobExecutorName,
                    })
                    .ToList(),
            })
            .ToList();
        rspObj.TotalCount = logicalRspObj.TotalCount;
        return rspObj;
    }

    /// <summary>管理者後台-工作-工項-控制器-取得專案里程碑工項</summary>
    [HttpPost]
    public async Task<MbsWrkJobCtlGetProjectStoneJobRspMdl> GetProjectStoneJob(MbsWrkJobCtlGetProjectStoneJobReqMdl reqObj)
    {
        var rspObj = new MbsWrkJobCtlGetProjectStoneJobRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        // logical, 管理者後台-工作-工項-邏輯服務-取得專案里程碑工項
        var logicalReqObj = new MbsWrkJobLgcGetProjectStoneJobReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken.Trim(),
            OperatorIP = this.HttpContext.GetClientIP(),
            EmployeeProjectStoneJobID = reqObj.EmployeeProjectStoneJobID,
        };
        var logicalRspObj = await this._mbsWrkJobLogical.GetProjectStoneJobAsync(logicalReqObj);
        if (logicalRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }
        if (logicalRspObj.ErrorCode != MbsErrorCodeEnum.Success)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");

            rspObj.ErrorCode = logicalRspObj.ErrorCode;
            return rspObj;
        }

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        rspObj.EmployeeProjectName = logicalRspObj.EmployeeProjectName;
        rspObj.EmployeeProjectStartTime = logicalRspObj.EmployeeProjectStartTime;
        rspObj.EmployeeProjectEndTime = logicalRspObj.EmployeeProjectEndTime;
        rspObj.EmployeeProjectStoneName = logicalRspObj.EmployeeProjectStoneName;
        rspObj.EmployeeProjectStoneStartTime = logicalRspObj.EmployeeProjectStoneStartTime;
        rspObj.EmployeeProjectStoneEndTime = logicalRspObj.EmployeeProjectStoneEndTime;
        rspObj.EmployeeProjectStoneJobName = logicalRspObj.EmployeeProjectStoneJobName;
        rspObj.EmployeeProjectStoneJobStartTime = logicalRspObj.EmployeeProjectStoneJobStartTime;
        rspObj.EmployeeProjectStoneJobEndTime = logicalRspObj.EmployeeProjectStoneJobEndTime;
        rspObj.EmployeeProjectStoneJobStatus = logicalRspObj.EmployeeProjectStoneJobStatus;
        rspObj.EmployeeProjectStoneJobRemark = logicalRspObj.EmployeeProjectStoneJobRemark;
        rspObj.EmployeeProjectStoneJobExecutorList = logicalRspObj.EmployeeProjectStoneJobExecutorList
            .Select(x => new MbsWrkJobCtlGetProjectStoneJobRspItemExecutorMdl()
            {
                EmployeeProjectStoneJobExecutorName = x.EmployeeProjectStoneJobExecutorName,
            })
            .ToList();
        rspObj.EmployeeProjectStoneJobBucketList = logicalRspObj.EmployeeProjectStoneJobBucketList
            .Select(x => new MbsWrkJobCtlGetProjectStoneJobRspItemBucketMdl()
            {
                EmployeeProjectStoneJobBucketID = x.EmployeeProjectStoneJobBucketID,
                EmployeeProjectStoneJobBucketName = x.EmployeeProjectStoneJobBucketName,
                EmployeeProjectStoneJobBucketIsFinish = x.EmployeeProjectStoneJobBucketIsFinish,
            })
            .ToList();
        rspObj.EmployeeProjectStoneJobWorkList = logicalRspObj.EmployeeProjectStoneJobWorkList
            .Select(x => new MbsWrkJobCtlGetProjectStoneJobRspItemWorkMdl()
            {
                EmployeeProjectStoneJobWorkID = x.EmployeeProjectStoneJobWorkID,
                EmployeeProjectStoneJobWorkStartTime = x.EmployeeProjectStoneJobWorkStartTime,
                EmployeeProjectStoneJobWorkEndTime = x.EmployeeProjectStoneJobWorkEndTime,
                EmployeeProjectStoneJobWorkRemark = x.EmployeeProjectStoneJobWorkRemark?.Trim(),
                EmployeeName = x.EmployeeName,
            })
            .ToList();
        rspObj.EmployeeProjectStoneJobWorkFileList = logicalRspObj.EmployeeProjectStoneJobWorkFileList
            .Select(x => new MbsWrkJobCtlGetProjectStoneJobRspItemFileMdl()
            {
                EmployeeProjectStoneJobWorkFileUrl = x.EmployeeProjectStoneJobWorkFileUrl,
            })
            .ToList();
        return rspObj;
    }

    /// <summary>管理者後台-工作-工項-控制器-更新專案里程碑工項</summary>
    [HttpPost]
    public async Task<MbsWrkJobCtlUpdateProjectStoneJobRspMdl> UpdateProjectStoneJob(MbsWrkJobCtlUpdateProjectStoneJobReqMdl reqObj)
    {
        var rspObj = new MbsWrkJobCtlUpdateProjectStoneJobRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        // logical, 管理者後台-工作-工項-邏輯服務-更新專案里程碑工項
        var logicalReqObj = new MbsWrkJobLgcUpdateProjectStoneJobReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken.Trim(),
            OperatorIP = this.HttpContext.GetClientIP(),
            EmployeeProjectStoneJobID = reqObj.EmployeeProjectStoneJobID,
            EmployeeProjectStoneJobRemark = reqObj.EmployeeProjectStoneJobRemark?.Trim(),
            EmployeeProjectStoneJobBucketList = reqObj.EmployeeProjectStoneJobBucketList
                ?.Select(x => new MbsWrkJobLgcUpdateProjectStoneJobReqItemBucketMdl()
                {
                    EmployeeProjectStoneJobBucketID = x.EmployeeProjectStoneJobBucketID,
                    EmployeeProjectStoneJobBucketName = x.EmployeeProjectStoneJobBucketName?.Trim(),
                    EmployeeProjectStoneJobBucketIsFinish = x.EmployeeProjectStoneJobBucketIsFinish,
                })
                .ToList(),
        };
        var logicalRspObj = await this._mbsWrkJobLogical.UpdateProjectStoneJobAsync(logicalReqObj);
        if (logicalRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }
        if (logicalRspObj.ErrorCode != MbsErrorCodeEnum.Success)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");

            rspObj.ErrorCode = logicalRspObj.ErrorCode;
            return rspObj;
        }

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        return rspObj;
    }

    #endregion

    #region 專案里程碑工作

    /// <summary>管理者後台-工作-工項-控制器-取得專案里程碑工項工作</summary>
    [HttpPost]
    public async Task<MbsWrkJobCtlGetProjectStoneJobWorkRspMdl> GetProjectStoneJobWork(MbsWrkJobCtlGetProjectStoneJobWorkReqMdl reqObj)
    {
        var rspObj = new MbsWrkJobCtlGetProjectStoneJobWorkRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        // logical, 管理者後台-工作-工項-邏輯服務-取得專案里程碑工項工作
        var logicalReqObj = new MbsWrkJobLgcGetProjectStoneJobWorkReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken.Trim(),
            OperatorIP = this.HttpContext.GetClientIP(),
            EmployeeProjectStoneJobWorkID = reqObj.EmployeeProjectStoneJobWorkID,
        };
        var logicalRspObj = await this._mbsWrkJobLogical.GetProjectStoneJobWorkAsync(logicalReqObj);
        if (logicalRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }
        if (logicalRspObj.ErrorCode != MbsErrorCodeEnum.Success)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");

            rspObj.ErrorCode = logicalRspObj.ErrorCode;
            return rspObj;
        }

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        rspObj.EmployeeProjectStoneJobID = logicalRspObj.EmployeeProjectStoneJobID;
        rspObj.EmployeeProjectStoneJobRemark = logicalRspObj.EmployeeProjectStoneJobRemark;
        rspObj.EmployeeProjectStoneJobBucketList = logicalRspObj.EmployeeProjectStoneJobBucketList
            .Select(x => new MbsWrkJobCtlGetProjectStoneJobWorkReqItemBucketMdl()
            {
                EmployeeProjectStoneJobBucketID = x.EmployeeProjectStoneJobBucketID,
                EmployeeProjectStoneJobBucketName = x.EmployeeProjectStoneJobBucketName,
                EmployeeProjectStoneJobBucketIsFinish = x.EmployeeProjectStoneJobBucketIsFinish,
            })
            .ToList();
        rspObj.EmployeeProjectStoneJobWorkStartTime = logicalRspObj.EmployeeProjectStoneJobWorkStartTime;
        rspObj.EmployeeProjectStoneJobWorkEndTime = logicalRspObj.EmployeeProjectStoneJobWorkEndTime;
        rspObj.EmployeeProjectStoneJobWorkRemark = logicalRspObj.EmployeeProjectStoneJobWorkRemark;
        rspObj.EmployeeProjectStoneJobWorkFileList = logicalRspObj.EmployeeProjectStoneJobWorkFileList
            .Select(x => new MbsWrkJobCtlGetProjectStoneJobWorkReqItemFileMdl()
            {
                EmployeeProjectStoneJobWorkFileID = x.EmployeeProjectStoneJobWorkFileID,
                EmployeeProjectStoneJobWorkFileUrl = x.EmployeeProjectStoneJobWorkFileUrl,
            })
            .ToList();
        // 專案
        rspObj.EmployeeProjectName = logicalRspObj.EmployeeProjectName;
        rspObj.EmployeeProjectStartTime = logicalRspObj.EmployeeProjectStartTime;
        rspObj.EmployeeProjectEndTime = logicalRspObj.EmployeeProjectEndTime;
        // 里程碑
        rspObj.EmployeeProjectStoneName = logicalRspObj.EmployeeProjectStoneName;
        rspObj.EmployeeProjectStoneStartTime = logicalRspObj.EmployeeProjectStoneStartTime;
        rspObj.EmployeeProjectStoneEndTime = logicalRspObj.EmployeeProjectStoneEndTime;
        // 工項時間
        rspObj.EmployeeProjectStoneJobName = logicalRspObj.EmployeeProjectStoneJobName;
        rspObj.EmployeeProjectStoneJobStartTime = logicalRspObj.EmployeeProjectStoneJobStartTime;
        rspObj.EmployeeProjectStoneJobEndTime = logicalRspObj.EmployeeProjectStoneJobEndTime;
        return rspObj;
    }

    /// <summary>管理者後台-工作-工項-控制器-新增專案里程碑工項工作</summary>
    [HttpPost]
    public async Task<MbsWrkJobCtlAddProjectStoneJobWorkRspMdl> AddProjectStoneJobWork(MbsWrkJobCtlAddProjectStoneJobWorkReqMdl reqObj)
    {
        var rspObj = new MbsWrkJobCtlAddProjectStoneJobWorkRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        // logical, 管理者後台-工作-工項-邏輯服務-新增專案里程碑工項工作
        var logicalReqObj = new MbsWrkJobLgcAddProjectStoneJobWorkReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken.Trim(),
            OperatorIP = this.HttpContext.GetClientIP(),
            EmployeeProjectStoneJobID = reqObj.EmployeeProjectStoneJobID,
            EmployeeProjectStoneJobRemark = reqObj.EmployeeProjectStoneJobRemark?.Trim(),
            EmployeeProjectStoneJobBucketList = reqObj.EmployeeProjectStoneJobBucketList
                ?.Select(x => new MbsWrkJobLgcAddProjectStoneJobWorkReqItemBucketMdl()
                {
                    EmployeeProjectStoneJobBucketID = x.EmployeeProjectStoneJobBucketID,
                    EmployeeProjectStoneJobBucketIsFinish = x.EmployeeProjectStoneJobBucketIsFinish,
                })
                .ToList(),
            EmployeeProjectStoneJobWorkStartTime = reqObj.EmployeeProjectStoneJobWorkStartTime,
            EmployeeProjectStoneJobWorkEndTime = reqObj.EmployeeProjectStoneJobWorkEndTime,
            EmployeeProjectStoneJobWorkRemark = reqObj.EmployeeProjectStoneJobWorkRemark?.Trim(),
            EmployeeProjectStoneJobWorkFileList = reqObj.EmployeeProjectStoneJobWorkFileList
                ?.Select(x => new MbsWrkJobLgcAddProjectStoneJobWorkReqItemFileMdl()
                {
                    EmployeeProjectStoneJobWorkFileUrl = x.EmployeeProjectStoneJobWorkFileUrl.Trim(),
                })
                .ToList(),
        };
        var logicalRspObj = await this._mbsWrkJobLogical.AddProjectStoneJobWorkAsync(logicalReqObj);
        if (logicalRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }
        if (logicalRspObj.ErrorCode != MbsErrorCodeEnum.Success)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");

            rspObj.ErrorCode = logicalRspObj.ErrorCode;
            return rspObj;
        }

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        return rspObj;
    }

    /// <summary>管理者後台-工作-工項-控制器-更新專案里程碑工項工作</summary>
    [HttpPost]
    public async Task<MbsWrkJobCtlUpdateProjectStoneJobWorkRspMdl> UpdateProjectStoneJobWork(MbsWrkJobCtlUpdateProjectStoneJobWorkReqMdl reqObj)
    {
        var rspObj = new MbsWrkJobCtlUpdateProjectStoneJobWorkRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        // logical, 管理者後台-工作-工項-邏輯服務-更新專案里程碑工項工作
        var logicalReqObj = new MbsWrkJobLgcUpdateProjectStoneJobWorkReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken.Trim(),
            OperatorIP = this.HttpContext.GetClientIP(),
            EmployeeProjectStoneJobID = reqObj.EmployeeProjectStoneJobID,
            EmployeeProjectStoneJobRemark = reqObj.EmployeeProjectStoneJobRemark?.Trim(),
            EmployeeProjectStoneJobBucketList = reqObj.EmployeeProjectStoneJobBucketList
                ?.Select(x => new MbsWrkJobLgcUpdateProjectStoneJobWorkReqItemBucketMdl()
                {
                    EmployeeProjectStoneJobBucketID = x.EmployeeProjectStoneJobBucketID,
                    EmployeeProjectStoneJobBucketIsFinish = x.EmployeeProjectStoneJobBucketIsFinish,
                })
                .ToList(),
            EmployeeProjectStoneJobWorkID = reqObj.EmployeeProjectStoneJobWorkID,
            EmployeeProjectStoneJobWorkStartTime = reqObj.EmployeeProjectStoneJobWorkStartTime,
            EmployeeProjectStoneJobWorkEndTime = reqObj.EmployeeProjectStoneJobWorkEndTime,
            EmployeeProjectStoneJobWorkRemark = reqObj.EmployeeProjectStoneJobWorkRemark?.Trim(),
            EmployeeProjectStoneJobWorkFileList = reqObj.EmployeeProjectStoneJobWorkFileList
                ?.Select(x => new MbsWrkJobLgcUpdateProjectStoneJobWorkReqItemFileMdl()
                {
                    EmployeeProjectStoneJobWorkFileID = x.EmployeeProjectStoneJobWorkFileID,
                    EmployeeProjectStoneJobWorkFileUrl = x.EmployeeProjectStoneJobWorkFileUrl.Trim(),
                })
                .ToList(),
        };
        var logicalRspObj = await this._mbsWrkJobLogical.UpdateProjectStoneJobWorkAsync(logicalReqObj);
        if (logicalRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }
        if (logicalRspObj.ErrorCode != MbsErrorCodeEnum.Success)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");

            rspObj.ErrorCode = logicalRspObj.ErrorCode;
            return rspObj;
        }

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        return rspObj;
    }

    /// <summary>管理者後台-工作-工項-控制器-取得多筆專案里程碑工項工作</summary>
    [HttpPost]
    public async Task<MbsWrkJobCtlGetManyProjectStoneJobWorkRspMdl> GetManyProjectStoneJobWork(MbsWrkJobCtlGetManyProjectStoneJobWorkReqMdl reqObj)
    {
        var rspObj = new MbsWrkJobCtlGetManyProjectStoneJobWorkRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        // logical, 管理者後台-工作-工項-邏輯服務-取得多筆專案里程碑工項工作
        var logicalReqObj = new MbsWrkJobLgcGetManyProjectStoneJobWorkReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken.Trim(),
            OperatorIP = this.HttpContext.GetClientIP(),
            EmployeeProjectID = reqObj.EmployeeProjectID,
            EmployeeProjectStoneID = reqObj.EmployeeProjectStoneID,
            EmployeeProjectStoneJobID = reqObj.EmployeeProjectStoneJobID,
            StartEmployeeProjectStoneJobWorkStartTime = reqObj.StartEmployeeProjectStoneJobWorkStartTime,
            EndEmployeeProjectStoneJobWorkEndTime = reqObj.EndEmployeeProjectStoneJobWorkEndTime,
            PageIndex = reqObj.PageIndex,
            PageSize = reqObj.PageSize,
        };
        var logicalRspObj = await this._mbsWrkJobLogical.GetManyProjectStoneJobWorkAsync(logicalReqObj);
        if (logicalRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }
        if (logicalRspObj.ErrorCode != MbsErrorCodeEnum.Success)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        rspObj.EmployeeProjectStoneJobWorkList = logicalRspObj.EmployeeProjectStoneJobWorkList
            .Select(x => new MbsWrkJobCtlGetManyProjectStoneJobWorkRspItemMdl()
            {
                EmployeeProjectStoneJobWorkID = x.EmployeeProjectStoneJobWorkID,
                EmployeeProjectStoneJobWorkStartTime = x.EmployeeProjectStoneJobWorkStartTime,
                EmployeeProjectStoneJobWorkEndTime = x.EmployeeProjectStoneJobWorkEndTime,
                EmployeeProjectName = x.EmployeeProjectName,
                EmployeeProjectStoneName = x.EmployeeProjectStoneName,
                EmployeeProjectStoneJobName = x.EmployeeProjectStoneJobName,
                EmployeeName = x.EmployeeName,
                EmployeeProjectStoneJobWorkRemark = x.EmployeeProjectStoneJobWorkRemark,
            })
            .ToList();
        rspObj.TotalCount = logicalRspObj.TotalCount;
        return rspObj;
    }

    /// <summary>管理者後台-工作-工項-控制器-移除專案里程碑工項工作</summary>
    [HttpPost]
    public async Task<MbsWrkJobCtlRemoveProjectStoneJobWorkRspMdl> RemoveProjectStoneJobWork(MbsWrkJobCtlRemoveProjectStoneJobWorkReqMdl reqObj)
    {
        var rspObj = new MbsWrkJobCtlRemoveProjectStoneJobWorkRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        // logical, 管理者後台-工作-工項-邏輯服務-移除專案里程碑工項工作
        var logicalReqObj = new MbsWrkJobLgcRemoveProjectStoneJobWorkReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken.Trim(),
            OperatorIP = this.HttpContext.GetClientIP(),
            EmployeeProjectStoneJobWorkID = reqObj.EmployeeProjectStoneJobWorkID,
        };
        var logicalRspObj = await this._mbsWrkJobLogical.RemoveProjectStoneJobWorkAsync(logicalReqObj);
        if (logicalRspObj == default)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");
            return rspObj;
        }
        if (logicalRspObj.ErrorCode != MbsErrorCodeEnum.Success)
        {
            this._logger.LogError($"reqObj: {JsonSerializer.Serialize(reqObj)}");

            rspObj.ErrorCode = logicalRspObj.ErrorCode;
            return rspObj;
        }

        rspObj.ErrorCode = MbsErrorCodeEnum.Success;
        return rspObj;
    }

    #endregion

}
