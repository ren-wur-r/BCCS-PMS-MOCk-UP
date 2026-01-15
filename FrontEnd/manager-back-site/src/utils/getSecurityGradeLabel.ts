import { DbAtomSecurityGradeEnum } from "@/constants/DbAtomSecurityGradeEnum";

/**
 * 取得資安責任等級文字標籤
 * @param value 資安責任等級列舉
 * @returns 顯示文字（未選擇 / 未定義 / A / B / C / D）
 */
export const getSecurityGradeLabel = (value: DbAtomSecurityGradeEnum): string => {
  switch (value) {
    case DbAtomSecurityGradeEnum.NotSelected:
      return "未選擇";
    case DbAtomSecurityGradeEnum.Undefined:
      return "未定義";
    case DbAtomSecurityGradeEnum.A:
      return "A";
    case DbAtomSecurityGradeEnum.B:
      return "B";
    case DbAtomSecurityGradeEnum.C:
      return "C";
    case DbAtomSecurityGradeEnum.D:
      return "D";
    default:
      return "-";
  }
};
