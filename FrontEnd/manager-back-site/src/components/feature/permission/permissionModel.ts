import { DbAtomMenuEnum } from "@/constants/DbAtomMenuEnum";
import { DbAtomPermissionKindEnum } from "@/constants/DbAtomPermissionKindEnum";

/** 權限項目模型 - 用於組件間傳遞權限資料 */
export interface PermissionItemMdl {
  atomMenu: DbAtomMenuEnum;
  managerRegionID: number;
  atomPermissionKindIdView: DbAtomPermissionKindEnum;
  atomPermissionKindIdCreate: DbAtomPermissionKindEnum;
  atomPermissionKindIdEdit: DbAtomPermissionKindEnum;
  atomPermissionKindIdDelete: DbAtomPermissionKindEnum;
}

// ----------------------------------------------------------------------------
/** 權限表格模型 - 定義權限表格的結構 */
export interface PermissionListMdl {
  groupTitle: string;
  groupKey: string;
  menuItemList: {
    menuName: string;
    menuEnum: DbAtomMenuEnum;
    hasRegion: boolean;
    permissionView: DbAtomPermissionKindEnum[];
    permissionCreate: DbAtomPermissionKindEnum[];
    permissionEdit: DbAtomPermissionKindEnum[];
    permissionDelete: DbAtomPermissionKindEnum[];
  }[];
}

