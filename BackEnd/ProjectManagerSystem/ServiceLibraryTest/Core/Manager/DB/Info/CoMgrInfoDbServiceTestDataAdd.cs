using System;
using CommonLibrary.CmnXunit;
using ServiceLibrary.Core.Manager.DB.Info.Format;

namespace ServiceLibraryTest.Core.Manager.DB.Info;

/// <summary>核心-管理者-資訊-資料庫服務-測試資料-新增</summary>
public class CoMgrInfoDbServiceTestDataAdd : TheoryData<CmnXunitTestData<CoMgrInfDbAddReqMdl, CoMgrInfDbAddRspMdl>>
{
    /// <summary>建構式</summary>
    public CoMgrInfoDbServiceTestDataAdd()
    {
        // 加入測試資料
        this.AddRange(
        [
            new CmnXunitTestData<CoMgrInfDbAddReqMdl, CoMgrInfDbAddRspMdl>()
            {
                Description = "失敗，加入重複帳號的管理者",
                InputObj = new CoMgrInfDbAddReqMdl()
                {
                    ManagerAccount = "charming",
                    ManagerPassword = "123456",
                    ManagerName = "cc",
                    ManagerEmail = "abc@ccc.com.tw",
                    ManagerIsEnable = true,
                },
                ExpectedObj = null,
            },
            new CmnXunitTestData<CoMgrInfDbAddReqMdl, CoMgrInfDbAddRspMdl>()
            {
                Description = "成功，加入新玩家",
                InputObj = new CoMgrInfDbAddReqMdl()
                {
                    ManagerAccount = "charming2",
                    ManagerPassword = "123456",
                    ManagerName = "cc",
                    ManagerEmail = "abc@ccc.com.tw",
                    ManagerIsEnable = true,
                },
                ExpectedObj = new CoMgrInfDbAddRspMdl()
                {
                    ManagerID = 3,
                    ManagerCreateTime = DateTimeOffset.UtcNow,
                    ManagerUpdateTime = DateTimeOffset.UtcNow,
                },
            },
        ]);
    }
}
