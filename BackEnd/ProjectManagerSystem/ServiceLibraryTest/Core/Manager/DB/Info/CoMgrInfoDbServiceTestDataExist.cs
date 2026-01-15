using CommonLibrary.CmnXunit;
using ServiceLibrary.Core.Player.DB.Info.Format;

namespace ServiceLibraryTest.Core.Manager.DB.Info;

/// <summary>核心-管理者-資訊-資料庫服務-測試資料-是否存在</summary>
public class CoMgrInfoDbServiceTestDataExist : TheoryData<CmnXunitTestData<CoMgrInfDbExistReqMdl, CoMgrInfDbExistRspMdl>>
{
    /// <summary>建構式</summary>
    public CoMgrInfoDbServiceTestDataExist()
    {
        // 加入測試資料
        this.AddRange(
        [
            new CmnXunitTestData<CoMgrInfDbExistReqMdl, CoMgrInfDbExistRspMdl>()
            {
                Description = "失敗，不存在的ID",
                InputObj = new CoMgrInfDbExistReqMdl()
                {
                    PlayerID = 99
                },
                ExpectedObj = new CoMgrInfDbExistRspMdl
                {
                    IsExist = false
                },
            },
            new CmnXunitTestData<CoMgrInfDbExistReqMdl, CoMgrInfDbExistRspMdl>()
            {
                Description = "成功，PlayerID =  1",
                InputObj = new CoMgrInfDbExistReqMdl()
                {
                     PlayerID = 1
                },
                ExpectedObj = new CoMgrInfDbExistRspMdl()
                {
                    IsExist = true
                },
            },
        ]);
    }
}
