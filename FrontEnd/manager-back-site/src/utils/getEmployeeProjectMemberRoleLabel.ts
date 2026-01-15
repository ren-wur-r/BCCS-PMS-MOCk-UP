import { DbEmployeeProjectMemberRoleEnum } from "@/constants/DbEmployeeProjectMemberRoleEnum";

/**
 * 取得專案成員角色文字標籤
 * @param value 專案成員角色列舉
 * @returns 顯示文字（未定義 / 總經理 / 業務 / 專案經理 / 部門主管 / 執行者 / 助理）
 */
export const getEmployeeProjectMemberRoleLabel = (
  value: DbEmployeeProjectMemberRoleEnum | null
): string => {
  switch (value) {
    case DbEmployeeProjectMemberRoleEnum.Undefined:
      return "未定義";
    case DbEmployeeProjectMemberRoleEnum.Boss:
      return "總經理";
    case DbEmployeeProjectMemberRoleEnum.Saler:
      return "業務";
    case DbEmployeeProjectMemberRoleEnum.ProjectManager:
      return "專案經理";
    case DbEmployeeProjectMemberRoleEnum.DepartmentLeader:
      return "部門主管";
    case DbEmployeeProjectMemberRoleEnum.Executor:
      return "執行者";
    case DbEmployeeProjectMemberRoleEnum.Assistant:
      return "助理";
    default:
      return "-";
  }
};
