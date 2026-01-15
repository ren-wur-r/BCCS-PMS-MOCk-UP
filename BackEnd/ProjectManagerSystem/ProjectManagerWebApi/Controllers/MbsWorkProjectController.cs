using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using ApiModelLibrary.ManagerBackSite.Common;
using ApiModelLibrary.ManagerBackSite.Controller.Work.Project;
using CommonLibrary.CmnHttpContext.Extension;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ServiceLibrary.ManagerBackSite.Logical.Work.Project.Format;
using ServiceLibrary.ManagerBackSite.Logical.Work.Project.Service;

namespace ProjectManagerWebApi.Controllers;

/// <summary>管理後台-工作-專案-控制器</summary>
[Route("api/[controller]/[action]")]
[ApiController]
public class MbsWorkProjectController : ControllerBase
{
    /// <summary>logger</summary>
    private readonly ILogger<MbsWorkProjectController> _logger;

    /// <summary>管理者後台-工作-專案-邏輯服務</summary>
    private readonly IMbsWrkProjectLogicalService _mbsWrkProjectLogical;

    /// <summary>建構式</summary>
    public MbsWorkProjectController(
        ILogger<MbsWorkProjectController> logger,
        IMbsWrkProjectLogicalService mbsWrkProjectLogical)
    {
        this._logger = logger;
        this._mbsWrkProjectLogical = mbsWrkProjectLogical;
    }

    #region 儀錶板

