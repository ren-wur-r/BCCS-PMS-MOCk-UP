import { DbAtomEmployeePipelineBillStatusEnum } from "@/constants/DbAtomEmployeePipelineBillStatusEnum";

/**
 * 取得商機發票紀錄狀態文字標籤
 * @param value 商機發票紀錄狀態列舉
 * @returns 顯示文字（未定義 / 未結案 / 處理中 / 已結案）
 */
export const getEmployeePipelineBillStatusLabel = (
  value: DbAtomEmployeePipelineBillStatusEnum | null
): string => {
  switch (value) {
    case DbAtomEmployeePipelineBillStatusEnum.Undefined:
      return "未定義";
    case DbAtomEmployeePipelineBillStatusEnum.NotCompleted:
      return "未結案";
    case DbAtomEmployeePipelineBillStatusEnum.InProgress:
      return "處理中";
    case DbAtomEmployeePipelineBillStatusEnum.Completed:
      return "已結案";
    default:
      return "-";
  }
};
