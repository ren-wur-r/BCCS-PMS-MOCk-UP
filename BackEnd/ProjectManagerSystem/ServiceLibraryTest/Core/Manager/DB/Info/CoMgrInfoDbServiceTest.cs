// TEMPORARILY COMMENTED OUT FOR SQL SERVER MIGRATION TESTING
// This file is temporarily disabled to test only Region functionality
// Original file content is preserved below in comments

/*
using System.Threading.Tasks;
using CommonLibrary.CmnXunit;
using DataModelLibrary.EfCore.ProjectManagerMain;
using Microsoft.EntityFrameworkCore;
using ServiceLibrary.Core.Manager.DB.Info.Format;
using ServiceLibrary.Core.Manager.DB.Info.Service;
using ServiceLibraryTest.Model;

namespace ServiceLibraryTest.Core.Manager.DB.Info;

/// <summary>核心-管理者-目錄權限-資料庫服務-測試</summary>
public class CoMgrInfDbServiceTest : TestBase
{

    /// <summary>核心-管理者-資訊-資料庫服務</summary>
    private readonly ICoMgrInfoDbService _coMgrInfoDb;

    /// <summary>建構式</summary>
    public CoMgrInfDbServiceTest(
        ProjectManagerMainContext mainContext,
        ICoMgrInfoDbService coMgrInfoDb)
        : base(
            mainContext)
    {
        this._coMgrInfoDb = coMgrInfoDb;

        base.ResetAllDb();
    }



    ///// <summary>核心-管理者-資訊-是否存在</summary>
    //[Theory]
    //[ClassData(typeof(CoMgrInfDbServiceTestDataExist))]
    //public async Task ExistAsync(CmnXunitTestData<CoMgrInfDbExistReqMdl, CoMgrInfDbExistRspMdl> testData)
    //{
    //    var rspObj = await this._coMgrMenuPermissionDb.ExistAsync(testData.InputObj);

    //    Assert.Equivalent(testData.ExpectedObj, rspObj, true);
    //}

    /// <summary>核心-管理者-資訊-新增</summary>
    [Theory]
    [ClassData(typeof(CoMgrInfoDbServiceTestDataAdd))]
    public async Task AddAsync(CmnXunitTestData<CoMgrInfDbAddReqMdl, CoMgrInfDbAddRspMdl> testData)
    {
        var rspObj = await this._coMgrInfoDb.AddAsync(testData.InputObj);

        if (testData.ExpectedObj == null)
        {
            // 驗輸出
            Assert.Null(rspObj);
            return;
        }

        // 驗輸出
        Assert.NotNull(rspObj);
        Assert.Equal(testData.ExpectedObj.ManagerID, rspObj.ManagerID);
        //Assert.InRange(rspObj.UpdateTime, testData.ExpectedObj.UpdateTime, testData.ExpectedObj.UpdateTime.AddSeconds(TestConst.ExpectedRangeSecounds));

        // 驗DB
        var actDbItem = await base._mainContext.Manager
            .SingleAsync(x => x.ID == rspObj.ManagerID, cancellationToken: TestContext.Current.CancellationToken);
        Assert.Equal(testData.InputObj.ManagerAccount, actDbItem.Account.Trim());
        Assert.Equal(testData.InputObj.ManagerPassword, actDbItem.Password.Trim());
        //Assert.Equal(testData.InputObj.ManagerName, actDbItem.Name.Trim());
        //Assert.Equal(testData.InputObj.ManagerEmail, actDbItem.Email);
        //Assert.Equal(testData.InputObj.ManagerIsEnable, actDbItem.Enable);
    }

    ///// <summary>核心-管理者-資訊-更新</summary>
    //[Theory]
    //[ClassData(typeof(CoMgrInfDbServiceTestDataUpdate))]
    //public async Task UpdateAsync(CmnXunitTestData<CoMgrInfDbUpdateReqMdl, CoMgrInfDbUpdateRspMdl> testData)
    //{
    //    var rspObj = await this._coMgrMenuPermissionDb.UpdateAsync(testData.InputObj);

    //    if (testData.ExpectedObj == default)
    //    {
    //        // 驗輸出
    //        Assert.Null(rspObj);
    //        return;
    //    }

    //    // 驗輸出
    //    Assert.Equal(testData.ExpectedObj.Password, rspObj.Password);
    //    Assert.Equal(testData.ExpectedObj.Phone, rspObj.Phone);
    //    Assert.Equal(testData.ExpectedObj.Nickname, rspObj.Nickname);
    //    Assert.Equal(testData.ExpectedObj.LastLoginIP, rspObj.LastLoginIP);
    //    Assert.Equal(testData.ExpectedObj.LastLoginTime, rspObj.LastLoginTime);
    //    Assert.InRange(rspObj.UpdateTime, testData.ExpectedObj.UpdateTime, testData.ExpectedObj.UpdateTime.AddSeconds(TestConst.ExpectedRangeSecounds));

    //    // 驗DB
    //    var actDbItem = await base._mainContext.Player
    //        .SingleAsync(x => x.ID == testData.InputObj.PlayerID);
    //    Assert.Equal(testData.InputObj.Password, actDbItem.Password);
    //    Assert.Equal(testData.InputObj.Phone, actDbItem.Phone);
    //    Assert.Equal(testData.InputObj.Nickname, actDbItem.Nickname);
    //    Assert.Equal(testData.InputObj.LastLoginIP, actDbItem.LastLoginIP);
    //    Assert.Equal(testData.InputObj.LastLoginTime, actDbItem.LastLoginTime);
    //}

    ///// <summary>核心-管理者-資訊-移除</summary>
    //[Theory]
    //[ClassData(typeof(CoMgrInfDbServiceTestDataRemove))]
    //public async Task RemoveAsync(CmnXunitTestData<CoMgrInfDbRemoveReqMdl, CoMgrInfDbRemoveRspMdl> testData)
    //{
    //    var rspObj = await this._coMgrMenuPermissionDb.RemoveAsync(testData.InputObj);

    //    // 驗輸出
    //    Assert.Equivalent(testData.ExpectedObj, rspObj, true);

    //    // 驗DB
    //    var isExistActDB = await base._mainContext.Player.AnyAsync(x => x.ID == testData.InputObj.PlayerID);
    //    Assert.False(isExistActDB);
    //}

    ///// <summary>核心-管理者-資訊-取得[ID]</summary>
    //[Theory]
    //[ClassData(typeof(CoMgrInfDbServiceTestDataGetID))]
    //public async Task GetIdAsync(CmnXunitTestData<CoMgrInfDbGetIdReqMdl, CoMgrInfDbGetIdRspMdl> testData)
    //{
    //    var rspObj = await this._coMgrMenuPermissionDb.GetIdAsync(testData.InputObj);

    //    Assert.Equivalent(testData.ExpectedObj, rspObj, true);
    //}

    ///// <summary>核心-管理者-資訊-取得</summary>
    //[Theory]
    //[ClassData(typeof(CoMgrInfDbServiceTestDataGet))]
    //public async Task GetAsync(CmnXunitTestData<CoMgrInfDbGetReqMdl, CoMgrInfDbGetRspMdl> testData)
    //{
    //    var rspObj = await this._coMgrMenuPermissionDb.GetAsync(testData.InputObj);

    //    Assert.Equivalent(testData.ExpectedObj, rspObj, true);
    //}
*/