    /// <summary>管理後台-工作-專案-控制器-取得儀錶板</summary>
    [HttpPost]
    public async Task<MbsWrkPrjCtlGetDashboardRspMdl> GetDashboard(MbsWrkPrjCtlGetDashboardReqMdl reqObj)
    {
        var rspObj = new MbsWrkPrjCtlGetDashboardRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        // logical, 理者後台-工作-專案-邏輯服務-取得儀錶板
        var logicalReqObj = new MbsWrkPrjLgcGetDashboardReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken.Trim(),
            OperatorIP = this.HttpContext.GetClientIP(),
        };
        var logicalRspObj = await this._mbsWrkProjectLogical.GetDashboardAsync(logicalReqObj);
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
        rspObj.DelayedEmployeeProjectList = logicalRspObj.DelayedEmployeeProjectList
            .Select(x => new MbsWrkPrjCtlGetDashboardRspItemMdl()
            {
                EmployeeProjectID = x.EmployeeProjectID,
                ManagerCompanyName = x.ManagerCompanyName,
                EmployeeProjectName = x.EmployeeProjectName,
                EmployeeProjectStartTime = x.EmployeeProjectStartTime,
                EmployeeProjectEndTime = x.EmployeeProjectEndTime,
            })
            .ToList();
        rspObj.AtRiskEmployeeProjectList = logicalRspObj.AtRiskEmployeeProjectList
            .Select(x => new MbsWrkPrjCtlGetDashboardRspItemMdl()
            {
                EmployeeProjectID = x.EmployeeProjectID,
                ManagerCompanyName = x.ManagerCompanyName,
                EmployeeProjectName = x.EmployeeProjectName,
                EmployeeProjectStartTime = x.EmployeeProjectStartTime,
                EmployeeProjectEndTime = x.EmployeeProjectEndTime,
            })
            .ToList();
        rspObj.NotAssignedEmployeeProjectList = logicalRspObj.NotAssignedEmployeeProjectList
            .Select(x => new MbsWrkPrjCtlGetDashboardRspItemMdl()
            {
                EmployeeProjectID = x.EmployeeProjectID,
                ManagerCompanyName = x.ManagerCompanyName,
                EmployeeProjectName = x.EmployeeProjectName,
                EmployeeProjectStartTime = x.EmployeeProjectStartTime,
                EmployeeProjectEndTime = x.EmployeeProjectEndTime,
            })
            .ToList();
        return rspObj;
    }

    #endregion

    #region 專案

    /// <summary>管理後台-工作-專案-控制器-取得多筆專案</summary>
    [HttpPost]
    public async Task<MbsWrkPrjCtlGetManyProjectRspMdl> GetManyProject(MbsWrkPrjCtlGetManyProjectReqMdl reqObj)
    {
        var rspObj = new MbsWrkPrjCtlGetManyProjectRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        // logical, 管理者後台-工作-專案-邏輯服務-取得多筆專案
        var logicalReqObj = new MbsWrkPrjLgcGetManyProjectReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken.Trim(),
            OperatorIP = this.HttpContext.GetClientIP(),
            AtomEmployeeProjectStatus = reqObj.AtomEmployeeProjectStatus,
            EmployeeProjectName = reqObj.EmployeeProjectName?.Trim(),
            PageIndex = reqObj.PageIndex,
            PageSize = reqObj.PageSize,
        };
        var logicalRspObj = await this._mbsWrkProjectLogical.GetManyProjectAsync(logicalReqObj);
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
        rspObj.EmployeeProjectList = logicalRspObj.EmployeeProjectList
            .Select(x => new MbsWrkPrjCtlGetManyProjectRspItemMdl()
            {
                EmployeeProjectID = x.EmployeeProjectID,
                AtomEmployeeProjectStatus = x.AtomEmployeeProjectStatus,
                EmployeeProjectName = x.EmployeeProjectName,
                ManagerCompanyName = x.ManagerCompanyName,
                EmployeeProjectStartTime = x.EmployeeProjectStartTime,
                EmployeeProjectEndTime = x.EmployeeProjectEndTime
            })
            .ToList();
        rspObj.TotalCount = logicalRspObj.TotalCount;
        return rspObj;
    }

    /// <summary>管理後台-工作-專案-控制器-取得專案</summary>
    [HttpPost]
    public async Task<MbsWrkPrjCtlGetProjectRspMdl> GetProject(MbsWrkPrjCtlGetProjectReqMdl reqObj)
    {
        var rspObj = new MbsWrkPrjCtlGetProjectRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        // logical, 理者後台-工作-專案-邏輯服務-取得專案
        var logicalReqObj = new MbsWrkPrjLgcGetProjectReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken.Trim(),
            OperatorIP = this.HttpContext.GetClientIP(),
            EmployeeProjectID = reqObj.EmployeeProjectID,
        };
        var logicalRspObj = await this._mbsWrkProjectLogical.GetProjectAsync(logicalReqObj);
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
        rspObj.AtomEmployeeProjectStatus = logicalRspObj.AtomEmployeeProjectStatus;
        rspObj.EmployeeProjectName = logicalRspObj.EmployeeProjectName;
        rspObj.EmployeeProjectCode = logicalRspObj.EmployeeProjectCode;
        rspObj.EmployeeProjectStartTime = logicalRspObj.EmployeeProjectStartTime;
        rspObj.EmployeeProjectEndTime = logicalRspObj.EmployeeProjectEndTime;
        rspObj.ManagerCompanyName = logicalRspObj.ManagerCompanyName;
        rspObj.EmployeeProjectContractUrl = logicalRspObj.EmployeeProjectContractUrl;
        rspObj.EmployeeProjectContractCreateTime = logicalRspObj.EmployeeProjectContractCreateTime;
        rspObj.EmployeeProjectWorkUrl = logicalRspObj.EmployeeProjectWorkUrl;
        rspObj.EmployeeProjectWorkCreateTime = logicalRspObj.EmployeeProjectWorkCreateTime;
        rspObj.EmployeeProjectMemberList = logicalRspObj.EmployeeProjectMemberList
            .Select(x => new MbsWrkPrjCtlGetProjectRspItemMdl()
            {
                EmployeeProjectMemberID = x.EmployeeProjectMemberID,
                EmployeeProjectMemberRole = x.EmployeeProjectMemberRole,
                ManagerRegionName = x.ManagerRegionName,
                ManagerDepartmentName = x.ManagerDepartmentName,
                EmployeeName = x.EmployeeName,
                EmployeeID = x.EmployeeID,
            })
            .ToList();
        return rspObj;
    }

    /// <summary>管理後台-工作-專案-控制器-新增專案</summary>
    [HttpPost]
    public async Task<MbsWrkPrjCtlAddProjectRspMdl> AddProject(MbsWrkPrjCtlAddProjectReqMdl reqObj)
    {
        var rspObj = new MbsWrkPrjCtlAddProjectRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        // logical, 理者後台-工作-專案-邏輯服務-新增專案
        var logicalReqObj = new MbsWrkPrjLgcAddProjectReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken.Trim(),
            OperatorIP = this.HttpContext.GetClientIP(),
            ManagerCompanyID = reqObj.ManagerCompanyID,
            EmployeeProjectCode = reqObj.EmployeeProjectCode.Trim(),
            EmployeeProjectName = reqObj.EmployeeProjectName.Trim(),
            EmployeeProjectStartTime = reqObj.EmployeeProjectStartTime,
            EmployeeProjectEndTime = reqObj.EmployeeProjectEndTime,
            EmployeeProjectContractUrl = reqObj.EmployeeProjectContractUrl?.Trim(),
            EmployeeProjectWorkUrl = reqObj.EmployeeProjectWorkUrl?.Trim(),
            EmployeeProjectMemberList = reqObj.EmployeeProjectMemberList
                .Select(x => new MbsWrkPrjLgcAddProjectReqItemMdl()
                {
                    EmployeeID = x.EmployeeID,
                    EmployeeProjectMemberRole = x.EmployeeProjectMemberRole,
                })
                .ToList(),
        };
        var logicalRspObj = await this._mbsWrkProjectLogical.AddProjectAsync(logicalReqObj);
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

    /// <summary>管理後台-工作-專案-控制器-更新專案</summary>
    [HttpPost]
    public async Task<MbsWrkPrjCtlUpdateProjectRspMdl> UpdateProject(MbsWrkPrjCtlUpdateProjectReqMdl reqObj)
    {
        var rspObj = new MbsWrkPrjCtlUpdateProjectRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        // logical, 管理者後台-工作-專案-邏輯服務-更新專案
        var logicalReqObj = new MbsWrkPrjLgcUpdateProjectReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken.Trim(),
            OperatorIP = this.HttpContext.GetClientIP(),
            EmployeeProjectID = reqObj.EmployeeProjectID,
            EmployeeProjectCode = reqObj.EmployeeProjectCode?.Trim(),
            EmployeeProjectName = reqObj.EmployeeProjectName?.Trim()
        };
        var logicalRspObj = await this._mbsWrkProjectLogical.UpdateProjectAsync(logicalReqObj);
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

    #region 合約

    /// <summary>管理後台-工作-專案-控制器-新增合約</summary>
    [HttpPost]
    public async Task<MbsWrkPrjCtlAddContractRspMdl> AddContract(MbsWrkPrjCtlAddContractReqMdl reqObj)
    {
        var rspObj = new MbsWrkPrjCtlAddContractRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        // logical, 管理者後台-工作-專案-邏輯服務-新增合約
        var logicalReqObj = new MbsWrkPrjLgcAddContractReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken.Trim(),
            OperatorIP = this.HttpContext.GetClientIP(),
            EmployeeProjectID = reqObj.EmployeeProjectID,
            EmployeeProjectContractUrl = reqObj.EmployeeProjectContractUrl.Trim(),
        };
        var logicalRspObj = await this._mbsWrkProjectLogical.AddContractAsync(logicalReqObj);
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

    #region 工作計劃書

    /// <summary>管理後台-工作-專案-控制器-新增工作計劃書</summary>
    [HttpPost]
    public async Task<MbsWrkPrjCtlAddWorkRspMdl> AddWork(MbsWrkPrjCtlAddWorkReqMdl reqObj)
    {
        var rspObj = new MbsWrkPrjCtlAddWorkRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        // logical, 管理者後台-工作-專案-邏輯服務-新增工作計劃書
        var logicalReqObj = new MbsWrkPrjLgcAddWorkReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken.Trim(),
            OperatorIP = this.HttpContext.GetClientIP(),
            EmployeeProjectID = reqObj.EmployeeProjectID,
            EmployeeProjectWorkUrl = reqObj.EmployeeProjectWorkUrl.Trim(),
        };
        var logicalRspObj = await this._mbsWrkProjectLogical.AddWorkAsync(logicalReqObj);
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

    #region 成員

    /// <summary>管理後台-工作-專案-控制器-新增成員</summary>
    [HttpPost]
    public async Task<MbsWrkPrjCtlAddMemberRspMdl> AddMember(MbsWrkPrjCtlAddMemberReqMdl reqObj)
    {
        var rspObj = new MbsWrkPrjCtlAddMemberRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        // logical, 管理者後台-工作-專案-邏輯服務-新增成員
        var logicalReqObj = new MbsWrkPrjLgcAddMemberReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken.Trim(),
            OperatorIP = this.HttpContext.GetClientIP(),
            EmployeeProjectID = reqObj.EmployeeProjectID,
            EmployeeID = reqObj.EmployeeID,
            EmployeeProjectMemberRole = reqObj.EmployeeProjectMemberRole,
        };
        var logicalRspObj = await this._mbsWrkProjectLogical.AddMemberAsync(logicalReqObj);
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

    /// <summary>管理後台-工作-專案-控制器-移除成員</summary>
    [HttpPost]
    public async Task<MbsWrkPrjCtlRemoveMemberRspMdl> RemoveMember(MbsWrkPrjCtlRemoveMemberReqMdl reqObj)
    {
        var rspObj = new MbsWrkPrjCtlRemoveMemberRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        // logical, 管理者後台-工作-專案-邏輯服務-移除成員
        var logicalReqObj = new MbsWrkPrjLgcRemoveMemberReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken.Trim(),
            OperatorIP = this.HttpContext.GetClientIP(),
            EmployeeProjectMemberID = reqObj.EmployeeProjectMemberID,
        };
        var logicalRspObj = await this._mbsWrkProjectLogical.RemoveMemberAsync(logicalReqObj);
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

    /// <summary>管理後台-工作-專案-控制器-取得多筆成員角色</summary>
    [HttpPost]
    public async Task<MbsWrkPrjCtlGetManyMemberRoleRspMdl> GetManyMemberRoleAsync(MbsWrkPrjCtlGetManyMemberRoleReqMdl reqObj)
    {
        var rspObj = new MbsWrkPrjCtlGetManyMemberRoleRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        // logical, 管理者後台-工作-專案-邏輯服務-取得多筆成員角色
        var logicalReqObj = new MbsWrkPrjLgcGetManyMemberRoleReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken.Trim(),
            OperatorIP = this.HttpContext.GetClientIP(),
            EmployeeProjectID = reqObj.EmployeeProjectID,
        };
        var logicalRspObj = await this._mbsWrkProjectLogical.GetManyMemberRoleAsync(logicalReqObj);
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
        rspObj.EmployeeProjectMemberList = logicalRspObj.EmployeeProjectMemberList
            .Select(x => new MbsWrkPrjCtlGetManyMemberRoleRspItemMdl()
            {
                EmployeeProjectMemberRole = x.EmployeeProjectMemberRole,
            })
            .ToList();
        return rspObj;
    }

    #endregion

    #region 里程碑

    /// <summary>管理後台-工作-專案-控制器-取得多筆專案里程碑</summary>
    [HttpPost]
    public async Task<MbsWrkPrjCtlGetManyProjectStoneRspMdl> GetManyProjectStone(MbsWrkPrjCtlGetManyProjectStoneReqMdl reqObj)
    {
        var rspObj = new MbsWrkPrjCtlGetManyProjectStoneRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        // logical, 管理者後台-工作-專案-邏輯服務-取得多筆專案里程碑
        var logicalReqObj = new MbsWrkPrjLgcGetManyProjectStoneReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken.Trim(),
            OperatorIP = this.HttpContext.GetClientIP(),
            EmployeeProjectID = reqObj.EmployeeProjectID,
        };
        var logicalRspObj = await this._mbsWrkProjectLogical.GetManyProjectStoneAsync(logicalReqObj);
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
        rspObj.EmployeeProjectStoneList = logicalRspObj.EmployeeProjectStoneList
            .Select(x => new MbsWrkPrjCtlGetManyProjectStoneRspItemStoneMdl()
            {
                EmployeeProjectStoneID = x.EmployeeProjectStoneID,
                EmployeeProjectStoneName = x.EmployeeProjectStoneName,
                EmployeeProjectStoneStartTime = x.EmployeeProjectStoneStartTime,
                EmployeeProjectStoneEndTime = x.EmployeeProjectStoneEndTime,
                EmployeeProjectStonePreNotifyDay = x.EmployeeProjectStonePreNotifyDay,
                AtomEmployeeProjectStatus = x.AtomEmployeeProjectStatus,
                EmployeeProjectStoneJobList = x.EmployeeProjectStoneJobList
                    .Select(t => new MbsWrkPrjCtlGetManyProjectStoneRspItemJobMdl()
                    {
                        EmployeeProjectStoneJobID = t.EmployeeProjectStoneJobID,
                        EmployeeProjectStoneJobName = t.EmployeeProjectStoneJobName,
                        EmployeeProjectStoneJobStartTime = t.EmployeeProjectStoneJobStartTime,
                        EmployeeProjectStoneJobEndTime = t.EmployeeProjectStoneJobEndTime,
                        AtomEmployeeProjectStatus = t.AtomEmployeeProjectStatus,
                        EmployeeProjectStoneJobExecutorCount = t.EmployeeProjectStoneJobExecutorCount,
                    })
                    .ToList()
            })
            .ToList();
        return rspObj;
    }

    /// <summary>管理後台-工作-專案-控制器-取得專案里程碑</summary>
    [HttpPost]
    public async Task<MbsWrkPrjCtlGetProjectStoneRspMdl> GetProjectStone(MbsWrkPrjCtlGetProjectStoneReqMdl reqObj)
    {
        var rspObj = new MbsWrkPrjCtlGetProjectStoneRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        // logical, 管理者後台-工作-專案-邏輯服務-取得多筆專案里程碑
        var logicalReqObj = new MbsWrkPrjLgcGetProjectStoneReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken.Trim(),
            OperatorIP = this.HttpContext.GetClientIP(),
            EmployeeProjectStoneID = reqObj.EmployeeProjectStoneID,
        };
        var logicalRspObj = await this._mbsWrkProjectLogical.GetProjectStoneAsync(logicalReqObj);
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

        rspObj.EmployeeProjectStoneName = logicalRspObj.EmployeeProjectStoneName;
        rspObj.EmployeeProjectStoneStartTime = logicalRspObj.EmployeeProjectStoneStartTime;
        rspObj.EmployeeProjectStoneEndTime = logicalRspObj.EmployeeProjectStoneEndTime;
        rspObj.EmployeeProjectStonePreNotifyDay = logicalRspObj.EmployeeProjectStonePreNotifyDay;
        rspObj.EmployeeProjectStoneJobList = logicalRspObj.EmployeeProjectStoneJobList
            .Select(t => new MbsWrkPrjCtlGetProjectStoneRspItemJobMdl()
            {
                EmployeeProjectStoneJobID = t.EmployeeProjectStoneJobID,
                EmployeeProjectStoneJobName = t.EmployeeProjectStoneJobName,
                EmployeeProjectStoneJobStartTime = t.EmployeeProjectStoneJobStartTime,
                EmployeeProjectStoneJobEndTime = t.EmployeeProjectStoneJobEndTime,
                EmployeeProjectStoneJobWorkHour = t.EmployeeProjectStoneJobWorkHour,
                EmployeeProjectStoneJobExecutorList = t.EmployeeProjectStoneJobExecutorList
                    .Select(e => new MbsWrkPrjCtlGetProjectStoneRspItemExecutorMdl()
                    {
                        EmployeeID = e.EmployeeID,
                        EmployeeName = e.EmployeeName,
                    })
                    .ToList(),
            })
            .ToList();
        return rspObj;
    }

    /// <summary>管理後台-工作-專案-控制器-新增專案里程碑</summary>
    [HttpPost]
    public async Task<MbsWrkPrjCtlAddProjectStoneRspMdl> AddProjectStone(MbsWrkPrjCtlAddProjectStoneReqMdl reqObj)
    {
        var rspObj = new MbsWrkPrjCtlAddProjectStoneRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        // logical, 管理者後台-工作-專案-邏輯服務-新增專案里程碑
        var logicalReqObj = new MbsWrkPrjLgcAddProjectStoneReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken.Trim(),
            OperatorIP = this.HttpContext.GetClientIP(),
            EmployeeProjectID = reqObj.EmployeeProjectID,
            EmployeeProjectStoneName = reqObj.EmployeeProjectStoneName.Trim(),
            EmployeeProjectStonePreNotifyDay = reqObj.EmployeeProjectStonePreNotifyDay,
            EmployeeProjectStoneStartTime = reqObj.EmployeeProjectStoneStartTime,
            EmployeeProjectStoneEndTime = reqObj.EmployeeProjectStoneEndTime,
            EmployeeProjectStoneJobList = reqObj.EmployeeProjectStoneJobList
                ?.Select(x => new MbsWrkPrjLgcAddProjectStoneReqItemJobMdl()
                {
                    EmployeeProjectStoneJobName = x.EmployeeProjectStoneJobName.Trim(),
                    EmployeeProjectStoneJobStartTime = x.EmployeeProjectStoneJobStartTime,
                    EmployeeProjectStoneJobEndTime = x.EmployeeProjectStoneJobEndTime,
                    EmployeeProjectStoneJobWorkHour = x.EmployeeProjectStoneJobWorkHour,
                    EmployeeProjectStoneJobRemark = x.EmployeeProjectStoneJobRemark?.Trim(),
                    EmployeeProjectStoneJobExecutorList = x.EmployeeProjectStoneJobExecutorList
                        ?.Select(e => new MbsWrkPrjLgcAddProjectStoneReqItemExecutorMdl()
                        {
                            EmployeeID = e.EmployeeID,
                        })
                        .ToList(),
                })
                .ToList(),
        };
        var logicalRspObj = await this._mbsWrkProjectLogical.AddProjectStoneAsync(logicalReqObj);
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

    /// <summary>管理後台-工作-專案-控制器-更新專案里程碑</summary>
    [HttpPost]
    public async Task<MbsWrkPrjCtlUpdateProjectStoneRspMdl> UpdateProjectStone(MbsWrkPrjCtlUpdateProjectStoneReqMdl reqObj)
    {
        var rspObj = new MbsWrkPrjCtlUpdateProjectStoneRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        // logical, 管理者後台-工作-專案-邏輯服務-更新專案里程碑
        var logicalReqObj = new MbsWrkPrjLgcUpdateProjectStoneReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken.Trim(),
            OperatorIP = this.HttpContext.GetClientIP(),
            EmployeeProjectStoneID = reqObj.EmployeeProjectStoneID,
            EmployeeProjectStoneName = reqObj.EmployeeProjectStoneName?.Trim(),
            EmployeeProjectStoneStartTime = reqObj.EmployeeProjectStoneStartTime,
            EmployeeProjectStoneEndTime = reqObj.EmployeeProjectStoneEndTime,
            EmployeeProjectStonePreNotifyDay = reqObj.EmployeeProjectStonePreNotifyDay,
            EmployeeProjectStoneJobList = reqObj.EmployeeProjectStoneJobList
                ?.Select(x => new MbsWrkPrjLgcUpdateProjectStoneReqItemJobMdl()
                {
                    EmployeeProjectStoneJobID = x.EmployeeProjectStoneJobID,
                    EmployeeProjectStoneJobName = x.EmployeeProjectStoneJobName.Trim(),
                    EmployeeProjectStoneJobStartTime = x.EmployeeProjectStoneJobStartTime,
                    EmployeeProjectStoneJobEndTime = x.EmployeeProjectStoneJobEndTime,
                    EmployeeProjectStoneJobWorkHour = x.EmployeeProjectStoneJobWorkHour,
                    EmployeeProjectStoneJobExecutorList = x.EmployeeProjectStoneJobExecutorList
                        ?.Select(e => new MbsWrkPrjLgcUpdateProjectStoneReqItemExecutorMdl()
                        {
                            EmployeeID = e.EmployeeID,
                        })
                        .ToList(),
                })
                .ToList(),
        };
        var logicalRspObj = await this._mbsWrkProjectLogical.UpdateProjectStoneAsync(logicalReqObj);
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

    /// <summary>管理後台-工作-專案-控制器-移除專案里程碑</summary>
    [HttpPost]
    public async Task<MbsWrkPrjCtlRemoveProjectStoneRspMdl> RemoveProjectStone(MbsWrkPrjCtlRemoveProjectStoneReqMdl reqObj)
    {
        var rspObj = new MbsWrkPrjCtlRemoveProjectStoneRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        // logical, 管理者後台-工作-專案-邏輯服務-移除專案里程碑
        var logicalReqObj = new MbsWrkPrjLgcRemoveProjectStoneReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken.Trim(),
            OperatorIP = this.HttpContext.GetClientIP(),
            EmployeeProjectStoneID = reqObj.EmployeeProjectStoneID,
        };
        var logicalRspObj = await this._mbsWrkProjectLogical.RemoveProjectStoneAsync(logicalReqObj);
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

    #region 工項

    /// <summary>管理後台-工作-專案-控制器-取得多筆專案里程碑工項</summary>
    [HttpPost]
    public async Task<MbsWrkPrjCtlGetManyProjectStoneJobRspMdl> GetManyProjectStoneTask(MbsWrkPrjCtlGetManyProjectStoneJobReqMdl reqObj)
    {
        var rspObj = new MbsWrkPrjCtlGetManyProjectStoneJobRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        // logical, 管理者後台-工作-專案-邏輯服務-取得多筆專案里程碑工項
        var logicalReqObj = new MbsWrkPrjLgcGetManyProjectStoneJobReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken.Trim(),
            OperatorIP = this.HttpContext.GetClientIP(),
            EmployeeProjectID = reqObj.EmployeeProjectID,
        };
        var logicalRspObj = await this._mbsWrkProjectLogical.GetManyProjectStoneJobAsync(logicalReqObj);
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
        rspObj.EmployeeProjectStoneList = logicalRspObj.EmployeeProjectStoneList
            .Select(stone => new MbsWrkPrjCtlGetManyProjectStoneJobRspItemStoneMdl()
            {
                EmployeeProjectStoneID = stone.EmployeeProjectStoneID,
                EmployeeProjectStoneName = stone.EmployeeProjectStoneName,
                EmployeeProjectStonePreNotifyDay = stone.EmployeeProjectStonePreNotifyDay,
                EmployeeProjectStoneStartTime = stone.EmployeeProjectStoneStartTime,
                EmployeeProjectStoneEndTime = stone.EmployeeProjectStoneEndTime,
                AtomEmployeeProjectStatus = stone.AtomEmployeeProjectStatus,
                EmployeeProjectStoneJobList = stone.EmployeeProjectStoneJobList
                    .Select(task => new MbsWrkPrjCtlGetManyProjectStoneJobRspItemJobMdl()
                    {
                        EmployeeProjectStoneJobID = task.EmployeeProjectStoneJobID,
                        EmployeeProjectStoneJobName = task.EmployeeProjectStoneJobName,
                        EmployeeProjectStoneJobStartTime = task.EmployeeProjectStoneJobStartTime,
                        EmployeeProjectStoneJobEndTime = task.EmployeeProjectStoneJobEndTime,
                        EmployeeProjectStoneJobWorkHour = task.EmployeeProjectStoneJobWorkHour,
                        AtomEmployeeProjectStatus = task.AtomEmployeeProjectStatus,
                        EmployeeProjectStoneJobExecutorCount = task.EmployeeProjectStoneJobExecutorCount,
                        EmployeeProjectStoneJobBucketList = task.EmployeeProjectStoneJobBucketList
                            .Select(bucket => new MbsWrkPrjCtlGetManyProjectStoneJobRspItemBucketMdl()
                            {
                                EmployeeProjectStoneJobBucketID = bucket.EmployeeProjectStoneJobBucketID,
                                EmployeeProjectStoneJobBucketName = bucket.EmployeeProjectStoneJobBucketName,
                                EmployeeProjectStoneJobBucketIsFinish = bucket.EmployeeProjectStoneJobBucketIsFinish,
                            })
                            .ToList()
                    })
                    .ToList()
            })
            .ToList();
        return rspObj;
    }

    /// <summary>管理後台-工作-專案-控制器-取得專案里程碑工項</summary>
    [HttpPost]
    public async Task<MbsWrkPrjCtlGetProjectStoneJobRspMdl> GetProjectStoneTask(MbsWrkPrjCtlGetProjectStoneJobReqMdl reqObj)
    {
        var rspObj = new MbsWrkPrjCtlGetProjectStoneJobRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        // logical, 管理者後台-工作-專案-邏輯服務-取得專案里程碑工項
        var logicalReqObj = new MbsWrkPrjLgcGetProjectStoneJobReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken.Trim(),
            OperatorIP = this.HttpContext.GetClientIP(),
            EmployeeProjectStoneJobID = reqObj.EmployeeProjectStoneJobID,
        };
        var logicalRspObj = await this._mbsWrkProjectLogical.GetProjectStoneJobAsync(logicalReqObj);
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
        rspObj.EmployeeProjectStoneID = logicalRspObj.EmployeeProjectStoneID;
        rspObj.EmployeeProjectStoneName = logicalRspObj.EmployeeProjectStoneName;
        rspObj.EmployeeProjectStoneStartTime = logicalRspObj.EmployeeProjectStoneStartTime;
        rspObj.EmployeeProjectStoneEndTime = logicalRspObj.EmployeeProjectStoneEndTime;
        rspObj.EmployeeProjectStoneJobName = logicalRspObj.EmployeeProjectStoneJobName;
        rspObj.EmployeeProjectStoneJobStartTime = logicalRspObj.EmployeeProjectStoneJobStartTime;
        rspObj.EmployeeProjectStoneJobEndTime = logicalRspObj.EmployeeProjectStoneJobEndTime;
        rspObj.EmployeeProjectStoneJobWorkHour = logicalRspObj.EmployeeProjectStoneJobWorkHour;
        rspObj.AtomEmployeeProjectStatus = logicalRspObj.AtomEmployeeProjectStatus;
        rspObj.EmployeeProjectStoneJobRemark = logicalRspObj.EmployeeProjectStoneJobRemark;
        rspObj.EmployeeProjectStoneJobExecutorList = logicalRspObj.EmployeeProjectStoneJobExecutorList
            .Select(x => new MbsWrkPrjCtlGetProjectStoneBucketRspItemExecutorMdl()
            {
                EmployeeProjectStoneJobExecutorEmployeeID = x.EmployeeProjectStoneJobExecutorEmployeeID,
                EmployeeProjectStoneJobExecutorEmployeeName = x.EmployeeProjectStoneJobExecutorEmployeeName,
            })
            .ToList();
        rspObj.EmployeeProjectStoneJobBucketList = logicalRspObj.EmployeeProjectStoneJobBucketList
            .Select(x => new MbsWrkPrjCtlGetProjectStoneJobRspItemBucketMdl()
            {
                EmployeeProjectStoneJobBucketID = x.EmployeeProjectStoneJobBucketID,
                EmployeeProjectStoneJobBucketName = x.EmployeeProjectStoneJobBucketName,
                EmployeeProjectStoneJobBucketIsFinish = x.EmployeeProjectStoneJobBucketIsFinish,
            })
            .ToList();
        return rspObj;
    }

    /// <summary>管理後台-工作-專案-控制器-更新專案里程碑工項</summary>
    [HttpPost]
    public async Task<MbsWrkPrjCtlUpdateProjectStoneJobRspMdl> UpdateProjectStoneTask(MbsWrkPrjCtlUpdateProjectStoneJobReqMdl reqObj)
    {
        var rspObj = new MbsWrkPrjCtlUpdateProjectStoneJobRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        // logical, 管理者後台-工作-專案-邏輯服務-更新專案里程碑工項
        var logicalReqObj = new MbsWrkPrjLgcUpdateProjectStoneJobReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken.Trim(),
            OperatorIP = this.HttpContext.GetClientIP(),
            EmployeeProjectStoneJobID = reqObj.EmployeeProjectStoneJobID,
            EmployeeProjectStoneJobRemark = reqObj.EmployeeProjectStoneJobRemark?.Trim(),
            EmployeeProjectStoneJobExecutorIdList = reqObj.EmployeeProjectStoneJobExecutorIdList,
            EmployeeProjectStoneJobBucketList = reqObj.EmployeeProjectStoneJobBucketList?
                .Select(x => new MbsWrkPrjLgcUpdateProjectStoneJobReqItemMdl()
                {
                    EmployeeProjectStoneJobBucketID = x.EmployeeProjectStoneJobBucketID,
                    EmployeeProjectStoneJobBucketName = x.EmployeeProjectStoneJobBucketName?.Trim(),
                    EmployeeProjectStoneJobBucketIsFinish = x.EmployeeProjectStoneJobBucketIsFinish,
                })
                .ToList(),
        };
        var logicalRspObj = await this._mbsWrkProjectLogical.UpdateProjectStoneJobAsync(logicalReqObj);
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

    /// <summary>管理後台-工作-專案-控制器-更新專案里程碑工項清單</summary>
    [HttpPost]
    public async Task<MbsWrkPrjCtlUpdateProjectStoneJobBucketRspMdl> UpdateProjectStoneTaskBucket(MbsWrkPrjCtlUpdateProjectStoneJobBucketReqMdl reqObj)
    {
        var rspObj = new MbsWrkPrjCtlUpdateProjectStoneJobBucketRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        // logical, 管理者後台-工作-專案-邏輯服務-更新專案里程碑工項清單
        var logicalReqObj = new MbsWrkPrjLgcUpdateProjectStoneJobBucketReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken.Trim(),
            OperatorIP = this.HttpContext.GetClientIP(),
            EmployeeProjectStoneJobBucketID = reqObj.EmployeeProjectStoneJobBucketID,
            EmployeeProjectStoneJobBucketIsFinish = reqObj.EmployeeProjectStoneJobBucketIsFinish,
        };
        var logicalRspObj = await this._mbsWrkProjectLogical.UpdateProjectStoneJobBucketAsync(logicalReqObj);
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

    #region 支出

    /// <summary>管理者後台-工作-專案-控制器-取得專案支出</summary>
    [HttpPost]
    public async Task<MbsWrkPrjCtlGetExpenseRspMdl> GetExpense(MbsWrkPrjCtlGetExpenseReqMdl reqObj)
    {
        var rspObj = new MbsWrkPrjCtlGetExpenseRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        // logical, 管理者後台-工作-專案-邏輯服務-取得專案支出
        var logicalReqObj = new MbsWrkPrjLgcGetExpenseReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken.Trim(),
            OperatorIP = this.HttpContext.GetClientIP(),
            EmployeeProjectExpenseID = reqObj.EmployeeProjectExpenseID,
        };
        var logicalRspObj = await this._mbsWrkProjectLogical.GetExpenseAsync(logicalReqObj);
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
        rspObj.EmployeeProjectID = logicalRspObj.EmployeeProjectID;
        rspObj.EmployeeProjectExpenseName = logicalRspObj.EmployeeProjectExpenseName;
        rspObj.EmployeeProjectExpensePredictAmount = logicalRspObj.EmployeeProjectExpensePredictAmount;
        rspObj.EmployeeProjectExpenseActualAmount = logicalRspObj.EmployeeProjectExpenseActualAmount;
        rspObj.EmployeeProjectExpenseBillNumber = logicalRspObj.EmployeeProjectExpenseBillNumber;
        rspObj.EmployeeProjectExpenseBillTime = logicalRspObj.EmployeeProjectExpenseBillTime;
        rspObj.EmployeeProjectExpenseRemark = logicalRspObj.EmployeeProjectExpenseRemark;
        return rspObj;
    }

    /// <summary>管理者後台-工作-專案-控制器-取得多筆專案支出</summary>
    [HttpPost]
    public async Task<MbsWrkPrjCtlGetManyExpenseRspMdl> GetManyExpense(MbsWrkPrjCtlGetManyExpenseReqMdl reqObj)
    {
        var rspObj = new MbsWrkPrjCtlGetManyExpenseRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        // logical, 管理者後台-工作-專案-邏輯服務-取得多筆專案支出
        var logicalReqObj = new MbsWrkPrjLgcGetManyExpenseReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken.Trim(),
            OperatorIP = this.HttpContext.GetClientIP(),
            EmployeeProjectID = reqObj.EmployeeProjectID,
        };
        var logicalRspObj = await this._mbsWrkProjectLogical.GetManyExpenseAsync(logicalReqObj);
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
        rspObj.EmployeeProjectExpenseList = logicalRspObj.EmployeeProjectExpenseList
            .Select(x => new MbsWrkPrjCtlGetManyExpenseRspItemMdl()
            {
                EmployeeProjectExpenseID = x.EmployeeProjectExpenseID,
                EmployeeProjectExpenseName = x.EmployeeProjectExpenseName,
                EmployeeProjectExpensePredictAmount = x.EmployeeProjectExpensePredictAmount,
                EmployeeProjectExpenseActualAmount = x.EmployeeProjectExpenseActualAmount,
                EmployeeProjectExpenseBillNumber = x.EmployeeProjectExpenseBillNumber,
                EmployeeProjectExpenseBillTime = x.EmployeeProjectExpenseBillTime,
                EmployeeProjectExpenseRemark = x.EmployeeProjectExpenseRemark,
            })
            .ToList();
        rspObj.EipProjectExpenseList = logicalRspObj.EipProjectExpenseList
            .Select(x => new MbsWrkPrjCtlGetEipExpenseRspItemMdl()
            {
                ProjectExpenseApprovalStatus = x.ProjectExpenseApprovalStatus,
                ProjectExpenseApplicant = x.ProjectExpenseApplicant,
                ProjectExpenseDate = x.ProjectExpenseDate,
                ProjectExpenseReason = x.ProjectExpenseReason,
                ProjectExpenseParticipants = x.ProjectExpenseParticipants,
                ProjectExpenseAmount = x.ProjectExpenseAmount,
            })
            .ToList();
        rspObj.EipProjectTravelExpenseList = logicalRspObj.EipProjectTravelExpenseList
            .Select(x => new MbsWrkPrjCtlGetEipTravelExpenseRspItemMdl()
            {
                ProjectTravelExpenseApprovalStatus = x.ProjectTravelExpenseApprovalStatus,
                ProjectTravelExpenseApplicant = x.ProjectTravelExpenseApplicant,
                ProjectTravelExpenseTravelDate = x.ProjectTravelExpenseTravelDate,
                ProjectTravelExpenseTravelRoute = x.ProjectTravelExpenseTravelRoute,
                ProjectTravelExpenseWorkDescription = x.ProjectTravelExpenseWorkDescription,
                ProjectTravelExpenseTransportationTool = x.ProjectTravelExpenseTransportationTool,
                ProjectTravelExpenseTransportationAmount = x.ProjectTravelExpenseTransportationAmount,
                ProjectTravelExpenseMileage = x.ProjectTravelExpenseMileage,
                ProjectTravelExpenseAccommodationAmount = x.ProjectTravelExpenseAccommodationAmount,
                ProjectTravelExpenseSpecialExpenseReason = x.ProjectTravelExpenseSpecialExpenseReason,
                ProjectTravelExpenseSpecialExpenseAmount = x.ProjectTravelExpenseSpecialExpenseAmount,
                ProjectTravelExpenseTotalAmount = x.ProjectTravelExpenseTotalAmount,
            })
            .ToList();
        return rspObj;
    }

    /// <summary>管理者後台-工作-專案-控制器-新增專案支出</summary>
    [HttpPost]
    public async Task<MbsWrkPrjCtlAddExpenseRspMdl> AddExpense(MbsWrkPrjCtlAddExpenseReqMdl reqObj)
    {
        var rspObj = new MbsWrkPrjCtlAddExpenseRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        // logical, 管理者後台-工作-專案-邏輯服務-新增專案支出
        var logicalReqObj = new MbsWrkPrjLgcAddExpenseReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken.Trim(),
            OperatorIP = this.HttpContext.GetClientIP(),
            EmployeeProjectID = reqObj.EmployeeProjectID,
            EmployeeProjectExpenseName = reqObj.EmployeeProjectExpenseName.Trim(),
            EmployeeProjectExpensePredictAmount = reqObj.EmployeeProjectExpensePredictAmount,
            EmployeeProjectExpenseActualAmount = reqObj.EmployeeProjectExpenseActualAmount,
            EmployeeProjectExpenseBillNumber = reqObj.EmployeeProjectExpenseBillNumber?.Trim(),
            EmployeeProjectExpenseBillTime = reqObj.EmployeeProjectExpenseBillTime,
            EmployeeProjectExpenseRemark = reqObj.EmployeeProjectExpenseRemark?.Trim(),
        };
        var logicalRspObj = await this._mbsWrkProjectLogical.AddExpenseAsync(logicalReqObj);
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

    /// <summary>管理者後台-工作-專案-控制器-更新專案支出</summary>
    [HttpPost]
    public async Task<MbsWrkPrjCtlUpdateExpenseRspMdl> UpdateExpense(MbsWrkPrjCtlUpdateExpenseReqMdl reqObj)
    {
        var rspObj = new MbsWrkPrjCtlUpdateExpenseRspMdl
        {
            ErrorCode = MbsErrorCodeEnum.Fail
        };

        // logical, 管理者後台-工作-專案-邏輯服務-更新專案支出
        var logicalReqObj = new MbsWrkPrjLgcUpdateExpenseReqMdl()
        {
            EmployeeLoginToken = reqObj.EmployeeLoginToken.Trim(),
            OperatorIP = this.HttpContext.GetClientIP(),
            EmployeeProjectExpenseID = reqObj.EmployeeProjectExpenseID,
            EmployeeProjectExpenseName = reqObj.EmployeeProjectExpenseName.Trim(),
            EmployeeProjectExpensePredictAmount = reqObj.EmployeeProjectExpensePredictAmount,
            EmployeeProjectExpenseActualAmount = reqObj.EmployeeProjectExpenseActualAmount,
            EmployeeProjectExpenseBillNumber = reqObj.EmployeeProjectExpenseBillNumber?.Trim(),
            EmployeeProjectExpenseBillTime = reqObj.EmployeeProjectExpenseBillTime,
            EmployeeProjectExpenseRemark = reqObj.EmployeeProjectExpenseRemark?.Trim(),
        };
        var logicalRspObj = await this._mbsWrkProjectLogical.UpdateExpenseAsync(logicalReqObj);
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
