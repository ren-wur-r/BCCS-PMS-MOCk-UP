using CommonLibrary.CmnXunit;
using ServiceLibrary.Core.Player.DB.Info.Format;
using System;

namespace ServiceLibraryTest.Core.Manager.DB.Info;

/// <summary>核心-管理者-資訊-資料庫服務-測試資料-取得</summary>
public class CoMgrInfoDbServiceTestDataGet : TheoryData<CmnXunitTestData<CoMgrInfDbGetReqMdl, CoMgrInfDbGetRspMdl>>
{
    /// <summary>建構式</summary>
    public CoMgrInfoDbServiceTestDataGet()
    {
        // 加入測試資料
        this.AddRange(
        [
             new CmnXunitTestData<CoMgrInfDbGetReqMdl, CoMgrInfDbGetRspMdl>()
             {
                Description = "失敗，不存在的 PlayerID = 99",
                InputObj = new CoMgrInfDbGetReqMdl()
                {
                    PlayerID = 99,
                },
                ExpectedObj = null,
             },
             new CmnXunitTestData<CoMgrInfDbGetReqMdl, CoMgrInfDbGetRspMdl>()
             {
                Description = "成功，取得 PlayerID = 1",
                InputObj = new CoMgrInfDbGetReqMdl()
                {
                    PlayerID = 1,
                },
                ExpectedObj = new CoMgrInfDbGetRspMdl()
                {
                    Password = "password1",
                    Phone = "886900000001",
                    Nickname = "nickname1",
                    LastLoginIP = null,
                    LastLoginTime = null,
                    UpdateTime = new DateTime(2024, 10, 31, 11, 24, 28, 277, DateTimeKind.Utc),
                },
             },
             new CmnXunitTestData<CoMgrInfDbGetReqMdl, CoMgrInfDbGetRspMdl>()
             {
                Description = "成功，取得 ID 3",
                InputObj = new CoMgrInfDbGetReqMdl()
                {
                    PlayerID = 3,
                },
                ExpectedObj = new CoMgrInfDbGetRspMdl()
                {
                    Password = "password1",
                    Phone = "886900000003",
                    Nickname = "nickname3",
                    LastLoginIP = "3.3.3.3",
                    LastLoginTime = new DateTime(2024, 01, 03, 03, 03, 03, 03, DateTimeKind.Utc),
                    UpdateTime = new DateTime(2024, 10, 31, 11, 35, 21, 487, DateTimeKind.Utc),
                },
             },
        ]);
    }
}