/** 共用權限表格設定 */
export const permissionTableConfig: PermissionListMdl[] = [
  {
    groupTitle: "ERP",
    groupKey: "erp",
    menuItemList: [
      {
        menuName: "發票結案",
        menuEnum: DbAtomMenuEnum.ErpBill,
        hasRegion: false,
        permissionView: [DbAtomPermissionKindEnum.Denied, DbAtomPermissionKindEnum.Everyone],
        permissionCreate: [DbAtomPermissionKindEnum.Denied, DbAtomPermissionKindEnum.Everyone],
        permissionEdit: [DbAtomPermissionKindEnum.Denied, DbAtomPermissionKindEnum.Everyone],
        permissionDelete: [DbAtomPermissionKindEnum.Denied, DbAtomPermissionKindEnum.Everyone],
      },
    ],
  },
  {
    groupTitle: "工作",
    groupKey: "work",
    menuItemList: [
      {
        menuName: "專案管理",
        menuEnum: DbAtomMenuEnum.WorkProject,
        hasRegion: false,
        permissionView: [DbAtomPermissionKindEnum.Everyone],
        permissionCreate: [DbAtomPermissionKindEnum.Everyone],
        permissionEdit: [DbAtomPermissionKindEnum.Everyone],
        permissionDelete: [DbAtomPermissionKindEnum.Everyone],
      },
      {
        menuName: "工項管理",
        menuEnum: DbAtomMenuEnum.WorkJob,
        hasRegion: false,
        permissionView: [DbAtomPermissionKindEnum.Self],
        permissionCreate: [DbAtomPermissionKindEnum.Self],
        permissionEdit: [DbAtomPermissionKindEnum.Self],
        permissionDelete: [DbAtomPermissionKindEnum.Self],
      },
    ],
  },
  {
    groupTitle: "CRM",
    groupKey: "crm",
    menuItemList: [
      {
        menuName: "商機管理",
        menuEnum: DbAtomMenuEnum.CrmPipeline,
        hasRegion: true,
        permissionView: [
          DbAtomPermissionKindEnum.Denied,
          DbAtomPermissionKindEnum.Self,
          DbAtomPermissionKindEnum.Everyone,
        ],
        permissionCreate: [
          DbAtomPermissionKindEnum.Denied,
          DbAtomPermissionKindEnum.Self,
          DbAtomPermissionKindEnum.Everyone,
        ],
        permissionEdit: [
          DbAtomPermissionKindEnum.Denied,
          DbAtomPermissionKindEnum.Self,
          DbAtomPermissionKindEnum.Everyone,
        ],
        permissionDelete: [
          DbAtomPermissionKindEnum.Denied,
          DbAtomPermissionKindEnum.Self,
          DbAtomPermissionKindEnum.Everyone,
        ],
      },
      {
        menuName: "Booking",
        menuEnum: DbAtomMenuEnum.CrmBooking,
        hasRegion: false,
        permissionView: [DbAtomPermissionKindEnum.Denied, DbAtomPermissionKindEnum.Everyone],
        permissionCreate: [DbAtomPermissionKindEnum.Denied],
        permissionEdit: [DbAtomPermissionKindEnum.Denied, DbAtomPermissionKindEnum.Everyone],
        permissionDelete: [DbAtomPermissionKindEnum.Denied],
      },
      {
        menuName: "電銷管理",
        menuEnum: DbAtomMenuEnum.CrmPhone,
        hasRegion: false,
        permissionView: [DbAtomPermissionKindEnum.Denied, DbAtomPermissionKindEnum.Everyone],
        permissionCreate: [DbAtomPermissionKindEnum.Denied, DbAtomPermissionKindEnum.Everyone],
        permissionEdit: [DbAtomPermissionKindEnum.Denied, DbAtomPermissionKindEnum.Everyone],
        permissionDelete: [DbAtomPermissionKindEnum.Denied, DbAtomPermissionKindEnum.Everyone],
      },
      {
        menuName: "活動管理",
        menuEnum: DbAtomMenuEnum.CrmActivity,
        hasRegion: false,
        permissionView: [DbAtomPermissionKindEnum.Denied, DbAtomPermissionKindEnum.Everyone],
        permissionCreate: [DbAtomPermissionKindEnum.Denied, DbAtomPermissionKindEnum.Everyone],
        permissionEdit: [DbAtomPermissionKindEnum.Denied, DbAtomPermissionKindEnum.Everyone],
        permissionDelete: [DbAtomPermissionKindEnum.Denied, DbAtomPermissionKindEnum.Everyone],
      },
    ],
  },
  {
    groupTitle: "報表",
    groupKey: "report",
    menuItemList: [
      {
        menuName: "產品同比",
        menuEnum: DbAtomMenuEnum.ReportProductPeriod,
        hasRegion: false,
        permissionView: [
          DbAtomPermissionKindEnum.Denied,
          DbAtomPermissionKindEnum.Self,
          DbAtomPermissionKindEnum.Everyone,
        ],
        permissionCreate: [DbAtomPermissionKindEnum.Denied],
        permissionEdit: [DbAtomPermissionKindEnum.Denied],
        permissionDelete: [DbAtomPermissionKindEnum.Denied],
      },
      {
        menuName: "產品趨勢",
        menuEnum: DbAtomMenuEnum.ReportProductTrend,
        hasRegion: false,
        permissionView: [
          DbAtomPermissionKindEnum.Denied,
          DbAtomPermissionKindEnum.Self,
          DbAtomPermissionKindEnum.Everyone,
        ],
        permissionCreate: [DbAtomPermissionKindEnum.Denied],
        permissionEdit: [DbAtomPermissionKindEnum.Denied],
        permissionDelete: [DbAtomPermissionKindEnum.Denied],
      },
      {
        menuName: "產品分析",
        menuEnum: DbAtomMenuEnum.ReportProductAnalysis,
        hasRegion: false,
        permissionView: [
          DbAtomPermissionKindEnum.Denied,
          DbAtomPermissionKindEnum.Self,
          DbAtomPermissionKindEnum.Everyone,
        ],
        permissionCreate: [DbAtomPermissionKindEnum.Denied],
        permissionEdit: [DbAtomPermissionKindEnum.Denied],
        permissionDelete: [DbAtomPermissionKindEnum.Denied],
      },
      {
        menuName: "窗口分析",
        menuEnum: DbAtomMenuEnum.ReportContacterAnalysis,
        hasRegion: false,
        permissionView: [
          DbAtomPermissionKindEnum.Denied,
          DbAtomPermissionKindEnum.Self,
          DbAtomPermissionKindEnum.Everyone,
        ],
        permissionCreate: [DbAtomPermissionKindEnum.Denied],
        permissionEdit: [DbAtomPermissionKindEnum.Denied],
        permissionDelete: [DbAtomPermissionKindEnum.Denied],
      },
      {
        menuName: "客戶分析",
        menuEnum: DbAtomMenuEnum.ReportCompanyAnalysis,
        hasRegion: false,
        permissionView: [
          DbAtomPermissionKindEnum.Denied,
          DbAtomPermissionKindEnum.Self,
          DbAtomPermissionKindEnum.Everyone,
        ],
        permissionCreate: [DbAtomPermissionKindEnum.Denied],
        permissionEdit: [DbAtomPermissionKindEnum.Denied],
        permissionDelete: [DbAtomPermissionKindEnum.Denied],
      },
      {
        menuName: "活動分析",
        menuEnum: DbAtomMenuEnum.ReportActivityAnalysis,
        hasRegion: false,
        permissionView: [
          DbAtomPermissionKindEnum.Denied,
          DbAtomPermissionKindEnum.Self,
          DbAtomPermissionKindEnum.Everyone,
        ],
        permissionCreate: [DbAtomPermissionKindEnum.Denied],
        permissionEdit: [DbAtomPermissionKindEnum.Denied],
        permissionDelete: [DbAtomPermissionKindEnum.Denied],
      },
    ],
  },
  {
    groupTitle: "系統設定",
    groupKey: "system",
    menuItemList: [
      {
        menuName: "窗口",
        menuEnum: DbAtomMenuEnum.SystemContacter,
        hasRegion: false,
        permissionView: [DbAtomPermissionKindEnum.Denied, DbAtomPermissionKindEnum.Everyone],
        permissionCreate: [DbAtomPermissionKindEnum.Denied, DbAtomPermissionKindEnum.Everyone],
        permissionEdit: [DbAtomPermissionKindEnum.Denied, DbAtomPermissionKindEnum.Everyone],
        permissionDelete: [DbAtomPermissionKindEnum.Denied, DbAtomPermissionKindEnum.Everyone],
      },
      {
        menuName: "客戶",
        menuEnum: DbAtomMenuEnum.SystemCompany,
        hasRegion: false,
        permissionView: [DbAtomPermissionKindEnum.Denied, DbAtomPermissionKindEnum.Everyone],
        permissionCreate: [DbAtomPermissionKindEnum.Denied, DbAtomPermissionKindEnum.Everyone],
        permissionEdit: [DbAtomPermissionKindEnum.Denied, DbAtomPermissionKindEnum.Everyone],
        permissionDelete: [DbAtomPermissionKindEnum.Denied, DbAtomPermissionKindEnum.Everyone],
      },
      {
        menuName: "產品",
        menuEnum: DbAtomMenuEnum.SystemProduct,
        hasRegion: false,
        permissionView: [DbAtomPermissionKindEnum.Denied, DbAtomPermissionKindEnum.Everyone],
        permissionCreate: [DbAtomPermissionKindEnum.Denied, DbAtomPermissionKindEnum.Everyone],
        permissionEdit: [DbAtomPermissionKindEnum.Denied, DbAtomPermissionKindEnum.Everyone],
        permissionDelete: [DbAtomPermissionKindEnum.Denied],
      },
      {
        menuName: "員工",
        menuEnum: DbAtomMenuEnum.SystemEmployee,
        hasRegion: false,
        permissionView: [DbAtomPermissionKindEnum.Denied, DbAtomPermissionKindEnum.Everyone],
        permissionCreate: [DbAtomPermissionKindEnum.Denied, DbAtomPermissionKindEnum.Everyone],
        permissionEdit: [DbAtomPermissionKindEnum.Denied, DbAtomPermissionKindEnum.Everyone],
        permissionDelete: [DbAtomPermissionKindEnum.Denied],
      },
      {
        menuName: "角色",
        menuEnum: DbAtomMenuEnum.SystemRole,
        hasRegion: false,
        permissionView: [DbAtomPermissionKindEnum.Denied, DbAtomPermissionKindEnum.Everyone],
        permissionCreate: [DbAtomPermissionKindEnum.Denied, DbAtomPermissionKindEnum.Everyone],
        permissionEdit: [DbAtomPermissionKindEnum.Denied, DbAtomPermissionKindEnum.Everyone],
        permissionDelete: [DbAtomPermissionKindEnum.Denied],
      },
      {
        menuName: "部門",
        menuEnum: DbAtomMenuEnum.SystemDepartment,
        hasRegion: false,
        permissionView: [DbAtomPermissionKindEnum.Denied, DbAtomPermissionKindEnum.Everyone],
        permissionCreate: [DbAtomPermissionKindEnum.Denied, DbAtomPermissionKindEnum.Everyone],
        permissionEdit: [DbAtomPermissionKindEnum.Denied, DbAtomPermissionKindEnum.Everyone],
        permissionDelete: [DbAtomPermissionKindEnum.Denied],
      },
      {
        menuName: "地區",
        menuEnum: DbAtomMenuEnum.SystemRegion,
        hasRegion: false,
        permissionView: [DbAtomPermissionKindEnum.Denied, DbAtomPermissionKindEnum.Everyone],
        permissionCreate: [DbAtomPermissionKindEnum.Denied, DbAtomPermissionKindEnum.Everyone],
        permissionEdit: [DbAtomPermissionKindEnum.Denied, DbAtomPermissionKindEnum.Everyone],
        permissionDelete: [DbAtomPermissionKindEnum.Denied],
      },
    ],
  },
];
