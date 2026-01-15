/** 資料庫-員工-專案成員-角色-列舉 */
export enum DbEmployeeProjectMemberRoleEnum {
  /** 未定義 */
  Undefined = 0,
  /** 總經理 */
  Boss = 1,
  /** 業務 */
  Saler = 2,
  /** 專案經理 */
  ProjectManager = 3,
  /** 部門主管 */
  DepartmentLeader = 4,
  /** 執行者 */
  Executor = 5,
  /** 助理 */
  Assistant = 6
}