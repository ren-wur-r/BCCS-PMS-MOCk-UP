using CommonLibrary.CmnXunit;
using DataModelLibrary.Database.AtomMenu;
using DataModelLibrary.Database.ManagerRolePermission;
using ServiceLibrary.Core.Manager.DB.MenuPermission.Format;

namespace ServiceLibraryTest.Core.Manager.DB.MenuPermission;
public class CoMgrMenuPermissionDbServiceTestDataGet : TheoryData<CmnXunitTestData<CoMgrMpDbGetReqMdl, CoMgrMpDbGetRspMdl>>
{
    /// <summary>建構式</summary>
    public CoMgrMenuPermissionDbServiceTestDataGet()
    {
        // 加入測試資料
        this.AddRange(
        [
             //new CmnXunitTestData<CoMgrMpDbGetReqMdl, CoMgrMpDbGetRspMdl>()
             //{
             //   Description = "失敗，不存在的ManagerID",
             //   InputObj = new CoMgrMpDbGetReqMdl()
             //   {
             //       ManagerID = 999,
             //       AtomMenu = DbAtomMenuEnum.BusinessOpportunity
             //   },
             //   ExpectedObj = null,
             //},
             //new CmnXunitTestData<CoMgrMpDbGetReqMdl, CoMgrMpDbGetRspMdl>()
             //{
             //   Description = "失敗，不存在的Menu",
             //   InputObj = new CoMgrMpDbGetReqMdl()
             //   {
             //       ManagerID = 1,
             //       AtomMenu = DbAtomMenuEnum.Undefined
             //   },
             //   ExpectedObj = null,
             //},
             new CmnXunitTestData<CoMgrMpDbGetReqMdl, CoMgrMpDbGetRspMdl>()
             {
                Description = "成功",
                InputObj = new CoMgrMpDbGetReqMdl()
                {
                    ManagerID = 1,
                    AtomMenu = DbAtomMenuEnum.BusinessOpportunity
                },
                ExpectedObj = new CoMgrMpDbGetRspMdl()
                {
                    ManagerMenuPermissionKind =
                        DbManagerRolePermissionKindEnum.View
                        | DbManagerRolePermissionKindEnum.Add
                        | DbManagerRolePermissionKindEnum.Edit
                        | DbManagerRolePermissionKindEnum.Delete,
                },
             },
        ]);
    }
}
