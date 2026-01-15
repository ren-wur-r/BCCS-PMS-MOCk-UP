using CommonLibrary.CmnXunit;
using ServiceLibrary.Core.Player.DB.Info.Format;

namespace ServiceLibraryTest.Core.Manager.DB.Info;

/// <summary>核心-管理者-資訊-資料庫服務-測試資料-移除</summary>
public class CoMgrInfoDbServiceTestDataRemove : TheoryData<CmnXunitTestData<CoMgrInfDbRemoveReqMdl, CoMgrInfDbRemoveRspMdl>>
{
    /// <summary>建構式</summary>
    public CoMgrInfoDbServiceTestDataRemove()
    {
        // 加入測試資料
        this.AddRange(
        [
            new CmnXunitTestData<CoMgrInfDbRemoveReqMdl, CoMgrInfDbRemoveRspMdl>()
            {
                Description = "成功，移除不存在 ID 99",
                InputObj = new CoMgrInfDbRemoveReqMdl()
                {
                    PlayerID = 99,
                },
                ExpectedObj = new CoMgrInfDbRemoveRspMdl()
                {
                    IsSuccess = true,
                },
            },
            new CmnXunitTestData<CoMgrInfDbRemoveReqMdl, CoMgrInfDbRemoveRspMdl>()
            {
                Description = "成功，移除 ID 1",
                InputObj = new CoMgrInfDbRemoveReqMdl()
                {
                    PlayerID = 1,
                },
                ExpectedObj = new CoMgrInfDbRemoveRspMdl()
                {
                    IsSuccess = true,
                },
            },
        ]);
    }
}
