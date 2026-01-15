import { DbAtomCustomerGradeEnum } from "@/constants/DbAtomCustomerGradeEnum";

/**
 * 取得客戶分級文字標籤
 * @param value 客戶分級列舉
 * @returns 顯示文字（未選擇 / 未定義 / A / B / C）
 */
export const getCustomerGradeLabel = (value: DbAtomCustomerGradeEnum | null): string => {
  switch (value) {
    case DbAtomCustomerGradeEnum.NotSelected:
      return "未選擇";
    case DbAtomCustomerGradeEnum.Undefined:
      return "未定義";
    case DbAtomCustomerGradeEnum.A:
      return "A";
    case DbAtomCustomerGradeEnum.B:
      return "B";
    case DbAtomCustomerGradeEnum.C:
      return "C";
    default:
      return "-";
  }
};
