import { DbAtomEmployeeProjectStatusEnum } from "@/constants/DbAtomEmployeeProjectStatusEnum";

/**
 * 取得員工專案狀態文字標籤
 * @param value 員工專案狀態列舉
 * @returns 顯示文字（未定義 / 未指派 / 未開始 / 如期 / 注意 / 延遲 / 已完成）
 */
export const getEmployeeProjectStatusLabel = (
  value: DbAtomEmployeeProjectStatusEnum | null
): string => {
  switch (value) {
    case DbAtomEmployeeProjectStatusEnum.Undefined:
      return "未定義";
    case DbAtomEmployeeProjectStatusEnum.NotAssigned:
      return "未指派";
    case DbAtomEmployeeProjectStatusEnum.NotStarted:
      return "未開始";
    case DbAtomEmployeeProjectStatusEnum.OnSchedule:
      return "如期";
    case DbAtomEmployeeProjectStatusEnum.AtRisk:
      return "注意";
    case DbAtomEmployeeProjectStatusEnum.Delayed:
      return "延遲";
    case DbAtomEmployeeProjectStatusEnum.Completed:
      return "已完成";
    default:
      return "-";
  }
};
