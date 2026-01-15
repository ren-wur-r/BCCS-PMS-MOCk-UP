import { DbAtomPermissionKindEnum } from "@/constants/DbAtomPermissionKindEnum";

/**
 * 取得權限文字標籤
 * @param value 權限種類列舉
 * @returns 顯示文字（無權限 / 自己 / 所有人）
 */
export const getPermissionKindLabel = (value: DbAtomPermissionKindEnum): string => {
  switch (value) {
    case DbAtomPermissionKindEnum.Denied:
      return "無權限";
    case DbAtomPermissionKindEnum.Self:
      return "自己";
    case DbAtomPermissionKindEnum.Everyone:
      return "所有人";
    default:
      return "-";
  }
};
