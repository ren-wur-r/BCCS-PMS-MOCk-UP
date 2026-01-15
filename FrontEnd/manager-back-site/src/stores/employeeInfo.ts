import { defineStore } from "pinia";
import { computed, ref } from "vue";
import { DbAtomPermissionKindEnum } from "@/constants/DbAtomPermissionKindEnum";
import { DbAtomMenuEnum } from "@/constants/DbAtomMenuEnum";
import {
  MbsBscHttpGetEmployeeRspMdl,
  MbsBscHttpGetEmployeeRspItemMdl,
} from "@/services/pms-http/basic/basicHttpFormat";

/** 權限操作類型 */
export type PermissionAction = "view" | "create" | "edit" | "delete";

/** 權限模型 */
interface MenuPermission {
  view: DbAtomPermissionKindEnum;
  create: DbAtomPermissionKindEnum;
  edit: DbAtomPermissionKindEnum;
  delete: DbAtomPermissionKindEnum;
}

export const useEmployeeInfoStore = defineStore("employeeInfo", () => {
  const employeeName = ref<string>("");
  const managerRoleID = ref<number>(0);
  const managerRoleName = ref<string>("");
  const managerRegionID = ref<number>(0);
  const managerRegionName = ref<string>("");
  const managerDepartmentID = ref<number>(0);
  const managerDepartmentName = ref<string>("");
  const devRoleName = ref<string>("");

  const effectiveRoleName = computed(
    () => devRoleName.value || managerRoleName.value || ""
  );

  const getAllMenuEnums = () =>
    Object.values(DbAtomMenuEnum).filter(
      (value) => typeof value === "number" && value !== DbAtomMenuEnum.Undefined
    ) as DbAtomMenuEnum[];

  const roleMenuAccess: Record<string, DbAtomMenuEnum[]> = {
    Admin: getAllMenuEnums(),
    總經理: [],
    各處處長: [
      DbAtomMenuEnum.CrmPipeline,
      DbAtomMenuEnum.WorkProject,
      DbAtomMenuEnum.WorkJob,
      DbAtomMenuEnum.CrmPhone,
      DbAtomMenuEnum.CrmActivity,
    ],
    各部門經理: [
      DbAtomMenuEnum.WorkProject,
      DbAtomMenuEnum.WorkJob,
      DbAtomMenuEnum.SystemContacter,
      DbAtomMenuEnum.SystemCompany,
      DbAtomMenuEnum.SystemProduct,
      DbAtomMenuEnum.SystemEmployee,
      DbAtomMenuEnum.SystemRole,
      DbAtomMenuEnum.SystemDepartment,
      DbAtomMenuEnum.SystemRegion,
    ],
    工程部中部: [
      DbAtomMenuEnum.WorkProject,
      DbAtomMenuEnum.WorkJob,
      DbAtomMenuEnum.SystemContacter,
      DbAtomMenuEnum.SystemCompany,
      DbAtomMenuEnum.SystemProduct,
      DbAtomMenuEnum.SystemEmployee,
      DbAtomMenuEnum.SystemRole,
      DbAtomMenuEnum.SystemDepartment,
      DbAtomMenuEnum.SystemRegion,
    ],
    顧問中部: [
      DbAtomMenuEnum.WorkProject,
      DbAtomMenuEnum.WorkJob,
      DbAtomMenuEnum.SystemContacter,
      DbAtomMenuEnum.SystemCompany,
      DbAtomMenuEnum.SystemProduct,
      DbAtomMenuEnum.SystemEmployee,
      DbAtomMenuEnum.SystemRole,
      DbAtomMenuEnum.SystemDepartment,
      DbAtomMenuEnum.SystemRegion,
    ],
    活動人員: [DbAtomMenuEnum.CrmActivity],
    電銷人員: [DbAtomMenuEnum.CrmPhone],
    業務: [
      DbAtomMenuEnum.CrmPipeline,
      DbAtomMenuEnum.WorkProject,
      DbAtomMenuEnum.WorkJob,
      DbAtomMenuEnum.SystemContacter,
      DbAtomMenuEnum.SystemCompany,
      DbAtomMenuEnum.SystemProduct,
      DbAtomMenuEnum.SystemEmployee,
      DbAtomMenuEnum.SystemRole,
      DbAtomMenuEnum.SystemDepartment,
      DbAtomMenuEnum.SystemRegion,
    ],
    顧問: [DbAtomMenuEnum.WorkProject, DbAtomMenuEnum.WorkJob],
    產品經理: [DbAtomMenuEnum.WorkProject, DbAtomMenuEnum.WorkJob],
    工程師: [DbAtomMenuEnum.WorkProject, DbAtomMenuEnum.WorkJob],
    資深顧問: [DbAtomMenuEnum.WorkProject, DbAtomMenuEnum.WorkJob],
  };

  const isMenuAllowedByRole = (menu: DbAtomMenuEnum): boolean => {
    const roleName = effectiveRoleName.value;
    if (!roleName) return true;
    if (roleName === "Admin" || roleName === "總經理") return true;
    const allowList = roleMenuAccess[roleName];
    if (!allowList) return true;
    return allowList.includes(menu);
  };

  const employeePermissionList = ref<Map<DbAtomMenuEnum, MenuPermission>>(
    new Map()
  );

  /** 設定員工資訊 */
  const setEmployeeInfo = (data: MbsBscHttpGetEmployeeRspMdl) => {
    employeeName.value = data.employeeName;
    managerRoleID.value = data.managerRoleID;
    managerRoleName.value = data.managerRoleName;
    managerRegionID.value = data.managerRegionID;
    managerRegionName.value = data.managerRegionName;
    managerDepartmentID.value = data.managerDepartmentID;
    managerDepartmentName.value = data.managerDepartmentName;

    const map = new Map<DbAtomMenuEnum, MenuPermission>();
    data.managerBackSiteMenuPermissionList?.forEach(
      (item: MbsBscHttpGetEmployeeRspItemMdl) => {
        map.set(item.atomMenu, {
          view: item.atomPermissionKindIdView,
          create: item.atomPermissionKindIdCreate,
          edit: item.atomPermissionKindIdEdit,
          delete: item.atomPermissionKindIdDelete,
        });
      }
    );
    employeePermissionList.value = map;

    // 存進 localStorage
    localStorage.setItem(
      "employeeInfo",
      JSON.stringify({
        employeeName: employeeName.value,
        managerRoleID: managerRoleID.value,
        managerRoleName: managerRoleName.value,
        managerRegionID: managerRegionID.value,
        managerRegionName: managerRegionName.value,
        managerDepartmentID: managerDepartmentID.value,
        managerDepartmentName: managerDepartmentName.value,
        employeePermissionList: Array.from(
          employeePermissionList.value.entries()
        ), // 轉成陣列
      })
    );
  };

  /** 從 localStorage 讀取員工資訊 */
  const getEmployeeInfo = () => {
    const stored = localStorage.getItem("employeeInfo");
    if (stored) {
      try {
        const parsed = JSON.parse(stored);
        employeeName.value = parsed.employeeName || "";
        managerRoleID.value = parsed.managerRoleID || 0;
        managerRoleName.value = parsed.managerRoleName || "";
        managerRegionID.value = parsed.managerRegionID || 0;
        managerRegionName.value = parsed.managerRegionName || "";
        managerDepartmentID.value = parsed.managerDepartmentID || 0;
        managerDepartmentName.value = parsed.managerDepartmentName || "";

        employeePermissionList.value = new Map(parsed.employeePermissionList);
        devRoleName.value = parsed.devRoleName || "";
      } catch {
        removeEmployeeInfo();
      }
    }
  };

  /** 清除員工資訊 */
  const removeEmployeeInfo = () => {
    employeeName.value = "";
    managerRoleID.value = 0;
    managerRoleName.value = "";
    managerRegionID.value = 0;
    managerRegionName.value = "";
    managerDepartmentID.value = 0;
    managerDepartmentName.value = "";
    employeePermissionList.value.clear();
    devRoleName.value = "";
    localStorage.removeItem("employeeInfo");
    localStorage.removeItem("devRoleName");
  };

  /** 判斷有沒有權限(無權限/自身/所有人) */
  const hasPermission = (
    menu: DbAtomMenuEnum,
    action: PermissionAction
  ): boolean => {
    if (!isMenuAllowedByRole(menu)) return false;
    const roleName = effectiveRoleName.value;
    if (roleName === "總經理") return action === "view";
    if (roleName === "Admin") return true;
    return true;
  };

  /** 判斷有沒有權限(無權限/所有人) */
  const hasEveryonePermission = (
    menu: DbAtomMenuEnum,
    action: PermissionAction
  ): boolean => {
    return hasPermission(menu, action);
  };

  const setDevRoleName = (roleName: string) => {
    devRoleName.value = roleName;
    localStorage.setItem("devRoleName", roleName);
  };

  const clearDevRoleName = () => {
    devRoleName.value = "";
    localStorage.removeItem("devRoleName");
  };

  // 初始化時讀取員工資訊
  getEmployeeInfo();
  const storedDevRole = localStorage.getItem("devRoleName");
  if (storedDevRole) {
    devRoleName.value = storedDevRole;
  }

  return {
    employeeName,
    managerRoleID,
    managerRoleName,
    managerRegionID,
    managerRegionName,
    managerDepartmentID,
    managerDepartmentName,
    devRoleName,
    effectiveRoleName,
    employeePermissionList,
    setEmployeeInfo,
    getEmployeeInfo,
    removeEmployeeInfo,
    hasPermission,
    hasEveryonePermission,
    setDevRoleName,
    clearDevRoleName,
  };
});
