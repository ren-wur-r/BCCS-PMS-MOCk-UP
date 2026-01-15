using CommonLibrary.CmnXunit;
using ServiceLibrary.Core.Player.DB.Info.Format;
using System;

namespace ServiceLibraryTest.Core.Manager.DB.Info;

/// <summary>核心-管理者-資訊-資料庫服務-測試資料-更新</summary>
public class CoMgrInfoDbServiceTestDataUpdate : TheoryData<CmnXunitTestData<CoMgrInfDbUpdateReqMdl, CoMgrInfDbUpdateRspMdl>>
{
    /// <summary>建構式</summary>
    public CoMgrInfoDbServiceTestDataUpdate()
    {
        // 加入測試資料
        this.AddRange(
        [
            new CmnXunitTestData<CoMgrInfDbUpdateReqMdl, CoMgrInfDbUpdateRspMdl>()
            {
                Description = "失敗，更新不存在的 PlayerID = 99",
                InputObj = new CoMgrInfDbUpdateReqMdl()
                {
                    PlayerID = 99,
                    Password = "Password11",
                    Phone = "886911111111",
                    Nickname = "Nickname11",
                    LastLoginIP = "11.11.11.11",
                    LastLoginTime = new DateTimeOffset(2024, 11, 11, 11, 11, 11, 11, TimeSpan.Zero)
                },
                ExpectedObj = null,
            },
            new CmnXunitTestData<CoMgrInfDbUpdateReqMdl, CoMgrInfDbUpdateRspMdl>()
            {
                Description = "成功，更新 PlayerID = 1",
                InputObj = new CoMgrInfDbUpdateReqMdl()
                {
                    PlayerID = 1,
                    Password = "Password11",
                    Phone = "886911111111",
                    Nickname = "Nickname11",
                    LastLoginIP = "11.11.11.11",
                    LastLoginTime = new DateTimeOffset(2024, 11, 11, 11, 11, 11, 11, TimeSpan.Zero)
                },
                ExpectedObj = new CoMgrInfDbUpdateRspMdl()
                {
                    Password = "Password11",
                    Phone = "886911111111",
                    Nickname = "Nickname11",
                    LastLoginIP = "11.11.11.11",
                    LastLoginTime = new DateTimeOffset(2024, 11, 11, 11, 11, 11, 11, TimeSpan.Zero),
                    UpdateTime = DateTimeOffset.UtcNow,
                },
            },
        ]);
    }
}
