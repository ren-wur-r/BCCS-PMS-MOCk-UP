import { permissionTableConfig } from "@/components/feature/permission/permissionModel";
import { DbManagerRegionConst } from "@/constants/DbManagerRegionConst";
import { DbAtomPermissionKindEnum } from "@/constants/DbAtomPermissionKindEnum";
import type { PermissionItemMdl } from "@/components/feature/permission/permissionModel";

/**
 * 初始化權限列表
 * 根據 permissionTableConfig 中定義的第一個權限選項來設定預設值
 * @returns 預設的 PermissionItemMdl 陣列
 */
export const initializePermissionList = (): PermissionItemMdl[] => {
  return permissionTableConfig.flatMap((group) =>
    group.menuItemList.map((item) => ({
      atomMenu: item.menuEnum,
      managerRegionID: DbManagerRegionConst.Denied.ID,
      // 使用配置中的第一個權限選項作為預設值
      atomPermissionKindIdView: item.permissionView[0] || DbAtomPermissionKindEnum.Denied,
      atomPermissionKindIdCreate: item.permissionCreate[0] || DbAtomPermissionKindEnum.Denied,
      atomPermissionKindIdEdit: item.permissionEdit[0] || DbAtomPermissionKindEnum.Denied,
      atomPermissionKindIdDelete: item.permissionDelete[0] || DbAtomPermissionKindEnum.Denied,
    }))
  );
};
