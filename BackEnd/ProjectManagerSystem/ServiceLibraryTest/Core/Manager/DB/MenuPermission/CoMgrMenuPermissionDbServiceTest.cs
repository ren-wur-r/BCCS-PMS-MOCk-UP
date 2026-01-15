using System.Threading.Tasks;
using CommonLibrary.CmnXunit;
using DataModelLibrary.EfCore.ProjectManagerMain;
using ServiceLibrary.Core.Manager.DB.MenuPermission.Format;
using ServiceLibrary.Core.Manager.DB.MenuPermission.Service;
using ServiceLibraryTest.Model;

namespace ServiceLibraryTest.Core.Manager.DB.MenuPermission;
public class CoMgrMenuPermissionDbServiceTest : TestBase
{
    /// <summary>核心-管理者-目錄權限-資料庫服務</summary>
    private readonly ICoMgrMenuPermissionDbService _coMgrMenuPermissionDb;


    /// <summary>建構式</summary>
    public CoMgrMenuPermissionDbServiceTest(
        ProjectManagerMainContext mainContext,
        ICoMgrMenuPermissionDbService coMgrMenuPermissionDb)
        : base(
            mainContext)
    {
        this._coMgrMenuPermissionDb = coMgrMenuPermissionDb;

        base.ResetAllDb();
    }

    ///// <summary>核心-管理者-目錄權限-資料庫-新增多筆</summary>
    //[Theory]
    //[ClassData(typeof(CoMgrMenuPermissionDbServiceTestDataAddMany))]
    //public async Task AddAsync(CmnXunitTestData<CoMgrMpDbAddManyReqMdl, CoMgrMpDbAddManyRspMdl> testData)
    //{

    //}

    ///// <summary>核心-管理者-目錄權限-資料庫-移除多筆</summary>
    //[Theory]
    //[ClassData(typeof(CoMgrMenuPermissionDbServiceTestDataRemoveMany))]
    //public async Task RemoveManyAsync(CmnXunitTestData<CoMgrMpDbRemoveManyReqMdl, CoMgrMpDbRemoveManyRspMdl> testData)
    //{

    //}

    /// <summary>核心-管理者-目錄權限-資料庫-取得</summary>
    [Theory]
    [ClassData(typeof(CoMgrMenuPermissionDbServiceTestDataGet))]
    public async Task GetAsync(CmnXunitTestData<CoMgrMpDbGetReqMdl, CoMgrMpDbGetRspMdl> testData)
    {
        var rspObj = await this._coMgrMenuPermissionDb.GetAsync(testData.InputObj);

        Assert.Equivalent(testData.ExpectedObj, rspObj, true);
    }

    ///// <summary>核心-管理者-目錄權限-資料庫-取得多筆</summary>
    //[Theory]
    //[ClassData(typeof(CoMgrMenuPermissionDbServiceTestDataGetMany))]
    //public async Task GetManyAsync(CmnXunitTestData<CoMgrMpDbGetManyReqMdl, CoMgrMpDbGetManyRspMdl> testData)
    //{

    //}


}
